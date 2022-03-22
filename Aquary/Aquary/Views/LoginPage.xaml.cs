using Aquary.Models;
using Aquary.ViewModels;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Aquary.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class LoginPage : ContentPage
    {
        RestService _restService;
        public LoginPage()
        {
            InitializeComponent();
            this.BindingContext = new LoginViewModel();
        }
        protected async override void OnAppearing()
        {
            //Write the code of your page here
            base.OnAppearing();
            Email.Text = "";
            Password.Text = "";


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
            //   var My_Profile = Navigation.NavigationStack.FirstOrDefault(p => p.Title == "Page 2");
            // Navigation.RemovePage(My_Profile);
            //  if (My_Profile != null)
            //   {
            //     Navigation.RemovePage(My_Profile);
            //   }
           
              //  var _navigation = Application.Current.MainPage.Navigation;
              //  var _lastPage = _navigation.NavigationStack.LastOrDefault();
                //Remove last page
               // _navigation.RemovePage(_lastPage);
                //Go back 
               // _navigation.PopAsync();

          

        }
        private void Button_Clicked(object sender, EventArgs e)
        {
            Navigation.PushAsync(new Register());

        }

        private async void Forget_Password(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new Forget_Password());
        }

        public async void Button_Clicked_1(object sender, EventArgs e)
        {
           // if (Email.Text == "" || Password.Text == "")
            //{
              //  await DisplayAlert("Alert", "Please enter username / Password", "OK");

            //}
           // else
           // {
                string API = Constants.GitHubReposEndpoint1 + "login";
                string result = await login(API, Email.Text, Password.Text);
           // }
                
           
        }

        public async Task<string> login(string url, string username, string password)
        {
            var result="" ;
            try
            {
                var client = new HttpClient();
                Login_User users = new Login_User();
                users.email = Email.Text;
                users.password = Password.Text;

                string jsonData = JsonConvert.SerializeObject(users);
                // string jsonData = @"{""email"" : ""username"", ""password"" :  " + password + "";
                var content = new StringContent(jsonData, Encoding.UTF8, "application/json");
                HttpResponseMessage response = await client.PostAsync(url, content);

                 result = await response.Content.ReadAsStringAsync();

                // var Get_Result = JsonConvert.DeserializeObject(result);

                users = JsonConvert.DeserializeObject<Login_User>(result);

                if (users.result == "success")
                {
                    // await Navigation.PushAsync(new Main_Service());
               
                    Application.Current.Properties["register_id"] = users.register_id;
                    //   await Shell.Current.GoToAsync("//Main_Service", false);

                    //   Navigation.PushAsync(new Main_Service());
                    await Navigation.PopToRootAsync();
                }
                else
                {
                    await DisplayAlert("Alert", users.msg_en, "OK");
                }
                return result;
            }
            catch (Exception ex)
            {

            }
            return result;
        }

     
    }
    class Login_User
    {
        public string email { get; set; }
        public string password { get; set; }

        public string result { get; set; }
        public int error_code { get; set; }
        public string msg_en { get; set; }
        public string msg_ar { get; set; }
        public int register_id { get; set; }
        public string full_name { get; set; }
       
    }

   
}