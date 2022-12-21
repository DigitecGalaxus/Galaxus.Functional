using OriginalIs = NUnit.Framework.Is;

namespace Galaxus.Functional.NUnit;

/// <inheritdoc />
public class Is : OriginalIs
{

    /// <summary>
    /// A constraint that checks that the actual value is of type <see cref="Option{T}"/> and wraps a <b>None</b>
    /// </summary>
    public static NoneConstraint None => new();

    /// <summary>
    /// A constraint that checks that the actual value is of type <see cref="Option{T}"/> and wraps <b>Some</b> value.
    /// Can be chained with <c>WithValue</c> which applies further chained constraints on the option's value
    /// </summary>
    public static SomeConstraint Some => new();

    /// <summary>
    /// A constraint that checks that the actual value is of type <see cref="Result{TOk,TErr}"/> which is an <b>Ok</b>-Result
    /// Can be chained with <c>WithValue</c> which applies further chained constraints on the result's value
    /// </summary>
    public static ResultInStateConstraint Ok => new(true);

    /// <summary>
    /// A constraint that checks that the actual value is of type <see cref="Result{TOk,TErr}"/> which is an <b>Err</b>-Result
    /// Can be chained with <c>WithValue</c> which applies further chained constraints on the result's value
    /// </summary>
    public static ResultInStateConstraint Err => new(false);
}
