using NUnit.Framework.Constraints;

namespace Galaxus.Functional.NUnit;

/// <summary>
/// The base constraint that checks if an actual value is of type <see cref="Option{T}"/>
/// </summary>
public abstract class OptionConstraint : Constraint
{
    /// <inheritdoc />
    public override ConstraintResult ApplyTo<TActual>(TActual actual)
    {
        if (actual is IOption option)
        {
            return Matches(option);
        }

        Description = "an object of type Option";
        return new ConstraintResult(this, actual.GetType(), false);
    }

    /// <summary>
    /// Subtypes have to implement this method which is performed on the actual value, if it is an <see cref="IOption"/>
    /// </summary>
    /// <param name="option">The actual value as an <see cref="IOption"/></param>
    /// <returns>Information whether actual matches the constraint</returns>
    protected abstract ConstraintResult Matches(IOption option);
}
