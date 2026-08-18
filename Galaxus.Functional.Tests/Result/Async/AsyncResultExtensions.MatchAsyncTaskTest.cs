using System;
using System.Threading.Tasks;
using NUnit.Framework;
using static Galaxus.Functional.Tests.Result.Async.ResultFactory;

namespace Galaxus.Functional.Tests.Result.Async;

/// <summary>
/// Covers the <c>MatchAsync</c> overloads whose callbacks return a non-generic <see cref="Task" />.
/// The obsolete overloads guarding against a nested <see cref="Task" /> are covered by <see cref="NestedTaskGuards.NestedTaskGuardDiagnosticsTest" />.
/// </summary>
[TestFixture]
internal class MatchAsyncTaskTest
{
    public sealed class BothContinuationsReturnTask : MatchAsyncTaskTest
    {
        [Test]
        public async Task AwaitsOnOk_WhenResultIsOk()
        {
            var gate = new TaskCompletionSource<bool>();
            var onOkCompleted = false;
            var onErrCalled = false;

            var matching = CreateOk("ok")
                .MatchAsync(_ => AwaitGateAsync(gate.Task, () => onOkCompleted = true), _ => Mark(() => onErrCalled = true));

            Assert.IsFalse(matching.IsCompleted, "MatchAsync must not complete before onOk does");
            gate.SetResult(true);
            await matching;

            Assert.IsTrue(onOkCompleted);
            Assert.IsFalse(onErrCalled);
        }

        [Test]
        public async Task AwaitsOnErr_WhenResultIsErr()
        {
            var gate = new TaskCompletionSource<bool>();
            var onErrCompleted = false;
            var onOkCalled = false;

            var matching = CreateErr("err")
                .MatchAsync(_ => Mark(() => onOkCalled = true), _ => AwaitGateAsync(gate.Task, () => onErrCompleted = true));

            Assert.IsFalse(matching.IsCompleted, "MatchAsync must not complete before onErr does");
            gate.SetResult(true);
            await matching;

            Assert.IsTrue(onErrCompleted);
            Assert.IsFalse(onOkCalled);
        }
    }

    public sealed class SelfIsInTaskAndBothContinuationsReturnTask : MatchAsyncTaskTest
    {
        [Test]
        public async Task AwaitsOnOk_WhenResultIsOk()
        {
            var gate = new TaskCompletionSource<bool>();
            var onOkCompleted = false;

            var matching = CreateOkTask("ok").MatchAsync(_ => AwaitGateAsync(gate.Task, () => onOkCompleted = true), _ => Task.CompletedTask);

            Assert.IsFalse(matching.IsCompleted, "MatchAsync must not complete before onOk does");
            gate.SetResult(true);
            await matching;

            Assert.IsTrue(onOkCompleted);
        }

        [Test]
        public async Task AwaitsOnErr_WhenResultIsErr()
        {
            var gate = new TaskCompletionSource<bool>();
            var onErrCompleted = false;

            var matching = CreateErrTask("err").MatchAsync(_ => Task.CompletedTask, _ => AwaitGateAsync(gate.Task, () => onErrCompleted = true));

            Assert.IsFalse(matching.IsCompleted, "MatchAsync must not complete before onErr does");
            gate.SetResult(true);
            await matching;

            Assert.IsTrue(onErrCompleted);
        }
    }

    public sealed class OneContinuationIsAnAction : MatchAsyncTaskTest
    {
        [Test]
        public async Task AppliesOnOk_WhenOnOkIsAnAction()
        {
            var called = false;
            await CreateOk("ok").MatchAsync(_ => called = true, _ => Task.CompletedTask);
            Assert.IsTrue(called);
        }

        [Test]
        public async Task AppliesOnErr_WhenOnErrIsAnAction()
        {
            var called = false;
            await CreateErr("err").MatchAsync(_ => Task.CompletedTask, _ => called = true);
            Assert.IsTrue(called);
        }

        [Test]
        public async Task AppliesOnOk_WhenSelfIsInTaskAndOnOkIsAnAction()
        {
            var called = false;
            await CreateOkTask("ok").MatchAsync(_ => called = true, _ => Task.CompletedTask);
            Assert.IsTrue(called);
        }

        [Test]
        public async Task AppliesOnErr_WhenSelfIsInTaskAndOnErrIsAnAction()
        {
            var called = false;
            await CreateErrTask("err").MatchAsync(_ => Task.CompletedTask, _ => called = true);
            Assert.IsTrue(called);
        }

        [Test]
        public async Task AppliesOnOk_WhenSelfIsInTaskAndBothAreActions()
        {
            var called = false;
            await CreateOkTask("ok").MatchAsync(_ => called = true, _ => { });
            Assert.IsTrue(called);
        }

        [Test]
        public async Task AppliesOnErr_WhenSelfIsInTaskAndBothAreActions()
        {
            var called = false;
            await CreateErrTask("err").MatchAsync(_ => { }, _ => called = true);
            Assert.IsTrue(called);
        }

        [Test]
        public void ThrowsArgumentNullException_WhenTheSelectedActionIsNull()
        {
            Assert.ThrowsAsync<ArgumentNullException>(async () => await CreateOk("ok").MatchAsync((Action<string>)null, _ => Task.CompletedTask));
            Assert.ThrowsAsync<ArgumentNullException>(async () => await CreateErr("err").MatchAsync(_ => Task.CompletedTask, (Action<string>)null));
            Assert.ThrowsAsync<ArgumentNullException>(async () => await CreateOkTask("ok").MatchAsync((Action<string>)null, _ => Task.CompletedTask));
            Assert.ThrowsAsync<ArgumentNullException>(async () =>
                await CreateErrTask("err").MatchAsync(_ => Task.CompletedTask, (Action<string>)null));
        }

        [Test]
        public async Task DoesNotThrow_WhenAnUnselectedActionIsNull()
        {
            await CreateOk("ok").MatchAsync(_ => Task.CompletedTask, (Action<string>)null);
            await CreateErr("err").MatchAsync((Action<string>)null, _ => Task.CompletedTask);
            await CreateOkTask("ok").MatchAsync(_ => Task.CompletedTask, (Action<string>)null);
            await CreateErrTask("err").MatchAsync((Action<string>)null, _ => Task.CompletedTask);
        }
    }

    private static async Task AwaitGateAsync(Task gate, Action onGateOpened)
    {
        await gate;
        onGateOpened();
    }

    private static Task Mark(Action mark)
    {
        mark();
        return Task.CompletedTask;
    }
}
