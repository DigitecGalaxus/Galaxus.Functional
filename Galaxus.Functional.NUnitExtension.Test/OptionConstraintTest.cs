using NUnit.Framework;
using NUnit.Framework.Constraints;

namespace Galaxus.Functional.NUnitExtension.Test
{
    [TestFixture]
    public class OptionConstraintTest
    {
        private readonly Option<int> _some = 3.ToOption();
        private readonly Option<int> _none = Option<int>.None;

        private const string Description = nameof(ConstraintResult.Description);
        private const string Status = nameof(ConstraintResult.Status);
        private const string ActualValue = nameof(ConstraintResult.ActualValue);

        [Test]
        public void OptionNone_Works()
        {
            Assert.Multiple(
                () =>
                {
                    Assert.That(Resolve(Is.OptionOfNone).ApplyTo(_none).Status, Is.EqualTo(ConstraintStatus.Success));

                    Assert.That(
                        Resolve(Is.OptionOfNone).ApplyTo(_some),
                        Has.Property(Status).EqualTo(ConstraintStatus.Failure)
                            .And.Property(Description).EqualTo("option of none")
                            .And.Property(ActualValue).EqualTo(_some.Unwrap()));

                    Assert.That(
                        Resolve(Is.OptionOfNone).ApplyTo("string"),
                        Has.Property(Status).EqualTo(ConstraintStatus.Failure)
                            .And.Property(Description).EqualTo("an object of type Option")
                            .And.Property(ActualValue).EqualTo(typeof(string)));
                });
        }

        [Test]
        public void OptionSome_Works()
        {
            Assert.Multiple(
                () =>
                {
                    Assert.That(
                        Resolve(Is.OptionOfSome).ApplyTo(_none),
                        Has.Property(Status).EqualTo(ConstraintStatus.Failure)
                            .And.Property(Description).EqualTo("an option containing some value")
                            .And.Property(ActualValue).EqualTo(Option<int>.None));

                    Assert.That(Resolve(Is.OptionOfSome).ApplyTo(_some).Status, Is.EqualTo(ConstraintStatus.Success));

                    Assert.That(
                        Resolve(Is.OptionOfSome).ApplyTo("string"),
                        Has.Property(Status).EqualTo(ConstraintStatus.Failure)
                            .And.Property(Description).EqualTo("an object of type Option")
                            .And.Property(ActualValue).EqualTo(typeof(string)));
                });
        }

        [Test]
        public void OptionSome_ChainableToFurtherExpressions()
        {
            Assert.Multiple(
                () =>
                {
                    Assert.That(
                        Resolve(Is.OptionOfSome.WithValue.EqualTo(2)).ApplyTo(_none),
                        Has.Property(Status).EqualTo(ConstraintStatus.Failure)
                            .And.Property(Description)
                            .EqualTo("an option containing some value and it's value was expected: 2"));

                    Assert.That(
                        Resolve(Is.OptionOfSome.WithValue.EqualTo(2)).ApplyTo("string"),
                        Has.Property(Status).EqualTo(ConstraintStatus.Failure)
                            .And.Property(Description)
                            .EqualTo("an object of type Option and it's value was expected: 2"));

                    Assert.That(
                        Resolve(Is.OptionOfSome.WithValue.EqualTo(_some.Unwrap())).ApplyTo(_some).Status,
                        Is.EqualTo(ConstraintStatus.Success));

                    var expectationWhenDifferentToActualValue = _some.Unwrap() + 1;
                    Assert.That(
                        Resolve(Is.OptionOfSome.WithValue.EqualTo(expectationWhenDifferentToActualValue))
                            .ApplyTo(_some),
                        Has.Property(Status).EqualTo(ConstraintStatus.Failure)
                            .And.Property(ActualValue).EqualTo(_some)
                            .And.Property(Description).EqualTo(
                                "an option containing some value and it's value was expected: "
                                + expectationWhenDifferentToActualValue));

                    Assert.That(
                        Resolve(Is.OptionOfSome.WithValue.GreaterThan(0)).ApplyTo(_some),
                        Has.Property(Status).EqualTo(ConstraintStatus.Success)
                            .And.Property(Description).EqualTo(
                                "an option containing some value and it's value was expected: greater than 0"));
                });
        }

        private static IConstraint Resolve(IResolveConstraint expression) => expression.Resolve();
    }
}
