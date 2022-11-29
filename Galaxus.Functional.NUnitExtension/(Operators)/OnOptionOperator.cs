using NUnit.Framework.Constraints;

namespace Galaxus.Functional.NUnitExtension;

public class OnOptionOperator : AndOperator
{
    public override IConstraint ApplyOperator(IConstraint left, IConstraint right)
    {
        return base.ApplyOperator(left, new ApplyOnOptionValueConstraint(right));
    }
}
