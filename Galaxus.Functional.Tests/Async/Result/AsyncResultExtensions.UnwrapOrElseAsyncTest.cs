using System.Threading.Tasks;
using Galaxus.Functional.Async;
using NUnit.Framework;
using static Galaxus.Functional.Tests.Async.Result.ResultFactory;

namespace Galaxus.Functional.Tests.Async.Result;

[TestFixture]
internal class UnwrapOrElseAsyncTest
{
    public sealed class SelfIsInTask : UnwrapOrElseAsyncTest
    {
        [Test]
        public async Task ReturnsOkValue_WhenSelfIsOk()
        {
            var value = await CreateOkTask("ok").UnwrapOrElseAsync(() => "fallback");
            Assert.AreEqual("ok", value);
        }

        [Test]
        public async Task ReturnsFallback_WhenSelfIsErr()
        {
            var value = await CreateErrTask("err").UnwrapOrElseAsync(() => "fallback");
            Assert.AreEqual("fallback", value);
        }
    }

    public sealed class SelfAndFallbackAreInTask : UnwrapOrElseAsyncTest
    {
        [Test]
        public async Task ReturnsOkValue_WhenSelfIsOk()
        {
            var value = await CreateOkTask("ok").UnwrapOrElseAsync(async () => "fallback");
            Assert.AreEqual("ok", value);
        }

        [Test]
        public async Task ReturnsAwaitedFallback_WhenSelfIsErr()
        {
            var value = await CreateErrTask("err").UnwrapOrElseAsync(async () => "fallback");
            Assert.AreEqual("fallback", value);
        }
    }
}
