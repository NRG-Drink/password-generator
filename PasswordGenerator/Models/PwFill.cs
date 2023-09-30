using Sprache;

namespace PasswordCreator.Models;
public class PwFill
{
    public int MinLength { get; set; }
    public PwSequence Sequence { get; set; }


    #region Config
    public PwFill SetSequence(Func<PwSequence, PwSequence> seqFunc)
    {
        Sequence = seqFunc(new PwSequence());
        return this;
    }

    public PwFill SetMinlength(int position)
    {
        MinLength = position;
        return this;
    }
    #endregion
}
