namespace PasswordCreator;
public class PwGeneratorTest
{
    public void ExampleConfig()
    {
        var wordlist = new string[] { "Musicstudio", "Wonderwall", "Bananaphnoe", "Airplane", "Mustard" };
        var pwGen = new PwGenerator()
            .Concat(e => e
                .SetLength(1)
                .AddCharset(e => e
                    .AddChars(wordlist)
                )
            )
            .InsertAt(e => e
                .SetPosition(0)
                .SetSequence(e => e
                    .SetLength(3)
                    .AddCharset(e => e.AddChars('X','Y','Z'))
                    .AddCharset (e => e.SetMin(1).AddChars('%'))
                )
            )
            .InsertAt(e => e
                .SetPosition(8)
                .SetSequence(e => e
                    .SetLength(4)
                    .AddCharset(e => e.AddChars('1','2','3','4'))
                    .AddCharset(e => e.SetMin(2).AddChars('7','8'))
                )
            )
            .FillUntil(e => e
                .SetMinlength(25)
                .SetSequence(e => e
                    .AddCharset(e => e.AddChars('%','&','#','*','@','+'))
                    .AddCharset(e => e.SetMin(3).AddChars('7','8'))
                )
            )
            ;

        for (int i = 0; i < 5; i++)
        {
            Console.WriteLine(pwGen.GeneratePassword());
        }
    }

    public void ExampleConfigWithTemplateString()
    {
        var wordlist = new string[] { "Musicstudio", "Wonderwall", "Bananaphnoe", "Airplane", "Mustard" };
        var pwGen = new PwGenerator()
            .Concat(e => e
                .SetLength(1)
                .AddCharset (e => e
                    .AddChars(wordlist)
                )
            )
            .AddConfigFromTemplateString("(1a6s){7}(a){3}(z){3}[0,z]{3}f[30,n]")
            ;

        for (int i = 0; i < 5; i++)
        {
            Console.WriteLine(pwGen.GeneratePassword());
        }
    }
}
