namespace Quiche.Tests.Features
{
    public class RemovedAttribute : FeatureAttribute
    {
        public RemovedAttribute(string featureText) : base("removed", featureText) { }
    }
}