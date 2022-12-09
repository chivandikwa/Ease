namespace Ease.Tests.Samples;

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