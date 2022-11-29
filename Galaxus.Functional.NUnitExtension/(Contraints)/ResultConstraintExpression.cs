using NUnit.Framework.Constraints;

namespace Galaxus.Functional.NUnitExtension;

public class ResultConstraintExpression : ResolvableConstraintExpression
{
    public ResultConstraintExpression(ConstraintBuilder builder) : base(builder)
    {
    }

    public ConstraintExpression WithValue
    {
        get
        {
            builder.Append(new OnResultValueOperator());
            return new ConstraintExpression(builder);
        }
    }
}
