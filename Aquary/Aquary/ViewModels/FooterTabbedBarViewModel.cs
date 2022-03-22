using System;
using System.Windows.Input;
using Aquary.Views;
using Xamarin.Forms;

namespace Aquary.ViewModels
{
    public class FooterTabbedBarViewModel:BaseViewModel
    {
        public ICommand OpenHomeCommand { get; }
        public ICommand OpenLoginCommand { get; }
        public ICommand OpenAddCommand { get; }
        public ICommand OpenNotificationsCommand { get; }
        public ICommand OpenMoreCommand { get; }

        public FooterTabbedBarViewModel()
        {
            OpenHomeCommand = new Command(async () =>
            {
            });

            OpenLoginCommand = new Command(async () =>
            {
            });

            OpenAddCommand = new Command(async () =>
            {
            });

            OpenNotificationsCommand = new Command(async () =>
            {
            });

            OpenMoreCommand = new Command(async () =>
            {
               await App.Current.MainPage.Navigation.PushAsync(new MenuPage());
            });
        }
    }
}
