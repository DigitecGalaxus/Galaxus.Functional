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
                Func<string, Task<Result<string, string>>> continuation = s =>
                    Task.FromResult(Result<string, string>.FromOk(continuationResult));
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
                Func<string, Task<Result<string, string>>> continuation = s =>
                    Task.FromResult(Result<string, string>.FromOk(continuationResult));
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
                Func<string, Task<Result<string, string>>> continuation = s =>
                    Task.FromResult(Result<string, string>.FromOk(continuationResult));
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
                Func<string, Task<Result<string, string>>> continuation = s =>
                    Task.FromResult(Result<string, string>.FromOk(continuationResult));
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


        [Test]
        public async Task UnwrapAsync_WhenTOk_ThenOkReturned()
        {
            var ok = Task.FromResult("hello".ToOk<string, int>());
            Assert.AreEqual("hello", await ok.UnwrapAsync());
        }

        [Test]
        public void UnwrapAsync_WhenTErr_ThenExceptionThrown()
        {
            var err = Task.FromResult(99.ToErr<string, int>());
            Assert.ThrowsAsync<AttemptToUnwrapErrWhenResultWasOkException>(() => err.UnwrapAsync());
        }

        [Test]
        public async Task UnwrapAsync_WhenSynchronousContinuation_ThenReturnsSuccessTask()
        {
            var okTask = Task.FromResult("Ford Prefect".ToOk<string, int>());
            await okTask.UnwrapAsync();
            Assert.AreEqual(expected: TaskStatus.RanToCompletion, actual: okTask.Status);
        }

        [Test]
        public void UnwrapAsync_WhenPreviousTaskThrewArgumentException_ThenArgumentExceptionIsThrown()
        {
            Assert.ThrowsAsync<ArgumentException>(async () =>
            {
                await Task.FromException<Result<int, string>>(new ArgumentException())
                    .UnwrapAsync();
            });
        }

        [Test]
        public async Task UnwrapAsyncWithErrorMessage_WhenTOk_ThenOkReturned()
        {
            var ok = Task.FromResult("hello".ToOk<string, int>());
            Assert.AreEqual("hello", await ok.UnwrapAsync("world"));
        }

        [Test]
        public void UnwrapAsyncWithErrorMessage_WhenResultIsErr_ThenExceptionWithCorrectMessageIsThrown()
        {
            var err = Task.FromResult(99.ToErr<string, int>());
            Assert.ThrowsAsync<AttemptToUnwrapErrWhenResultWasOkException>(async () =>
            {
                try
                {
                    await err.UnwrapAsync("YOLO");
                }
                catch (AttemptToUnwrapErrWhenResultWasOkException ex)
                {
                    Assert.AreEqual("YOLO", ex.Message);
                    throw;
                }
            });
        }

        [Test]
        public async Task UnwrapAsyncWithErrorMessage_WhenSynchronousContinuation_ThenReturnsSuccessTask()
        {
            var okTask = Task.FromResult("Ford Prefect".ToOk<string, int>());
            await okTask.UnwrapAsync("Arthur Dent");
            Assert.AreEqual(expected: TaskStatus.RanToCompletion, actual: okTask.Status);
        }

        [Test]
        public void UnwrapAsyncWithErrorMessage_WhenPreviousTaskThrewArgumentException_ThenArgumentExceptionIsThrown()
        {
            Assert.ThrowsAsync<ArgumentException>(async () =>
            {
                await Task.FromException<Result<int, string>>(new ArgumentException())
                    .UnwrapAsync("Arthur Dent");
            });
        }

        [Test]
        public async Task UnwrapAsyncWithErrorFunction_WhenResultIsOk_ThenFunctionNotInvoked()
        {
            var ok = Task.FromResult("hello".ToOk<string, int>());
            var invoked = false;
            Assert.AreEqual("hello", await ok.UnwrapAsync(err =>
            {
                invoked = true;
                return "YOLO";
            }));
            Assert.IsFalse(invoked);
        }

        [Test]
        public void UnwrapAsyncWithErrorFunction_WhenResultIsErr_ThenFunctionInvoked()
        {
            var err = Task.FromResult(0.ToErr<string, int>());
            var invoked = false;
            Assert.ThrowsAsync<AttemptToUnwrapErrWhenResultWasOkException>(async () =>
            {
                try
                {
                    await err.UnwrapAsync(e =>
                    {
                        invoked = true;
                        return "YOLO";
                    });
                }
                catch (AttemptToUnwrapErrWhenResultWasOkException ex)
                {
                    Assert.AreEqual("YOLO", ex.Message);
                    throw;
                }
            });

            Assert.IsTrue(invoked);
        }

        [Test]
        public async Task UnwrapAsyncWithErrorFunction_WhenSynchronousContinuation_ThenReturnsSuccessTask()
        {
            var okTask = Task.FromResult("Ford Prefect".ToOk<string, int>());
            await okTask.UnwrapAsync(err => "Arthur Dent");
            Assert.AreEqual(expected: TaskStatus.RanToCompletion, actual: okTask.Status);
        }

        [Test]
        public void UnwrapAsyncWithErrorFunction_WhenPreviousTaskThrewArgumentException_ThenArgumentExceptionIsThrown()
        {
            Assert.ThrowsAsync<ArgumentException>(async () =>
            {
                await Task.FromException<Result<int, string>>(new ArgumentException())
                    .UnwrapAsync(err => "Arthur Dent");
            });
        }

        [Test]
        public async Task UnwrapAsyncWithErrorFunctionTask_WhenResultIsOk_ThenFunctionNotInvoked()
        {
            var ok = Task.FromResult("hello".ToOk<string, int>());
            var invoked = false;
            Assert.AreEqual("hello", await ok.UnwrapAsync(async err =>
            {
                invoked = true;
                return await Task.FromResult("YOLO");
            }));
            Assert.IsFalse(invoked);
        }

        [Test]
        public void UnwrapAsyncWithErrorFunctionTask_WhenResultIsErr_ThenFunctionInvoked()
        {
            var err = Task.FromResult(0.ToErr<string, int>());
            var invoked = false;
            Assert.ThrowsAsync<AttemptToUnwrapErrWhenResultWasOkException>(async () =>
            {
                try
                {
                    await err.UnwrapAsync(async e =>
                    {
                        invoked = true;
                        return await Task.FromResult("YOLO");
                    });
                }
                catch (AttemptToUnwrapErrWhenResultWasOkException ex)
                {
                    Assert.AreEqual("YOLO", ex.Message);
                    throw;
                }
            });

            Assert.IsTrue(invoked);
        }

        [Test]
        public async Task UnwrapAsyncWithErrorFunctionTask_WhenSynchronousContinuation_ThenReturnsSuccessTask()
        {
            var okTask = Task.FromResult("Ford Prefect".ToOk<string, int>());
            await okTask.UnwrapAsync(async err => await Task.FromResult("Arthur Dent"));
            Assert.AreEqual(expected: TaskStatus.RanToCompletion, actual: okTask.Status);
        }

        [Test]
        public void
            UnwrapAsyncWithErrorFunctionTask_WhenPreviousTaskThrewArgumentException_ThenArgumentExceptionIsThrown()
        {
            Assert.ThrowsAsync<ArgumentException>(async () =>
            {
                await Task.FromException<Result<int, string>>(new ArgumentException())
                    .UnwrapAsync(async err => await Task.FromResult("Arthur Dent"));
            });
        }

        [Test]
        public async Task UnwrapOrAsync_WhenOk_ThenResultIsOkValue()
        {
            var ok = Task.FromResult("hello".ToOk<string, int>());
            Assert.AreEqual("hello", await ok.UnwrapOrAsync("world"));
        }

        [Test]
        public async Task UnwrapOrAsync_WhenErr_ThenResultIsParameterValue()
        {
            var err = Task.FromResult(33.ToErr<string, int>());
            Assert.AreEqual("world", await err.UnwrapOrAsync("world"));
        }

        [Test]
        public async Task UnwrapOrAsync_WhenSynchronousContinuation_ThenReturnsSuccessTask()
        {
            var okTask = Task.FromResult("Ford Prefect".ToOk<string, int>());
            await okTask.UnwrapOrAsync("Arthur Dent");
            Assert.AreEqual(expected: TaskStatus.RanToCompletion, actual: okTask.Status);
        }

        [Test]
        public void UnwrapOrAsync_WhenPreviousTaskThrewArgumentException_ThenArgumentExceptionIsThrown()
        {
            Assert.ThrowsAsync<ArgumentException>(async () =>
            {
                await Task.FromException<Result<string, int>>(new ArgumentException())
                    .UnwrapOrAsync("Arthur Dent");
            });
        }

        [Test]
        public async Task UnwrapOrAsyncWithTaskParam_WhenOk_ThenResultIsOkValue()
        {
            var ok = Task.FromResult("hello".ToOk<string, int>());
            Assert.AreEqual("hello", await ok.UnwrapOrAsync(Task.FromResult("world")));
        }

        [Test]
        public async Task UnwrapOrAsyncWithTaskParam_WhenErr_ThenResultIsParameterValue()
        {
            var err = Task.FromResult(99.ToErr<string, int>());
            Assert.AreEqual("world", await err.UnwrapOrAsync(Task.FromResult("world")));
        }

        [Test]
        public async Task UnwrapOrAsyncWithTaskParam_WhenSynchronousContinuation_ThenReturnsSuccessTask()
        {
            var okTask = Task.FromResult("Ford Prefect".ToOk<string, int>());
            await okTask.UnwrapOrAsync(Task.FromResult("Arthur Dent"));
            Assert.AreEqual(expected: TaskStatus.RanToCompletion, actual: okTask.Status);
        }

        [Test]
        public void UnwrapOrAsyncWithTaskParam_WhenPreviousTaskThrewArgumentException_ThenArgumentExceptionIsThrown()
        {
            Assert.ThrowsAsync<ArgumentException>(async () =>
            {
                await Task.FromException<Result<string, int>>(new ArgumentException())
                    .UnwrapOrAsync(Task.FromResult("Arthur Dent"));
            });
        }
        
        [Test]
        public async Task UnwrapOrElseAsync_WhenOk_ThenResultIsOkValue()
        {
            var ok = Task.FromResult("hello".ToOk<string, int>());
            Assert.AreEqual("hello", await ok.UnwrapOrElseAsync(() => "world"));
        }

        [Test]
        public async Task UnwrapOrElseAsync_WhenErr_ThenResultIsParameterValue()
        {
            var err = Task.FromResult(99.ToErr<string, int>());
            Assert.AreEqual("world", await err.UnwrapOrElseAsync(() => "world"));
        }
        
        [Test]
        public async Task UnwrapOrElseAsync_WhenSynchronousContinuation_ThenReturnsSuccessTask()
        {
            var okTask = Task.FromResult("Ford Prefect".ToOk<string, int>());
            await okTask.UnwrapOrElseAsync(() => "Arthur Dent");
            Assert.AreEqual(expected: TaskStatus.RanToCompletion, actual: okTask.Status);
        }

        [Test]
        public void UnwrapOrElseAsync_WhenPreviousTaskThrewArgumentException_ThenArgumentExceptionIsThrown()
        {
            Assert.ThrowsAsync<ArgumentException>(async () =>
            {
                await Task.FromException<Result<string, int>>(new ArgumentException())
                    .UnwrapOrElseAsync(() => "Arthur Dent");
            });
        }
        
        [Test]
        public async Task UnwrapOrElseAsync_WhenUnwrapOkWithCallBack_ThenNoCallbackInvoked()
        {
            var ok = Task.FromResult("hello".ToOk<string, int>());
            var invoked = false;
            Assert.AreEqual("hello", await ok.UnwrapOrElseAsync(() =>
            {
                invoked = true;
                return "world";
            }));
            Assert.IsFalse(invoked);
        }

        [Test]
        public async Task UnwrapOrElseAsync_WhenUnwrapErrWithCallBack_ThenCallbackInvoked()
        {
            var err = Task.FromResult(77.ToErr<string, int>());
            var invoked = false;
            Assert.AreEqual("world", await err.UnwrapOrElseAsync(() =>
            {
                invoked = true;
                return "world";
            }));
            Assert.IsTrue(invoked);
        }


        [Test]
        public void UnwrapOrElse_WhenUnwrapErrWithFunctionIsNull_ThenArgumentExceptionIsThrown()
        {
            Func<string> nullFunc = null;
            Assert.ThrowsAsync<ArgumentNullException>(() =>
                Task.FromResult(88.ToErr<string, int>()).UnwrapOrElseAsync(nullFunc));
        }
        
        [Test]
        public async Task UnwrapOrElseAsyncWithTaskParam_WhenOk_ThenResultIsOkValue()
        {
            var ok = Task.FromResult("hello".ToOk<string, int>());
            Func<Task<string>> continuation = () => Task.FromResult("world");
            Assert.AreEqual("hello", await ok.UnwrapOrElseAsync(continuation));
        }
        
        [Test]
        public async Task UnwrapOrElseAsyncWithTaskParam_WhenErr_ThenResultIsParameterValue()
        {
            var err = Task.FromResult(99.ToErr<string, int>());
            Func<Task<string>> continuation = () => Task.FromResult("world");
            Assert.AreEqual("world", await err.UnwrapOrElseAsync(continuation));
        }
        
        [Test]
        public async Task UnwrapOrElseAsyncWithTaskParam_WhenSynchronousContinuation_ThenReturnsSuccessTask()
        {
            var okTask = Task.FromResult("Ford Prefect".ToOk<string, int>());
            Func<Task<string>> continuation = () => Task.FromResult("Arthur Dent");
            await okTask.UnwrapOrElseAsync(continuation);
            Assert.AreEqual(expected: TaskStatus.RanToCompletion, actual: okTask.Status);
        }

        [Test]
        public void UnwrapOrElseAsyncWithTaskParam_WhenPreviousTaskThrewArgumentException_ThenArgumentExceptionIsThrown()
        {
            Assert.ThrowsAsync<ArgumentException>(async () =>
            {
                Func<Task<string>> continuation = () => Task.FromResult("Arthur Dent");
                await Task.FromException<Result<string, int>>(new ArgumentException())
                    .UnwrapOrElseAsync(continuation);
            });
        }
        
        [Test]
        public async Task UnwrapOrElseAsyncWithTaskParam_WhenUnwrapOkWithCallBack_ThenNoCallbackInvoked()
        {
            var ok = Task.FromResult("hello".ToOk<string, int>());
            var invoked = false;
            Func<Task<string>> continuation = () => Task.Run(() =>
            {
                invoked = true;
                return "world";
            });
            Assert.AreEqual("hello", await ok.UnwrapOrElseAsync(continuation));
            Assert.IsFalse(invoked);
        }

        [Test]
        public async Task UnwrapOrElseAsyncWithTaskParam_WhenUnwrapErrWithCallBack_ThenCallbackInvoked()
        {
            var err = Task.FromResult(11.ToErr<string, int>());
            var invoked = false;
            Func<Task<string>> continuation = () => Task.Run(() =>
            {
                invoked = true;
                return "world";
            });
            Assert.AreEqual("world", await err.UnwrapOrElseAsync(continuation));
            Assert.IsTrue(invoked);
        }

        [Test]
        public void UnwrapOrElseAsyncWithTaskParam_WhenUnwrapOkWithFunctionTaskIsNull_ThenArgumentExceptionIsThrown()
        {
            Func<Task<string>> nullFunc = null;
            Assert.ThrowsAsync<ArgumentNullException>(() =>
                Task.FromResult(12.ToErr<string, int>()).UnwrapOrElseAsync(nullFunc));
        }
        
        [Test]
        public async Task UnwrapOrDefaultAsync_WhenOk_ThenResultIsOkValue()
        {
            var ok = Task.FromResult("hello".ToOk<string, int>());
            Assert.AreEqual("hello", await ok.UnwrapOrDefaultAsync());
        }

        [Test]
        public async Task UnwrapOrDefaultAsync_WhenErr_ThenResultIsDefaultValue()
        {
            var err = Task.FromResult(11.ToErr<string, int>());
            Assert.AreEqual(null, await err.UnwrapOrDefaultAsync());
        }
        
        [Test]
        public async Task UnwrapOrDefaultAsync_WhenSynchronousContinuation_ThenReturnsSuccessTask()
        {
            var ok = Task.FromResult("Ford Prefect".ToOk<string, int>());
            await ok.UnwrapOrDefaultAsync();
            Assert.AreEqual(expected: TaskStatus.RanToCompletion, actual: ok.Status);
        }
        
        [Test]
        public async Task UnwrapOrDefaultAsync_WhenSynchronousContinuationForErr_ThenReturnsSuccessTask()
        {
            var okTask = Task.FromResult(99.ToErr<string, int>());
            await okTask.UnwrapOrDefaultAsync();
            Assert.AreEqual(expected: TaskStatus.RanToCompletion, actual: okTask.Status);
        }
        
        [Test]
        public void UnwrapOrDefaultAsync_WhenPreviousTaskThrewArgumentException_ThenArgumentExceptionIsThrown()
        {
            Assert.ThrowsAsync<ArgumentException>(async () =>
            {
                await Task.FromException<Result<string, int>>(new ArgumentException())
                    .UnwrapOrDefaultAsync();
            });
        }
    }
}