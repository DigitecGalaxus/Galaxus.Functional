using NUnit.Framework.Constraints;

namespace Galaxus.Functional.NUnit;

public class ResultInStateConstraint : Constraint
{
    private readonly bool _isInOkState;

    public ResultInStateConstraint(bool isInOkState)
    {
        _isInOkState = isInOkState;
    }

    public ConstraintExpression WithValue
    {
        get
        {
            ConstraintBuilder builder = Builder;
            if (builder == null)
            {
                builder = new ConstraintBuilder();
                builder.Append(this);
            }

            builder.Append(new OnResultValueOperator());
            return new(builder);
        }
    }

    public override ConstraintResult ApplyTo<TActual>(TActual actual)
    {
        var actualType = actual.GetType();
        var isResult = actualType.IsGenericType && actualType.GetGenericTypeDefinition() == typeof(Result<,>);
        if (!isResult)
        {
            Description = "an object of type Result";
            return new ConstraintResult(this, actualType, ConstraintStatus.Failure);
        }

        Description = $"a result in state {(_isInOkState ? "Ok" : "Err")}";
        var isOk = (bool)(actualType.GetProperty(nameof(Result<int, int>.IsOk))?.GetValue(actual)
                          ?? throw new ArgumentException("Argument was not a Result"));
        var isRightState = isOk == _isInOkState;

        return new ConstraintResult(this, actual, isRightState);
    }
}
