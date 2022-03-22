using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Aquary.Models;
using Android.Graphics;

using Xamarin.Forms.Platform.Android;
using Google.Android.Material.BottomNavigation;

namespace Aquary.Droid
{
    class TodoShellItemRenderer : ShellItemRenderer
    {
        FrameLayout _shellOverlay ;
        BottomNavigationView _bottomView;

        public TodoShellItemRenderer(IShellContext shellContext) : base(shellContext)
        {
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            var outerlayout = base.OnCreateView(inflater, container, savedInstanceState);
            _bottomView = outerlayout.FindViewById<BottomNavigationView>(Resource.Id.bottomtab_tabbar);
            _shellOverlay = outerlayout.FindViewById<FrameLayout>(Resource.Id.bottomtab_tabbar_container);
          //  _bottomView.SetOnClickListener(new IOnClickListener(
            //void onClick(View v)
            //{

           // }
          //  ));
          //  _bottomView.SetOnNavigationItemSelectedListener(new BottomNavigationView.IOnNavigationItemSelectedListener() 
            //bool OnNavigationItemSelected(IMenuItem item){
              //  int id = item.ItemId();

//                switch (id)
  //              {
    //                case ShellItem.Id.menu Resource.Sh.menu_pr:

      //                      break;
        //        }
          //      return true;
            //    }
            //);
            if (ShellItem is TodoTabBar todoTabBar && todoTabBar.LargeTab != null)
               SetupLargeTab();

            return outerlayout;
        }

        public async void SetupLargeTab()
        {
            var todoTabBar = (TodoTabBar)ShellItem;
            var layout = new FrameLayout(Context);

           var imageHandler = todoTabBar.LargeTab.Icon.GetHandler();
              Bitmap bitmap = await imageHandler.LoadImageAsync(todoTabBar.LargeTab.Icon, Context);
              var image = new ImageView(Context);
            image.SetImageBitmap(bitmap);

             layout.AddView(image);

            var lp = new FrameLayout.LayoutParams(300, 300);
            _bottomView.Measure((int)MeasureSpecMode.Unspecified, (int)MeasureSpecMode.Unspecified);
            lp.BottomMargin = _bottomView.MeasuredHeight / 2;

            layout.LayoutParameters = lp;

            _shellOverlay.RemoveAllViews();
           _shellOverlay.AddView(layout);
        }
    }
}