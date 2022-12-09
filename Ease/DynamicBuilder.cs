namespace Ease;

public class DynamicBuilder<T> : Builder<T> where T : class
{
    public override Builder<T> ThatIsValid() => this;
}