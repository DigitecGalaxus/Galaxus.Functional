using System.Threading.Tasks;
using Galaxus.Functional.Async;
using NUnit.Framework;
using static Galaxus.Functional.Tests.Async.Result.ResultAssert;
using static Galaxus.Functional.Tests.Async.Result.ResultFactory;

namespace Galaxus.Functional.Tests.Async.Result;

[TestFixture]
internal class OrElseAsyncTest
{
    public sealed class ContinuationIsAsync : OrElseAsyncTest
    {
        [Test]
        public async Task ContinuationIsApplied_WhenSelfIsErr()
        {
            var continuation = await CreateErr("err").OrElseAsync(async x => CreateOk(AppendPeriod(x)));
            IsOk("err.", continuation);
        }

        [Test]
        public async Task ContinuationIsNotApplied_WhenSelfIsOkErr()
        {
            var continuation = await CreateOk("ok").OrElseAsync(async x => CreateOk(AppendPeriod(x)));
            IsOk("ok", continuation);
        }
    }

    public sealed class SelfIsInTask : OrElseAsyncTest
    {
        [Test]
        public async Task ContinuationIsApplied_WhenSelfIsErr()
        {

            var continuation = await CreateErrTask("err").OrElseAsync(async x => CreateOk(AppendPeriod(x)));
            IsOk("err.", continuation);
        }

        [Test]
        public async Task ContinuationIsNotApplied_WhenSelfIsOkErr()
        {
            var continuation = await CreateOkTask("ok").OrElseAsync(async x => CreateOk(AppendPeriod(x)));
            IsOk("ok", continuation);
        }
    }

    public sealed class SelfIsInTaskAndContinuationIsAsync : OrElseAsyncTest
    {
        [Test]
        public async Task ContinuationIsApplied_WhenSelfIsErr()
        {

            var continuation = await CreateErrTask("err").OrElseAsync(async x => CreateOk(AppendPeriod(x)));
            IsOk("err.", continuation);
        }

        [Test]
        public async Task ContinuationIsNotApplied_WhenSelfIsOkErr()
        {
            var continuation = await CreateOkTask("ok").OrElseAsync(async x => CreateOk(AppendPeriod(x)));
            IsOk("ok", continuation);
        }
    }

    private static string AppendPeriod(string value)
    {
        return value + ".";
    }
}
