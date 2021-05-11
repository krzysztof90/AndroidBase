using Android.Content;
using Android.Widget;

namespace AndroidBase.Tools
{
    public static class AndroidValidateUtils
    {
        public static bool CheckFailed(Context context, string text)
        {
            Toast.MakeText(context, text, ToastLength.Long).Show();
            return false;
        }
    }
}