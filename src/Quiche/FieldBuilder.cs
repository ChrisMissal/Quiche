namespace Quiche
{
    using System;

    internal class FieldBuilder
    {
        public Field Build(object value, string field, Func<string, string> fieldConverter)
        {
            var key = fieldConverter(field);

            return new Field(key, value != null ? value.ToString() : "");
        }
    }
}