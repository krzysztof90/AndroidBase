namespace AndroidBase.Attributes
{
    public class SettingNameStringAttribute : SettingNameBaseAttribute
    {
        public string DefaultValue { get; set; }

        public SettingNameStringAttribute(string name, string defaultValue, int descriptionResource) : base(name, descriptionResource)
        {
            DefaultValue = defaultValue;
        }
    }
}