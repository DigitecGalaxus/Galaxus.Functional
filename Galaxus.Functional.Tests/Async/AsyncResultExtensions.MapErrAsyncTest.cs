using System.Threading.Tasks;
using Galaxus.Functional.Async;
using NUnit.Framework;
using static Galaxus.Functional.Tests.Async.ResultAssert;
using static Galaxus.Functional.Tests.Async.ResultFactory;

namespace Galaxus.Functional.Tests.Async;

[TestFixture]
internal class MapErrAsyncTest
{
    public sealed class ContinuationIsAsync : MapErrAsyncTest
    {
        [Test]
        public async Task ContinuationIsApplied_WhenSelfIsErr()
        {
            var continuation = await CreateErr("err").MapErrAsync(async x => AppendPeriod(x));
            IsErr(continuation, "err.");
        }

        [Test]
        public async Task ContinuationIsNotApplied_WhenSelfIsOk()
        {
            var continuation = await CreateOk("ok").MapErrAsync(async x => AppendPeriod(x));
            IsOk(continuation, "ok");
        }
    }

    public sealed class SelfIsInTask : MapErrAsyncTest
    {
        [Test]
        public async Task ContinuationIsApplied_WhenAwaitedSelfIsErr()
        {
            var continuation = await CreateErrTask("err").MapErrAsync(AppendPeriod);
            IsErr(continuation, "err.");
        }

        [Test]
        public async Task ContinuationIsNotApplied_WhenAwaitedSelfIsOk()
        {
            var continuation = await CreateOkTask("ok").MapErrAsync(AppendPeriod);
            IsOk(continuation, "ok");
        }
    }

    public sealed class SelfIsInTaskAndContinuationIsAsync : MapErrAsyncTest
    {
        [Test]
        public async Task ContinuationIsApplied_WhenAwaitedSelfIsErr()
        {
            var continuation = await CreateErrTask("err").MapErrAsync(async x => AppendPeriod(x));
            IsErr(continuation, "err.");
        }

        [Test]
        public async Task ContinuationIsNotApplied_WhenAwaitedSelfIsOk()
        {
            var continuation = await CreateOkTask("ok").MapErrAsync(async x => AppendPeriod(x));
            IsOk(continuation, "ok");
        }
    }

    private static string AppendPeriod(string x)
    {
        return x + ".";
    }
}
