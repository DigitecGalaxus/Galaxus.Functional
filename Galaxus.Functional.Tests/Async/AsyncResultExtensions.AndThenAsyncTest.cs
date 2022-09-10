using System.Threading.Tasks;
using Galaxus.Functional.Async;
using NUnit.Framework;
using static Galaxus.Functional.Tests.Async.ResultAssert;
using static Galaxus.Functional.Tests.Async.ResultFactory;

namespace Galaxus.Functional.Tests.Async;

[TestFixture]
internal class AndThenAsyncTest
{
    public sealed class ContinuationIsAsync : AndThenAsyncTest
    {
        [Test]
        public async Task ContinuationIsApplied_WhenSelfIsOk()
        {
            var continuation = await CreateOk("ok").AndThenAsync(x => CreateOkTask(AppendPeriod(x)));
            IsOk(continuation, "ok.");
        }

        [Test]
        public async Task ContinuationIsNotApplied_WhenSelfIsErr()
        {
            var continuation = await CreateErr("err").AndThenAsync(x => CreateOkTask(AppendPeriod(x)));
            IsErr(continuation, "err");
        }
    }

    public sealed class SelfIsInTask : AndThenAsyncTest
    {
        [Test]
        public async Task ContinuationIsApplied_WhenAwaitedSelfIsOk()
        {
            var continuation = await CreateOkTask("ok").AndThenAsync(x => CreateOk(AppendPeriod(x)));
            IsOk(continuation, "ok.");
        }

        [Test]
        public async Task ContinuationIsNotApplied_WhenAwaitedSelfIsErr()
        {
            var continuation = await CreateErrTask("err").AndThenAsync(x => CreateOk(AppendPeriod(x)));
            IsErr(continuation, "err");
        }
    }

    public sealed class SelfIsInTaskAndContinuationIsAsync : AndThenAsyncTest
    {
        [Test]
        public async Task ContinuationIsApplied_WhenAwaitedSelfIsOk()
        {
            var continuation = await CreateOkTask("ok").AndThenAsync(x => CreateOkTask(AppendPeriod(x)));
            IsOk(continuation, "ok.");
        }

        [Test]
        public async Task ContinuationIsNotApplied_WhenAwaitedSelfIsErr()
        {
            var continuation = await CreateErrTask("err").AndThenAsync(x => CreateOkTask(AppendPeriod(x)));
            IsErr(continuation, "err");
        }
    }

    private static string AppendPeriod(string value)
    {
        return value + ".";
    }
}
