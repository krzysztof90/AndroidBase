using Android.Content;
using Android.Widget;
using System;

namespace AndroidBase.Tools
{
    public static class AndroidValidateUtils
    {
        public class ResourcesFormatData
        {
            public readonly Context Context;
            public int ResourceId { get; set; }
            public object[] Args { get; set; }

            public string Message => String.Format(Context.Resources.GetString(ResourceId), Args);

            public ResourcesFormatData(Context context, int resourceId, params object[] args)
            {
                Context = context;
                ResourceId = resourceId;
                Args = args;
            }
        }

        public static bool CheckFailed(Context context, string text)
        {
            Toast.MakeText(context, text, ToastLength.Long).Show();
            return false;
        }
    }
}