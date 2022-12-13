using NUnit.Framework;
using NUnit.Framework.Constraints;
using NUnit.Framework.Internal;

namespace Galaxus.Functional.NUnitExtension.Test;

[TestFixture]
public class ResultConstraintTest
{
    private readonly Result<long, string> _okResult = 42L;
    private readonly Result<long, string> _errResult = "Failed";

    private record TestClass(int A, long B);

    private readonly Result<TestClass, string> _complexResult = new TestClass(1, 3).ToOk<TestClass, string>();


    #region Is.OK

    [Test]
    public void IsOk_ForOkResult_Succeeds() => Assert.That(Resolve(Is.Ok).ApplyTo(_okResult).Status, Is.EqualTo(ConstraintStatus.Success));

    #region Type

    [Test]
    public void IsOk_OnOtherType_Fails() => Assert.That(Resolve(Is.Ok).ApplyTo("string").Status, Is.EqualTo(ConstraintStatus.Failure));

    [Test]
    public void IsOk_OnOtherType_HasMeaningFullDescription()
        => Assert.That(Resolve(Is.Ok).ApplyTo("string").Description, Is.EqualTo("an object of type Result"));

    [Test]
    public void IsOk_OnOtherType_ActualValueIsTypeOfActualValue()
    {
        var butWas = GetButWas(Resolve(Is.Ok).ApplyTo("string"));
        Assert.That(butWas, Is.EqualTo($"<{typeof(string)}>"));
    }

    #endregion

    #region State

    [Test]
    public void IsOk_ForErrResult_Fails() => Assert.That(Resolve(Is.Ok).ApplyTo(_errResult).Status, Is.EqualTo(ConstraintStatus.Failure));

    [Test]
    public void IsOk_ForErrResult_HasMeaningFullDescription()
        => Assert.That(Resolve(Is.Ok).ApplyTo(_errResult).Description, Is.EqualTo("a result in state Ok"));

    [Test]
    public void IsOk_ForErrResult_ActualValueIsReadable()
    {
        var butWas = GetButWas(Resolve(Is.Ok).ApplyTo(_errResult));
        Assert.That(butWas, Is.EqualTo($"<Err: Failed>"));
    }

    #endregion

    #region WithValue

    [Test]
    public void IsOkWithValue_ForMatchingValue_Succeeds()
        => Assert.That(Resolve(Is.Ok.WithValue.EqualTo(42)).ApplyTo(_okResult).Status, Is.EqualTo(ConstraintStatus.Success));

    [Test]
    public void IsOkWithValue_WithoutMatch_Fails()
        => Assert.That(Resolve(Is.Ok.WithValue.EqualTo(43)).ApplyTo(_okResult).Status, Is.EqualTo(ConstraintStatus.Failure));

    [Test]
    public void IsOkWithValue_WithoutMatch_HasMeaningFullDescription()
        => Assert.That(
            Resolve(Is.Ok.WithValue.EqualTo(43)).ApplyTo(_okResult).Description,
            Is.EqualTo("a result in state Ok and it's value was expected to be 43"));

    [Test]
    public void IsOkWithValue_WithoutMatch_HasReadableActualValue()
    {
        var butWas = GetButWas(Is.Ok.WithValue.EqualTo(43).ApplyTo(_okResult));
        Assert.That(butWas, Is.EqualTo("<Ok: 42>"));
    }

    [Test]
    public void IsOkWithValue_IsChainableWithAnds()
        => Assert.That(Resolve(Is.Ok.WithValue.GreaterThan(40).And.LessThan(44)).ApplyTo(_okResult).Status, Is.EqualTo(ConstraintStatus.Success));

    [Test]
    public void IsOkWithValue_ChainedAnd_HasMeaningfulDescription()
        => Assert.That(
            Resolve(Is.Ok.WithValue.GreaterThan(42).And.LessThan(44)).ApplyTo(_okResult).Description,
            Is.EqualTo("a result in state Ok and it's value was expected to be greater than 42 and less than 44"));

    [Test]
    public void IsOkWithValue_AllowsComplexValueExpression()
        => Assert.That(
            Resolve(Is.Ok.WithValue.Property("A").EqualTo(1).And.Property("B").EqualTo(3)).ApplyTo(_complexResult).Status,
            Is.EqualTo(ConstraintStatus.Success));

    #endregion

    #endregion

    #region Is.Err

    [Test]
    public void IsErr_ForErrResult_Succeeds() => Assert.That(Resolve(Is.Err).ApplyTo(_errResult).Status, Is.EqualTo(ConstraintStatus.Success));

    #region Type

    [Test]
    public void IsErr_OnOtherType_Fails() => Assert.That(Resolve(Is.Err).ApplyTo(3).Status, Is.EqualTo(ConstraintStatus.Failure));

