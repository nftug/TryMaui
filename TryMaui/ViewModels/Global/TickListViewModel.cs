using Reactive.Bindings;
using System.Windows.Input;
using TryMaui.Models;
using System.Reactive.Linq;
using System.Collections.Specialized;
using Reactive.Bindings.Extensions;
using TryMaui.Shared;

namespace TryMaui.ViewModels.Global;

public class TickListViewModel : BindableBase
{
    public ReactiveCollection<TestItem> Items { get; set; }
    public ReactivePropertySlim<TestItem?> SelectedItem { get; }

    public ICommand AddItemCommand { get; }
    public ICommand DeleteItemCommand { get; }
    public ReactiveCommandSlim<TestItem> StartItemCommand { get; }
    public ReactiveTimer Timer { get; }

    public ReadOnlyReactivePropertySlim<bool> DeleteEnabled { get; }

    public TickListViewModel()
    {
        Items = new ReactiveCollection<TestItem>();
        SelectedItem = new ReactivePropertySlim<TestItem?>();

        DeleteEnabled = SelectedItem.Select(x => x != null).ToReadOnlyReactivePropertySlim();

        Timer = new ReactiveTimer(TimeSpan.FromSeconds(1));
        Timer.Start();

        AddItemCommand = new Command(() => {
            var newItem = new TestItem(Timer);

            if (SelectedItem.Value is not null)
            {
                int index = Items.ToList().FindIndex(x => x.Id == SelectedItem.Value.Id);
                if (index == -1) index = Items.Count - 1;
                Items.InsertOnScheduler(index, newItem);
            }
            else
            {
                Items.AddOnScheduler(newItem);
            }
        });

        DeleteItemCommand = new Command(
            () => Items.RemoveOnScheduler(SelectedItem.Value!),
            () => SelectedItem.Value != null
        );

        Items.ToCollectionChanged()
            .Where(x =>
                (x.Action == NotifyCollectionChangedAction.Remove && x.Value?.Id == SelectedItem.Value?.Id)
                || x.Action == NotifyCollectionChangedAction.Reset
            )
            .Subscribe(_ => SelectedItem.Value = null)
            .AddTo(Disposable);

        StartItemCommand = new ReactiveCommandSlim<TestItem>()
            .WithSubscribe(x => x?.Start());
    }
}

