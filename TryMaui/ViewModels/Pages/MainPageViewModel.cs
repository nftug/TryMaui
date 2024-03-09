using System.Reactive.Linq;
using System.Windows.Input;
using Reactive.Bindings;
using Reactive.Bindings.Extensions;
using TryMaui.Shared;
using TryMaui.ViewModels.Global;

namespace TryMaui.ViewModels.Pages;

public class MainPageViewModel : BindableBase
{
    public TickListViewModel TickListViewModel { get; } = null!;

    public ReactivePropertySlim<int> Count { get; }
    public ReactivePropertySlim<string> CounterButtonText { get; }
    public ReadOnlyReactivePropertySlim<string> FizzBuzzText { get; }

    public ICommand ClickCounterCommand { get; }

    public ReadOnlyReactivePropertySlim<string> TickCounterText { get; }

#if DEBUG
    public MainPageViewModel() : this(null!) { }
#endif

    public MainPageViewModel(TickListViewModel tickListViewModel)
    {
        TickListViewModel = tickListViewModel;

        Count = new ReactivePropertySlim<int>();
        CounterButtonText = new ReactivePropertySlim<string>("Click me!");
        ClickCounterCommand = new Command(OnCounterClicked);

        FizzBuzzText = Count
            .Where(x => x > 0)
            .Select(x => x switch
            {
                _ when x % 15 == 0 => "FizzBuzz",
                _ when x % 3 == 0 => "Fizz",
                _ when x % 5 == 0 => "Buzz",
                _ => x.ToString()
            })
            .ToReadOnlyReactivePropertySlim(initialValue: string.Empty)
            .AddTo(Disposable);

        TickCounterText = TickListViewModel.Items
            .ObserveElementObservableProperty(x => x.StartedOn)
            .CombineLatest(TickListViewModel.Items.ToCollectionChanged())
            .Select(_ => $"{TickListViewModel.Items.Count(x => x.StartedOn.Value != null)} timers are active.")
            .ToReadOnlyReactivePropertySlim("0 timers are active.")
            .AddTo(Disposable);
    }

    public void OnCounterClicked()
    {
        Count.Value++;
        if (Count.Value == 1)
            CounterButtonText.Value = $"Clicked {Count} time";
        else
            CounterButtonText.Value = $"Clicked {Count} times";

        SemanticScreenReader.Announce(CounterButtonText.Value);
    }
}