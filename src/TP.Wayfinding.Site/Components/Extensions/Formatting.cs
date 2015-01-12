using System;
using System.Collections.Generic;
using System.ComponentModel;
using Fasterflect;
using Newtonsoft.Json;

namespace TP.Wayfinding.Site.Components.Extensions
{
    public static class Formatting
    {
        public static string ToDescription(this Enum enumValue)
        {
            var attribute = enumValue.Attribute<DescriptionAttribute>();
            return attribute != null ? attribute.Description : enumValue.ToString();
        }

        public static string ToJson(this object source)
        {
            return JsonConvert.SerializeObject(source);
        }
    }
}