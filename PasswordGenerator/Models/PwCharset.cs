using System.Text;

namespace PasswordGenerator.Models;
public class PwCharset
{
    internal int MinOccurrences { get; set; }
    internal List<string> Charset { get; private set; } = new List<string>();

    #region Config
    public PwCharset SetMin(int minOccurrences)
    {
        MinOccurrences = minOccurrences;
        return this;
    }

    public PwCharset AddChars(params char[] chars)
    {
        Charset.AddRange(chars.Select(e => e.ToString()));
        return this;
    }

    public PwCharset AddChars(params string[] strings)
    {
        Charset.AddRange(strings);
        return this;
    }

    public PwCharset AddCharset(params List<string>[] strings)
    {
        Charset.AddRange(strings.SelectMany(e => e));
        return this;
    }
    #endregion

    internal string PickRandom()
    {
        var r = new Random();
        return Charset[r.Next(Charset.Count)];
    }

    internal IEnumerable<string> PickMin()
    {
        if (MinOccurrences <= 0) yield break;

        for (int i = 0; i < MinOccurrences; i++)
        {
            yield return PickRandom();
        }
    }
}
