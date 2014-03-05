namespace Quiche.Tests.Features
{
    using System.Reflection;

    internal static class Extensions
    {
        internal static string GetBuildVersion(this Assembly assembly)
        {
            var assemblyName = assembly.GetName();
            return string.Format("{0}.{1}.{2}", assemblyName.Version.Major, assemblyName.Version.Minor, assemblyName.Version.Build);
        }
    }
}