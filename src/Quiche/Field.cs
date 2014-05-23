namespace Quiche
{
    using System.Web;

    internal class Field
    {
        internal string Key { get; set; }
        internal string Value { get; set; }

        internal Field(string key, string value)
        {
            Key = key;
            Value = value;
        }

        public override string ToString()
        {
            return string.Format("{0}={1}&", Key, HttpUtility.UrlEncode(Value));
        }
    }
}