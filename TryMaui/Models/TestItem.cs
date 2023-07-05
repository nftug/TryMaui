using System.Reactive.Linq;
using Reactive.Bindings;
using Reactive.Bindings.Extensions;
using TryMaui.Shared;

namespace TryMaui.Models;

public class TestItem : BindableBase
{
    public ReactivePropertySlim<DateTime?> StartedOn { get; }
    public ReadOnlyReactivePropertySlim<TimeSpan> Duration { get; }
    public ReadOnlyReactivePropertySlim<string?> DisplayText { get; }

    public TestItem(ReactiveTimer timer)
    {
        StartedOn = new ReactivePropertySlim<DateTime?>().AddTo(Disposable);

        // timerのかわりにObservable.Interval()をSubscribeすると、個々のインスタンスでタイマーを生成する
        Duration = timer
            .CombineLatest(StartedOn, (_, s) => (_, s))
            .Where(x => x.s != null)
            .Select(x => DateTime.Now - x.s ?? TimeSpan.Zero)
            .ToReadOnlyReactivePropertySlim()
            .AddTo(Disposable);

        DisplayText = Duration
            .Select(x => $"{x:mm\\:ss}")
            .ToReadOnlyReactivePropertySlim()
            .AddTo(Disposable);
    }

    public void Start()
    {
        StartedOn.Value = DateTime.Now;
    }
}
