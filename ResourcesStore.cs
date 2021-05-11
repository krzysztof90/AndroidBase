using Android.Views;
using System;

namespace AndroidBase
{
    public static class ResourcesStore
    {
        public static int allowedRangeText;
        public static int exceptRangeText;

        public static int viewLineNumberLayout;
        public static int viewLineCheckboxLayout;
        public static int listExtensionModuleGroupLayout;
        public static int listExtensionModuleItemLayout;
        public static int listExtensionModuleItemRadioButtonsLayout;

        public static int controlLabelId;
        public static int controlNumberId;
        public static int controlCheckBoxId;
        public static int listTitleId;
        public static int expandedListItemId;
        public static int expandedListItemImageId;

        public static int labelColorAttribute;
        public static int labelValueAttribute;

        //TODO problem with references
        public static Func<LayoutInflater, int, View> InflateMethod;
    }
}
