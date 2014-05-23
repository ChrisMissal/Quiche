namespace Quiche
{
    internal class FieldBuilder
    {
        internal Field Build(object value, string field, FieldConverter fieldConverter)
        {
            var key = fieldConverter.Convert(field);

            return new Field(key, value != null ? value.ToString() : "");
        }
    }
}