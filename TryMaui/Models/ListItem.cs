namespace TryMaui.Models;

public record ListItem(string Name)
{
    public Guid Id { get; } = Guid.NewGuid();
}