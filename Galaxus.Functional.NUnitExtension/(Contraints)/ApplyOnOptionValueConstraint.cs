using NUnit.Framework.Constraints;

namespace Galaxus.Functional.NUnitExtension;

public class ApplyOnOptionValueConstraint : OptionConstraint
{
    private readonly IConstraint _onValueConstraint;
    public override string Description => "it's value was expected: " + _onValueConstraint.Description;

    public ApplyOnOptionValueConstraint(IConstraint constraint)
    {
        _onValueConstraint = constraint;
    }

    protected override ConstraintResult Matches(IOption option)
    {
        var innerResult = _onValueConstraint.ApplyTo(option.ToObject());
        return new ConstraintResult(this, option.ToObject(), innerResult.Status);
    }
}
