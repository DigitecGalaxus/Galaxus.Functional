using NUnit.Framework;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Galaxus.Functional.Tests
{
    public class TExtensionsForResultTests
    {
        [Test]
        public async Task MapOkAsync_SynchronousContinuation_ReturnsSuccessTask()
        {
            const string initialResult = "a";
            const string continuationResult = "b";
            Func<string, string> continuation = s => continuationResult;
            var resultTask = Task.FromResult(Result<string, string>.FromOk(initialResult))
                .MapAsync(continuation);
            await resultTask;
            Assert.AreEqual(expected: TaskStatus.RanToCompletion, actual: resultTask.Status);
        }

        [Test]
        public void MapOkAsync_PreviousTaskThrewArgumentException_ArgumentExceptionIsThrown()
        {
            Assert.ThrowsAsync<ArgumentException>(async () =>
            {
                const string continuationResult = "b";
                Func<string, Task<string>> continuation = s => Task.FromResult(continuationResult);
                await Task.FromException<Result<string, string>>(new ArgumentException())
                    .MapAsync(continuation);
            });
        }

        [Test]
        public void MapOkAsync_PreviousTaskWasCanceled_TaskCanceledExceptionIsThrown()
        {
            Assert.ThrowsAsync<TaskCanceledException>(async () =>
            {
                const string continuationResult = "b";
                Func<string, Task<string>> continuation = s => Task.FromResult(continuationResult);
                var cancellationTokenSource = new CancellationTokenSource();
                cancellationTokenSource.Cancel();
                await Task.FromCanceled<Result<string, string>>(cancellationTokenSource.Token)
                    .MapAsync(continuation);
            });
        }

        [Test]
        public async Task MapErrAsync_SynchronousContinuation_ReturnsSuccessTask()
        {
            const string initialResult = "a";
            const string continuationResult = "b";
            Func<string, string> continuation = s => continuationResult;
            var resultTask = Task.FromResult(Result<string, string>.FromErr(initialResult))
                .MapErrAsync(continuation);
            await resultTask;
            Assert.AreEqual(expected: TaskStatus.RanToCompletion, actual: resultTask.Status);
        }

        [Test]
        public void MapErrAsync_PreviousTaskThrewArgumentException_ArgumentExceptionIsThrown()
        {
            Assert.ThrowsAsync<ArgumentException>(async () =>
            {
                const string continuationResult = "b";
                Func<string, Task<string>> continuation = s => Task.FromResult(continuationResult);
                await Task.FromException<Result<string, string>>(new ArgumentException())
                    .MapErrAsync(continuation);
            });
        }

        [Test]
        public void MapErrAsync_PreviousTaskWasCanceled_TaskCanceledExceptionIsThrown()
        {
            Assert.ThrowsAsync<TaskCanceledException>(async () =>
            {
                const string continuationResult = "b";
                Func<string, Task<string>> continuation = s => Task.FromResult(continuationResult);
                var cancellationTokenSource = new CancellationTokenSource();
                cancellationTokenSource.Cancel();
                await Task.FromCanceled<Result<string, string>>(cancellationTokenSource.Token)
                    .MapErrAsync(continuation);
            });
        }

        [Test]
        public void AndThenAsync_PreviousTaskThrewArgumentException_ArgumentExceptionIsThrown()
        {
            Assert.ThrowsAsync<ArgumentException>(async () =>
            {
                const string continuationResult = "b";
                Func<string, Task<Result<string, string>>> continuation = s => Task.FromResult(Result<string, string>.FromOk(continuationResult));
                await Task.FromException<Result<string, string>>(new ArgumentException())
                    .AndThenAsync(continuation);
            });
        }

        [Test]
        public void AndThenAsync_PreviousTaskWasCanceled_TaskCanceledExceptionIsThrown()
        {
            Assert.ThrowsAsync<TaskCanceledException>(async () =>
            {
                const string continuationResult = "b";
                Func<string, Task<Result<string, string>>> continuation = s => Task.FromResult(Result<string, string>.FromOk(continuationResult));
                var cancellationTokenSource = new CancellationTokenSource();
                cancellationTokenSource.Cancel();
                await Task.FromCanceled<Result<string, string>>(cancellationTokenSource.Token)
                    .AndThenAsync(continuation);
            });
        }

        [Test]
        public async Task AndThenAsync_SynchronousContinuation_ReturnsSuccessTask()
        {
            const string initialResult = "a";
            const string continuationResult = "b";
            Func<string, Result<string, string>> continuation = s => Result<string, string>.FromOk(continuationResult);
            var resultTask = Task.FromResult(Result<string, string>.FromOk(initialResult))
                .AndThenAsync(continuation);
            await resultTask;
            Assert.AreEqual(expected: TaskStatus.RanToCompletion, actual: resultTask.Status);
        }

        [Test]
        public void OrElseAsync_PreviousTaskThrewArgumentException_ArgumentExceptionIsThrown()
        {
            Assert.ThrowsAsync<ArgumentException>(async () =>
            {
                const string continuationResult = "b";
                Func<string, Task<Result<string, string>>> continuation = s => Task.FromResult(Result<string, string>.FromOk(continuationResult));
                await Task.FromException<Result<string, string>>(new ArgumentException())
                    .OrElseAsync(continuation);
            });
        }

        [Test]
        public void OrElseAsync_PreviousTaskWasCanceled_TaskCanceledExceptionIsThrown()
        {
            Assert.ThrowsAsync<TaskCanceledException>(async () =>
            {
                const string continuationResult = "b";
                Func<string, Task<Result<string, string>>> continuation = s => Task.FromResult(Result<string, string>.FromOk(continuationResult));
                var cancellationTokenSource = new CancellationTokenSource();
                cancellationTokenSource.Cancel();
                await Task.FromCanceled<Result<string, string>>(cancellationTokenSource.Token)
                    .OrElseAsync(continuation);
            });
        }

        [Test]
        public async Task OrElseAsync_SynchronousContinuation_ReturnsSuccessTask()
        {
            const string initialResult = "a";
            const string continuationResult = "b";
            Func<string, Result<string, string>> continuation = s => Result<string, string>.FromErr(continuationResult);
            var resultTask = Task.FromResult(Result<string, string>.FromErr(initialResult))
                .OrElseAsync(continuation);
            await resultTask;
            Assert.AreEqual(expected: TaskStatus.RanToCompletion, actual: resultTask.Status);
        }
    }
}
