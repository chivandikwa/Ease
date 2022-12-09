using System.Collections.Specialized;
using System.Collections;
using System.Linq.Expressions;

namespace Ease;

public static class Builder
{
    public static DynamicBuilder<T> Of<T>() where T : class, new() => new();
}

public abstract class Builder<T> : IBuilder<T> where T : class
{
    private readonly ListDictionary _properties = new();

    /// <summary>
    /// Set the value of a given property by expression
    /// </summary>
    /// <typeparam name="TProp">The value type</typeparam>
    /// <param name="expression">The property selection expression</param>
    /// <param name="value">The value</param>
    /// <returns>The current builder for a fluent interface</returns>
    public Builder<T> With<TProp>(Expression<Func<T, TProp>> expression, TProp value)
    {
        _properties[((MemberExpression)expression.Body).Member.Name] = value;
        return this;
    }

    /// <summary>
    /// Set the value of a given property by expression
    /// </summary>
    /// <typeparam name="TProp">The value type</typeparam>
    /// <typeparam name="TBuilder">The builder type</typeparam>
    /// <param name="expression">The property selection expression</param>
    /// <param name="value">The value</param>
    /// <returns>The current builder for a fluent interface</returns>
    public Builder<T> With<TProp, TBuilder>(Expression<Func<T, TProp>> expression, TProp value)
        where TProp : IBuilder<TBuilder>
    {
        _properties[((MemberExpression)expression.Body).Member.Name] = value.Build();
        return this;
    }

    /// <summary>
    /// Set the value by an explicit string key
    /// </summary>
    /// <typeparam name="TProp">The value type</typeparam>
    /// <param name="key">The string key</param>
    /// <param name="value">The value</param>
    /// <returns>The current builder for a fluent interface</returns>
    public Builder<T> With<TProp>(string key, TProp value)
    {
        _properties[key] = value;
        return this;
    }

    /// <summary>
    /// Set the value by an explicit string key
    /// </summary>
    /// <typeparam name="TProp">The value type</typeparam>
    /// <typeparam name="TBuilder">The builder type</typeparam>
    /// <param name="key">The string key</param>
    /// <param name="value">The value</param>
    /// <returns>The current builder for a fluent interface</returns>
    public Builder<T> With<TProp, TBuilder>(string key, TProp value)
        where TProp : IBuilder<TBuilder>
    {
        _properties[key] = value.Build();
        return this;
    }

    /// <summary>
    /// Set the value of a given collection property by expression
    /// </summary>
    /// <typeparam name="TProp">The value type</typeparam>
    /// <typeparam name="TBuilder">The builder type</typeparam>
    /// <param name="expression">The property selection expression</param>
    /// <param name="values">The values</param>
    /// <returns>The current builder for a fluent interface</returns>
    public Builder<T> WithMany<TProp, TBuilder>(Expression<Func<T, IEnumerable<TProp>>> expression,
        params TBuilder[] values)
        where TBuilder : IBuilder<TProp>
    {
        _properties[((MemberExpression)expression.Body).Member.Name] = values.Select(x => x.Build()).ToList();
        return this;
    }

    /// <summary>
    /// Set the value of a given collection property by expression
    /// </summary>
    /// <typeparam name="TProp">The value type</typeparam>
    /// <typeparam name="TBuilder">The builder type</typeparam>
    /// <param name="key">The string key</param>
    /// <param name="values">The values</param>
    /// <returns>The current builder for a fluent interface</returns>
    public Builder<T> WithMany<TProp, TBuilder>(string key,
        params TBuilder[] values)
        where TBuilder : IBuilder<TProp>
    {
        _properties[key] = values.Select(x => x.Build()).ToList();
        return this;
    }

    /// <summary>
    /// Set the value of a given property by expression
    /// </summary>
    /// <typeparam name="TProp">The value type</typeparam>
    /// <param name="expression">The property selection expression</param>
    /// <param name="value">The value</param>
    /// <returns>The current builder for a fluent interface</returns>
    public Builder<T> Having<TProp>(Expression<Func<T, TProp>> expression, TProp value)
    {
        _properties[((MemberExpression)expression.Body).Member.Name] = value;
        return this;
    }

    /// <summary>
    /// Set the value of a given property by expression
    /// </summary>
    /// <typeparam name="TProp">The value type</typeparam>
    /// <typeparam name="TBuilder">The builder type</typeparam>
    /// <param name="expression">The property selection expression</param>
    /// <param name="value">The value</param>
    /// <returns>The current builder for a fluent interface</returns>
    public Builder<T> Having<TProp, TBuilder>(Expression<Func<T, TProp>> expression, TProp value)
        where TProp : IBuilder<TBuilder>
    {
        _properties[((MemberExpression)expression.Body).Member.Name] = value.Build();
        return this;
    }

