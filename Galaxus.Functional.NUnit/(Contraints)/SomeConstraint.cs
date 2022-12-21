using NUnit.Framework.Constraints;

namespace Galaxus.Functional.NUnit;

/// <summary>
/// A constraint that checks that the actual value is of type <see cref="Option{T}"/> and wraps <b>Some</b> value.
/// Can be chained with <c>WithValue</c> which applies further chained constraints on the option's value
/// </summary>
public class SomeConstraint : OptionConstraint
{
    /// <summary>
    /// Applies further chained constraints on the options value
    /// </summary>
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

    /// <inheritdoc />
    protected override ConstraintResult Matches(IOption option)
    {
        Description = "an option containing some value";
        var value = option.ToObject();
        return new ConstraintResult(this, option, value != null);
    }
}
