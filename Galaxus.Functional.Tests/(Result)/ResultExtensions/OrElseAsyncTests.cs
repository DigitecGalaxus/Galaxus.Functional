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
            async Task<Result<string, string>> Continuation(string s) => await Task.FromResult(Result<string, string>.FromOk(continuationResult));

            // act
            async Task Act() =>
                await Task.FromException<Result<string, string>>(new ArgumentException())
                    .OrElseAsync((Func<string, Task<Result<string, string>>>) Continuation);

            // assert
            Assert.ThrowsAsync<ArgumentException>(Act);
        }

        [Test]
        public void OrElseAsync_PreviousTaskWasCanceled_TaskCanceledExceptionIsThrown()
        {
            // arrange
            const string continuationResult = "b";
            async Task<Result<string, string>> Continuation(string s) => await Task.FromResult(Result<string, string>.FromOk(continuationResult));
            var cancellationTokenSource = new CancellationTokenSource();
            
            // act
            cancellationTokenSource.Cancel();
            async Task Act() =>
                await Task.FromCanceled<Result<string, string>>(cancellationTokenSource.Token)
                    .OrElseAsync((Func<string, Task<Result<string, string>>>) Continuation);

            // assert
            Assert.ThrowsAsync<TaskCanceledException>(Act);
        }

        [Test]
        public async Task OrElseAsync_SynchronousContinuation_ReturnsSuccessTask()
        {
            // arrange
            const string initialResult = "a";
            const string continuationResult = "b";
            Result<string, string> Continuation(string s) => Result<string, string>.FromErr(continuationResult);
            var resultTask = Task.FromResult(Result<string, string>.FromErr(initialResult))
                .OrElseAsync((Func<string, Result<string, string>>) Continuation);
            
            // act
            await resultTask;
            
            // assert
            Assert.AreEqual(expected: TaskStatus.RanToCompletion, actual: resultTask.Status);
        }
    }
}