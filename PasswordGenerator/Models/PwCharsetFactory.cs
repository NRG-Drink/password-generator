using PasswordGenerator.Alphabets;

namespace PasswordGenerator.Models;

public class PwCharsetFactory
{
    private readonly ICharsetCollection _alphabet;
    private readonly Charset[]? _customCharsets;

    public PwCharsetFactory(ICharsetCollection alphabet, Charset[]? customCharsets)
    {
        _alphabet = alphabet;
        _customCharsets = customCharsets;
    }

    public Charset GetCharsetByKey(string key, int num = 0)
    => key switch
    {
        "c" => _alphabet.LowerLatinLetters,
        "C" => _alphabet.UpperLatinLetters,
        "n" or "N" => _alphabet.Numbers,
        "s" or "S" => _alphabet.SpecialChars,
        "a" or "A" => _alphabet.AllAlphabets,
        "x" or "X" => GetCustomCharset(num),
        _ => throw new ArgumentException($"No default charset found for '{key}'.")
    };

    private Charset GetCustomCharset(int num)
    {
        if (_customCharsets is null) throw new ArgumentNullException($"No custom charset defined.");
        if (_customCharsets.Length <= num) 
            throw new IndexOutOfRangeException(
                $"No custom charset with index {num} defined\n" +
                $"Make sure start counting at 0, not 1."
            );
        return _customCharsets[num];
    }
}
