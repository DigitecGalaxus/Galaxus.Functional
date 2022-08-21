using System.Threading.Tasks;
using Galaxus.Functional.Async;
using NUnit.Framework;

namespace Galaxus.Functional.Tests.Async;

[TestFixture]
internal class AsyncResultExtensionsTest
{
    public sealed class IfOkAsyncTest : AsyncResultExtensionsTest
    {
        [Test]
        public async Task ContinuationIsExecuted_WhenSelfIsOk()
        {
            var result = string.Empty;
            await CreateOk("ok").IfOkAsync(x => StoreValueAsync(x, out result));
            Assert.That(result, Is.EqualTo("ok"));
        }

        [Test]
        public async Task ContinuationIsNotExecuted_WhenSelfIsErr()
        {
            var result = string.Empty;
            await CreateErr("err").IfOkAsync(x => StoreValueAsync(x, out result));
            Assert.That(result, Is.EqualTo(string.Empty));
        }
    }

    public sealed class IfErrAsyncTest : AsyncResultExtensionsTest
    {
        [Test]
        public async Task ContinuationIsExecuted_WhenSelfIsErr()
        {
            var result = string.Empty;
            await CreateErr("err").IfErrAsync(x => StoreValueAsync(x, out result));
            Assert.That(result, Is.EqualTo("err"));
        }

        [Test]
        public async Task ContinuationIsNotExecuted_WhenSelfIsOk()
        {
            var result = string.Empty;
            await CreateOk("ok").IfErrAsync(x => StoreValueAsync(x, out result));
            Assert.That(result, Is.EqualTo(string.Empty));
        }
    }

    public class AndThenAsyncTest : AsyncResultExtensionsTest
    {
        public sealed class ContinuationIsAsync : AndThenAsyncTest
        {
            [Test]
            public async Task ContinuationIsApplied_WhenSelfIsOk()
            {
                var result = string.Empty;
                var continuation = await CreateOk("ok").AndThenAsync(x => StoreValueAndReturnWrappedAsync(x, out result));
                Assert.That(result, Is.EqualTo("ok"));
                AssertOk(continuation, "ok");
            }

            [Test]
            public async Task ContinuationIsNotApplied_WhenSelfIsErr()
            {
                var result = string.Empty;
                var continuation = await CreateErr("err").AndThenAsync(x => StoreValueAndReturnWrappedAsync(x, out result));
                Assert.That(result, Is.EqualTo(string.Empty));
                AssertErr(continuation, "err");
            }
        }

        public sealed class SelfIsInTask : AndThenAsyncTest
        {
            [Test]
            public async Task ContinuationIsApplied_WhenAwaitedSelfIsOk()
            {
                var result = string.Empty;
                var continuation = await CreateOkTask("ok").AndThenAsync(x => StoreValueAndReturnWrapped(x, out result));
                Assert.That(result, Is.EqualTo("ok"));
                AssertOk(continuation, "ok");
            }

            [Test]
            public async Task ContinuationIsNotApplied_WhenAwaitedSelfIsErr()
            {
                var result = string.Empty;
                var continuation = await CreateErrTask("err").AndThenAsync(x => StoreValueAndReturnWrapped(x, out result));
                Assert.That(result, Is.EqualTo(string.Empty));
                AssertErr(continuation, "err");
            }
        }

        public sealed class SelfIsInTaskAndContinuationIsAsync : AndThenAsyncTest
        {
            [Test]
            public async Task ContinuationIsApplied_WhenAwaitedSelfIsOk()
            {
                var result = string.Empty;
                var continuation = await CreateOkTask("ok").AndThenAsync(x => StoreValueAndReturnWrappedAsync(x, out result));
                Assert.That(result, Is.EqualTo("ok"));
                AssertOk(continuation, "ok");
            }

            [Test]
            public async Task ContinuationIsNotApplied_WhenAwaitedSelfIsErr()
            {
                var result = string.Empty;
                var continuation = await CreateErrTask("err").AndThenAsync(x => StoreValueAndReturnWrappedAsync(x, out result));
                Assert.That(result, Is.EqualTo(string.Empty));
                AssertErr(continuation, "err");
            }
        }
    }

    private static Result<string, string> CreateOk(string value)
    {
        return Result<string, string>.FromOk(value);
    }

    private static Task<Result<string, string>> CreateOkTask(string value)
    {
        return Task.FromResult(CreateOk(value));
    }

    private static Result<string, string> CreateErr(string value)
    {
        return Result<string, string>.FromErr(value);
    }

    private static Task<Result<string, string>> CreateErrTask(string value)
    {
        return Task.FromResult(CreateErr(value));
    }

    private static Task StoreValueAsync(string value, out string result)
    {
        result = value;

        return Task.CompletedTask;
    }

    private static Result<string, string> StoreValueAndReturnWrapped(string value, out string result)
    {
        result = value;

        return Result<string, string>.FromOk(value);
    }

    private static Task<Result<string, string>> StoreValueAndReturnWrappedAsync(string value, out string result)
    {
        var returnValue = StoreValueAndReturnWrapped(value, out result);

        return Task.FromResult(returnValue);
    }

    private static void AssertOk(Result<string, string> result, string value)
    {
        Assert.That(result.Ok, Is.EqualTo(Option<string>.Some(value)));
    }
    private static void AssertErr(Result<string, string> result, string value)
    {
        Assert.That(result.Err, Is.EqualTo(Option<string>.Some(value)));
    }
}
