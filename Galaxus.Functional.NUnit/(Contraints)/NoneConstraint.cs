using NUnit.Framework.Constraints;

namespace Galaxus.Functional.NUnit;

public class NoneConstraint : OptionConstraint
{
    protected override ConstraintResult Matches(IOption option)
    {
        Description = "an option of none";
        var value = option.ToObject();
        return new ConstraintResult(this, value, value == null);
    }
}
