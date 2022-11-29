using NUnit.Framework.Constraints;

namespace Galaxus.Functional.NUnitExtension;

public class SomeConstraint : OptionConstraint
{
    public ConstraintExpression WithValue
    {
        get
        {
            ConstraintBuilder builder = Builder;
            if (builder == null)
            {
                builder = new ConstraintBuilder();
                builder.Append(this);
            }

            builder.Append(new OnOptionOperator());
            return new ConstraintExpression(builder);
        }
    }

    protected override ConstraintResult Matches(IOption option)
    {
        Description = "an option containing some value";
        var value = option.ToObject();
        return new ConstraintResult(this, option, value != null);
    }
}
