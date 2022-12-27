using System;
using System.Threading.Tasks;
using NUnit.Framework;

namespace Galaxus.Functional.Tests.Option.Async;

[TestFixture]
internal class MatchAsyncTest
{
    public sealed class VoidReturnOnSomeTest : MatchAsyncTest
    {
        private readonly Option<string> some = OptionFactory.CreateSome("hello");

        [Test]
        public async Task BothContinuationsAreAsync_Works()
        {
            var called = false;
            await some.MatchAsync(_ =>
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
            await some.MatchAsync(_ =>
            {
                called = true;
            }, () => Task.CompletedTask);
            Assert.IsTrue(called);
        }

        [Test]
        public async Task OnSomeIsAsync_Works()
        {
            var called = false;
            await some.MatchAsync(_ =>
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
        private readonly Option<string> some = OptionFactory.CreateSome("hello");

        [Test]
        public async Task BothContinuationsAreAsync_Works()
        {
            var number = await some.MatchAsync(_ => Task.FromResult(42),
                () => Task.FromResult(-1));

            Assert.AreEqual(42, number);
        }

        [Test]
        public async Task OnNoneIsAsync_Works()
        {
            var number = await some.MatchAsync(_ => 42,
                () => Task.FromResult(-1));

            Assert.AreEqual(42, number);
        }

        [Test]
        public async Task OnSomeIsAsync_Works()
        {
            var number = await some.MatchAsync(_ => Task.FromResult(42),
                () => -1);

            Assert.AreEqual(42, number);
        }
    }

    public sealed class TaskOptionVoidReturnOnSomeTest : MatchAsyncTest
    {
        private readonly Task<Option<string>> someOptionTask = OptionFactory.CreateSomeTask("hello");

        [Test]
        public async Task BothContinuationsAreAsync_Works()
        {
            var called = false;
            await someOptionTask.MatchAsync(_ =>
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
            await someOptionTask.MatchAsync(_ =>
            {
                called = true;
            }, () => Task.CompletedTask);
            Assert.IsTrue(called);
        }

        [Test]
        public async Task OnSomeIsAsync_Works()
        {
            var called = false;
            await someOptionTask.MatchAsync(_ =>
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
            await someOptionTask.MatchAsync(_ =>
                {
                    called = true;
                },
                () => { });
            Assert.IsTrue(called);
        }
    }

    public sealed class TaskOptionValueReturnOnSomeTest : MatchAsyncTest
    {
        private readonly Task<Option<string>> someOptionTask = OptionFactory.CreateSomeTask("hello");

        [Test]
        public async Task BothContinuationsAreAsync_Works()
        {
            var number = await someOptionTask.MatchAsync(_ => Task.FromResult(42),
                () => Task.FromResult(-1));

            Assert.AreEqual(42, number);
        }

        [Test]
        public async Task OnNoneIsAsync_Works()
        {
            var number = await someOptionTask.MatchAsync(_ => 42,
                () => Task.FromResult(-1));

            Assert.AreEqual(42, number);
        }

        [Test]
        public async Task OnSomeIsAsync_Works()
        {
            var number = await someOptionTask.MatchAsync(_ => Task.FromResult(42),
                () => -1);

            Assert.AreEqual(42, number);
        }

        [Test]
        public async Task NeitherContinuationIsAsync_Works()
        {
            var number = await someOptionTask.MatchAsync(_ => 42,
                () => -1);

            Assert.AreEqual(42, number);
        }
    }

    public sealed class VoidReturnOnNoneTest : MatchAsyncTest
    {
        private readonly Option<string> none = Option<string>.None;

        [Test]
        public async Task OnSomeIsAsync_Works()
        {
            var called = false;
            await none.MatchAsync(_ => Task.CompletedTask, () =>
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
            await none.MatchAsync(_ =>
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
            await none.MatchAsync(_ => Task.CompletedTask,
                () =>
                {
                    called = true;
                });
            Assert.IsTrue(called);
        }
    }

    public sealed class ValueReturnOnNoneTest : MatchAsyncTest
    {
        private readonly Option<string> none = Option<string>.None;

        [Test]
        public async Task BothContinuationsAreAsync_Works()
        {
            var number = await none.MatchAsync(_ => Task.FromResult(42),
                () => Task.FromResult(-1));

            Assert.AreEqual(-1, number);
        }

        [Test]
        public async Task OnNoneIsAsync_Works()
        {
            var number = await none.MatchAsync(_ => 42,
                () => Task.FromResult(-1));

            Assert.AreEqual(-1, number);
        }

        [Test]
        public async Task OnSomeIsAsync_Works()
        {
            var number = await none.MatchAsync(_ => Task.FromResult(42),
                () => -1);

            Assert.AreEqual(-1, number);
        }
    }

    public class TaskOptionVoidReturnOnNoneTest : MatchAsyncTest
    {
        private readonly Task<Option<string>> noneTask = OptionFactory.CreateNoneTask();

        [Test]
        public async Task BothContinuationsAreAsync_Works()
        {
            var called = false;
            await noneTask.MatchAsync(_ => Task.CompletedTask, () =>
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
            await noneTask.MatchAsync(_ =>
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
            await noneTask.MatchAsync(_ => Task.CompletedTask,
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
            await noneTask.MatchAsync(_ =>
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
        private readonly Task<Option<string>> noneTask = OptionFactory.CreateNoneTask();

        [Test]
        public async Task BothContinuationsAreAsync_Works()
        {
            var number = await noneTask.MatchAsync(_ => Task.FromResult(42),
                () => Task.FromResult(-1));

            Assert.AreEqual(-1, number);
        }

        [Test]
        public async Task OnNoneIsAsync_Works()
        {
            var number = await noneTask.MatchAsync(_ => 42,
                () => Task.FromResult(-1));

            Assert.AreEqual(-1, number);
        }

        [Test]
        public async Task OnSomeIsAsync_Works()
        {
            var number = await noneTask.MatchAsync(_ => Task.FromResult(42),
                () => -1);

            Assert.AreEqual(-1, number);
        }

        [Test]
        public async Task NeitherContinuationIsAsync_Works()
        {
            var number = await noneTask.MatchAsync(_ => 42,
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
