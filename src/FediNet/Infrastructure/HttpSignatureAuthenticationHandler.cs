using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Text.Encodings.Web;
using System.Text.RegularExpressions;
using FediNet.Services;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Options;

namespace FediNet.Infrastructure;

public class HttpSignatureAuthenticationOptions : AuthenticationSchemeOptions
{
}

public partial class HttpSignatureAuthenticationHandler : AuthenticationHandler<HttpSignatureAuthenticationOptions>
{
    private readonly ActorHelper _actorHelper;

    public HttpSignatureAuthenticationHandler(
        IOptionsMonitor<HttpSignatureAuthenticationOptions> options,
        ILoggerFactory logger,
        UrlEncoder encoder,
        ISystemClock clock,
        ActorHelper actorHelper)
        : base(options, logger, encoder, clock)
    {
        _actorHelper = actorHelper;
    }

    protected override async Task<AuthenticateResult> HandleAuthenticateAsync()
    {
        var requestHeaders = Request.Headers
            .ToDictionary(h => h.Key.ToLowerInvariant(), h => (string)h.Value!);

        using var reader = new StreamReader(Request.Body);
        var body = await reader.ReadToEndAsync();

        if (!requestHeaders.TryGetValue("digest", out var digest) ||
            !CheckDigest(digest, body))
            return AuthenticateResult.Fail("Bad digest");

        if (!requestHeaders.TryGetValue("signature", out var signature) ||
            !await CheckSignature(signature, body, Request.Method, Request.Path, Request.QueryString.ToString(), requestHeaders))
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

    private async Task<bool> CheckSignature(string signature, string body, string method, string path, string queryString, Dictionary<string, string> requestHeaders)
    {
        var sigRegex = new Regex(@"^([a-zA-Z0-9]+)=""(.+)""$");
        var signatureHeaders = signature.Split(',')
            .Select(x => sigRegex.Match(x))
            .ToDictionary(m => m.Groups[1].Value, m => m.Groups[2].Value);

        var keyId = signatureHeaders["keyId"];
        var headers = signatureHeaders["headers"];
        var algorithm = signatureHeaders["algorithm"];
        var sig = Convert.FromBase64String(signatureHeaders["signature"]);

        var actor = await _actorHelper.GetActor(keyId);
        if (actor.PublicKey == null)
            return false;

        var toDecode = actor.PublicKey.PublicKeyPem.Trim().Remove(0, actor.PublicKey.PublicKeyPem.IndexOf('\n'));
        toDecode = toDecode.Remove(toDecode.LastIndexOf('\n')).Replace("\n", "");
        var signKey = ASN1.ToRSA(Convert.FromBase64String(toDecode));

        var toSign = string.Join('\n', headers.Split(' ')
            .Select(headerKey =>
            {
                if (headerKey == "(request-target)")
                    return $"(request-target): {method.ToLower()} {path}{queryString}";
                return $"{headerKey}: {string.Join(", ", requestHeaders[headerKey])}";
            }));

        var key = new RSACryptoServiceProvider();
        var rsaKeyInfo = key.ExportParameters(false);
        rsaKeyInfo.Modulus = Convert.FromBase64String(toDecode);
        key.ImportParameters(rsaKeyInfo);

        var result = signKey.VerifyData(Encoding.UTF8.GetBytes(toSign), sig, HashAlgorithmName.SHA256, RSASignaturePadding.Pkcs1);

        return result;
    }
}
