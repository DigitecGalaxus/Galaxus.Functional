using System.Threading.Tasks;
using Galaxus.Functional.Async;
using NUnit.Framework;
using static Galaxus.Functional.Tests.Async.ResultFactory;

namespace Galaxus.Functional.Tests.Async;

[TestFixture]
internal class UnwrapOrElseAsyncTest
{
    public class SelfIsInTask : UnwrapOrElseAsyncTest
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

    public class SelfAndFallbackAreInTask : UnwrapOrElseAsyncTest
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
