using System;
using System.Threading.Tasks;
using NUnit.Framework;

namespace Galaxus.Functional.Tests.Either.Async;

/// <summary>
/// Covers the <c>MatchAsync</c> overloads whose callbacks return a non-generic <see cref="Task" />.
/// The obsolete overloads guarding against a nested <see cref="Task" /> are covered by <see cref="NestedTaskGuards.NestedTaskGuardDiagnosticsTest" />.
/// </summary>
[TestFixture]
internal class MatchAsyncTaskTest
{
    public sealed class AllContinuationsReturnTask : MatchAsyncTaskTest
    {
        [Test]
        public async Task AwaitsOnA_WhenSelfIsA()
        {
            var gate = new TaskCompletionSource<bool>();
            var onACompleted = false;
            var onBCalled = false;

            var matching = CreateA("a")
                .MatchAsync(_ => AwaitGateAsync(gate.Task, () => onACompleted = true), _ => Mark(() => onBCalled = true));

            Assert.IsFalse(matching.IsCompleted, "MatchAsync must not complete before onA does");
            gate.SetResult(true);
            await matching;

            Assert.IsTrue(onACompleted);
            Assert.IsFalse(onBCalled);
        }

        [Test]
        public async Task AwaitsOnB_WhenSelfIsB()
        {
            var gate = new TaskCompletionSource<bool>();
            var onBCompleted = false;

            var matching = CreateB("b").MatchAsync(_ => Task.CompletedTask, _ => AwaitGateAsync(gate.Task, () => onBCompleted = true));

            Assert.IsFalse(matching.IsCompleted, "MatchAsync must not complete before onB does");
            gate.SetResult(true);
            await matching;

            Assert.IsTrue(onBCompleted);
        }

        [Test]
        public async Task AwaitsOnC_WhenSelfIsC()
        {
            var gate = new TaskCompletionSource<bool>();
            var onCCompleted = false;

            var matching = new Either<string, string, string>(c: "c")
                .MatchAsync(_ => Task.CompletedTask, _ => Task.CompletedTask, _ => AwaitGateAsync(gate.Task, () => onCCompleted = true));

            Assert.IsFalse(matching.IsCompleted, "MatchAsync must not complete before onC does");
            gate.SetResult(true);
            await matching;

            Assert.IsTrue(onCCompleted);
        }
    }

    public sealed class SomeContinuationsAreActions : MatchAsyncTaskTest
    {
        [Test]
        public async Task AppliesOnA_WhenOnAIsAnAction()
        {
            var called = false;
            await CreateA("a").MatchAsync(_ => called = true, _ => Task.CompletedTask);
            Assert.IsTrue(called);
        }

        [Test]
        public async Task AwaitsOnB_WhenOnAIsAnAction()
        {
            var gate = new TaskCompletionSource<bool>();
            var onBCompleted = false;

            var matching = CreateB("b").MatchAsync(_ => { }, _ => AwaitGateAsync(gate.Task, () => onBCompleted = true));

            Assert.IsFalse(matching.IsCompleted, "MatchAsync must not complete before onB does");
            gate.SetResult(true);
            await matching;

            Assert.IsTrue(onBCompleted);
        }

        [Test]
        public async Task AppliesOnB_WhenOnBIsAnAction()
        {
            var called = false;
            await CreateB("b").MatchAsync(_ => Task.CompletedTask, _ => called = true);
            Assert.IsTrue(called);
        }

        [Test]
        public async Task AwaitsOnA_WhenOnBIsAnAction()
        {
            var gate = new TaskCompletionSource<bool>();
            var onACompleted = false;

            var matching = CreateA("a").MatchAsync(_ => AwaitGateAsync(gate.Task, () => onACompleted = true), _ => { });

            Assert.IsFalse(matching.IsCompleted, "MatchAsync must not complete before onA does");
            gate.SetResult(true);
            await matching;

            Assert.IsTrue(onACompleted);
        }

        [Test]
        public async Task AppliesOnA_WhenOnlyOnAIsAnAction()
        {
            var called = false;
            await CreateAbc(a: "a").MatchAsync(_ => called = true, _ => Task.CompletedTask, _ => Task.CompletedTask);
            Assert.IsTrue(called);
        }

