namespace PasswordGenerator.Alphabets;

public static class PwAlphabet
{
    public static List<string> LatinLettersL { get; } = GetLatinLettersL().ToList();
    public static List<string> LatinLettersU { get; } = GetLatinLettersU().ToList();
    public static List<string> Numbers { get; } = GetNumbers().ToList();
    public static List<string> SpecialChars { get; } = GetSpecialChars().ToList();
    public static List<string> All { get; } = GetAllAlphabets().ToList();


    private static IEnumerable<string> GetAllAlphabets()
        => GetLatinLettersL()
            .Concat(LatinLettersU)
            .Concat(GetNumbers())
            .Concat(GetSpecialChars());

    private static IEnumerable<string> GetLatinLettersL()
        => Enumerable
                .Range(97, 26)
                .Select(e => ((char)e)
                    .ToString()
                );

    private static IEnumerable<string> GetLatinLettersU()
        => Enumerable
                .Range(41, 26)
                .Select(e => ((char)e)
                    .ToString()
                );

    private static IEnumerable<string> GetNumbers()
        => Enumerable
            .Range(0, 9)
            .Select(e => e.ToString()
            );

    private static IEnumerable<string> GetSpecialChars()
    {
        for (int i = 0; i <= 255; i++)
        {
            char character = (char)i;

            if (!char.IsLetterOrDigit(character) && !char.IsWhiteSpace(character) && !char.IsPunctuation(character) && !char.IsControl(character))
            {
                yield return character.ToString();
            }
        }
    }
}
