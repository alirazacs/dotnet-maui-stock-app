using Newtonsoft.Json;
using System.Collections.ObjectModel;

namespace DotnetTrainingStockApp.Views;

public partial class TempListView : ContentPage
{
	public ObservableCollection<ScannedEntitiesExtended> scannedEntities {get; set;}
	private DataBaseService dataBaseService;
	public TempListView()
	{
        InitializeComponent();
		dataBaseService = new DataBaseService();
		GetListFromDatabase();
		BindingContext = this;
    }

	private async void GetListFromDatabase()
	{
		List<ScannedEntity> list = await dataBaseService.GetAllScannedEntities();

		scannedEntities = new ObservableCollection<ScannedEntitiesExtended>();
		foreach(ScannedEntity entity in list)
		{
			scannedEntities.Add(new ScannedEntitiesExtended
			{
				ExpiryDate = entity.ExpiryDate,
				Id = entity.Id,
				EntityImageSource = ImageSource.FromStream(() => new MemoryStream(entity.Image)),
				EntityTagsList = JsonConvert.DeserializeObject<List<string>>(entity.Tags)
			});
		}

		ItemsListView.ItemsSource = scannedEntities;
		ItemsListView.IsVisible = scannedEntities.Count != 0;
		EmptyListViewMessage.IsVisible = scannedEntities.Count == 0;

    }
}

//need to create this class because image and tags are saved as byte array and string in database.
//We are unable to call a method in xaml to get both of these attributes and modify them in required way
public class ScannedEntitiesExtended : ScannedEntity
{
	public ImageSource EntityImageSource { get; set;}
	public List<string> EntityTagsList { get; set; }
}