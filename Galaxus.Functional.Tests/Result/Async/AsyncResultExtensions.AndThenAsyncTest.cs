using System.Threading.Tasks;
using NUnit.Framework;
using static Galaxus.Functional.Tests.Result.Async.ResultAssert;
using static Galaxus.Functional.Tests.Result.Async.ResultFactory;

namespace Galaxus.Functional.Tests.Result.Async;

[TestFixture]
internal class AndThenAsyncTest
{
    public sealed class ContinuationIsAsync : AndThenAsyncTest
    {
        [Test]
        public async Task ContinuationIsApplied_WhenSelfIsOk()
        {
            var continuation = await CreateOk("ok").AndThenAsync(x => CreateOkTask(AppendPeriod(x)));
            IsOk("ok.", continuation);
        }

        [Test]
        public async Task ContinuationIsNotApplied_WhenSelfIsErr()
        {
            var continuation = await CreateErr("err").AndThenAsync(x => CreateOkTask(AppendPeriod(x)));
            IsErr("err", continuation);
        }
    }

    public sealed class SelfIsInTask : AndThenAsyncTest
    {
        [Test]
        public async Task ContinuationIsApplied_WhenAwaitedSelfIsOk()
        {
            var continuation = await CreateOkTask("ok").AndThenAsync(x => CreateOk(AppendPeriod(x)));
            IsOk("ok.", continuation);
        }

        [Test]
        public async Task ContinuationIsNotApplied_WhenAwaitedSelfIsErr()
        {
            var continuation = await CreateErrTask("err").AndThenAsync(x => CreateOk(AppendPeriod(x)));
            IsErr("err", continuation);
        }
    }

    public sealed class SelfIsInTaskAndContinuationIsAsync : AndThenAsyncTest
    {
        [Test]
        public async Task ContinuationIsApplied_WhenAwaitedSelfIsOk()
        {
            var continuation = await CreateOkTask("ok").AndThenAsync(x => CreateOkTask(AppendPeriod(x)));
            IsOk("ok.", continuation);
        }

        [Test]
        public async Task ContinuationIsNotApplied_WhenAwaitedSelfIsErr()
        {
            var continuation = await CreateErrTask("err").AndThenAsync(x => CreateOkTask(AppendPeriod(x)));
            IsErr("err", continuation);
        }
    }

    private static string AppendPeriod(string value)
    {
        return value + ".";
    }
}
