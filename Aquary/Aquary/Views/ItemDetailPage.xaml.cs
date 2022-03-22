using Aquary.ViewModels;
using System.ComponentModel;
using Xamarin.Forms;

namespace Aquary.Views
{
    public partial class ItemDetailPage : ContentPage
    {
        public ItemDetailPage()
        {
            InitializeComponent();
            BindingContext = new ItemDetailViewModel();
        }
    }
}