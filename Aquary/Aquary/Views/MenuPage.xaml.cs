using Aquary.Models;
using Newtonsoft.Json;
using Plugin.Multilingual;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Maps;
using Xamarin.Forms.Xaml;

namespace Aquary.Views
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class MenuPage : ContentPage
	{
        double latitude, longitude = 0;
        string Lang = "En";
        public MenuPage ()
		{
			InitializeComponent ();
		}
        protected async override void OnAppearing()
        {
            try
            {

          
            base.OnAppearing();
            Tab_Complain.IsVisible = false;


                var selectedLanguage = await Xamarin.Essentials.SecureStorage.GetAsync(Constants.SelctedLanguage);

                if (selectedLanguage is null || selectedLanguage.ToString() == "En")
                {
                    //
                    Menu_Scroll_Lang.FlowDirection = FlowDirection.LeftToRight;
                }
                else
                {
                    Menu_Scroll_Lang.FlowDirection = FlowDirection.RightToLeft;
                //    Lang_title.Text = "اللغة";
                    //rdb_langar.Content = "العربية";
                   // rdb_langen.Content = "أنجليزي";
                    //btn_Setting.Text = "الأعدادات";
                }







            //    Lang = await SecureStorage.GetAsync("Lang");
              //  if (Lang == "Ar")
          //  {
               // Menu_Scroll_Lang.FlowDirection = FlowDirection.RightToLeft;
               // Lang_title.Text = "اللغة";
                //rdb_langar.Content = "العربية";
              //  rdb_langen.Content = "أنجليزي";
               // btn_Setting.Text = "الأعدادات";
             // 
             //   }
           // else
            //{
          // Lang_title.Text = "Language";
                   
           // Menu_Scroll_Lang.FlowDirection = FlowDirection.LeftToRight;
           // }
            }
            catch (Exception ex)
            {
               
            }
            //
        }
      
        private async void Aboutus_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new About_us());
        }

        private async void contactus_Clicked(object sender, EventArgs e)
        {
           // await Navigation.PushAsync(new Contact_UsPage());
            Tab_Contact_Us.IsVisible = true;
            Tab_Complain.IsVisible = false;
            Tab_SETTING.IsVisible = false;
        }     

        private async void SETTING_Clicked(object sender, EventArgs e)
        {
            //await Navigation.PushAsync(new SettingPage());
            Tab_SETTING.IsVisible = true;
            Tab_Contact_Us.IsVisible = false;
            Tab_Complain.IsVisible = false;
        }
        private void rdb_langar_CheckedChanged(object sender, CheckedChangedEventArgs e)
        {
           // RadioButton button = sender as RadioButton;
            //Lang = button.Value.ToString();
            //Application.Current.Properties["Lang"] = Lang;
           // Xamarin.Essentials.SecureStorage.SetAsync("Lang", "Ar");
           // Menu_Scroll_Lang.FlowDirection = FlowDirection.RightToLeft;

            RadioButton button = sender as RadioButton;
            ChangeLanguageToArabic(button.Value.ToString());

        }

        private void rdb_langen_CheckedChanged(object sender, CheckedChangedEventArgs e)
        {
            //RadioButton button = sender as RadioButton;          
           // Lang = button.Value.ToString();
           // Application.Current.Properties["Lang"] = Lang;
           // Xamarin.Essentials.SecureStorage.SetAsync("Lang", "En");
           // Menu_Scroll_Lang.FlowDirection = FlowDirection.LeftToRight;
            RadioButton button = sender as RadioButton;
            ChangeLanguageToEnglish(button.Value.ToString());


        }
        private async void ChangeLanguageToArabic(string lang)
        {
            await Xamarin.Essentials.SecureStorage.SetAsync(Constants.SelctedLanguage, lang);

            CrossMultilingual.Current.CurrentCultureInfo = CrossMultilingual.Current.NeutralCultureInfoList.ToList().First(element => element.EnglishName.Contains(lang));
            AppResources.Culture = CrossMultilingual.Current.CurrentCultureInfo;
            Menu_Scroll_Lang.FlowDirection = FlowDirection.RightToLeft;
            App.Current.MainPage = new AppShell();
        }

        private async void ChangeLanguageToEnglish(string lang)
        {
            await Xamarin.Essentials.SecureStorage.SetAsync(Constants.SelctedLanguage, lang);

            CrossMultilingual.Current.CurrentCultureInfo = CrossMultilingual.Current.NeutralCultureInfoList.ToList().First(element => element.EnglishName.Contains(lang));
            AppResources.Culture = CrossMultilingual.Current.CurrentCultureInfo;
             Menu_Scroll_Lang.FlowDirection = FlowDirection.LeftToRight;
            App.Current.MainPage = new AppShell();
        }
        private async void Submit_Clicked(object sender, EventArgs e)
        {
            string API = Constants.GitHubReposEndpoint1 + "complain_suggest";
            string result = await Complain(API);


        }
        public async Task<string> Complain(string url)
        {

            var client = new HttpClient();
            complain_Parameters RP = new complain_Parameters();
            RP.register_id = Convert.ToInt32(Application.Current.Properties["register_id"]);
            RP.Tittle = Title.Text;
            RP.description = Description.Text;


            string jsonData = JsonConvert.SerializeObject(RP);
            // string jsonData = @"{""email"" : ""username"", ""password"" :  " + password + "";
            var content = new StringContent(jsonData, Encoding.UTF8, "application/json");
            HttpResponseMessage response = await client.PostAsync(url, content);

            var result = await response.Content.ReadAsStringAsync();

            // var Get_Result = JsonConvert.DeserializeObject(result);
            Register_User RU = new Register_User();
            RU = JsonConvert.DeserializeObject<Register_User>(result);

            if (RU.result == "success")
            {

                await DisplayAlert("Alert", "Success", "OK");
                Title.Text = "";
                Description.Text = "";

            }
            else
            {
                await DisplayAlert("Alert", "Error occurred", "OK");
            }
            return result;
        }

        private void btn_COMPLAIN_Clicked(object sender, EventArgs e)
        {
            Tab_Complain.IsVisible = true;
            Tab_Contact_Us.IsVisible = false;
            Tab_SETTING.IsVisible = false;


        }

        #region Map
        public async void SetupMap()
        {
            try
            {

                var addressPosition = new Position();
                latitude = 31.9777706;
                longitude = 35.855822;
                SetAddress(addressPosition);



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
                latitude = postion.Latitude;
                longitude = postion.Longitude;
                SetAddress(postion);
            }
            catch (Exception ex)
            {
                await DisplayAlert("Alert", ex.Message, "OK");
            }

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
                await DisplayAlert("Alert", ex.Message, "OK");
            }
        }

        private async void SetAddress(Position p)
        {
            try
            {


                var addrs = (await Geocoding.GetPlacemarksAsync(new Xamarin.Essentials.Location(latitude, longitude))).FirstOrDefault();

                // var addrs = (await Geocoding.GetPlacemarksAsync(new Xamarin.Essentials.Location(p.Latitude, p.Longitude))).FirstOrDefault();
                string Street = $"{addrs.Thoroughfare} {addrs.SubThoroughfare} ";
                string City = $"{addrs.PostalCode} {addrs.Locality} ";
                string Country = addrs.CountryName;

                addressEntry.Text = Street + City + Country;

                AddPins(p);
            }
            catch (Exception ex)
            {
                await DisplayAlert("Alert", ex.Message, "OK");
            }
        }

      

        private void Btn_LOCATION_Clicked(object sender, EventArgs e)
        {
            //RadioButton button = sender as RadioButton;
           // Application.Current.Properties["Lang"] = button.Value.ToString();
           // var newpage = new MenuPage();
           // newpage.FlowDirection = FlowDirection.LeftToRight;
        }

        private void rdb_langar_clicked(object sender, EventArgs e)
        {
            ChangeLanguageToArabic(rdb_langar.Value.ToString());
        }

        private void rdb_langen_clicked(object sender, EventArgs e)
        {
            ChangeLanguageToEnglish(rdb_langen.Value.ToString());
        }

        private async void SetPinByAddress(string Country)
        {
            var addressPosition = new Position();
            // var addressLocation = await geocoder.GetPositionsForAddressAsync(Country);           
            addressPosition = new Position(31.9777706, 35.855822);

            //  foreach (var position in addressLocation)
            //  {
            //    addressPosition = new Position(position.Latitude, position.Longitude);
            // }

            AddPins(addressPosition);
        }
        #endregion
    }
}