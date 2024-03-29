using NUnit.Framework.Constraints;

namespace Galaxus.Functional.NUnit;

/// <summary>
/// A wrapper constraint to apply an other constraint on a result's value
/// </summary>
public class ApplyOnResultValueConstraint : Constraint
{
    private readonly IConstraint _onValueConstraint;

    /// <inheritdoc />
    public override ConstraintResult ApplyTo<TActual>(TActual actual)
    {
        var actualType = actual.GetType();
        var isResult = actualType.IsGenericType && actualType.GetGenericTypeDefinition() == typeof(Result<,>);
        if (isResult)
        {
            var isOk = (bool)(actualType.GetProperty(nameof(Result<int, int>.IsOk))?.GetValue(actual)
                              ?? throw new ArgumentException("Argument was not a Result"));
            var value = (IOption)((isOk
                                      ? actualType.GetProperty(nameof(Result<int, int>.Ok))?.GetValue(actual)
                                      : actualType.GetProperty(nameof(Result<int, int>.Err))?.GetValue(actual))
                                  ?? throw new InvalidOperationException("Can not test a result without a value"));

            var innerResult = _onValueConstraint.ApplyTo(value.ToObject());
            return new ConstraintResult(_onValueConstraint, actual, innerResult.Status);
        }

        throw new ArgumentException($"The actual value must be a Result. The value passed was of type {typeof(TActual)}", nameof(actual));
    }

    /// <inheritdoc />
    public override string Description => "it's value was expected to be " + _onValueConstraint.Description;

    /// <summary>
    /// Constructs this constraint with a constraint applied on the result's value
    /// </summary>
    /// <param name="constraint">the constraint applied on the result's value</param>
    public ApplyOnResultValueConstraint(IConstraint constraint)
    {
        _onValueConstraint = constraint;
    }
}
