using Azure;
using Azure.AI.Vision.ImageAnalysis;
using CommunityToolkit.Maui.Converters;
using DotnetTrainingStockApp.ViewModels;
using Microsoft.Maui.ApplicationModel;
using System.Drawing;
using System.Text.Json;
using System.Text.RegularExpressions;


namespace DotnetTrainingStockApp.Views;

[QueryProperty(nameof(Photo), nameof(Photo))]
public partial class StockItemDetailsPage : ContentPage
{
	public string? Photo {  get; set; }
    private AnalyzedImage analyzeImage { get; set; }
    private DataBaseService dataBaseService { get; set; }
    public StockItemDetailsPage(StockItemDetailsViewModel stockItemDetailsViewModel)
	{
		InitializeComponent();
        BindingContext = stockItemDetailsViewModel;
        dataBaseService = new DataBaseService();
    }

	protected override void OnAppearing()
	{
		base.OnAppearing();

        ((StockItemDetailsViewModel)BindingContext).Path = Photo;
        if (Photo != null)
        {
            //showing loading icon on main thread
            SetActivityIndicatorVisibility(true);

            //running another thread to analyze picture 
            Task.Run(async () =>
            {
                await PerformImageAnalysisAsync(Photo);
            });

        }
    }

    private async Task PerformImageAnalysisAsync(string Photo)
    {
        try
        {
            analyzeImage = await AnalyzeImage(File.ReadAllBytes(Photo)) as AnalyzedImage;
            //updating UI on main thread 
            MainThread.BeginInvokeOnMainThread(() =>
            {
                ((StockItemDetailsViewModel)BindingContext).Tags = analyzeImage.tags;
                ((StockItemDetailsViewModel)BindingContext).ExpiryDate = analyzeImage.expiryDate;
            });
        }
        finally
        {
            SetActivityIndicatorVisibility(false);
        }
    }

    private void SetActivityIndicatorVisibility(bool isVisible)
    {
        MainThread.BeginInvokeOnMainThread(() =>
        {
            ActivityHandler.IsRunning = isVisible;
            ActivityHandler.IsVisible = isVisible;
            ContentLayout.IsVisible = !isVisible; 
        });
    }

    private async Task<AnalyzedImage> AnalyzeImage(byte[] data)
    {

        //images used for scanning expiry date:
        //https://lovefoodhatewaste.co.nz/wp-content/uploads/2016/03/Butter-best-before-date-1920-cropped.jpg
        //https://res.cloudinary.com/bunch-media-library/image/upload/w_645,h_378,c_fill,g_faces,q_auto,f_auto,g_auto,fl_lossy/v1605837183/articles/apw7xpihtlrdf5ckdhic.jpg
        //https://www.shutterstock.com/shutterstock/photos/2171820503/display_1500/stock-photo-selective-focus-on-manufacturing-date-and-expiry-date-indicator-aka-mfg-and-exp-dates-in-the-bottle-2171820503.jpg
        //https://pirg.org/edfund/wp-content/uploads/2021/09/Best-ByCan-1-scaled.jpg
        //https://prescriptionhope.com/wp-content/uploads/2020/01/Do-Medications-Expire.jpg

        BinaryData binaryData = new BinaryData(data);
        int maxTag = 5;
        string endpoint = "https://stockvision.cognitiveservices.azure.com/";
        string key = "209f0f88a4fd4522808428efe55be4c3";
        List<string> dates = new List<string>();
        List<string> tags = new List<string>();
        string expiryDate = null;

        ImageAnalysisClient client = new ImageAnalysisClient(new Uri(endpoint), new AzureKeyCredential(key));

        ImageAnalysisResult result = await client.AnalyzeAsync(binaryData, VisualFeatures.Tags | VisualFeatures.Read);

        string pattern = @"\b\d{1,2}/\d{1,2}/\d{2}(?:\d{2})?\b"; //for dates like : 12/01/21  OR 12/01/2021 (date/month/year)

        if (result?.Tags?.Values.Count > maxTag)
        {
            for (int i = 0; i < maxTag; i++)
            {
                tags.Add(result.Tags.Values[i].Name);
                Console.WriteLine(result.Tags.Values[i].Name);
            }
        }
        else
        {
            foreach (DetectedTag obj in result?.Tags?.Values)
            {
                tags.Add(obj.Name);
                Console.WriteLine(obj.Name);
            }
        }

        foreach (DetectedTextBlock block in result.Read.Blocks)
        {
            foreach (DetectedTextLine line in block.Lines)
            {
                foreach (DetectedTextWord word in line.Words)
                {
                    if (word.Text.Contains("/"))
                    {
                        //adding all dates having format with /
                        dates.Add(word.Text);
                    }
                }
            }
        }

        if (dates.Count > 0)
        {
            Match match = Regex.Match(dates[dates.Count - 1], pattern);
            if (match.Success)
            {
                expiryDate = dates[dates.Count - 1];
            }
        }

        return new AnalyzedImage
        {
            expiryDate = expiryDate ?? "Not Found",
            tags = tags
        };
    }

    private async void OnAddToCartBtnClicked(object sender, EventArgs e)
	{
		await Shell.Current.GoToAsync("..");
	}

    private async void AddToDbBtn_Clicked(object sender, EventArgs e)
    {
        byte[] image = File.ReadAllBytes(Photo);
        await dataBaseService.AddScannedEntity(new ScannedEntity { ExpiryDate = analyzeImage.expiryDate,
        Tags = JsonSerializer.Serialize(analyzeImage.tags),
        Image = image
        });
        await Shell.Current.GoToAsync("..");
        //ImageIcon.Source = ImageSource.FromStream(() => new MemoryStream(image)); ;
    }
}




