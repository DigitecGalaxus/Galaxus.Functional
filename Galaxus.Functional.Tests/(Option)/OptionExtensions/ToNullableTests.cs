using NUnit.Framework;

namespace Galaxus.Functional.Tests.OptionExtensions
{
    [TestFixture]
    public class ToNullableTests
    {
        [Test]
        public void ToNullable_NonNull_ReturnsOriginalValue()
        {
            Assert.AreEqual(true, true.ToOption().ToNullable());
            Assert.AreEqual(false, false.ToOption().ToNullable());
        }

        [Test]
        public void ToNullable_Null_ReturnsDefaultValue()
        {
            Assert.AreEqual(default(bool?), Option<bool>.None.ToNullable());
        }
    }
}
