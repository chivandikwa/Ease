using Ease.Tests.Samples;
using FluentAssertions;

namespace Ease.Tests;

public class BuilderTests
{
    [Fact]
    public void Test1()
    {
        Team team = A.Team.ThatIsValid();

        team.Name.Should().NotBeNullOrWhiteSpace();
        team.Description.Should().NotBeNullOrWhiteSpace();
        team.CreatedAt.Should().BeOnOrBefore(DateTimeOffset.Now);

        team.Users.Should().AllSatisfy(user =>
        {
            user.FullName.Should().NotBeNullOrWhiteSpace();
            user.Email.Should().NotBeNullOrWhiteSpace();
            user.JoinedAt.Should().BeOnOrBefore(DateTimeOffset.Now);
        });
    }


    [Fact]
    public void Test2()
    {
        Team team = A.Team.ThatIsValid()
            .IgnoreProperty(x => x.Description);

        team.Name.Should().NotBeNullOrWhiteSpace();
        team.Description.Should().BeNull();
        team.CreatedAt.Should().BeOnOrBefore(DateTimeOffset.Now);

        team.Users.Should().AllSatisfy(user =>
        {
            user.FullName.Should().NotBeNullOrWhiteSpace();
            user.Email.Should().NotBeNullOrWhiteSpace();
            user.JoinedAt.Should().BeOnOrBefore(DateTimeOffset.Now);
        });
    }

    [Fact]
    public void Sample()
    {
        const string teamName = "awesome";
        Team team = A.Team.WithName(teamName)
            .WithUsers(A.User.ThatIsValid());

        team.Name.Should().Be(teamName);

        team.Users.Should().ContainSingle();

        var user = team.Users.Single();
        user.FullName.Should().NotBeNullOrWhiteSpace();
        user.Email.Should().NotBeNullOrWhiteSpace();
        user.JoinedAt.Should().BeOnOrBefore(DateTimeOffset.Now);
    }

    [Fact]
    public void Sample2()
    {
        const string teamName = "awesome";
        Team team = A.Team.With(x => x.Name, teamName)
            .WithMany(x => x.Users, A.User.ThatIsValid());

        team.Name.Should().Be(teamName);

        team.Users.Should().ContainSingle();

        var user = team.Users.Single();
        user.FullName.Should().NotBeNullOrWhiteSpace();
        user.Email.Should().NotBeNullOrWhiteSpace();
        user.JoinedAt.Should().BeOnOrBefore(DateTimeOffset.Now);
    }
}