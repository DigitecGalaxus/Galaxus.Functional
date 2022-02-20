using NUnit.Framework;

namespace Galaxus.Functional.Tests
{
    public class ResultTests_Transpose
    {
        [Test]
        public void Result_TransposeOkSome_ReturnsSomeOk()
        {
            var result = 3.ToOption().ToOk<Option<int>, string>();
            var option = result.Transpose();

            Assert.IsTrue(condition: option.IsSome);
            Assert.IsTrue(condition: option.Unwrap().IsOk);
        }

        [Test]
        public void Result_TransposeOkNone_ReturnsNone()
        {
            var result = Option<int>.None.ToOk<Option<int>, string>();
            var option = result.Transpose();

            Assert.IsTrue(condition: option.IsNone);
        }

        [Test]
        public void Result_TransposeErr_ReturnsSomeErr()
        {
            var result = "error".ToErr<Option<int>, string>();
            var option = result.Transpose();

            Assert.IsTrue(condition: option.IsSome);
            Assert.IsTrue(condition: option.Unwrap().IsErr);
        }
    }
}
