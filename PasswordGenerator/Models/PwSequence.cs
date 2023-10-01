using System.Runtime.InteropServices;
using System.Text;

namespace PasswordCreator.Models;
public class PwSequence
{
    internal List<PwCharset> Values { get; set; } = new();
    internal int SeqLength { get; set; } = 0;

    #region Config
    public PwSequence AddCharset(Func<PwCharset, PwCharset> charsetFunc)
    {
        var charset = charsetFunc(new PwCharset());
        Values.Add(charset);
        return this;
    }

    public PwSequence AddCharsets(IEnumerable<PwCharset> charsets)
    {
        Values.AddRange(charsets);
        return this;
    }

    public PwSequence SetLength(int length)
    {
        SeqLength = length;
        return this;
    }
    #endregion

    internal string GetPwSequence()
    {
        var words = new List<string>();
        foreach (var e in Values)
        {
            words.AddRange(e.PickMin());
        }

        var len = words.Sum(e => e.Length);
        var allChar = Values.SelectMany(e => e.Charset).ToList();
        var r = new Random();
        for (int i = len; i < SeqLength; i++)
        {
            var n = r.Next(allChar.Count);
            words.Add(allChar[n]);
        }

        return Scramble(words);
    }

    private string Scramble(List<string> input)
    {
        var nums = Enumerable.Range(0, input.Count).ToList();
        var sb = new StringBuilder();
        var r = new Random();
        for (int i = 0; i < input.Count; i++)
        {
            var n = r.Next(nums.Count);
            var p = nums[n];
            sb.Append(input[p]);
            nums.Remove(p);
        }

        return sb.ToString();
    }
}
