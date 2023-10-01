using System.Text;
using PasswordGenerator.Models;

namespace PasswordGenerator;
public class PwGenerator
{
    private readonly PwConfig _config;

    public PwGenerator(PwConfig config)
    {
        _config = config;
    }

    public string GeneratePassword()
    {
        var sb = new StringBuilder();

        foreach (var sequence in _config.Concat)
        {
            sb.Append(sequence.GetPwSequence());
        }
        foreach (var e in _config.Insert)
        {
            sb.Insert(e.Position, e.Sequence.GetPwSequence());
        }
        foreach (var e in _config.Fill)
        {
            if (e.MinLength < sb.Length) continue;
            e.Sequence.SetLength(e.MinLength - sb.Length);
            sb.Append(e.Sequence.GetPwSequence());
        }

        return sb.ToString();
    }
}
