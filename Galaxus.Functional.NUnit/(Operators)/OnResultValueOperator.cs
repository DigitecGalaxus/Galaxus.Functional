using NUnit.Framework.Constraints;

namespace Galaxus.Functional.NUnit;

public class OnResultValueOperator : AndOperator
{
    public override IConstraint ApplyOperator(IConstraint left, IConstraint right)
    {
        return base.ApplyOperator(left, new ApplyOnResultValueConstraint(right));
    }
}
