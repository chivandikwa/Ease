using System.Collections.Specialized;
using System.Collections;
using System.Linq.Expressions;

namespace Ease;

public abstract class Builder<T> : IBuilder<T> where T : class
{
    private readonly ListDictionary _properties = new();

    public static implicit operator T(Builder<T> builder) => builder.Build();

    public Builder<T> With<TProp>(Expression<Func<T, TProp>> action, TProp value)
    {
        _properties[((MemberExpression)action.Body).Member.Name] = value;
        return this;
    }

    public Builder<T> WithMany<TProp, TBuilder>(Expression<Func<T, IEnumerable<TProp>>> action,
        params TBuilder[] values)
        where TBuilder : IBuilder<TProp>
    {
        _properties[((MemberExpression)action.Body).Member.Name] = values.Select(x => x.Build()).ToList();
        return this;
    }

    public Builder<T> Having<TProp>(Expression<Func<T, TProp>> action, TProp value)
    {
        _properties[((MemberExpression)action.Body).Member.Name] = value;
        return this;
    }

    public Builder<T> Having<TProp, TBuilder>(Expression<Func<T, TProp>> action, TProp value)
        where TProp : IBuilder<TBuilder>
    {
        _properties[((MemberExpression)action.Body).Member.Name] = value.Build();
        return this;
    }

    public Builder<T> IgnoreProperty<TProp>(Expression<Func<T, TProp>> action)
    {
        _properties.Remove(((MemberExpression)action.Body).Member.Name);
        return this;
    }

    public Builder<T> HavingMany<TProp, TBuilder>(Expression<Func<T, IEnumerable<TProp>>> action,
        params TBuilder[] values)
        where TBuilder : IBuilder<TProp>
    {
        _properties[((MemberExpression)action.Body).Member.Name] = values.Select(x => x.Build()).ToList();
        return this;
    }

    public Builder<T> For<TProp>(Expression<Func<T, TProp>> action, TProp value)
    {
        _properties[((MemberExpression)action.Body).Member.Name] = value;
        return this;
    }

    private void Set<TProp>(Expression<Func<T, TProp>> action, TProp value) =>
        _properties[((MemberExpression)action.Body).Member.Name] = value;

    private void Set(string key, object value) =>
        _properties[key] = value;

    protected TProp Get<TProp>(Expression<Func<T, TProp>> action) =>
        (TProp)_properties[((MemberExpression)action.Body).Member.Name];

    protected TProp Get<TProp>(string key) =>
        (TProp)_properties[key];

    protected virtual T CreateInstance()
    {
        var typeConstructor = typeof(T).GetConstructor(Type.EmptyTypes);

        if (typeConstructor == null)
            throw new InvalidOperationException(
                "Unable to invoke default constructor. Override CreateInstance() in builder class.");

        var instance = typeConstructor.Invoke(Array.Empty<object>()) as T;

        foreach (DictionaryEntry property in _properties)
        {
            var (key, value) = property;

            typeof(T).GetProperty((string)key)?.SetValue(instance, value);
        }

        return instance;
    }

    public abstract Builder<T> ThatIsValid();

    public T Build() => CreateInstance();
}