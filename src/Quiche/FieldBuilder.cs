namespace Quiche
{
    using System;
    using System.Web;

    internal class FieldBuilder
    {
        public string Build(object value, string field, Func<string, string> fieldConverter)
        {
            var url = value != null ? value.ToString() : "";
            var fieldValue = fieldConverter(field);

            return string.Format("{0}={1}&", fieldValue, HttpUtility.UrlEncode(url));
        }
    }
}