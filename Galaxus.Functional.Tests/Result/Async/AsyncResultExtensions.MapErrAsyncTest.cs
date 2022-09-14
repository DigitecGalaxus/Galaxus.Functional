using System.Threading.Tasks;
using NUnit.Framework;
using static Galaxus.Functional.Tests.Result.Async.ResultAssert;
using static Galaxus.Functional.Tests.Result.Async.ResultFactory;

namespace Galaxus.Functional.Tests.Result.Async;

[TestFixture]
internal class MapErrAsyncTest
{
    public sealed class ContinuationIsAsync : MapErrAsyncTest
    {
        [Test]
        public async Task ContinuationIsApplied_WhenSelfIsErr()
        {
            var continuation = await CreateErr("err").MapErrAsync(async x => AppendPeriod(x));
            IsErr("err.", continuation);
        }

        [Test]
        public async Task ContinuationIsNotApplied_WhenSelfIsOk()
        {
            var continuation = await CreateOk("ok").MapErrAsync(async x => AppendPeriod(x));
            IsOk("ok", continuation);
        }
    }

    public sealed class SelfIsInTask : MapErrAsyncTest
    {
        [Test]
        public async Task ContinuationIsApplied_WhenAwaitedSelfIsErr()
        {
            var continuation = await CreateErrTask("err").MapErrAsync(AppendPeriod);
            IsErr("err.", continuation);
        }

        [Test]
        public async Task ContinuationIsNotApplied_WhenAwaitedSelfIsOk()
        {
            var continuation = await CreateOkTask("ok").MapErrAsync(AppendPeriod);
            IsOk("ok", continuation);
        }
    }

    public sealed class SelfIsInTaskAndContinuationIsAsync : MapErrAsyncTest
    {
        [Test]
        public async Task ContinuationIsApplied_WhenAwaitedSelfIsErr()
        {
            var continuation = await CreateErrTask("err").MapErrAsync(async x => AppendPeriod(x));
            IsErr("err.", continuation);
        }

        [Test]
        public async Task ContinuationIsNotApplied_WhenAwaitedSelfIsOk()
        {
            var continuation = await CreateOkTask("ok").MapErrAsync(async x => AppendPeriod(x));
            IsOk("ok", continuation);
        }
    }

    private static string AppendPeriod(string x)
    {
        return x + ".";
    }
}
