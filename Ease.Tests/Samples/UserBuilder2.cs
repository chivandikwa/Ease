namespace Ease.Tests.Samples;

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