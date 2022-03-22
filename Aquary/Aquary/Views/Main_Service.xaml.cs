using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Aquary.Models;
using Aquary.Services;
using Aquary.ViewModels;
using Newtonsoft.Json;
using Xamarin.Forms;


namespace Aquary.Views
{

    public partial class Main_Service : ContentPage
    {
        int add_ads_session;

        RestService _restService;
        private Item _selectedItem;
        Main_ServiceViewModel _viewModel;
        BaseViewModel _BaseViewModel;

        private double width;
        private double height;
        public Main_Service()
        {
            try
            {
                InitializeComponent();
                _restService = new RestService();

                BindingContext = new Main_ServiceViewModel();

                int box = 2;
                string API_ads = Constants.GitHubReposEndpoint1 + "get_slider?box_id=" + box;
                GetSliderPhoto(API_ads);

                footer.ProfileIconSource = "home.png";
                int register_id = Convert.ToInt32(Application.Current.Properties["register_id"]);
            }
            catch (Exception ex)
            {

            }

        }
        protected override void OnSizeAllocated(double width, double height)
        {
            base.OnSizeAllocated(width, height);
            if (width != this.width || height != this.height)
            {
                this.width = width;
                this.height = height;
                if (width > height)
                {
                    // innerGrid.RowDefinitions.Clear();
                    // innerGrid.ColumnDefinitions.Clear();
                    //innerGrid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(1, GridUnitType.Star) });
                    //innerGrid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });
                    // innerGrid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });
                    // innerGrid.Children.Remove(controlsGrid);
                    //innerGrid.Children.Add(controlsGrid, 1, 0);
                }
                else
                {
                    // innerGrid.ColumnDefinitions.Clear();

                    //innerGrid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });
                    // innerGrid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(1, GridUnitType.Auto) });
                    // innerGrid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(1, GridUnitType.Star) });
                    // innerGrid.Children.Remove(controlsGrid);
                    // innerGrid.Children.Add(controlsGrid, 0, 1);
                }
            }
        }
        public async void Get_MainService()
        {
            string API = Constants.GitHubReposEndpoint1 + "get_main_service";
            List<Repository> repositories = await _restService.GetRepositoriesAsync(API);

            collectionView.ItemsSource = repositories;
            Category_Val.ItemsSource = repositories;
        }
        public async void Get_SubService(int main_service_id)
        {
            //  int main_service_id = Convert.ToInt32(Application.Current.Properties["main_service_id"]);
            string API = Constants.GitHubReposEndpoint1 + "get_sub_service?main_service_Id=" + main_service_id + "";
            List<Repository> repositories = await _restService.GetRepositoriesAsync(API);
            Sub_Category_val.ItemsSource = repositories;
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
        private async void OnLoginClicked(object obj)
        {
            // Prefixing with `//` switches to a different navigation stack instead of pushing to the active one
            await Shell.Current.GoToAsync(nameof(NewItemPage));
            // await InputBox(this.Navigation);
        }
        void CollectionViewListSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            UpdateSelectionData(e.PreviousSelection, e.CurrentSelection);

        }
        private async void UpdateSelectionData(IEnumerable<object> previousSelectedContact, IEnumerable<object> currentSelectedContact)
        {
            try
            {
                var selectedContact = currentSelectedContact.FirstOrDefault() as Repository;
                Application.Current.Properties["main_service_id"] = selectedContact.main_service_id;
                var selectedLanguage = await Xamarin.Essentials.SecureStorage.GetAsync(Constants.SelctedLanguage);

                if (selectedLanguage is null || selectedLanguage.ToString() == "En")
                {
                    Application.Current.Properties["main_service_Name"] = selectedContact.service_en;
                }
                else if (selectedLanguage.ToString() == "Ar")
                {
                    Application.Current.Properties["main_service_Name"] = selectedContact.service_ar;
                }
                if (selectedContact.service_en == "Property For sale")
                {
                    ((App)(App.Current)).Sale_Section_Selected = true;
                }
                else if (selectedContact.service_en == " Property For Rent")
                {
                    ((App)(App.Current)).Rent_Section_Selected = true;
                }
                else if (selectedContact.service_en == " Decoration and design")
                {
                    ((App)(App.Current)).Decoration_Section_Selected = true;
                }
                else if (selectedContact.service_en == "Construction and maintenance")
                {
                    ((App)(App.Current)).Maintenance_Section_Selected = true;
                }
                Move_next();
            }
            catch (Exception ex)
            { }
        }
        private async void Move_next()
        {
            // await Shell.Current.GoToAsync("//Sub_Service", false);
            await Navigation.PushAsync(new Sub_Service());
            //DateTime yesterday = DateTime.Now.AddDays(-1).Date;
        }
        private async void ToolbarItem_Clicked(object sender, EventArgs e)
        {
            try
            {
                int register_id = Convert.ToInt32(Application.Current.Properties["register_id"]);
                if (register_id > 0)
                {
                    Application.Current.Properties["add_ads_session"] = 1;
                    Application.Current.Properties["Operation_Type"] = "I";
                    await Navigation.PushAsync(new Main_Service_add_ads());
                }
                else
                {
                    await DisplayAlert("Alert", "Please Login to continue the process", "OK");
                }
            }
            catch (Exception ex)
            {
                await DisplayAlert("Alert", "Please Login to continue the process", "OK");
            }


        }
        protected override void OnAppearing()
        {
            //Write the code of your page here
            base.OnAppearing();
            try
            {

                Get_MainService();




                Application.Current.Properties["add_ads_session"] = 0;
                Application.Current.Properties["Is_Interested"] = 0;
                Application.Current.Properties["Is_Favorite"] = 0;
                Application.Current.Properties["main_service_id"] = -1;
                Application.Current.Properties["sub_service_id"] = -1;
                Application.Current.Properties["Region_Id"] = -1;
                Application.Current.Properties["City_Id"] = -1;
                Application.Current.Properties["Price_From"] = 0;
                Application.Current.Properties["Price_To"] = 0;


                int register_id = Convert.ToInt32(Application.Current.Properties["register_id"]);

                if (register_id > 0)
                {
                    MessagingCenter.Send<App, string>(App.Current as App, "MyFav_enable", "");
                    MessagingCenter.Send<App, string>(App.Current as App, "Myads_enable", "");
                    MessagingCenter.Send<App, string>(App.Current as App, "Myprofile_enable", "");
                    MessagingCenter.Send<App, string>(App.Current as App, "changepassword_enable", "");
                    MessagingCenter.Send<App, string>(App.Current as App, "Logout_enable", "");
                    MessagingCenter.Send<App, string>(App.Current as App, "My_interested_enable", "");
                }
            }

            catch (Exception ex)
            {
                MessagingCenter.Send<App, string>(App.Current as App, "MyFav_Disable", "");
                MessagingCenter.Send<App, string>(App.Current as App, "Myads_Disable", "");
                MessagingCenter.Send<App, string>(App.Current as App, "Myprofile_Disable", "");
                MessagingCenter.Send<App, string>(App.Current as App, "changepassword_Disable", "");
                MessagingCenter.Send<App, string>(App.Current as App, "Logout_Disable", "");
                MessagingCenter.Send<App, string>(App.Current as App, "My_interested_Disable", "");
                Application.Current.Properties["Lang"] = "En";

            }
        }
        private async void Search_Clicked_1(object sender, EventArgs e)
        {
            //  popupLoginView.IsVisible = true;
            Get_City();
            // await Navigation.PushModalAsync(new Search_Page());
            await Navigation.PushAsync(new Search_Page());
            //  await Navigation.PushAsync(new MAP());

            // await InputBox(this.Navigation);
            // activityIndicator.IsRunning = true;
        }
        private void Category_Val_SelectedIndexChanged(object sender, EventArgs e)
        {
            var selectedItem = (Repository)Category_Val.SelectedItem;

            int categ = Convert.ToInt32(selectedItem.main_service_id);
            Application.Current.Properties["main_service_id"] = categ;


            Get_SubService(categ);


        }
        private void Sub_Category_val_SelectedIndexChanged(object sender, EventArgs e)
        {
            var selectedItem = (Repository)Sub_Category_val.SelectedItem;

            int Sub_categ = Convert.ToInt32(selectedItem.sub_service_id);
            Application.Current.Properties["sub_service_id"] = Sub_categ;

        }
        private void Region_val_SelectedIndexChanged(object sender, EventArgs e)
        {
            var selectedItem = (Repository)Region_val.SelectedItem;

            int Region_Id = Convert.ToInt32(selectedItem.region_id);
            Application.Current.Properties["Region_Id"] = Region_Id;

        }
        private void City_Val_SelectedIndexChanged(object sender, EventArgs e)
        {

            var selectedItem = (Repository)City_Val.SelectedItem;
            int City_Id = Convert.ToInt32(selectedItem.city_id);
            Application.Current.Properties["City_Id"] = City_Id;

            Get_Region(City_Id);


        }
        private async void Search_Result_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new Filter_Result());

        }
        public static Task<string> InputBox(INavigation navigation)
        {
            // wait in this proc, until user did his input 
            var tcs = new TaskCompletionSource<string>();

            var lblTitle = new Label { Text = "Title", HorizontalOptions = LayoutOptions.Center, FontAttributes = FontAttributes.Bold };
            var lblMessage = new Label { Text = "Enter new text:" };
            var txtInput = new Entry { Text = "" };

            var btnOk = new Button
            {
                Text = "Ok",
                WidthRequest = 100,
                BackgroundColor = Color.FromRgb(0.8, 0.8, 0.8),
            };
            btnOk.Clicked += async (s, e) =>
            {
                // close page
                var result = txtInput.Text;
                await navigation.PopModalAsync();
                // pass result
                tcs.SetResult(result);
            };

            var btnCancel = new Button
            {
                Text = "Cancel",
                WidthRequest = 100,
                BackgroundColor = Color.FromRgb(0.8, 0.8, 0.8)
            };
            btnCancel.Clicked += async (s, e) =>
            {
                // close page
                await navigation.PopModalAsync();
                // pass empty result
                tcs.SetResult(null);
            };

            var slButtons = new StackLayout
            {
                Orientation = StackOrientation.Horizontal,
                Children = { btnOk, btnCancel },
            };

            var layout = new StackLayout
            {
                Padding = new Thickness(0, 40, 0, 0),
                VerticalOptions = LayoutOptions.StartAndExpand,
                HorizontalOptions = LayoutOptions.CenterAndExpand,
                Orientation = StackOrientation.Vertical,
                Children = { lblTitle, lblMessage, txtInput, slButtons },
            };

            // create and show page
            var page = new ContentPage();
            page.Content = layout;

            navigation.PushModalAsync(page);
            // open keyboard
            txtInput.Focus();

            // code is waiting her, until result is passed with tcs.SetResult() in btn-Clicked
            // then proc returns the result
            return tcs.Task;
        }
        private async void Profile_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new My_Profile());
        }
        private async void notification_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new NotificationPage());
        }
        private async void More_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new MenuPage());
        }

        public async Task<List<Get_Slider_Property>> GetSliderPhoto(string uri)
        {

            List<Get_Slider_Property> repositories = null;
            try
            {
                using (HttpClient httpClient = new HttpClient(new HttpClientHandler()))
                {
                    using (HttpResponseMessage response = await httpClient.GetAsync(uri))
                    {
                        if (response.IsSuccessStatusCode)
                        {
                            string content = await response.Content.ReadAsStringAsync();
                            repositories = JsonConvert.DeserializeObject<List<Get_Slider_Property>>(content);
                            Home_ads_photo.ItemsSource = repositories;
                            // latitude.Text = repositories[0].latitude;
                        }
                    }
                }
            }
            catch (Exception ex)
            { }
            return repositories;
        }
    }
}