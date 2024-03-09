using System.Reactive.Linq;
using System.Windows.Input;
using Reactive.Bindings;
using Reactive.Bindings.Extensions;
using TryMaui.Models;
using TryMaui.Shared;

namespace TryMaui.ViewModels.Pages;

public class ListViewPageViewModel : BindableBase
{
    public List<string> Source { get; set; }
    public ReactiveCollection<ListItem> Items { get; set; }
    public ReactivePropertySlim<ListItem?> SelectedItem { get; }
    public ReadOnlyReactivePropertySlim<bool> IsRemoveButtonEnabled { get; }

    public ReactiveCommandSlim ClickAddButtonCommand { get; }
    public ICommand ClickRemoveButtonCommand { get; }
    public ICommand ClickEditButtonCommand { get; }

    public ListViewPageViewModel()
    {
        Source = new()
        {
            new("C#"), new("Visual Basic"), new("F#"),
            new("Java"), new("Kotlin"), new("Swift"),
            new("C++"), new("Rust"), new("Go")
        };

        Items = new ReactiveCollection<ListItem>().AddTo(Disposable);
        SelectedItem = new ReactivePropertySlim<ListItem?>().AddTo(Disposable);
        IsRemoveButtonEnabled = SelectedItem
            .Select(x => x != null)
            .ToReadOnlyReactivePropertySlim()
            .AddTo(Disposable);

        ClickAddButtonCommand = new ReactiveCommandSlim().WithSubscribe(OnAddButtonClicked).AddTo(Disposable);
        ClickRemoveButtonCommand = new Command(() =>
        {
            if (SelectedItem.Value is null) return;

#if ANDROID
            // Reference: https://github.com/dotnet/maui/issues/12220
            // This is just a workaround.
            var items = Items.Where(x => x != SelectedItem.Value).ToList();
            Items.ClearOnScheduler();
            Items.AddRangeOnScheduler(items);
#else
            Items.RemoveOnScheduler(SelectedItem.Value);
#endif
            SelectedItem.Value = null;
        });

        ClickEditButtonCommand = new Command(async () =>
        {
            if (SelectedItem.Value is null) return;

            var promptResult = await Application.Current!.MainPage!.DisplayActionSheet(
                "Select language",
                "Cancel",
                null,
                Source.ToArray()
            );

            // var item = Items.First(x => x.Id == SelectedItem.Value.Id);
            SelectedItem.Value.Name.Value = promptResult;
        });
    }

    private async void OnAddButtonClicked()
    {
        var promptResult = await Application.Current!.MainPage!.DisplayActionSheet(
            "Select language",
            "Cancel",
            null,
            Source.ToArray()
        );
        if (!Source.Contains(promptResult)) return;

        var item = new ListItem();
        item.Name.Value = promptResult;

        if (item != null) Items.AddOnScheduler(item);
    }
}
