namespace ei8.Avatar.Installer.Common
{
    public static class StringExtensions
    {
        /// <summary>
        /// Converts this <see cref="string"/> to snake_case
        /// </summary>
        public static string ToSnakeCase(this string value)
        {
            return AppendUnderscores(value).ToLower();
        }

        /// <summary>
        /// Converts this <see cref="string"/> to MACRO_CASE
        /// </summary>
        /// <param name="value"></param>
        public static string ToMacroCase(this string value)
        {
            return AppendUnderscores(value).ToUpper();
        }

        private static string AppendUnderscores(string value) => 
            string.Concat(value.Select((x, i) =>
            {
                if (i > 0 && char.IsUpper(x))
                    return "_" + x.ToString();
                else
                    return x.ToString();
            }));
    }
}