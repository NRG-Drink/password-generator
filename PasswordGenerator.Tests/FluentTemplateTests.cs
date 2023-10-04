using PasswordGenerator.Models;
using Xunit.Abstractions;

namespace PasswordGenerator.Tests;
[Trait("Fluent", "Unit")]
public class FluentTemplateTests
{
	private readonly ITestOutputHelper _output;

	public FluentTemplateTests(ITestOutputHelper output)
	{
		_output = output;
	}


	[Fact]
	public void Simple1()
	{
		var pwGenerator = new PwGeneratorBuilder()
			.TemplateString("('a',5)('c',3)[5,'b',4]")
			.Build();

		var pw = pwGenerator.GeneratePassword();
		Assert.Equal("aaaaabbbbccc", pw);
	}

	[Fact]
	public void CustomCharset1()
	{
		var pwGen = new PwGeneratorBuilder()
			.TemplateString(
				"(x)(1a6s,7)(a,3)(c,3)[4,s,3]f(n,30)",
				"Programming"
			)
			.Build();

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
		var pwGen = new PwGeneratorBuilder()
			.TemplateString(
				"(x)(1a6s,7)(a,3)(c,3)[4,s,3]f(n,30)",
				"Programming",
				"Coding"
			)
			.Build();

		for (int i = 0; i < 5; i++)
		{
			var pw = pwGen.GeneratePassword();
			_output.WriteLine(pw);
		}
	}

	[Fact]
	public void CustomCharset3()
	{
		var pwGen = new PwGeneratorBuilder()
			.TemplateString(
				"(x)(1a6s,7)(x^1)(a,3)(c,3)[4,s]f(n,30)",
				new Charset() { "Programming", "Coding" },
				new Charset() { "hello", "world" }
			)
			.Build();

		for (int i = 0; i < 5; i++)
		{
			var pw = pwGen.GeneratePassword();
			_output.WriteLine(pw);
		}
	}
}
