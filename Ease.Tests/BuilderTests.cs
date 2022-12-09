using Ease.Tests.Samples;
using FluentAssertions;

namespace Ease.Tests;

public class BuilderTests
{
    [Fact]
    public void ThatIsValid()
    {
        User user = A.User.ThatIsValid();

        user.FullName.Should().NotBeNullOrWhiteSpace();
        user.Email.Should().NotBeNullOrWhiteSpace();
        user.JoinedAt.Should().BeWithin(TimeSpan.FromMinutes(1));
    }

    [Fact]
    public void ThatIsValid_ExplicitBuild()
    {
        var user = A.User.ThatIsValid().Build();

        user.FullName.Should().NotBeNullOrWhiteSpace();
        user.Email.Should().NotBeNullOrWhiteSpace();
        user.JoinedAt.Should().BeWithin(TimeSpan.FromMinutes(1));
    }

    [Fact]
    public void ThatIsValid_IgnoreProperty()
    {
        User user = A.User.ThatIsValid()
            .IgnoreProperty(x => x.Email);

        user.FullName.Should().NotBeNullOrWhiteSpace();
        user.JoinedAt.Should().BeWithin(TimeSpan.FromMinutes(1));

        user.Email.Should().BeNull();
    }

    [Fact]
    public void ControlOverValues_CustomBuilderMethods()
    {
        const string name = nameof(name);

        User user = A.User.WithFullName(name);

        user.FullName.Should().Be(name);

        user.Email.Should().BeNull();
        user.JoinedAt.Should().Be(default);
    }

    [Fact]
    public void ControlOverValues_DynamicBuilderMethods()
    {
        const string name = nameof(name);

        User user = A.User.With(x => x.FullName, name);

        user.FullName.Should().Be(name);

        user.Email.Should().BeNull();
        user.JoinedAt.Should().Be(default);
    }

    [Fact]
    public void DynamicBuilderSample()
    {
        const string fullName = "Dynamic Name";

        User dynamic = Builder.Of<User>()
            .With(x => x.FullName, "Dynamic Name")
            .Build();

        dynamic.FullName.Should().Be(fullName);
    }

}