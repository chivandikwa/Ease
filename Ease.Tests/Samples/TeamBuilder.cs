using Bogus;

namespace Ease.Tests.Samples;

internal class TeamBuilder : Builder<Team>
{
    private readonly Faker _faker = new();

    public TeamBuilder WithName(string name)
    {
        With(x => x.Name, name);

        return this;
    }

    public TeamBuilder WithUsers(params UserBuilder[] users)
    {
        WithMany(x => x.Users, users);

        return this;
    }

    public override TeamBuilder ThatIsValid()
    {
        With(x => x.Name, _faker.Company.CompanyName());
        With(x => x.Description, _faker.Company.CatchPhrase());
        With(x => x.CreatedAt, DateTimeOffset.Now);

        HavingMany(x => x.Users, A.User.ThatIsValid(), A.User.ThatIsValid(), A.User.ThatIsValid());

        return this;
    }
}