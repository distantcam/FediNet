using FluentAssertions;
using Xunit;

namespace FediNet.Tests;

public class AcctUriTests
{
    [Theory]
    [InlineData("acct:user@host", "user", "host")]
    [InlineData("acct:me@test", "me", "test")]
    public void GoodAcctTests(string acct, string user, string host)
    {
        AcctUri.TryParse(acct, out var uri).Should().BeTrue();

        uri.Should().NotBeNull();
        uri!.User.Should().Be(user);
        uri!.Host.Should().Be(host);
    }

    [Theory]
    [InlineData("user@host")]
    [InlineData("acc:user@host")]
    [InlineData("acct:user@")]
    [InlineData("acct:@host")]
    [InlineData("acct:@user@host")]
    public void BadAcctTests(string acct)
    {
        AcctUri.TryParse(acct, out var uri).Should().BeFalse();

        uri.Should().BeNull();
    }
}
