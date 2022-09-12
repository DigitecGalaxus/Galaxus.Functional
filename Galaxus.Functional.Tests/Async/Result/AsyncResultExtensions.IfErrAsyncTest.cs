using System.Threading.Tasks;
using Galaxus.Functional.Async;
using NUnit.Framework;
using static Galaxus.Functional.Tests.Async.Result.ResultFactory;

namespace Galaxus.Functional.Tests.Async.Result;

[TestFixture]
internal sealed class IfErrAsyncTest
{
    [Test]
    public async Task ContinuationIsExecuted_WhenSelfIsErr()
    {
        string capturedValue = null;
        await CreateErr("err").IfErrAsync(async x => capturedValue = x);
        Assert.AreEqual("err", capturedValue);
    }

    [Test]
    public async Task ContinuationIsNotExecuted_WhenSelfIsOk()
    {
        string capturedValue = null;
        await CreateOk("ok").IfErrAsync(async x => capturedValue = x);
        Assert.IsNull(capturedValue);
    }
}
