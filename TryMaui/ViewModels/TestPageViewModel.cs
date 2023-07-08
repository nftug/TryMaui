using System.Reactive.Linq;
using System.Windows.Input;
using Reactive.Bindings;
using Reactive.Bindings.Extensions;
using TryMaui.Models;
using TryMaui.Shared;

namespace TryMaui.ViewModels;

public class TestPageViewModel : BindableBase
{
    public ReactiveCollection<TestItem> Items { get; }
    public ReactivePropertySlim<TestItem?> SelectedItem { get; }

    public ICommand AddItemCommand { get; }
    public ICommand DeleteItemCommand { get; }
    public ReactiveCommand<TestItem> StartItemCommand { get; }
    public ReactiveTimer Timer { get; }

    public ReadOnlyReactivePropertySlim<bool> DeleteEnabled { get; }

    public TestPageViewModel()
    {
        Items = new ReactiveCollection<TestItem>();
        SelectedItem = new ReactivePropertySlim<TestItem?>();

        DeleteEnabled = SelectedItem.Select(x => x != null).ToReadOnlyReactivePropertySlim();
        DeleteEnabled.Subscribe(x => System.Diagnostics.Debug.WriteLine(x));

        Timer = new ReactiveTimer(TimeSpan.FromSeconds(1));
        Timer.Start();

        AddItemCommand = new Command(() =>
        {
            var newItem = new TestItem(Timer);
            Items.AddOnScheduler(newItem);
        });

        DeleteItemCommand = new Command(() =>
        {
            if (SelectedItem.Value is null) return;
            Items.RemoveOnScheduler(SelectedItem.Value);
            SelectedItem.Value = null;
        });

        StartItemCommand = new ReactiveCommand<TestItem>()
            .WithSubscribe(x => x.Start());
    }
}
