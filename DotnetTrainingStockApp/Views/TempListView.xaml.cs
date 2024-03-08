namespace DotnetTrainingStockApp.Views;

public partial class TempListView : ContentPage
{
	public List<ScannedEntities> scannedEntities;
	private ScannedEntitiesContextModel scannedEntitiesContextModel;
	public TempListView()
	{
        InitializeComponent();
		scannedEntitiesContextModel = new ScannedEntitiesContextModel();
		GetListFromDatabase();
    }

	private async void GetListFromDatabase()
	{
		scannedEntities = await scannedEntitiesContextModel.getItemsFromDb();
		Console.WriteLine(scannedEntities);
		ItemsListView.ItemsSource = scannedEntities;
	}
}