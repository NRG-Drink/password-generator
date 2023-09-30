namespace PasswordCreator.Models;
public class PwInsert
{
    public int Position { get; set; }
    public PwSequence Sequence { get; set; } = new PwSequence();

    #region Config
    public PwInsert AddCharset(Func<PwCharset, PwCharset> charsetFunc)
    {
        var charset = charsetFunc(new PwCharset());
        Sequence.Values.Add(charset);
        return this;
    }

    public PwInsert SetLength(int length)
    {
        Sequence.SeqLength = length;
        return this;
    }

    public PwInsert SetSequence(Func<PwSequence, PwSequence> seqFunc)
    {
        Sequence = seqFunc(new PwSequence());
        return this;
    }

    public PwInsert SetPosition(int position)
    {
        Position = position;
        return this;
    }
    #endregion
}
