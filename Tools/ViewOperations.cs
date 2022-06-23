using Android.Views;

namespace AndroidBase.Tools
{
    public static class ViewOperations
    {
        public static void SetVisibility(this View view, bool condition)
        {
            view.Visibility = condition ? ViewStates.Visible : ViewStates.Gone;
        }
    }
}