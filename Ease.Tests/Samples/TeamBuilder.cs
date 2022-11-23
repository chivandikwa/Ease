using Bogus;

namespace Ease.Tests.Samples;

internal class TeamBuilder : Builder<Team>
{
    public override Builder<Team> ThatIsValid()
    {
        throw new NotImplementedException();
    }
}