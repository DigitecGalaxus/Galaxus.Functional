using NUnit.Framework;
using NUnit.Framework.Constraints;
using NUnit.Framework.Internal;

namespace Galaxus.Functional.NUnitExtension.Test;

[TestFixture]
public class ResultConstraintTest
{
    private static readonly Result<long, string> _okResult = 42L;
    private static readonly Result<long, string> _errResult = "Failed";

    private record TestClass(int A, long B);

    private static readonly Result<TestClass, string> _complexResult = new TestClass(1, 3).ToOk<TestClass, string>();

    private class IsOk
    {
        [Test]
        public void OnOkResult_Succeeds()
        {
            Assert.That(Resolve(Is.Ok).ApplyTo(_okResult).Status, Is.EqualTo(ConstraintStatus.Success));
        }

        private class OnOtherType
        {
            [Test]
            public void Fails()
            {
                Assert.That(Resolve(Is.Ok).ApplyTo("string").Status, Is.EqualTo(ConstraintStatus.Failure));
            }

            [Test]
            public void HasMeaningFullDescription()
            {
                Assert.That(Resolve(Is.Ok).ApplyTo("string").Description, Is.EqualTo("an object of type Result"));
            }

            [Test]
            public void ActualValueIsTypeOfActualValue()
            {
                var butWas = GetButWas(Resolve(Is.Ok).ApplyTo("string"));
                Assert.That(butWas, Is.EqualTo($"<{typeof(string)}>"));
            }
        }

        private class OnErrResult
        {
            [Test]
            public void Fails()
            {
                Assert.That(Resolve(Is.Ok).ApplyTo(_errResult).Status, Is.EqualTo(ConstraintStatus.Failure));
            }

            [Test]
            public void HasMeaningFullDescription()
            {
                Assert.That(Resolve(Is.Ok).ApplyTo(_errResult).Description, Is.EqualTo("a result in state Ok"));
            }

            [Test]
            public void ActualValueIsReadable()
            {
                var butWas = GetButWas(Resolve(Is.Ok).ApplyTo(_errResult));
                Assert.That(butWas, Is.EqualTo($"<Err: Failed>"));
            }
        }

        private class WithValue
        {
            [Test]
            public void MatchingConstraint_Succeeds()
            {
                Assert.That(Resolve(Is.Ok.WithValue.EqualTo(42)).ApplyTo(_okResult).Status, Is.EqualTo(ConstraintStatus.Success));
            }

            private class NotMatchingConstraint
            {
                [Test]
                public void Fails()
                {
                    Assert.That(Resolve(Is.Ok.WithValue.EqualTo(43)).ApplyTo(_okResult).Status, Is.EqualTo(ConstraintStatus.Failure));
                }

                [Test]
                public void HasMeaningFullDescription()
                {
                    Assert.That(
                        Resolve(Is.Ok.WithValue.EqualTo(43)).ApplyTo(_okResult).Description,
                        Is.EqualTo("a result in state Ok and it's value was expected to be 43"));
                }

                [Test]
                public void HasReadableActualValue()
                {
                    var butWas = GetButWas(Is.Ok.WithValue.EqualTo(43).ApplyTo(_okResult));
                    Assert.That(butWas, Is.EqualTo("<Ok: 42>"));
                }
            }

            private class MultipleConstraintsChainedWithAnd
            {
                [Test]
                public void MatchingAllConstraints_Succeeds()
                {
                    Assert.That(
                        Resolve(Is.Ok.WithValue.GreaterThan(40).And.LessThan(44)).ApplyTo(_okResult).Status,
                        Is.EqualTo(ConstraintStatus.Success));
                }

                [Test]
                public void BreakingAllConstraints_Fails()
                {
                    Assert.That(
                        Resolve(Is.Ok.WithValue.GreaterThan(44).And.LessThan(40)).ApplyTo(_okResult).Status,
                        Is.EqualTo(ConstraintStatus.Failure));
                }

                [Test]
                public void BreakingFirstConstraints_Fails()
                {
                    Assert.That(
                        Resolve(Is.Ok.WithValue.GreaterThan(44).And.LessThan(45)).ApplyTo(_okResult).Status,
                        Is.EqualTo(ConstraintStatus.Failure));
                }

                [Test]
                public void BreakingSecondConstraints_Fails()
                {
                    Assert.That(
                        Resolve(Is.Ok.WithValue.GreaterThan(40).And.LessThan(41)).ApplyTo(_okResult).Status,
                        Is.EqualTo(ConstraintStatus.Failure));
                }

                [Test]
                public void HasMeaningfulDescription()
                {
                    Assert.That(
                        Resolve(Is.Ok.WithValue.GreaterThan(42).And.LessThan(44)).ApplyTo(_okResult).Description,
                        Is.EqualTo("a result in state Ok and it's value was expected to be greater than 42 and less than 44"));
                }

                [Test]
                public void AllowsComplexValueExpression()
                {
                    Assert.That(
                        Resolve(Is.Ok.WithValue.Property("A").EqualTo(1).And.Property("B").EqualTo(3)).ApplyTo(_complexResult).Status,
                        Is.EqualTo(ConstraintStatus.Success));
                }
            }
        }
    }

