namespace ei8.Avatar.Installer.Common
{
    public static class PathHelper
    {
        public static bool IsRootedOrUncPath(string path)
        {
            var isRootedPath = Path.IsPathRooted(path);
            var isUncPath = Uri.TryCreate(path, UriKind.Absolute, out var uri) && uri.IsUnc;

            return isRootedPath || isUncPath;
        }

        public static string NormalizeRelativePath(string configuredPath) =>
            configuredPath
                .TrimStart(Path.DirectorySeparatorChar, Path.AltDirectorySeparatorChar)
                .Replace('/', Path.DirectorySeparatorChar)
                .Replace('\\', Path.DirectorySeparatorChar);
    }
}
