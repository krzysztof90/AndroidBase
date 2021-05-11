using AndroidBase.Attributes;
using AndroidBase.Tools;
using System;
using Xamarin.Essentials;

namespace AndroidBase.Options
{
    public static class AndroidSettings<SettingsType> where SettingsType : struct, IComparable, IFormattable, IConvertible
    {
        public static void Set(SettingsType settingsType, bool value)
        {
            Preferences.Set(settingsType.GetEnumAttribute<SettingsType, SettingNameBoolAttribute>().Name, value);
        }
        public static void Set(SettingsType settingsType, string value)
        {
            Preferences.Set(settingsType.GetEnumAttribute<SettingsType, SettingNameStringAttribute>().Name, value);
        }

        public static bool GetBool(SettingsType settingsType)
        {
            SettingNameBoolAttribute settingNameAttribute = settingsType.GetEnumAttribute<SettingsType, SettingNameBoolAttribute>();
            return Preferences.Get(settingNameAttribute.Name, settingNameAttribute.DefaultValue);
        }
        public static string GetString(SettingsType settingsType)
        {
            SettingNameStringAttribute settingNameAttribute = settingsType.GetEnumAttribute<SettingsType, SettingNameStringAttribute>();
            return Preferences.Get(settingNameAttribute.Name, settingNameAttribute.DefaultValue);
        }
    }
}