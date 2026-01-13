using System.ComponentModel;
using System.Reflection;

namespace BB.CR.Providers.Extensions
{
    public static class EnumExtension
    {
        public static List<T> ToValueList<T>() => Enum.GetValues(typeof(T)).Cast<T>().ToList();

        public static string GetDescription(this Enum enumValue)
        {
            return enumValue.GetType()
                       .GetMember(enumValue.ToString())
                       .First()
                       .GetCustomAttribute<DescriptionAttribute>()?
                       .Description ?? string.Empty;
        }

        public static List<KeyValuePair<T, string>> ToList<T>() where T : Enum
        {
            var enumList = new List<KeyValuePair<T, string>>();
            foreach (T item in Enum.GetValues(typeof(T)))
            {
                enumList.Add(new KeyValuePair<T, string>(item, item.GetDescription()));
            }
            return enumList;
        }
    }
}
