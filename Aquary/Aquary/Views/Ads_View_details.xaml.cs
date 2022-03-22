
using Aquary.Models;
using Aquary.ViewModels;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Map = Xamarin.Forms.Maps.Map;
using Xamarin.Forms;
using Xamarin.Forms.Maps;
using Xamarin.Forms.Xaml;
using Xamarin.Essentials;
namespace Aquary.Views
{
    public partial class Ads_View_details : ContentPage
    {

        Xamarin.Forms.Maps.Geocoder geocoder;
        int region_id = 0;
        double latitude, longitude;
        RestService _restService;
        public Ads_View_details()
        {
            InitializeComponent();
            _restService = new RestService();
            BindingContext = new Ads_View_details_1();
            geocoder = new Xamarin.Forms.Maps.Geocoder();


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

            string API = Constants.GitHubReposEndpoint1 + "get_ads_comment?ads_id=" + ads_id + "&sub_service_Id=" + sub_service_id;
            List<Repository> repositories = await _restService.GetRepositoriesAsync(API);
            collectionView.ItemsSource = repositories;
        }

        private void Button_Clicked(object sender, EventArgs e)
        {

        }
        private async void Icon_back_Clicked(object sender, EventArgs e)
        {
            await Navigation.PopAsync();
           
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
        private async void ImageButton_Clicked(object sender, EventArgs e)
        {


            ShareUri("http://aeqary.com/#/general-view/" + Convert.ToInt32(Application.Current.Properties["ads_id"]));


        }
        public async Task ShareUri(string uri)
        {
            await Share.RequestAsync(new ShareTextRequest
            {
                Uri = uri,
                Title = "Share Web Link"
            });
        }
        public async Task<string> add_Intrested(string url)
        {

            var client = new HttpClient();
            Cls_Fav_Intresed FI = new Cls_Fav_Intresed();
            FI.ads_id = Convert.ToInt32(Application.Current.Properties["ads_id"]);
            FI.fk_sub_service = Convert.ToInt32(Application.Current.Properties["sub_service_id"]);
            FI.fk_register_id = Convert.ToInt32(Application.Current.Properties["register_id"]);
            FI.Region = region_id;


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
                    var result = await this.DisplayAlert("Alert!", "Please Login to continue the process?", "Login", "Cancel");

                    if (result)
                    {
                        await Shell.Current.GoToAsync("//LoginPage");
                    }
                    else
                    {
                        //Dont do anything
                    }
                }
            }
            catch
            {
                var result = await this.DisplayAlert("Alert!", "Please Login to continue the process?", "Login", "Cancel");

                if (result)
                {
                    await Shell.Current.GoToAsync("//LoginPage");
                }
                else
                {
                    //Dont do anything
                }
            }
        }
        public async void Check_Fav_Intrrestead()
        {
            List<Deco_Ads_Details_Property> repositories = null;
            try
            {
                using (HttpClient httpClient = new HttpClient(new HttpClientHandler()))
                {
                    string API = Constants.GitHubReposEndpoint1 + "get_ads_favorite_interested?ads_id=" + Convert.ToInt32(Application.Current.Properties["ads_id"]) + "&fk_sub_Service=" + Convert.ToInt32(Application.Current.Properties["sub_service_id"]) + "&fk_register_id=" + Convert.ToInt32(Application.Current.Properties["register_id"]) + "";
                    using (HttpResponseMessage response = await httpClient.GetAsync(API))
                    {
                        if (response.IsSuccessStatusCode)
                        {
                            string content = await response.Content.ReadAsStringAsync();
                            repositories = JsonConvert.DeserializeObject<List<Deco_Ads_Details_Property>>(content);

                            Application.Current.Properties["Is_Interested"] = repositories[0].is_interested;
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
                    var result = await this.DisplayAlert("Alert!", "Please Login to continue the process?", "Login", "Cancel");

                    if (result)
                    {
                        await Shell.Current.GoToAsync("//LoginPage");
                    }
                    else
                    {
                        //Dont do anything
                    }
                }
            }
            catch
            {
                var result = await this.DisplayAlert("Alert!", "Please Login to continue the process?", "Login", "Cancel");

                if (result)
                {
                    await Shell.Current.GoToAsync("//LoginPage");
                }
                else
                {
                    //Dont do anything
                }
            }

        }

        public async void Get_Ads_Details()
        {
            Deco_Ads_Details_Property DADP = new Deco_Ads_Details_Property();
            var client = new HttpClient();
            //  int main_service_id = Convert.ToInt32(Application.Current.Properties["main_service_id"]);
            string API = Constants.GitHubReposEndpoint1 + "get_ads_sale_details?ads_id=" + Convert.ToInt32(Application.Current.Properties["ads_id"]) + "";


            GetRepositoriesAsync1(API);


        }


        public async Task<List<Ads_General_Details_Property>> GetRepositoriesAsync1(string uri)
        {

            List<Ads_General_Details_Property> repositories = null;
            try
            {
                using (HttpClient httpClient = new HttpClient(new HttpClientHandler()))
                {
                    using (HttpResponseMessage response = await httpClient.GetAsync(uri))
                    {
                        if (response.IsSuccessStatusCode)
                        {
                            string content = await response.Content.ReadAsStringAsync();
                            repositories = JsonConvert.DeserializeObject<List<Ads_General_Details_Property>>(content);


                            // Company_Name.Text = repositories[0].comp_name;
                            Lbltitle.Text= repositories[0].title;
                            LblPrice.Text = repositories[0].price;
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
                            Register_Logo.Source = repositories[0].register_logo;
                            //Register_Logo.Source = repositories[0].register_logo;

                            parking_val.Text = repositories[0].parking;
                            Street_categoury_Val.Text = repositories[0].street_category;
                            ads_description.Text = repositories[0].ads_description;
                            region_id =repositories[0].region_Id;
                            Payment_Val.Text=repositories[0].payment_type;
                            advertiser_type_Val.Text = repositories[0].advertiser_type;
                            //Owner_Phone.Text = repositories[0].phone;
                            //comp_description_result.Text = repositories[0].comp_description;
                            //register_full_name.Text = repositories[0].register_full_name;
                            //register_phone.Text = repositories[0].register_phone;
                            // inserted_date.Text = repositories[0].inserted_date;
                            latitude = repositories[0].latitude;
                            longitude = repositories[0].longitude;
                            #region Store
                            Store_building_Area.Text = repositories[0].building_area_fixed_value;                           
                            Store_Building_Status.Text = repositories[0].Building_Condition;
                            Store_Maintenance.Text = repositories[0].need_maintenance;
                            Store_Age.Text = repositories[0].building_age;                         
                            Store_Door.Text = repositories[0].number_doors;
                            #endregion


                            #region Hanger
                            Hanger_Area_Val.Text = repositories[0].building_area_fixed_value;
                            Hanger_Land_Area_Val.Text = repositories[0].land_area_value + "  " + repositories[0].land_area_type;                            
                            Hanger_Building_status.Text = repositories[0].Building_Condition;
                            Hanger_need_to_maintenance.Text = repositories[0].need_maintenance;
                            Hanger_Building_Age.Text = repositories[0].building_age;
                           // Hanger_Price.Text = repositories[0].price;
                            Hanger_door.Text = repositories[0].number_doors;

                            #endregion

                            #region Shop
                            Shop_Area_Val.Text = repositories[0].building_area_fixed_value;
                            shop_Building_status.Text = repositories[0].Building_Condition;
                            Shop_need_to_maintenance.Text = repositories[0].need_maintenance;
                            shop_Building_Age.Text = repositories[0].building_age;
                            //Shopr_Price.Text = repositories[0].price;
                            Shop_door.Text = repositories[0].number_doors;
                            Shop_Kitchen.Text = repositories[0].has_kitchen;

                            Shop_bathroom.Text = repositories[0].has_bathroom;
                          //  Shop_upper_block.Text = repositories[0].has;
                            #endregion

                            #region Office
                            Office_Area_Val.Text = repositories[0].building_area_fixed_value;
                            Office_Building_status.Text = repositories[0].Building_Condition;
                            Office_need_to_maintenance.Text = repositories[0].need_maintenance;
                            Office_Building_Age.Text = repositories[0].building_age;
                          //  Office_Price.Text = repositories[0].price;
                            office_room_number.Text = repositories[0].number_rooms;
                            Office_Kitchen.Text = repositories[0].has_kitchen;
                            Office_bathroom.Text = repositories[0].has_bathroom;
                            office_floor_number.Text = repositories[0].floor_number;
                            #endregion

                            #region Complex1
                            Complex_Land_Area_Val.Text = repositories[0].land_area_value;
                            Complex_area_val.Text = repositories[0].building_area_fixed_value;
                            Complex_need_to_maintenance.Text = repositories[0].need_maintenance;
                            Complex_Building_Age.Text = repositories[0].building_age;
                           // Complex_Price.Text = repositories[0].price;
                            Complex_Annual_income.Text = repositories[0].annual_income;
                            Complex_Floor_number.Text = repositories[0].floor_number;
                            complex_offices_number.Text = repositories[0].number_of_offices;
                            Complex_shop_number.Text = repositories[0].number_of_shops;
                            Complex_Store_number.Text = repositories[0].number_of_store;
                            Complex_Elevator.Text = repositories[0].has_elevator;


                            #endregion

                            #region ResidentIal
                            ResidentIal_Building_area_val.Text = repositories[0].building_area_fixed_value;
                            ResidentIal_Building_status.Text = repositories[0].Building_Condition;
                            ResidentIal_need_to_maintenance.Text = repositories[0].need_maintenance;
                            ResidentIal_Building_Age.Text = repositories[0].building_age;
                           // ResidentIal_Price.Text = repositories[0].price;
                            ResidentIal_Annual_income.Text = repositories[0].annual_income;
                            ResidentIal_Elevator.Text = repositories[0].has_elevator;
                            ResidentIal_Floor_number.Text = repositories[0].floor_number;
                            ResidentIal_appartment_number.Text = repositories[0].number_of_Appartments;
                            #endregion

                            #region Appartment
                            Appartment_area_val.Text = repositories[0].building_area_fixed_value;
                            Appartment_Building_status.Text = repositories[0].Building_Condition;
                            Appartment_need_to_maintenance.Text = repositories[0].need_maintenance;
                            Appartment_Building_Age.Text = repositories[0].building_age;
                           // Appartment_Price.Text = repositories[0].price;
                            Appartment_Elevator.Text = repositories[0].has_elevator;
                            Appartment_Floor_number.Text = repositories[0].floor_number;
                            Appartment_Number_of_room.Text = repositories[0].number_rooms;
                            Appartment_Number_of_normal_bath.Text = repositories[0].number_of_normal_bathroom;
                            Appartment_Number_of_master_bath.Text = repositories[0].number_of_master_bathroom;
                            Appartment_Number_of_balcons.Text = repositories[0].number_of_balcones;
                            Appartment_Roof.Text = repositories[0].has_roof;
                            Appartment_Furnished.Text = repositories[0].has_furnished;
                            Appartment_Garden.Text = repositories[0].has_garden;
                            #endregion

                            #region Farm
                            Farm_land_area_val.Text = repositories[0].land_area_value;
                            Farm_Has_building.Text = repositories[0].has_building;
                            Farm_Building_area_val.Text = repositories[0].building_area_fixed_value;
                            Farm_Number_of_room.Text = repositories[0].number_rooms;
                            Farm_Number_of_normal_bath.Text = repositories[0].number_of_normal_bathroom;
                            Farm_Number_of_master_bath.Text = repositories[0].number_of_master_bathroom;

                            Farm_Building_status.Text = repositories[0].Building_Condition;
                            Farm_need_to_maintenance.Text = repositories[0].need_maintenance;
                            Farm_Building_Age.Text = repositories[0].building_age;
                          //  Farm_Price.Text = repositories[0].price;

                            Farm_pool.Text = repositories[0].has_Pool;
                            Farm_Entertament.Text = repositories[0].has_entertament;
                            Farm_Crops.Text = repositories[0].has_crops;
                            Farm_Garden.Text = repositories[0].has_garden;
                            #endregion

                            #region Fill_Villa
                            Villa_land_area_val.Text = repositories[0].land_area_value + "  " + repositories[0].land_area_type; ;
                            Villa_Building_area_val.Text = repositories[0].building_area_fixed_value;
                            villa_Number_of_room.Text = repositories[0].number_rooms;
                            Villa_Number_of_normal_bath.Text = repositories[0].number_of_normal_bathroom;
                            Villa_Number_of_master_bath.Text = repositories[0].number_of_master_bathroom;
                            Villa_Building_status.Text = repositories[0].Building_Condition;
                            Villa_need_to_maintenance.Text = repositories[0].need_maintenance;
                            Villa_Building_Age.Text = repositories[0].building_age;


                          //  Villa_Price.Text = repositories[0].price;
                            Villa_elevator.Text = repositories[0].has_elevator;
                            Villa_Number_of_Floors.Text = repositories[0].number_of_floors;
                            Villa_Garden.Text = repositories[0].has_garden;

                            #endregion

                            #region Fill_Land
                            land_area_val.Text = repositories[0].land_area_value + "  " + repositories[0].land_area_type; ;
                            Land_Organization_type.Text = repositories[0].land_organization_type;
                            Land_Organization_Name.Text = repositories[0].Organizer_name_en;
                          
                            Land_Existence_of_services.Text = repositories[0].existence_of_services;
                            Land_Number_of_Street.Text= repositories[0].number_of_street;
                            #endregion
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
                    var result = await this.DisplayAlert("Alert!", "Please Login to continue the process?", "Login", "Cancel");

                    if (result)
                    {
                        await Shell.Current.GoToAsync("//LoginPage");
                    }
                    else
                    {
                        //Dont do anything
                    }
                }
            }
            catch
            {
                var result = await this.DisplayAlert("Alert!", "Please Login to continue the process?", "Login", "Cancel");

                if (result)
                {
                    await Shell.Current.GoToAsync("//LoginPage");
                }
                else
                {
                    //Dont do anything
                }
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


        protected async override void OnAppearing()
        {
            //Write the code of your page here
            base.OnAppearing();
            try
            {
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
                string code = Application.Current.Properties["service_code"].ToString();
                if (code == "HA")
                {
                    Hanger.IsVisible = true;
                }
                else if (code == "ST")
                {
                    Store.IsVisible = true;
                }
                else if (code == "SH")
                {
                    Shop.IsVisible = true;
                }
                else if (code == "OF")
                {
                    Office.IsVisible = true;
                }            
                else if (code == "CO")
                {
                    Complex1.IsVisible = true;
                }
                else if (code == "RE")
                {
                    ResidentIal.IsVisible = true;
                }
                else if (code == "AP")
                {
                    Appartment.IsVisible = true;
                }
                else if (code == "FA")
                {
                    Farm.IsVisible = true;
                }
                else if (code == "VI")
                {
                    Villa.IsVisible = true;
                }
                else if (code == "LA")
                {
                    Land.IsVisible = true;
                    Show_parking.IsVisible = false;
                }
                Get_Ads_Details();
                Check_Fav_Intrrestead();
                Get_comments();

                string API_ads = Constants.GitHubReposEndpoint1 + "get_ads_photo?ads_id=" + Convert.ToInt32(Application.Current.Properties["ads_id"]) + "&Sub_Service_Id=" + Convert.ToInt32(Application.Current.Properties["sub_service_id"]);

                GetAdsPhoto(API_ads);

            }
            catch (Exception EX)
            {
                await DisplayAlert("Alert", "Please Login to continue the process", "OK");
            }
        }

        private void Btn_DETAILS(object sender, EventArgs e)
        {
            Comment_tab.IsVisible = false;
            Location_tab.IsVisible = false;
            Description_tab.IsVisible = false;
            Details_tab.IsVisible = true;



            Btn_DESCRIPTION.BackgroundColor = Color.White;
            Btn_DESCRIPTION.TextColor = Color.FromHex("#1269C8");

            Btn_COMMENTS.BackgroundColor = Color.White;
            Btn_COMMENTS.TextColor = Color.FromHex("#1269C8");

            Btn_LOCATION.BackgroundColor = Color.White;
            Btn_LOCATION.TextColor = Color.FromHex("#1269C8");

            Btn_DETAILS1.BackgroundColor = Color.FromHex("#1269C8");
            Btn_DETAILS1.TextColor = Color.White;
        }

        private void Btn_DESCRIPTION_Clicked(object sender, EventArgs e)
        {
            Comment_tab.IsVisible = false;
            Location_tab.IsVisible = false;
            Description_tab.IsVisible = true;
            Details_tab.IsVisible = false;

            Btn_DESCRIPTION.BackgroundColor = Color.FromHex("#1269C8");
            Btn_DESCRIPTION.TextColor = Color.White;

            Btn_COMMENTS.BackgroundColor = Color.White;
            Btn_COMMENTS.TextColor = Color.FromHex("#1269C8");

            Btn_LOCATION.BackgroundColor = Color.White;
            Btn_LOCATION.TextColor = Color.FromHex("#1269C8");

            Btn_DETAILS1.BackgroundColor = Color.White;
            Btn_DETAILS1.TextColor = Color.FromHex("#1269C8");

        }

        private void Btn_COMMENTS_Clicked(object sender, EventArgs e)
        {
            Comment_tab.IsVisible = true;
            Location_tab.IsVisible = false;
            Description_tab.IsVisible = false;
            Details_tab.IsVisible = false;


            Btn_COMMENTS.BackgroundColor = Color.FromHex("#1269C8");
            Btn_COMMENTS.TextColor = Color.White;

            Btn_DESCRIPTION.BackgroundColor = Color.White;
            Btn_DESCRIPTION.TextColor = Color.FromHex("#1269C8");

            Btn_LOCATION.BackgroundColor = Color.White;
            Btn_LOCATION.TextColor = Color.FromHex("#1269C8");

            Btn_DETAILS1.BackgroundColor = Color.White;
            Btn_DETAILS1.TextColor = Color.FromHex("#1269C8");
        }

        private void Btn_LOCATION_Clicked(object sender, EventArgs e)
        {
            Comment_tab.IsVisible = false;
            Location_tab.IsVisible = true;
            Description_tab.IsVisible = false;
            Details_tab.IsVisible = false;
            SetupMap();

            Btn_LOCATION.BackgroundColor = Color.FromHex("#1269C8");
            Btn_LOCATION.TextColor = Color.White;

            Btn_COMMENTS.BackgroundColor = Color.White;
            Btn_COMMENTS.TextColor = Color.FromHex("#1269C8");

            Btn_DESCRIPTION.BackgroundColor = Color.White;
            Btn_DESCRIPTION.TextColor = Color.FromHex("#1269C8");


            Btn_DETAILS1.BackgroundColor = Color.White;
            Btn_DETAILS1.TextColor = Color.FromHex("#1269C8");
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


        async void btnCall_Click(object sender, System.EventArgs e)
        {

            if (await DisplayAlert(
               "Dial a Number",
               $"Would you like to call {"555555555555"}?",
               "Yes",
               "No"))
            {
                try
                {
                 
                    PhoneDialer.Open("555555555555");
                }
                catch (ArgumentNullException)
                {
                    await DisplayAlert("Unable to dial", "Phone number was not valid.", "OK");
                }
                catch (FeatureNotSupportedException)
                {
                    await DisplayAlert("Unable to dial", "Phone dialing not supported.", "OK");
                }
                catch (Exception ex)
                {
                    await DisplayAlert("Unable to dial", "Phone dialing failed.", "OK");
                }
            }













          //  if (!string.IsNullOrEmpty(LblRegisterphone.Text))
            //{
              //  await Call(LblRegisterphone.Text);
           // }
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
                await this.DisplayAlert("Alert!", "Phone Dialer is not supported on this device.", "OK");
            }
            catch (Exception ex)
            {
                await this.DisplayAlert("Alert!", ex.Message, "OK");
                // Other error has occurred.  
            }
        }

        #endregion
        private void Call_btn_Clicked_1(object sender, EventArgs e)
        {
            PhoneDialer.Open(LblRegisterphone.Text);
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
                    var result = await this.DisplayAlert("Alert!", "Please Login to continue the process?", "Login", "Cancel");

                    if (result)
                    {
                        await Shell.Current.GoToAsync("//LoginPage");
                    }
                    else
                    {
                        //Dont do anything
                    }
                }
            }
            catch
            {
                var result = await this.DisplayAlert("Alert!", "Please Login to continue the process?", "Login", "Cancel");

                if (result)
                {
                    await Shell.Current.GoToAsync("//LoginPage");
                }
                else
                {
                    //Dont do anything
                }
            }
            //Icon_Fav.Source = "icon_Filled_favorite.png";
        }
    }

    public class  Ads_View_details_1 :BaseViewModel
    {
        public Ads_View_details_1()
        {
            Title = "Ads details";
            // Items = new ObservableCollection<Item>();
            // LoadItemsCommand = new Command(async () => await ExecuteLoadItemsCommand());

            // ItemTapped = new Command<Item>(OnItemSelected);

            //AddItemCommand = new Command(OnAddItem);
        }

      
    }

    public class Ads_General_Details_Property
    {
        #region Common

        [JsonProperty("Area")]
        public string Area { get; set; }

        [JsonProperty("area_en")]
        public string area_en { get; set; }
        [JsonProperty("area_ar")]
        public string area_ar { get; set; }

        [JsonProperty("region_en")]
        public string region_en { get; set; }

        [JsonProperty("region_ar")]
        public string region_ar { get; set; }

        [JsonProperty("region_Id")]
        public int region_Id { get; set; }


        [JsonProperty("register_full_name")]
        public string register_full_name { get; set; }

        [JsonProperty("register_phone")]
        public string register_phone { get; set; }

        [JsonProperty("register_email")]
        public string register_email { get; set; }


        [JsonProperty("register_logo")]
        public string register_logo { get; set; }


        [JsonProperty("ads_description")]
        public string ads_description { get; set; }

        [JsonProperty("latitude")]
        public double latitude { get; set; }


        [JsonProperty("longitude")]
        public double longitude { get; set; }

        [JsonProperty("allow_comments")]
        public string allow_comments { get; set; }

        [JsonProperty("inserted_date")]
        public string inserted_date { get; set; }

        [JsonProperty("street_category")]
        public string street_category { get; set; }

        [JsonProperty("building_status")]
        public string Building_Condition { get; set; }

        [JsonProperty("parking")]
        public string parking { get; set; }

        [JsonProperty("building_age")]
        public string building_age { get; set; }

        [JsonProperty("need_maintenance")]
        public string need_maintenance { get; set; }

        [JsonProperty("advertiser_type")]
        public string advertiser_type { get; set; }

        [JsonProperty("price")]
        public string price { get; set; }

        [JsonProperty("rental_period")]
        public string rental_period { get; set; }

        [JsonProperty("payment_type")]
        public string payment_type { get; set; }

        [JsonProperty("number_doors")]
        public string number_doors { get; set; }
        [JsonProperty("building_area_type")]
        public string building_area_type { get; set; }

        [JsonProperty("building_area_fixed_value")]
        public string building_area_fixed_value { get; set; }

        [JsonProperty("building_area_from_value")]
        public string building_area_from_value { get; set; }

        [JsonProperty("building_area_to_value")]
        public string building_area_to_value { get; set; }

        [JsonProperty("land_area_value")]
        public string land_area_value { get; set; }

        [JsonProperty("land_area_type")]
        public string land_area_type { get; set; }
        
        [JsonProperty("has_bathroom")]
        public string has_bathroom { get; set; }

        [JsonProperty("has_kitchen")]
        public string has_kitchen { get; set; }

        [JsonProperty("number_rooms")]
        public string number_rooms { get; set; }

        [JsonProperty("floor_number")]
        public string floor_number { get; set; }

        [JsonProperty("number_of_floors")]
        public string number_of_floors { get; set; }

        [JsonProperty("number_of_offices")]
        public string number_of_offices { get; set; }

        [JsonProperty("number_of_shops")]
        public string number_of_shops { get; set; }

        [JsonProperty("number_of_store")]
        public string number_of_store { get; set; }

        [JsonProperty("number_of_Appartments")]
        public string number_of_Appartments { get; set; }

        [JsonProperty("number_of_normal_bathroom")]
        public string number_of_normal_bathroom { get; set; }

        [JsonProperty("number_of_master_bathroom")]
        public string number_of_master_bathroom { get; set; }


        [JsonProperty("number_of_balcones")]
        public string number_of_balcones { get; set; }

        [JsonProperty("number_of_street")]
        public string number_of_street { get; set; }

        [JsonProperty("annual_income")]
        public string annual_income { get; set; }

            [JsonProperty("has_elevator")]
            public string has_elevator { get; set; }

        [JsonProperty("has_roof")]
        public string has_roof { get; set; }

        [JsonProperty("has_furnished")]
        public string has_furnished { get; set; }

        [JsonProperty("has_garden")]
        public string has_garden { get; set; }

        [JsonProperty("has_Pool")]
        public string has_Pool { get; set; }


        [JsonProperty("has_entertament")]
        public string has_entertament { get; set; }

        [JsonProperty("has_crops")]
        public string has_crops { get; set; }

        [JsonProperty("land_organization_type")]
        public string land_organization_type { get; set; }

        [JsonProperty("land_organization_name")]
        public string land_organization_name { get; set; }

        [JsonProperty("Organizer_name_en")]
        public string Organizer_name_en { get; set; }

        [JsonProperty("organizer_name_ar")]
        public string organizer_name_ar { get; set; }

        [JsonProperty("existence_of_services")]
        public string existence_of_services { get; set; }
       
       
        [JsonProperty("title")]
        public string title { get; set; }
        [JsonProperty("has_building")]
        public string has_building { get; set; }

       

        #endregion
    }
}