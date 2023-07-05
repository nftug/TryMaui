using System.Windows.Input;
using Reactive.Bindings;
using Reactive.Bindings.Extensions;
using TryMaui.Models;
using TryMaui.Shared;

namespace TryMaui.ViewModels;

public class TestPageViewModel : BindableBase
{
    public ReactiveCollection<TestItem> Items { get; }

    public ICommand AddItemCommand { get; }
    public ReactiveCommand<TestItem> StartItemCommand { get; }
    public ReactiveTimer Timer { get; }

    public TestPageViewModel()
    {
        Items = new ReactiveCollection<TestItem>().AddTo(Disposable);
        Timer = new ReactiveTimer(TimeSpan.FromSeconds(1)).AddTo(Disposable);
        Timer.Start();

        AddItemCommand = new Command(() =>
        {
            var newItem = new TestItem(Timer);
            Items.AddOnScheduler(newItem);
        });

        StartItemCommand = new ReactiveCommand<TestItem>()
            .WithSubscribe(x => x.Start())
            .AddTo(Disposable);
    }
}
