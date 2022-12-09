using Ease.Tests.Samples;
using FluentAssertions;

namespace Ease.Tests;

public class BuilderTests
{
    [Fact]
    public void ThatIsValid()
    {
        // User user = A.User.ThatIsValid();

        User user = A.User.With(x => x.FullName, "John Doe")
            .With(x => x.Email, "john@acme.crop");
    }

    [Fact]
    public void ThatIsValid_IgnoreProperty()
    {
        User dynamic = Builder.Of<User>()
            .With(x => x.FullName, "Dynamic Name")
            .Build();
    }

    [Fact]
    public void ControlOverValues_CustomBuilderMethods()
    {
        
    }

    [Fact]
    public void ControlOverValues_DynamicBuilderMethods()
    {
        
    }
}