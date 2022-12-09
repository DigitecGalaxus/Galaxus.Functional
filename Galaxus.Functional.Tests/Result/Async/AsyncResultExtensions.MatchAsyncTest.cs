using System.Threading.Tasks;
using NUnit.Framework;
using static Galaxus.Functional.Tests.Result.Async.ResultFactory;

namespace Galaxus.Functional.Tests.Result.Async;

[TestFixture]
internal class MatchAsyncTest
{
    public sealed class SelfIsInTask : MatchAsyncTest
    {
        [Test]
        public async Task AppliesOnOk_WhenResultIsOk()
        {
            var result = await CreateOkTask("ok").MatchAsync(AppendPeriod, PrependPeriod);
            Assert.AreEqual("ok.", result);
        }

        [Test]
        public async Task AppliesOnErr_WhenResultIsErr()
        {
            var result = await CreateErrTask("err").MatchAsync(AppendPeriod, PrependPeriod);
            Assert.AreEqual(".err", result);
        }
    }

    public sealed class SelfIsInTaskAndOnOkIsAsync : MatchAsyncTest
    {
        [Test]
        public async Task AppliesOnOk_WhenResultIsOk()
        {
            var result = await CreateOkTask("ok").MatchAsync(async x => AppendPeriod(x), PrependPeriod);
            Assert.AreEqual("ok.", result);
        }

        [Test]
        public async Task AppliesOnErr_WhenResultIsErr()
        {
            var result = await CreateErrTask("err").MatchAsync(async x => AppendPeriod(x), PrependPeriod);
            Assert.AreEqual(".err", result);
        }
    }

    public sealed class SelfIsInTaskAndOnErrIsAsync : MatchAsyncTest
    {
        [Test]
        public async Task AppliesOnOk_WhenResultIsOk()
        {
            var result = await CreateOkTask("ok").MatchAsync(AppendPeriod, async x => PrependPeriod(x));
            Assert.AreEqual("ok.", result);
        }

        [Test]
        public async Task AppliesOnErr_WhenResultIsErr()
        {
            var result = await CreateErrTask("err").MatchAsync(AppendPeriod, async x => PrependPeriod(x));
            Assert.AreEqual(".err", result);
        }
    }

    public sealed class SelfIsInTaskAndBothContinuationsAreAsync : MatchAsyncTest
    {
        [Test]
        public async Task AppliesOnOk_WhenResultIsOk()
        {
            var result = await CreateOkTask("ok").MatchAsync(async x => AppendPeriod(x), async x => PrependPeriod(x));
            Assert.AreEqual("ok.", result);
        }

        [Test]
        public async Task AppliesOnErr_WhenResultIsErr()
        {
            var result = await CreateErrTask("err").MatchAsync(async x => AppendPeriod(x), async x => PrependPeriod(x));
            Assert.AreEqual(".err", result);
        }
    }

    public sealed class OnOkIsAsync : MatchAsyncTest
    {
        [Test]
        public async Task AppliesOnOk_WhenResultIsOk()
        {
            var result = await CreateOk("ok").MatchAsync(async x => AppendPeriod(x), PrependPeriod);
            Assert.AreEqual("ok.", result);
        }

        [Test]
        public async Task AppliesOnErr_WhenResultIsErr()
        {
            var result = await CreateErr("err").MatchAsync(async x => AppendPeriod(x), PrependPeriod);
            Assert.AreEqual(".err", result);
        }
    }

    public sealed class OnErrIsAsync : MatchAsyncTest
    {
        [Test]
        public async Task AppliesOnOk_WhenResultIsOk()
        {
            var result = await CreateOk("ok").MatchAsync(AppendPeriod, async x => PrependPeriod(x));
            Assert.AreEqual("ok.", result);
        }

        [Test]
        public async Task AppliesOnErr_WhenResultIsErr()
        {
            var result = await CreateErr("err").MatchAsync(AppendPeriod, async x => PrependPeriod(x));
            Assert.AreEqual(".err", result);
        }
    }

    public sealed class BothContinuationsAreAsync : MatchAsyncTest
    {
        [Test]
        public async Task AppliesOnOk_WhenResultIsOk()
        {
            var result = await CreateOk("ok").MatchAsync(async x => AppendPeriod(x), async x => PrependPeriod(x));
            Assert.AreEqual("ok.", result);
        }

        [Test]
        public async Task AppliesOnErr_WhenResultIsErr()
        {
            var result = await CreateErr("err").MatchAsync(async x => AppendPeriod(x), async x => PrependPeriod(x));
            Assert.AreEqual(".err", result);
        }
    }

    private static string AppendPeriod(string value)
    {
        return value + ".";
    }

    private static string PrependPeriod(string value)
    {
        return "." + value;
    }
}
