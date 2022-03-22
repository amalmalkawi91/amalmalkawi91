using Aquary.Models;
using Aquary.ViewModels;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
namespace Aquary.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Register : ContentPage
    {
        public Register()
        {
            InitializeComponent();
            BindingContext = new Register_1();
            
        }
        protected async override void OnAppearing()
        {
            //Write the code of your page here
            base.OnAppearing();
            Email.Text = "";
            Password.Text = "";
            Phone.Text = "";



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
        }

            private async void Button_Clicked(object sender, EventArgs e)
        {
            if (Name.Text == "" || Phone.Text == "" || Password.Text == "")
            {
                await DisplayAlert("Alert", "Please Fill all fields", "OK");

            }
            else if (Password.Text != Confirm_Password.Text)
            {
                await DisplayAlert("Alert", "Password and confirm password not match", "OK");
            }
            else
            {
                string API = Constants.GitHubReposEndpoint1 + "register";
                string result = await Register_1(API);
            }

           

          

        
        }

        public async Task<string> Register_1(string url)
        {

            var client = new HttpClient();
            Register_Parameters RP = new Register_Parameters();
            RP.email = Email.Text;
            RP.password = Password.Text;
            RP.phone = Phone.Text;
            RP.full_name = Name.Text;
            RP.operation = "I";

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
                Application.Current.Properties["email"] = Email.Text;
                await Navigation.PushAsync(new Verify_Email());
            }
            else
            {
                await DisplayAlert("Alert", RU.msg_en, "OK");
            }
            return result;
        }

        private async void Login_Clicked_1(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new LoginPage());
        }
    }



    public class Register_1 : BaseViewModel
    {
        public Register_1()
        {
            Title = "REGISTER";

          
        }

    }

    class Register_User
    {
       
        public int register_id { get; set; }
        public string result { get; set; }
        public int error_code { get; set; }
        public string msg_en { get; set; }
        public string msg_ar { get; set; }
      
    }
    class Register_Parameters
    {
        public string email { get; set; }
        public string password { get; set; }
        public string full_name { get; set; }
        public string phone { get; set; }
       public string operation { get; set; }

    }

}