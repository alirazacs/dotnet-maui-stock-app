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
        public ObservableCollection<Items> ItemsList {  get; set; }
        public ItemsModel()
        {
            ItemsList = new ObservableCollection<Items>();
            if (Preferences.ContainsKey("cart"))
            {
                string data = Preferences.Get("cart", string.Empty);
                if (data != null)
                {
                    ItemsList = JsonSerializer.Deserialize<ObservableCollection<Items>>(data);
                }

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