    /// <summary>
    /// Set the value by an explicit string key
    /// </summary>
    /// <typeparam name="TProp">The value type</typeparam>
    /// <param name="key">The string key</param>
    /// <param name="value">The value</param>
    /// <returns>The current builder for a fluent interface</returns>
    public Builder<T> Having<TProp>(string key, TProp value)
    {
        _properties[key] = value;
        return this;
    }

    /// <summary>
    /// Set the value by an explicit string key
    /// </summary>
    /// <typeparam name="TProp">The value type</typeparam>
    /// <typeparam name="TBuilder">The builder type</typeparam>
    /// <param name="key">The string key</param>
    /// <param name="value">The value</param>
    /// <returns>The current builder for a fluent interface</returns>
    public Builder<T> Having<TProp, TBuilder>(string key, TProp value)
        where TProp : IBuilder<TBuilder>
    {
        _properties[key] = value.Build();
        return this;
    }

    /// <summary>
    /// Set the value of a given collection property by expression
    /// </summary>
    /// <typeparam name="TProp">The value type</typeparam>
    /// <typeparam name="TBuilder">The builder type</typeparam>
    /// <param name="expression">The property selection expression</param>
    /// <param name="values">The values</param>
    /// <returns>The current builder for a fluent interface</returns>
    public Builder<T> HavingMany<TProp, TBuilder>(Expression<Func<T, IEnumerable<TProp>>> expression,
        params TBuilder[] values)
        where TBuilder : IBuilder<TProp>
    {
        _properties[((MemberExpression)expression.Body).Member.Name] = values.Select(x => x.Build()).ToList();
        return this;
    }

    /// <summary>
    /// Set the value of a given collection property by expression
    /// </summary>
    /// <typeparam name="TProp">The value type</typeparam>
    /// <typeparam name="TBuilder">The builder type</typeparam>
    /// <param name="key">The string key</param>
    /// <param name="values">The values</param>
    /// <returns>The current builder for a fluent interface</returns>
    public Builder<T> HavingMany<TProp, TBuilder>(string key,
        params TBuilder[] values)
        where TBuilder : IBuilder<TProp>
    {
        _properties[key] = values.Select(x => x.Build()).ToList();
        return this;
    }

    /// <summary>
    /// Set the value of a given property by expression
    /// </summary>
    /// <typeparam name="TProp">The value type</typeparam>
    /// <param name="expression">The property selection expression</param>
    /// <param name="value">The value</param>
    /// <returns>The current builder for a fluent interface</returns>
    public Builder<T> For<TProp>(Expression<Func<T, TProp>> expression, TProp value)
    {
        _properties[((MemberExpression)expression.Body).Member.Name] = value;
        return this;
    }

    /// <summary>
    /// Set the value of a given property by expression
    /// </summary>
    /// <typeparam name="TProp">The value type</typeparam>
    /// <typeparam name="TBuilder">The builder type</typeparam>
    /// <param name="expression">The property selection expression</param>
    /// <param name="value">The value</param>
    /// <returns>The current builder for a fluent interface</returns>
    public Builder<T> For<TProp, TBuilder>(Expression<Func<T, TProp>> expression, TProp value)
        where TProp : IBuilder<TBuilder>
    {
        _properties[((MemberExpression)expression.Body).Member.Name] = value.Build();
        return this;
    }

    /// <summary>
    /// Set the value by an explicit string key
    /// </summary>
    /// <typeparam name="TProp">The value type</typeparam>
    /// <param name="key">The string key</param>
    /// <param name="value">The value</param>
    /// <returns>The current builder for a fluent interface</returns>
    public Builder<T> For<TProp>(string key, TProp value)
    {
        _properties[key] = value;
        return this;
    }

    /// <summary>
    /// Set the value by an explicit string key
    /// </summary>
    /// <typeparam name="TProp">The value type</typeparam>
    /// <typeparam name="TBuilder">The builder type</typeparam>
    /// <param name="key">The string key</param>
    /// <param name="value">The value</param>
    /// <returns>The current builder for a fluent interface</returns>
    public Builder<T> For<TProp, TBuilder>(string key, TProp value)
        where TProp : IBuilder<TBuilder>
    {
        _properties[key] = value.Build();
        return this;
    }

