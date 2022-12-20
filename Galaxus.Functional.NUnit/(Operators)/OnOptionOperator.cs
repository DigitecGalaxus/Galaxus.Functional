using NUnit.Framework.Constraints;

namespace Galaxus.Functional.NUnit;

/// <summary>
/// An operator to perform further constraint's on an option's value
/// </summary>
public class OnOptionOperator : AndOperator
{
    /// <inheritdoc />
    public override IConstraint ApplyOperator(IConstraint left, IConstraint right)
    {
        return base.ApplyOperator(left, new ApplyOnOptionValueConstraint(right));
    }
}
