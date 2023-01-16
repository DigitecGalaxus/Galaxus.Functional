using System;
using System.Threading.Tasks;
using NUnit.Framework;

namespace Galaxus.Functional.Tests.Option.Async;

[TestFixture]
internal class MatchAsyncTest
{
    private readonly Option<string> _some = OptionFactory.CreateSome("hello");
    private readonly Option<string> _none = Option<string>.None;
    private readonly Task<Option<string>> _someOptionTask = OptionFactory.CreateSomeTask("hello");
    private readonly Task<Option<string>> _noneTask = OptionFactory.CreateNoneTask();

    public sealed class VoidReturnOnSomeTest : MatchAsyncTest
    {
        [Test]
        public async Task BothContinuationsAreAsync_Works()
        {
            var called = false;
            await _some.MatchAsync(_ =>
            {
                called = true;
                return Task.CompletedTask;
            }, () => Task.CompletedTask);
            Assert.IsTrue(called);
        }

        [Test]
        public async Task OnNoneIsAsync_Works()
        {
            var called = false;
            await _some.MatchAsync(_ =>
            {
                called = true;
            }, () => Task.CompletedTask);
            Assert.IsTrue(called);
        }

        [Test]
        public async Task OnSomeIsAsync_Works()
        {
            var called = false;
            await _some.MatchAsync(_ =>
                {
                    called = true;
                    return Task.CompletedTask;
                },
                () => { });
            Assert.IsTrue(called);
        }
    }

    public sealed class ValueReturnOnSomeTest : MatchAsyncTest
    {
        [Test]
        public async Task BothContinuationsAreAsync_Works()
        {
            var number = await _some.MatchAsync(_ => Task.FromResult(42),
                () => Task.FromResult(-1));

            Assert.AreEqual(42, number);
        }

        [Test]
        public async Task OnNoneIsAsync_Works()
        {
            var number = await _some.MatchAsync(_ => 42,
                () => Task.FromResult(-1));

            Assert.AreEqual(42, number);
        }

        [Test]
        public async Task OnSomeIsAsync_Works()
        {
            var number = await _some.MatchAsync(_ => Task.FromResult(42),
                () => -1);

            Assert.AreEqual(42, number);
        }
    }

    public sealed class TaskOptionVoidReturnOnSomeTest : MatchAsyncTest
    {
        [Test]
        public async Task BothContinuationsAreAsync_Works()
        {
            var called = false;
            await _someOptionTask.MatchAsync(_ =>
            {
                called = true;
                return Task.CompletedTask;
            }, () => Task.CompletedTask);
            Assert.IsTrue(called);
        }

        [Test]
        public async Task OnNoneIsAsync_Works()
        {
            var called = false;
            await _someOptionTask.MatchAsync(_ =>
            {
                called = true;
            }, () => Task.CompletedTask);
            Assert.IsTrue(called);
        }

        [Test]
        public async Task OnSomeIsAsync_Works()
        {
            var called = false;
            await _someOptionTask.MatchAsync(_ =>
                {
                    called = true;
                    return Task.CompletedTask;
                },
                () => { });
            Assert.IsTrue(called);
        }

        [Test]
        public async Task NeitherContinuationIsAsync_Works()
        {
            var called = false;
            await _someOptionTask.MatchAsync(_ =>
                {
                    called = true;
                },
                () => { });
            Assert.IsTrue(called);
        }
    }

    public sealed class TaskOptionValueReturnOnSomeTest : MatchAsyncTest
    {
        [Test]
        public async Task BothContinuationsAreAsync_Works()
        {
            var number = await _someOptionTask.MatchAsync(_ => Task.FromResult(42),
                () => Task.FromResult(-1));

            Assert.AreEqual(42, number);
        }

        [Test]
        public async Task OnNoneIsAsync_Works()
        {
            var number = await _someOptionTask.MatchAsync(_ => 42,
                () => Task.FromResult(-1));

            Assert.AreEqual(42, number);
        }

        [Test]
        public async Task OnSomeIsAsync_Works()
        {
            var number = await _someOptionTask.MatchAsync(_ => Task.FromResult(42),
                () => -1);

            Assert.AreEqual(42, number);
        }

        [Test]
        public async Task NeitherContinuationIsAsync_Works()
        {
            var number = await _someOptionTask.MatchAsync(_ => 42,
                () => -1);

            Assert.AreEqual(42, number);
        }
    }

