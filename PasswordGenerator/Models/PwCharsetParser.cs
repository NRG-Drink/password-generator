using PasswordGenerator.Alphabets;

namespace PasswordGenerator.Models;

public class PwCharsetParser
{
    private readonly List<string>[]? _customCharsets;

    public PwCharsetParser(List<string>[]? customCharsets)
    {
        _customCharsets = customCharsets;
    }

    public List<string> GetCharsetByKey(string key, int num = 0)
    => key switch
    {
        "s" or "S" => PwAlphabet.SpecialChars,
        "n" or "N" => PwAlphabet.Numbers,
        "c" => PwAlphabet.LatinLettersL,
        "C" => PwAlphabet.LatinLettersU,
        "a" or "A" => PwAlphabet.All,
        "x" or "X" => GetCustomCharset(num),
        _ => throw new ArgumentException($"No default charset found for '{key}'.")
    };

    private List<string> GetCustomCharset(int num)
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
