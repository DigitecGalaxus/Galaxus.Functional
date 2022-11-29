using NUnit.Framework;
using NUnit.Framework.Constraints;
using NUnit.Framework.Internal;

namespace Galaxus.Functional.NUnitExtension.Test;

[TestFixture]
public class ResultConstraintTest
{
    private readonly Result<long, string> _okResult = 42L;
    private readonly Result<long, string> _errResult = "Failed";
    private readonly Result<string, string> _okOnSameTypeResult = "ok".ToOk<string, string>();

    private record TestClass(int A, long B);

    private readonly Result<TestClass, string> _complexResult = new TestClass(1, 3).ToOk<TestClass, string>();
    private const string Description = nameof(ConstraintResult.Description);
    private const string Status = nameof(ConstraintResult.Status);
    private const string ActualValue = nameof(ConstraintResult.ActualValue);

    [Test]
    public void IsResult_Works()
    {
        Assert.Multiple(
            () =>
            {
                Assert.That(
                    Resolve(Is.Result).ApplyTo(_okResult),
                    Has.Property(Status).EqualTo(ConstraintStatus.Success)
                        .And.Property(Description).EqualTo("an object of type Result"));

                Assert.That(Resolve(Is.Result).ApplyTo(_errResult).Status, Is.EqualTo(ConstraintStatus.Success));

                Assert.That(
                    Resolve(Is.Result).ApplyTo("string"),
                    Has.Property(Status).EqualTo(ConstraintStatus.Failure)
                        .And.Property(ActualValue).EqualTo(typeof(string)));
            });
    }

    [Test]
    public void IsResult_InStateOk_FailsForNonResultTypes()
    {
        Assert.Multiple(
            () =>
            {
                var butWas = GetButWas(Resolve(Is.Result.InState.Ok).ApplyTo("string"));
                Assert.That(
                    Resolve(Is.Result.InState.Ok).ApplyTo("string"),
                    Has.Property(Status).EqualTo(ConstraintStatus.Failure)
                        .And.Property(Description).EqualTo("an object of type Result and result in state Ok"));
                Assert.That(butWas, Is.EqualTo($"<{typeof(string)}>"));
            });
    }

    [Test]
    public void IsResult_InStateOk_FailsForErrResult()
    {
        Assert.Multiple(
            () =>
            {
                var butWas = GetButWas(Resolve(Is.Result.InState.Ok).ApplyTo(_errResult));
                Assert.That(
                    Resolve(Is.Result.InState.Ok).ApplyTo(_errResult),
                    Has.Property(Status).EqualTo(ConstraintStatus.Failure)
                        .And.Property(ActualValue).EqualTo(_errResult)
                        .And.Property(Description).EqualTo("an object of type Result and result in state Ok"));
                Assert.That(butWas, Is.EqualTo($"<{_errResult}>"));
            });
    }

    [Test]
    public void IsResult_InStateOk_SucceedsForOkResult()
    {
        Assert.That(
            Resolve(Is.Result.InState.Ok).ApplyTo(_okResult).Status,
            Is.EqualTo(ConstraintStatus.Success));
    }


    [Test]
    public void IsResult_InStateErr_FailsForNonResultTypes()
    {
        Assert.Multiple(
            () =>
            {
                var butWas = GetButWas(Resolve(Is.Result.InState.Err).ApplyTo("string"));
                Assert.That(
                    Resolve(Is.Result.InState.Err).ApplyTo("string"),
                    Has.Property(Status).EqualTo(ConstraintStatus.Failure)
                        .And.Property(Description).EqualTo("an object of type Result and result in state Err"));
                Assert.That(butWas, Is.EqualTo($"<{typeof(string)}>"));
            });
    }

    [Test]
    public void IsResult_InStateErr_FailsForOkResult()
    {
        Assert.Multiple(
            () =>
            {
                var butWas = GetButWas(Resolve(Is.Result.InState.Err).ApplyTo(_okResult));
                Assert.That(
                    Resolve(Is.Result.InState.Err).ApplyTo(_okResult),
                    Has.Property(Status).EqualTo(ConstraintStatus.Failure)
                        .And.Property(ActualValue).EqualTo(_okResult)
                        .And.Property(Description).EqualTo("an object of type Result and result in state Err"));
                Assert.That(butWas, Is.EqualTo($"<{_okResult}>"));
            });
    }

    [Test]
    public void IsResult_InStateErr_SucceedsForErrResult()
    {
        Assert.That(
            Resolve(Is.Result.InState.Err).ApplyTo(_errResult).Status,
            Is.EqualTo(ConstraintStatus.Success));
    }


