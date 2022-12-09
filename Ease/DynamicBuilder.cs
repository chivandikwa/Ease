namespace Ease;

public class DynamicBuilder<T> : Builder<T> where T : class, new()
{
    public override Builder<T> ThatIsValid() => this;
}