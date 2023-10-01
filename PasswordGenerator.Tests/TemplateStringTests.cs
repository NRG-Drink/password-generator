using PasswordGenerator;
using PasswordGenerator.Models;
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
        var pwGenerator = new PwGeneratorBuilder("('a',5)('c',3)[5,'b',4]").Build();

        var pw = pwGenerator.GeneratePassword();
        Assert.Equal("aaaaabbbbccc", pw);
    }

    [Fact]
    public void CustomCharset1()
    {
        var pwGen = new PwGeneratorBuilder("('Programming')(1a6s,7)(a,3)(c,3)[4,s,3]f(n,30)").Build();

        for (int i = 0; i < 5; i++)
        {
            var pw = pwGen.GeneratePassword();
            _output.WriteLine(pw);
            Assert.StartsWith("Prog", pw);
            Assert.Contains("ramming", pw);
        }
    }

    [Fact]
    public void CustomCharset2()
    {
        var pwGen = new PwGeneratorBuilder("(x)(1a6s,7)(a,3)(c,3)[4,s,3]f(n,30)", "Programming").Build();

        for (int i = 0; i < 5; i++)
        {
            var pw = pwGen.GeneratePassword();
            _output.WriteLine(pw);
            Assert.StartsWith("Prog", pw);
            Assert.Contains("ramming", pw);
        }
    }

    [Fact]
    public void CustomCharset3()
    {
        var wordlist = new Charset { "Programming" };
        var pwGen = new PwGeneratorBuilder("(x)(1a6s,7)(a,3)(c,3)[4,s,3]f(n,30)", wordlist).Build();

        for (int i = 0; i < 5; i++)
        {
            var pw = pwGen.GeneratePassword();
            _output.WriteLine(pw);
            Assert.StartsWith("Prog", pw);
            Assert.Contains("ramming", pw);
        }
    }

    [Fact]
    public void CustomCharset4()
    {
        var wordlist1 = new Charset { "Programming" };
        var wordlist2 = new Charset { "Coding" };
        var pwGen = new PwGeneratorBuilder("(x^0)(x^2)(x^1)(1a6s,7)(a,3)(s,3)[2,s,3]f(n,40)", wordlist1, wordlist2, ["and"]).Build();

        for (int i = 0; i < 5; i++)
        {
            var pw = pwGen.GeneratePassword();
            _output.WriteLine(pw);
            Assert.StartsWith("Pr", pw);
            Assert.Contains("ogramming", pw);
            Assert.Contains("andCoding", pw);
            Assert.True(int.TryParse(pw[36..], out var num));
        }
    }

    [Fact]
    public void Readme1()
    {
        var pwGen = new PwGeneratorBuilder("(c)(2sn,4)[4,a,3]f(n,10)").Build();

        for (int i = 0; i < 5; i++)
        {
            var pw = pwGen.GeneratePassword();
            _output.WriteLine(pw);
            Assert.True(int.TryParse(pw[8..], out var num));
        }
    }

    [Fact]
    public void Readme2()
    {
        var wordlist1 = new Charset { "Password", "Generator" };
        var wordlist2 = new Charset { " is ", "s are " };
        var wordlist3 = new Charset { "fantastic", "great", "awesome" };
        var pwGen = new PwGeneratorBuilder("(x)(x^1)(x^2)f('!',25)", wordlist1, wordlist2, wordlist3).Build();

        for (int i = 0; i < 5; i++)
        {
            var pw = pwGen.GeneratePassword();
            _output.WriteLine(pw);
            Assert.EndsWith("!", pw);
        }
    }
}