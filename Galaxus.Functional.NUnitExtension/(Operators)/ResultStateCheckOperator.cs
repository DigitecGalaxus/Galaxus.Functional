using NUnit.Framework.Constraints;

namespace Galaxus.Functional.NUnitExtension;

public class ResultStateCheckOperator : SelfResolvingOperator
{
    private readonly bool _isInStateOk;

    public ResultStateCheckOperator(bool isInStateOk)
    {
        _isInStateOk = isInStateOk;
    }

    public override void Reduce(ConstraintBuilder.ConstraintStack stack)
    {
        stack.Push(new ResultInStateConstraint(_isInStateOk));
    }
}
