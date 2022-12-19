using NUnit.Framework.Constraints;

namespace Galaxus.Functional.NUnit;

public class ApplyOnResultValueConstraint : Constraint
{
    private readonly IConstraint _onValueConstraint;

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

    public override string Description => "it's value was expected to be " + _onValueConstraint.Description;

    public ApplyOnResultValueConstraint(IConstraint constraint)
    {
        _onValueConstraint = constraint;
    }
}
