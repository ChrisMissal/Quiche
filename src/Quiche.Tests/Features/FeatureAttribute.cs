namespace Quiche.Tests.Features
{
    using System;
    using System.Reflection;

    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false, Inherited = true)]
    public abstract class FeatureAttribute : Attribute
    {
        protected FeatureAttribute(string type, string featureText)
        {
            FeatureText = featureText;
            Type = type;
            ReferenceAssembly = typeof(Builder).Assembly;
        }

        public string Type { get; set; }

        public Assembly ReferenceAssembly { get; set; }

        public string FeatureText { get; set; }
    }
}