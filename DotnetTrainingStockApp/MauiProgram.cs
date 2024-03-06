using CommunityToolkit.Maui;
using DotnetTrainingStockApp.ViewModels;
using DotnetTrainingStockApp.Views;
using Microsoft.Extensions.Logging;

namespace DotnetTrainingStockApp
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();
            builder
                .UseMauiApp<App>()
                .UseMauiCommunityToolkit()
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                    fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
                });

            builder.Services.AddSingleton<StockItemDetailsPage>();
            builder.Services.AddSingleton<StockItemDetailsViewModel>();


#if DEBUG
    		builder.Logging.AddDebug();
#endif

            return builder.Build();
        }
    }
}
