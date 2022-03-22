using Aquary.ViewModels;
using Aquary.Views;
using System;
using System.Collections.Generic;
using Xamarin.Forms;

namespace Aquary
{
    public partial class AppShell : Xamarin.Forms.Shell
    {
        public AppShell()
        {
            InitializeComponent();
            Routing.RegisterRoute(nameof(ItemDetailPage), typeof(ItemDetailPage));
            Routing.RegisterRoute(nameof(NewItemPage), typeof(NewItemPage));

            //MyAds.IsVisible = false;
            Routing.RegisterRoute(nameof(NewItemPage), typeof(NewItemPage));

            MessagingCenter.Subscribe<App, string>(App.Current, "MyFav_Disable", (snd, arg) =>
            {
                Device.BeginInvokeOnMainThread(() => {
                    //Navigation.PushAsync(new NavigationPage(new DetailsInfo(arg)));
                 //   MyFav.IsVisible = false;
                });
            });
            MessagingCenter.Subscribe<App, string>(App.Current, "MyFav_enable", (snd, arg) =>
            {
                Device.BeginInvokeOnMainThread(() => {
                    //Navigation.PushAsync(new NavigationPage(new DetailsInfo(arg)));
                 //   MyFav.IsVisible = true;
                });
            });


            MessagingCenter.Subscribe<App, string>(App.Current, "Myads_Disable", (snd, arg) =>
            {
                Device.BeginInvokeOnMainThread(() => {
                  //  MyAds.IsVisible = false;
                });
            });
            MessagingCenter.Subscribe<App, string>(App.Current, "Myads_enable", (snd, arg) =>
            {
                Device.BeginInvokeOnMainThread(() => {
                   // MyAds.IsVisible = true;
                });
            });


            MessagingCenter.Subscribe<App, string>(App.Current, "Myprofile_Disable", (snd, arg) =>
            {
                Device.BeginInvokeOnMainThread(() => {
                   // Myprofile.IsVisible = false;
                });
            });
            MessagingCenter.Subscribe<App, string>(App.Current, "Myprofile_enable", (snd, arg) =>
            {
                Device.BeginInvokeOnMainThread(() => {
                   // Myprofile.IsVisible = true;
                });
            });

            MessagingCenter.Subscribe<App, string>(App.Current, "changepassword_Disable", (snd, arg) =>
            {
                Device.BeginInvokeOnMainThread(() => {
                //    changepassword.IsVisible = false;
                });
            });
            MessagingCenter.Subscribe<App, string>(App.Current, "changepassword_enable", (snd, arg) =>
            {
                Device.BeginInvokeOnMainThread(() => {
                  //  changepassword.IsVisible = true;
                });
            });

            MessagingCenter.Subscribe<App, string>(App.Current, "My_interested_Disable", (snd, arg) =>
            {
                Device.BeginInvokeOnMainThread(() => {
                  //  My_interested.IsVisible = false;
                });
            });
            MessagingCenter.Subscribe<App, string>(App.Current, "My_interested_enable", (snd, arg) =>
            {
                Device.BeginInvokeOnMainThread(() => {
                  //  My_interested.IsVisible = true;
                });
            });

            MessagingCenter.Subscribe<App, string>(App.Current, "Logout_Disable", (snd, arg) =>
            {
                Device.BeginInvokeOnMainThread(() => {
                  
                    Logout.Text = "Login";
                });
            });
            MessagingCenter.Subscribe<App, string>(App.Current, "Logout_enable", (snd, arg) =>
            {
                Device.BeginInvokeOnMainThread(() => {
                    Logout.Text = "Logout";
                });
            });

        }

        private async void OnMenuItemClicked(object sender, EventArgs e)
        {
            await Shell.Current.GoToAsync("//LoginPage");
        }

    }
}
