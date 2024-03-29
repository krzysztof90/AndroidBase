﻿using Android.App;
using Android.Content;
using System;

namespace AndroidBase
{
    public abstract class AndroidBaseApplication : Application
    {
        public AndroidBaseApplication(IntPtr javaReference, Android.Runtime.JniHandleOwnership transfer) : base(javaReference, transfer)
        {
        }
        public override void OnCreate()
        {
            base.OnCreate();
        }

        protected void NewActivity(Type activityType, params ActivityFlags[] activityFlags)
        {
            Intent intent = new Intent(ApplicationContext, activityType);
            intent.AddFlags(ActivityFlags.NewTask);
            foreach (ActivityFlags flags in activityFlags)
                intent.AddFlags(flags);
            StartActivity(intent);
        }
    }
}