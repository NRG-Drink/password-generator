using PasswordGenerator.Models;
using System.Linq;

namespace PasswordGenerator.Alphabets;

public class PwCharsetCollection : ICharsetCollection
{
    public Charset LowerLatinLetters { get; } = GetLowerLatinLetters();
    public Charset UpperLatinLetters { get; } = GetUpperLatinLetters();
    public Charset Numbers { get; } = GetNumbers();
    public Charset SpecialChars { get; } = GetSpecialChars();
    public Charset AllAlphabets { get; } = GetAllAlphabets();

    private static Charset GetAllAlphabets()
        => GetLowerLatinLetters()
            .Concat(GetUpperLatinLetters())
            .Concat(GetNumbers())
            .Concat(GetSpecialChars()).ToCharset();

    private static Charset GetLowerLatinLetters()
        => Enumerable
            .Range(97, 26)
            .Select(e => ((char)e)
                .ToString()
            )
            .ToCharset();

    private static Charset GetUpperLatinLetters()
        => Enumerable
            .Range(41, 26)
            .Select(e => ((char)e)
                .ToString()
            )
            .ToCharset();

    private static Charset GetNumbers()
        => Enumerable
            .Range(0, 9)
            .Select(e => e.ToString()
            )
            .ToCharset();

    private static Charset GetSpecialChars()
    {
        var set = new Charset();
        for (int i = 0; i <= 255; i++)
        {
            var c = (char)i;
            if (!char.IsLetterOrDigit(c) 
                && !char.IsWhiteSpace(c) 
                && !char.IsPunctuation(c) 
                && !char.IsControl(c))
            {
                set.Add(c.ToString());
            }
        }
        return set;
    }
}
