namespace AndroidBase.Attributes
{
    public abstract class SettingNameBaseAttribute : DescriptionResourceAttribute
    {
        public string Name { get; set; }

        public SettingNameBaseAttribute(string name, int descriptionResource) : base(descriptionResource)
        {
            Name = name;
        }
    }
}
