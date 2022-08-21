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
            await CreateOk("ok").IfOkAsync(StoreValueAsync);
            AssertStoredValue("ok");
        }

        [Test]
        public async Task ContinuationIsNotExecuted_WhenSelfIsErr()
        {
            await CreateErr("err").IfOkAsync(StoreValueAsync);
            AssertNoValueStored();
        }
    }

    public sealed class IfErrAsyncTest : AsyncResultExtensionsTest
    {
        [Test]
        public async Task ContinuationIsExecuted_WhenSelfIsErr()
        {
            await CreateErr("err").IfErrAsync(StoreValueAsync);
            AssertStoredValue("err");
        }

        [Test]
        public async Task ContinuationIsNotExecuted_WhenSelfIsOk()
        {
            await CreateOk("ok").IfErrAsync(StoreValueAsync);
            AssertNoValueStored();
        }
    }

    public class AndThenAsyncTest : AsyncResultExtensionsTest
    {
        public sealed class ContinuationIsAsync : AndThenAsyncTest
        {
            [Test]
            public async Task ContinuationIsApplied_WhenSelfIsOk()
            {
                var continuation = await CreateOk("ok").AndThenAsync(StoreValueAndReturnWrappedAsync);
                AssertStoredValue("ok");
                AssertOk(continuation, "ok");
            }

            [Test]
            public async Task ContinuationIsNotApplied_WhenSelfIsErr()
            {
                var continuation = await CreateErr("err").AndThenAsync(StoreValueAndReturnWrappedAsync);
                AssertErr(continuation, "err");
                AssertNoValueStored();
            }
        }

        public sealed class SelfIsInTask : AndThenAsyncTest
        {
            [Test]
            public async Task ContinuationIsApplied_WhenAwaitedSelfIsOk()
            {
                var continuation = await CreateOkTask("ok").AndThenAsync(StoreValueAndReturnWrapped);
                AssertStoredValue("ok");
                AssertOk(continuation, "ok");
            }

            [Test]
            public async Task ContinuationIsNotApplied_WhenAwaitedSelfIsErr()
            {
                var continuation = await CreateErrTask("err").AndThenAsync(StoreValueAndReturnWrapped);
                AssertErr(continuation, "err");
                AssertNoValueStored();
            }
        }

        public sealed class SelfIsInTaskAndContinuationIsAsync : AndThenAsyncTest
        {
            [Test]
            public async Task ContinuationIsApplied_WhenAwaitedSelfIsOk()
            {
                var continuation = await CreateOkTask("ok").AndThenAsync(StoreValueAndReturnWrappedAsync);
                AssertStoredValue("ok");
                AssertOk(continuation, "ok");
            }

            [Test]
            public async Task ContinuationIsNotApplied_WhenAwaitedSelfIsErr()
            {
                var continuation = await CreateErrTask("err").AndThenAsync(StoreValueAndReturnWrappedAsync);
                AssertErr(continuation, "err");
                AssertNoValueStored();
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
                var continuation = await CreateOk("ok").MapAsync(AppendPeriodAsync);
                AssertOk(continuation, "ok.");
            }

            [Test]
            public async Task ContinuationIsNotApplied_WhenSelfIsErr()
            {
                var continuation = await CreateErr("err").MapAsync(AppendPeriodAsync);
                AssertErr(continuation, "err");
            }
        }

        public sealed class SelfIsInTask : AndThenAsyncTest
        {
            [Test]
            public async Task ContinuationIsApplied_WhenAwaitedSelfIsOk()
            {
                var continuation = await CreateOkTask("ok").MapAsync(AppendPeriod);
                AssertOk(continuation, "ok.");
            }

            [Test]
            public async Task ContinuationIsNotApplied_WhenAwaitedSelfIsErr()
            {
                var continuation = await CreateErrTask("err").MapAsync(AppendPeriod);
                AssertErr(continuation, "err");
            }
        }

        public sealed class SelfIsInTaskAndContinuationIsAsync : AndThenAsyncTest
        {
            [Test]
            public async Task ContinuationIsApplied_WhenAwaitedSelfIsOk()
            {
                var continuation = await CreateOkTask("ok").MapAsync(AppendPeriodAsync);
                AssertOk(continuation, "ok.");
            }

            [Test]
            public async Task ContinuationIsNotApplied_WhenAwaitedSelfIsErr()
            {
                var continuation = await CreateErrTask("err").MapAsync(AppendPeriodAsync);
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
                var continuation = await CreateErr("err").MapErrAsync(AppendPeriodAsync);
                AssertErr(continuation, "err.");
            }

            [Test]
            public async Task ContinuationIsNotApplied_WhenSelfIsOk()
            {
                var continuation = await CreateOk("ok").MapErrAsync(AppendPeriodAsync);
                AssertOk(continuation, "ok");
            }
        }

        public sealed class SelfIsInTask : AndThenAsyncTest
        {
            [Test]
            public async Task ContinuationIsApplied_WhenAwaitedSelfIsErr()
            {
                var continuation = await CreateErrTask("err").MapErrAsync(AppendPeriod);
                AssertErr(continuation, "err.");
            }

            [Test]
            public async Task ContinuationIsNotApplied_WhenAwaitedSelfIsOk()
            {
                var continuation = await CreateOkTask("ok").MapErrAsync(AppendPeriod);
                AssertOk(continuation, "ok");
            }
        }

        public sealed class SelfIsInTaskAndContinuationIsAsync : AndThenAsyncTest
        {
            [Test]
            public async Task ContinuationIsApplied_WhenAwaitedSelfIsErr()
            {
                var continuation = await CreateErrTask("err").MapErrAsync(AppendPeriodAsync);
                AssertErr(continuation, "err.");
            }

            [Test]
            public async Task ContinuationIsNotApplied_WhenAwaitedSelfIsOk()
            {
                var continuation = await CreateOkTask("ok").MapErrAsync(AppendPeriodAsync);
                AssertOk(continuation, "ok");
            }
        }
    }
}
