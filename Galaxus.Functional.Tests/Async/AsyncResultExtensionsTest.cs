using System.Threading.Tasks;
using Galaxus.Functional.Async;
using NUnit.Framework;

namespace Galaxus.Functional.Tests.Async;

[TestFixture]
internal partial class AsyncResultExtensionsTest
{
    public sealed class IfOkAsyncTest : AsyncResultExtensionsTest
    {
        [Test]
        public async Task ContinuationIsExecuted_WhenSelfIsOk()
        {
            await CreateOk("ok").IfOkAsync(StoreValueAsync);
            AssertStoredValue("ok");
        }

        [Test]
        public async Task ContinuationIsNotExecuted_WhenSelfIsErr()
        {
            await CreateErr("err").IfOkAsync(StoreValueAsync);
            AssertNoValueStored();
        }
    }

    public sealed class IfErrAsyncTest : AsyncResultExtensionsTest
    {
        [Test]
        public async Task ContinuationIsExecuted_WhenSelfIsErr()
        {
            await CreateErr("err").IfErrAsync(StoreValueAsync);
            AssertStoredValue("err");
        }

        [Test]
        public async Task ContinuationIsNotExecuted_WhenSelfIsOk()
        {
            await CreateOk("ok").IfErrAsync(StoreValueAsync);
            AssertNoValueStored();
        }
    }

    public class AndThenAsyncTest : AsyncResultExtensionsTest
    {
        public sealed class ContinuationIsAsync : AndThenAsyncTest
        {
            [Test]
            public async Task ContinuationIsApplied_WhenSelfIsOk()
            {
                var continuation = await CreateOk("ok").AndThenAsync(StoreValueAndReturnOkAsync);
                AssertStoredValue("ok");
                AssertOk(continuation, "ok");
            }

            [Test]
            public async Task ContinuationIsNotApplied_WhenSelfIsErr()
            {
                var continuation = await CreateErr("err").AndThenAsync(StoreValueAndReturnOkAsync);
                AssertErr(continuation, "err");
                AssertNoValueStored();
            }
        }

        public sealed class SelfIsInTask : AndThenAsyncTest
        {
            [Test]
            public async Task ContinuationIsApplied_WhenAwaitedSelfIsOk()
            {
                var continuation = await CreateOkTask("ok").AndThenAsync(StoreValueAndReturnOk);
                AssertStoredValue("ok");
                AssertOk(continuation, "ok");
            }

            [Test]
            public async Task ContinuationIsNotApplied_WhenAwaitedSelfIsErr()
            {
                var continuation = await CreateErrTask("err").AndThenAsync(StoreValueAndReturnOk);
                AssertErr(continuation, "err");
                AssertNoValueStored();
            }
        }

        public sealed class SelfIsInTaskAndContinuationIsAsync : AndThenAsyncTest
        {
            [Test]
            public async Task ContinuationIsApplied_WhenAwaitedSelfIsOk()
            {
                var continuation = await CreateOkTask("ok").AndThenAsync(StoreValueAndReturnOkAsync);
                AssertStoredValue("ok");
                AssertOk(continuation, "ok");
            }

            [Test]
            public async Task ContinuationIsNotApplied_WhenAwaitedSelfIsErr()
            {
                var continuation = await CreateErrTask("err").AndThenAsync(StoreValueAndReturnOkAsync);
                AssertErr(continuation, "err");
                AssertNoValueStored();
            }
        }
    }

    public class MapAsyncTest : AsyncResultExtensionsTest
    {
        public sealed class ContinuationIsAsync : AndThenAsyncTest
        {
            [Test]
            public async Task ContinuationIsApplied_WhenSelfIsOk()
            {
                var continuation = await CreateOk("ok").MapAsync(AppendPeriodAsync);
                AssertOk(continuation, "ok.");
            }

            [Test]
            public async Task ContinuationIsNotApplied_WhenSelfIsErr()
            {
                var continuation = await CreateErr("err").MapAsync(AppendPeriodAsync);
                AssertErr(continuation, "err");
            }
        }

        public sealed class SelfIsInTask : AndThenAsyncTest
        {
            [Test]
            public async Task ContinuationIsApplied_WhenAwaitedSelfIsOk()
            {
                var continuation = await CreateOkTask("ok").MapAsync(AppendPeriod);
                AssertOk(continuation, "ok.");
            }

