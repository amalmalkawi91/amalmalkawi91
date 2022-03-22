using Aquary.Models;
using Aquary.ViewModels;
using Newtonsoft.Json;
using Plugin.Media;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Aquary.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class My_Profile : ContentPage
    {
        RestService _restService;
        public My_Profile()
        {
            InitializeComponent();
            _restService = new RestService();
            BindingContext = new My_Profile_header();
            footer.ProfileIconSource = "icon_Login_blue.png";


        }

       
        public async void Get_My_Profile()
        {

            var client = new HttpClient();
            //  int main_service_id = Convert.ToInt32(Application.Current.Properties["main_service_id"]);
            string API = Constants.GitHubReposEndpoint1 + "get_user_details?register_id=" + Convert.ToInt32(Application.Current.Properties["register_id"]) + "";
            HttpResponseMessage response = await client.GetAsync(API);
            var result = await response.Content.ReadAsStringAsync();

            // var Get_Result = JsonConvert.DeserializeObject(result);
            My_Profile_Property MPP = new My_Profile_Property();
            MPP = JsonConvert.DeserializeObject<My_Profile_Property>(result);
            Email.Text = MPP.email;
            Name.Text = MPP.full_name;
            Phone.Text = MPP.phone;
            LblName.Text = MPP.full_name;
            Profile_Picture.Source = MPP.logo;
        }

        private async void Edit_Clicked(object sender, EventArgs e)
        {
            string API = Constants.GitHubReposEndpoint1 + "update_user";
            string result = await Update_Register(API);

        }

        public async Task<string> Update_Register(string url)
        {

            var client = new HttpClient();
            My_Profile_Property MPP = new My_Profile_Property();
            MPP.register_id = Convert.ToInt32(Application.Current.Properties["register_id"]);
            MPP.phone = Phone.Text;
            MPP.full_name = Name.Text;


            string jsonData = JsonConvert.SerializeObject(MPP);
            var content = new StringContent(jsonData, Encoding.UTF8, "application/json");
            HttpResponseMessage response = await client.PostAsync(url, content);

            var result = await response.Content.ReadAsStringAsync();

            // var Get_Result = JsonConvert.DeserializeObject(result);
            Common_My_Profile VER = new Common_My_Profile();
            VER = JsonConvert.DeserializeObject<Common_My_Profile>(result);

            if (VER.result == "success")
            {
                await DisplayAlert("Alert", "Your information updated successfully", "OK");
               // await Navigation.PushAsync(new LoginPage());

            }
            else
            {
                await DisplayAlert("Alert", VER.msg_en, "OK");
            }
            return result;
        }

        private void upload_Clicked(object sender, EventArgs e)
        {
            UploadPhoto();
        }
        async void UploadPhoto()
        {
            await CrossMedia.Current.Initialize();

            if (!CrossMedia.Current.IsPickPhotoSupported)
            {
                await DisplayAlert("Alert", "Upload not supported to upload photo", "OK");
                return;
            }

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
        
            string url = Constants.GitHubReposEndpoint1 + "update_user/Register_photo/";        

            try
            {
                //read file into upfilebytes arrayProfile_Picture
                var upfilebytes = File.ReadAllBytes(file);
                var File_Name = Path.GetFileName(file);

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

        protected async override void OnAppearing()
        {
            //Write the code of your page here
            base.OnAppearing();
            try
            {
                NavigationPage.SetHasBackButton(this, false);
                //    var _navigation = Application.Current.MainPage.Navigation;
                // var _lastPage = _navigation.NavigationStack.LastOrDefault();
                //Remove last page
                //  _navigation.RemovePage(MenuPage);
      
               
               


                    int register_id = Convert.ToInt32(Application.Current.Properties["register_id"]);
                if (register_id > 0)
                {
                    var selectedLanguage = await Xamarin.Essentials.SecureStorage.GetAsync(Constants.SelctedLanguage);

                    if (selectedLanguage is null || selectedLanguage.ToString() == "En")
                    {
                      
                        Menu_Scroll_Lang.FlowDirection = FlowDirection.LeftToRight;
                    }
                    else
                    {
                        Menu_Scroll_Lang.FlowDirection = FlowDirection.RightToLeft;
                    
                    }

                    // await Shell.Current.GoToAsync("//LoginPage");
                    Get_My_Profile();
                }
                else
                {
                    var result = await this.DisplayAlert("Alert!", "Please Login to continue the process?", "Login", "Cancel");

                    if (result)
                    {
                     //  Navigation.PushAsync(new LoginPage());
                        //await Shell.Current.GoToAsync("//LoginPage");
                        await Navigation.PushAsync(new LoginPage());
                    }
                    else
                    {
                        App.CurrentSelectedIndex = 0;
                        await Navigation.PopToRootAsync();
                    }
                }
            }

            catch (Exception ex)
            {
              
                var result = await this.DisplayAlert("Alert!", "Please Login to continue the process?", "Login", "Cancel");

                if (result)
                {
                  //  Navigation.PushAsync(new LoginPage());
                    //await Shell.Current.GoToAsync("//LoginPage");
                    await Navigation.PushAsync(new LoginPage());
                
                }
                else
                {
                    App.CurrentSelectedIndex = 0;

                    await Navigation.PopToRootAsync();
                }

            }
        }

        private async void My_ads_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new My_AdsPage());
        }

        private async void My_Inter_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new My_interested());
        }

        private async void My_fav_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new My_Favorite());
        }

        private async void Change_Password_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new Change_Password());
        }

        private async void Logout_Clicked(object sender, EventArgs e)
        {
            //  await Shell.Current.GoToAsync("//LoginPage");
            Application.Current.Properties["register_id"]=0;
            Navigation.PushAsync(new LoginPage(),true);
           // await Shell.Current.GoToAsync("//LoginPage");
        }

        private void Show_Profile_Clicked(object sender, EventArgs e)
        {
            Show_Myprofile.IsVisible = true;
        }
    }
    class My_Profile_header : BaseViewModel
    {
        private Item _selectedItem;


        public My_Profile_header()
        {
            Title = "PROFILE";
            // Items = new ObservableCollection<Item>();
            // LoadItemsCommand = new Command(async () => await ExecuteLoadItemsCommand());

            // ItemTapped = new Command<Item>(OnItemSelected);

            //AddItemCommand = new Command(OnAddItem);
        }

    }
    public class My_Profile_Property
    {
        #region Common

        [JsonProperty("full_name")]
        public string full_name { get; set; }

        [JsonProperty("phone")]
        public string phone { get; set; }


        [JsonProperty("email")]
        public string email { get; set; }

        [JsonProperty("register_id")]
        public int register_id { get; set; }

        [JsonProperty("logo")]
        public string logo { get; set; }

        #endregion
    }
    class Common_My_Profile
    {

        public int register_id { get; set; }
        public string result { get; set; }
        public int error_code { get; set; }
        public string msg_en { get; set; }
        public string msg_ar { get; set; }

        public string logo { get; set; }

    }
}