using System.Reactive.Linq;
using System.Windows.Input;
using Reactive.Bindings;
using Reactive.Bindings.Extensions;
using TryMaui.Shared;

namespace TryMaui.ViewModels;

public class MainPageViewModel : BindableBase
{
    public ReactivePropertySlim<int> Count { get; }
    public ReactivePropertySlim<string> CounterButtonText { get; }
    public ReadOnlyReactivePropertySlim<string> FizzBuzzText { get; }

    public ICommand ClickCounterCommand { get; }

    public MainPageViewModel()
    {
        Count = new ReactivePropertySlim<int>().AddTo(Disposable);
        CounterButtonText = new ReactivePropertySlim<string>("Click me!").AddTo(Disposable);
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