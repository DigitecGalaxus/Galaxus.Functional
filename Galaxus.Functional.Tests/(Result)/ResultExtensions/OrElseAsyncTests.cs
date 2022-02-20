using System;
using System.Threading;
using System.Threading.Tasks;
using NUnit.Framework;

namespace Galaxus.Functional.Tests.ResultExtensions
{
    public class OrElseAsyncTests
    {
        [Test]
        public void OrElseAsync_PreviousTaskThrewArgumentException_ArgumentExceptionIsThrown()
        {
            // arrange
            const string continuationResult = "b";

            async Task<Result<string, string>> Continuation(string s)
            {
                return await Task.FromResult(Result<string, string>.FromOk(ok: continuationResult));
            }

            // act
            async Task Act()
            {
                await Task.FromException<Result<string, string>>(new ArgumentException())
                    .OrElseAsync((Func<string, Task<Result<string, string>>>)Continuation);
            }

            // assert
            Assert.ThrowsAsync<ArgumentException>(code: Act);
        }

        [Test]
        public void OrElseAsync_PreviousTaskWasCanceled_TaskCanceledExceptionIsThrown()
        {
            // arrange
            const string continuationResult = "b";

            async Task<Result<string, string>> Continuation(string s)
            {
                return await Task.FromResult(Result<string, string>.FromOk(ok: continuationResult));
            }

            var cancellationTokenSource = new CancellationTokenSource();

            // act
            cancellationTokenSource.Cancel();

            async Task Act()
            {
                await Task.FromCanceled<Result<string, string>>(cancellationToken: cancellationTokenSource.Token)
                    .OrElseAsync((Func<string, Task<Result<string, string>>>)Continuation);
            }

            // assert
            Assert.ThrowsAsync<TaskCanceledException>(code: Act);
        }

        [Test]
        public async Task OrElseAsync_SynchronousContinuation_ReturnsSuccessTask()
        {
            // arrange
            const string initialResult = "a";
            const string continuationResult = "b";

            Result<string, string> Continuation(string s)
            {
                return Result<string, string>.FromErr(err: continuationResult);
            }

            var resultTask = Task.FromResult(Result<string, string>.FromErr(err: initialResult))
                .OrElseAsync((Func<string, Result<string, string>>)Continuation);

            // act
            await resultTask;

            // assert
            Assert.AreEqual(expected: TaskStatus.RanToCompletion, actual: resultTask.Status);
        }
    }
}
