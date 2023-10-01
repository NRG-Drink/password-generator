using PasswordGenerator.Models;
using PasswordGenerator.StringTemplateParser;

namespace PasswordGenerator;

public class PwGeneratorBuilder
{
    private PwConfig _config = new PwConfig();

    public PwGeneratorBuilder() 
    { }

    public PwGeneratorBuilder(string templateString) 
        : this(templateString, customCharsets: null) 
    { }

    public PwGeneratorBuilder(string templateString, params string[] singleCharset) 
        : this(templateString, customCharsets: [singleCharset.ToCharset()])
    { }

    public PwGeneratorBuilder(string templateString, params Charset[]? customCharsets)
    {
        var parser = new Parser(customCharsets);
        _config.Add(parser.GetConfig(templateString));
    }

    public PwGenerator Build()
        => new PwGenerator(_config);


    #region Config
    public PwGeneratorBuilder TemplateString(
        string templateString
        )
        => TemplateString(templateString, customCharsets: null);

    public PwGeneratorBuilder TemplateString(
        string templateString,
        params string[] singleCharset
        )
        => TemplateString(templateString, customCharsets: [singleCharset.ToCharset()]);

    public PwGeneratorBuilder TemplateString(
        string templateString,
        params Charset[]? customCharsets
        )
    {
        var parser = new Parser(customCharsets);
        _config.Add(parser.GetConfig(templateString));
        return this;
    }

    public PwGeneratorBuilder Append(Func<PwSequence, PwSequence> seqFunc)
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
