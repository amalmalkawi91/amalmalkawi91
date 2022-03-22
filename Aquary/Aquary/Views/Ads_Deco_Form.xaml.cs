using Aquary.Models;
using Newtonsoft.Json;
using Plugin.Media;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms.Maps;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Xamarin.Essentials;

using Aquary.ViewModels;
using System.Diagnostics;
using Map = Xamarin.Forms.Maps.Map;
using MultiImagePicker;

using Plugin.Media;
using Plugin.Permissions;
using Plugin.Permissions.Abstractions;
using Aquary.Services;
using Aquary.Models;

namespace Aquary.Views
{
 
    public partial class Ads_Deco_Form : ContentPage
    {
        Xamarin.Forms.Maps.Geocoder geocoder;

        RestService _restService;
        Ads_Deco_Parameters Adp = new Ads_Deco_Parameters();
        Repository RU = new Repository();
        DateTime End_date;
        string Photo_Path = "0";
        double latitude, longitude=0;
        int Price, ads_id, Sub_Service_Id=0;
        string[] Photo_path_array;
        public Ads_Deco_Form()
        {
            InitializeComponent();
            _restService = new RestService();
            BindingContext = new Ads_Deco_header();
            geocoder = new Xamarin.Forms.Maps.Geocoder();
            Device.BeginInvokeOnMainThread(async () => await AskForPermissions());
            SetupMap();

        }
          private async Task<bool> AskForPermissions()
        {
            try
            {
                await CrossMedia.Current.Initialize();

                var storagePermissions = await CrossPermissions.Current.CheckPermissionStatusAsync(Permission.Storage);
                var photoPermissions = await CrossPermissions.Current.CheckPermissionStatusAsync(Permission.Photos);
                if (storagePermissions != Plugin.Permissions.Abstractions.PermissionStatus.Granted || photoPermissions != Plugin.Permissions.Abstractions.PermissionStatus.Granted)
                {
                    var results = await CrossPermissions.Current.RequestPermissionsAsync(new[] { Permission.Storage, Permission.Photos });
                    storagePermissions = results[Permission.Storage];
                    photoPermissions = results[Permission.Photos];
                }

                if (storagePermissions != Plugin.Permissions.Abstractions.PermissionStatus.Granted || photoPermissions != Plugin.Permissions.Abstractions.PermissionStatus.Granted)
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
        protected async override void OnAppearing()
        {
            try { 
            //Write the code of your page here
            base.OnAppearing();
            var selectedLanguage = await Xamarin.Essentials.SecureStorage.GetAsync(Constants.SelctedLanguage);

            if (selectedLanguage is null || selectedLanguage.ToString() == "En")
            {
                //
                Menu_Scroll_Lang.FlowDirection = FlowDirection.LeftToRight;
            }
            else
            {
                Menu_Scroll_Lang.FlowDirection = FlowDirection.RightToLeft;

            }

          //  MessagingCenter.Subscribe<App, List<string>>((App)Xamarin.Forms.Application.Current, "ImagesSelected", (s, images) =>
         //   {
               // if (images is null || !images.Any())
               // {
                 //   return;
              //  }

              //  foreach (var image in images)
               // {
                 //   Upload_Toserver(image);
              //  }
          //  });
            Get_City();
            Price_Val.Text = "0";
            lbldisp.Text = "100";



            if (Application.Current.Properties["Operation_Type"] == "I")
            {
                Application.Current.Properties["fk_ads_pack_id"] = 1;
                Application.Current.Properties["fk_AdvertiserType_id"] = 1;
                Application.Current.Properties["allow_comments"] = 1;
                Application.Current.Properties["ads_status"] = 1;
                Application.Current.Properties["latitude"] = 0;
                Application.Current.Properties["longitude"] = 0;
                Application.Current.Properties["chk_Has_ads"] = 0;
                End_date = DateTime.Today.AddDays(100);
            }
            else if (Application.Current.Properties["Operation_Type"] == "U")
            {
                 ads_id = Convert.ToInt32(Application.Current.Properties["ads_id"]);
                Get_Ads_Details(ads_id);
            }

        }
            catch (Exception e)
            {
                //debug

                await DisplayAlert("Alert", e.ToString(), "OK");
                return;
            }
}
                public async void Get_Ads_Details(int ads_id)
        {
            try
           {

         
            List<Repository> repositories = null;
            var client = new HttpClient();
            //  int main_service_id = Convert.ToInt32(Application.Current.Properties["main_service_id"]);
            string API = Constants.GitHubReposEndpoint1 + "get_adv_select?ads_id=" + Convert.ToInt32(Application.Current.Properties["ads_id"]) + "&register_id=" + Convert.ToInt32(Application.Current.Properties["register_id"]) + "";
            HttpResponseMessage response = await client.GetAsync(API);
            var result = await response.Content.ReadAsStringAsync();
            if (response.IsSuccessStatusCode)
            {

                repositories = JsonConvert.DeserializeObject<List<Repository>>(result);

                if (repositories.Count>0)
                {
                        Email_Val.Text = repositories[0].email;
                        Company_Name.Text = repositories[0].comp_name;
                        comp_description_result.Text = repositories[0].comp_description;
                        Owner_Phone.Text= repositories[0].phone;
                        Facebook_Val.Text= repositories[0].facebook_link;
                        Instagram_Val.Text= repositories[0].instagram_link;
                       City_Val.SelectedItem= repositories[0].fk_city_id;
                        City_Val.SelectedIndex = repositories[0].fk_city_id;
                        Get_Region(repositories[0].fk_city_id);
                        Region_val.SelectedItem = repositories[0].fk_region_id;
                        Region_val.SelectedIndex = repositories[0].fk_region_id;
                        chk_Has_ads.IsChecked = Convert.ToBoolean(repositories[0].has_ads);

                        var addressPosition = new Position();
                        latitude =Convert.ToDouble(repositories[0].latitude);
                        longitude = Convert.ToDouble(repositories[0].longitude);
                        SetAddress(addressPosition);

                        #region Ads_status
                        int Ads_status = Convert.ToInt32(repositories[0].ads_status);
                        if (Ads_status == 1)
                        {
                            rdb_Active_1.IsChecked = true;
                            rdb_Inactive_0.IsChecked = false;

                        }
                        else if (Ads_status == 0)
                        {
                            rdb_Active_1.IsChecked = false;
                            rdb_Inactive_0.IsChecked = true;

                        }
                        #endregion

                        #region Allow_comment
                        int Allow_comment = Convert.ToInt32(repositories[0].allow_comments);
                        if (Allow_comment == 1)
                        {
                            rdb_Allow_comment_1.IsChecked = true;
                            rdb_Allow_comment_0.IsChecked = false;

                        }
                        else if (Allow_comment == 0)
                        {
                            rdb_Allow_comment_1.IsChecked = false;
                            rdb_Allow_comment_0.IsChecked = true;

                        }
                        #endregion

                  

                        #region Ads_Pack_Type
                        int Rdb_Ads_Pack_Type = Convert.ToInt32(repositories[0].fk_ads_pack_id);
                        if (Rdb_Ads_Pack_Type == 1)
                        {
                            rdb_Ads_Type_1.IsChecked = true;
                            rdb_Ads_Type_2.IsChecked = false;
                            rdb_Ads_Type_3.IsChecked = false;
                            rdb_Ads_Type_4.IsChecked = false;
                        }
                        else if (Rdb_Ads_Pack_Type == 2)
                        {
                            rdb_Ads_Type_1.IsChecked = false;
                            rdb_Ads_Type_2.IsChecked = true;
                            rdb_Ads_Type_3.IsChecked = false;
                            rdb_Ads_Type_4.IsChecked = false;
                        }
                        else if (Rdb_Ads_Pack_Type == 3)
                        {
                            rdb_Ads_Type_1.IsChecked = false;
                            rdb_Ads_Type_2.IsChecked = false;
                            rdb_Ads_Type_3.IsChecked = true;
                            rdb_Ads_Type_4.IsChecked = false;
                        }
                        else if (Rdb_Ads_Pack_Type == 4)
                        {
                            rdb_Ads_Type_1.IsChecked = false;
                            rdb_Ads_Type_2.IsChecked = false;
                            rdb_Ads_Type_3.IsChecked = false;
                            rdb_Ads_Type_4.IsChecked = true;
                        }
                        #endregion


                    }
                    else
                {
                    await DisplayAlert("Alert", repositories[0].msg_en, "OK");
                }
              
            }
            }
            catch (Exception ex)
            {
                await DisplayAlert("Alert", ex.Message, "OK");
            }
         
        }

        private async void Submit_Clicked(object sender, EventArgs e)
        {
            string API = Constants.GitHubReposEndpoint1 + "decoration";
           Ads_Deco_1(API);
        }
        public async void Get_City()
        {
            string API = Constants.GitHubReposEndpoint1 + "get_city";
            List<Repository> repositories = await _restService.GetRepositoriesAsync(API);
            City_Val.ItemsSource = repositories;
          
        }
        public async void Get_Region(int City_id)
        {
           // int City_id = Convert.ToInt32(Application.Current.Properties["get_region"]);
            string API = Constants.GitHubReposEndpoint1 + "get_region?city_Id=" + City_id + "";
            List<Repository> repositories = await _restService.GetRepositoriesAsync(API);
            Region_val.ItemsSource = repositories;
        }

        private void City_Val_SelectedIndexChanged(object sender, EventArgs e)
        {
       
           var selectedItem = (Repository)City_Val.SelectedItem;

             int City_id = Convert.ToInt32(selectedItem.city_id);

            Application.Current.Properties["fk_city_id"] = City_id;
      
            Get_Region(City_id);
        }

        private void Region_val_SelectedIndexChanged(object sender, EventArgs e)
        {
            var selectedItem = (Repository)Region_val.SelectedItem;

          //  int Region_Id = Convert.ToInt32(selectedItem.region_id);

        //    Adp.fk_region_id = Convert.ToInt32(selectedItem.region_id);
            Application.Current.Properties["fk_Region_id"] = selectedItem.region_id;

           

        }

        void OnAdsTypeRadioButtonCheckedChanged(object sender, CheckedChangedEventArgs e)
        {           
            RadioButton button = sender as RadioButton;  
            int AdsType_Val= Convert.ToInt32(button.Value);


            Application.Current.Properties["fk_ads_pack_id"] = Convert.ToInt32(button.Value);
          
            if (AdsType_Val != 1)
            {
                lbldisp.Text = "1";
                Stepper_price.Value = 1;
               
            }
            else
            {
                lbldisp.Text = "100";
                Stepper_price.Value = 100;
               // Price = 0;
              //  Price_Val.Text = "0";
            }
            Get_Price();
        }

        void OnAdvertiserTypeRadioButtonCheckedChanged(object sender, CheckedChangedEventArgs e)
        {
            RadioButton button = sender as RadioButton;
            Adp.fk_AdvertiserType_id = Convert.ToInt32(button.Value);
        }

        void OnAllowcommentRadioButtonCheckedChanged(object sender, CheckedChangedEventArgs e)
        {
            RadioButton button = sender as RadioButton;
            //Adp.allow_comments = Convert.ToInt32(button.Value);
            Application.Current.Properties["allow_comments"] = Convert.ToInt32(button.Value);
        }
        void OnAds_statusRadioButtonCheckedChanged(object sender, CheckedChangedEventArgs e)
        {
            RadioButton button = sender as RadioButton;
           // Adp.ads_status = Convert.ToInt32(button.Value);
            Application.Current.Properties["ads_status"] = Convert.ToInt32(button.Value);
        }

        private void chk_Has_ads_CheckedChanged(object sender, CheckedChangedEventArgs e)
        {
            CheckBox button = sender as CheckBox;
            Application.Current.Properties["chk_Has_ads"] = Convert.ToInt32(button.IsChecked);
        }

        private void Stepper_ValueChanged(object sender, ValueChangedEventArgs e)
        {
            int days =Convert.ToInt32(e.NewValue);
            lbldisp.Text = days.ToString();

            int Total_Price = Price * days;
            Price_Val.Text = Total_Price.ToString();

            End_date = DateTime.Today.AddDays(days);


        }

        public async void Get_Price()
        {
            var client = new HttpClient();        

            string API = Constants.GitHubReposEndpoint1 + "get_ads_type_price?main_service_Id=" + Convert.ToInt32(Application.Current.Properties["main_service_id"]) + "&sub_service_Id=" + Convert.ToInt32(Application.Current.Properties["sub_service_id"]) + "&ads_type_id=" + Convert.ToInt32(Application.Current.Properties["fk_ads_pack_id"]) + "";
            HttpResponseMessage response = await client.GetAsync(API);
            var result = await response.Content.ReadAsStringAsync();

            Repository MPP = new Repository();
            MPP = JsonConvert.DeserializeObject<Repository>(result);
            Price = Convert.ToInt32(MPP.price);
            Price_Val.Text = Price.ToString();
          

        }

        private async void upload_Clicked(object sender, EventArgs e)
        {
           //  UploadPhoto();
            Upload_Multi_Photo();


        }

        public async void Upload_Multi_Photo()
        {
            try
            {


                //Check users permissions.
                var storagePermissions = await CrossPermissions.Current.CheckPermissionStatusAsync(Permission.Storage);
                var photoPermissions = await CrossPermissions.Current.CheckPermissionStatusAsync(Permission.Photos);
                if (storagePermissions == Plugin.Permissions.Abstractions.PermissionStatus.Granted && photoPermissions == Plugin.Permissions.Abstractions.PermissionStatus.Granted)
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
                            int count = 1;
                            if (images.Count > 0)
                            {
                                foreach (string img in images)
                                {
                                    Upload_Toserver(img);
                                    Photo_path_array[count] = img;
                                    // Retrieve reference to a blob named "newphoto#.jpg".
                                    //  CloudBlockBlob blockBlob = container.GetBlockBlobReference("newphoto" + count + ".jpg");

                                    // Create the "newphoto#.jpg" blob with the current image in our list.
                                    //  await blockBlob.UploadFromFileAsync(img);

                                    count++;
                                }

                             
                                //  ImgCarouselView.ItemsSource = images;
                                // InfoText.IsVisible = true; //InfoText is optional
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

        private void Next_Clicked(object sender, EventArgs e)
        {
            
            image_Location_tab.IsVisible = true;
            Details_tab.IsVisible = false;


        }

        async void UploadPhoto()
        {
            await CrossMedia.Current.Initialize();

            if (!CrossMedia.Current.IsPickPhotoSupported)
            {
                await DisplayAlert("Alert", "Upload not supported to upload photo", "OK");
                return;
            }
         //   await DependencyService.Get<DependencyServices.IMediaService>().OpenGallery();
            var file = await CrossMedia.Current.PickPhotoAsync(new Plugin.Media.Abstractions.PickMediaOptions
            {
                PhotoSize = Plugin.Media.Abstractions.PhotoSize.Full,
                CompressionQuality = 40
           });
            string image_name = file.Path;
      
            Upload_Toserver(image_name);
            // Convert file to byre array, to bitmap and set it to our ImageView

            //byte[] imageArray = System.IO.File.ReadAllBytes(file.Path);


        }

        private async void Upload_Toserver(string file)
        {

            string url = Constants.GitHubReposEndpoint1 + "decoration/saveads/";

            try
            {
                //read file into upfilebytes arrayProfile_Picture
                var upfilebytes = File.ReadAllBytes(file);
                var File_Name = Path.GetFileName(file);
                Photo_Path = File_Name;
                //create new HttpClient and MultipartFormDataContent and add our file, and StudentId
                HttpClient client = new HttpClient();
                MultipartFormDataContent content = new MultipartFormDataContent();
                ByteArrayContent baContent = new ByteArrayContent(upfilebytes);
                //StringContent studentIdContent = new StringContent("2123");
                content.Add(baContent, "File", File_Name);
                //upload MultipartFormDataContent content async and store response in response var
                var response =
                    await client.PostAsync(url, content);

                //read response result as a string async into json var
                var responsestr = response.Content.ReadAsStringAsync().Result;

                //debug
                await DisplayAlert("Alert", responsestr, "OK");
            

            }
            catch (Exception e)
            {
                //debug

                await DisplayAlert("Alert", e.ToString(), "OK");
                return;
            }
        }

        public void Save_Multi_Photos_Data()
        {
            MessagingCenter.Subscribe<App, List<string>>((App)Xamarin.Forms.Application.Current, "ImagesSelectedAndroid", (s, images) =>
            {
                //If we have selected images, put them into the carousel view.
                int count = 1;
                if (images.Count > 0)
                {
                    foreach (string img in images)
                    {
                        // Upload_Toserver(img);
                        var File_Name = Path.GetFileName(img);
                    //    Photo_Path = File_Name;
                      //  Save_Photos_Data( Photo_Path);

                        count++;
                    }
                }
            });
        }

        public async Task<string> Save_Photos_Data(string Photo_Path)
        {

            string API = Constants.GitHubReposEndpoint1 + "decoration/saveadsphotodata";
            var client = new HttpClient();
            CLS_Photo_Data RP = new CLS_Photo_Data();
            RP.operation_type = "I";
            RP.ads_id = ads_id;
            RP.FK_Sub_Service_Id = Convert.ToInt32(Application.Current.Properties["sub_service_id"]);
            RP.Photo_Path = Photo_Path;


            string jsonData = JsonConvert.SerializeObject(RP);
            // string jsonData = @"{""email"" : ""username"", ""password"" :  " + password + "";
            var content = new StringContent(jsonData, Encoding.UTF8, "application/json");
            HttpResponseMessage response = await client.PostAsync(API, content);

            var result = await response.Content.ReadAsStringAsync();

            // var Get_Result = JsonConvert.DeserializeObject(result);
            Register_User RU = new Register_User();
            RU = JsonConvert.DeserializeObject<Register_User>(result);

           
            return result;
        }

        //  public async Task<List<Repository>> GetRepositoriesAsync(string uri)
        public async Task<List<Repository>> Ads_Deco_1(string url)
        {
            List<Repository> repositories = null;
            var result="";
            try
            {
                var client = new HttpClient();

                if (Application.Current.Properties["Operation_Type"] == "U")
                {
                    Adp.operation_type = "U";
                    Adp.ads_id = Convert.ToInt32(Application.Current.Properties["ads_id"]);
                }
                else
                {
                    Adp.operation_type = "I";
                    Adp.ads_id = 0;
                }
              
                Adp.comp_name = Company_Name.Text;
                Adp.fk_main_service = Convert.ToInt32(Application.Current.Properties["main_service_id"]);
                Adp.fk_sub_service = Convert.ToInt32(Application.Current.Properties["sub_service_id"]);              

                Adp.fk_city_id = Convert.ToInt32(Application.Current.Properties["fk_city_id"]);
                Adp.fk_region_id = Convert.ToInt32(Application.Current.Properties["fk_Region_id"]);
                Adp.FK_Register_Id = Convert.ToInt32(Application.Current.Properties["register_id"]);
                Adp.fk_ads_pack_id = Convert.ToInt32(Application.Current.Properties["fk_ads_pack_id"]);
                Adp.facebook_link = Facebook_Val.Text;
                Adp.instagram_link = Instagram_Val.Text;
                Adp.email = Email_Val.Text;
                Adp.phone = Owner_Phone.Text;
                Adp.comp_description = comp_description_result.Text;
                Adp.has_ads = Convert.ToInt32(Application.Current.Properties["chk_Has_ads"]);
                Adp.ads_status = Convert.ToInt32(Application.Current.Properties["ads_status"]);
                Adp.allow_comments = Convert.ToInt32(Application.Current.Properties["allow_comments"]);
                Adp.start_date = DateTime.Today;
                Adp.end_date = End_date;
                Adp.image = "";
                Adp.latitude = latitude ;
                Adp.longitude = longitude;



                string jsonData = JsonConvert.SerializeObject(Adp);
             
                var content = new StringContent(jsonData, Encoding.UTF8, "application/json");
                HttpResponseMessage response = await client.PostAsync(url, content);

                 result = await response.Content.ReadAsStringAsync();
             
                repositories = JsonConvert.DeserializeObject<List<Repository>>(result);
              
                if (repositories[0].result == "success")
                {
                    ads_id =Convert.ToInt32(repositories[0].ads_id);

                      string API = Constants.GitHubReposEndpoint1 + "decoration/saveadsphotodata";


                      Save_Photos_Data(Photo_Path);
                   // Save_Multi_Photos_Data();
                    await DisplayAlert("Alert", repositories[0].result, "OK");
                    await Navigation.PopToRootAsync();
                  //  await Navigation.PushAsync(new My_AdsPage());
                }
                else
                {
                    await DisplayAlert("Alert", repositories[0].msg_en, "OK");
                }
            }
            catch (Exception ex)
            {
                await DisplayAlert("Alert", ex.Message, "OK");
            }
                return repositories;
            

          
        }


        #region Map
        public async void SetupMap()
        {
            try
            {

                addressEntry.Text = "Jordan";

                SetPinByAddress();

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                await DisplayAlert("Alert", ex.Message, "OK");
            }
        }

        private async void MapObject_MapClicked(object sender, MapClickedEventArgs e)
        {
            try
            {

            
            var postion = e.Position;
            SetAddress(postion);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                await DisplayAlert("Alert", ex.Message, "OK");
            }
        }


        private void rdb_Active_1_clicked(object sender, EventArgs e)
        {
            Application.Current.Properties["ads_status"] = Convert.ToInt32(rdb_Active_1.Value);

        }

        private void rdb_Inactive_0_clicked(object sender, EventArgs e)
        {
            Application.Current.Properties["ads_status"] = Convert.ToInt32(rdb_Inactive_0.Value);

        }

        private void rdb_Allow_comment_1_clicked(object sender, EventArgs e)
        {
            Application.Current.Properties["allow_comments"] = Convert.ToInt32(rdb_Allow_comment_1.Value);
        }

        private void rdb_Allow_comment_0_clicked(object sender, EventArgs e)
        {
            Application.Current.Properties["allow_comments"] = Convert.ToInt32(rdb_Allow_comment_0.Value);
        }

        private void rdb_Ads_Type_1_clicked(object sender, EventArgs e)
        {
            int AdsType_Val = Convert.ToInt32(rdb_Ads_Type_1.Value);


            Application.Current.Properties["fk_ads_pack_id"] = Convert.ToInt32(rdb_Ads_Type_1.Value);

            if (AdsType_Val != 1)
            {
                lbldisp.Text = "1";
                Stepper_price.Value = 1;

            }
            else
            {
                lbldisp.Text = "100";
                Stepper_price.Value = 100;
                // Price = 0;
                //  Price_Val.Text = "0";
            }
            Get_Price();
        }
        private void rdb_Ads_Type_2_clicked(object sender, EventArgs e)
        {
            int AdsType_Val = Convert.ToInt32(rdb_Ads_Type_2.Value);


            Application.Current.Properties["fk_ads_pack_id"] = Convert.ToInt32(rdb_Ads_Type_2.Value);

            if (AdsType_Val != 1)
            {
                lbldisp.Text = "1";
                Stepper_price.Value = 1;

            }
            else
            {
                lbldisp.Text = "100";
                Stepper_price.Value = 100;
                // Price = 0;
                //  Price_Val.Text = "0";
            }
            Get_Price();
        }

        private void rdb_Ads_Type_3_clicked(object sender, EventArgs e)
        {
            int AdsType_Val = Convert.ToInt32(rdb_Ads_Type_3.Value);


            Application.Current.Properties["fk_ads_pack_id"] = Convert.ToInt32(rdb_Ads_Type_3.Value);

            if (AdsType_Val != 1)
            {
                lbldisp.Text = "1";
                Stepper_price.Value = 1;

            }
            else
            {
                lbldisp.Text = "100";
                Stepper_price.Value = 100;
                // Price = 0;
                //  Price_Val.Text = "0";
            }
            Get_Price();
        }
        private void rdb_Ads_Type_4_clicked(object sender, EventArgs e)
        {
            int AdsType_Val = Convert.ToInt32(rdb_Ads_Type_4.Value);


            Application.Current.Properties["fk_ads_pack_id"] = Convert.ToInt32(rdb_Ads_Type_4.Value);

            if (AdsType_Val != 1)
            {
                lbldisp.Text = "1";
                Stepper_price.Value = 1;

            }
            else
            {
                lbldisp.Text = "100";
                Stepper_price.Value = 100;
                // Price = 0;
                //  Price_Val.Text = "0";
            }
            Get_Price();
        }
        public async void AddPins(Position position)
        {
            try
            {
                var pin = new Pin
            {
                Type = PinType.Place,
                Position = position,
                Label = addressEntry.Text
            };

            mapObject.Pins.Add(pin);
            mapObject.MoveToRegion(MapSpan.FromCenterAndRadius(position, new Distance(500)));
        }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                await DisplayAlert("Alert", ex.Message, "OK");
              }
}

        private async void SetAddress(Position p)
        {
            try
            {
                latitude = p.Latitude;
                longitude = p.Longitude;
                var addrs = (await Geocoding.GetPlacemarksAsync(new Xamarin.Essentials.Location(p.Latitude, p.Longitude))).FirstOrDefault();
             
                // var addrs = (await Geocoding.GetPlacemarksAsync(new Xamarin.Essentials.Location(p.Latitude, p.Longitude))).FirstOrDefault();
                string Street = $"{addrs.Thoroughfare} {addrs.SubThoroughfare} ";
           // string City = $"{addrs.PostalCode} {addrs.Locality} ";
          //  string Country = addrs.CountryName;

          //  addressEntry.Text = Street + City + Country;
                addressEntry.Text = Street ;

                mapObject.Pins.Clear();
                AddPins(p);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                await DisplayAlert("Alert", ex.Message, "OK");
            }
        }

        private async void SetPinByAddress()
        {
            var addressPosition = new Position();
            //var addressLocation = await geocoder.GetPositionsForAddressAsync(addressEntry.Text);
          //  addressPosition = new Position(latitude, longitude);
             addressPosition = new Position(31.8851779, 35.8481716);

            //  foreach (var position in addressLocation)
            //  {
            //    addressPosition = new Position(position.Latitude, position.Longitude);
            // }

            AddPins(addressPosition);
        }




        #endregion

    }

    class Ads_Deco_Parameters
    {
        public string operation_type { get; set; }
        public int ads_id { get; set; }
        public string comp_name { get; set; }
        public int fk_main_service { get; set; }
        public int fk_sub_service { get; set; }
        public int fk_city_id { get; set; }
        public int fk_region_id { get; set; }
        public int FK_Register_Id { get; set; }
        public int fk_ads_pack_id { get; set; }
        public int fk_AdvertiserType_id { get; set; }
        public string facebook_link { get; set; }
        public string instagram_link { get; set; }
        public string email { get; set; }
        public string phone { get; set; }
        public string comp_description { get; set; }
        public int has_ads { get; set; }
        public string logo { get; set; }
        public int ads_status { get; set; }
        public int allow_comments { get; set; }
        public DateTime start_date { get; set; }
        public DateTime end_date { get; set; }
        public string image { get; set; }
        public Double latitude { get; set; }
        public Double longitude { get; set; }

    }
    class Ads_Deco_header : BaseViewModel
    {
        private Item _selectedItem;


        public Ads_Deco_header()
        {
            Title = "ADD ADVERTISEMENT";
            // Items = new ObservableCollection<Item>();
            // LoadItemsCommand = new Command(async () => await ExecuteLoadItemsCommand());

            // ItemTapped = new Command<Item>(OnItemSelected);

            //AddItemCommand = new Command(OnAddItem);
        }

    }
    class CLS_Photo_Data
    {
        public string operation_type { get; set; }
        public int ads_id { get; set; }
        public int FK_Sub_Service_Id { get; set; }
        public string Photo_Path { get; set; }
       

    }
}