namespace PasswordGenerator.Models;

public static class IEnumerableExtensions
{
    public static Charset ToCharset(this IEnumerable<string> list)
        => new Charset(list);
}
