namespace FediNet;

public record AcctUri
{
    public AcctUri(string acctString)
    {
        var uri = new Uri(acctString);
        if (!uri.Scheme.Equals("acct", StringComparison.InvariantCultureIgnoreCase))
        {
            throw new Exception("Must be acct scheme.");
        }
        Scheme = uri.Scheme;
        var split = uri.AbsolutePath.Split('@');
        if (split.Length != 2)
        {
            throw new Exception("Invalid acct resource.");
        }
        User = split[0];
        Host = split[0];
    }

    public string Scheme { get; }
    public string User { get; }
    public string Host { get; }

    public override string ToString() => Scheme + ":" + User + "@" + Host;
}
