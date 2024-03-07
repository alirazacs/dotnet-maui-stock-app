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
            addDataInLocalStorage();
        }

        private void addDataInLocalStorage()
        {
           /* List<Items>ItemsList = new List<Items>();
            ItemsList.Add(new Items("ABC", 1, 3.5));
            ItemsList.Add(new Items("ABC1", 23, 2));
            ItemsList.Add(new Items("ABC2", 3, 4.5));
            ItemsList.Add(new Items("ABC3", 5, 7.5));
            ItemsList.Add(new Items("ABC4", 1, 3.5));
            ItemsList.Add(new Items("ABC5", 23, 2));
            ItemsList.Add(new Items("ABC6", 3, 4.5));
            ItemsList.Add(new Items("ABC7", 5, 7.5));
            ItemsList.Add(new Items("ABC8", 1, 3.5));
            ItemsList.Add(new Items("ABC9", 23, 2));
            ItemsList.Add(new Items("ABC10", 3, 4.5));
            ItemsList.Add(new Items("ABC11", 5, 7.5));
            ItemsList.Add(new Items("ABC12", 3, 4.5));
            ItemsList.Add(new Items("ABC13", 5, 7.5));
            ItemsList.Add(new Items("ABC14", 3, 4.5));
            ItemsList.Add(new Items("ABC15", 5, 7.5));
            ItemsList.Add(new Items("ABC16", 3, 4.5));
            ItemsList.Add(new Items("ABC17", 5, 7.5));
            string serializedList = JsonSerializer.Serialize(ItemsList);
            preferenceService.SetDataInPreferences("cart", serializedList);*/
        }

        /*private void OnCounterClicked(object sender, EventArgs e)
        {
            count++;

            if (count == 1)
                CounterBtn.Text = $"Clicked {count} time";
            else
                CounterBtn.Text = $"Clicked {count} times";

            SemanticScreenReader.Announce(CounterBtn.Text);
        }*/

        private async void OnTakePhotoBtnClicked(object sender, EventArgs e)
        {
            if (MediaPicker.Default.IsCaptureSupported)
            {
                FileResult photo = await MediaPicker.Default.CapturePhotoAsync();

                if (photo != null)
                {
                    // save the file into local storage
                    string localFilePath = Path.Combine(FileSystem.CacheDirectory, photo.FileName);

                    //using Stream sourceStream = await photo.OpenReadAsync();
                    //using FileStream localFileStream = File.OpenWrite(localFilePath);

                    using (Stream sourceStream = await photo.OpenReadAsync())
                    {
                        using (FileStream localFileStream = File.OpenWrite(localFilePath))
                        {
                            await sourceStream.CopyToAsync(localFileStream);
                        }
                    }

                    //await sourceStream.CopyToAsync(localFileStream);

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
            Navigation.PushAsync(new ListView());
        }
    }

}
