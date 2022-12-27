using NUnit.Framework.Constraints;

namespace Galaxus.Functional.NUnit;

/// <summary>
/// An operator to perform further constraint's on a result's value
/// </summary>
public class OnResultValueOperator : AndOperator
{
    /// <inheritdoc />
    public override IConstraint ApplyOperator(IConstraint left, IConstraint right)
    {
        return base.ApplyOperator(left, new ApplyOnResultValueConstraint(right));
    }
}
