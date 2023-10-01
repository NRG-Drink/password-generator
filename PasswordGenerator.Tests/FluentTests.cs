using PasswordCreator;
using PasswordGenerator;
using Xunit.Abstractions;

namespace TestProject1;
[Trait("Fluent", "")]
public class FluentTests
{
    private readonly ITestOutputHelper _output;

    public FluentTests(ITestOutputHelper output)
    {
        _output = output;
    }


    [Fact]
    public void Simple1()
    {
        var pwGenerator = new PwGeneratorBuilder()
            .Concat(e => e
                .SetLength(5)
                .AddCharset(e => e
                    .AddChars('a')
                )
            )
            .InsertAt(e => e
                .SetPosition(5)
                .SetLength(4)
                .AddCharset(e => e
                    .AddChars('b')
                )
            )
            .Concat(e => e
                .SetLength(3)
                .AddCharset(e => e
                    .AddChars('c')
                )
            )
            .Build()
            ;

        var pw = pwGenerator.GeneratePassword();
        Assert.Equal("aaaaabbbbccc", pw);
    }

    [Fact]
    public void Example1()
    {
        var wordlist = new string[] { "Musicstudio", "Wonderwall", "Bananaphnoe", "Airplane", "Mustard" };
        var pwGen = new PwGeneratorBuilder()
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
                    .AddCharset(e => e.AddChars('X', 'Y', 'Z'))
                    .AddCharset(e => e.SetMin(1).AddChars('%'))
                )
            )
            .InsertAt(e => e
                .SetPosition(8)
                .SetSequence(e => e
                    .SetLength(4)
                    .AddCharset(e => e.AddChars('1', '2', '3', '4'))
                    .AddCharset(e => e.SetMin(2).AddChars('7', '8'))
                )
            )
            .FillUntil(e => e
                .SetMinlength(25)
                .SetSequence(e => e
                    .AddCharset(e => e.AddChars('%', '&', '#', '*', '@', '+'))
                    .AddCharset(e => e.SetMin(3).AddChars('7', '8'))
                )
            )
            .Build()
            ;

        for (int i = 0; i < 5; i++)
        {
            _output.WriteLine(pwGen.GeneratePassword());
        }
    }
}