using Aquary;
using Aquary.Services;
using MultiImagePicker;

using Plugin.Media;
using Plugin.Permissions;
using Plugin.Permissions.Abstractions;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Aquary.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ImageSelectionPage : ContentPage
    {
        public ImageSelectionPage()
        {
            InitializeComponent();
            Device.BeginInvokeOnMainThread(async () => await AskForPermissions() );
        }

        private async void SelectImagesButton_Clicked(object sender, EventArgs e)
        {
            try
            {

            
            //Check users permissions.
            var storagePermissions = await CrossPermissions.Current.CheckPermissionStatusAsync(Permission.Storage);
            var photoPermissions = await CrossPermissions.Current.CheckPermissionStatusAsync(Permission.Photos);
            if (storagePermissions == PermissionStatus.Granted && photoPermissions == PermissionStatus.Granted)
            {
                //If we are on iOS, call GMMultiImagePicker.
             
                //If we are on Android, call IMediaService.
                 if (Device.RuntimePlatform == Device.Android)
                {
                    DependencyService.Get<IMediaService>().OpenGallery();

                    MessagingCenter.Unsubscribe<App, List<string>>((App)Xamarin.Forms.Application.Current, "ImagesSelectedAndroid");
                    MessagingCenter.Subscribe<App, List<string>>((App)Xamarin.Forms.Application.Current, "ImagesSelectedAndroid", (s, images) =>
                    {
                        //If we have selected images, put them into the carousel view.
                        if (images.Count > 0)
                        {
                            ImgCarouselView.ItemsSource = images;
                            InfoText.IsVisible = true; //InfoText is optional
                        }
                    });
                }
            }
            else
            {
                await DisplayAlert("Permission Denied!", "\nPlease go to your app settings and enable permissions.", "Ok");
            }
            }
            catch (Exception ex)
            {
                await DisplayAlert("Alert", ex.Message, "OK");
            }
        }

  

        /// <summary>
        ///     Compress Android images before uploading them to Azure Blob Storage.
        /// </summary>
        /// <param name="totalImages"></param>
        /// <returns></returns>
        private List<string> CompressAllImages(List<string> totalImages)
        {
            int displayCount = 1;
            int totalCount = totalImages.Count;
            List<string> compressedImages = new List<string>();
            foreach (string path in totalImages)
            {
                compressedImages.Add(DependencyService.Get<ICompressImages>().CompressImage(path));
                displayCount++;
            }
            return compressedImages;
        }

        /// <summary>
        ///     Make sure Permissions are given to the users storage.
        /// </summary>
        /// <returns></returns>
        private async Task<bool> AskForPermissions()
        {
            try
            {
                await CrossMedia.Current.Initialize();

                var storagePermissions = await CrossPermissions.Current.CheckPermissionStatusAsync(Permission.Storage);
                var photoPermissions = await CrossPermissions.Current.CheckPermissionStatusAsync(Permission.Photos);
                if (storagePermissions != PermissionStatus.Granted || photoPermissions != PermissionStatus.Granted)
                {
                    var results = await CrossPermissions.Current.RequestPermissionsAsync(new[] { Permission.Storage, Permission.Photos });
                    storagePermissions = results[Permission.Storage];
                    photoPermissions = results[Permission.Photos];
                }

                if (storagePermissions != PermissionStatus.Granted || photoPermissions != PermissionStatus.Granted)
                {
                    await DisplayAlert("Permissions Denied!", "Please go to your app settings and enable permissions.", "Ok");
                    return false;
                }
                else
                {
                    return true;
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("error. permissions not set. here is the stacktrace: \n" + ex.StackTrace);
                return false;
            }
        }

        /// <summary>
        ///     Unsubsribe from the MessagingCenter on disappearing.
        /// </summary>
        protected override void OnDisappearing()
        {
            base.OnDisappearing();
            MessagingCenter.Unsubscribe<App, List<string>>((App)Xamarin.Forms.Application.Current, "ImagesSelectedAndroid");
            MessagingCenter.Unsubscribe<App, List<string>>((App)Xamarin.Forms.Application.Current, "ImagesSelectediOS");
            GC.Collect();
        }
    }
}