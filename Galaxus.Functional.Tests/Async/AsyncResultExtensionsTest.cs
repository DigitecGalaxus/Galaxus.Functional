using System.Threading.Tasks;
using Galaxus.Functional.Async;
using NUnit.Framework;

namespace Galaxus.Functional.Tests.Async;

[TestFixture]
internal partial class AsyncResultExtensionsTest
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

    public class MapAsyncTest : AsyncResultExtensionsTest
    {
        public sealed class ContinuationIsAsync : AndThenAsyncTest
        {
            [Test]
            public async Task ContinuationIsApplied_WhenSelfIsOk()
            {
                var continuation = await CreateOk("ok").MapAsync(_ => Task.FromResult("ok2"));
                AssertOk(continuation, "ok2");
            }

            [Test]
            public async Task ContinuationIsNotApplied_WhenSelfIsErr()
            {
                var continuation = await CreateErr("err").MapAsync(_ => Task.FromResult("ok2"));
                AssertErr(continuation, "err");
            }
        }

        public sealed class SelfIsInTask : AndThenAsyncTest
        {
            [Test]
            public async Task ContinuationIsApplied_WhenAwaitedSelfIsOk()
            {
                var continuation = await CreateOkTask("ok").MapAsync(_ => "ok2");
                AssertOk(continuation, "ok2");
            }

            [Test]
            public async Task ContinuationIsNotApplied_WhenAwaitedSelfIsErr()
            {
                var continuation = await CreateErrTask("err").MapAsync(_ => "ok2");
                AssertErr(continuation, "err");
            }
        }

        public sealed class SelfIsInTaskAndContinuationIsAsync : AndThenAsyncTest
        {
            [Test]
            public async Task ContinuationIsApplied_WhenAwaitedSelfIsOk()
            {
                var continuation = await CreateOkTask("ok").MapAsync(_ => Task.FromResult("ok2"));
                AssertOk(continuation, "ok2");
            }

            [Test]
            public async Task ContinuationIsNotApplied_WhenAwaitedSelfIsErr()
            {
                var continuation = await CreateErrTask("err").MapAsync(_ => Task.FromResult("ok2"));
                AssertErr(continuation, "err");
            }
        }
    }

    public class MapErrAsyncTest : AsyncResultExtensionsTest
    {
        public sealed class ContinuationIsAsync : AndThenAsyncTest
        {
            [Test]
            public async Task ContinuationIsApplied_WhenSelfIsErr()
            {
                var continuation = await CreateErr("err").MapErrAsync(_ => Task.FromResult("err2"));
                AssertErr(continuation, "err2");
            }

            [Test]
            public async Task ContinuationIsNotApplied_WhenSelfIsOk()
            {
                var continuation = await CreateOk("ok").MapErrAsync(_ => Task.FromResult("err2"));
                AssertOk(continuation, "ok");
            }
        }

        public sealed class SelfIsInTask : AndThenAsyncTest
        {
            [Test]
            public async Task ContinuationIsApplied_WhenAwaitedSelfIsErr()
            {
                var continuation = await CreateErrTask("err").MapErrAsync(_ => "err2");
                AssertErr(continuation, "err2");
            }

            [Test]
            public async Task ContinuationIsNotApplied_WhenAwaitedSelfIsOk()
            {
                var continuation = await CreateOkTask("ok").MapErrAsync(_ => "err2");
                AssertOk(continuation, "ok");
            }
        }

        public sealed class SelfIsInTaskAndContinuationIsAsync : AndThenAsyncTest
        {
            [Test]
            public async Task ContinuationIsApplied_WhenAwaitedSelfIsErr()
            {
                var continuation = await CreateErrTask("err").MapErrAsync(_ => Task.FromResult("err2"));
                AssertErr(continuation, "err2");
            }

            [Test]
            public async Task ContinuationIsNotApplied_WhenAwaitedSelfIsOk()
            {
                var continuation = await CreateOkTask("ok").MapErrAsync(_ => Task.FromResult("err2"));
                AssertOk(continuation, "ok");
            }
        }
    }
}
