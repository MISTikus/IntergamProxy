namespace TfsNotifications.Extensions
{
    public static class StringExtensions
    {
        public static string ReplaceAll(this string source, params (string from, string to)[] replaces)
        {
            foreach (var (from, to) in replaces)
            {
                source = source.Replace(from, to);
            }
            return source;
        }
    }
}