using NUnit.Framework;

namespace Galaxus.Functional.Tests
{
    public class ResultTests_Contains
    {
        [Test]
        public void Result_ErrContains_ReturnsFalse()
        {
            var result = "value".ToErr<int, string>();
            var contains = result.Contains(3);
            Assert.IsFalse(condition: contains);
        }

        [Test]
        public void Result_OkContains_ReturnsFalse()
        {
            var result = 3.ToOk<int, string>();
            var contains = result.Contains(5);
            Assert.IsFalse(condition: contains);
        }

        [Test]
        public void Result_OkContains_ReturnsTrue()
        {
            var result = 3.ToOk<int, string>();
            var contains = result.Contains(3);
            Assert.IsTrue(condition: contains);
        }

        [Test]
        public void Result_OkContainsErr_ReturnsFalse()
        {
            var result = 3.ToOk<int, string>();
            var contains = result.ContainsErr("hello");
            Assert.IsFalse(condition: contains);
        }

        [Test]
        public void Result_ErrContainsErr_ReturnsFalse()
        {
            var result = "hello".ToErr<int, string>();
            var contains = result.ContainsErr("world");
            Assert.IsFalse(condition: contains);
        }

        [Test]
        public void Result_ErrContainsErr_ReturnsTrue()
        {
            var result = "hello".ToErr<int, string>();
            var contains = result.ContainsErr("hello");
            Assert.IsTrue(condition: contains);
        }
    }
}
