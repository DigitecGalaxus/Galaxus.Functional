using System.Threading.Tasks;
using Galaxus.Functional.Async;
using NUnit.Framework;
using static Galaxus.Functional.Tests.Async.Result.ResultFactory;

namespace Galaxus.Functional.Tests.Async.Result;

[TestFixture]
internal sealed class IfOkAsyncTest
{
    [Test]
    public async Task ContinuationIsExecuted_WhenSelfIsOk()
    {
        string capturedValue = null;
        await CreateOk("ok").IfOkAsync(async x => capturedValue = x);
        Assert.AreEqual("ok", capturedValue);
    }

    [Test]
    public async Task ContinuationIsNotExecuted_WhenSelfIsErr()
    {
        string capturedValue = null;
        await CreateErr("err").IfOkAsync(async x => capturedValue = x);
        Assert.IsNull(capturedValue);
    }
}
