using Android.Content;
using Android.Text;
using Android.Util;
using Android.Views;
using Android.Widget;
using AndroidBase.InputFilters;
using Java.Lang;
using System;
using System.Collections.Generic;
using System.Linq;

namespace AndroidBase.UI
{
    public class ControlNumberView : ControlViewBase<int>
    {
        private TextView textView;
        private EditText editText;

        public ControlNumberView(Context context) : base(context)
        {
        }

        public ControlNumberView(Context context, IAttributeSet attrs) : base(context, attrs)
        {
        }

        protected override int ResourceLayout => Resource.Layout.view_line_number;

        protected override TextView LabelControl => textView;
        protected override View InputControl => editText;

        protected override void CreateControls()
        {
            textView = FindViewById<TextView>(Resource.Id.controlLabel);
            editText = FindViewById<EditText>(Resource.Id.controlNumber);
        }

        protected override void SetControlsProperties()
        {
            editText.TextChanged += new EventHandler<TextChangedEventArgs>((object sender, TextChangedEventArgs e) =>
            {
                bool restore = false;
                bool handleTextChanged = false;
                foreach (StackTraceElement element in Thread.CurrentThread().GetStackTrace())
                {
                    if (element.ClassName == "android.widget.TextView" && element.MethodName == "onRestoreInstanceState")
                    {
                        restore = true;
                        break;
                    }
                    if (element.ClassName == "android.widget.TextView" && element.MethodName == "handleTextChanged")
                    {
                        handleTextChanged = true;
                    }
                }

                if (!restore)
                {
                    AssignStoredValue();
                    if (!handleTextChanged)
                        editText.SetSelection(editText.Text.Length);
                }

                OnValueChange?.Invoke();
            });
        }

        protected override void SetValue(int? value)
        {
            if (value == null)
                editText.Text = System.String.Empty;
            else
                editText.Text = value.ToString();
        }

        protected override int? GetValue()
        {
            string text = editText.Text;
            if (text.Length == 0 || this.Visibility == ViewStates.Gone)
                return null;
            return Int32.Parse(text);
        }

        public override void SetFocus()
        {
            editText.RequestFocus();
        }

        public void SetNumberRange<SettingsType>(int min, int max, SettingsType? validationSettingsType) where SettingsType : struct, System.IComparable, System.IFormattable, System.IConvertible
        {
            List<IInputFilter> filters = editText.GetFilters().ToList();
            filters.Add(new MinMaxFilter<SettingsType>(Context, min, max, validationSettingsType));
            editText.SetFilters(filters.ToArray());
        }

        public void SetNumberExceptions<SettingsType>(List<int> numbers, SettingsType? validationSettingsType) where SettingsType : struct, System.IComparable, System.IFormattable, System.IConvertible
        {
            List<IInputFilter> filters = editText.GetFilters().ToList();
            filters.Add(new NumberFilter<SettingsType>(Context, numbers, validationSettingsType));
            editText.SetFilters(filters.ToArray());
        }

        public bool ValidateNumberRange<SettingsType>() where SettingsType : struct, System.IComparable, System.IFormattable, System.IConvertible
        {
            if (editText != null)
                foreach (IInputFilter inputFilter in editText.GetFilters())
                    //TODO do textView.Text dołączone nowe property ValidationMessage z przypisaną nazwą gracza
                    if (inputFilter is MinMaxFilter<SettingsType> minMaxFilter && !minMaxFilter.ValidateNumberRange(editText.Text, true, DefaultValue, textView.Text))
                        return false;
                    else if (inputFilter is NumberFilter<SettingsType> numberFilter && !numberFilter.ValidateNumberRange(editText.Text, true, DefaultValue, textView.Text))
                        return false;
            return true;
        }
    }
}