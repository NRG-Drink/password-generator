using PasswordGenerator.Alphabets;
using PasswordGenerator.Models;
using PasswordGenerator.StringTemplateParser.Models;
using Sprache;

namespace PasswordGenerator.StringTemplateParser;
public class Parser
{
    private readonly Parser<IEnumerable<ParsedConfig>> _parser;
    private readonly PwCharsetFactory _charsetFactory;

    public Parser(Charset[]? customCharsets = null)
    {
        _charsetFactory = new PwCharsetFactory(new PwCharsetCollection(), customCharsets);

        var charsetP = GetCharsetParser();
        var countP = GetCountParser();

        var concatP = from p in Parse.Ref(() => GetSequenceParser(charsetP, countP)) 
                      select new ParsedConfig(p, null, null);
        var insertP = from p in Parse.Ref(() => GetInsertParser(charsetP, countP)) 
                      select new ParsedConfig(null, p, null);
        var untilP = from p in Parse.Ref(() => GetFillUntilParser(charsetP)) 
                     select new ParsedConfig(null, null, p);

        _parser =
            from ret in Parse.Ref(() => concatP.Or(insertP).Or(untilP)).AtLeastOnce()
            from end in Parse.LineTerminator
            select ret;
    }

    public PwConfig GetConfig(string templateString)
    {
        var config = _parser.Parse(templateString);

        var concat = config
            .Where(e => e.Concat is not null)
            .Select(e => e.Concat) 
            as IEnumerable<PwSequence>;
        var insert = config
            .Where(e => e.Insert is not null)
            .Select(e => e.Insert) 
            as IEnumerable<PwInsert>;
        var fill = config
            .Where(e => e.Fill is not null)
            .Select(e => e.Fill) 
            as IEnumerable<PwFill>;

        return new PwConfig(concat.ToList(), insert.ToList(), fill.ToList());
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

    private Charset GetCharset(ParsedCharset charset, IOption<IEnumerable<char>> num)
        => charset.Key is not null
            ? _charsetFactory.GetCharsetByKey(charset.Key, ParseNumber(num))
            : [charset.Element!];

    private Parser<ParsedCharset> GetKeyParser()
        => from key in Parse.Letter.Once().Text()
           select new ParsedCharset(key, null);

    private Parser<ParsedCharset> GetLetterParser()
        => from start in Parse.Char('\'')
           from values in Parse.CharExcept('\'').AtLeastOnce().Text()
           from end in Parse.Char('\'')
           select new ParsedCharset(null, values);

    private Parser<int> GetCountParser()
        => from comma in Parse.Char(',')
           from value in Parse.Digit.AtLeastOnce()
           select ParseNumber(value);
    #endregion

    private int ParseNumber(IOption<IEnumerable<char>> num)
    {
        if (num.IsEmpty) return 0;
        return ParseNumber(num.Get());
    }

    private int ParseNumber(IEnumerable<char> num)
    {
        var val = new string(num.ToArray());
        var number = int.Parse(val);
        return number;
    }
}
