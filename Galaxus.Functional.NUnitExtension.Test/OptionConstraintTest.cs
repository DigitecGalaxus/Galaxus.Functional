using NUnit.Framework;
using NUnit.Framework.Constraints;
using NUnit.Framework.Internal;

namespace Galaxus.Functional.NUnitExtension.Test
{
    [TestFixture]
    public class OptionConstraintTest
    {
        private readonly Option<int> _some = 3.ToOption();
        private readonly Option<string> _someString = "Hello World".ToOption();
        private readonly Option<int> _none = Option<int>.None;

        #region Is.None

        [Test]
        public void IsNone_OnNone_Succeeds() => Assert.That(_none, Is.None);

        [Test]
        public void IsNone_OnSome_Fails() => Assert.That(Resolve(Is.None).ApplyTo(_some).Status, Is.EqualTo(ConstraintStatus.Failure));

        [Test]
        public void IsNone_OnSome_HasMeaningFullDescription()
            => Assert.That(Resolve(Is.None).ApplyTo(_some).Description, Is.EqualTo("an option of none"));

        [Test]
        public void IsNone_OnSome_ActualValueIsTypeOfActualValue()
        {
            var butWas = GetButWas(Resolve(Is.None).ApplyTo(_some));
            Assert.That(butWas, Is.EqualTo("3"));
        }

        [Test]
        public void IsNone_OnDifferentType_Fails() => Assert.That(Resolve(Is.None).ApplyTo(_some).Status, Is.EqualTo(ConstraintStatus.Failure));

        [Test]
        public void IsNone_OnDifferentType_HasMeaningFullDescription()
            => Assert.That(Resolve(Is.None).ApplyTo(_some).Description, Is.EqualTo("an option of none"));

        [Test]
        public void IsNone_OnDifferentType_ActualValueIsTypeOfActualValue()
        {
            var butWas = GetButWas(Resolve(Is.None).ApplyTo("string"));
            Assert.That(butWas, Is.EqualTo($"<{typeof(string)}>"));
        }

        #endregion

        #region Is.Some

        [Test]
        public void IsSome_OnSome_Succeeds() => Assert.That(_some, Is.Some);

        [Test]
        public void IsSome_OnNone_Fails() => Assert.That(Resolve(Is.Some).ApplyTo(_none).Status, Is.EqualTo(ConstraintStatus.Failure));

        [Test]
        public void IsSome_OnNone_HasMeaningFullDescription() => Assert.That(
            Resolve(Is.Some).ApplyTo(_none).Description,
            Is.EqualTo("an option containing some value"));

        [Test]
        public void IsSome_OnNone_ActualValueIsTypeOfActualValue()
        {
            var butWas = GetButWas(Resolve(Is.Some).ApplyTo(_none));
            Assert.That(butWas, Is.EqualTo("None"));
        }

        [Test]
        public void IsSome_OnDifferentType_Fails() => Assert.That(Resolve(Is.Some).ApplyTo(_none).Status, Is.EqualTo(ConstraintStatus.Failure));

        [Test]
        public void IsSome_OnDifferentType_HasMeaningFullDescription()
            => Assert.That(Resolve(Is.Some).ApplyTo(_none).Description, Is.EqualTo("an option containing some value"));

        [Test]
        public void IsSome_OnDifferentType_ActualValueIsTypeOfActualValue()
        {
            var butWas = GetButWas(Resolve(Is.Some).ApplyTo(3));
            Assert.That(butWas, Is.EqualTo($"<{typeof(int)}>"));
        }

        #endregion

        #region Is.Some.WithValue

        [Test]
        public void IsSomeWithValue_WithMatchingValue_Succeeds() => Assert.That(_some, Is.Some.WithValue.EqualTo(3));

        [Test]
        public void IsSomeWithValue_OnNone_Fails()
            => Assert.That(Resolve(Is.Some.WithValue.EqualTo(3)).ApplyTo(_none).Status, Is.EqualTo(ConstraintStatus.Failure));

        [Test]
        public void IsSomeWithValue_OnNone_HasMeaningFullDescription()
            => Assert.That(
                Resolve(Is.Some.WithValue.EqualTo(3)).ApplyTo(_none).Description,
                Is.EqualTo("an option containing some value and it's value to be 3"));

        [Test]
        public void IsSomeWithValue_OnNone_ActualValueIsTypeOfActualValue()
        {
            var butWas = GetButWas(Resolve(Is.Some.WithValue.EqualTo(3)).ApplyTo(_none));
            Assert.That(butWas, Is.EqualTo("None"));
        }

        [Test]
        public void IsSomeWithValue_OnDifferentType_Fails()
            => Assert.That(Resolve(Is.Some.WithValue.EqualTo(3)).ApplyTo(_none).Status, Is.EqualTo(ConstraintStatus.Failure));

        [Test]
        public void IsSomeWithValue_OnDifferentType_HasMeaningFullDescription()
            => Assert.That(
                Resolve(Is.Some.WithValue.GreaterThan(2)).ApplyTo(_none).Description,
                Is.EqualTo("an option containing some value and it's value to be greater than 2"));

        [Test]
        public void IsSomeWithValue_OnDifferentType_ActualValueIsTypeOfActualValue()
        {
            var butWas = GetButWas(Resolve(Is.Some.WithValue.GreaterThan(2)).ApplyTo(true));
            Assert.That(butWas, Is.EqualTo($"<{typeof(bool)}>"));
        }

#pragma warning disable NUnit2041 // Incompatible types for comparison constraint
        [Test]
        public void IsSomeWithValue_IsAndChainable() => Assert.That(_some, Is.Some.WithValue.GreaterThan(2).And.LessThan(4));
#pragma warning restore NUnit2041

        [Test]
        public void IsSomeWithValue_IsStringConstraintChainable()
            => Assert.That(_someString, Is.Some.WithValue.StartWith("hello").IgnoreCase.And.EndsWith("World").And.Length.GreaterThan(5));

        #endregion

        private static IConstraint Resolve(IResolveConstraint expression) => expression.Resolve();

        private static string GetButWas(ConstraintResult result)
        {
            var writer = new TextMessageWriter();
            result.WriteActualValueTo(writer);
            return writer.ToString();
        }
    }
}
