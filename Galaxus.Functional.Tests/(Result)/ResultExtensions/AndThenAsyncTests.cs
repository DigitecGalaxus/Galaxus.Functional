using System;
using System.Threading;
using System.Threading.Tasks;
using NUnit.Framework;

namespace Galaxus.Functional.Tests.ResultExtensions;

public class AndThenAsyncTests
{
    [Test]
    public void AndThenAsync_PreviousTaskThrewArgumentException_ArgumentExceptionIsThrown()
    {
        // arrange
        const string continuationResult = "b";

        async Task<Result<string, string>> Continuation(string s)
        {
            return await Task.FromResult(Result<string, string>.FromOk(continuationResult));
        }

        // act
        async Task Act()
        {
            await Task.FromException<Result<string, string>>(new ArgumentException())
                .AndThenAsync(Continuation);
        }

        // assert
        Assert.ThrowsAsync<ArgumentException>(Act);
    }

    [Test]
    public void AndThenAsync_PreviousTaskWasCanceled_TaskCanceledExceptionIsThrown()
    {
        // arrange
        const string continuationResult = "b";

        async Task<Result<string, string>> Continuation(string s)
        {
            return await Task.FromResult(Result<string, string>.FromOk(continuationResult));
        }

        var cancellationTokenSource = new CancellationTokenSource();

        // act
        cancellationTokenSource.Cancel();

        async Task Act()
        {
            await Task.FromCanceled<Result<string, string>>(cancellationTokenSource.Token)
                .AndThenAsync(Continuation);
        }

        // assert
        Assert.ThrowsAsync<TaskCanceledException>(Act);
    }

    [Test]
    public async Task AndThenAsync_SynchronousContinuation_ReturnsSuccessTask()
    {
        // arrange
        const string initialResult = "a";
        const string continuationResult = "b";

        Result<string, string> Continuation(string s)
        {
            return Result<string, string>.FromOk(continuationResult);
        }

        var resultTask = Task.FromResult(Result<string, string>.FromOk(initialResult))
            .AndThenAsync(Continuation);

        // act
        await resultTask;

        // assert
        Assert.AreEqual(TaskStatus.RanToCompletion, resultTask.Status);
    }
}
