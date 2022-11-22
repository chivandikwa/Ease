namespace Ease;

public interface IBuilder
{
}

public interface IBuilder<out T> : IBuilder
{
    T Build();
}