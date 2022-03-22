using Aquary.Models;
using Aquary.Services;
using Aquary.Views;
using Phoneword;
using Plugin.Multilingual;
using System;
using System.Linq;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Aquary
{
    public partial class App : Application
    {
        public static int CurrentSelectedIndex { get; set; }
        public static int selectedLanguageForIntireUse { get; set; }
        public  bool Sale_Section_Selected { get; set; }
        public  bool Rent_Section_Selected { get; set; }
        public  bool Decoration_Section_Selected { get; set; }
        public  bool Maintenance_Section_Selected { get; set; }

        public App()
        {
            InitializeComponent();

            DependencyService.Register<MockDataStore>();
          //  MainPage = new MainPage();
            MainPage = new AppShell();
            InitializeApp();
            CurrentSelectedIndex = 0;
            //  MainPage = new ImageSelectionPage();
            // MainPage = new MAP();
            // MainPage = new LoginPage();
            // Application.Current.MainPage = new NavigationPage(new Main_Service ());
            // Application.Current.MainPage = new NavigationPage(new FlyoutPage1());


        }

        private async static void InitializeApp()
        {
            var selectedLanguage = await Xamarin.Essentials.SecureStorage.GetAsync(Constants.SelctedLanguage);

            if (selectedLanguage is null || selectedLanguage.ToString() == "En")
            {
                //
                CrossMultilingual.Current.CurrentCultureInfo = CrossMultilingual.Current.NeutralCultureInfoList.ToList().First(element => element.EnglishName.Contains("En"));
                AppResources.Culture = CrossMultilingual.Current.CurrentCultureInfo;

            }
            else
            {
                CrossMultilingual.Current.CurrentCultureInfo = CrossMultilingual.Current.NeutralCultureInfoList.ToList().First(element => element.EnglishName.Contains("Ar"));
                AppResources.Culture = CrossMultilingual.Current.CurrentCultureInfo;

            }
        }
        protected async override void OnStart()
        {
           
        }

        protected override void OnSleep()
        {

        }

        protected override void OnResume()
        {
        }
    }
}
