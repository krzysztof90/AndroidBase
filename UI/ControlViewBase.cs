﻿using Android.Content;
using Android.Content.Res;
using Android.Util;
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
        protected abstract void CreateControls();
        protected abstract void SetControlsProperties();
        public abstract void SetLabel(string label);
        public abstract void SetTooltip(string label);
        public abstract void SetColor(ColorStateList color);
        protected abstract void SetValue(T? value);
        protected abstract T? GetValue();
        public abstract void SetFocus();
        public abstract new bool Enabled { get; set; }

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