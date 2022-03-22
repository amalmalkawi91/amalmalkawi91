using Aquary.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
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


    public partial class Ads_Deco_View_Details : ContentPage
    {
        double latitude, longitude;
        RestService _restService;
        Xamarin.Forms.Maps.Geocoder geocoder;
        public Ads_Deco_View_Details()
        {
            InitializeComponent();
            _restService = new RestService();
            geocoder = new Xamarin.Forms.Maps.Geocoder();



        }
        protected async override void OnAppearing()
        {
            try
            {
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
                Get_My_Ads();
                Get_comments();
                Check_Fav_Intrrestead();
                Description_tab.IsVisible = true;


            }
            catch (Exception ex)
            {
                Debug.WriteLine("\tERROR {0}", ex.Message);
            }

        }

        public async void Check_Fav_Intrrestead()
        {
            List<Deco_Ads_Details_Property> repositories = null;
            try
            {
                using (HttpClient httpClient = new HttpClient(new HttpClientHandler()))
                {
                    string API = Constants.GitHubReposEndpoint1 +"get_ads_favorite_interested?ads_id=" + Convert.ToInt32(Application.Current.Properties["ads_id"])+"&fk_sub_Service="+Convert.ToInt32(Application.Current.Properties["sub_service_id"])+"&fk_register_id=" + Convert.ToInt32(Application.Current.Properties["register_id"]) +"";
                    using (HttpResponseMessage response = await httpClient.GetAsync(API))
                    {
                        if (response.IsSuccessStatusCode)
                        {
                            string content = await response.Content.ReadAsStringAsync();
                            repositories = JsonConvert.DeserializeObject<List<Deco_Ads_Details_Property>>(content);

                            Application.Current.Properties["Is_Interested"]= repositories[0].is_interested;
                            Application.Current.Properties["Is_Favorite"] = repositories[0].is_favorite;

                            if (Convert.ToInt32(Application.Current.Properties["Is_Interested"]) == 1)
                            {
                                //   btn_Inter.BackgroundColor = Color.Blue;
                                Icon_Intres.Source = "intrested_fill.png";
                            }
                            else
                            {
                                //  btn_Inter.BackgroundColor = Color.White;
                                Icon_Intres.Source = "Intrested_black.png";
                            }
                            if (Convert.ToInt32(Application.Current.Properties["Is_Favorite"]) == 1)
                            {
                                //btn_Fav.BackgroundColor = Color.Red;
                                Icon_Fav.Source = "Fav_Fill_Black.png";
                            }
                            else
                            {
                                //  btn_Fav.BackgroundColor = Color.White;
                                Icon_Fav.Source = "Fav_Black.png";
                            }


                        }


                    }
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine("\tERROR {0}", ex.Message);
            }

         


        }
        public async void Get_My_Ads()
        {
            Deco_Ads_Details_Property DADP = new Deco_Ads_Details_Property();
            var client = new HttpClient();
            //  int main_service_id = Convert.ToInt32(Application.Current.Properties["main_service_id"]);
            string API = Constants.GitHubReposEndpoint1 + "get_adv_details?ads_id=" + Convert.ToInt32(Application.Current.Properties["ads_id"]) + "";

            string API_ads = Constants.GitHubReposEndpoint1 + "get_ads_photo?ads_id=" + Convert.ToInt32(Application.Current.Properties["ads_id"]) + "&Sub_Service_Id="+ Convert.ToInt32(Application.Current.Properties["sub_service_id"]);
                     
          
            GetRepositoriesAsync1(API);
            GetAdsPhoto(API_ads);

        }

        public async Task<List<Deco_Ads_Details_Property>> GetAdsPhoto(string uri)
        {

            List<Deco_Ads_Details_Property> repositories = null;
            try
            {
                using (HttpClient httpClient = new HttpClient(new HttpClientHandler()))
                {
                    using (HttpResponseMessage response = await httpClient.GetAsync(uri))
                    {
                        if (response.IsSuccessStatusCode)
                        {
                            string content = await response.Content.ReadAsStringAsync();
                            repositories = JsonConvert.DeserializeObject<List<Deco_Ads_Details_Property>>(content);
                            collectionView_Photo.ItemsSource = repositories;
                            // latitude.Text = repositories[0].latitude;
                        }


                    }
                }


            }
            catch (Exception ex)
            {
                Debug.WriteLine("\tERROR {0}", ex.Message);
            }

            return repositories;
        }
        public async Task<List<Deco_Ads_Details_Property>> GetRepositoriesAsync1(string uri)
        {
          
            List<Deco_Ads_Details_Property> repositories = null;
            try
            {
                using (HttpClient httpClient = new HttpClient(new HttpClientHandler()))
                {
                    using (HttpResponseMessage response = await httpClient.GetAsync(uri))
                    {
                        if (response.IsSuccessStatusCode)
                        {
                            string content = await response.Content.ReadAsStringAsync();
                            repositories = JsonConvert.DeserializeObject<List<Deco_Ads_Details_Property>>(content);


                            Lbltitle.Text = repositories[0].comp_name;
                            LblArea.Text = repositories[0].area_en + "," + repositories[0].region_en;
                            var selectedLanguage = await Xamarin.Essentials.SecureStorage.GetAsync(Constants.SelctedLanguage);

                            if (selectedLanguage is null || selectedLanguage.ToString() == "En")
                            {
                                //
                                LblArea.Text = repositories[0].area_en + " , " + repositories[0].region_en;
                            }
                            else
                            {
                                LblArea.Text = repositories[0].Area + " , " + repositories[0].region_ar;
                            }
                            LblRegistername.Text = repositories[0].register_full_name;
                            LblRegisteremail.Text = repositories[0].register_email;
                            LblRegisterphone.Text = repositories[0].register_phone;
                            Facebook_Val.Text = repositories[0].facebook_link;
                            Instagram_Val.Text = repositories[0].instagram_link;
                            Email_Val.Text = repositories[0].email;
                            Owner_Phone.Text = repositories[0].phone;
                            comp_description_result.Text = repositories[0].comp_description;
                            Register_Logo.Source = repositories[0].register_logo;
                            // register_full_name.Text = repositories[0].register_full_name;
                            //  register_phone.Text = repositories[0].register_phone;
                            inserted_date.Text = repositories[0].inserted_date;
                            latitude = repositories[0].latitude;
                            longitude = repositories[0].longitude;
                        }
                       

                    }
                }

            
            }
            catch (Exception ex)
            {
                Debug.WriteLine("\tERROR {0}", ex.Message);
            }

            return repositories;
        }

        public async void Get_comments()
        {
            int ads_id = Convert.ToInt32(Application.Current.Properties["ads_id"]);
            int sub_service_id = Convert.ToInt32(Application.Current.Properties["sub_service_id"]);

            string API = Constants.GitHubReposEndpoint1 + "get_ads_comment?ads_id=" + ads_id +"&sub_service_Id=" + sub_service_id;
            List<Repository> repositories = await _restService.GetRepositoriesAsync(API);
            collectionView.ItemsSource = repositories;
        }

        private async void Comment_Clicked(object sender, EventArgs e)
        {
            try
            {
                int register_id = (Convert.ToInt32(Application.Current.Properties["register_id"]));
                if (register_id > 0)
                {
                    string API = Constants.GitHubReposEndpoint1 + "ads_comment";
                    string result = await Comment_1(API);
                }
                else
                {
                    await DisplayAlert("Alert", "Please login", "OK");
                }
            }
            catch
            {
                await DisplayAlert("Alert", "Please login", "OK");
            }
          
         

        }

        public async Task<string> Comment_1(string url)
        {

            var client = new HttpClient();
            Cls_Comments cm = new Cls_Comments();
            cm.ads_id = Convert.ToInt32(Application.Current.Properties["ads_id"]);
            cm.fk_sub_service = Convert.ToInt32(Application.Current.Properties["sub_service_id"]);
            cm.fk_register_id = Convert.ToInt32(Application.Current.Properties["register_id"]);
            cm.description = txt_Comment.Text;
            cm.comment_id = 0;
            cm.operation_type = "I";

            string jsonData = JsonConvert.SerializeObject(cm);
            // string jsonData = @"{""email"" : ""username"", ""password"" :  " + password + "";
            var content = new StringContent(jsonData, Encoding.UTF8, "application/json");
            HttpResponseMessage response = await client.PostAsync(url, content);

            var result = await response.Content.ReadAsStringAsync();

            // var Get_Result = JsonConvert.DeserializeObject(result);
            Common_Response_API CRA = new Common_Response_API();
            CRA = JsonConvert.DeserializeObject<Common_Response_API>(result);

            if (CRA.result == "success")
            {
                await DisplayAlert("Alert", "Your comment submitted successfully", "OK");

            }
            else
            {
                await DisplayAlert("Alert", CRA.msg_en, "OK");
            }
            return result;
        }


        public async Task<string> add_Fav(string url)
        {

            var client = new HttpClient();
            Cls_Fav_Intresed FI = new Cls_Fav_Intresed();
            FI.ads_id = Convert.ToInt32(Application.Current.Properties["ads_id"]);
            FI.fk_sub_service = Convert.ToInt32(Application.Current.Properties["sub_service_id"]);
            FI.fk_register_id = Convert.ToInt32(Application.Current.Properties["register_id"]);
           

            string jsonData = JsonConvert.SerializeObject(FI);          
            var content = new StringContent(jsonData, Encoding.UTF8, "application/json");
            HttpResponseMessage response = await client.PostAsync(url, content);
            var result = await response.Content.ReadAsStringAsync();

           
            Common_Response_API CRA = new Common_Response_API();
            CRA = JsonConvert.DeserializeObject<Common_Response_API>(result);

            if (CRA.result == "success")
            {
                if (Convert.ToInt32(Application.Current.Properties["Is_Favorite"]) == 0)
                {
                    Application.Current.Properties["Is_Favorite"] = 1;
                    //  btn_Fav.BackgroundColor = Color.Red;
                    Icon_Fav.Source = "Fav_Fill_Black.png";
                }
                else
                {
                    Application.Current.Properties["Is_Favorite"] = 0;
                    //  btn_Fav.BackgroundColor = Color.White;
                    Icon_Fav.Source = "Fav_Black.png";
                }

            }
            else
            {
                await DisplayAlert("Alert", CRA.msg_en, "OK");
            }
            return result;
        }

        public async Task<string> add_Intrested(string url)
        {

            var client = new HttpClient();
            Cls_Fav_Intresed FI = new Cls_Fav_Intresed();
            FI.ads_id = Convert.ToInt32(Application.Current.Properties["ads_id"]);
            FI.fk_sub_service = Convert.ToInt32(Application.Current.Properties["sub_service_id"]);
            FI.fk_register_id = Convert.ToInt32(Application.Current.Properties["register_id"]);
            FI.Region = 0;


            string jsonData = JsonConvert.SerializeObject(FI);
            var content = new StringContent(jsonData, Encoding.UTF8, "application/json");
            HttpResponseMessage response = await client.PostAsync(url, content);
            var result = await response.Content.ReadAsStringAsync();


            Common_Response_API CRA = new Common_Response_API();
            CRA = JsonConvert.DeserializeObject<Common_Response_API>(result);

            if (CRA.result == "success")
            {
                if (Convert.ToInt32(Application.Current.Properties["Is_Interested"]) == 0)
                {
                    Application.Current.Properties["Is_Interested"] = 1;
                    Icon_Intres.Source = "intrested_fill.png";

                }
                else
                {
                    Application.Current.Properties["Is_Interested"] = 0;
                    Icon_Intres.Source = "Intrested_black.png";
                    //   btn_Inter.BackgroundColor = Color.White;
                }

            }
            else
            {
                await DisplayAlert("Alert", CRA.msg_en, "OK");
            }
            return result;
        }

        private async void Add_Fav_Clicked(object sender, EventArgs e)
        {
            try
            {
                int register_id = (Convert.ToInt32(Application.Current.Properties["register_id"]));
                if (register_id > 0)
                {
                    string API = Constants.GitHubReposEndpoint1 + "ads_my_favorite";
                    string result = await add_Fav(API);
                }
                else
                {
                    await DisplayAlert("Alert", "Please login", "OK");
                }
            }
            catch
            {
                await DisplayAlert("Alert", "Please login", "OK");
            }
        }

        private async void Add_Inter_Clicked(object sender, EventArgs e)
        {
            try
            {
                int register_id = (Convert.ToInt32(Application.Current.Properties["register_id"]));
             

                if (register_id > 0)
                {
                    string API = Constants.GitHubReposEndpoint1 + "ads_my_interested";
                    string result = await add_Intrested(API);
                }
                else
                {
                    await DisplayAlert("Alert", "Please login", "OK");
                }
            }
            catch
            {
                await DisplayAlert("Alert", "Please login", "OK");
            }

        }

        private void Btn_DESCRIPTION_Clicked(object sender, EventArgs e)
        {
            Comment_tab.IsVisible = false;
            Location_tab.IsVisible = false;
            Description_tab.IsVisible = true;

            Btn_DESCRIPTION.BackgroundColor = Color.FromHex("#1269C8");
            Btn_DESCRIPTION.TextColor = Color.White;

            Btn_COMMENTS.BackgroundColor = Color.White;
            Btn_COMMENTS.TextColor = Color.FromHex("#1269C8");

            Btn_LOCATION.BackgroundColor = Color.White;
            Btn_LOCATION.TextColor = Color.FromHex("#1269C8"); ;
            //   Details_tab.IsVisible = false;
        }

        private void Btn_COMMENTS_Clicked(object sender, EventArgs e)
        {
            Comment_tab.IsVisible = true;
            Location_tab.IsVisible = false;
            Description_tab.IsVisible = false;


            Btn_COMMENTS.BackgroundColor = Color.FromHex("#1269C8");
            Btn_COMMENTS.TextColor = Color.White;

            Btn_DESCRIPTION.BackgroundColor = Color.White;
            Btn_DESCRIPTION.TextColor = Color.FromHex("#1269C8");

            Btn_LOCATION.BackgroundColor = Color.White;
            Btn_LOCATION.TextColor = Color.FromHex("#1269C8");
            //  Details_tab.IsVisible = false;
        }

        private void Btn_LOCATION_Clicked(object sender, EventArgs e)
        {
            Comment_tab.IsVisible = false;
           Location_tab.IsVisible = true;
            Description_tab.IsVisible = false;
          //  Details_tab.IsVisible = false;
            SetupMap();

            Btn_LOCATION.BackgroundColor = Color.FromHex("#1269C8");
            Btn_LOCATION.TextColor = Color.White;

            Btn_COMMENTS.BackgroundColor = Color.White;
            Btn_COMMENTS.TextColor = Color.FromHex("#1269C8");

            Btn_DESCRIPTION.BackgroundColor = Color.White;
            Btn_DESCRIPTION.TextColor = Color.FromHex("#1269C8");
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

        private void MapObject_MapClicked(object sender, MapClickedEventArgs e)
        {
            var postion = e.Position;
            SetAddress(postion);
        }

        public void AddPins(Position position)
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

        private async void SetAddress(Position p)
        {
            var addrs = (await Geocoding.GetPlacemarksAsync(new Xamarin.Essentials.Location(latitude, longitude))).FirstOrDefault();

            // var addrs = (await Geocoding.GetPlacemarksAsync(new Xamarin.Essentials.Location(p.Latitude, p.Longitude))).FirstOrDefault();
            string Street = $"{addrs.Thoroughfare} {addrs.SubThoroughfare} ";
            string City = $"{addrs.PostalCode} {addrs.Locality} ";
            string Country = addrs.CountryName;

            addressEntry.Text = Street + City + Country;

            AddPins(p);
        }

        private async void Icon_Intres_Clicked(object sender, EventArgs e)
        {
            try
            {
                int register_id = (Convert.ToInt32(Application.Current.Properties["register_id"]));


                if (register_id > 0)
                {
                    string API = Constants.GitHubReposEndpoint1 + "ads_my_interested";
                    string result = await add_Intrested(API);
                }
                else
                {
                    await DisplayAlert("Alert", "Please login", "OK");
                }
            }
            catch
            {
                await DisplayAlert("Alert", "Please login", "OK");
            }
        }

        private async void Icon_Fav_Clicked(object sender, EventArgs e)
        {
            try
            {
                int register_id = (Convert.ToInt32(Application.Current.Properties["register_id"]));
                if (register_id > 0)
                {
                    string API = Constants.GitHubReposEndpoint1 + "ads_my_favorite";
                    string result = await add_Fav(API);
                }
                else
                {
                    await DisplayAlert("Alert", "Please login", "OK");
                }
            }
            catch
            {
                await DisplayAlert("Alert", "Please login", "OK");
            }
        }

        private async void ImageButton_Clicked(object sender, EventArgs e)
        {


            ShareUri("http://aeqary.com/#/general-view/"+ Convert.ToInt32(Application.Current.Properties["ads_id"]));


        }
        public async Task ShareUri(string uri)
        {
            await Share.RequestAsync(new ShareTextRequest
            {
                Uri = uri,
                Title = "Share Web Link"
            });
        }

        private async void Icon_back_Clicked(object sender, EventArgs e)
        {
            await Navigation.PopAsync();
        }

        async void btnCall_Click(object sender, System.EventArgs e)
        {
            if (!string.IsNullOrEmpty(LblRegisterphone.Text))
            {
                await Call(LblRegisterphone.Text);
            }
        }
        public async Task Call(string number)
        {
            try
            {
                PhoneDialer.Open(number);
            }
            catch (FeatureNotSupportedException ex)
            {
                // txtNum.Text = "Phone Dialer is not supported on this device.";
            }
            catch (Exception ex)
            {
                // Other error has occurred.  
            }
        }
        private async void SetPinByAddress()
        {
            var addressPosition = new Position();
            //var addressLocation = await geocoder.GetPositionsForAddressAsync(addressEntry.Text);
            addressPosition = new Position(latitude, longitude);
            // addressPosition = new Position(31.8851779, 35.8481716);

            //  foreach (var position in addressLocation)
            //  {
            //    addressPosition = new Position(position.Latitude, position.Longitude);
            // }

            AddPins(addressPosition);
        }
        #endregion


    }


}
    public class Deco_Ads_Details_Property 
    {
    #region Common

    [JsonProperty("register_full_name")]
    public string register_full_name { get; set; }

    [JsonProperty("register_phone")]
    public string register_phone { get; set; }

    [JsonProperty("register_email")]
    public string register_email { get; set; }

    [JsonProperty("comp_name")]
    public string comp_name { get; set; }

        [JsonProperty("Area")]
    public string Area { get; set; }

    [JsonProperty("area_en")]
    public string area_en { get; set; }

   

    [JsonProperty("region_en")]
    public string region_en { get; set; }

    [JsonProperty("region_ar")]
    public string region_ar { get; set; }

    [JsonProperty("facebook_link")]
    public string facebook_link { get; set; }
    [JsonProperty("instagram_link")]
    public string instagram_link { get; set; }

  

    [JsonProperty("email")]
    public string email { get; set; }

    [JsonProperty("phone")]
    public string phone { get; set; }

    [JsonProperty("comp_description")]
    public string comp_description { get; set; }

    [JsonProperty("allow_comments")]
    public string allow_comments { get; set; }

    [JsonProperty("inserted_date")]
    public string inserted_date { get; set; }

    [JsonProperty("latitude")]
    public double latitude { get; set; }


    [JsonProperty("longitude")]
    public double longitude { get; set; }

    [JsonProperty("is_favorite")]
    public string is_favorite { get; set; }

    [JsonProperty("is_interested")]
    public string is_interested { get; set; }


    [JsonProperty("photo_path")]
    public string photo_path { get; set; }



    [JsonProperty("register_logo")]
    public string register_logo { get; set; }
    




    public int register_id { get; set; }
    public string result { get; set; }
    public int error_code { get; set; }
    public string msg_en { get; set; }
    public string msg_ar { get; set; }

    //  [JsonProperty("phone")]
    //public string phone { get; set; }


    //[JsonProperty("email")]
    //public string email { get; set; }

    //[JsonProperty("register_id")]
    //  public int register_id { get; set; }

    #endregion
}


