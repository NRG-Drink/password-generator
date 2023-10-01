using PasswordCreator;
using PasswordCreator.Models;
using PasswordCreator.StringTemplateParser;
using PasswordGenerator.Models;

namespace PasswordGenerator;

public class PwGeneratorBuilder
{
    private PwConfig _config = new PwConfig(new List<PwSequence>(), new List<PwInsert>(), new List<PwFill>());

    public PwGeneratorBuilder() { }

    public PwGeneratorBuilder(
        IEnumerable<PwSequence>? concat,
        IEnumerable<PwInsert>? insert,
        IEnumerable<PwFill>? fill
        )
    {
        if (concat is not null) _config.Concat.AddRange(concat);
        if (insert is not null) _config.Insert.AddRange(insert);
        if (fill is not null) _config.Fill.AddRange(fill);
    }

    public PwGeneratorBuilder(string templateString) : this(templateString, customCharsets: null) 
    { }

    public PwGeneratorBuilder(string templateString, params string[] singleCharset) : 
        this(templateString, customCharsets: [singleCharset.ToList()])
    { }

    public PwGeneratorBuilder(string templateString, params List<string>[]? customCharsets)
    {
        var parser = new Parser(customCharsets);
        _config.Add(parser.GetConfig(templateString));
    }

    public PwGenerator Build()
        => new PwGenerator(_config);


    #region Config
    public PwGeneratorBuilder Concat(Func<PwSequence, PwSequence> seqFunc)
    {
        var seq = seqFunc(new PwSequence());
        _config.Concat.Add(seq);
        return this;
    }

    public PwGeneratorBuilder InsertAt(Func<PwInsert, PwInsert> insertFunc)
    {
        var insrt = insertFunc(new PwInsert());
        _config.Insert.Add(insrt);
        return this;
    }

    public PwGeneratorBuilder FillUntil(Func<PwFill, PwFill> fillFunc)
    {
        var insrt = fillFunc(new PwFill());
        _config.Fill.Add(insrt);
        return this;
    }
    #endregion
}
