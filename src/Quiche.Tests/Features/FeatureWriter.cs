namespace Quiche.Tests.Features
{
    using System.IO;
    using System.Reflection;
    using Fixie;

    public class FeatureWriter
    {
        private const int ClassIndex = 0;
        private const int MethodIndex = 1;

        private readonly AssemblyName _assemblyName;
        private readonly string _releaseVersion;
        private readonly string _path;

        public FeatureWriter(Assembly assembly)
        {
            _assemblyName = assembly.GetName();
            _releaseVersion = assembly.GetBuildVersion();
            _path = string.Format("{0}-{1}-features.csv", _assemblyName.Name, _releaseVersion);

            new FileInfo(_path).Delete();
        }

        public void Write(Case @case)
        {
            var attribute = @case.Method.GetCustomAttribute<FeatureAttribute>(true);
            var parts = @case.Class.Name.Split('_');

            var line = string.Format("{0},{1},{2},{3},{4},{5}",
                attribute.Type,
                _assemblyName.Name,
                _releaseVersion,
                parts[ClassIndex],
                parts[MethodIndex],
                attribute.FeatureText);

            File.AppendAllLines(_path, new[] { line });
        }
    }
}
