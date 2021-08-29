using Android.Content;
using Android.Content.Res;
using Android.Util;
using Android.Views;
using Android.Widget;
using System;

namespace AndroidBase.UI
{
    public interface IControlViewBase
    {
        void Initialize();
        void RestoreValue();
    }

    public abstract class ControlViewBase<T> : LinearLayout, IControlViewBase where T : struct
    {
        public T DefaultValue { get; set; }
        protected T? StoredValue { get; private set; }
        public Action OnValueChange;

        protected abstract int ResourceLayout { get; }
        protected abstract TextView LabelControl { get; }
        protected abstract View InputControl { get; }
        protected abstract void CreateControls();
        protected abstract void SetControlsProperties();
        protected abstract void SetValue(T? value);
        protected abstract T? GetValue();
        public abstract void SetFocus();

        public new bool Enabled { get => InputControl.Enabled; set => InputControl.Enabled = value; }

        public ControlViewBase(Context context, IAttributeSet attrs) : base(context, attrs)
        {
            Inflate(context, ResourceLayout, this);

            //Must be alphabetically
            TypedArray typedArray = context.ObtainStyledAttributes(attrs, new int[] { Resource.Attribute.labelColor, Resource.Attribute.labelValue });
            string label = typedArray.GetText(1);
            ColorStateList color = typedArray.GetColorStateList(0);
            typedArray.Recycle();

            DefaultValue = default;

            CreateControls();
            SetControlsProperties();
            SetLabel(label);
            if (Android.OS.Build.VERSION.SdkInt >= Android.OS.BuildVersionCodes.NMr1)
                SetTooltip(label);
            SetColor(color);
        }

        public void Initialize()
        {
            StoredValue = null;
        }

        public void SetLabel(string label)
        {
            LabelControl.Text = label;
        }

        public void SetTooltip(string label)
        {
            LabelControl.TooltipText = label;
        }

        public void SetColor(ColorStateList color)
        {
            if (color != null)
            {
                LabelControl.SetTextColor(color);
                if (color.DefaultColor == -658699 || color.DefaultColor == -1)
                    LabelControl.SetShadowLayer(1, 1, 1, Android.Graphics.Color.Black);
            }
        }

        protected void AssignStoredValue()
        {
            StoredValue = GetValue();
        }

        public void RestoreValue()
        {
            SetValue(StoredValue);
        }

        public void ResetValue()
        {
            SetValue(null);
            AssignStoredValue();
        }

        public T Value
        {
            get
            {
                return (Enabled && StoredValue != null) ? (T)StoredValue : DefaultValue;
            }
            set
            {
                SetValue(value);
                AssignStoredValue();
            }
        }
    }
}