    private class IsErr
    {
        [Test]
        public void OnErrResult_Succeeds()
        {
            Assert.That(Resolve(Is.Err).ApplyTo(_errResult).Status, Is.EqualTo(ConstraintStatus.Success));
        }

        private class OnOtherType
        {
            [Test]
            public void Fails()
            {
                Assert.That(Resolve(Is.Err).ApplyTo(3).Status, Is.EqualTo(ConstraintStatus.Failure));
            }

            [Test]
            public void HasMeaningFullDescription()
            {
                Assert.That(Resolve(Is.Err).ApplyTo(3).Description, Is.EqualTo("an object of type Result"));
            }

            [Test]
            public void ActualValueIsTypeOfActualValue()
            {
                var butWas = GetButWas(Resolve(Is.Err).ApplyTo(3));
                Assert.That(butWas, Is.EqualTo($"<{typeof(int)}>"));
            }
        }

        private class OnOkResult
        {
            [Test]
            public void Fails()
            {
                Assert.That(Resolve(Is.Err).ApplyTo(_okResult).Status, Is.EqualTo(ConstraintStatus.Failure));
            }

            [Test]
            public void HasMeaningFullDescription()
            {
                Assert.That(Resolve(Is.Err).ApplyTo(_okResult).Description, Is.EqualTo("a result in state Err"));
            }

            [Test]
            public void ActualValueIsReadable()
            {
                var butWas = GetButWas(Resolve(Is.Err).ApplyTo(_okResult));
                Assert.That(butWas, Is.EqualTo($"<Ok: 42>"));
            }
        }

        private class WithValue
        {
            [Test]
            public void MatchingConstraint_Succeeds()
            {
                Assert.That(Resolve(Is.Err.WithValue.EqualTo("Failed")).ApplyTo(_errResult).Status, Is.EqualTo(ConstraintStatus.Success));
            }

            private class BreakingConstraint
            {
                [Test]
                public void Fails()
                {
                    Assert.That(Resolve(Is.Err.WithValue.EqualTo(43)).ApplyTo(_errResult).Status, Is.EqualTo(ConstraintStatus.Failure));
                }

                [Test]
                public void HasMeaningFullDescription()
                {
                    Assert.That(
                        Resolve(Is.Err.WithValue.EqualTo("fail")).ApplyTo(_errResult).Description,
                        Is.EqualTo("a result in state Err and it's value was expected to be \"fail\""));
                }

                [Test]
                public void HasReadableActualValue()
                {
                    var butWas = GetButWas(Is.Err.WithValue.EqualTo(43).ApplyTo(_errResult));
                    Assert.That(butWas, Is.EqualTo("<Err: Failed>"));
                }
            }

            private class TestedOnMultipleChainedConstraints
            {
                [Test]
                public void MatchingAll_Succeeds()
                {
                    Assert.That(
                        Resolve(Is.Err.WithValue.StartsWith("F").And.EndsWith("d")).ApplyTo(_errResult).Status,
                        Is.EqualTo(ConstraintStatus.Success));
                }

                [Test]
                public void HasMeaningfulDescription()
                {
                    Assert.That(
                        Resolve(Is.Err.WithValue.StartsWith("f").And.EndsWith("D").IgnoreCase).ApplyTo(_errResult).Description,
                        Is.EqualTo(
                            "a result in state Err and it's value was expected to be String starting with \"f\" and String ending with \"D\", ignoring case"));
                }
            }
        }
    }

    private class RealWorldExamples
    {
        [Test]
        public void IsOk()
        {
            Assert.That(_okResult, Is.Ok);
        }

        [Test]
        public void IsErr()
        {
            Assert.That(_errResult, Is.Err);
        }

        [Test]
        public void IsOk_WithValue()
        {
            Assert.That(_okResult, Is.Ok.WithValue.EqualTo(42));
        }

        [Test]
        public void IsErr_WithValue()
        {
            Assert.That(_errResult, Is.Err.WithValue.EqualTo("Failed"));
        }

        [Test]
        public void IsOkWithValue_WithComplexValueExpression()
        {
            Assert.That(_complexResult, Is.Ok.WithValue.Property("A").EqualTo(1).And.Property("B").EqualTo(3));
        }

        [Test]
        public void IsErrWithValue_FurtherChained()
        {
            Assert.That(_errResult, Is.Err.WithValue.EqualTo("failed").IgnoreCase);
        }

        [Test]
        public void IsErrWithValue_ConstraintsChainedWithAnd()
        {
            Assert.That(_errResult, Is.Err.WithValue.StartWith("F").And.EndWith("d"));
        }

        [Test]
        public void IsOkWithValue_IsChainableWithAnds()
        {
#pragma warning disable NUnit2041 // Incompatible types for comparison constraint
            Assert.That(_okResult, Is.Ok.WithValue.GreaterThan(40).And.LessThan(44));
#pragma warning restore NUnit2041
        }
    }

    private static IConstraint Resolve(IResolveConstraint expression) => expression.Resolve();

    private static string GetButWas(ConstraintResult result)
    {
        var writer = new TextMessageWriter();
        result.WriteActualValueTo(writer);
        return writer.ToString();
    }
}
