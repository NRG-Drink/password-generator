using PasswordCreator;
using Xunit.Abstractions;

namespace TestProject1;
[Trait("Template", "")]
public class TemplateStringTests
{
    private readonly ITestOutputHelper _output;

    public TemplateStringTests(ITestOutputHelper output)
    {
        _output = output;
    }


    [Fact]
    public void Simple1()
    {
        var pwGenerator = new PwGenerator()
            .AddConfigFromTemplateString("");

        var pw = pwGenerator.GeneratePassword();
        Assert.Equal("aaaaabbbbccc", pw);
    }

    [Fact]
    public void Example()
    {
        var wordlist = new string[] { "Musicstudio", "Wonderwall", "Bananaphnoe", "Airplane", "Mustard" };
        var pwGen = new PwGenerator()
            .Concat(e => e
                .SetLength(1)
                .AddCharset(e => e
                    .AddChars(wordlist)
                )
            )
            .AddConfigFromTemplateString("(1a6s){7}(a){3}(z){3}[2,z]{3}f[30,n]")
            ;

        for (int i = 0; i < 5; i++)
        {
            _output.WriteLine(pwGen.GeneratePassword());
        }
    }
}