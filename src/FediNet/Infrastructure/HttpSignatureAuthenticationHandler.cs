using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Text.Encodings.Web;
using System.Text.RegularExpressions;
using FediNet.Caching;
using FediNet.Services;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Options;

namespace FediNet.Infrastructure;

public class HttpSignatureAuthenticationOptions : AuthenticationSchemeOptions
{
}

public partial class HttpSignatureAuthenticationHandler : AuthenticationHandler<HttpSignatureAuthenticationOptions>
{
    private readonly PublicKeyCache _publicKeyCache;
    private readonly ActorHelper _actorHelper;

    public HttpSignatureAuthenticationHandler(
        IOptionsMonitor<HttpSignatureAuthenticationOptions> options,
        ILoggerFactory logger,
        UrlEncoder encoder,
        ISystemClock clock,
        PublicKeyCache publicKeyCache,
        ActorHelper actorHelper)
        : base(options, logger, encoder, clock)
    {
        _publicKeyCache = publicKeyCache;
        _actorHelper = actorHelper;
    }

    protected override async Task<AuthenticateResult> HandleAuthenticateAsync()
    {
        var requestHeaders = Request.Headers
            .ToDictionary(h => h.Key.ToLowerInvariant(), h => (string)h.Value!);

        using var reader = new StreamReader(Request.Body);
        var body = await reader.ReadToEndAsync();

        if (requestHeaders.TryGetValue("digest", out var digest) &&
            !CheckDigest(digest, body))
            return AuthenticateResult.Fail("Bad digest");

        if (requestHeaders.TryGetValue("signature", out var signature) &&
            !await CheckSignature(signature, Request.Method, Request.Path, Request.QueryString.ToString(), requestHeaders))
            return AuthenticateResult.Fail("Bad signature");

        var claims = new[] {
            new Claim("signed", "true")
        };
        var claimsIdentity = new ClaimsIdentity(claims, nameof(HttpSignatureAuthenticationHandler));
        var ticket = new AuthenticationTicket(new ClaimsPrincipal(claimsIdentity), this.Scheme.Name);
        return AuthenticateResult.Success(ticket);
    }

    private static bool CheckDigest(string digest, string body)
    {
        var digestHash = digest.Split(new[] { "SHA-256=" }, StringSplitOptions.RemoveEmptyEntries).LastOrDefault();
        using var sha256Hash = SHA256.Create();
        var bytes = sha256Hash.ComputeHash(Encoding.UTF8.GetBytes(body));
        var calculatedDigestHash = Convert.ToBase64String(bytes);
        return digestHash == calculatedDigestHash;
    }

    private async Task<bool> CheckSignature(string signature, string method, string path, string queryString, Dictionary<string, string> requestHeaders)
    {
        var sigRegex = SignatureRegex();
        var signatureHeaders = signature.Split(',')
            .Select(x => sigRegex.Match(x))
            .ToDictionary(m => m.Groups[1].Value, m => m.Groups[2].Value);

        var keyId = signatureHeaders["keyId"];
        var headers = signatureHeaders["headers"];
        var algorithm = signatureHeaders["algorithm"];
        var sig = Convert.FromBase64String(signatureHeaders["signature"]);

        var toSign = Encoding.UTF8.GetBytes(string.Join('\n', headers.Split(' ')
            .Select(headerKey =>
            {
                if (headerKey == "(request-target)")
                    return $"(request-target): {method.ToLower()} {path}{queryString}";
                return $"{headerKey}: {requestHeaders[headerKey]}";
            })));

        var result = false;
        var publicKeyPem = await _publicKeyCache.GetOrCreateAsync(keyId, e => GetPublicKeyPemFromActor((string)e.Key));
        result = VerifySignature(publicKeyPem!, algorithm, toSign, sig);
        if (!result)
        {
            // Maybe the cached version is bad. Try getting the key again.
            _publicKeyCache.Remove(keyId);
            publicKeyPem = await GetPublicKeyPemFromActor(keyId);
            _publicKeyCache.Set(keyId, publicKeyPem);
            result = VerifySignature(publicKeyPem, algorithm, toSign, sig);
        }

        return result;
    }

    private async Task<string> GetPublicKeyPemFromActor(string actorId)
    {
        throw new NotImplementedException();
        //return (await _actorHelper.GetActor(actorId)).PublicKey!.PublicKeyPem;
    }

    private bool VerifySignature(string publicKeyPem, string algorithm, byte[] toSign, byte[] sig)
    {
        var rsa = RSA.Create();
        rsa.ImportFromPem(publicKeyPem);
        if (algorithm.Equals("rsa-sha256", StringComparison.InvariantCultureIgnoreCase))
        {
            return rsa.VerifyData(toSign, sig, HashAlgorithmName.SHA256, RSASignaturePadding.Pkcs1);
        }
        throw new Exception("Unknown signature algorithm: " + algorithm);
    }

    [GeneratedRegex("^([a-zA-Z0-9]+)=\"(.+)\"$")]
    private static partial Regex SignatureRegex();
}
