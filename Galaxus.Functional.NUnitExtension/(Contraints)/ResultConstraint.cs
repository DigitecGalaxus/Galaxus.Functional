using NUnit.Framework.Constraints;

namespace Galaxus.Functional.NUnitExtension;

public class ResultConstraint : Constraint
{
    public ResultInStateConstraintExpression InState
    {
        get
        {
            ConstraintBuilder builder = Builder;
            if (builder == null)
            {
                builder = new ConstraintBuilder();
                builder.Append(new AndOperator());
                builder.Append(this);
            }

            return new(builder);
        }
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
        Description = "an object of type Result";
        var actualType = actual.GetType();
        var isResult = actualType.IsGenericType && actualType.GetGenericTypeDefinition() == typeof(Result<,>);
        return new ConstraintResult(this, isResult ? actual : actualType, isResult);
    }
}
