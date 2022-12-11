using System.Threading.Tasks;
using NUnit.Framework;
using static Galaxus.Functional.Tests.Option.Async.OptionFactory;

namespace Galaxus.Functional.Tests.Option.Async;

[TestFixture]
internal class UnwrapAsyncTest
{
    public sealed class NoArgument : UnwrapAsyncTest
    {
        [Test]
        public async Task ReturnsValue_WhenSelfIsSome()
        {
            var value = await CreateSomeTask("value").UnwrapAsync();
            Assert.AreEqual("value", value);
        }

        [Test]
        public async Task ThrowsException_WhenSelfIsNone()
        {
            Assert.ThrowsAsync<TriedToUnwrapNoneException>(() => CreateNoneTask().UnwrapAsync());
        }
    }

    public sealed class StringArgument : UnwrapAsyncTest
    {
        [Test]
        public async Task ReturnsValue_WhenSelfIsSome()
        {
            var value = await CreateSomeTask("value").UnwrapAsync("it failed");
            Assert.AreEqual("value", value);
        }

        [Test]
        public async Task ThrowsException_WhenSelfIsNone()
        {
            var exception = Assert.ThrowsAsync<TriedToUnwrapNoneException>(() => CreateNoneTask().UnwrapAsync("it failed"));
            Assert.AreEqual("it failed", exception!.Message);
        }
    }

    public sealed class FunctionArgument : UnwrapAsyncTest
    {
        [Test]
        public async Task ReturnsValue_WhenSelfIsSome()
        {
            var value = await CreateSomeTask("value").UnwrapAsync(() => "it failed");
            Assert.AreEqual("value", value);
        }

        [Test]
        public async Task ThrowsException_WhenSelfIsNone()
        {
            var exception = Assert.ThrowsAsync<TriedToUnwrapNoneException>(() => CreateNoneTask().UnwrapAsync(() => "it failed"));
            Assert.AreEqual("it failed", exception!.Message);
        }
    }

    public sealed class AsyncFunctionArgument : UnwrapAsyncTest
    {
        [Test]
        public async Task ReturnsValue_WhenSelfIsSome()
        {
            var value = await CreateSomeTask("value").UnwrapAsync(async () => "it failed");
            Assert.AreEqual("value", value);
        }

        [Test]
        public async Task ThrowsException_WhenSelfIsNone()
        {
            var exception = Assert.ThrowsAsync<TriedToUnwrapNoneException>(() => CreateNoneTask().UnwrapAsync(async () => "it failed"));
            Assert.AreEqual("it failed", exception!.Message);
        }
    }
}
