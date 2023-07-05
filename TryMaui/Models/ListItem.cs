using Reactive.Bindings;
using Reactive.Bindings.Extensions;
using TryMaui.Shared;

namespace TryMaui.Models;

public class ListItem : BindableBase
{
    public Guid Id { get; } = Guid.NewGuid();
    public ReactivePropertySlim<string> Name { get; }

    public ListItem()
    {
        Name = new ReactivePropertySlim<string>().AddTo(Disposable);
    }
}