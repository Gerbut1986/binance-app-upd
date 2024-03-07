using System;
using System.ComponentModel;
using System.Linq;
using System.Windows.Markup;

namespace BinanceOptionsApp
{
    public class EnumToItemsSource : MarkupExtension
    {
        private readonly Type _type;
        public EnumToItemsSource(Type type)
        {
            _type = type;
        }

        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            return Enum.GetValues(_type).Cast<Enum>().Select(value => new
            {
                (Attribute.GetCustomAttribute(value.GetType().GetField(value.ToString()), typeof(DescriptionAttribute)) as DescriptionAttribute).Description,
                value
            }).OrderBy(item => item.value).ToList();
        }
    }
    public class EnumLangToItemsSource : MarkupExtension
    {
        private readonly Type _type;
        public EnumLangToItemsSource(Type type)
        {
            _type = type;
        }

        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            return Enum.GetValues(_type).Cast<Enum>().Select(value => new
            {
                Description=App.LanguageKey((Attribute.GetCustomAttribute(value.GetType().GetField(value.ToString()), typeof(DescriptionAttribute)) as DescriptionAttribute).Description),
                value
            }).OrderBy(item => item.value).ToList();
        }
    }
}
