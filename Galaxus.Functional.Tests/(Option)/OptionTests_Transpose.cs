using NUnit.Framework;

namespace Galaxus.Functional.Tests
{
    public class OptionTests_Transpose
    {
        [Test]
        public void Option_TransposeNone_ReturnsOkNone()
        {
            var option = Option<Result<int, string>>.None;
            var result = option.Transpose();

            Assert.IsTrue(result.IsOk);
            Assert.IsTrue(result.Unwrap().IsNone);
        }

        [Test]
        public void Option_TransposeSomeOk_ReturnsOkSome()
        {
            var option = 3.ToOk<int, string>().ToOption();
            var result = option.Transpose();

            Assert.IsTrue(result.IsOk);
            Assert.AreEqual(result.Unwrap().Unwrap(), 3);
        }

        [Test]
        public void Option_TransposeSomeErr_ReturnsErr()
        {
            var option = "error".ToErr<int, string>().ToOption();
            var result = option.Transpose();

            Assert.IsTrue(result.IsErr);
            Assert.AreEqual("error", result.Err.Unwrap());
        }
    }
}
