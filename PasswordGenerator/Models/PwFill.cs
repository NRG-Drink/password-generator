namespace PasswordGenerator.Models;
public class PwFill
{
    public int MinLength { get; private set; }
    public PwSequence Sequence { get; } = new PwSequence();

    #region Config
    public PwFill SetMinlength(int position)
    {
        MinLength = position;
        return this;
    }

    //public PwFill SetSequence(Func<PwSequence, PwSequence> seqFunc)
    //{
    //    Sequence = seqFunc(new PwSequence());
    //    return this;
    //}

    public PwFill AddCharset(Func<PwCharset, PwCharset> charsetFunc)
    {
        Sequence.AddCharset(charsetFunc);
        return this;
    }

    public PwFill AddCharsets(IEnumerable<PwCharset> charsets)
    {
        Sequence.AddCharsets(charsets);
        return this;
    }

    public PwFill SetLength(int length)
    {
        Sequence.SetLength(length);
        return this;
    }
    #endregion
}
