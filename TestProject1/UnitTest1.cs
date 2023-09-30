using PasswordCreator;

namespace TestProject1
{
    public class UnitTest1
    {
        [Fact]
        public void FluentSimpleTest()
        {
            var pwGenerator = new PwGenerator()
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
                ;

            var pw = pwGenerator.GeneratePassword();
            Assert.Equal("aaaaabbbbccc", pw);
        }
    }
}