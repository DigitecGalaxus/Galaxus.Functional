using NUnit.Framework.Constraints;

namespace Galaxus.Functional.NUnit;

/// <summary>
/// A wrapper constraint to apply an other constraint on an option's value
/// </summary>
public class ApplyOnOptionValueConstraint : OptionConstraint
{
    private readonly IConstraint _onValueConstraint;

    /// <inheritdoc />
    public override string Description => "it's value to be " + _onValueConstraint.Description;

    /// <summary>
    /// Constructs this constraint with a constraint applied on the option's value
    /// </summary>
    /// <param name="constraint">the constraint applied on the option's value</param>
    public ApplyOnOptionValueConstraint(IConstraint constraint)
    {
        _onValueConstraint = constraint;
    }

    /// <inheritdoc />
    protected override ConstraintResult Matches(IOption option)
    {
        var innerResult = _onValueConstraint.ApplyTo(option.ToObject());
        return new ConstraintResult(this, option.ToObject(), innerResult.Status);
    }
}