    [Test]
    public void IsErr_OnOtherType_HasMeaningFullDescription()
        => Assert.That(Resolve(Is.Err).ApplyTo(3).Description, Is.EqualTo("an object of type Result"));

    [Test]
    public void IsErr_OnOtherType_ActualValueIsTypeOfActualValue()
    {
        var butWas = GetButWas(Resolve(Is.Err).ApplyTo(3));
        Assert.That(butWas, Is.EqualTo($"<{typeof(int)}>"));
    }

    #endregion

    #region State

    [Test]
    public void IsErr_ForOkResult_Fails() => Assert.That(Resolve(Is.Err).ApplyTo(_okResult).Status, Is.EqualTo(ConstraintStatus.Failure));

    [Test]
    public void IsErr_ForOkResult_HasMeaningFullDescription()
        => Assert.That(Resolve(Is.Err).ApplyTo(_okResult).Description, Is.EqualTo("a result in state Err"));

    [Test]
    public void IsErr_ForOkResult_ActualValueIsReadable()
    {
        var butWas = GetButWas(Resolve(Is.Err).ApplyTo(_okResult));
        Assert.That(butWas, Is.EqualTo($"<Ok: 42>"));
    }

    #endregion

    #region With_Value

    [Test]
    public void IsErrWithValue_ForMatchingValue_Succeeds()
        => Assert.That(Resolve(Is.Err.WithValue.EqualTo("Failed")).ApplyTo(_errResult).Status, Is.EqualTo(ConstraintStatus.Success));

    [Test]
    public void IsErrWithValue_WithoutMatch_Fails()
        => Assert.That(Resolve(Is.Err.WithValue.EqualTo(43)).ApplyTo(_errResult).Status, Is.EqualTo(ConstraintStatus.Failure));

    [Test]
    public void IsErrWithValue_WithoutMatch_HasMeaningFullDescription()
        => Assert.That(
            Resolve(Is.Err.WithValue.EqualTo("fail")).ApplyTo(_errResult).Description,
            Is.EqualTo("a result in state Err and it's value was expected to be \"fail\""));

    [Test]
    public void IsErrWithValue_WithoutMatch_HasReadableActualValue()
    {
        var butWas = GetButWas(Is.Err.WithValue.EqualTo(43).ApplyTo(_errResult));
        Assert.That(butWas, Is.EqualTo("<Err: Failed>"));
    }

    [Test]
    public void IsErrWithValue_IsChainableWithAnds()
        => Assert.That(Resolve(Is.Err.WithValue.StartsWith("F").And.EndsWith("d")).ApplyTo(_errResult).Status, Is.EqualTo(ConstraintStatus.Success));

    [Test]
    public void IsErrWithValue_ChainedAnd_HasMeaningfulDescription()
        => Assert.That(
            Resolve(Is.Err.WithValue.StartsWith("f").And.EndsWith("D").IgnoreCase).ApplyTo(_errResult).Description,
            Is.EqualTo(
                "a result in state Err and it's value was expected to be String starting with \"f\" and String ending with \"D\", ignoring case"));

    #endregion

    #endregion

    #region Examples as used in tests

    [Test]
    public void IsOk_ActuallyWorks() => Assert.That(_okResult, Is.Ok);

    [Test]
    public void IsErr_ActuallyWorks() => Assert.That(_errResult, Is.Err);

    [Test]
    public void IsOk_WithValue_ActuallyWorks() => Assert.That(_okResult, Is.Ok.WithValue.EqualTo(42));

    [Test]
    public void IsErr_WithValue_ActuallyWorks() => Assert.That(_errResult, Is.Err.WithValue.EqualTo("Failed"));


    [Test]
    public void IsOkWithValue_WithComplexValueExpression_ActuallyWorks()
        => Assert.That(_complexResult, Is.Ok.WithValue.Property("A").EqualTo(1).And.Property("B").EqualTo(3));

    [Test]
    public void IsErrWithValue_FurtherChained_ActuallyWorks()
        => Assert.That(_errResult, Is.Err.WithValue.EqualTo("failed").IgnoreCase);

    [Test]
    public void IsErrWithValue_AndChained_ActuallyWorks()
        => Assert.That(_errResult, Is.Err.WithValue.StartWith("F").And.EndWith("d"));


    [Test]
#pragma warning disable NUnit2041 // Incompatible types for comparison constraint
    public void IsOkWithValue_IsChainableWithAnds_ActuallyWorks()
        => Assert.That(_okResult, Is.Ok.WithValue.GreaterThan(40).And.LessThan(44));
#pragma warning restore NUnit2041

    #endregion

    private static IConstraint Resolve(IResolveConstraint expression) => expression.Resolve();

    private static string GetButWas(ConstraintResult result)
    {
        var writer = new TextMessageWriter();
        result.WriteActualValueTo(writer);
        return writer.ToString();
    }
}
