using NUnit.Framework;

namespace Galaxus.Functional.Tests
{
    public class UnitTests
    {
        [Test]
        public void Unit_AreEqual()
        {
            Assert.AreEqual(Unit.Value, new Unit());
            Assert.AreEqual(new Unit(), new Unit());
        }

        [Test]
        public void Unit_AreNotEqual()
        {
            Assert.AreNotEqual(Unit.Value, null);
            Assert.AreNotEqual(Unit.Value, 0);
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