    /// <summary>
    /// Set the value of a given collection property by expression
    /// </summary>
    /// <typeparam name="TProp">The value type</typeparam>
    /// <typeparam name="TBuilder">The builder type</typeparam>
    /// <param name="expression">The property selection expression</param>
    /// <param name="values">The values</param>
    /// <returns>The current builder for a fluent interface</returns>
    public Builder<T> ForMany<TProp, TBuilder>(Expression<Func<T, IEnumerable<TProp>>> expression,
        params TBuilder[] values)
        where TBuilder : IBuilder<TProp>
    {
        _properties[((MemberExpression)expression.Body).Member.Name] = values.Select(x => x.Build()).ToList();
        return this;
    }

    /// <summary>
    /// Set the value of a given collection property by expression
    /// </summary>
    /// <typeparam name="TProp">The value type</typeparam>
    /// <typeparam name="TBuilder">The builder type</typeparam>
    /// <param name="key">The string key</param>
    /// <param name="values">The values</param>
    /// <returns>The current builder for a fluent interface</returns>
    public Builder<T> ForMany<TProp, TBuilder>(string key,
        params TBuilder[] values)
        where TBuilder : IBuilder<TProp>
    {
        _properties[key] = values.Select(x => x.Build()).ToList();
        return this;
    }

    /// <summary>
    /// Ignore the specified property by expression selection on building object
    /// </summary>
    /// <typeparam name="TProp">The property type</typeparam>
    /// <param name="expression">The property selection expression</param>
    /// <returns>The current builder for a fluent interface</returns>
    public Builder<T> IgnoreProperty<TProp>(Expression<Func<T, TProp>> expression)
    {
        _properties.Remove(((MemberExpression)expression.Body).Member.Name);
        return this;
    }

    /// <summary>
    /// Ignore the specified property by explicit string key on building object
    /// </summary>
    /// <param name="key">The string key</param>
    /// <returns>The current builder for a fluent interface</returns>
    public Builder<T> IgnoreProperty<TProp>(string key)
    {
        _properties.Remove(key);
        return this;
    }

    private void Set<TProp>(Expression<Func<T, TProp>> expression, TProp value) =>
        _properties[((MemberExpression)expression.Body).Member.Name] = value;

    private void Set(string key, object value) =>
        _properties[key] = value;

    /// <summary>
    /// Get the property value by expression selection
    /// </summary>
    /// <typeparam name="TProp">The property type</typeparam>
    /// <param name="expression">The property selection expression</param>
    /// <returns>The property value</returns>
    protected TProp Get<TProp>(Expression<Func<T, TProp>> expression)
    {
        var value = _properties[((MemberExpression)expression.Body).Member.Name];
        return value is not null ? (TProp)value : default;
    }

    /// <summary>
    /// Get the value by explicit string key
    /// </summary>
    /// <typeparam name="TProp">The property type</typeparam>
    /// <param name="key">The string key</param>
    /// <returns>The value</returns>
    protected TProp Get<TProp>(string key)
    {
        var value = _properties[key];
        return value is not null ? (TProp)value : default;
    }

    /// <summary>
    /// Create a new object instance for the builder Build
    /// Override if type does not have parameter-less ctor or for more control
    /// </summary>
    /// <returns>The object instance</returns>
    /// <exception cref="InvalidOperationException"></exception>
    protected virtual T CreateInstance()
    {
        var typeConstructor = typeof(T).GetConstructor(Type.EmptyTypes);

        if (typeConstructor == null)
            throw new InvalidOperationException(
                "Unable to invoke default constructor. Override CreateInstance() in builder class.");

        var instance = typeConstructor.Invoke(Array.Empty<object>()) as T;

        foreach (DictionaryEntry property in _properties)
        {
            typeof(T).GetProperty((string)property.Key)?.SetValue(instance, property.Value);
        }

        return instance;
    }

    /// <summary>
    /// Implicit cast conversion to create the object instance from the builder
    /// </summary>
    /// <param name="builder">The builder</param>
    public static implicit operator T(Builder<T> builder) => builder.Build();

    /// <summary>
    /// Override to implement the builder state required for a valid object
    /// </summary>
    /// <returns>The current builder for a fluent interface</returns>
    public abstract Builder<T> ThatIsValid();

    /// <summary>
    /// Create object instance from builder
    /// </summary>
    /// <returns>The object instance based on builder setup</returns>
    public T Build() => CreateInstance();
}