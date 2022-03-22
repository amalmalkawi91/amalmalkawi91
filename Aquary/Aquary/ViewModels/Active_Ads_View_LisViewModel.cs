using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Threading.Tasks;
using Aquary.Models;
using Aquary.Views;
using Xamarin.Forms;
namespace Aquary.ViewModels
{
    class Active_Ads_View_LisViewModel : BaseViewModel
    {
        public Active_Ads_View_LisViewModel()
        {
            Title = "Advertisment";
            // Items = new ObservableCollection<Item>();
            // LoadItemsCommand = new Command(async () => await ExecuteLoadItemsCommand());

            // ItemTapped = new Command<Item>(OnItemSelected);

            //AddItemCommand = new Command(OnAddItem);
        }
    }
}