    [Test]
    public void IsResult_WithValue_SucceedsIfValueMatches()
    {
        Assert.Multiple(
            () =>
            {
                Assert.That(
                    Resolve(Is.Result.WithValue.EqualTo(_errResult.Err.Unwrap())).ApplyTo(_errResult).Status,
                    Is.EqualTo(ConstraintStatus.Success));
                Assert.That(
                    Resolve(Is.Result.WithValue.EqualTo(_okResult.Ok.Unwrap())).ApplyTo(_okResult).Status,
                    Is.EqualTo(ConstraintStatus.Success));
                Assert.That(
                    Resolve(Is.Result.WithValue.EqualTo("ok")).ApplyTo(_okOnSameTypeResult).Status,
                    Is.EqualTo(ConstraintStatus.Success));
            });
    }

    [Test]
    public void IsResult_WithValue_FailsForNonResultType()
    {
        Assert.Multiple(
            () =>
            {
                var butWas = GetButWas(Resolve(Is.Result.WithValue.EqualTo("string")).ApplyTo("string"));
                Assert.That(
                    Resolve(Is.Result.WithValue.EqualTo("string")).ApplyTo("string"),
                    Has.Property(Status).EqualTo(ConstraintStatus.Failure)
                        .And.Property(Description)
                        .EqualTo("an object of type Result and it's value was expected to be \"string\"")
                        .And.Property(ActualValue).EqualTo("string"));
                Assert.That(butWas, Is.EqualTo($"<{typeof(string)}>"));
            });
    }

    [Test]
    public void IsResult_WithValue_FailsIfValueDoesNotMatch()
    {
        Assert.Multiple(
            () =>
            {
                Assert.That(
                    Resolve(Is.Result.WithValue.EqualTo(42L)).ApplyTo(_errResult),
                    Has.Property(Status).EqualTo(ConstraintStatus.Failure)
                        .And.Property(Description)
                        .EqualTo("an object of type Result and it's value was expected to be 42")
                        .And.Property(ActualValue).EqualTo(_errResult));
            });
    }

    [Test]
    public void IsResult_InState_WithValue_SucceedsIfAllMatch()
    {
        Assert.Multiple(
            () =>
            {
                Assert.That(
                    Resolve(Is.Result.InState.Ok.WithValue.EqualTo(_okResult.Ok.Unwrap()))
                        .ApplyTo(_okResult).Status,
                    Is.EqualTo(ConstraintStatus.Success));
                Assert.That(
                    Resolve(Is.Result.InState.Err.WithValue.EqualTo(_errResult.Err.Unwrap()))
                        .ApplyTo(_errResult).Status,
                    Is.EqualTo(ConstraintStatus.Success));
            });
    }

    [Test]
    public void IsResult_InState_WithValue_FailsForNonResultType()
    {
        Assert.That(
            Resolve(Is.Result.InState.Ok.WithValue.EqualTo("string")).ApplyTo("string"),
            Has.Property(Status).EqualTo(ConstraintStatus.Failure)
                .And.Property(Description).EqualTo(
                    "an object of type Result and result in state Ok and it's value was expected to be \"string\""));
    }

    [Test]
    public void IsResult_InState_WithValue_FailsForResultInDifferentState()
    {
        Assert.That(
            Resolve(Is.Result.InState.Err.WithValue.EqualTo(_okResult.Ok.Unwrap())).ApplyTo(_okResult),
            Has.Property(Status).EqualTo(ConstraintStatus.Failure)
                .And.Property(Description).EqualTo(
                    $"an object of type Result and result in state Err and it's value was expected to be {_okResult.Ok.Unwrap()}"));
    }

    [Test]
    public void IsResult_InState_WithValue_FailsForResultValueNoteMatching()
    {
        Assert.That(
            Resolve(Is.Result.InState.Ok.WithValue.EqualTo('c')).ApplyTo(_okResult),
            Has.Property(Status).EqualTo(ConstraintStatus.Failure)
                .And.Property(Description).EqualTo(
                    "an object of type Result and result in state Ok and it's value was expected to be 'c'"));
    }

    [Test]
    public void IsResult_InState_WithValue_AllowsComplexValueExpression()
    {
        Assert.That(
            Resolve(Is.Result.InState.Ok.WithValue.Property("A").EqualTo(1).And.Property("B").EqualTo(3)).ApplyTo(_complexResult),
            Has.Property(Status).EqualTo(ConstraintStatus.Success));
    }

    private static IConstraint Resolve(IResolveConstraint expression) => expression.Resolve();

    private static string GetButWas(ConstraintResult result)
    {
        var writer = new TextMessageWriter();
        result.WriteActualValueTo(writer);
        return writer.ToString();
    }
}
