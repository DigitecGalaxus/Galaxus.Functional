using System.Threading.Tasks;
using NUnit.Framework;
using Galaxus.Functional.Async;
using static Galaxus.Functional.Tests.Async.ResultFactory;

namespace Galaxus.Functional.Tests.Async;

[TestFixture]
internal class UnwrapAsyncTest
{
    public class NoArgument : UnwrapAsyncTest
    {
        [Test]
        public async Task ReturnsOkValue_WhenSelfIsOk()
        {
            var value = await CreateOkTask("ok").UnwrapAsync();
            Assert.AreEqual("ok", value);
        }
        [Test]
        public async Task ThrowsException_WhenSelfIsErr()
        {
            Assert.ThrowsAsync<TriedToUnwrapErrException>(() => CreateErrTask("err").UnwrapAsync());
        }
    }

    public class StringArgument : UnwrapAsyncTest
    {
        [Test]
        public async Task ReturnsOkValue_WhenSelfIsOk()
        {
            var value = await CreateOkTask("ok").UnwrapAsync("it failed");
            Assert.AreEqual("ok", value);
        }
        [Test]
        public async Task ThrowsException_WhenSelfIsErr()
        {
            var exception = Assert.ThrowsAsync<TriedToUnwrapErrException>(() => CreateErrTask("err").UnwrapAsync("it failed"));
            Assert.AreEqual("it failed", exception!.Message);
        }
    }

    public class FunctionArgument : UnwrapAsyncTest
    {
        [Test]
        public async Task ReturnsOkValue_WhenSelfIsOk()
        {
            var value = await CreateOkTask("ok").UnwrapAsync(x => x + " failed");
            Assert.AreEqual("ok", value);
        }
        [Test]
        public async Task ThrowsException_WhenSelfIsErr()
        {
            var exception = Assert.ThrowsAsync<TriedToUnwrapErrException>(() => CreateErrTask("err").UnwrapAsync(x => x + " failed"));
            Assert.AreEqual("err failed", exception!.Message);
        }
    }

    public class AsyncFunctionArgument : UnwrapAsyncTest
    {
        [Test]
        public async Task ReturnsOkValue_WhenSelfIsOk()
        {
            var value = await CreateOkTask("ok").UnwrapAsync(async x => x + " failed");
            Assert.AreEqual("ok", value);
        }
        [Test]
        public async Task ThrowsException_WhenSelfIsErr()
        {
            var exception = Assert.ThrowsAsync<TriedToUnwrapErrException>(() => CreateErrTask("err").UnwrapAsync(async x => x + " failed"));
            Assert.AreEqual("err failed", exception!.Message);
        }
    }
}
