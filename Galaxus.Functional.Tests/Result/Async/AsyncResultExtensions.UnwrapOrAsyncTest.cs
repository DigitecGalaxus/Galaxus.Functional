using System.Threading.Tasks;
using NUnit.Framework;
using static Galaxus.Functional.Tests.Result.Async.ResultFactory;

namespace Galaxus.Functional.Tests.Result.Async;

[TestFixture]
internal class UnwrapOrAsyncTest
{
    public sealed class SelfIsInTask : UnwrapOrAsyncTest
    {
        [Test]
        public async Task ReturnsOkValue_WhenSelfIsOk()
        {
            var value = await CreateOkTask("ok").UnwrapOrAsync("fallback");
            Assert.AreEqual("ok", value);
        }

        [Test]
        public async Task ReturnsFallback_WhenSelfIsErr()
        {
            var value = await CreateErrTask("err").UnwrapOrAsync("fallback");
            Assert.AreEqual("fallback", value);
        }
    }

    public sealed class SelfAndFallbackAreInTask : UnwrapOrAsyncTest
    {
        [Test]
        public async Task ReturnsOkValue_WhenSelfIsOk()
        {
            var value = await CreateOkTask("ok").UnwrapOrAsync(Task.FromResult("fallback"));
            Assert.AreEqual("ok", value);
        }

        [Test]
        public async Task ReturnsAwaitedFallback_WhenSelfIsErr()
        {
            var value = await CreateErrTask("err").UnwrapOrAsync(Task.FromResult("fallback"));
            Assert.AreEqual("fallback", value);
        }
    }
}
