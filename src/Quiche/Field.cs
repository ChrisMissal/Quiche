namespace Quiche
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Web;

    internal class MultiField : Field
    {
        private readonly IEnumerable<Field> _fields;

        internal MultiField(IEnumerable<Field> fields)
        {
            _fields = fields;
        }

        public override string ToString()
        {
            return _fields.Aggregate("", (s, f) => s + f);
        }
    }

    internal class Field
    {
        internal string Key { get; set; }
        internal string Value { get; set; }

        protected Field()
        {
        }

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