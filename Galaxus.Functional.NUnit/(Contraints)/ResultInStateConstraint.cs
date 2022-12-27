using NUnit.Framework.Constraints;

namespace Galaxus.Functional.NUnit;

/// <summary>
/// A aonstraint that checks if an actual value is of type <see cref="Result{TOk,TErr}"/> and is in a specified State
/// </summary>
public class ResultInStateConstraint : Constraint
{
    private readonly bool _isInOkState;

    /// <summary>
    /// Constructor specifying the state which state the actual value should represent
    /// </summary>
    /// <param name="isInOkState"><c>true</c> for <b>OK</b>-Results, <c>false</c> for <b>ERR</b>-Results</param>
    public ResultInStateConstraint(bool isInOkState)
    {
        _isInOkState = isInOkState;
    }

    /// <summary>
    /// Applies further chained constraints on the result's value
    /// </summary>
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

    /// <inheritdoc />
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
