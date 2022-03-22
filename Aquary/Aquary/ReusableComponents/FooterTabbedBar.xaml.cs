using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using Aquary.Models;
using Aquary.ViewModels;
using Aquary.Views;
using Xamarin.Forms;

using Xamarin.Forms.Xaml;
namespace Aquary.ReusableComponents
{
  

    public partial class FooterTabbedBar : ContentView
    {
        public static readonly BindableProperty HomeIconSourceProperty = BindableProperty.Create(nameof(HomeIconSource),
            typeof(ImageSource), typeof(FooterTabbedBar));

        public ImageSource HomeIconSource
        {
            get => (ImageSource)GetValue(HomeIconSourceProperty);
            set
            {
                SetValue(HomeIconSourceProperty, value);
                SetHomeImageIconSource();
            }
        }

        public static readonly BindableProperty ProfileIconSourceProperty = BindableProperty.Create(nameof(ProfileIconSource),
            typeof(ImageSource), typeof(FooterTabbedBar));

        public ImageSource ProfileIconSource
        {
            get => (ImageSource)GetValue(ProfileIconSourceProperty);
            set
            {
                SetValue(ProfileIconSourceProperty, value);
                SetProfileImageIconSource();
            }
        }

        public static readonly BindableProperty NotificationsIconSourceProperty = BindableProperty.Create(nameof(NotificationsIconSource),
           typeof(ImageSource), typeof(FooterTabbedBar));

        public ImageSource NotificationsIconSource
        {
            get => (ImageSource)GetValue(NotificationsIconSourceProperty);
            set
            {
                SetValue(NotificationsIconSourceProperty, value);
                Icon_notification.Source = value;
            }
        }

        public FooterTabbedBar()
        {
            InitializeComponent();
        }

        async void HomeTapped(System.Object sender, System.EventArgs e)
        {
            LoadHomePage();
        }

        private async void LoadHomePage()
        {
            try
            {
                App.CurrentSelectedIndex = 0;
                SetHomeImageIconSource();

                var currentPage = Shell.Current.CurrentPage;
                if (currentPage is Main_Service)
                {
                    return;
                }
                else
                {
                    await Navigation.PopToRootAsync();

                }
            }
            catch (Exception ex)
            {


            }
        }

        private void SetHomeImageIconSource()
        {
            Device.BeginInvokeOnMainThread(() =>
            {
                Icon_Home.Source = "home.png";
                Icon_login.Source = "login.png";
                Icon_notification.Source = "notification.png";
                Icon_MORE.Source = "More.png";
            });
        }

        async void LoginTapped(System.Object sender, System.EventArgs e)
        {
            await LoadProfilePage();
        }

        private async Task LoadProfilePage()
        {
            try
            {

                App.CurrentSelectedIndex = 1;
                SetProfileImageIconSource();

                var currentPage = Shell.Current.CurrentPage;

                if (currentPage is My_Profile)
                {
                    return;
                }
                else
                {
                   
                        await Navigation.PushAsync(new My_Profile());
                 }
                  

                

            }

            catch (Exception ex)
            {

            }
        }

        private void SetProfileImageIconSource()
        {
            Icon_Home.Source = "icon_Home_Gray.png";
            Icon_login.Source = "icon_Login_blue.png";
            Icon_notification.Source = "notification.png";
            Icon_MORE.Source = "More.png";
        }

      

        async void AddTapped(System.Object sender, System.EventArgs e)
        {
            try
            {

                var currentPage = Navigation.NavigationStack.LastOrDefault();
                // if (currentPage is object && !nameof(currentPage).Equals(nameof(Main_Service_add_ads)))
                await App.Current.MainPage.Navigation.PushAsync(new Main_Service_add_ads());
            }
            catch (Exception ex)
            {

            }
        }
        async void NotificationsTapped(System.Object sender, System.EventArgs e)
        {
            await LoadNotificationsPage();
        }

        private async Task LoadNotificationsPage()
        {
            try
            {
                App.CurrentSelectedIndex = 2;
                SetNotificationsImageIconSource();
                var currentPage = Shell.Current.CurrentPage;

                if (currentPage is NotificationPage)
                {
                    return;
                }
                else
                {
                    await Navigation.PushAsync(new NotificationPage());

                }
            }
            catch (Exception ex)
            {

            }
        }

        private void SetNotificationsImageIconSource()
        {
            Icon_Home.Source = "icon_Home_Gray.png";
            Icon_login.Source = "login.png";
            Icon_notification.Source = "notification.png";
            Icon_MORE.Source = "More.png";
        }
        async void MoreTapped(System.Object sender, System.EventArgs e)
        {
            await LoadMorePage();
        }

        private async Task LoadMorePage()
        {
            try
            {
                App.CurrentSelectedIndex = 3;

                var currentPage = Shell.Current.CurrentPage;

                if (currentPage is MenuPage)
                {
                    return;
                }
                else
                {
                    await Navigation.PushAsync(new MenuPage());

                }

            }
            catch (Exception ex)
            {

            }

        }
    }
}
