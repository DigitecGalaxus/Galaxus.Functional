using System.Threading.Tasks;
using Galaxus.Functional.Async;
using NUnit.Framework;
using static Galaxus.Functional.Tests.Async.ResultFactory;

namespace Galaxus.Functional.Tests.Async;

[TestFixture]
internal class UnwrapOrAsyncTest
{
    public class SelfIsInTask : UnwrapOrAsyncTest
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

    public class SelfAndFallbackAreInTask : UnwrapOrAsyncTest
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
