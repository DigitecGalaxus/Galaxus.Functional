using System.Threading.Tasks;
using Galaxus.Functional.Async;
using NUnit.Framework;
using static Galaxus.Functional.Tests.Async.ResultFactory;

namespace Galaxus.Functional.Tests.Async;

[TestFixture]
internal sealed class IfErrAsyncTest
{
    [Test]
    public async Task ContinuationIsExecuted_WhenSelfIsErr()
    {
        string capturedValue = null;
        await CreateErr("err").IfErrAsync(async x => capturedValue = x);
        Assert.That(capturedValue, Is.EqualTo("err"));
    }

    [Test]
    public async Task ContinuationIsNotExecuted_WhenSelfIsOk()
    {
        string capturedValue = null;
        await CreateOk("ok").IfErrAsync(async x => capturedValue = x);
        Assert.That(capturedValue, Is.EqualTo(null));
    }
}
