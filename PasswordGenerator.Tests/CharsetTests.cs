using PasswordGenerator.Models;

namespace PasswordGenerator.Tests;
[Trait("Misc", "Unit")]
public class CharsetTests
{
    [Fact]
    public void ConvertTest()
    {
        var list = new List<string> { "hello", "world" };
        var charset = list.ToCharset();

        Assert.Equal(list.Count, charset.Count);

        for (int i = 0; i < list.Count; i++)
        {
            Assert.Equal(list[i], charset[i]);    
        }
    }
}
