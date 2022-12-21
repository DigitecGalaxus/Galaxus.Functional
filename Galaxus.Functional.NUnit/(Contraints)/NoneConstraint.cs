using NUnit.Framework.Constraints;

namespace Galaxus.Functional.NUnit;

/// <summary>
/// A constraint that checks that the actual value is of type <see cref="Option{T}"/> and wraps a <b>None</b>
/// </summary>
public class NoneConstraint : OptionConstraint
{
    /// <inheritdoc />
    protected override ConstraintResult Matches(IOption option)
    {
        Description = "an option of none";
        var value = option.ToObject();
        return new ConstraintResult(this, value, value == null);
    }
}
