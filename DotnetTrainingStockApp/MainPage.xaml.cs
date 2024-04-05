using DotnetTrainingStockApp.Views;

using System.Collections.ObjectModel;
using System.Text.Json;

namespace DotnetTrainingStockApp
{
    public partial class MainPage : ContentPage
    {
        PreferenceService preferenceService;

        public MainPage()
        {
            InitializeComponent();
            preferenceService  = new PreferenceService();
        }


        private async void OnTakePhotoBtnClicked(object sender, EventArgs e)
        {
            if (MediaPicker.Default.IsCaptureSupported)
            {
                FileResult photo = await MediaPicker.Default.CapturePhotoAsync();

                if (photo != null)
                {
                    string localFilePath = Path.Combine(FileSystem.CacheDirectory, photo.FileName);

                    using (Stream sourceStream = await photo.OpenReadAsync())
                    {
                        using (FileStream localFileStream = File.OpenWrite(localFilePath))
                        {
                            await sourceStream.CopyToAsync(localFileStream);
                        }
                    }

                    var route = $"{nameof(StockItemDetailsPage)}";
                    await Shell.Current.GoToAsync($"{route}?Photo={localFilePath}");
                }
            }
        }

        private async void OnSelectPhotoBtnClicked(object sender, EventArgs e)
        {
            if (MediaPicker.Default.IsCaptureSupported)
            {
                FileResult photo = await MediaPicker.Default.PickPhotoAsync();

                if (photo != null)
                {
                    // save the file into local storage
                    string localFilePath = Path.Combine(FileSystem.CacheDirectory, photo.FileName);

                    using (Stream sourceStream = await photo.OpenReadAsync())
                    {
                        using (FileStream localFileStream = File.OpenWrite(localFilePath))
                        {
                            await sourceStream.CopyToAsync(localFileStream);
                        }
                    }


                    var route = $"{nameof(StockItemDetailsPage)}";
                    await Shell.Current.GoToAsync($"{route}?Photo={localFilePath}");
                }
            }
        }

        private void NextPageButtonClicked(object sender, EventArgs e)
        {
            Navigation.PushAsync(new TempListView());
        }
    }

}
