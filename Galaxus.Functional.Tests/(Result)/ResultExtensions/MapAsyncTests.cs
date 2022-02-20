using System;
using System.Threading;
using System.Threading.Tasks;
using NUnit.Framework;

namespace Galaxus.Functional.Tests.ResultExtensions
{
    public class MapAsyncTests
    {
        [Test]
        public async Task MapOkAsync_SynchronousContinuation_ReturnsSuccessTask()
        {
            // arrange
            const string initialResult = "a";
            const string continuationResult = "b";

            string Continuation(string s)
            {
                return continuationResult;
            }

            var resultTask = Task.FromResult(Result<string, string>.FromOk(ok: initialResult))
                .MapAsync((Func<string, string>)Continuation);

            // act
            await resultTask;

            // assert
            Assert.AreEqual(expected: TaskStatus.RanToCompletion, actual: resultTask.Status);
        }

        [Test]
        public void MapOkAsync_PreviousTaskThrewArgumentException_ArgumentExceptionIsThrown()
        {
            // arrange
            const string continuationResult = "b";

            async Task<string> Continuation(string s)
            {
                return await Task.FromResult(result: continuationResult);
            }

            // act
            async Task Act()
            {
                await Task.FromException<Result<string, string>>(new ArgumentException())
                    .MapAsync((Func<string, Task<string>>)Continuation);
            }

            // assert
            Assert.ThrowsAsync<ArgumentException>(code: Act);
        }

        [Test]
        public void MapOkAsync_PreviousTaskWasCanceled_TaskCanceledExceptionIsThrown()
        {
            // arrange
            const string continuationResult = "b";

            async Task<string> Continuation(string s)
            {
                return await Task.FromResult(result: continuationResult);
            }

            var cancellationTokenSource = new CancellationTokenSource();

            // act
            cancellationTokenSource.Cancel();

            async Task Act()
            {
                await Task.FromCanceled<Result<string, string>>(cancellationToken: cancellationTokenSource.Token)
                    .MapAsync((Func<string, Task<string>>)Continuation);
            }

            // assert
            Assert.ThrowsAsync<TaskCanceledException>(code: Act);
        }

        [Test]
        public async Task MapErrAsync_SynchronousContinuation_ReturnsSuccessTask()
        {
            // arrange
            const string initialResult = "a";
            const string continuationResult = "b";

            string Continuation(string s)
            {
                return continuationResult;
            }

            var resultTask = Task.FromResult(Result<string, string>.FromErr(err: initialResult))
                .MapErrAsync((Func<string, string>)Continuation);

            // act
            await resultTask;

            // assert
            Assert.AreEqual(expected: TaskStatus.RanToCompletion, actual: resultTask.Status);
        }

        [Test]
        public void MapErrAsync_PreviousTaskThrewArgumentException_ArgumentExceptionIsThrown()
        {
            // arrange
            const string continuationResult = "b";

            async Task<string> Continuation(string s)
            {
                return await Task.FromResult(result: continuationResult);
            }

            // act
            async Task Act()
            {
                await Task.FromException<Result<string, string>>(new ArgumentException())
                    .MapErrAsync((Func<string, Task<string>>)Continuation);
            }

            // assert
            Assert.ThrowsAsync<ArgumentException>(code: Act);
        }

        [Test]
        public void MapErrAsync_PreviousTaskWasCanceled_TaskCanceledExceptionIsThrown()
        {
            // arrange
            const string continuationResult = "b";

            async Task<string> Continuation(string s)
            {
                return await Task.FromResult(result: continuationResult);
            }

            var cancellationTokenSource = new CancellationTokenSource();

            // act
            cancellationTokenSource.Cancel();

            async Task Act()
            {
                await Task.FromCanceled<Result<string, string>>(cancellationToken: cancellationTokenSource.Token)
                    .MapErrAsync((Func<string, Task<string>>)Continuation);
            }

            // assert
            Assert.ThrowsAsync<TaskCanceledException>(code: Act);
        }
    }
}
