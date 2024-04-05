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
    private string AzureCoginitiveServiceConnectionString = "https://smartstockvision.cognitiveservices.azure.com/";
    private string AzureCoginitiveServiceKey = "93dedf53a6b743a5b61580088c7fe82a";
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

        BinaryData binaryData = new BinaryData(data);
        int maxTag = 5;
        List<string> dates = new List<string>();
        List<string> tags = new List<string>();
        string expiryDate = null;

        ImageAnalysisClient client = new ImageAnalysisClient(new Uri(AzureCoginitiveServiceConnectionString), new AzureKeyCredential(AzureCoginitiveServiceKey));

        ImageAnalysisResult result = await client.AnalyzeAsync(binaryData, VisualFeatures.Tags | VisualFeatures.Read);

        string pattern = @"\b\d{1,2}/\d{1,2}/\d{2}(?:\d{2})?\b"; //for dates like : 12/01/21  OR 12/01/2021 (date/month/year)

        if (result?.Tags?.Values.Count > maxTag)
        {
            for (int i = 0; i < maxTag; i++)
            {
                tags.Add(result.Tags.Values[i].Name);
            }
        }
        else
        {
            foreach (DetectedTag obj in result?.Tags?.Values)
            {
                tags.Add(obj.Name);
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
        //converting tags into a string and image in bytes
        byte[] image = File.ReadAllBytes(Photo);
        await dataBaseService.AddScannedEntity(
            new ScannedEntity { 
                ExpiryDate = analyzeImage.expiryDate,
                Tags = JsonSerializer.Serialize(analyzeImage.tags),
                Image = image
        });
        await Shell.Current.GoToAsync("..");
    }
}




