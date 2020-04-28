using System.Reflection;
using System.IO;

namespace NetGameShared.Util
{
    public static class Paths
    {
        public static string AssemblyPath
        {
            get {
                return Assembly.GetExecutingAssembly().Location;
            }
        }

        public static string AssemblyDirectory
        {
            get {
                return Path.GetDirectoryName(AssemblyPath);
            }
        }
    }
}
