using NUnit.Framework.Constraints;

namespace Galaxus.Functional.NUnitExtension;

public class ResultInStateConstraint : Constraint
{
    private readonly bool _isInOkState;

    public override string Description => $"result in state {(_isInOkState ? "Ok" : "Err")}";
    
    public ResultInStateConstraint(bool isInOkState)
    {
        _isInOkState = isInOkState;
    }

    public override ConstraintResult ApplyTo<TActual>(TActual actual)
    {
        var actualType = actual.GetType();
        var isResult = actualType.IsGenericType && actualType.GetGenericTypeDefinition() == typeof(Result<,>);
        if (isResult)
        {
            var isOk = (bool)(actualType.GetProperty(nameof(Result<int, int>.IsOk))?.GetValue(actual)
                              ?? throw new ArgumentException("Argument was not a Result"));
            var isRightState = isOk == _isInOkState;
            
            return new ConstraintResult(this, actual, isRightState);
        }
        
        throw new ArgumentException($"The actual value must be a Result. The value passed was of type {typeof(TActual)}", nameof(actual));
    }
}