            [Test]
            public async Task ContinuationIsNotApplied_WhenAwaitedSelfIsErr()
            {
                var continuation = await CreateErrTask("err").MapAsync(AppendPeriod);
                AssertErr(continuation, "err");
            }
        }

        public sealed class SelfIsInTaskAndContinuationIsAsync : AndThenAsyncTest
        {
            [Test]
            public async Task ContinuationIsApplied_WhenAwaitedSelfIsOk()
            {
                var continuation = await CreateOkTask("ok").MapAsync(AppendPeriodAsync);
                AssertOk(continuation, "ok.");
            }

            [Test]
            public async Task ContinuationIsNotApplied_WhenAwaitedSelfIsErr()
            {
                var continuation = await CreateErrTask("err").MapAsync(AppendPeriodAsync);
                AssertErr(continuation, "err");
            }
        }
    }

    public class MapErrAsyncTest : AsyncResultExtensionsTest
    {
        public sealed class ContinuationIsAsync : AndThenAsyncTest
        {
            [Test]
            public async Task ContinuationIsApplied_WhenSelfIsErr()
            {
                var continuation = await CreateErr("err").MapErrAsync(AppendPeriodAsync);
                AssertErr(continuation, "err.");
            }

            [Test]
            public async Task ContinuationIsNotApplied_WhenSelfIsOk()
            {
                var continuation = await CreateOk("ok").MapErrAsync(AppendPeriodAsync);
                AssertOk(continuation, "ok");
            }
        }

        public sealed class SelfIsInTask : AndThenAsyncTest
        {
            [Test]
            public async Task ContinuationIsApplied_WhenAwaitedSelfIsErr()
            {
                var continuation = await CreateErrTask("err").MapErrAsync(AppendPeriod);
                AssertErr(continuation, "err.");
            }

            [Test]
            public async Task ContinuationIsNotApplied_WhenAwaitedSelfIsOk()
            {
                var continuation = await CreateOkTask("ok").MapErrAsync(AppendPeriod);
                AssertOk(continuation, "ok");
            }
        }

        public sealed class SelfIsInTaskAndContinuationIsAsync : AndThenAsyncTest
        {
            [Test]
            public async Task ContinuationIsApplied_WhenAwaitedSelfIsErr()
            {
                var continuation = await CreateErrTask("err").MapErrAsync(AppendPeriodAsync);
                AssertErr(continuation, "err.");
            }

            [Test]
            public async Task ContinuationIsNotApplied_WhenAwaitedSelfIsOk()
            {
                var continuation = await CreateOkTask("ok").MapErrAsync(AppendPeriodAsync);
                AssertOk(continuation, "ok");
            }
        }
    }

    public class OrElseAsyncTest : AsyncResultExtensionsTest
    {
        public class ContinuationIsAsync : OrElseAsyncTest
        {
            [Test]
            public async Task ContinuationIsApplied_WhenSelfIsErr()
            {
                var continuation = await CreateErr("err").OrElseAsync(AppendPeriodAndReturnOkAsync);
                AssertOk(continuation, "err.");
            }

            [Test]
            public async Task ContinuationIsNotApplied_WhenSelfIsOkErr()
            {
                var continuation = await CreateOk("ok").OrElseAsync(AppendPeriodAndReturnOkAsync);
                AssertOk(continuation, "ok");
            }
        }

        public class SelfIsInTask : OrElseAsyncTest
        {
            [Test]
            public async Task ContinuationIsApplied_WhenSelfIsErr()
            {

                var continuation = await CreateErrTask("err").OrElseAsync(AppendPeriodAndReturnOk);
                AssertOk(continuation, "err.");
            }

            [Test]
            public async Task ContinuationIsNotApplied_WhenSelfIsOkErr()
            {
                var continuation = await CreateOkTask("ok").OrElseAsync(AppendPeriodAndReturnOk);
                AssertOk(continuation, "ok");
            }
        }

        public class SelfIsInTaskAndContinuationIsAsync : OrElseAsyncTest
        {
            [Test]
            public async Task ContinuationIsApplied_WhenSelfIsErr()
            {

                var continuation = await CreateErrTask("err").OrElseAsync(AppendPeriodAndReturnOkAsync);
                AssertOk(continuation, "err.");
            }

            [Test]
            public async Task ContinuationIsNotApplied_WhenSelfIsOkErr()
            {
                var continuation = await CreateOkTask("ok").OrElseAsync(AppendPeriodAndReturnOkAsync);
                AssertOk(continuation, "ok");
            }
        }
    }

