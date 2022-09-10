using System.Threading.Tasks;
using Galaxus.Functional.Async;
using NUnit.Framework;
using static Galaxus.Functional.Tests.Async.ResultAssert;
using static Galaxus.Functional.Tests.Async.ResultFactory;

namespace Galaxus.Functional.Tests.Async;

[TestFixture]
internal class MapAsyncTest
{
    public sealed class ContinuationIsAsync : MapAsyncTest
    {
        [Test]
        public async Task ContinuationIsApplied_WhenSelfIsOk()
        {
            var continuation = await CreateOk("ok").MapAsync(async x => AppendPeriod(x));
            IsOk(continuation, "ok.");
        }

        [Test]
        public async Task ContinuationIsNotApplied_WhenSelfIsErr()
        {
            var continuation = await CreateErr("err").MapAsync(async x => AppendPeriod(x));
            IsErr(continuation, "err");
        }
    }

    public sealed class SelfIsInTask : MapAsyncTest
    {
        [Test]
        public async Task ContinuationIsApplied_WhenAwaitedSelfIsOk()
        {
            var continuation = await CreateOkTask("ok").MapAsync(AppendPeriod);
            IsOk(continuation, "ok.");
        }

        [Test]
        public async Task ContinuationIsNotApplied_WhenAwaitedSelfIsErr()
        {
            var continuation = await CreateErrTask("err").MapAsync(AppendPeriod);
            IsErr(continuation, "err");
        }
    }

    public sealed class SelfIsInTaskAndContinuationIsAsync : MapAsyncTest
    {
        [Test]
        public async Task ContinuationIsApplied_WhenAwaitedSelfIsOk()
        {
            var continuation = await CreateOkTask("ok").MapAsync(async x => AppendPeriod(x));
            IsOk(continuation, "ok.");
        }

        [Test]
        public async Task ContinuationIsNotApplied_WhenAwaitedSelfIsErr()
        {
            var continuation = await CreateErrTask("err").MapAsync(async x => AppendPeriod(x));
            IsErr(continuation, "err");
        }
    }

    private static string AppendPeriod(string value)
    {
        return value + ".";
    }
}
