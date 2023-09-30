using System.Text;
using PasswordCreator.Models;
using PasswordCreator.StringTemplateParser;

namespace PasswordCreator;
public class PwGenerator
{
    private readonly List<PwSequence> _concat = new ();
    private readonly List<PwInsert> _insert = new ();
    private readonly List<PwFill> _fill = new();

    #region Config
    internal void SetConcat(List<PwSequence> list) => _concat.AddRange(list);
    internal void SetInsert(List<PwInsert> list) => _insert.AddRange(list);
    internal void SetFill(List<PwFill> list) => _fill.AddRange(list);

    public PwGenerator AddConfigFromTemplateString(string templateString)
    {
        var parser = new Parser();
        parser.AddConfig(this, templateString);
        return this;
    }

    public PwGenerator Concat(Func<PwSequence, PwSequence> seqFunc)
    {
        var seq = seqFunc(new PwSequence());
        _concat.Add(seq); 
        return this;
    }

    public PwGenerator InsertAt(Func<PwInsert, PwInsert> insertFunc)
    {
        var insrt = insertFunc(new PwInsert());
        _insert.Add(insrt);
        return this;
    }

    public PwGenerator FillUntil(Func<PwFill, PwFill> fillFunc)
    {
        var insrt = fillFunc(new PwFill());
        _fill.Add(insrt);
        return this;
    }
    #endregion

    public string GeneratePassword()
    {
        var sb = new StringBuilder();

        // Get Sequences.
        foreach (var sequence in _concat)
        {
            sb.Append(sequence.GetPwSequence());
        }
        // Insert snippets.
        foreach (var e in _insert)
        {
            sb.Insert(e.Position, e.Sequence.GetPwSequence());
        }
        // Fill rest.
        foreach (var e in _fill)
        {
            if (e.MinLength < sb.Length) continue;
            e.Sequence.SeqLength = e.MinLength - sb.Length;
            sb.Append(e.Sequence.GetPwSequence());
        }

        return sb.ToString();
    }
}
