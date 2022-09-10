using System.Threading.Tasks;
using Galaxus.Functional.Async;
using NUnit.Framework;
using static Galaxus.Functional.Tests.Async.ResultAssert;
using static Galaxus.Functional.Tests.Async.ResultFactory;

namespace Galaxus.Functional.Tests.Async;

[TestFixture]
internal class OrElseAsyncTest
{
    public class ContinuationIsAsync : OrElseAsyncTest
    {
        [Test]
        public async Task ContinuationIsApplied_WhenSelfIsErr()
        {
            var continuation = await CreateErr("err").OrElseAsync(async x => CreateOk(AppendPeriod(x)));
            IsOk(continuation, "err.");
        }

        [Test]
        public async Task ContinuationIsNotApplied_WhenSelfIsOkErr()
        {
            var continuation = await CreateOk("ok").OrElseAsync(async x => CreateOk(AppendPeriod(x)));
            IsOk(continuation, "ok");
        }
    }

    public class SelfIsInTask : OrElseAsyncTest
    {
        [Test]
        public async Task ContinuationIsApplied_WhenSelfIsErr()
        {

            var continuation = await CreateErrTask("err").OrElseAsync(async x => CreateOk(AppendPeriod(x)));
            IsOk(continuation, "err.");
        }

        [Test]
        public async Task ContinuationIsNotApplied_WhenSelfIsOkErr()
        {
            var continuation = await CreateOkTask("ok").OrElseAsync(async x => CreateOk(AppendPeriod(x)));
            IsOk(continuation, "ok");
        }
    }

    public class SelfIsInTaskAndContinuationIsAsync : OrElseAsyncTest
    {
        [Test]
        public async Task ContinuationIsApplied_WhenSelfIsErr()
        {

            var continuation = await CreateErrTask("err").OrElseAsync(async x => CreateOk(AppendPeriod(x)));
            IsOk(continuation, "err.");
        }

        [Test]
        public async Task ContinuationIsNotApplied_WhenSelfIsOkErr()
        {
            var continuation = await CreateOkTask("ok").OrElseAsync(async x => CreateOk(AppendPeriod(x)));
            IsOk(continuation, "ok");
        }
    }

    private static string AppendPeriod(string value)
    {
        return value + ".";
    }
}
