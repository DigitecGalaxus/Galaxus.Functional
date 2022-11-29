using NUnit.Framework.Constraints;

namespace Galaxus.Functional.NUnitExtension;

public abstract class OptionConstraint : Constraint
{
    public override ConstraintResult ApplyTo<TActual>(TActual actual)
    {
        if (actual is IOption option)
        {
            return Matches(option);
        }

        Description = "an object of type Option";
        return new ConstraintResult(this, actual.GetType(), false);
    }

    protected abstract ConstraintResult Matches(IOption option);
}
