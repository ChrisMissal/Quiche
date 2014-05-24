namespace Quiche
{
    using Settings;

    internal class NullBuilder
    {
        public NullBuilder(BuilderSettings settings)
        {
        }

        public Field Build(object value, string field, params string[] parentFields)
        {
            return new Field(field, null);
        }
    }
}