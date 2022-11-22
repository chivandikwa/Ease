namespace Ease.Tests.Samples;

internal record Team
{
    public string Name { get; set; }
    public string Description { get; set; }
    public DateTimeOffset CreatedAt { get; set; }
    public IEnumerable<User> Users { get; set; }
}