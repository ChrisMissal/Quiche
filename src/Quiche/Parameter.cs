namespace Quiche
{
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;

    internal class Parameter
    {
        private readonly List<string> _parents = new List<string>();

        internal Parameter(object value, PropertyInfo property)
        {
            Value = value;
            Property = property;

            IsArray = property.PropertyType.IsArray;

            var propertyValue = Property.GetValue(Value, null);
            Objects = (IsArray && propertyValue != null)
                ? ((IEnumerable)propertyValue).Cast<object>().ToArray()
                : new object[0];
            IsSingleTypeArray = IsArray && IsArrayOnlyOneType(Objects);
            AreArrayObjectsValueTypes = IsArray && !IsSingleTypeArray;
        }

        private static bool IsArrayOnlyOneType(IEnumerable<object> objects)
        {
            return (objects.Select(x => x.GetType()).Distinct().Count() == 1);
        }

        internal object Value { get; private set; }
        internal PropertyInfo Property { get; private set; }

        internal string[] Parents
        {
            get { return _parents.ToArray(); }
        }

        internal bool IsArray { get; private set; }
        internal bool IsSingleTypeArray { get; private set; }
        internal bool AreArrayObjectsValueTypes { get; private set; }
        public object[] Objects { get; private set; }

        internal Parameter AddParent(params string[] parent)
        {
            _parents.AddRange(parent);
            return this;
        }

        public void InsertParents(params string[] parents)
        {
            foreach (var parent in parents)
                _parents.Insert(0, parent);
        }
    }
}