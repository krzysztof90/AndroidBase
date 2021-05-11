namespace AndroidBase.Attributes
{
    public class SettingNameBoolAttribute : SettingNameBaseAttribute
    {
        public bool DefaultValue { get; set; }

        public SettingNameBoolAttribute(string name, bool defaultValue, int descriptionResource) : base(name, descriptionResource)
        {
            DefaultValue = defaultValue;
        }
    }
}