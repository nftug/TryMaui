using System.Windows.Input;
using Reactive.Bindings;
using Reactive.Bindings.Extensions;
using TryMaui.Models;
using TryMaui.Shared;

namespace TryMaui.ViewModels;

public class ListViewPageViewModel : ViewModelBase
{
    public ReactiveCollection<ListItem> Items { get; set; }
    public ReactiveCollection<ListItem> Items2 { get; set; }
    public ReactivePropertySlim<ListItem> SelectedItem { get; }

    public ICommand ClickAddButtonCommand { get; }

    public ListViewPageViewModel()
    {
        Items = new ReactiveCollection<ListItem>
        {
            new("C#"), new("Visual Basic"), new("F#"),
            new("Java"), new("Kotlin"), new("Swift"),
            new("C++"), new("Rust"), new("Go")
        }
        .AddTo(Disposable);
        SelectedItem = new ReactivePropertySlim<ListItem>().AddTo(Disposable);
        Items2 = new ReactiveCollection<ListItem>().AddTo(Disposable);

        ClickAddButtonCommand = new Command(OnAddButtonClicked);
    }

    public async void OnAddButtonClicked()
    {
        var items = Items.Select(x => x.Name).ToArray();
        var selected = await Application.Current!.MainPage!.DisplayActionSheet("Select language", "Cancel", null, items);
        Items2.AddOnScheduler(Items.First(x => x.Name == selected));
    }
}
