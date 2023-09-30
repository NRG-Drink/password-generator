using PasswordCreator.Models;
using Sprache;
using System.Runtime.InteropServices;

namespace PasswordCreator.StringTemplateParser;
public class Parser
{
    private readonly Parser<IEnumerable<Return>> _parser;

    public Parser()
    {
        var charsetP = GetCharsetParser();
        var countP = GetCountParser();

        var concatP = from p in Parse.Ref(() => GetSequenceParser(charsetP, countP)) select new Return(p, null, null);
        var insertP = from p in Parse.Ref(() => GetInsertParser(charsetP, countP)) select new Return(null, p, null);
        var untilP = from p in Parse.Ref(() => GetFillUntilParser(charsetP)) select new Return(null, null, p);

        _parser =
            from ret in Parse.Ref(() => concatP.Or(insertP).Or(untilP)).AtLeastOnce()
            select (ret);
    }


    public PwGenerator AddConfig(PwGenerator pwGenerator, string input)
    {
        /* Needs
         * Add Sequence (c)
         * Insert At    [n, c]
         * Length       {n}
         * Fill Until   f[n, c]
         * 
         * c = charset
         * 1c = min seq of charset
         * n = number
         * f = fill until
         * 
         * c = characters
         * n = numbers
         * s = special characters
         * x = custom strings
         *  x = all custom alphabets
         *  x^1 = specific custom alphabet
         */

        var pwConfig = _parser.Parse(input);
        return GetPwGen(pwGenerator, pwConfig);
    }


    #region Init Parser
    private Parser<PwFill> GetFillUntilParser(
        Parser<Charset> charsetParser
        )
        => from seq in Parse.Ref(() =>
                from start in Parse.String("f[")
                from minLength in Parse.Digit.AtLeastOnce()
                from comma in Parse.Char(',')
                from alphabets in Parse.Ref(() => charsetParser).AtLeastOnce()
                from end in Parse.Char(']')
                select new PwFill()
                {
                    MinLength = ParseNumber(minLength),
                    Sequence = new PwSequence
                    {
                        SeqLength = 0,
                        Values = alphabets.Select(e => new PwCharset()
                        {
                            MinOccurrences = e.MinContaining,
                            Charset = GetCharsetByKey(e.Key)
                        }).ToList()
                    }
                }
            )
           select seq;

    private Parser<PwInsert> GetInsertParser(
        Parser<Charset> charsetParser,
        Parser<int> countParser
        )
        => from seq in Parse.Ref(() =>
                from start in Parse.Char('[')
                from insertAt in Parse.Digit.AtLeastOnce()
                from comma in Parse.Char(',')
                from alphabets in Parse.Ref(() => charsetParser).AtLeastOnce()
                from end in Parse.Char(']')
                from count in countParser.Optional()
                select new PwInsert()
                {
                    Position = ParseNumber(insertAt),
                    Sequence = new PwSequence
                    {
                        SeqLength = count.IsEmpty ? 1 : count.Get(),
                        Values = alphabets.Select(e => new PwCharset()
                        {
                            MinOccurrences = e.MinContaining,
                            Charset = GetCharsetByKey(e.Key)
                        }).ToList()
                    }
                }
                
            )
           select seq;

    private Parser<PwSequence> GetSequenceParser(
        Parser<Charset> charsetParser,
        Parser<int> countParser
        )
        => from seq in Parse.Ref(() =>
                from start in Parse.Char('(')
                from alphabets in Parse.Ref(() => charsetParser).AtLeastOnce()
                from end in Parse.Char(')')
                from count in countParser.Optional()
                select new PwSequence()
                {
                    SeqLength = count.IsEmpty ? 1 : count.Get(),
                    Values = alphabets.Select(e => new PwCharset()
                    {
                        MinOccurrences = e.MinContaining,
                        Charset = GetCharsetByKey(e.Key)
                    }).ToList()
                }
            )
            select seq;

    private Parser<Charset> GetCharsetParser()
        => from min in Parse.Digit.AtLeastOnce().Optional()
           from key in Parse.Letter.Once().Text()
           select new Charset(ParseNumber(min), key);

    private Parser<int> GetCountParser()
        => from start in Parse.Char('{')
           from value in Parse.Digit.AtLeastOnce()
           from end in Parse.Char('}')
           select ParseNumber(value);
    #endregion

    private List<string> GetCharsetByKey(string key)
        => key switch
        {
            "a" => new List<string>() { "a", "b", "c" },
            "s" => new List<string>() { "%", "/", "!", "?" },
            //"c" => Enumerable.Range(97, 26).Select(e => ((char)e).ToString()).ToList(),
            "n" => Enumerable.Range(0, 9).Select(e => e.ToString()).ToList(),
            "z" => new List<string>() { "x", "y", "z" },
            _ => new List<string>() { "-" }
        };

    private int ParseNumber(IOption<IEnumerable<char>> num)
    {
        if (num.IsEmpty) return 0;
        return ParseNumber(num.Get());
    }

    private int ParseNumber(IEnumerable<char> num)
    {
        var val = new string(num.ToArray()).ToString() ?? "0";
        var number = int.Parse(val);
        return number;
    }


    private PwGenerator GetPwGen(PwGenerator gen, IEnumerable<Return> config)
    {
        gen.SetConcat(config
            .Where(e => e.Concat is not null)
            .Select(e => new PwSequence()
            { SeqLength = e.Concat!.SeqLength, Values = e.Concat!.Values }
            )
            .ToList()
        );
        gen.SetInsert(config
            .Where(e => e.Insert is not null)
            .Select(e => new PwInsert()
            { Position = e.Insert!.Position, Sequence = e.Insert.Sequence }
            )
            .ToList()
        );
        gen.SetFill(config
            .Where(e => e.Fill is not null)
            .Select(e => new PwFill()
            { MinLength = e.Fill!.MinLength, Sequence = e.Fill!.Sequence, }
            )
            .ToList()
        );

        return gen;
    }
}



public record Charset(int MinContaining, string Key);
public record Return(PwSequence? Concat, PwInsert? Insert, PwFill? Fill);