    public class MatchAsyncTest : AsyncResultExtensionsTest
    {
        public class SelfIsInTask : MatchAsyncTest
        {
            [Test]
            public async Task AppliesOnOk_WhenResultIsOk()
            {
                var result = await CreateOkTask("ok").MatchAsync(s => $"{s}.", s => $".{s}");
                Assert.AreEqual("ok.", result);
            }

            [Test]
            public async Task AppliesOnErr_WhenResultIsErr()
            {
                var result = await CreateErrTask("err").MatchAsync(s => $"{s}.", s => $".{s}");
                Assert.AreEqual(".err", result);
            }
        }

        public class SelfIsInTaskAndOnOkIsAsync : MatchAsyncTest
        {
            [Test]
            public async Task AppliesOnOk_WhenResultIsOk()
            {
                var result = await CreateOkTask("ok").MatchAsync(s => Task.FromResult($"{s}."), s => $".{s}");
                Assert.AreEqual("ok.", result);
            }

            [Test]
            public async Task AppliesOnErr_WhenResultIsErr()
            {
                var result = await CreateErrTask("err").MatchAsync(s => Task.FromResult($"{s}."), s => $".{s}");
                Assert.AreEqual(".err", result);
            }
        }

        public class SelfIsInTaskAndOnErrIsAsync : MatchAsyncTest
        {
            [Test]
            public async Task AppliesOnOk_WhenResultIsOk()
            {
                var result = await CreateOkTask("ok").MatchAsync(s => $"{s}.", s => Task.FromResult($".{s}"));
                Assert.AreEqual("ok.", result);
            }

            [Test]
            public async Task AppliesOnErr_WhenResultIsErr()
            {
                var result = await CreateErrTask("err").MatchAsync(s => $"{s}.", s => Task.FromResult($".{s}"));
                Assert.AreEqual(".err", result);
            }
        }

        public class SelfIsInTaskAndBothContinuationsAreAsync : MatchAsyncTest
        {
            [Test]
            public async Task AppliesOnOk_WhenResultIsOk()
            {
                var result = await CreateOkTask("ok").MatchAsync(s => Task.FromResult($"{s}."), s => Task.FromResult($".{s}"));
                Assert.AreEqual("ok.", result);
            }

            [Test]
            public async Task AppliesOnErr_WhenResultIsErr()
            {
                var result = await CreateErrTask("err").MatchAsync(s => Task.FromResult($"{s}.("), s => Task.FromResult($".{s}"));
                Assert.AreEqual(".err", result);
            }
        }

        public class OnOkIsAsync : MatchAsyncTest
        {
            [Test]
            public async Task AppliesOnOk_WhenResultIsOk()
            {
                var result = await CreateOk("ok").MatchAsync(s => Task.FromResult($"{s}."), s => $".{s}");
                Assert.AreEqual("ok.", result);
            }

            [Test]
            public async Task AppliesOnErr_WhenResultIsErr()
            {
                var result = await CreateErr("err").MatchAsync(s => Task.FromResult($"{s}."), s => $".{s}");
                Assert.AreEqual(".err", result);
            }
        }

        public class OnErrIsAsync : MatchAsyncTest
        {
            [Test]
            public async Task AppliesOnOk_WhenResultIsOk()
            {
                var result = await CreateOk("ok").MatchAsync(s => $"{s}.", s => Task.FromResult($".{s}"));
                Assert.AreEqual("ok.", result);
            }

            [Test]
            public async Task AppliesOnErr_WhenResultIsErr()
            {
                var result = await CreateErr("err").MatchAsync(s => $"{s}.", s => Task.FromResult($".{s}"));
                Assert.AreEqual(".err", result);
            }
        }

        public class BothContinuationsAreAsync : MatchAsyncTest
        {
            [Test]
            public async Task AppliesOnOk_WhenResultIsOk()
            {
                var result = await CreateOk("ok").MatchAsync(s => Task.FromResult($"{s}."), s => Task.FromResult($".{s}"));
                Assert.AreEqual("ok.", result);
            }

            [Test]
            public async Task AppliesOnErr_WhenResultIsErr()
            {
                var result = await CreateErr("err").MatchAsync(s => Task.FromResult($"{s}."), s => Task.FromResult($".{s}"));
                Assert.AreEqual(".err", result);
            }
        }
    }
}
