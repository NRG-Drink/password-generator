namespace PasswordGenerator.Models;
public class PwInsert
{
    internal int Position { get; private set; }
    internal PwSequence Sequence { get; } = new PwSequence();

    #region Config
    public PwInsert SetPosition(int position)
    {
        Position = position;
        return this;
    }

    public PwInsert AddCharset(Func<PwCharset, PwCharset> charsetFunc)
    {
        Sequence.AddCharset(charsetFunc);
        return this;
    }

    public PwInsert AddCharsets(IEnumerable<PwCharset> charsets)
    {
        Sequence.AddCharsets(charsets);
        return this;
    }

    public PwInsert SetLength(int length)
    {
        Sequence.SetLength(length);
        return this;
    }
    #endregion
}
