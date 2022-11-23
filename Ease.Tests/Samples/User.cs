namespace Ease.Tests.Samples;

internal record User
{
    public User(string fullName, string email, DateTimeOffset joinedAt)
    {
        FullName = fullName;
        Email = email;
        JoinedAt = joinedAt;
    }

    public User()
    {
        
    }

    public string FullName { get; set; }
    public string Email { get; set; }
    public DateTimeOffset JoinedAt { get; set; }
}