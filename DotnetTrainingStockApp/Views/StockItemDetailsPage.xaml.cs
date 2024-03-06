namespace DotnetTrainingStockApp.Views;

public partial class StockItemDetailsPage : ContentPage
{
	public StockItemDetailsPage()
	{
		InitializeComponent();
	}

    private async void OnAddToCartBtnClicked(object sender, EventArgs e)
	{
		await Shell.Current.GoToAsync("..");
	}
}