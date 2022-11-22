using Bogus;

namespace Ease.Tests.Samples;

internal class UserBuilder : Builder<User>
{
    private readonly Faker _faker = new();

    public override UserBuilder ThatIsValid()
    {
        With(x => x.FullName, _faker.Person.FullName);
        With(x => x.Email, _faker.Person.Email);
        With(x => x.JoinedAt, DateTimeOffset.Now);

        return this;
    }

    protected override User CreateInstance()
        => new User(Get(x => x.FullName), Get(x => x.Email), Get(x => x.JoinedAt));
}

internal class MediocreUserBuilder
{
    internal static User CreateUser(
        string fullName = default,
        string email = default,
        DateTimeOffset joinedAt = default
    ) =>
        new User(fullName, email, joinedAt);
}