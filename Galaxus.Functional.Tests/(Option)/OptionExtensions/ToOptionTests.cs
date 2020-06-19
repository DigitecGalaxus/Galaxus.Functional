using Galaxus.Functional.Tests.Helpers;
using NUnit.Framework;

namespace Galaxus.Functional.Tests.OptionExtensions
{
    [TestFixture]
    public class ToOptionTests
    {
        [Test]
        public void ToOption_ContainsOriginalValue()
        {
            Assert.AreEqual(true, true.ToOption().Unwrap());
            Assert.AreEqual(false, false.ToOption().Unwrap());
            Assert.AreEqual("hello", "hello".ToOption().Unwrap());
            Assert.AreEqual("hello", "hello".ToOption<object>().Unwrap());

            var instance = new DummyReferenceType();
            Assert.AreSame(instance, instance.ToOption().Unwrap());
        }

        [Test]
        public void ToOption_ValueType_ReturnsSome()
        {
            Assert.IsTrue(1.ToOption().IsSome);
        }

        [Test]
        public void ToOption_ReferenceType_ReturnsSome()
        {
            Assert.IsTrue(new DummyReferenceType().ToOption().IsSome);
        }

        [Test]
        public void ToOption_NullValueType_ReturnsNone()
        {
            Assert.IsTrue(((int?)null).ToOption().IsNone);
        }

        [Test]
        public void ToOption_NullReferenceType_ReturnsNone()
        {
            Assert.IsTrue(((DummyReferenceType)null).ToOption().IsNone);
        }
    }
}
