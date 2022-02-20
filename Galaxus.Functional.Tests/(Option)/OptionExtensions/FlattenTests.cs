using NUnit.Framework;

namespace Galaxus.Functional.Tests.OptionExtensions
{
    [TestFixture]
    public class FlattenTests
    {
        [Test]
        public void Flatten_StringWithDepth2()
        {
            var option2 = "test".ToOption().ToOption();
            var singleOption = option2.Flatten();

            Assert.IsTrue(condition: option2.IsSome);
            Assert.AreEqual(typeof(Option<Option<string>>), option2.GetType());
            Assert.AreEqual(typeof(Option<string>), singleOption.GetType());
            Assert.IsTrue(condition: singleOption.IsSome);
            Assert.AreEqual("test", singleOption.Unwrap());
        }

        [Test]
        public void Flatten_IntWithDepth2()
        {
            var option2 = 1.ToOption().ToOption();
            var singleOption = option2.Flatten();

            Assert.IsTrue(condition: option2.IsSome);
            Assert.AreEqual(typeof(Option<Option<int>>), option2.GetType());
            Assert.AreEqual(typeof(Option<int>), singleOption.GetType());
            Assert.IsTrue(condition: singleOption.IsSome);
            Assert.AreEqual(1, singleOption.Unwrap());
        }

        [Test]
        public void Flatten_NoneWithDepth2()
        {
            var option2 = Option<int>.None.ToOption();
            var singleOption = option2.Flatten();

            Assert.IsTrue(condition: option2.IsSome);
            Assert.AreEqual(typeof(Option<Option<int>>), option2.GetType());
            Assert.AreEqual(typeof(Option<int>), singleOption.GetType());
            Assert.IsTrue(condition: singleOption.IsNone);
        }

        [Test]
        public void Flatten_StringWithDepth3()
        {
            var option3 = "test".ToOption().ToOption().ToOption();
            var singleOption = option3.Flatten();

            Assert.IsTrue(condition: option3.IsSome);
            Assert.AreEqual(typeof(Option<Option<Option<string>>>), option3.GetType());
            Assert.AreEqual(typeof(Option<string>), singleOption.GetType());
            Assert.IsTrue(condition: singleOption.IsSome);
            Assert.AreEqual("test", singleOption.Unwrap());
        }

        [Test]
        public void Flatten_IntWithDepth3()
        {
            var option3 = 1.ToOption().ToOption().ToOption();
            var singleOption = option3.Flatten();

            Assert.IsTrue(condition: option3.IsSome);
            Assert.AreEqual(typeof(Option<Option<Option<int>>>), option3.GetType());
            Assert.AreEqual(typeof(Option<int>), singleOption.GetType());
            Assert.IsTrue(condition: singleOption.IsSome);
            Assert.AreEqual(1, singleOption.Unwrap());
        }

        [Test]
        public void Flatten_NoneWithDepth3()
        {
            var option3 = Option<int>.None.ToOption().ToOption();
            var singleOption = option3.Flatten();

            Assert.IsTrue(condition: option3.IsSome);
            Assert.AreEqual(typeof(Option<Option<Option<int>>>), option3.GetType());
            Assert.AreEqual(typeof(Option<int>), singleOption.GetType());
            Assert.IsTrue(condition: singleOption.IsNone);
        }
    }
}
