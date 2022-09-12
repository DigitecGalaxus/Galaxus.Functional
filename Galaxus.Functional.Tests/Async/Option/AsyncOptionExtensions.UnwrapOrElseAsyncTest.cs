using System.Threading.Tasks;
using Galaxus.Functional.Async;
using NUnit.Framework;
using static Galaxus.Functional.Tests.Async.Option.OptionFactory;

namespace Galaxus.Functional.Tests.Async.Option;

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
