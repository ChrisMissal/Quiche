namespace Quiche.Tests.Samples
{
    public class ObjectWithChildObject
    {
        public FlatObject Child { get; set; }
        public string Name { get; set; }
    }

    public class MixedObject
    {
        public string[] ItemNames { get; set; }
        public string TextMessage { get; set; }
    }

    public class ObjectWithArray
    {
        public int[] Items { get; set; }
    }

    public class FlatObject
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }
}