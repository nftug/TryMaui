using Reactive.Bindings;
using TryMaui.Shared;
using TryMaui.ViewModels.Global;

namespace TryMaui.ViewModels.Pages;

public class TestViewPageViewModel : BindableBase
{
    public TickListViewModel ViewModel { get; } = null!;

    public TestViewPageViewModel() : this(AppEnvironment.Services.GetService<TickListViewModel>()!)
    {
    }

    public TestViewPageViewModel(TickListViewModel vm)
	{
        ViewModel = vm;
        ViewModel.Items
            .ToCollectionChanged()
            .Subscribe(x => System.Diagnostics.Debug.WriteLine(x.Action));
    }
}

