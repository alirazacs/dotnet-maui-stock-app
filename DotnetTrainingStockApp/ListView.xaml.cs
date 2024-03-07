using System.Collections.ObjectModel;

namespace DotnetTrainingStockApp;
public partial class ListView : ContentPage
{
    private PreferenceService preferenceService = new PreferenceService();
    public ObservableCollection<Items> ItemsList { get; set; }
    public ListView()
	{
        InitializeComponent();
        ItemsList = new ObservableCollection<Items>();
        if (preferenceService.DoesContainsKey("cart"))
        {
            ItemsList = preferenceService.GetDataFromPreferences<ObservableCollection<Items>>("cart");
        }
        BindingContext = this;
    }

    private void DeleteItem(object sender, EventArgs e)
    {
		
        Button button = (Button)sender;
        var data = (Items)button.CommandParameter;
        ItemsList.Remove(data);
        
    }
}