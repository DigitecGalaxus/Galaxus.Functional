using NUnit.Framework;

namespace Galaxus.Functional.Tests
{
    public class UnitTests
    {
        [Test]
        public void Unit_AreEqual()
        {
            Assert.AreEqual(expected: Unit.Value, new Unit());
            Assert.AreEqual(new Unit(), new Unit());
        }

        [Test]
        public void Unit_AreNotEqual()
        {
            Assert.AreNotEqual(expected: Unit.Value, null);
            Assert.AreNotEqual(expected: Unit.Value, 0);
        }

        [Test]
        public void Unit_Hash()
        {
            Assert.AreEqual(
                Unit.Value.GetHashCode(),
                new Unit().GetHashCode());
        }

        [Test]
        public void Unit_ToString()
        {
            Assert.AreEqual(
                "()",
                Unit.Value.ToString());
        }
    }
}
