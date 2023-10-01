using PasswordCreator.Models;

namespace PasswordGenerator.Models;

public record PwConfig(List<PwSequence> Concat, List<PwInsert> Insert, List<PwFill> Fill)
{
    public PwConfig() : this(new List<PwSequence>(), new List<PwInsert>(), new List<PwFill>())
    { }

    internal void Add(PwConfig pwConfig)
    {
        Concat.AddRange(pwConfig.Concat); 
        Insert.AddRange(pwConfig.Insert); 
        Fill.AddRange(pwConfig.Fill);
    }
}
