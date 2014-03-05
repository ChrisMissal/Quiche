namespace Quiche.Tests.Features
{
    using System;
    using System.Collections.Concurrent;
    using System.Collections.Generic;
    using System.Reflection;
    using Fixie;
    using Fixie.Conventions;

    public class FeaturesConvention : DefaultConvention
    {
        private readonly IDictionary<string, FeatureWriter> _writers = new ConcurrentDictionary<string, FeatureWriter>();

        public FeaturesConvention()
        {
            CaseExecution
                .Wrap(TestCaseWrapper)
                .Skip(arg => arg.Method.HasOrInherits<RemovedAttribute>(), LogRemovedFeature);
        }

        private string LogRemovedFeature(Case arg)
        {
            var attr = arg.Method.GetCustomAttribute<FeatureAttribute>(true);
            
            var writer = GetWriter(attr.ReferenceAssembly);
            writer.Write(arg);

            return arg.Method.GetCustomAttribute<RemovedAttribute>().FeatureText;
        }

        private void TestCaseWrapper(CaseExecution caseExecution, object instance, Action innerbehavior)
        {
            var attr = caseExecution.Case.Method.GetCustomAttribute<FeatureAttribute>(true);

            var writer = GetWriter(attr.ReferenceAssembly);
            writer.Write(caseExecution.Case);

            innerbehavior();
        }

        private FeatureWriter GetWriter(Assembly assembly)
        {
            var key = assembly.GetName().Name + "-" + assembly.GetBuildVersion();
            if (_writers.ContainsKey(key))
                return _writers[key];

            var writer = new FeatureWriter(assembly);
            _writers.Add(key, writer);
            return writer;
        }
    }
}