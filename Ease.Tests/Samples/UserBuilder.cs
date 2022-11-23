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

    public UserBuilder ThatIsSuspended()
    {
        // complex setup of object to reflect suspended
        // for example setting multiple properties
        return this;
    }
}

internal class UserFactory
{
    internal static User CreateUser(
        string fullName = default,
        string email = default,
        DateTimeOffset joinedAt = default) =>
        new User(fullName, email, joinedAt);
}

internal class UserBuilder1
{
    private string _fullName;
    private string _email;
    private DateTimeOffset _joinedAt;
    
    public UserBuilder1 WithFullName(string fullName)
    {
        _fullName = fullName;
        return this;
    }
    
    public UserBuilder1 WithEmail(string email)
    {
        _email = email;
        return this;
    }
    
    public UserBuilder1 HavingJoinedAt(DateTimeOffset joinedAt)
    {
        _joinedAt = joinedAt;
        return this;
    }

    public User Build() =>
        new User
        {
            FullName = _fullName,
            Email = _email,
            JoinedAt = _joinedAt
        };
}

internal class UserBuilder2
{
    private readonly User _user = new();
    
    public UserBuilder2 WithFullName(string fullName)
    {
        _user.FullName = fullName;
        return this;
    }
    
    public UserBuilder2 WithEmail(string email)
    {
        _user.Email = email;
        return this;
    }
    
    public UserBuilder2 HavingJoinedAt(DateTimeOffset joinedAt)
    {
        _user.JoinedAt = joinedAt;
        return this;
    }

    public User Build() =>
        _user;
}