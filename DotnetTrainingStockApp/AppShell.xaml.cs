using DotnetTrainingStockApp.Views;

namespace DotnetTrainingStockApp
{
    public partial class AppShell : Shell
    {
        public AppShell()
        {
            InitializeComponent();

            Routing.RegisterRoute(nameof(StockItemDetailsPage), typeof(StockItemDetailsPage));
        }
    }
}
