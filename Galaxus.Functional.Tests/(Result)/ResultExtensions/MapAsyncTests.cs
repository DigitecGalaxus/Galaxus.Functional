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
            string Continuation(string s) => continuationResult;
            var resultTask = Task.FromResult(Result<string, string>.FromOk(initialResult))
                .MapAsync((Func<string, string>) Continuation);
            
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
            async Task<string> Continuation(string s) => await Task.FromResult(continuationResult);

            // act
            async Task Act() =>
                await Task.FromException<Result<string, string>>(new ArgumentException())
                    .MapAsync((Func<string, Task<string>>) Continuation);

            // assert
            Assert.ThrowsAsync<ArgumentException>(Act);
        }

        [Test]
        public void MapOkAsync_PreviousTaskWasCanceled_TaskCanceledExceptionIsThrown()
        {
            // arrange
            const string continuationResult = "b";
            async Task<string> Continuation(string s) => await Task.FromResult(continuationResult);
            var cancellationTokenSource = new CancellationTokenSource();
            
            // act
            cancellationTokenSource.Cancel();
            async Task Act() => await Task.FromCanceled<Result<string, string>>(cancellationTokenSource.Token)
                    .MapAsync((Func<string, Task<string>>) Continuation);

            // assert
            Assert.ThrowsAsync<TaskCanceledException>(Act);
        }

        [Test]
        public async Task MapErrAsync_SynchronousContinuation_ReturnsSuccessTask()
        {
            // arrange
            const string initialResult = "a";
            const string continuationResult = "b";
            string Continuation(string s) => continuationResult;
            var resultTask = Task.FromResult(Result<string, string>.FromErr(initialResult))
                .MapErrAsync((Func<string, string>) Continuation);
            
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
            async Task<string> Continuation(string s) => await Task.FromResult(continuationResult);
            
            // act
            async Task Act() =>
                await Task.FromException<Result<string, string>>(new ArgumentException())
                    .MapErrAsync((Func<string, Task<string>>) Continuation);

            // assert
            Assert.ThrowsAsync<ArgumentException>(Act);
        }

        [Test]
        public void MapErrAsync_PreviousTaskWasCanceled_TaskCanceledExceptionIsThrown()
        {
            // arrange
            const string continuationResult = "b";
            async Task<string> Continuation(string s) => await Task.FromResult(continuationResult);
            var cancellationTokenSource = new CancellationTokenSource();
            
            // act
            cancellationTokenSource.Cancel();
            async Task Act() =>
                await Task.FromCanceled<Result<string, string>>(cancellationTokenSource.Token)
                    .MapErrAsync((Func<string, Task<string>>) Continuation);

            // assert
            Assert.ThrowsAsync<TaskCanceledException>(Act);
        }
    }
}