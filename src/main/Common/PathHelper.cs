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

        public static string ResolveInProcessPrivateKeyPath(string configuredPath, string avatarDirectory)
        {
            var resolvedPath = string.Empty;

            if (!string.IsNullOrWhiteSpace(configuredPath))
            {
                var trimmedPath = configuredPath.Trim();

                if (IsRootedOrUncPath(trimmedPath))
                {
                    resolvedPath = trimmedPath;
                }
                else if (!string.IsNullOrWhiteSpace(avatarDirectory))
                {
                    var normalizedRelativePath = NormalizeRelativePath(trimmedPath);
                    resolvedPath = Path.GetFullPath(Path.Combine(avatarDirectory, normalizedRelativePath));
                }
            }

            return resolvedPath;
        }
    }
}
