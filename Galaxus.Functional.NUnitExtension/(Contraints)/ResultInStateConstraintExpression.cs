using NUnit.Framework.Constraints;

namespace Galaxus.Functional.NUnitExtension;

public class ResultInStateConstraintExpression : ConstraintExpression
{
    public ResultInStateConstraintExpression(ConstraintBuilder builder) : base(builder)
    {
    }

    public ResultConstraintExpression Ok
    {
        get
        {
            builder.Append(new ResultStateCheckOperator(true));
            return new(builder);
        }
    }

    public ResultConstraintExpression Err
    {
        get
        {
            builder.Append(new ResultStateCheckOperator(false));
            return new(builder);
        }
    }
}
