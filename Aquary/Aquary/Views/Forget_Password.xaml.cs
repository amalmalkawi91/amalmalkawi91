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
    public partial class Forget_Password : ContentPage
    {
        public Forget_Password()
        {
            InitializeComponent();
            BindingContext = new Forget_Password_1();
            Email.Text = "";
        }

        private async void Button_Clicked(object sender, EventArgs e)
        {
            if (Email.Text == "" )
            {
                await DisplayAlert("Alert", "Please enter Email", "OK");

            }
            else
            {
                string API = Constants.GitHubReposEndpoint1 + "forgot_password";
                string result = await forget_1(API);
            }
        }
        public async Task<string> forget_1(string url)
        {

            var client = new HttpClient();
            forget_Parameters FP = new forget_Parameters();
            FP.email = Email.Text;


            string jsonData = JsonConvert.SerializeObject(FP);
            // string jsonData = @"{""email"" : ""username"", ""password"" :  " + password + "";
            var content = new StringContent(jsonData, Encoding.UTF8, "application/json");
            HttpResponseMessage response = await client.PostAsync(url, content);

            var result = await response.Content.ReadAsStringAsync();

            // var Get_Result = JsonConvert.DeserializeObject(result);
            Common_Parmeters CP = new Common_Parmeters();
            CP = JsonConvert.DeserializeObject<Common_Parmeters>(result);

            if (CP.result == "success")
            {
                Application.Current.Properties["email"] = Email.Text;
                Application.Current.Properties["register_id"] = CP.register_id;
                Navigation.PushAsync(new Verify_OTP());
            }

            else
            {
                await DisplayAlert("Alert", CP.msg_en, "OK");
            }
            return result;
        }


    }
       
    public class Forget_Password_1 : BaseViewModel
    {
        public Forget_Password_1()
        {
            Title = "Forget Password";
          
        }

    }
    class forget_Parameters
    {
        public string email { get; set; }
       
    }

    class Common_Parmeters
    {

        public int register_id { get; set; }
        public string result { get; set; }
        public int error_code { get; set; }
        public string msg_en { get; set; }
        public string msg_ar { get; set; }

    }
}