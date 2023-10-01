using System.Runtime.CompilerServices;

namespace PasswordGenerator.Models;

public class Charset : List<string>
{
    public Charset()
    {
    }

    public Charset(IEnumerable<string> collection) : base(collection)
    { 
    }
}
