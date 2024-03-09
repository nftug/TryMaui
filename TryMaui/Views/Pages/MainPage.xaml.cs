using TryMaui.ViewModels.Pages;

namespace TryMaui.Views.Pages;

public partial class MainPage : ContentPage
{
	public MainPage(MainPageViewModel viewModel)
	{
		InitializeComponent();
		BindingContext = viewModel;
	}
}
