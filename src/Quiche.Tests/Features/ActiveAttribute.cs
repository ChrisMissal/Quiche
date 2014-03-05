namespace Quiche.Tests.Features
{
    public class ActiveAttribute : FeatureAttribute
    {
        public ActiveAttribute(string featureText) : base("active", featureText) { }
    }
}