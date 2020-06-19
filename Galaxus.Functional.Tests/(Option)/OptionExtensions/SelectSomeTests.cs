using System.Linq;
using NUnit.Framework;

namespace Galaxus.Functional.Tests.OptionExtensions
{
    [TestFixture]
    public class SelectSomeTests
    {
        [Test]
        public void SelectSome_AllSomeValuesRemainInOriginalOrder()
        {
            var some = new[]
            {
                0.ToOption(),
                99.ToOption(),
                new Option<int>(),
                new Option<int>(),
                666.ToOption(),
                new Option<int>(),
                999.ToOption()
            }.SelectSome().ToList();

            Assert.AreEqual(4, some.Count);
            Assert.AreEqual(0, some[0]);
            Assert.AreEqual(99, some[1]);
            Assert.AreEqual(666, some[2]);
            Assert.AreEqual(999, some[3]);
        }

        [Test]
        public void SelectSome_WithSelector_CorrectValueMapping()
        {
            var some = new[]
            {
                "hello".ToOption(),
                "world".ToOption(),
                new Option<string>(),
                new Option<string>(),
                "!".ToOption(),
                new Option<string>()
            }.SelectSome(str => str[0]).ToList();

            Assert.AreEqual("hw!", string.Join("", some));
        }
    }
}