    public sealed class VoidReturnOnNoneTest : MatchAsyncTest
    {
        [Test]
        public async Task OnSomeIsAsync_Works()
        {
            var called = false;
            await _none.MatchAsync(_ => Task.CompletedTask, () =>
            {
                called = true;
                return Task.CompletedTask;
            });
            Assert.IsTrue(called);
        }

        [Test]
        public async Task OnNoneIsAsync_Works()
        {
            var called = false;
            await _none.MatchAsync(_ =>
            {
            }, () =>
            {
                called = true;
                return Task.CompletedTask;
            });
            Assert.IsTrue(called);
        }

        [Test]
        public async Task BothContinuationsAreAsync_Works()
        {
            var called = false;
            await _none.MatchAsync(_ => Task.CompletedTask,
                () =>
                {
                    called = true;
                });
            Assert.IsTrue(called);
        }
    }

    public sealed class ValueReturnOnNoneTest : MatchAsyncTest
    {
        [Test]
        public async Task BothContinuationsAreAsync_Works()
        {
            var number = await _none.MatchAsync(_ => Task.FromResult(42),
                () => Task.FromResult(-1));

            Assert.AreEqual(-1, number);
        }

        [Test]
        public async Task OnNoneIsAsync_Works()
        {
            var number = await _none.MatchAsync(_ => 42,
                () => Task.FromResult(-1));

            Assert.AreEqual(-1, number);
        }

        [Test]
        public async Task OnSomeIsAsync_Works()
        {
            var number = await _none.MatchAsync(_ => Task.FromResult(42),
                () => -1);

            Assert.AreEqual(-1, number);
        }
    }

    public class TaskOptionVoidReturnOnNoneTest : MatchAsyncTest
    {
        [Test]
        public async Task BothContinuationsAreAsync_Works()
        {
            var called = false;
            await _noneTask.MatchAsync(_ => Task.CompletedTask, () =>
            {
                called = true;
                return Task.CompletedTask;
            });
            Assert.IsTrue(called);
        }

        [Test]
        public async Task OnNoneIsAsync_Works()
        {
            var called = false;
            await _noneTask.MatchAsync(_ =>
            {
            }, () =>
            {
                called = true;
                return Task.CompletedTask;
            });
            Assert.IsTrue(called);
        }

        [Test]
        public async Task OnSomeIsAsync_Works()
        {
            var called = false;
            await _noneTask.MatchAsync(_ => Task.CompletedTask,
                () =>
                {
                    called = true;
                });
            Assert.IsTrue(called);
        }

        [Test]
        public async Task NeitherContinuationIsAsync_Works()
        {
            var called = false;
            await _noneTask.MatchAsync(_ =>
                {
                },
                () =>
                {
                    called = true;
                });
            Assert.IsTrue(called);
        }
    }

    public class TaskOptionValueReturnOnNoneTest : MatchAsyncTest
    {
        [Test]
        public async Task BothContinuationsAreAsync_Works()
        {
            var number = await _noneTask.MatchAsync(_ => Task.FromResult(42),
                () => Task.FromResult(-1));

            Assert.AreEqual(-1, number);
        }

        [Test]
        public async Task OnNoneIsAsync_Works()
        {
            var number = await _noneTask.MatchAsync(_ => 42,
                () => Task.FromResult(-1));

            Assert.AreEqual(-1, number);
        }

        [Test]
        public async Task OnSomeIsAsync_Works()
        {
            var number = await _noneTask.MatchAsync(_ => Task.FromResult(42),
                () => -1);

            Assert.AreEqual(-1, number);
        }

        [Test]
        public async Task NeitherContinuationIsAsync_Works()
        {
            var number = await _noneTask.MatchAsync(_ => 42,
                () => -1);

            Assert.AreEqual(-1, number);
        }
    }

    [Test]
    public void Option_MatchAsyncThrowsIfMatchArmIsNull()
    {
        Assert.ThrowsAsync<ArgumentNullException>(async () =>
        {
            await OptionFactory.CreateSome(0).MatchAsync(null, () => Task.CompletedTask);
        });

        Assert.ThrowsAsync<ArgumentNullException>(async () => { await Option<int>.None.MatchAsync(_ => Task.CompletedTask, null); });

        Assert.ThrowsAsync<ArgumentNullException>(async () =>
        {
            _ = await OptionFactory.CreateSome(0).MatchAsync((Func<int, Task<int>>)null, () => Task.FromResult(0));
        });
        Assert.ThrowsAsync<ArgumentNullException>(async () => { _ = await Option<int>.None.MatchAsync(Task.FromResult, (Func<Task<int>>)null); });
    }
}
