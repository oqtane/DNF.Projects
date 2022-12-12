namespace DNF.Projects.Shared
{
    public static class Common
    {
        public const string UrlPrefix = "https://github.com/";

        public static string TruncateString(string value, int length)
        {
            if (value.Length > length)
            {
                value = value.Substring(0, length);
            }
            return value;
        }
    }
}
