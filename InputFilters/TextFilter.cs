using Android.Content;
using Android.Text;
using AndroidBase.Options;
using AndroidBase.Tools;
using Java.Lang;

namespace AndroidBase.InputFilters
{
    public abstract class TextFilter<SettingsType> : Java.Lang.Object, IInputFilter where SettingsType : struct, System.IComparable, System.IFormattable, System.IConvertible
    {
        protected readonly Context Context;
        protected readonly SettingsType? ValidationSettingsType;

        protected abstract bool ValidateNumber(string text, bool validateFull, int? defaultValue = null, string fieldName = null);
        protected abstract string ValidationMessage { get; }

        public TextFilter(Context context, SettingsType? validationSettingsType)
        {
            Context = context;
            ValidationSettingsType = validationSettingsType;
        }

        //TODO pełna walidacja przy lost focus
        ICharSequence IInputFilter.FilterFormatted(ICharSequence source, int start, int end, ISpanned dest, int dstart, int dend)
        {
            string newString = dest.ToString().Substring(0, dstart) + source.ToString().Substring(start, end) + dest.ToString().Substring(dend);

            return ValidateNumberRange(newString, false) ? null : new Java.Lang.String(System.String.Empty);
        }

        public bool ValidateNumberRange(string text, bool validateFull, int? defaultValue = null, string fieldName = null)
        {
            return (ValidationSettingsType != null && !AndroidSettings<SettingsType>.GetBool((SettingsType)ValidationSettingsType))
                || ValidateNumber(text, validateFull, defaultValue, fieldName);
        }

        protected bool ValidateEmptyValue(string text, bool validateFull, int? defaultValue, string fieldName)
        {
            return System.String.IsNullOrEmpty(text) && defaultValue != null && ValidateNumberRange(((int)defaultValue).ToString(), validateFull, null, fieldName);
        }

        protected bool ValidationError(string fieldName)
        {
            StringBuilder message = new StringBuilder();
            if (fieldName != null)
                message.Append($"{fieldName}: ");
            message.Append(ValidationMessage);
            return AndroidValidateUtils.CheckFailed(Context, message.ToString());
        }
    }
}