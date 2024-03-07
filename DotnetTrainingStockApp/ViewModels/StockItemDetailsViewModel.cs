using CommunityToolkit.Mvvm.ComponentModel;

namespace DotnetTrainingStockApp.ViewModels;

public partial class StockItemDetailsViewModel : ObservableObject
{
    [ObservableProperty]
    string? path;
}

