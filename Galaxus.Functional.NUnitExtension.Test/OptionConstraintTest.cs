using NUnit.Framework;
using NUnit.Framework.Constraints;
using NUnit.Framework.Internal;

namespace Galaxus.Functional.NUnitExtension.Test;

[TestFixture]
public class OptionConstraintTest
{
    private static readonly Option<int> _some = 3.ToOption();
    private static readonly Option<string> _someString = "Hello World".ToOption();
    private static readonly Option<int> _none = Option<int>.None;

    private class IsNone
    {
        [Test]
        public void OnNone_Succeeds()
        {
            Assert.That(_none, Is.None);
        }

        private class OnSome
        {
            [Test]
            public void Fails()
            {
                Assert.That(Resolve(Is.None).ApplyTo(_some).Status, Is.EqualTo(ConstraintStatus.Failure));
            }

            [Test]
            public void HasMeaningFullDescription()
            {
                Assert.That(Resolve(Is.None).ApplyTo(_some).Description, Is.EqualTo("an option of none"));
            }

            [Test]
            public void ActualValueIsTypeOfActualValue()
            {
                var butWas = GetButWas(Resolve(Is.None).ApplyTo(_some));
                Assert.That(butWas, Is.EqualTo("3"));
            }
        }

        private class OnNonOptionType
        {
            [Test]
            public void Fails()
            {
                Assert.That(Resolve(Is.None).ApplyTo("string").Status, Is.EqualTo(ConstraintStatus.Failure));
            }

            [Test]
            public void HasMeaningFullDescription()
            {
                Assert.That(Resolve(Is.None).ApplyTo("string").Description, Is.EqualTo("an object of type Option"));
            }

            [Test]
            public void ActualValueIsTypeOfActualValue()
            {
                var butWas = GetButWas(Resolve(Is.None).ApplyTo("string"));
                Assert.That(butWas, Is.EqualTo($"<{typeof(string)}>"));
            }
        }
    }

    private class IsSome
    {
        [Test]
        public void OnSome_Succeeds()
        {
            Assert.That(_some, Is.Some);
        }

        private class OnNone
        {
            [Test]
            public void Fails()
            {
                Assert.That(Resolve(Is.Some).ApplyTo(_none).Status, Is.EqualTo(ConstraintStatus.Failure));
            }

            [Test]
            public void HasMeaningFullDescription()
            {
                Assert.That(
                    Resolve(Is.Some).ApplyTo(_none).Description,
                    Is.EqualTo("an option containing some value"));
            }

            [Test]
            public void ActualValueIsTypeOfActualValue()
            {
                var butWas = GetButWas(Resolve(Is.Some).ApplyTo(_none));
                Assert.That(butWas, Is.EqualTo("None"));
            }
        }

        private class OnNonOptionType
        {
            [Test]
            public void Fails()
            {
                Assert.That(Resolve(Is.Some).ApplyTo(3).Status, Is.EqualTo(ConstraintStatus.Failure));
            }

            [Test]
            public void HasMeaningFullDescription()
            {
                Assert.That(Resolve(Is.Some).ApplyTo(3).Description, Is.EqualTo("an object of type Option"));
            }

            [Test]
            public void ActualValueIsTypeOfActualValue()
            {
                var butWas = GetButWas(Resolve(Is.Some).ApplyTo(3));
                Assert.That(butWas, Is.EqualTo($"<{typeof(int)}>"));
            }
        }

        private class WithValue
        {
            [Test]
            public void MatchingConstraint_Succeeds()
            {
                Assert.That(_some, Is.Some.WithValue.EqualTo(3));
            }

            private class OnNone
            {
                [Test]
                public void Fails()
                {
                    Assert.That(Resolve(Is.Some.WithValue.EqualTo(3)).ApplyTo(_none).Status, Is.EqualTo(ConstraintStatus.Failure));
                }

                [Test]
                public void HasMeaningFullDescription()
                {
                    Assert.That(
                        Resolve(Is.Some.WithValue.EqualTo(3)).ApplyTo(_none).Description,
                        Is.EqualTo("an option containing some value and it's value to be 3"));
                }

                [Test]
                public void ActualValueIsNone()
                {
                    var butWas = GetButWas(Resolve(Is.Some.WithValue.EqualTo(3)).ApplyTo(_none));
                    Assert.That(butWas, Is.EqualTo("None"));
                }
            }

            private class OnNonOptionType
            {
                [Test]
                public void Fails()
                {
                    Assert.That(Resolve(Is.Some.WithValue.EqualTo(3)).ApplyTo(true).Status, Is.EqualTo(ConstraintStatus.Failure));
                }

                [Test]
                public void HasMeaningFullDescription()
                {
                    Assert.That(
                        Resolve(Is.Some.WithValue.GreaterThan(2)).ApplyTo(true).Description,
                        Is.EqualTo("an object of type Option and it's value to be greater than 2"));
                }

                [Test]
                public void ActualValueIsTypeOfActualValue()
                {
                    var butWas = GetButWas(Resolve(Is.Some.WithValue.GreaterThan(2)).ApplyTo(true));
                    Assert.That(butWas, Is.EqualTo($"<{typeof(bool)}>"));
                }
            }

            private class TestedOnMultipleConstraints
            {
                [Test]
                public void MatchingAll_Succeeds()
                {
#pragma warning disable NUnit2041 // Incompatible types for comparison constraint
                    Assert.That(_some, Is.Some.WithValue.GreaterThan(2).And.LessThan(4));
#pragma warning restore NUnit2041
                }

                [Test]
                public void PerformedOnOptionOfString()
                {
                    Assert.That(_someString, Is.Some.WithValue.StartWith("hello").IgnoreCase.And.EndsWith("World").And.Length.GreaterThan(5));
                }
            }
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
