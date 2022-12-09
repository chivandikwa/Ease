# Ease.NET

![banner](https://raw.githubusercontent.com/chivandikwa/Ease/main/banner.png)

> Test builders done with ease and done right

Ease is a simple Framework for creating dynamic and fluent builders in .NET, biased towards the use in tests. Intentionally does not come with batteries, for example random data is not part of Ease but it is trivial to add such functionality.

![](https://img.shields.io/badge/.net-5.0-blue?style=for-the-badge&logo=Microsoft)

![](https://img.shields.io/badge/.net-6.0-blue?style=for-the-badge&logo=Microsoft)

![](https://img.shields.io/badge/.net-7.0-blue?style=for-the-badge&logo=Microsoft)

[![NuGet version (Ease.NET)](https://img.shields.io/nuget/v/Ease.NET.svg?style=flat-square)](https://www.nuget.org/packages/Ease.NET/)

### Convince me!

Imagine creating tests objects like this:


```csharp
const string teamName = "awesome";
var team = A.Team.WithName(teamName)
    .WithUsers(A.User.ThatIsValid());
```

```csharp
const string teamName = "awesome";
var team = A.Team.With(x => x.Name, teamName)
   .WithMany(x => x.Users, A.User.ThatIsValid());
```

```csharp
var team = A.Team.ThatIsValid()
    .IgnoreProperty(x =>x.Description);
```

While working with DTOs, models, entities, etc, particularly those that are used throughout your domain and boundaries, you will find that they are required in a multitude of tests. A natural approach is to call the constructor of each when required and hydrate them with the required setup. While this is straightforward there are a couple of challenges. To illustrate this, lets us introduce a simple domain model showing the relationship between a Team and Users

```csharp
internal record User
{
    public User(string fullName, string email, DateTimeOffset joinedAt)
    {
        FullName = fullName;
        Email = email;
        JoinedAt = joinedAt;
    }

    public string FullName { get; set; }
    public string Email { get; set; }
    public DateTimeOffset JoinedAt { get; set; }
}

internal record Team
{
    public string Name { get; set; }
    public string Description { get; set; }
    public DateTimeOffset CreatedAt { get; set; }
    public IEnumerable<User> Users { get; set; }
}
```

Now, what could go wrong with the simple approach of calling the constructor or object initializers directly in tests? Well, the following:

- As your solution expands and the domain entities are used in hundreds of tests and you may update the domain by adding/removing properties that affect the ctor. In this case, you need to update a multitude of tests manually.
- Over time you may change the meaning of things in the domain and how they are set up. To give a naive example, imagine changing the JoinedAt type from DateTime to DateTimeOffset. Any tests that were created already will also need to be updated with this change. This breaks a lot of principles, ideally, the state and validity of an object should be self-contained, and if the outside has to understand the inner workings to attain a given state that is a fail, and yes even for tests.
- In many circumstances, you may simply want a domain entity for the test without a need for it to be in a specific state, but rather just to be valid. In this circumstance, you will likely find the same code copy-pasted all over the code base creating yet another maintenance nuisance.

### So, how do I solve this?

Let's start with another very tempting pattern that I have seen frequently.

```csharp
internal class MediocreUserFactory
{
    internal static User CreateUser(
        string fullName = default,
        string email= default,
        DateTimeOffset joinedAt = default
    ) =>
        new User(fullName, email, joinedAt);
}
```

This does not solve all the problems or at least does so by introducing new ones. As our domain evolves then the method params here become chaotic, notice the optional parameters that are in place to cater to creating the objects without providing everything, this does not scale well at all as more properties are introduced. Things get even more chaotic when there are nested objects and these builder methods also cater to that by maybe falling into the temptation of accepting the raw parameter values.

The biggest problems with this pattern are that it does not communicate intention nor does it evolve well with domain changes. Even with such an abstraction, a lot is still left to the setup phase of the tests, which is not great as this in many test cases would be an auxiliary concern and not the focus of the test, at least something that should not be a distraction with the test. As you can have multiple scenarios also how does this work with this pattern? One way is to add more parameters to control this. Or maybe to create multiple of these methods for each scenario 🤦‍♂️. With this pattern, I often find this code will still be copy pasted to add flexibility, so naturally, I am not promoting this one.

Finally, the tests that use this pattern or other such alternatives all tend to be very long and messy to read. By looking at the code at a glance it is not possible to understand what the `arrange` stage is doing, worse still it makes it hard to distinguish clearly the `arrange` from the `act`.

### A better way

A more reasonable approach would be to make use of custom builers

```csharp
// option 1
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

// option 2
internal class UserBuilder2
{
    private readonly User _user = new();

    public UserBuilder2 WithFullName(string fullName)
    {
        _user.FullName = fullName;
        return this;
    }

    public UserBuilder2 WithEmail(string email)
    {
        _user.Email = email;
        return this;
    }

    public UserBuilder2 HavingJoinedAt(DateTimeOffset joinedAt)
    {
        _user.JoinedAt = joinedAt;
        return this;
    }

    public User Build() =>
        _user;
}
```

This is great and precisely the approach this library tries to cater for. However, if you observe closely you will notice that the `WithX` methods are just pass-throughs to some backing field or object. This becomes quite some boilerplate as this will likely be the most common scenario with builders. The abstraction introduced here takes those scenarios away by making use of expressions as you will see in the examples that follow. However, with that abstraction, you can still make use of custom methods for scenarios like `ThatIsValid`, the one thing this abstraction enforces, and anything custom in your scenario. For example, a user may have a state Suspended which is not just some boolean flag, but maybe an orchestration of various properties in a given state. To avoid calling multiple builder methods and having this all over your test code, you can have one source of truth for this, that is if the meaning changes, one place to update. Something like this:

```csharp
public UserBuilder ThatIsSuspended()
{
    // complex setup of object to reflect suspended
    // for example setting multiple properties
    return this;
}
```

### Get to it already, show me the way!

Let's get straight to it and look at a different pattern in code.

Starting with the one I most recommend, dynamic builders

```csharp
internal class TeamBuilder : Builder<Team>
{
    private readonly Faker _faker = new();

    public override TeamBuilder ThatIsValid()
    {
        With(x => x.Name, _faker.Company.CompanyName());
        With(x => x.Description, _faker.Company.CatchPhrase());
        With(x => x.CreatedAt, DateTimeOffset.Now);

        // notice ability to chain builders
        HavingMany(x => x.Users, A.User.ThatIsValid(), A.User.ThatIsValid(), A.User.ThatIsValid());

        return this;
    }
}

// create a team without caring about the details, it should just be valid
var team = A.Team.ThatIsValid();

// create a team without caring about the details, it should just be valid, but with the exception of some property/properties
var team = A.Team.ThatIsValid()
            .IgnoreProperty(x => x.Description);

// take control of the values
const string teamName = "awesome";
var team = A.Team.With(x => x.Name, teamName)
    .WithMany(x => x.Users, A.User.ThatIsValid());
```

Delegate control to the builder, by having specific builder methods. Not my favorite, but possible especially if you want to massage data as part of the builder or create abstractions for scenarios like say a team that is suspended. While a team that is suspended may mean an interaction of many properties, from usage it would simply be `A.Team.ThatIsSuspended()`.

```csharp
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
        // can also leverage ability to chain builders
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

const string teamName = "awesome";
var team = A.Team.WithName(teamName)
    .WithUsers(A.User.ThatIsValid());
```

Ok so why is this better? I'm glad you asked!

- **Communication of intent**. The methods we see here are very clear on what they are building and while we kept this simple with our examples, this scales very well. This makes it easy to create objects in tests, especially in scenarios when the object is not primary to the test but still required for the test setup.
- **Fluent**. Who does not love fluent code, this one makes this further easy to use and very natural to read. If there is anything you should strive for is easily readable tests. Recall that when we ditched explicit documentation in code, we made an oath to write self-documenting code, one of which is through tests, so they better be easy to read and understand.
- **Less code**. So if the auxiliary act of creating objects for our tests is not key to the tests why should that mess make the test hard to read? I would rather see `A.Team.ThatIsSuspended()` than see all the code that entails this.
- **Ease of refactoring**. This approach isolates the actual creation of something to one place and one place only much like a factory. So now as your domain evolves and you change the meaning of things, ctors change, etc among many changes, as far as your tests are concerned this change only needs to be done in one place.

All things considered, we can certainly say that this is both simple and powerful. Creating the builders is easy and the pattern fosters clean coding patterns. Using the builders is also very intuitive and the fluent pattern further makes this a pleasure to use. Ease!

If you were paying close attention you would have noticed the readability added by the use of the `A` or `Some` to give results like `A.User` and `Some.Team` that conform to natural language. While optional this can be the cherry on the top to make the calls natural to read. You can achieve this as follows

```csharp
internal static class A
{
    public static UserBuilder User => new();
    public static TeamBuilder Team => new();
}
```

> Notice the use of `=>` and not `=`. This is intentional and care must be taken to make sure you always do it this way. You want to ensure that each call to this property makes a new builder. This isolation for tests is essential, especially considering that the builders have a state.

This pattern is very simple. However given that *1.* this involves 'only tests' and *2.* the problem does not seem that complex or pressing, this tends to be highly neglected. The *consequences* are however dire and do not discriminate because these are only tests. The amount of time lost to go around the challenges of not handling this problem properly can be great.

## Dynamic Builders

In scenarios where overriding `ThatIsValid` is not required and you are basically creating dynamic objects each time, you can avoid creating a builder altogether and make use of a dynamic builder.

```csharp
User dynamic = Builder.Of<User>()
    .With(x => x.FullName, "Dynamic Name")
    .Build();
```

> ⚠️ Note that the default instance creation only works for types with a default parameterless constructor.


## Setup

.NET CLI

> dotnet add package Ease.NET

Package Reference

> `<PackageReference Include="Ease.NET" Version="1.0.5" />`

Paket CLI

> paket add Ease.NET

You can also choose to install this as a dependency from NuGet.

The code behind all this is less than 100 lines of code, so you could also choose to copy this into your code bases and maintain that over time, because this is focused on simplicity, it is very unlikely new features will be introduced to this repository.


## Recommended practices

✅ **DO** use test builders for repeatable data initialization. Key candidates are things EF entities, domain entities, models, etc.

✅ **DO** consider adding generic test constructs such as builders that can and should be reused in multiple test projects to some common test project to avoid duplication.

✅ **DO** consider the `A` or `Some` static container pattern as shown in the examples above to make it easy to work with pre-initialized builders.

```csharp
internal static class A
{
    public static UserBuilder User => new();
}
```

✅ **DO** favor the dynamic test builder pattern for simple scenarios and only create custom methods in your builders for the more complex scenarios to avoid boilerplate code.

```csharp
var team = A.Team.With(x => x.Name, teamName)
    .WithMany(x => x.Users, A.User.ThatIsValid());
```

✅ **DO** as a requirement, override the `CreateInstance()` method when the object being created does not have a parameterless ctor of when properties do not match what was set, for example for objects that simulate property bags.

```csharp
protected override User CreateInstance()
    => new User(Get(x => x.FullName), Get(x => x.Email), Get(x => x.JoinedAt));
```

🛑 **DO NOT** solve the test builder problem with some mediocre factory method. This is the main takeaway from this project.
