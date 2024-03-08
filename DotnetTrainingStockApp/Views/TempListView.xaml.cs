using System.Collections.ObjectModel;

namespace DotnetTrainingStockApp.Views;

public partial class TempListView : ContentPage
{
	public ObservableCollection<ScannedEntities> scannedEntities {get; set;}
	private ScannedEntitiesContextModel scannedEntitiesContextModel;
	public TempListView()
	{
        InitializeComponent();
		scannedEntitiesContextModel = new ScannedEntitiesContextModel();
		GetListFromDatabase();
    }

	private async void GetListFromDatabase()
	{
		List<ScannedEntities> list = await scannedEntitiesContextModel.getItemsFromDb();
		scannedEntities = new ObservableCollection<ScannedEntities>(list);
		BindingContext = this;
	}
}