using PasswordGenerator.Models;

namespace PasswordGenerator.Alphabets;

public interface ICharsetCollection
{
    public Charset LowerLatinLetters { get; }
    public Charset UpperLatinLetters { get; }
    public Charset Numbers { get; }
    public Charset SpecialChars { get; }
    public Charset AllAlphabets { get; }
}
