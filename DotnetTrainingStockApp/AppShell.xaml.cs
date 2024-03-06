using DotnetTrainingStockApp.Views;

namespace DotnetTrainingStockApp
{
    public partial class AppShell : Shell
    {
        public AppShell()
        {
            //syncfusion license key
            Syncfusion.Licensing.SyncfusionLicenseProvider.RegisterLicense("MzE0NDA3MEAzMjM0MmUzMDJlMzBHMm1sVDhoTmZybytXK2N2cjNlOVJMMFMzYnc1d3E4a1VWeUxSWkV6dXEwPQ==");
            InitializeComponent();

            Routing.RegisterRoute(nameof(StockItemDetailsPage), typeof(StockItemDetailsPage));
        }
    }
}
