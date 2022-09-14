using System.Threading.Tasks;
using NUnit.Framework;
using static Galaxus.Functional.Tests.Option.Async.OptionFactory;

namespace Galaxus.Functional.Tests.Option.Async;

[TestFixture]
internal class UnwrapOrAsyncTest
{
    public sealed class SelfIsInTask : UnwrapOrAsyncTest
    {
        [Test]
        public async Task ReturnsValue_WhenSelfIsSome()
        {
            var value = await CreateSomeTask("value").UnwrapOrAsync("failed");
            Assert.AreEqual("value", value);
        }

        [Test]
        public async Task ReturnsFallback_WhenSelfIsNone()
        {
            var value = await CreateNoneTask().UnwrapOrAsync("failed");
            Assert.AreEqual("failed", value);
        }
    }

    public sealed class SelfAndFallbackAreInTask : UnwrapOrAsyncTest
    {
        [Test]
        public async Task ReturnsValue_WhenSelfIsSome()
        {
            var value = await CreateSomeTask("value").UnwrapOrAsync(Task.FromResult("failed"));
            Assert.AreEqual("value", value);
        }

        [Test]
        public async Task ReturnsFallback_WhenSelfIsNone()
        {
            var value = await CreateNoneTask().UnwrapOrAsync(Task.FromResult("failed"));
            Assert.AreEqual("failed", value);
        }
    }
}
