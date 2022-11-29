using NUnit.Framework.Constraints;

namespace Galaxus.Functional.NUnitExtension;

public class NoneConstraint : OptionConstraint
{
    protected override ConstraintResult Matches(IOption option)
    {
        Description = "option of none";
        var value = option.ToObject();
        return new ConstraintResult(this, value, value == null);
    }
}
