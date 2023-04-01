using System.Reactive.Linq;
using System.Windows.Input;
using Reactive.Bindings;
using Reactive.Bindings.Extensions;
using TryMaui.Shared;

namespace TryMaui.ViewModels;

public class MainPageViewModel : ViewModelBase
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
                int n when n % 15 == 0 => "FizzBuzz",
                int n when n % 3 == 0 => "Fizz",
                int n when n % 5 == 0 => "Buzz",
                _ => x.ToString()
            })
            .ToReadOnlyReactivePropertySlim(initialValue: string.Empty);
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