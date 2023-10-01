using PasswordGenerator.Models;
using PasswordGenerator.StringTemplateParser.Models;
using Sprache;

namespace PasswordGenerator.StringTemplateParser;
public class Parser
{
    private readonly Parser<IEnumerable<Return>> _parser;
    private readonly PwCharsetParser _charsetParser;

    public Parser(List<string>[]? customCharsets = null)
    {
        _charsetParser = new PwCharsetParser(customCharsets);

        var charsetP = GetCharsetParser();
        var countP = GetCountParser();

        var concatP = from p in Parse.Ref(() => GetSequenceParser(charsetP, countP)) select new Return(p, null, null);
        var insertP = from p in Parse.Ref(() => GetInsertParser(charsetP, countP)) select new Return(null, p, null);
        var untilP = from p in Parse.Ref(() => GetFillUntilParser(charsetP)) select new Return(null, null, p);

        _parser =
            from ret in Parse.Ref(() => concatP.Or(insertP).Or(untilP)).AtLeastOnce()
            from end in Parse.LineTerminator
            select ret;
    }

    public PwConfig GetConfig(string templateString)
    {
        var pwConfig = _parser.Parse(templateString);
        return GetPwGen(pwConfig);
    }


    #region Init Parsers
    private Parser<PwFill> GetFillUntilParser(
        Parser<PwCharset> charsetParser
        )
        => from seq in Parse.Ref(() =>
                from start in Parse.String("f(")
                from alphabets in Parse.Ref(() => charsetParser).AtLeastOnce()
                from comma in Parse.Char(',')
                from minLength in Parse.Digit.AtLeastOnce()
                from end in Parse.Char(')')
                select new PwFill()
                    .SetMinlength(ParseNumber(minLength))
                    .SetLength(0)
                    .AddCharsets(alphabets)
                )
           select seq;

    private Parser<PwInsert> GetInsertParser(
        Parser<PwCharset> charsetParser,
        Parser<int> countParser
        )
        => from seq in Parse.Ref(() =>
                from start in Parse.Char('[')
                from insertAt in Parse.Digit.AtLeastOnce()
                from comma in Parse.Char(',')
                from alphabets in Parse.Ref(() => charsetParser).AtLeastOnce()
                from count in countParser.Optional()
                from end in Parse.Char(']')
                select new PwInsert()
                    .SetPosition(ParseNumber(insertAt))
                        .SetLength(count.IsEmpty ? 1 : count.Get())
                        .AddCharsets(alphabets)
                )
           select seq;

    private Parser<PwSequence> GetSequenceParser(
        Parser<PwCharset> charsetParser,
        Parser<int> countParser
        )
        => from seq in Parse.Ref(() =>
                from start in Parse.Char('(')
                from alphabets in Parse.Ref(() => charsetParser).AtLeastOnce()
                from count in countParser.Optional()
                from end in Parse.Char(')')
                select new PwSequence()
                    .SetLength(count.IsEmpty ? 1 : count.Get())
                    .AddCharsets(alphabets)
                )
           select seq;

    private Parser<PwCharset> GetCharsetParser()
        => from min in Parse.Digit.AtLeastOnce().Optional()
           from charset in Parse.Ref(() => GetKeyParser().Or(GetLetterParser()))
           from roof in Parse.Char('^').Optional()
           from num in Parse.Digit.AtLeastOnce().Optional()
           select new PwCharset()
            .SetMin(ParseNumber(min))
            .AddCharset(GetCharset(charset, num))
           ;

    private List<string> GetCharset(Charset charset, IOption<IEnumerable<char>> num)
        => charset.Key is not null
            ? _charsetParser.GetCharsetByKey(charset.Key, ParseNumber(num))
            : [charset.Chars ?? string.Empty];

    private Parser<Charset> GetKeyParser()
        => from key in Parse.Letter.Once().Text()
           select new Charset(key, null);

    private Parser<Charset> GetLetterParser()
        => from start in Parse.Char('\'')
           from values in Parse.CharExcept('\'').AtLeastOnce().Text()
           from end in Parse.Char('\'')
           select new Charset(null, values);

    private Parser<int> GetCountParser()
        => from comma in Parse.Char(',')
           from value in Parse.Digit.AtLeastOnce()
           select ParseNumber(value);
    #endregion

    //private List<string> GetCharsetByKey(string key, int num = 0)
    //    => key switch
    //    {
    //        "s" => new List<string>() { "%", "/", "!", "?" },
    //        "n" => Enumerable.Range(0, 9).Select(e => e.ToString()).ToList(),
    //        "c" => Enumerable.Range(97, 26).Select(e => ((char)e).ToString()).ToList(),
    //        "C" => Enumerable.Range(97, 26).Select(e => ((char)e).ToString().ToUpper()).ToList(),
    //        "a" => GetCharsetByKey("s").Concat(GetCharsetByKey("n")).Concat(GetCharsetByKey("c")).Concat(GetCharsetByKey("C")).ToList(),
    //        "x" => GetCustomCharset(num),
    //        _ => throw new ArgumentException($"No default charset found for '{key}'.")
    //    };

    //private List<string> GetCustomCharset(int num)
    //{
    //    if (_customCharsets is null) throw new ArgumentNullException($"No custom charset defined.");
    //    if (_customCharsets.Length <= num) throw new IndexOutOfRangeException(
    //            $"No custom charset with index {num} defined\n" +
    //            $"Make sure start counting at 0, not 1."
    //            );
    //    return _customCharsets[num];
    //}

    private string? ParseString(IOption<IEnumerable<char>> num)
    {
        if (num.IsEmpty) return null;
        return new string(num.Get().ToArray());
    }

    private int ParseNumber(IOption<IEnumerable<char>> num)
    {
        if (num.IsEmpty) return 0;
        return ParseNumber(num.Get());
    }

    private int ParseNumber(IEnumerable<char> num)
    {
        var val = new string(num.ToArray()) ?? "0";
        var number = int.Parse(val);
        return number;
    }

    private PwConfig GetPwGen(IEnumerable<Return> config)
    {
        var concat = config.Where(e => e.Concat is not null).Select(e => e.Concat) as IEnumerable<PwSequence>;
        var insert = config.Where(e => e.Insert is not null).Select(e => e.Insert) as IEnumerable<PwInsert>;
        var fill = config.Where(e => e.Fill is not null).Select(e => e.Fill) as IEnumerable<PwFill>;

        return new PwConfig(concat.ToList(), insert.ToList(), fill.ToList());
    }
}
