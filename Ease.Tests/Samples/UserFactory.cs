namespace Ease.Tests.Samples;

internal class UserFactory
{
    internal static User CreateUser(
        string fullName = default,
        string email = default,
        DateTimeOffset joinedAt = default) =>
        new User(fullName, email, joinedAt);
}