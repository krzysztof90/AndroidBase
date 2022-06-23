using Android.Content;
using Android.Util;
using Android.Views;
using Android.Widget;
using System;

namespace AndroidBase.UI
{
    public class ControlCheckBoxView : ControlViewBase<bool>
    {
        private CheckBox scoreLineCheckBox;

        public ControlCheckBoxView(Context context) : base(context)
        {
        }

        public ControlCheckBoxView(Context context, IAttributeSet attrs) : base(context, attrs)
        {
        }

        protected override int ResourceLayout => Resource.Layout.view_line_checkbox;

        protected override TextView LabelControl => scoreLineCheckBox;
        protected override View InputControl => scoreLineCheckBox;

        protected override void CreateControls()
        {
            scoreLineCheckBox = FindViewById<CheckBox>(Resource.Id.controlCheckBox);
        }

        protected override void SetControlsProperties()
        {
            scoreLineCheckBox.Click += new EventHandler((object sender, EventArgs e) =>
            {
                AssignStoredValue();
                OnValueChange?.Invoke();
            });
        }

        protected override void SetValue(bool? value)
        {
            scoreLineCheckBox.Checked = (value == true);
        }

        protected override bool? GetValue()
        {
            if (this.Visibility == ViewStates.Gone)
                return null;
            return scoreLineCheckBox.Checked;
        }

        public override void SetFocus()
        {
            scoreLineCheckBox.RequestFocus();
        }
    }
}