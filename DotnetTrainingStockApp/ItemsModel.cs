using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace DotnetTrainingStockApp
{
    public class ItemsModel
    {
        private PreferenceService preferenceService = new PreferenceService();
        public ObservableCollection<Items> ItemsList {  get; set; }
        public ItemsModel()
        {
            ItemsList = new ObservableCollection<Items>();
            if (preferenceService.DoesContainsKey("cart"))
            {
                ItemsList = preferenceService.GetDataFromPreferences<ObservableCollection<Items>>("cart");
            }
        }
    }


    public class Items
    {
        public Items()
        {

        }
        public Items(string itemName, int quanity, double price)
        {
            ItemName = itemName;
            Quanity = quanity;
            ItemPrice = price;
            TotalAmount = quanity * price;
        }
        public string ItemName { get; set; }
        public int Quanity { get; set; }
        public double TotalAmount { get; set; }
        public double ItemPrice { get; set; }
    }
}
