using System.Threading.Tasks;
using NUnit.Framework;
using static Galaxus.Functional.Tests.Option.Async.OptionFactory;

namespace Galaxus.Functional.Tests.Option.Async;

[TestFixture]
internal class UnwrapOrElseAsyncTest
{
    public sealed class FunctionArgument : UnwrapOrElseAsyncTest
    {
        [Test]
        public async Task ReturnsValue_WhenSelfIsSome()
        {
            var value = await CreateSomeTask("value").UnwrapOrElseAsync(() => "failed");
            Assert.AreEqual("value", value);
        }

        [Test]
        public async Task ReturnsFallback_WhenSelfIsNone()
        {
            var value = await CreateNoneTask().UnwrapOrElseAsync(() => "failed");
            Assert.AreEqual("failed", value);
        }
    }

    public sealed class AsyncFunctionArgument : UnwrapOrElseAsyncTest
    {
        [Test]
        public async Task ReturnsValue_WhenSelfIsSome()
        {
            var value = await CreateSomeTask("value").UnwrapOrElseAsync(async () => "failed");
            Assert.AreEqual("value", value);
        }

        [Test]
        public async Task ReturnsFallback_WhenSelfIsNone()
        {
            var value = await CreateNoneTask().UnwrapOrElseAsync(async () => "failed");
            Assert.AreEqual("failed", value);
        }
    }
}
