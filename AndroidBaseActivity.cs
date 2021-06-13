using Android.OS;
using Android.Views;
using AndroidX.AppCompat.App;
using AndroidX.AppCompat.Widget;
using System;
using System.Collections.Generic;

namespace AndroidBase
{
    public abstract class AndroidBaseActivity<T> : AppCompatActivity where T : AndroidBaseApplication
    {
        protected new T Application => (T)base.Application;

        protected abstract int ToolbarResource { get; }
        protected abstract int MainMenuResource { get; }
        protected virtual bool ShowBackButton => true;
        protected abstract Dictionary<int, Action> MenuActions { get; }

        protected abstract int ContentView { get; }

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(ContentView);
            Toolbar toolbar = FindViewById<Toolbar>(ToolbarResource);
            SetSupportActionBar(toolbar);
            SupportActionBar.SetDisplayHomeAsUpEnabled(ShowBackButton);
            //SupportActionBar.SetDisplayShowHomeEnabled(ShowBackButton);
        }

        public override bool OnCreateOptionsMenu(IMenu menu)
        {
            MenuInflater.Inflate(MainMenuResource, menu);
            return true;
        }

        public override bool OnOptionsItemSelected(IMenuItem item)
        {
            int id = item.ItemId;

            if (MenuActions.ContainsKey(id))
            {
                MenuActions[id].Invoke();
                return true;
            }

            if (id == Android.Resource.Id.Home)
            {
                OnBackPressed();
            }

            return base.OnOptionsItemSelected(item);
        }
    }
}