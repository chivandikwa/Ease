using Bogus;

namespace Ease.Tests.Samples;

internal class UserBuilder : Builder<User>
{
    private readonly Faker _faker = new();
    public override Builder<User> ThatIsValid()
    {
        With(x => x.FullName, _faker.Person.FullName);
        With(x => x.Email, _faker.Person.Email);
        With(x => x.JoinedAt, DateTimeOffset.Now);

        return this;
    }

    public UserBuilder WithFullName(string fullName)
    {
        With(x => x.FullName, fullName);

        return this;
    }


    public UserBuilder ThatIsSuspended()
    {
        // complex setup of object to reflect suspended
        // for example setting multiple properties
        return this;
    }
}