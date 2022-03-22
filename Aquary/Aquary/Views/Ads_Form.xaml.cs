using Aquary.Models;
using Aquary.Services;
using Aquary.ViewModels;
using Newtonsoft.Json;
using Plugin.Media;
using Plugin.Permissions;
using Plugin.Permissions.Abstractions;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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

    public partial class Ads_Form : ContentPage
    {
        Ads_General_Details__Form_Property Adp = new Ads_General_Details__Form_Property();
        RestService _restService;
        DateTime End_date;
        int street_category, building_status, building_age, rental_period, need_maintenance, price, number_doors, building_area_type, building_area_fixed_value, building_area_from_value, building_area_to_value = 0, Parking;
        int land_area_aalue, land_area_type, has_bathroom, has_kitchen, has_upper_block, number_rooms, floor_number, number_of_floors, number_of_offices, number_of_shops, number_of_Store, number_of_appartments = 0;
        int number_of_normal_bathroom, number_of_master_bathroom, number_of_balcones, number_of_street, annual_income, size_per_apartment, has_elevator, has_roof, has_furnished, has_garden, has_pool, has_entertament, has_crops, land_organization_type, existence_of_services, has_building, Ads_Default_Price, ads_id = 0;
        int fk_ads_pack_id, fk_AdvertiserType_id, allow_comments, ads_status, advertiser_type, payment_type = 1;
        string title, Photo_Path = "";
        int City_Id, Region_Id, land_organization_name = -1;
        double latitude, Ads_Price, Total_Price, longitude = 0;
        int x;
        Xamarin.Forms.Maps.Geocoder geocoder;

        public ObservableCollection<Advertiser_Type_List> advertiser_char = new ObservableCollection<Advertiser_Type_List>();
        public ObservableCollection<Advertiser_Type_List> Advertiser_char { get { return advertiser_char; } }

        public ObservableCollection<type_of_advertisment_list> advertisment_type = new ObservableCollection<type_of_advertisment_list>();
        public ObservableCollection<type_of_advertisment_list> Advertisment_type { get { return advertisment_type; } }


        public Ads_Form()
        {
            Console.WriteLine("debug");

            InitializeComponent();
            CityName.Text = "City";
            RegionName.Text = "Region";
            _restService = new RestService();
            BindingContext = new Ads_Deco_header();
            Advertiser_Type_Val.ItemsSource = advertiser_char;
            advertiser_char.Add(new Advertiser_Type_List { Name = "Proker" });
            advertiser_char.Add(new Advertiser_Type_List { Name = "Owner" });
            advertiser_char.Add(new Advertiser_Type_List { Name = "Company" });

            type_of_advertisment_Val.ItemsSource = advertisment_type;
            advertisment_type.Add(new type_of_advertisment_list { Name = "Normal" });
            advertisment_type.Add(new type_of_advertisment_list { Name = "Special" });
            advertisment_type.Add(new type_of_advertisment_list { Name = "Freez" });
            advertisment_type.Add(new type_of_advertisment_list { Name = "Unique" });

            Get_City();
            Device.BeginInvokeOnMainThread(async () => await AskForPermissions());
            geocoder = new Xamarin.Forms.Maps.Geocoder();
            SetupMap();
            lbldisp.Text = "1";
        }

        public async void Get_City()
        {
            string API = Constants.GitHubReposEndpoint1 + "get_city";
            List<Repository> repositories = await _restService.GetRepositoriesAsync(API);
            City_Val.ItemsSource = repositories;

        }

        public async void Get_Organization()
        {
            string API = Constants.GitHubReposEndpoint1 + "get_land_organizer";
            List<Repository> repositories = await _restService.GetRepositoriesAsync(API);
            Regulation_Val.ItemsSource = repositories;
            //Land_Organization_Name.ItemsSource = repositories;

        }



        public async void Get_Region(int City_id)
        {
            // int City_id = Convert.ToInt32(Application.Current.Properties["get_region"]);
            string API = Constants.GitHubReposEndpoint1 + "get_region?city_Id=" + City_id + "";
            List<Repository> repositories = await _restService.GetRepositoriesAsync(API);
            Region_Val.ItemsSource = repositories;


        }
        private void Land_Organization_Name_SelectedIndexChanged(object sender, EventArgs e)
        {

            var selectedItem = (Repository)Region_Val.SelectedItem;

            land_organization_name = Convert.ToInt32(selectedItem.organizer_id);


        }
        private async void City_Val_SelectedIndexChanged(object sender, EventArgs e)
        {
            var selectedItem = (Repository)City_Val.SelectedItem;

            City_Id = Convert.ToInt32(selectedItem.city_id);

            Get_Region(City_Id);

        }

        private async void CityButton_Clicked(object sender, EventArgs e)
        {
            if (!this.Citypopuplayout.IsVisible)
            {
                this.Citypopuplayout.IsVisible = !this.Citypopuplayout.IsVisible;
                this.Citypopuplayout.AnchorX = 1;
                this.Citypopuplayout.AnchorY = 1;

                Animation scaleAnimation = new Animation(
                    f => this.Citypopuplayout.Scale = f,
                    0.5,
                    1,
                    Easing.SinInOut);

                Animation fadeAnimation = new Animation(
                    f => this.Citypopuplayout.Opacity = f,
                    0.2,
                    1,
                    Easing.SinInOut);

                scaleAnimation.Commit(this.Citypopuplayout, "popupScaleAnimation", 250);
                fadeAnimation.Commit(this.Citypopuplayout, "popupFadeAnimation", 250);
            }
            else
            {
                await Task.WhenAny<bool>
                  (
                    this.Citypopuplayout.FadeTo(0, 200, Easing.SinInOut)
                  );

                this.Citypopuplayout.IsVisible = !this.Citypopuplayout.IsVisible;
            }
        }
        private void SelectCity_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            var clicked = e.Item as Repository;
            var selectedItem = (Repository)City_Val.SelectedItem;
            City_Id = Convert.ToInt32(selectedItem.city_id);
            Get_Region(City_Id);
            CityName.Text = selectedItem.name_en;
            City_Val.SelectedItem = null;
            Citypopuplayout.IsVisible = false;
        }
        private void SelectRegion_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            var clicked = e.Item as Repository;
            var selectedItem = (Repository)Region_Val.SelectedItem;
            Region_Id = Convert.ToInt32(selectedItem.region_id);
            Get_Region(Region_Id);
            RegionName.Text = selectedItem.name_en;
            Region_Val.SelectedItem = null;
            Regionpopuplayout.IsVisible = false;
        }

        private async void RegionButton_Clicked(object sender, EventArgs e)
        {
            if (!this.Regionpopuplayout.IsVisible)
            {
                this.Regionpopuplayout.IsVisible = !this.Regionpopuplayout.IsVisible;
                this.Regionpopuplayout.AnchorX = 1;
                this.Regionpopuplayout.AnchorY = 1;

                Animation scaleAnimation = new Animation(
                    f => this.Regionpopuplayout.Scale = f,
                    0.5,
                    1,
                    Easing.SinInOut);

                Animation fadeAnimation = new Animation(
                    f => this.Regionpopuplayout.Opacity = f,
                    0.2,
                    1,
                    Easing.SinInOut);

                scaleAnimation.Commit(this.Regionpopuplayout, "popupScaleAnimation", 250);
                fadeAnimation.Commit(this.Regionpopuplayout, "popupFadeAnimation", 250);
            }
            else
            {
                await Task.WhenAny<bool>
                  (
                    this.Regionpopuplayout.FadeTo(0, 200, Easing.SinInOut)
                  );

                this.Regionpopuplayout.IsVisible = !this.Regionpopuplayout.IsVisible;
            }
        }
        private void Region_val_SelectedIndexChanged(object sender, EventArgs e)
        {
            var selectedItem = (Repository)Region_Val.SelectedItem;

            Region_Id = Convert.ToInt32(selectedItem.region_id);
            //  Application.Current.Properties["fk_Region_id"] = selectedItem.region_id;



        }
        public async void Get_comments()
        {
            int ads_id = Convert.ToInt32(Application.Current.Properties["ads_id"]);
            int sub_service_id = Convert.ToInt32(Application.Current.Properties["sub_service_id"]);

            string API = Constants.GitHubReposEndpoint1 + "get_ads_comment?ads_id=" + ads_id + "&sub_service_Id=" + sub_service_id;
            List<Repository> repositories = await _restService.GetRepositoriesAsync(API);
            collectionView.ItemsSource = repositories;
        }

        private void Stepper_ValueChanged(object sender, ValueChangedEventArgs e)
        {
            int days = Convert.ToInt32(e.NewValue);
            lbldisp.Text = days.ToString();

            double Total_Price = Ads_Price * days;
            Price_Val.Text = Total_Price.ToString();
            End_date = DateTime.Today.AddDays(days);
        }

        public async void Get_Price()
        {
            var client = new HttpClient();

            string API = Constants.GitHubReposEndpoint1 + "get_ads_type_price?main_service_Id=" + Convert.ToInt32(Application.Current.Properties["main_service_id"]) + "&sub_service_Id=" + Convert.ToInt32(Application.Current.Properties["sub_service_id"]) + "&ads_type_id=" + fk_ads_pack_id + "";
            HttpResponseMessage response = await client.GetAsync(API);
            var result = await response.Content.ReadAsStringAsync();

            Repository MPP = new Repository();
            MPP = JsonConvert.DeserializeObject<Repository>(result);

            string[] multiArray = MPP.price.Split(new Char[] { '.' });
            string[] def_price_array = MPP.Default_Price.Split(new Char[] { '.' });
            if (multiArray[1] == "0" || multiArray[1] == "00")
            {
                Ads_Price = Convert.ToDouble(multiArray[0]);
            }
            else
            {
                Ads_Price = Convert.ToDouble(MPP.price);
            }
            if (def_price_array[1] == "0" || def_price_array[1] == "00")
            {
                Ads_Default_Price = Convert.ToInt32(def_price_array[0]);
            }
            else
            {
                Ads_Default_Price = Convert.ToInt32(MPP.Default_Price);
            }
        }
        protected async override void OnAppearing()
        {
            //Write the code of your page here
            base.OnAppearing();
            lbldisp.Text = "1";
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
                if (((App)(App.Current)).Rent_Section_Selected == true)
                {
                    show_rental_period.IsVisible = true;
                }
                else
                {
                    show_rental_period.IsVisible = false;
                }
                ads_status = 1;
                fk_ads_pack_id = 1;
                ads_status = 1;
                allow_comments = 1;
                advertiser_type = 1;
                street_category = 1;
                Parking = 0;
                building_status = 2;
                need_maintenance = 0;
                building_age = 0;
                payment_type = 1;
                number_doors = 0;
                building_area_type = 0;
                land_area_type = 0;
                has_bathroom = 0;
                has_kitchen = 0;
                has_upper_block = 0;
                number_rooms = 0;
                floor_number = 0;
                number_of_floors = 0;
                number_of_offices = 0;
                number_of_shops = 0;
                number_of_Store = 0;
                number_of_appartments = 0;
                number_of_normal_bathroom = 0;
                number_of_master_bathroom = 0;
                number_of_balcones = 0;
                number_of_street = 0;
                has_elevator = 0;
                has_roof = 0;
                has_furnished = 0;
                has_garden = 0;
                has_pool = 0;
                has_entertament = 0;
                has_crops = 0;
                //  Building_Area.Text = "0";
                Area_From.Text = "0";
                Area_To.Text = "0";
                //Price_Val.Text = "0";
                lbldisp.Text = "1";
                string code = Application.Current.Properties["service_code"].ToString();
                if (code == "HA")
                {

                    Show_Door_No.IsVisible = true;
                    number_doors = 1;
                    Show_Land_Area.IsVisible = true;
                    Show_Street_categoury.IsVisible = false;
                    street_category = 0;


                }
                else if (code == "ST")
                {

                    Show_Door_No.IsVisible = false;
                    Complex1.IsVisible = true;
                    show_Number_of_store.IsVisible = true;
                    number_doors = 1;


                }
                else if (code == "SH")
                {
                    Shop.IsVisible = true;
                    Show_Upper_block.IsVisible = true;
                    show_Number_of_shop.IsVisible = true;
                    Show_Bathroom.IsVisible = true;
                    Show_Kitchen.IsVisible = true;
                    number_doors = 1;
                    has_bathroom = 1;
                    has_kitchen = 1;

                }
                else if (code == "OF")
                {
                    Office.IsVisible = true;
                    has_bathroom = 1;
                    has_kitchen = 1;
                    number_rooms = 1;
                    floor_number = 1;
                    Show_Bathroom.IsVisible = true;
                    Show_Kitchen.IsVisible = true;
                    Show_Number_of_room.IsVisible = false;
                    Show_Number_of_office_room.IsVisible = true;
                    Show_Floor_number.IsVisible = true;

                }
                else if (code == "CO")
                {
                    show_Number_of_offices.IsVisible = true;
                    Complex1.IsVisible = true;
                    Show_Land_Area.IsVisible = true;
                    Shop.IsVisible = true;
                    show_Number_of_shop.IsVisible = true;
                    Show_Annual_income.IsVisible = true;
                    Show_Elevator.IsVisible = true;
                    Show_Number_of_floor.IsVisible = true;
                    number_of_floors = 1;
                    number_of_offices = 1;
                    number_of_shops = 1;
                    number_of_Store = 1;

                }
                else if (code == "RE")
                {
                    ResidentIal.IsVisible = true;
                    Show_Land_Area.IsVisible = true;
                    show_Number_of_Appartment.IsVisible = true;
                    Show_Annual_income.IsVisible = true;
                    Show_Size_per_apartment.IsVisible = true;
                    Show_buiding_type.IsVisible = true;
                    Show_Elevator.IsVisible = true;
                    Show_Number_of_floor.IsVisible = true;
                    number_of_floors = 1;
                    number_of_appartments = 1;


                }
                else if (code == "AP")
                {
                    Appartment.IsVisible = true;
                    RoofStack.IsVisible = true;
                    FurnishedStack.IsVisible = true;
                    Show_Street_categoury.IsVisible = false;
                    Show_Number_of_room.IsVisible = true;
                    Show__Number_of_normal_bath_room.IsVisible = true;
                    Show__Number_of_master_bath_room.IsVisible = true;
                    Show_Floor_number.IsVisible = true;
                    Show_Garden.IsVisible = true;
                    Show_balcons.IsVisible = true;
                    street_category = 0;
                    number_rooms = 1;
                    floor_number = 1;
                    number_of_normal_bathroom = 1;
                    number_of_master_bathroom = 1;
                    number_of_balcones = 1;
                    Show_Elevator.IsVisible = true;



                }
                else if (code == "FA")
                {
                    Farm.IsVisible = true;
                    Show_Land_Area.IsVisible = true;
                    Show_Has_building.IsVisible = true;
                    Show_Building_Area.IsVisible = false;
                    PoolStack.IsVisible = true;
                    CropsStack.IsVisible = true;
                    EntertamentStack.IsVisible = true;
                    Show_Building_status.IsVisible = false;
                    Show_Garden.IsVisible = true;
                    //Show_Building_Area.IsVisible = false;
                    //Show_Number_of_room.IsVisible = true;
                    //Show__Number_of_normal_bath_room.IsVisible = true;
                    //Show__Number_of_master_bath_room.IsVisible = true;
                    //number_rooms = 1;
                    //number_of_normal_bathroom = 1;
                    //number_of_master_bathroom = 1;

                }
                else if (code == "VI")
                {
                    Villa.IsVisible = true;
                    Show_Land_Area.IsVisible = true;
                    Show_Existence_services.IsVisible = false;
                    Show_Street_categoury.IsVisible = false;
                    Show_Elevator.IsVisible = true;
                    Show_Number_of_floor.IsVisible = true;
                    Show_Number_of_room.IsVisible = true;
                    Show__Number_of_normal_bath_room.IsVisible = true;
                    Show__Number_of_master_bath_room.IsVisible = true;
                    Show_balcons.IsVisible = true;
                    Show_Garden.IsVisible = true;
                    street_category = 0;
                    number_rooms = 1;
                    number_of_floors = 1;
                    number_of_normal_bathroom = 1;
                    number_of_master_bathroom = 1;

                }
                else if (code == "LA")
                {
                    Get_Organization();
                    //Land.IsVisible = true;
                    Show_Regulation_type.IsVisible = true;
                    show_no_of_street.IsVisible = true;
                    Show_Existence_services.IsVisible = true;
                    Show_maintenace.IsVisible = false;
                    Show_Building_Area.IsVisible = false;

                    Show_Land_Area.IsVisible = true;
                    building_status = 0;
                    number_of_street = 1;
                    land_area_type = 1;
                    land_organization_type = 2;
                    Show_Parking.IsVisible = false;
                    Show_Building_status.IsVisible = false;
                }

                if (Application.Current.Properties["Operation_Type"] == "I")
                {

                    End_date = DateTime.Today.AddDays(100);

                }
                else if (Application.Current.Properties["Operation_Type"] == "U")
                {
                    int ads_id = Convert.ToInt32(Application.Current.Properties["ads_id"]);
                    Get_Ads_Details(ads_id);
                }

                string Lang = await SecureStorage.GetAsync("Lang");
                if (Lang == "Ar")
                {
                    Scroll_Lang.FlowDirection = FlowDirection.RightToLeft;
                }
                else
                {
                    //  Lang_title.Text = "Language";

                    Scroll_Lang.FlowDirection = FlowDirection.LeftToRight;
                }
                //  Get_comments();

            }
            catch (Exception EX)
            {
                await DisplayAlert("Alert", "Please Login to continue the process", "OK");
            }
        }
        public async void Get_Ads_Details(int ads_id)
        {
            try
            {
                List<Ads_General_Details__Form_Property> repositories = null;
                var client = new HttpClient();
                //  int main_service_id = Convert.ToInt32(Application.Current.Properties["main_service_id"]);
                string API = Constants.GitHubReposEndpoint1 + "get_ads_general_on_form?ads_id=" + Convert.ToInt32(Application.Current.Properties["ads_id"]) + "&register_id=" + Convert.ToInt32(Application.Current.Properties["register_id"]) + "";
                HttpResponseMessage response = await client.GetAsync(API);
                var result = await response.Content.ReadAsStringAsync();
                if (response.IsSuccessStatusCode)
                {
                    repositories = JsonConvert.DeserializeObject<List<Ads_General_Details__Form_Property>>(result);
                    if (repositories.Count > 0)
                    {
                        Title.Text = repositories[0].title;
                        land_area_val.Text = repositories[0].land_area_aalue.ToString();
                        Building_Area.Text = repositories[0].building_area_fixed_value.ToString();
                        Area_From.Text = repositories[0].building_area_from_value.ToString();
                        Area_To.Text = repositories[0].building_area_to_value.ToString();
                        Price.Text = repositories[0].price.ToString();
                        Annual_income.Text = repositories[0].annual_income.ToString();
                        Size_per_apartment.Text = repositories[0].Size_per_apartment.ToString();
                        ads_description.Text = repositories[0].ads_description.ToString();
                        rdp_maintenance_1.IsChecked = Convert.ToBoolean(repositories[0].need_maintenance);

                        #region Rdbland_area_type
                        int Rdbland_area_type = Convert.ToInt32(repositories[0].land_area_type);
                        if (Rdbland_area_type == 1)
                        {
                            //Rdbland_area_type_meter.IsChecked = true;
                            //Rdbland_area_type_acres.IsChecked = false;
                            //LandAreaTxtName.Text = "m2";
                        }
                        else if (Rdbland_area_type == 2)
                        {
                            //Rdbland_area_type_meter.IsChecked = false;
                            //Rdbland_area_type_acres.IsChecked = true;
                            //LandAreaTxtName.Text = "Acr";

                        }

                        #endregion


                        #region Rdp_Buiding_type
                        int RDBuiding_type = Convert.ToInt32(repositories[0].building_area_type);
                        if (RDBuiding_type == 1)
                        {
                            rdp_Buiding_type_1.IsChecked = true;
                            rdp_Buiding_type_2.IsChecked = false;
                            rdp_Buiding_type_3.IsChecked = false;
                        }
                        else if (RDBuiding_type == 2)
                        {
                            rdp_Buiding_type_1.IsChecked = false;
                            rdp_Buiding_type_2.IsChecked = true;
                            rdp_Buiding_type_3.IsChecked = false;
                        }
                        else if (RDBuiding_type == 3)
                        {
                            rdp_Buiding_type_1.IsChecked = false;
                            rdp_Buiding_type_2.IsChecked = false;
                            rdp_Buiding_type_3.IsChecked = true;
                        }

                        #endregion

                        #region Rdp_rdp_Kitchen
                        int rdp_Kitchen = Convert.ToInt32(repositories[0].has_kitchen);
                        if (rdp_Kitchen == 1)
                        {
                            rdp_Kitchen_switch.IsToggled = true;

                        }
                        else if (rdp_Kitchen == 0)
                        {
                            rdp_Kitchen_switch.IsToggled = false;

                        }


                        #endregion

                        #region Rdp_rdp_Bathroom


                        int rdp_Bathroom = Convert.ToInt32(repositories[0].has_bathroom);
                        if (rdp_Bathroom == 1)
                        {
                            rdp_Kitchen_switch.IsToggled = true;

                        }
                        else if (rdp_Kitchen == 0)
                        {
                            rdp_Kitchen_switch.IsToggled = false;

                        }
                        #endregion

                        #region Rdb_Floornumber
                        int Rdb_Floornumber = Convert.ToInt32(repositories[0].floor_number);
                        if (Rdb_Floornumber == 1)
                        {
                            Rdb_Floornumber_1.IsChecked = true;
                            Rdb_Floornumber_2.IsChecked = false;
                            Rdb_Floornumber_3.IsChecked = false;
                            Rdb_Floornumber_4.IsChecked = false;

                        }
                        else if (Rdb_Floornumber == 2)
                        {
                            Rdb_Floornumber_1.IsChecked = false;
                            Rdb_Floornumber_2.IsChecked = true;
                            Rdb_Floornumber_3.IsChecked = false;
                            Rdb_Floornumber_4.IsChecked = false;

                        }
                        else if (Rdb_Floornumber == 3)
                        {
                            Rdb_Floornumber_1.IsChecked = false;
                            Rdb_Floornumber_2.IsChecked = false;
                            Rdb_Floornumber_3.IsChecked = true;
                            Rdb_Floornumber_4.IsChecked = false;

                        }
                        else if (Rdb_Floornumber == 4)
                        {
                            Rdb_Floornumber_1.IsChecked = false;
                            Rdb_Floornumber_2.IsChecked = false;
                            Rdb_Floornumber_3.IsChecked = false;
                            Rdb_Floornumber_4.IsChecked = true;

                        }
                        #endregion

                        #region Rdb_Roomnumber
                        int Rdb_Roomnumber = Convert.ToInt32(repositories[0].number_rooms);
                        if (Rdb_Roomnumber == 1)
                        {
                            Rdb_roomnumber_1.IsChecked = true;
                            Rdb_roomnumber_2.IsChecked = false;
                            Rdb_roomnumber_3.IsChecked = false;
                            Rdb_roomnumber_4.IsChecked = false;

                        }
                        else if (Rdb_Roomnumber == 2)
                        {
                            Rdb_roomnumber_1.IsChecked = false;
                            Rdb_roomnumber_2.IsChecked = true;
                            Rdb_roomnumber_3.IsChecked = false;
                            Rdb_roomnumber_4.IsChecked = false;

                        }
                        else if (Rdb_Roomnumber == 3)
                        {
                            Rdb_roomnumber_1.IsChecked = false;
                            Rdb_roomnumber_2.IsChecked = false;
                            Rdb_roomnumber_3.IsChecked = true;
                            Rdb_roomnumber_4.IsChecked = false;

                        }
                        else if (Rdb_Roomnumber == 4)
                        {
                            Rdb_roomnumber_1.IsChecked = false;
                            Rdb_roomnumber_2.IsChecked = false;
                            Rdb_roomnumber_3.IsChecked = false;
                            Rdb_roomnumber_4.IsChecked = true;

                        }
                        #endregion


                        #region Rdp_Building_status
                        int Rdp_Building_status = Convert.ToInt32(repositories[0].Building_Condition);
                        if (Rdp_Building_status == 1)
                        {
                            rdp_Old_1.IsChecked = true;
                            rdp_Old_2.IsChecked = false;
                        }
                        else if (Rdp_Building_status == 2)
                        {
                            rdp_Old_1.IsChecked = false;
                            rdp_Old_2.IsChecked = true;
                        }
                        #endregion

                        #region rdb_parking
                        int rdb_parking = Convert.ToInt32(repositories[0].parking);
                        if (rdb_parking == 1)
                        {
                            rdb_parking_switch.IsToggled = true;

                        }
                        else if (rdb_parking == 0)
                        {
                            rdb_parking_switch.IsToggled = false;

                        }
                        #endregion

                        #region Payment_Type
                        int Payment_Type = Convert.ToInt32(repositories[0].payment_type);
                        if (Payment_Type == 1)
                        {
                            rdb_Payment_Type_1.IsChecked = true;
                            rdb_Payment_Type_2.IsChecked = false;

                        }
                        else if (Payment_Type == 2)
                        {
                            rdb_Payment_Type_1.IsChecked = false;
                            rdb_Payment_Type_2.IsChecked = true;

                        }
                        #endregion



                        #region Street_categoury
                        int rdb_Street_categoury = Convert.ToInt32(repositories[0].street_category);
                        if (rdb_Street_categoury == 1)
                        {
                            rdp_Main_1.IsChecked = true;
                            rdp_Sub_0.IsChecked = false;

                        }
                        else if (rdb_Street_categoury == 0)
                        {
                            rdp_Main_1.IsChecked = false;
                            rdp_Sub_0.IsChecked = true;

                        }
                        #endregion

                        #region Upper_block
                        int rdp_Upper_block = Convert.ToInt32(repositories[0].has_upper_block);
                        if (rdp_Upper_block == 1)
                        {
                            rdp_Upper_block_switch.IsToggled = true;

                        }
                        else if (rdp_Upper_block == 0)
                        {
                            rdp_Upper_block_switch.IsToggled = false;

                        }
                        #endregion

                        #region Ads_status
                        int Ads_status = Convert.ToInt32(repositories[0].ads_status);
                        if (Ads_status == 1)
                        {
                            rdb_Active_switch.IsToggled = true;

                        }
                        else if (Ads_status == 0)
                        {
                            rdb_Active_switch.IsToggled = false;
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

                        #region Advertiser_Type
                        int RdbAdvertiser_Type = Convert.ToInt32(repositories[0].advertiser_type);
                        if (RdbAdvertiser_Type == 1)
                        {
                            //rdb_mediator_1.IsChecked = true;
                            //rdb_Owner_2.IsChecked = false;
                            //rdb_company_3.IsChecked = false;
                        }
                        else if (RdbAdvertiser_Type == 2)
                        {
                            //rdb_mediator_1.IsChecked = false;
                            //rdb_Owner_2.IsChecked = true;
                            //rdb_company_3.IsChecked = false;
                        }
                        else if (RdbAdvertiser_Type == 3)
                        {
                            //rdb_mediator_1.IsChecked = false;
                            //rdb_Owner_2.IsChecked = false;
                            //rdb_company_3.IsChecked = true;
                        }

                        #endregion

                        #region Ads_Pack_Type
                        int Rdb_Ads_Pack_Type = Convert.ToInt32(repositories[0].fk_ads_pack_id);
                        if (Rdb_Ads_Pack_Type == 1)
                        {
                            //rdb_Ads_Type_1.IsChecked = true;
                            //rdb_Ads_Type_2.IsChecked = false;
                            //rdb_Ads_Type_3.IsChecked = false;
                            //rdb_Ads_Type_4.IsChecked = false;
                        }
                        else if (Rdb_Ads_Pack_Type == 2)
                        {
                            //rdb_Ads_Type_1.IsChecked = false;
                            //rdb_Ads_Type_2.IsChecked = true;
                            //rdb_Ads_Type_3.IsChecked = false;
                            //rdb_Ads_Type_4.IsChecked = false;
                        }
                        else if (Rdb_Ads_Pack_Type == 3)
                        {
                            //rdb_Ads_Type_1.IsChecked = false;
                            //rdb_Ads_Type_2.IsChecked = false;
                            //rdb_Ads_Type_3.IsChecked = true;
                            //rdb_Ads_Type_4.IsChecked = false;
                        }
                        else if (Rdb_Ads_Pack_Type == 4)
                        {
                            //rdb_Ads_Type_1.IsChecked = false;
                            //rdb_Ads_Type_2.IsChecked = false;
                            //rdb_Ads_Type_3.IsChecked = false;
                            //rdb_Ads_Type_4.IsChecked = true;
                        }
                        #endregion

                        #region Building_Age
                        int Rdb_Building_Age = Convert.ToInt32(repositories[0].building_age);
                        if (Rdb_Building_Age == 1)
                        {
                            rdp_Building_Age_1.IsChecked = true;
                            rdp_Building_Age_2.IsChecked = false;
                            Rdb_Floornumber_3.IsChecked = false;
                            rdp_Building_Age_4.IsChecked = false;
                        }
                        else if (Rdb_Building_Age == 2)
                        {
                            rdp_Building_Age_1.IsChecked = false;
                            rdp_Building_Age_2.IsChecked = true;
                            rdp_Building_Age_3.IsChecked = false;
                            rdp_Building_Age_4.IsChecked = false;
                        }
                        else if (Rdb_Building_Age == 3)
                        {
                            rdp_Building_Age_1.IsChecked = false;
                            rdp_Building_Age_2.IsChecked = false;
                            rdp_Building_Age_3.IsChecked = true;
                            rdp_Building_Age_4.IsChecked = false;
                        }
                        else if (Rdb_Building_Age == 4)
                        {
                            rdp_Building_Age_1.IsChecked = false;
                            rdp_Building_Age_2.IsChecked = false;
                            rdp_Building_Age_3.IsChecked = false;
                            rdp_Building_Age_4.IsChecked = true;
                        }
                        #endregion



                        #region Door_No
                        int Rdb_Door_No = Convert.ToInt32(repositories[0].number_doors);
                        if (Rdb_Door_No == 1)
                        {
                            rdp_door_1.IsChecked = true;
                            rdp_door_2.IsChecked = false;
                            rdp_door_3.IsChecked = false;
                            rdp_door_4.IsChecked = false;
                        }
                        else if (Rdb_Door_No == 2)
                        {
                            rdp_door_1.IsChecked = false;
                            rdp_door_2.IsChecked = true;
                            rdp_door_3.IsChecked = false;
                            rdp_door_4.IsChecked = false;
                        }
                        else if (Rdb_Door_No == 3)
                        {
                            rdp_door_1.IsChecked = false;
                            rdp_door_2.IsChecked = false;
                            rdp_door_3.IsChecked = true;
                            rdp_door_4.IsChecked = false;
                        }
                        else if (Rdb_Door_No == 4)
                        {
                            rdp_door_1.IsChecked = false;
                            rdp_door_2.IsChecked = false;
                            rdp_door_3.IsChecked = false;
                            rdp_door_4.IsChecked = true;
                        }
                        #endregion

                        #region Number_of_floor
                        int number_of_Floor = Convert.ToInt32(repositories[0].number_of_floors);
                        if (number_of_Floor == 1)
                        {
                            Rdb_number_of_Floor_1.IsChecked = true;
                            Rdb_number_of_Floor_2.IsChecked = false;
                            Rdb_number_of_Floor_3.IsChecked = false;
                            Rdb_number_of_Floor_4.IsChecked = false;
                        }
                        else if (number_of_Floor == 2)
                        {
                            Rdb_number_of_Floor_1.IsChecked = false;
                            Rdb_number_of_Floor_2.IsChecked = true;
                            Rdb_number_of_Floor_3.IsChecked = false;
                            Rdb_number_of_Floor_4.IsChecked = false;
                        }
                        else if (number_of_Floor == 3)
                        {
                            Rdb_number_of_Floor_1.IsChecked = false;
                            Rdb_number_of_Floor_2.IsChecked = false;
                            Rdb_number_of_Floor_3.IsChecked = true;
                            Rdb_number_of_Floor_4.IsChecked = false;
                        }
                        else if (number_of_Floor == 4)
                        {
                            Rdb_number_of_Floor_1.IsChecked = false;
                            Rdb_number_of_Floor_2.IsChecked = false;
                            Rdb_number_of_Floor_3.IsChecked = false;
                            Rdb_number_of_Floor_4.IsChecked = true;
                        }
                        #endregion

                        #region Number_of_offices
                        int Number_of_offices = Convert.ToInt32(repositories[0].number_of_offices);
                        if (Number_of_offices == 1)
                        {
                            Rdb_number_of_offices_1.IsChecked = true;
                            Rdb_number_of_offices_2.IsChecked = false;
                            Rdb_number_of_offices_3.IsChecked = false;
                            Rdb_number_of_offices_4.IsChecked = false;
                        }
                        else if (Number_of_offices == 2)
                        {
                            Rdb_number_of_offices_1.IsChecked = false;
                            Rdb_number_of_offices_2.IsChecked = true;
                            Rdb_number_of_offices_3.IsChecked = false;
                            Rdb_number_of_offices_4.IsChecked = false;
                        }
                        else if (Number_of_offices == 3)
                        {
                            Rdb_number_of_offices_1.IsChecked = false;
                            Rdb_number_of_offices_2.IsChecked = false;
                            Rdb_number_of_offices_3.IsChecked = true;
                            Rdb_number_of_offices_4.IsChecked = false;
                        }
                        else if (Number_of_offices == 4)
                        {
                            Rdb_number_of_offices_1.IsChecked = false;
                            Rdb_number_of_offices_2.IsChecked = false;
                            Rdb_number_of_offices_3.IsChecked = false;
                            Rdb_number_of_offices_4.IsChecked = true;
                        }
                        #endregion

                        #region Number_of_shop
                        int Number_of_shop = Convert.ToInt32(repositories[0].number_of_shops);
                        if (Number_of_shop == 1)
                        {
                            Rdb_number_of_shop_1.IsChecked = true;
                            Rdb_number_of_shop_2.IsChecked = false;
                            Rdb_number_of_shop_3.IsChecked = false;
                            Rdb_number_of_shop_4.IsChecked = false;
                        }
                        else if (Number_of_shop == 2)
                        {
                            Rdb_number_of_shop_1.IsChecked = false;
                            Rdb_number_of_shop_2.IsChecked = true;
                            Rdb_number_of_shop_3.IsChecked = false;
                            Rdb_number_of_shop_4.IsChecked = false;
                        }
                        else if (Number_of_shop == 3)
                        {
                            Rdb_number_of_shop_1.IsChecked = false;
                            Rdb_number_of_shop_2.IsChecked = false;
                            Rdb_number_of_shop_3.IsChecked = true;
                            Rdb_number_of_shop_4.IsChecked = false;
                        }
                        else if (Number_of_shop == 4)
                        {
                            Rdb_number_of_shop_1.IsChecked = false;
                            Rdb_number_of_shop_2.IsChecked = false;
                            Rdb_number_of_shop_3.IsChecked = false;
                            Rdb_number_of_shop_4.IsChecked = true;
                        }
                        #endregion

                        #region Number_of_store
                        int Number_of_store = Convert.ToInt32(repositories[0].number_of_store);
                        if (Number_of_store == 1)
                        {
                            Rdb_number_of_store_1.IsChecked = true;
                            Rdb_number_of_store_2.IsChecked = false;
                            Rdb_number_of_store_3.IsChecked = false;
                            Rdb_number_of_store_4.IsChecked = false;
                        }
                        else if (Number_of_store == 2)
                        {
                            Rdb_number_of_store_1.IsChecked = false;
                            Rdb_number_of_store_2.IsChecked = true;
                            Rdb_number_of_store_3.IsChecked = false;
                            Rdb_number_of_store_4.IsChecked = false;
                        }
                        else if (Number_of_store == 3)
                        {
                            Rdb_number_of_store_1.IsChecked = false;
                            Rdb_number_of_store_2.IsChecked = false;
                            Rdb_number_of_store_3.IsChecked = true;
                            Rdb_number_of_store_4.IsChecked = false;
                        }
                        else if (Number_of_store == 4)
                        {
                            Rdb_number_of_store_1.IsChecked = false;
                            Rdb_number_of_store_2.IsChecked = false;
                            Rdb_number_of_store_3.IsChecked = false;
                            Rdb_number_of_store_4.IsChecked = true;
                        }
                        #endregion

                        #region Elevator
                        int rdp_Elevator = Convert.ToInt32(repositories[0].has_elevator);
                        if (rdp_Elevator == 1)
                        {
                            Rdb_Elevator_switch.IsToggled = true;

                        }
                        else if (rdp_Elevator == 0)
                        {
                            Rdb_Elevator_switch.IsToggled = false;

                        }

                        #endregion

                        #region Existence_of_services
                        int rdp_Existence_of_services = Convert.ToInt32(repositories[0].existence_of_services);
                        if (rdp_Existence_of_services == 1)
                        {
                            Rdb_Existence_of_services_switch.IsToggled = true;

                        }
                        else if (rdp_Existence_of_services == 0)
                        {
                            Rdb_Existence_of_services_switch.IsToggled = false;

                        }

                        #endregion

                        #region Organization type
                        int rdp_Organization_type = Convert.ToInt32(repositories[0].land_organization_type);
                        if (rdp_Organization_type == 1)
                        {
                            Rdb_Organization_type_1.IsChecked = true;
                            Rdb_Organization_type_2.IsChecked = false;

                        }
                        else if (rdp_Organization_type == 2)
                        {
                            Rdb_Organization_type_1.IsChecked = false;
                            Rdb_Organization_type_2.IsChecked = true;

                        }

                        #endregion

                        #region Number_of_Street
                        int Rdb_Number_of_Street = Convert.ToInt32(repositories[0].number_of_street);
                        if (Rdb_Number_of_Street == 1)
                        {
                            Rdb_Number_of_Street_1.IsChecked = true;
                            Rdb_Number_of_Street_2.IsChecked = false;
                            Rdb_Number_of_Street_3.IsChecked = false;
                            Rdb_Number_of_Street_4.IsChecked = false;
                        }
                        else if (Rdb_Number_of_Street == 2)
                        {
                            Rdb_Number_of_Street_1.IsChecked = false;
                            Rdb_Number_of_Street_2.IsChecked = true;
                            Rdb_Number_of_Street_3.IsChecked = false;
                            Rdb_Number_of_Street_4.IsChecked = false;
                        }
                        else if (Rdb_Number_of_Street == 3)
                        {
                            Rdb_Number_of_Street_1.IsChecked = false;
                            Rdb_Number_of_Street_2.IsChecked = false;
                            Rdb_Number_of_Street_3.IsChecked = true;
                            Rdb_Number_of_Street_4.IsChecked = false;
                        }
                        else if (Rdb_Number_of_Street == 4)
                        {
                            Rdb_Number_of_Street_1.IsChecked = false;
                            Rdb_Number_of_Street_2.IsChecked = false;
                            Rdb_Number_of_Street_3.IsChecked = false;
                            Rdb_Number_of_Street_4.IsChecked = true;
                        }
                        #endregion

                        #region balcons
                        int Rdb_balcons = Convert.ToInt32(repositories[0].number_of_balcones);
                        if (Rdb_balcons == 1)
                        {
                            rdb_Number_of_balcons_1.IsChecked = true;
                            rdb_Number_of_balcons_2.IsChecked = false;
                            rdb_Number_of_balcons_3.IsChecked = false;
                            rdb_Number_of_balcons_4.IsChecked = false;
                        }
                        else if (Rdb_balcons == 2)
                        {
                            rdb_Number_of_balcons_1.IsChecked = false;
                            rdb_Number_of_balcons_2.IsChecked = true;
                            rdb_Number_of_balcons_3.IsChecked = false;
                            rdb_Number_of_balcons_4.IsChecked = false;
                        }
                        else if (Rdb_balcons == 3)
                        {
                            rdb_Number_of_balcons_1.IsChecked = false;
                            rdb_Number_of_balcons_2.IsChecked = false;
                            rdb_Number_of_balcons_3.IsChecked = true;
                            rdb_Number_of_balcons_4.IsChecked = false;
                        }
                        else if (Rdb_balcons == 4)
                        {
                            rdb_Number_of_balcons_1.IsChecked = false;
                            rdb_Number_of_balcons_2.IsChecked = false;
                            rdb_Number_of_balcons_3.IsChecked = false;
                            rdb_Number_of_balcons_4.IsChecked = true;
                        }
                        #endregion



                        #region Switches 
                        Switch_Garden.IsToggled = Convert.ToBoolean(repositories[0].has_garden);
                        Switch_Furnished.IsToggled = Convert.ToBoolean(repositories[0].has_furnished);
                        Switch_Roof.IsToggled = Convert.ToBoolean(repositories[0].has_roof);
                        Crops.IsToggled = Convert.ToBoolean(repositories[0].has_crops);
                        Entertament.IsToggled = Convert.ToBoolean(repositories[0].has_entertament);
                        Pool.IsToggled = Convert.ToBoolean(repositories[0].has_Pool);
                        #endregion




                        #region Number of Appartment
                        int Rdb_Number_of_Appartment = Convert.ToInt32(repositories[0].number_of_Appartments);
                        if (Rdb_Number_of_Appartment == 1)
                        {
                            Rdb_Appartment_1.IsChecked = true;
                            Rdb_Appartment_2.IsChecked = false;
                            Rdb_Appartment_3.IsChecked = false;
                            Rdb_Appartment_4.IsChecked = false;
                        }
                        else if (Rdb_Number_of_Appartment == 2)
                        {
                            Rdb_Appartment_1.IsChecked = false;
                            Rdb_Appartment_2.IsChecked = true;
                            Rdb_Appartment_3.IsChecked = false;
                            Rdb_Appartment_4.IsChecked = false;
                        }
                        else if (Rdb_Number_of_Appartment == 3)
                        {
                            Rdb_Appartment_1.IsChecked = false;
                            Rdb_Appartment_2.IsChecked = false;
                            Rdb_Appartment_3.IsChecked = true;
                            Rdb_Appartment_4.IsChecked = false;
                        }
                        else if (Rdb_Number_of_Appartment == 4)
                        {
                            Rdb_Appartment_1.IsChecked = false;
                            Rdb_Appartment_2.IsChecked = false;
                            Rdb_Appartment_3.IsChecked = false;
                            Rdb_Appartment_4.IsChecked = true;
                        }
                        #endregion

                        #region Number_of_normal_bath_room
                        int Rdb_Number_of_normal_bath_room = Convert.ToInt32(repositories[0].number_of_normal_bathroom);
                        if (Rdb_Number_of_normal_bath_room == 1)
                        {
                            rdb_Number_of_normal_bath_room_1.IsChecked = true;
                            rdb_Number_of_normal_bath_room_2.IsChecked = false;
                            rdb_Number_of_normal_bath_room_3.IsChecked = false;
                            rdb_Number_of_normal_bath_room_4.IsChecked = false;
                        }
                        else if (Rdb_Number_of_normal_bath_room == 2)
                        {
                            rdb_Number_of_normal_bath_room_1.IsChecked = false;
                            rdb_Number_of_normal_bath_room_2.IsChecked = true;
                            rdb_Number_of_normal_bath_room_3.IsChecked = false;
                            rdb_Number_of_normal_bath_room_4.IsChecked = false;
                        }
                        else if (Rdb_Number_of_normal_bath_room == 3)
                        {
                            rdb_Number_of_normal_bath_room_1.IsChecked = false;
                            rdb_Number_of_normal_bath_room_2.IsChecked = false;
                            rdb_Number_of_normal_bath_room_3.IsChecked = true;
                            rdb_Number_of_normal_bath_room_4.IsChecked = false;
                        }
                        else if (Rdb_Number_of_normal_bath_room == 4)
                        {
                            rdb_Number_of_normal_bath_room_1.IsChecked = false;
                            rdb_Number_of_normal_bath_room_2.IsChecked = false;
                            rdb_Number_of_normal_bath_room_3.IsChecked = false;
                            rdb_Number_of_normal_bath_room_4.IsChecked = true;
                        }
                        #endregion

                        #region Number_of_master_bath_room
                        int Number_of_master_bath_room = Convert.ToInt32(repositories[0].number_of_master_bathroom);
                        if (Number_of_master_bath_room == 1)
                        {
                            rdb_Number_of_master_bath_room_1.IsChecked = true;
                            rdb_Number_of_master_bath_room_2.IsChecked = false;
                            rdb_Number_of_master_bath_room_3.IsChecked = false;
                            rdb_Number_of_master_bath_room_4.IsChecked = false;
                        }
                        else if (Number_of_master_bath_room == 2)
                        {
                            rdb_Number_of_master_bath_room_1.IsChecked = false;
                            rdb_Number_of_master_bath_room_2.IsChecked = true;
                            rdb_Number_of_master_bath_room_3.IsChecked = false;
                            rdb_Number_of_master_bath_room_4.IsChecked = false;
                        }
                        else if (Number_of_master_bath_room == 3)
                        {
                            rdb_Number_of_normal_bath_room_1.IsChecked = false;
                            rdb_Number_of_master_bath_room_2.IsChecked = false;
                            rdb_Number_of_master_bath_room_3.IsChecked = true;
                            rdb_Number_of_master_bath_room_4.IsChecked = false;
                        }
                        else if (Number_of_master_bath_room == 4)
                        {
                            rdb_Number_of_normal_bath_room_1.IsChecked = false;
                            rdb_Number_of_master_bath_room_2.IsChecked = false;
                            rdb_Number_of_master_bath_room_3.IsChecked = false;
                            rdb_Number_of_master_bath_room_4.IsChecked = true;
                        }
                        #endregion


                        City_Val.SelectedItem = repositories[0].fk_city_id;

                        //City_Val.ItemDisplayBinding = x;
                        //City_Val.SelectedIndex = -1;

                        Get_Region(repositories[0].fk_city_id);
                        // Region_val.SelectedItem = repositories[0].fk_region_id;
                        //  Region_val.SelectedIndex =0;
                    }

                }
            }
            catch (Exception ex)
            {
                await DisplayAlert("Alert", ex.Message, "OK");
            }

        }
        public async Task<List<Repository>> Ads_Insert_Update(string url)
        {
            List<Repository> repositories = null;
            var result = "";
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

                Adp.fk_main_service = Convert.ToInt32(Application.Current.Properties["main_service_id"]);
                Adp.fk_sub_service = Convert.ToInt32(Application.Current.Properties["sub_service_id"]);
                Adp.fk_city_id = City_Id;
                Adp.fk_region_id = Region_Id;
                Adp.FK_Register_Id = Convert.ToInt32(Application.Current.Properties["register_id"]);
                Adp.fk_ads_pack_id = fk_ads_pack_id;
                Adp.ads_description = ads_description.Text;
                Adp.ads_status = ads_status;
                Adp.allow_comments = allow_comments;
                Adp.start_date = DateTime.Today;
                Adp.end_date = End_date;
                Adp.image = "";
                Adp.logo = "";
                Adp.latitude = latitude;
                Adp.longitude = longitude;
                Adp.street_category = street_category;
                Adp.parking = Parking;
                Adp.Building_Condition = building_status;
                Adp.building_age = building_age;
                Adp.need_maintenance = need_maintenance;
                Adp.advertiser_type = advertiser_type;
                Adp.price = Convert.ToInt32(Price.Text);
                Adp.payment_type = payment_type;
                Adp.number_doors = number_doors;
                Adp.building_area_type = building_area_type;
                Adp.building_area_fixed_value = Convert.ToInt32(Building_Area.Text);
                Adp.building_area_from_value = Convert.ToInt32(Area_From.Text);
                Adp.building_area_to_value = Convert.ToInt32(Area_To.Text);
                Adp.land_area_aalue = Convert.ToInt32(land_area_val.Text);
                Adp.land_area_type = land_area_type;
                Adp.has_bathroom = has_bathroom;
                Adp.has_kitchen = has_kitchen;
                Adp.has_upper_block = has_upper_block;
                Adp.number_rooms = number_rooms;
                Adp.floor_number = floor_number;
                Adp.number_of_floors = number_of_floors;
                Adp.number_of_offices = number_of_offices;
                Adp.number_of_shops = number_of_shops;
                Adp.number_of_store = number_of_Store;
                Adp.number_of_Appartments = number_of_appartments;
                Adp.number_of_normal_bathroom = number_of_normal_bathroom;
                Adp.number_of_master_bathroom = number_of_master_bathroom;
                Adp.number_of_balcones = number_of_balcones;
                Adp.number_of_street = number_of_street;
                Adp.annual_income = Annual_income.Text;
                Adp.Size_per_apartment = Size_per_apartment.Text;
                Adp.has_elevator = has_elevator;
                Adp.has_roof = has_roof;
                Adp.has_furnished = has_furnished;
                Adp.has_garden = has_garden;
                Adp.has_Pool = has_pool;
                Adp.has_entertament = has_entertament;
                Adp.has_crops = has_crops;
                Adp.land_organization_type = land_organization_type;
                Adp.land_organization_name = land_organization_name;
                Adp.existence_of_services = existence_of_services;
                Adp.title = Title.Text;




                string jsonData = JsonConvert.SerializeObject(Adp);

                var content = new StringContent(jsonData, Encoding.UTF8, "application/json");
                HttpResponseMessage response = await client.PostAsync(url, content);

                result = await response.Content.ReadAsStringAsync();

                repositories = JsonConvert.DeserializeObject<List<Repository>>(result);

                if (repositories[0].result == "success")
                {
                    ads_id = Convert.ToInt32(repositories[0].ads_id);
                    string API = Constants.GitHubReposEndpoint1 + "decoration/saveadsphotodata";
                    Save_Photos_Data(API);
                    await DisplayAlert("Alert", repositories[0].result, "OK");
                    // await Navigation.PushAsync(new My_AdsPage());
                    await Navigation.PopToRootAsync();

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
        private void Has_building_CheckedChanged(object sender, CheckedChangedEventArgs e)
        {
            RadioButton button = sender as RadioButton;
            //Adp.allow_comments = Convert.ToInt32(button.Value);
            has_building = Convert.ToInt32(button.Value);
            if (has_building == 1)
            {
                Show_Has_building.IsVisible = true;
                Show_Farm_Building_Area.IsVisible = true;
                Show_Building_status.IsVisible = true;
                Show_Number_of_room.IsVisible = true;
                Show__Number_of_normal_bath_room.IsVisible = true;
                Show__Number_of_master_bath_room.IsVisible = true;
            }
            else
            {
                Show_Has_building.IsVisible = false;
                Show_Farm_Building_Area.IsVisible = false;
                Show_Building_status.IsVisible = false;
                Show_maintenace.IsVisible = false;
                Show_Number_of_room.IsVisible = false;
                Show__Number_of_normal_bath_room.IsVisible = false;
                Show__Number_of_master_bath_room.IsVisible = false;
                need_maintenance = 0;
                building_age = 0;
            }

        }
        private void Organization_Type_CheckedChanged(object sender, CheckedChangedEventArgs e)
        {
            RadioButton button = sender as RadioButton;
            //Adp.allow_comments = Convert.ToInt32(button.Value);
        }
        private void land_area_type_CheckedChanged(object sender, CheckedChangedEventArgs e)
        {
            RadioButton button = sender as RadioButton;
            //Adp.allow_comments = Convert.ToInt32(button.Value);
            land_area_type = Convert.ToInt32(button.Value);
            //if (Rdbland_area_type_meter.IsChecked)
            //{
            //    LandAreaTxtName.Text = "m2";
            //    Rdbland_area_type_acres.IsChecked = false;
            //}
            //if (Rdbland_area_type_acres.IsChecked)
            //{
            //    LandAreaTxtName.Text = "Acr";
            //    Rdbland_area_type_meter.IsChecked = false;
            //}
        }
        private void Payment_Type(object sender, CheckedChangedEventArgs e)
        {
            RadioButton button = sender as RadioButton;
            //Adp.allow_comments = Convert.ToInt32(button.Value);
            payment_type = Convert.ToInt32(button.Value);
        }

        //private void Rdb_Elevator_OnToggled(object sender, CheckedChangedEventArgs e)
        //{
        //    RadioButton button = sender as RadioButton;
        //    has_elevator = Convert.ToInt32(button.Value);
        //}
        void Rdb_Elevator_OnToggled(object sender, ToggledEventArgs e)
        {
            has_elevator = Convert.ToInt32(Rdb_Elevator_switch.IsToggled);
        }
        private void rdp_Upper_block_OnToggled(object sender, CheckedChangedEventArgs e)
        {
            RadioButton button = sender as RadioButton;
            has_upper_block = Convert.ToInt32(button.Value);
        }
        private void buiding_type_CheckedChanged(object sender, CheckedChangedEventArgs e)
        {
            RadioButton button = sender as RadioButton;
            //Adp.allow_comments = Convert.ToInt32(button.Value);
            building_area_type = Convert.ToInt32(button.Value);

            if (building_area_type == 3)
            {
                Show_Building_Area_From_To.IsVisible = true;
                Show_Size_per_apartment.IsVisible = false;
                Show_Building_Area.IsVisible = false;
                Building_Area.Text = "0";
            }
            else
            {
                Show_Building_Area_From_To.IsVisible = false;
                Show_Building_Area.IsVisible = true;
                Show_Size_per_apartment.IsVisible = true;
                Area_From.Text = "0";
                Area_To.Text = "0";
            }
        }

        private void Payment_Type2_CheckedChanged(object sender, CheckedChangedEventArgs e)
        {
            RadioButton button = sender as RadioButton;
            //Adp.allow_comments = Convert.ToInt32(button.Value);
            payment_type = Convert.ToInt32(button.Value);
        }

        private void Building_status_CheckedChanged(object sender, CheckedChangedEventArgs e)
        {
            RadioButton button = sender as RadioButton;
            building_status = Convert.ToInt32(button.Value);
            if (building_status == 1)
            {
                Show_maintenace.IsVisible = true;
            }
            else
            {
                Show_maintenace.IsVisible = false;
                need_maintenance = 0;
                building_age = 0;
            }
        }

        private void Old_Building_status_CheckedChanged(object sender, EventArgs e)
        {
            building_status = 1;
            Show_maintenace.IsVisible = true;
        }
        private void New_Building_status_CheckedChanged(object sender, EventArgs e)
        {
            building_status = 0;
            Show_maintenace.IsVisible = false;
            need_maintenance = 0;
            building_age = 0;
        }

        private void Number_of_office_room_CheckedChanged(object sender, CheckedChangedEventArgs e)
        {
            RadioButton button = sender as RadioButton;
            number_rooms = Convert.ToInt32(button.Value);
        }
        //private void Advertiser_TypeButton_Clicked(object sender, EventArgs e)
        //{
        //    if (!this.Advertiser_Type_popuplayout.IsVisible)
        //    {
        //        this.Advertiser_Type_popuplayout.IsVisible = !this.Advertiser_Type_popuplayout.IsVisible;
        //    }
        //    else
        //    {
        //        this.Advertiser_Type_popuplayout.IsVisible = !this.Advertiser_Type_popuplayout.IsVisible;
        //    }
        //}

        private void Select_Advertiser_Type_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            var clicked = e.Item as Advertiser_Type_List;
            var selectedItem = (Advertiser_Type_List)Advertiser_Type_Val.SelectedItem;
            Advertiser_Type_Entry.Text = selectedItem.Name;
            Advertiser_Type_Val.SelectedItem = null;
            Advertiser_Type_popuplayout.IsVisible = false;
        }

        private void type_of_advertisment_Button_Clicked(object sender, EventArgs e)
        {
            if (!this.type_of_advertisment_popuplayout.IsVisible)
            {
                this.type_of_advertisment_popuplayout.IsVisible = !this.type_of_advertisment_popuplayout.IsVisible;
            }
            else
            {
                this.type_of_advertisment_popuplayout.IsVisible = !this.type_of_advertisment_popuplayout.IsVisible;
            }
        }

        private void Select_type_of_advertisment_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            var clicked = e.Item as type_of_advertisment_list;
            var selectedItem = (type_of_advertisment_list)type_of_advertisment_Val.SelectedItem;
            type_of_advertisment_Entry.Text = selectedItem.Name;
            type_of_advertisment_Val.SelectedItem = null;
            type_of_advertisment_popuplayout.IsVisible = false;
            if (type_of_advertisment_Entry.Text == "Normal")
            {
                Show_Ads_Price.IsVisible = false;
                fk_ads_pack_id = 1;
                Type_of_adv_description.Text = "Its free Advertisement and will be publish for 100 days.";
            }
            else if (type_of_advertisment_Entry.Text == "Special")
            {
                Show_Ads_Price.IsVisible = true;
                fk_ads_pack_id = 3;
                Type_of_adv_description.Text = "Give the second priority to shown yourAdvertisement for 1 day on AEQARY property category listing.";
                Get_Default_Price_Clicked();
            }
            else if (type_of_advertisment_Entry.Text == "Freez")
            {
                Show_Ads_Price.IsVisible = true;
                fk_ads_pack_id = 4;
                Type_of_adv_description.Text = "Give the first priority to shown yourAdvertisement for 1 day on AEQARY property category listing.";
                Get_Default_Price_Clicked();
            }
            else if (type_of_advertisment_Entry.Text == "Unique")
            {
                Show_Ads_Price.IsVisible = true;
                fk_ads_pack_id = 2;
                Type_of_adv_description.Text = "Give you a 8 photo to your property in high quality and high resolution , sponsored advertisementon Facebook , posted your property on AEQARY social media pages , and send it to all AEQARY users , give priority to shown yourAdvertisement for 1 day on AEQARY property listing. AEQARY team will call you tocoordination ";
                Get_Default_Price_Clicked();
            }
        }

        private void m2_Clicked(object sender, EventArgs e)
        {
            //if (m2Img.Source == "SelectedRadioBttn_3x.png")
            //{

            //}
        }

        private void acr_Clicked(object sender, EventArgs e)
        {

        }

        private async void Advertiser_TypeButton_Clicked(object sender, EventArgs e)
        {
            if (!this.Advertiser_Type_popuplayout.IsVisible)
            {
                this.Advertiser_Type_popuplayout.IsVisible = !this.Advertiser_Type_popuplayout.IsVisible;
            }
            else
            {
                this.Advertiser_Type_popuplayout.IsVisible = !this.Advertiser_Type_popuplayout.IsVisible;
            }

            //if (!this.Advertiser_Type_popuplayout.IsVisible)
            //{
            //    this.Advertiser_Type_popuplayout.IsVisible = !this.Advertiser_Type_popuplayout.IsVisible;
            //    this.Advertiser_Type_popuplayout.AnchorX = 1;
            //    this.Advertiser_Type_popuplayout.AnchorY = 1;

            //    Animation scaleAnimation = new Animation(
            //        f => this.Advertiser_Type_popuplayout.Scale = f,
            //        0.5,
            //        1,
            //        Easing.SinInOut);

            //    Animation fadeAnimation = new Animation(
            //        f => this.Advertiser_Type_popuplayout.Opacity = f,
            //        0.2,
            //        1,
            //        Easing.SinInOut);

            //    scaleAnimation.Commit(this.Advertiser_Type_popuplayout, "popupScaleAnimation", 250);
            //    fadeAnimation.Commit(this.Advertiser_Type_popuplayout, "popupFadeAnimation", 250);
            //}
            //else
            //{
            //    await Task.WhenAny<bool>
            //      (
            //        this.Advertiser_Type_popuplayout.FadeTo(0, 200, Easing.SinInOut)
            //      );

            //    this.Advertiser_Type_popuplayout.IsVisible = !this.Advertiser_Type_popuplayout.IsVisible;
            //}
        }

        private void land_area_type_CheckedChanged(object sender, EventArgs e)
        {
            RadioButton button = sender as RadioButton;
            land_area_type = Convert.ToInt32(button.Value);
        }

        private void Rdbland_area_type_meter_Clicked(object sender, EventArgs e)
        {
            RadioButton button = sender as RadioButton;
            land_area_type = Convert.ToInt32(button.Value);
            LandAreaTxtName.Text = "m2";
        }

        private void Rdbland_area_type_acres_Clicked(object sender, EventArgs e)
        {
            RadioButton button = sender as RadioButton;
            land_area_type = Convert.ToInt32(button.Value);
            LandAreaTxtName.Text = "acr";
        }

        //private void land_area_typeSelected(object sender, EventArgs e)
        //{

        //    int item = Convert.ToInt32(Rdbland_area_type_meter.Value);
        //    land_area_type = Convert.ToInt32(Rdbland_area_type_meter.Value);
        //    if (item ==1)
        //    {
        //        LandAreaTxtName.Text = "m2";
        //    }
        //    else if (item ==2)
        //    {
        //        LandAreaTxtName.Text = "acr";
        //    }
        //}

        private void land_area_m2_clicked(object sender, EventArgs e)
        {
            land_area_type = Convert.ToInt32(Rdbland_area_type_meter.Value);
            if (land_area_type == 1)
            {
                LandAreaTxtName.Text = "m2";
            }
            else if (land_area_type == 2)
            {
                LandAreaTxtName.Text = "acr";
            }
        }
        private void land_area_acr_clicked(object sender, EventArgs e)
        {
            land_area_type = Convert.ToInt32(Rdbland_area_type_acres.Value);
            if (land_area_type == 1)
            {
                LandAreaTxtName.Text = "m2";
            }
            else if (land_area_type == 2)
            {
                LandAreaTxtName.Text = "acr";
            }
        }

        private void rdp_rental_period_Monthly_clicked(object sender, EventArgs e)
        {
            rental_period = Convert.ToInt32(rdp_rental_period_Monthly.Value);
        }

        private void rdp_rental_period_Daily_clicked(object sender, EventArgs e)
        {
            rental_period = Convert.ToInt32(rdp_rental_period_Daily.Value);
        }

        private void rdp_rental_period_Yearly_clicked(object sender, EventArgs e)
        {
            rental_period = Convert.ToInt32(rdp_rental_period_Yearly.Value);
        }

        private void Rdb_Organization_type_1_clicked(object sender, EventArgs e)
        {
            land_organization_type = Convert.ToInt32(Rdb_Organization_type_1.Value);
            Show_Organization.IsVisible = true;
        }

        private void Rdb_Organization_type_2_clicked(object sender, EventArgs e)
        {
            land_organization_type = Convert.ToInt32(Rdb_Organization_type_2.Value);
            Show_Organization.IsVisible = false;
            land_organization_name = -1;
        }

        private void Number_of_Street_0_CheckedChanged(object sender, EventArgs e)
        {
            number_of_street = Convert.ToInt32(Rdb_Number_of_Street_0.Value);
        }
        private void Number_of_Street_1_CheckedChanged(object sender, EventArgs e)
        {
            number_of_street = Convert.ToInt32(Rdb_Number_of_Street_1.Value);
        }
        private void Number_of_Street_2_CheckedChanged(object sender, EventArgs e)
        {
            number_of_street = Convert.ToInt32(Rdb_Number_of_Street_2.Value);
        }
        private void Number_of_Street_3_CheckedChanged(object sender, EventArgs e)
        {
            number_of_street = Convert.ToInt32(Rdb_Number_of_Street_3.Value);
        }
        private void Number_of_Street_4_CheckedChanged(object sender, EventArgs e)
        {
            number_of_street = Convert.ToInt32(Rdb_Number_of_Street_4.Value);
        }

        private void Number_of_room_1_CheckedChanged(object sender, EventArgs e)
        {
            number_rooms = Convert.ToInt32(Rdb_roomnumber_1.Value);
        }
        private void Number_of_room_2_CheckedChanged(object sender, EventArgs e)
        {
            number_rooms = Convert.ToInt32(Rdb_roomnumber_2.Value);
        }
        private void Number_of_room_3_CheckedChanged(object sender, EventArgs e)
        {
            number_rooms = Convert.ToInt32(Rdb_roomnumber_3.Value);
        }
        private void Number_of_room_4_CheckedChanged(object sender, EventArgs e)
        {
            number_rooms = Convert.ToInt32(Rdb_roomnumber_4.Value);
        }

        private void Rdb_officeroomnumber_1_CheckedChanged(object sender, EventArgs e)
        {
            number_rooms = Convert.ToInt32(Rdb_officeroomnumber_1.Value);
        }
        private void Rdb_officeroomnumber_2_CheckedChanged(object sender, EventArgs e)
        {
            number_rooms = Convert.ToInt32(Rdb_officeroomnumber_2.Value);
        }
        private void Rdb_officeroomnumber_3_CheckedChanged(object sender, EventArgs e)
        {
            number_rooms = Convert.ToInt32(Rdb_officeroomnumber_3.Value);
        }
        private void Rdb_officeroomnumber_4_CheckedChanged(object sender, EventArgs e)
        {
            number_rooms = Convert.ToInt32(Rdb_officeroomnumber_4.Value);
        }

        private void rdp_Buiding_type_1_CheckedChanged(object sender, EventArgs e)
        {
            building_area_type = Convert.ToInt32(rdp_Buiding_type_1.Value);
            Show_Building_Area_From_To.IsVisible = false;
            Show_Building_Area.IsVisible = true;
            Show_Size_per_apartment.IsVisible = true;
            Area_From.Text = "0";
            Area_To.Text = "0";
        }
        private void rdp_Buiding_type_2_CheckedChanged(object sender, EventArgs e)
        {
            building_area_type = Convert.ToInt32(rdp_Buiding_type_2.Value);
            Show_Building_Area_From_To.IsVisible = false;
            Show_Building_Area.IsVisible = true;
            Show_Size_per_apartment.IsVisible = true;
            Area_From.Text = "0";
            Area_To.Text = "0";
        }

        private void rdp_Buiding_type_3_CheckedChanged(object sender, EventArgs e)
        {
            building_area_type = Convert.ToInt32(rdp_Buiding_type_3.Value);
            Show_Building_Area_From_To.IsVisible = true;
            Show_Size_per_apartment.IsVisible = false;
            Show_Building_Area.IsVisible = false;
            Building_Area.Text = "0";
        }

        private void rdb_Number_of_normal_bath_room_1_CheckedChanged(object sender, EventArgs e)
        {
            number_of_normal_bathroom = Convert.ToInt32(rdb_Number_of_normal_bath_room_1.Value);
        }
        private void rdb_Number_of_normal_bath_room_2_CheckedChanged(object sender, EventArgs e)
        {
            number_of_normal_bathroom = Convert.ToInt32(rdb_Number_of_normal_bath_room_2.Value);
        }
        private void rdb_Number_of_normal_bath_room_3_CheckedChanged(object sender, EventArgs e)
        {
            number_of_normal_bathroom = Convert.ToInt32(rdb_Number_of_normal_bath_room_3.Value);
        }
        private void rdb_Number_of_normal_bath_room_4_CheckedChanged(object sender, EventArgs e)
        {
            number_of_normal_bathroom = Convert.ToInt32(rdb_Number_of_normal_bath_room_4.Value);
        }
        private void rdb_Number_of_master_bath_room_0_CheckedChanged(object sender, EventArgs e)
        {
            number_of_master_bathroom = Convert.ToInt32(rdb_Number_of_master_bath_room_0.Value);
        }
        private void rdb_Number_of_master_bath_room_1_CheckedChanged(object sender, EventArgs e)
        {
            number_of_master_bathroom = Convert.ToInt32(rdb_Number_of_master_bath_room_1.Value);
        }
        private void rdb_Number_of_master_bath_room_2_CheckedChanged(object sender, EventArgs e)
        {
            number_of_master_bathroom = Convert.ToInt32(rdb_Number_of_master_bath_room_2.Value);
        }
        private void rdb_Number_of_master_bath_room_3_CheckedChanged(object sender, EventArgs e)
        {
            number_of_master_bathroom = Convert.ToInt32(rdb_Number_of_master_bath_room_3.Value);
        }
        private void rdb_Number_of_master_bath_room_4_CheckedChanged(object sender, EventArgs e)
        {
            number_of_master_bathroom = Convert.ToInt32(rdb_Number_of_master_bath_room_4.Value);
        }

        private void rdb_Number_of_balcons_0_CheckedChanged(object sender, EventArgs e)
        {
            number_of_balcones = Convert.ToInt32(rdb_Number_of_balcons_0.Value);
        }
        private void rdb_Number_of_balcons_1_CheckedChanged(object sender, EventArgs e)
        {
            number_of_balcones = Convert.ToInt32(rdb_Number_of_balcons_1.Value);
        }
        private void rdb_Number_of_balcons_2_CheckedChanged(object sender, EventArgs e)
        {
            number_of_balcones = Convert.ToInt32(rdb_Number_of_balcons_2.Value);
        }
        private void rdb_Number_of_balcons_3_CheckedChanged(object sender, EventArgs e)
        {
            number_of_balcones = Convert.ToInt32(rdb_Number_of_balcons_3.Value);
        }
        private void rdb_Number_of_balcons_4_CheckedChanged(object sender, EventArgs e)
        {
            number_of_balcones = Convert.ToInt32(rdb_Number_of_balcons_4.Value);
        }

        private void Rdb_number_of_Floor_1_CheckedChanged(object sender, EventArgs e)
        {
            number_of_floors = Convert.ToInt32(Rdb_number_of_Floor_1.Value);
        }
        private void Rdb_number_of_Floor_2_CheckedChanged(object sender, EventArgs e)
        {
            number_of_floors = Convert.ToInt32(Rdb_number_of_Floor_2.Value);
        }
        private void Rdb_number_of_Floor_3_CheckedChanged(object sender, EventArgs e)
        {
            number_of_floors = Convert.ToInt32(Rdb_number_of_Floor_3.Value);
        }
        private void Rdb_number_of_Floor_4_CheckedChanged(object sender, EventArgs e)
        {
            number_of_floors = Convert.ToInt32(Rdb_number_of_Floor_4.Value);
        }

        private void Rdb_Floornumber_minus_1_CheckedChanged(object sender, EventArgs e)
        {
            floor_number = Convert.ToInt32(Rdb_Floornumber_minus_1.Value);
        }
        private void Rdb_Floornumber_0_CheckedChanged(object sender, EventArgs e)
        {
            floor_number = Convert.ToInt32(Rdb_Floornumber_0.Value);
        }
        private void Rdb_Floornumber_1_CheckedChanged(object sender, EventArgs e)
        {
            floor_number = Convert.ToInt32(Rdb_Floornumber_1.Value);
        }
        private void Rdb_Floornumber_2_CheckedChanged(object sender, EventArgs e)
        {
            floor_number = Convert.ToInt32(Rdb_Floornumber_2.Value);
        }
        private void Rdb_Floornumber_3_CheckedChanged(object sender, EventArgs e)
        {
            floor_number = Convert.ToInt32(Rdb_Floornumber_3.Value);
        }
        private void Rdb_Floornumber_4_CheckedChanged(object sender, EventArgs e)
        {
            floor_number = Convert.ToInt32(Rdb_Floornumber_4.Value);
        }

        private void Rdb_number_of_shop_1_CheckedChanged(object sender, EventArgs e)
        {
            number_of_shops = Convert.ToInt32(Rdb_number_of_shop_1.Value);
        }

        private void Rdb_number_of_shop_2_CheckedChanged(object sender, EventArgs e)
        {
            number_of_shops = Convert.ToInt32(Rdb_number_of_shop_2.Value);
        }
        private void Rdb_number_of_shop_3_CheckedChanged(object sender, EventArgs e)
        {
            number_of_shops = Convert.ToInt32(Rdb_number_of_shop_3.Value);
        }
        private void Rdb_number_of_shop_4_CheckedChanged(object sender, EventArgs e)
        {
            number_of_shops = Convert.ToInt32(Rdb_number_of_shop_4.Value);
        }

        private void Rdb_Appartment_1_CheckedChanged(object sender, EventArgs e)
        {
            number_of_appartments = Convert.ToInt32(Rdb_Appartment_1.Value);
        }
        private void Rdb_Appartment_2_CheckedChanged(object sender, EventArgs e)
        {
            number_of_appartments = Convert.ToInt32(Rdb_Appartment_2.Value);
        }
        private void Rdb_Appartment_3_CheckedChanged(object sender, EventArgs e)
        {
            number_of_appartments = Convert.ToInt32(Rdb_Appartment_3.Value);
        }
        private void Rdb_Appartment_4_CheckedChanged(object sender, EventArgs e)
        {
            number_of_appartments = Convert.ToInt32(Rdb_Appartment_4.Value);
        }

        private void Rdb_number_of_offices_1_CheckedChanged(object sender, EventArgs e)
        {
            number_of_offices = Convert.ToInt32(Rdb_number_of_offices_1.Value);
        }
        private void Rdb_number_of_offices_2_CheckedChanged(object sender, EventArgs e)
        {
            number_of_offices = Convert.ToInt32(Rdb_number_of_offices_2.Value);
        }
        private void Rdb_number_of_offices_3_CheckedChanged(object sender, EventArgs e)
        {
            number_of_offices = Convert.ToInt32(Rdb_number_of_offices_3.Value);
        }
        private void Rdb_number_of_offices_4_CheckedChanged(object sender, EventArgs e)
        {
            number_of_offices = Convert.ToInt32(Rdb_number_of_offices_4.Value);
        }

        private void Rdb_number_of_store_1_CheckedChanged(object sender, EventArgs e)
        {
            number_of_Store = Convert.ToInt32(Rdb_number_of_store_1.Value);
        }
        private void Rdb_number_of_store_2_CheckedChanged(object sender, EventArgs e)
        {
            number_of_Store = Convert.ToInt32(Rdb_number_of_store_2.Value);
        }
        private void Rdb_number_of_store_3_CheckedChanged(object sender, EventArgs e)
        {
            number_of_Store = Convert.ToInt32(Rdb_number_of_store_3.Value);
        }
        private void Rdb_number_of_store_4_CheckedChanged(object sender, EventArgs e)
        {
            number_of_Store = Convert.ToInt32(Rdb_number_of_store_4.Value);
        }

        private void rdp_Building_Age_1_CheckedChanged(object sender, EventArgs e)
        {
            building_age = Convert.ToInt32(rdp_Building_Age_1.Value);
        }
        private void rdp_Building_Age_2_CheckedChanged(object sender, EventArgs e)
        {
            building_age = Convert.ToInt32(rdp_Building_Age_2.Value);
        }
        private void rdp_Building_Age_3_CheckedChanged(object sender, EventArgs e)
        {
            building_age = Convert.ToInt32(rdp_Building_Age_3.Value);
        }
        private void rdp_Building_Age_4_CheckedChanged(object sender, EventArgs e)
        {
            building_age = Convert.ToInt32(rdp_Building_Age_4.Value);
        }

        private void rdp_door_1_CheckedChanged(object sender, EventArgs e)
        {
            number_doors = Convert.ToInt32(rdp_door_1.Value);
        }
        private void rdp_door_2_CheckedChanged(object sender, EventArgs e)
        {
            number_doors = Convert.ToInt32(rdp_door_2.Value);
        }
        private void rdp_door_3_CheckedChanged(object sender, EventArgs e)
        {
            number_doors = Convert.ToInt32(rdp_door_3.Value);
        }
        private void rdp_door_4_CheckedChanged(object sender, EventArgs e)
        {
            number_doors = Convert.ToInt32(rdp_door_4.Value);
        }

        private async void RegulationButton_Clicked(object sender, EventArgs e)
        {
            if (!this.Regulationpopuplayout.IsVisible)
            {
                this.Regulationpopuplayout.IsVisible = !this.Regulationpopuplayout.IsVisible;
                this.Regulationpopuplayout.AnchorX = 1;
                this.Regulationpopuplayout.AnchorY = 1;

                Animation scaleAnimation = new Animation(
                    f => this.Regulationpopuplayout.Scale = f,
                    0.5,
                    1,
                    Easing.SinInOut);

                Animation fadeAnimation = new Animation(
                    f => this.Regulationpopuplayout.Opacity = f,
                    0.2,
                    1,
                    Easing.SinInOut);

                scaleAnimation.Commit(this.Regulationpopuplayout, "popupScaleAnimation", 250);
                fadeAnimation.Commit(this.Regulationpopuplayout, "popupFadeAnimation", 250);
            }
            else
            {
                await Task.WhenAny<bool>
                  (
                    this.Regulationpopuplayout.FadeTo(0, 200, Easing.SinInOut)
                  );

                this.Regulationpopuplayout.IsVisible = !this.Regulationpopuplayout.IsVisible;
            }
        }

        private void SelectRegulation_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            var clicked = (Repository)Regulation_Val.SelectedItem;
            land_organization_name = Convert.ToInt32(clicked.organizer_id);

            //            var selectedItem = (Repository)Regulation_Val.SelectedItem;
            //land_organization_name = Convert.ToInt32(clicked.organizer_id);
            //Get_Organization();
            RegulationName.Text = clicked.organizer_name_en;
            Regulation_Val.SelectedItem = null;
            Regulationpopuplayout.IsVisible = false;

        }

        private void aaaaaaaaaaaa(object sender, EventArgs e)
        {
            Advertiser_Type_popuplayout.IsVisible = true;
        }

        private async void Get_Default_Price_Clicked()
        {
            try
            {
                var client = new HttpClient();
                string API = Constants.GitHubReposEndpoint1 + "get_ads_type_price?main_service_Id=" + Convert.ToInt32(Application.Current.Properties["main_service_id"]) + "&sub_service_Id=" + Convert.ToInt32(Application.Current.Properties["sub_service_id"]) + "&ads_type_id=" + fk_ads_pack_id + "";
                HttpResponseMessage response = await client.GetAsync(API);
                var result = await response.Content.ReadAsStringAsync();
                Repository MPP = new Repository();
                MPP = JsonConvert.DeserializeObject<Repository>(result);
                int days = 1;
                //int Total_Price = MPP.Default_Price;
                string[] multiArray = MPP.Default_Price.Split(new Char[] {'.'}); 
                Price_Val.Text = multiArray[0];
                End_date = DateTime.Today.AddDays(days);
                no_of_days_for_advs.IsVisible = false;
            }
            catch (Exception)
            { }
        }
        private async void decreaseButton_Clicked(object sender, EventArgs e)
        {
            Get_Price();
            await Task.Delay(500);
            if ((Convert.ToInt32(lbldisp.Text) > 1) && ((Convert.ToInt32(lbldisp.Text) <= 100)))
            {
                int days = (Convert.ToInt32(lbldisp.Text)) - 1;
                lbldisp.Text = days.ToString();
                Total_Price = Total_Price - Ads_Price;
                Price_Val.Text = Total_Price.ToString();
                End_date = DateTime.Today.AddDays(days);
                no_of_days_for_advs.IsVisible = false;
                Price_Val.Text = Total_Price.ToString();
            }
            else if ((Convert.ToInt32(lbldisp.Text) == 1))
            {
                int days = 1;
                lbldisp.Text = days.ToString();
                no_of_days_for_advs.IsVisible = true;
                Price_Val.Text = Ads_Default_Price.ToString();
            }

            else
            {
                no_of_days_for_advs.IsVisible = true;
            }
        }
        private async void increaseButton_Clicked(object sender, EventArgs e)
        {
            Get_Price();
            await Task.Delay(500);
            if ((Convert.ToInt32(lbldisp.Text) >= 1) && ((Convert.ToInt32(lbldisp.Text) < 100)))
            {
                int days;
                if (lbldisp.Text == "1")
                {
                    days = 1 ;
                    int newdays = 2;
                    lbldisp.Text = newdays.ToString();

                    Total_Price = Ads_Default_Price + (Ads_Price);
                    Price_Val.Text = Total_Price.ToString();
                    End_date = DateTime.Today.AddDays(days);
                    no_of_days_for_advs.IsVisible = false;
                }
                else
                {
                    days = (Convert.ToInt32(lbldisp.Text)) ;
                    
                    Total_Price = Ads_Default_Price + (Ads_Price * days);
                    days = (Convert.ToInt32(lbldisp.Text)) + 1;
                    lbldisp.Text = days.ToString();
                    Price_Val.Text = Total_Price.ToString();
                    End_date = DateTime.Today.AddDays(days);
                    no_of_days_for_advs.IsVisible = false;
                }
                
            }
            else
            {
                no_of_days_for_advs.IsVisible = true;
            }
        }

        private void Has_building_1_clicked(object sender, EventArgs e)
        {
            has_building = Convert.ToInt32(Has_building_1.Value);
            Show_Has_building.IsVisible = true;
            Show_Farm_Building_Area.IsVisible = true;
            Show_Building_status.IsVisible = true;
            Show_Number_of_room.IsVisible = true;
            Show__Number_of_normal_bath_room.IsVisible = true;
            Show__Number_of_master_bath_room.IsVisible = true;
        }
        private void Has_building_0_clicked(object sender, EventArgs e)
        {
            has_building = Convert.ToInt32(Has_building_0.Value);
            Show_Has_building.IsVisible = false;
            Show_Farm_Building_Area.IsVisible = false;
            Show_Building_status.IsVisible = false;
            Show_maintenace.IsVisible = false;
            Show_Number_of_room.IsVisible = false;
            Show__Number_of_normal_bath_room.IsVisible = false;
            Show__Number_of_master_bath_room.IsVisible = false;
            need_maintenance = 0;
            building_age = 0;
        }

        private void rdp_Old_1_CheckedChanged(object sender, EventArgs e)
        {
            building_status = Convert.ToInt32(rdp_Old_1.Value);
            Show_maintenace.IsVisible = true;
        }

        private void rdp_Old_2_CheckedChanged(object sender, EventArgs e)
        {
            building_status = Convert.ToInt32(rdp_Old_2.Value);
            Show_maintenace.IsVisible = false;
            need_maintenance = 0;
            building_age = 0;
        }

        private void rdp_Main_1_CheckedChanged(object sender, EventArgs e)
        {
            street_category = Convert.ToInt32(rdp_Main_1.Value);
        }
        private void rdp_Sub_0_CheckedChanged(object sender, EventArgs e)
        {
            street_category = Convert.ToInt32(rdp_Sub_0.Value);
        }

        private void rdb_Payment_Type_1_CheckedChanged(object sender, EventArgs e)
        {
            payment_type = Convert.ToInt32(rdb_Payment_Type_1.Value);
        }

        private void rdb_Payment_Type_2_CheckedChanged(object sender, EventArgs e)
        {
            payment_type = Convert.ToInt32(rdb_Payment_Type_2.Value);
        }

        private void rdb_Allow_comment_1_clicked(object sender, EventArgs e)
        {
            allow_comments = Convert.ToInt32(rdb_Allow_comment_1.Value);
        }
        private void rdb_Allow_comment_0_clicked(object sender, EventArgs e)
        {
            allow_comments = Convert.ToInt32(rdb_Allow_comment_0.Value);
        }

        private void rdp_maintenance_1_clicked(object sender, EventArgs e)
        {
            need_maintenance = Convert.ToInt32(rdp_maintenance_1.IsChecked);
        }
        private void rdp_maintenance_0_clicked(object sender, EventArgs e)
        {
            need_maintenance = Convert.ToInt32(rdp_maintenance_0.IsChecked);
        }

        private void rental_period_CheckedChanged(object sender, CheckedChangedEventArgs e)
        {
            RadioButton button = sender as RadioButton;
            rental_period = Convert.ToInt32(button.Value);
        }

        private void Bathroom_CheckedChanged(object sender, CheckedChangedEventArgs e)
        {
            RadioButton button = sender as RadioButton;
            //Adp.allow_comments = Convert.ToInt32(button.Value);
            has_bathroom = Convert.ToInt32(button.Value);
        }

        private void Upper_block_CheckedChanged(object sender, CheckedChangedEventArgs e)
        {
            RadioButton button = sender as RadioButton;
            //Adp.allow_comments = Convert.ToInt32(button.Value);
            has_upper_block = Convert.ToInt32(button.Value);
        }

        private void chk_maintenace_CheckedChanged(object sender, CheckedChangedEventArgs e)
        {
            CheckBox button = sender as CheckBox;
            need_maintenance = Convert.ToInt32(button.IsChecked);
        }

        private void Number_of_Street_CheckedChanged(object sender, CheckedChangedEventArgs e)
        {
            //number_of_street = Convert.ToInt32(button.Value);
        }

        void rdb_parking_OnToggled(object sender, ToggledEventArgs e)
        {
            Parking = Convert.ToInt32(rdb_parking_switch.IsToggled);
        }
        private void Number_of_floor_CheckedChanged(object sender, CheckedChangedEventArgs e)
        {
            RadioButton button = sender as RadioButton;
            number_of_floors = Convert.ToInt32(button.Value);
        }

        private void Number_of_offices_CheckedChanged(object sender, CheckedChangedEventArgs e)
        {
            RadioButton button = sender as RadioButton;
            number_of_offices = Convert.ToInt32(button.Value);
        }
        private void Number_of_Shop_CheckedChanged(object sender, CheckedChangedEventArgs e)
        {
            RadioButton button = sender as RadioButton;
            number_of_shops = Convert.ToInt32(button.Value);
        }
        private void Number_of_store_CheckedChanged(object sender, CheckedChangedEventArgs e)
        {
            RadioButton button = sender as RadioButton;
            number_of_Store = Convert.ToInt32(button.Value);
        }

        private void Number_of_Appartment_CheckedChanged(object sender, CheckedChangedEventArgs e)
        {
            RadioButton button = sender as RadioButton;
            number_of_appartments = Convert.ToInt32(button.Value);
        }

        private void Number_of_normal_bath_room_CheckedChanged(object sender, CheckedChangedEventArgs e)
        {
            RadioButton button = sender as RadioButton;
            number_of_normal_bathroom = Convert.ToInt32(button.Value);
        }

        private void Number_of_master_bath_room_CheckedChanged(object sender, CheckedChangedEventArgs e)
        {
            RadioButton button = sender as RadioButton;
            number_of_master_bathroom = Convert.ToInt32(button.Value);
        }




        private void Number_of_balcons_CheckedChanged(object sender, CheckedChangedEventArgs e)
        {
            RadioButton button = sender as RadioButton;
            number_of_balcones = Convert.ToInt32(button.Value);
        }
        private void ElevatorCheckedChanged(object sender, CheckedChangedEventArgs e)
        {
            RadioButton button = sender as RadioButton;
            has_elevator = Convert.ToInt32(button.Value);
        }
        private void Pool_Toggled(object sender, ToggledEventArgs e)
        {
            has_pool = Convert.ToInt32(Pool.IsToggled);
        }
        private void Entertament_Toggled(object sender, ToggledEventArgs e)
        {
            has_entertament = Convert.ToInt32(Entertament.IsToggled);
        }
        private void Crops_Toggled(object sender, ToggledEventArgs e)
        {
            has_crops = Convert.ToInt32(Crops.IsToggled);
        }
        private void Roof_Toggled(object sender, ToggledEventArgs e)
        {
            has_roof = Convert.ToInt32(Switch_Roof.IsToggled);
        }
        private void Furnished_Toggled(object sender, ToggledEventArgs e)
        {
            has_furnished = Convert.ToInt32(Switch_Furnished.IsToggled);
        }

        private void Garden_Toggled(object sender, ToggledEventArgs e)
        {
            has_garden = Convert.ToInt32(Switch_Garden.IsToggled);
        }
        private void Existenceofservices_CheckedChanged(object sender, CheckedChangedEventArgs e)
        {
            RadioButton button = sender as RadioButton;
            existence_of_services = Convert.ToInt32(button.Value);
        }

        void Rdb_Existence_of_services_OnToggled(object sender, ToggledEventArgs e)
        {
            existence_of_services = Convert.ToInt32(Rdb_Existence_of_services_switch.IsToggled);
        }
        void rdb_Active_OnToggled(object sender, ToggledEventArgs e)
        {
            ads_status = Convert.ToInt32(rdb_Active_switch.IsToggled);
        }
        void rdp_Kitchen_switch_OnToggled(object sender, ToggledEventArgs e)
        {
            has_kitchen = Convert.ToInt32(rdp_Kitchen_switch.IsToggled);
        }
        void rdp_Bathroom_OnToggled(object sender, ToggledEventArgs e)
        {
            has_bathroom = Convert.ToInt32(rdp_Bathroom_switch.IsToggled);
        }
        private void Building_Age_CheckedChanged(object sender, CheckedChangedEventArgs e)
        {
            RadioButton button = sender as RadioButton;
            building_age = Convert.ToInt32(button.Value);
        }

        private void Floor_Number_CheckedChanged(object sender, CheckedChangedEventArgs e)
        {
            RadioButton button = sender as RadioButton;
            floor_number = Convert.ToInt32(button.Value);
        }

        private void Streetcategoury_CheckedChanged(object sender, CheckedChangedEventArgs e)
        {
            RadioButton button = sender as RadioButton;
            street_category = Convert.ToInt32(button.Value);
        }

        private void Buildingstatus_CheckedChanged(object sender, CheckedChangedEventArgs e)
        {
            RadioButton button = sender as RadioButton;
            building_status = Convert.ToInt32(button.Value);
            if (building_status == 1)
            {
                Show_maintenace.IsVisible = true;
            }
            else
            {
                Show_maintenace.IsVisible = false;
                need_maintenance = 0;
                building_age = 0;
            }
        }
        private void Number_of_doors_CheckedChanged(object sender, CheckedChangedEventArgs e)
        {
            RadioButton button = sender as RadioButton;
            number_doors = Convert.ToInt32(button.Value);
        }

        private void Number_of_room_CheckedChanged(object sender, CheckedChangedEventArgs e)
        {
            RadioButton button = sender as RadioButton;
            number_rooms = Convert.ToInt32(button.Value);
        }

        private void OnAllowcommentRadioButtonCheckedChanged(object sender, CheckedChangedEventArgs e)
        {
            RadioButton button = sender as RadioButton;

            allow_comments = Convert.ToInt32(button.Value);

        }
        private void OnAdvertiserTypeRadioButtonCheckedChanged(object sender, CheckedChangedEventArgs e)
        {
            RadioButton button = sender as RadioButton;
            advertiser_type = Convert.ToInt32(button.Value);
            CompayPaidType.IsVisible = true;
        }
        private void OnAdsTypeRadioButtonCheckedChanged(object sender, CheckedChangedEventArgs e)
        {
            RadioButton button = sender as RadioButton;
            fk_ads_pack_id = Convert.ToInt32(button.Value);
            int AdsType_Val = Convert.ToInt32(button.Value);
            if (AdsType_Val != 1)
            {
                lbldisp.Text = "1";
                //Stepper_price.Value = 1;

            }
            else
            {
                lbldisp.Text = "100";
                //Stepper_price.Value = 100;
                // Price = 0;
                //  Price_Val.Text = "0";
            }
            Get_Price();
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
                //  string City = $"{addrs.PostalCode} {addrs.Locality} ";
                // string Country = addrs.CountryName;

                addressEntry.Text = Street;
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
        private void Submit_Clicked(object sender, EventArgs e)
        {
            string API = Constants.GitHubReposEndpoint1 + "general_ads";
            Ads_Insert_Update(API);
        }

        private void Next_Clicked(object sender, EventArgs e)
        {
            image_Location_tab.IsVisible = true;
            Details_tab.IsVisible = false;

        }
        #region Region_Upload_Photo
        private async void upload_Clicked(object sender, EventArgs e)
        {
            //  UploadPhoto();
            Upload_Multi_Photo();

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
                                    //  Photo_path_array[count] = img;
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
        public async Task<string> Save_Photos_Data(String url)
        {

            var client = new HttpClient();
            CLS_Photo_Data RP = new CLS_Photo_Data();
            RP.operation_type = "I";
            RP.ads_id = ads_id;
            RP.FK_Sub_Service_Id = Convert.ToInt32(Application.Current.Properties["sub_service_id"]);
            RP.Photo_Path = Photo_Path;


            string jsonData = JsonConvert.SerializeObject(RP);
            // string jsonData = @"{""email"" : ""username"", ""password"" :  " + password + "";
            var content = new StringContent(jsonData, Encoding.UTF8, "application/json");
            HttpResponseMessage response = await client.PostAsync(url, content);

            var result = await response.Content.ReadAsStringAsync();

            // var Get_Result = JsonConvert.DeserializeObject(result);
            Register_User RU = new Register_User();
            RU = JsonConvert.DeserializeObject<Register_User>(result);


            return result;
        }
        #endregion

    }

    public class Ads_General_Details__Form_Property
    {
        #region Common

        public string operation_type { get; set; }
        public int ads_id { get; set; }
        public int fk_main_service { get; set; }
        public int fk_sub_service { get; set; }
        public int fk_city_id { get; set; }
        public int fk_region_id { get; set; }
        public int FK_Register_Id { get; set; }
        public int fk_ads_pack_id { get; set; }
        public string ads_description { get; set; }
        public string logo { get; set; }
        public int ads_status { get; set; }
        public int allow_comments { get; set; }
        public DateTime start_date { get; set; }
        public DateTime end_date { get; set; }
        public string image { get; set; }
        public double latitude { get; set; }
        public double longitude { get; set; }
        public int street_category { get; set; }
        public DateTime inserted_date { get; set; }
        public int Building_Condition { get; set; }
        public int parking { get; set; }
        public int building_age { get; set; }
        public int need_maintenance { get; set; }
        public int advertiser_type { get; set; }
        public int price { get; set; }
        public int rental_period { get; set; }
        public int payment_type { get; set; }
        public int number_doors { get; set; }
        public int building_area_type { get; set; }
        public int building_area_fixed_value { get; set; }
        public int building_area_from_value { get; set; }
        public int building_area_to_value { get; set; }
        public int land_area_aalue { get; set; }
        public int land_area_type { get; set; }
        public int has_bathroom { get; set; }
        public int has_kitchen { get; set; }
        public int number_rooms { get; set; }
        public int has_upper_block { get; set; }
        public int floor_number { get; set; }
        public int number_of_floors { get; set; }
        public int number_of_offices { get; set; }
        public int number_of_shops { get; set; }
        public int number_of_store { get; set; }
        public int number_of_Appartments { get; set; }
        public int number_of_normal_bathroom { get; set; }
        public int number_of_master_bathroom { get; set; }
        public int number_of_balcones { get; set; }
        public int number_of_street { get; set; }
        public string annual_income { get; set; }
        public string Size_per_apartment { get; set; }
        public int has_elevator { get; set; }
        public int has_roof { get; set; }
        public int has_furnished { get; set; }
        public int has_garden { get; set; }
        public int has_Pool { get; set; }
        public int has_entertament { get; set; }
        public int has_crops { get; set; }
        public int land_organization_type { get; set; }
        public int land_organization_name { get; set; }
        public int existence_of_services { get; set; }
        public string title { get; set; }
        public int has_building { get; set; }



        #endregion
    }

    public class Ads_Form_header : BaseViewModel
    {
        private Item _selectedItem;


        public Ads_Form_header()
        {
            Title = "ADD ADVERTISEMENT";
            // Items = new ObservableCollection<Item>();
            // LoadItemsCommand = new Command(async () => await ExecuteLoadItemsCommand());

            // ItemTapped = new Command<Item>(OnItemSelected);

            //AddItemCommand = new Command(OnAddItem);
        }

    }

    public class Advertiser_Type_List
    {
        public string Name { get; set; }
        public int Id { get; set; }
    }
    public class type_of_advertisment_list
    {
        public string Name { get; set; }
        public int Id { get; set; }
    }
}