        [Test]
        public async Task AppliesOnB_WhenOnlyOnBIsAnAction()
        {
            var called = false;
            await CreateAbc(b: "b").MatchAsync(_ => Task.CompletedTask, _ => called = true, _ => Task.CompletedTask);
            Assert.IsTrue(called);
        }

        [Test]
        public async Task AppliesOnC_WhenOnlyOnCIsAnAction()
        {
            var called = false;
            await CreateAbc(c: "c").MatchAsync(_ => Task.CompletedTask, _ => Task.CompletedTask, _ => called = true);
            Assert.IsTrue(called);
        }

        [Test]
        public async Task AwaitsOnC_WhenOnAAndOnBAreActions()
        {
            var gate = new TaskCompletionSource<bool>();
            var onCCompleted = false;

            var matching = CreateAbc(c: "c").MatchAsync(_ => { }, _ => { }, _ => AwaitGateAsync(gate.Task, () => onCCompleted = true));

            Assert.IsFalse(matching.IsCompleted, "MatchAsync must not complete before onC does");
            gate.SetResult(true);
            await matching;

            Assert.IsTrue(onCCompleted);
        }

        [Test]
        public async Task AwaitsOnB_WhenOnAAndOnCAreActions()
        {
            var gate = new TaskCompletionSource<bool>();
            var onBCompleted = false;

            var matching = CreateAbc(b: "b").MatchAsync(_ => { }, _ => AwaitGateAsync(gate.Task, () => onBCompleted = true), _ => { });

            Assert.IsFalse(matching.IsCompleted, "MatchAsync must not complete before onB does");
            gate.SetResult(true);
            await matching;

            Assert.IsTrue(onBCompleted);
        }

        [Test]
        public async Task AwaitsOnA_WhenOnBAndOnCAreActions()
        {
            var gate = new TaskCompletionSource<bool>();
            var onACompleted = false;

            var matching = CreateAbc(a: "a").MatchAsync(_ => AwaitGateAsync(gate.Task, () => onACompleted = true), _ => { }, _ => { });

            Assert.IsFalse(matching.IsCompleted, "MatchAsync must not complete before onA does");
            gate.SetResult(true);
            await matching;

            Assert.IsTrue(onACompleted);
        }

        [Test]
        public async Task AppliesOnA_WhenOnAAndOnBAreActions()
        {
            var called = false;
            await CreateAbc(a: "a").MatchAsync(_ => called = true, _ => { }, _ => Task.CompletedTask);
            Assert.IsTrue(called);
        }

        [Test]
        public void ThrowsArgumentNullException_WhenTheSelectedActionIsNull()
        {
            Assert.ThrowsAsync<ArgumentNullException>(async () => await CreateA("a").MatchAsync((Action<string>)null, _ => Task.CompletedTask));
            Assert.ThrowsAsync<ArgumentNullException>(async () => await CreateB("b").MatchAsync(_ => Task.CompletedTask, (Action<string>)null));
            Assert.ThrowsAsync<ArgumentNullException>(
                async () => await CreateAbc(c: "c").MatchAsync(_ => { }, _ => Task.CompletedTask, (Action<string>)null));
        }

        [Test]
        public async Task DoesNotThrow_WhenAnUnselectedActionIsNull()
        {
            await CreateB("b").MatchAsync((Action<string>)null, _ => Task.CompletedTask);
            await CreateAbc(b: "b").MatchAsync((Action<string>)null, _ => Task.CompletedTask, (Action<string>)null);
        }
    }

    private static Either<string, string> CreateA(string value)
    {
        return new Either<string, string>(a: value);
    }

    private static Either<string, string> CreateB(string value)
    {
        return new Either<string, string>(b: value);
    }

    private static Either<string, string, string> CreateAbc(string a = null, string b = null, string c = null)
    {
        if (a is not null)
        {
            return new Either<string, string, string>(a: a);
        }

        return b is not null ? new Either<string, string, string>(b: b) : new Either<string, string, string>(c: c);
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
