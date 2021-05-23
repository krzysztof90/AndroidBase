using Android.Content;
using Android.Util;
using Android.Widget;
using System;

namespace AndroidBase.UI
{
    public class AutoGridLayout : GridLayout
    {
        public int DefaultColumnCount { get; set; }
        public float ColumnWidth { get; set; }

        public AutoGridLayout(Context context) : base(context)
        {
            Init(null, 0);
        }

        public AutoGridLayout(Context context, IAttributeSet attrs) : base(context, attrs)
        {
            Init(attrs, 0);
        }

        public AutoGridLayout(Context context, IAttributeSet attrs, int defStyleAttr) : base(context, attrs, defStyleAttr)
        {
            Init(attrs, defStyleAttr);
        }

        private void Init(IAttributeSet attrs, int defStyleAttr)
        {
            Android.Content.Res.TypedArray typedArray = Context.ObtainStyledAttributes(attrs, new int[] { Resource.Attribute.columnCount, Resource.Attribute.columnWidth });
            DefaultColumnCount = typedArray.GetInt(0, 1);
            ColumnWidth = typedArray.GetDimension(1, 150);
            typedArray.Recycle();

            ColumnCount = 1;
        }

        protected override void OnMeasure(int widthSpec, int heightSpec)
        {
            base.OnMeasure(widthSpec, heightSpec);

            int width = MeasureSpec.GetSize(widthSpec);
            if (ColumnWidth > 0 && width > 0)
            {
                int totalSpace = width - PaddingRight - PaddingLeft;
                int columnCount = Math.Max(1, (int)((float)totalSpace / ColumnWidth));
                ColumnCount = columnCount;
            }
            else
            {
                ColumnCount = DefaultColumnCount;
            }
        }
    }
}