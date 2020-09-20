using System;
using System.Threading.Tasks;
using NUnit.Framework;

namespace Galaxus.Functional.Tests.ResultExtensions
{
    public class UnwrapAsyncTests
    {
        [Test]
        public async Task UnwrapAsync_WhenTOk_ThenOkReturned()
        {
            // arrange
            var ok = Task.FromResult("hello".ToOk<string, int>());
            
            // act
            var hello = await ok.UnwrapAsync();
            
            // assert
            Assert.AreEqual("hello", hello);
        }

        [Test]
        public void UnwrapAsync_WhenTErr_ThenExceptionThrown()
        {
            // arrange
            var err = Task.FromResult(99.ToErr<string, int>());
            
            // act
            async Task Act() => await err.UnwrapAsync();

            // assert
            Assert.ThrowsAsync<AttemptToUnwrapErrWhenResultWasOkException>(Act);
        }

        [Test]
        public async Task UnwrapAsync_WhenSynchronousContinuation_ThenReturnsSuccessTask()
        {
            // arrange
            var okTask = Task.FromResult("Ford Prefect".ToOk<string, int>());
            
            // act
            await okTask.UnwrapAsync();
            
            // assert
            Assert.AreEqual(expected: TaskStatus.RanToCompletion, actual: okTask.Status);
        }

        [Test]
        public void UnwrapAsync_WhenPreviousTaskThrewArgumentException_ThenArgumentExceptionIsThrown()
        {
            // arrange
            var failingTask = Task.FromException<Result<int, string>>(new ArgumentException());

            // act
            async Task Act() => await failingTask.UnwrapAsync();

            // assert
            Assert.ThrowsAsync<ArgumentException>(Act);
        }

        [Test]
        public async Task UnwrapAsyncWithErrorMessage_WhenTOk_ThenOkReturned()
        {
            // arrange
            var ok = Task.FromResult("hello".ToOk<string, int>());
            
            // act
            var hello = await ok.UnwrapAsync("world");
            
            // assert
            Assert.AreEqual("hello", hello);
        }

        [Test]
        public void UnwrapAsyncWithErrorMessage_WhenResultIsErr_ThenExceptionWithCorrectMessageIsThrown()
        {
            // arrange
            var err = Task.FromResult(99.ToErr<string, int>());
            
            // act
            async Task Act()
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
            }

            // assert
            Assert.ThrowsAsync<AttemptToUnwrapErrWhenResultWasOkException>(Act);
        }

        [Test]
        public async Task UnwrapAsyncWithErrorMessage_WhenSynchronousContinuation_ThenReturnsSuccessTask()
        {
            // arrange
            var okTask = Task.FromResult("Ford Prefect".ToOk<string, int>());
            
            // act
            await okTask.UnwrapAsync("Arthur Dent");
            
            // assert
            Assert.AreEqual(expected: TaskStatus.RanToCompletion, actual: okTask.Status);
        }

        [Test]
        public void UnwrapAsyncWithErrorMessage_WhenPreviousTaskThrewArgumentException_ThenArgumentExceptionIsThrown()
        {
            // arrange
            var failingTask = Task.FromException<Result<int, string>>(new ArgumentException());
            
            // act
            async Task Act() => await failingTask.UnwrapAsync("Arthur Dent");

            // assert
            Assert.ThrowsAsync<ArgumentException>(Act);
        }

        [Test]
        public async Task UnwrapAsyncWithErrorFunction_WhenResultIsOk_ThenFunctionNotInvoked()
        {
            // arrange
            var ok = Task.FromResult("hello".ToOk<string, int>());
            var invoked = false;
            
            // act
            var hello = await ok.UnwrapAsync(err =>
            {
                invoked = true;
                return "YOLO";
            });
            
            // assert
            Assert.AreEqual("hello", hello);
            Assert.IsFalse(invoked);
        }

        [Test]
        public void UnwrapAsyncWithErrorFunction_WhenResultIsErr_ThenFunctionInvoked()
        {
            // arrange
            var err = Task.FromResult(0.ToErr<string, int>());
            var invoked = false;
            
            // act
            async Task Act()
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
            }

            // assert
            Assert.ThrowsAsync<AttemptToUnwrapErrWhenResultWasOkException>(Act);
            Assert.IsTrue(invoked);
        }

        [Test]
        public async Task UnwrapAsyncWithErrorFunction_WhenSynchronousContinuation_ThenReturnsSuccessTask()
        {
            // arrange
            var okTask = Task.FromResult("Ford Prefect".ToOk<string, int>());
            
            // act
            await okTask.UnwrapAsync(err => "Arthur Dent");
            
            // assert
            Assert.AreEqual(expected: TaskStatus.RanToCompletion, actual: okTask.Status);
        }

        [Test]
        public void UnwrapAsyncWithErrorFunction_WhenPreviousTaskThrewArgumentException_ThenArgumentExceptionIsThrown()
        {
            // arrange
            var failingTask = Task.FromException<Result<int, string>>(new ArgumentException());

            // act
            async Task Act() => await failingTask.UnwrapAsync(err => "Arthur Dent");
            
            // arrange
            Assert.ThrowsAsync<ArgumentException>(Act);
        }

        [Test]
        public async Task UnwrapAsyncWithErrorFunctionTask_WhenResultIsOk_ThenFunctionNotInvoked()
        {
            // arrange
            var ok = Task.FromResult("hello".ToOk<string, int>());
            var invoked = false;
            
            // act
            var hello = await ok.UnwrapAsync(async err =>
            {
                invoked = true;
                return await Task.FromResult("YOLO");
            });
            
            // assert
            Assert.AreEqual("hello", hello);
            Assert.IsFalse(invoked);
        }

        [Test]
        public void UnwrapAsyncWithErrorFunctionTask_WhenResultIsErr_ThenFunctionInvoked()
        {
            // arrange
            var err = Task.FromResult(0.ToErr<string, int>());
            var invoked = false;
            
            // act
            async Task Act()
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
            }

            // arrange
            Assert.ThrowsAsync<AttemptToUnwrapErrWhenResultWasOkException>(Act);
            Assert.IsTrue(invoked);
        }

        [Test]
        public async Task UnwrapAsyncWithErrorFunctionTask_WhenSynchronousContinuation_ThenReturnsSuccessTask()
        {
            // arrange
            var okTask = Task.FromResult("Ford Prefect".ToOk<string, int>());
            
            // act
            await okTask.UnwrapAsync(async err => await Task.FromResult("Arthur Dent"));
            
            // assert
            Assert.AreEqual(expected: TaskStatus.RanToCompletion, actual: okTask.Status);
        }

        [Test]
        public void UnwrapAsyncWithErrorFunctionTask_WhenPreviousTaskThrewArgumentException_ThenArgumentExceptionIsThrown()
        {
            // arrange
            var failingTask = Task.FromException<Result<int, string>>(new ArgumentException());

            // act
            async Task Act() => await failingTask.UnwrapAsync(async err => await Task.FromResult("Arthur Dent"));

            // assert
            Assert.ThrowsAsync<ArgumentException>(Act);
        }

        [Test]
        public async Task UnwrapOrAsync_WhenOk_ThenResultIsOkValue()
        {
            // arrange
            var ok = Task.FromResult("hello".ToOk<string, int>());
            
            //act
            var hello = await ok.UnwrapOrAsync("world");
            
            // assert
            Assert.AreEqual("hello", hello);
        }

        [Test]
        public async Task UnwrapOrAsync_WhenErr_ThenResultIsParameterValue()
        {
            // arrange
            var err = Task.FromResult(33.ToErr<string, int>());
            
            // act
            var world = await err.UnwrapOrAsync("world");
            
            // assert
            Assert.AreEqual("world", world);
        }

        [Test]
        public async Task UnwrapOrAsync_WhenSynchronousContinuation_ThenReturnsSuccessTask()
        {
            // arrange
            var okTask = Task.FromResult("Ford Prefect".ToOk<string, int>());
            
            // act
            await okTask.UnwrapOrAsync("Arthur Dent");
            
            // assert
            Assert.AreEqual(expected: TaskStatus.RanToCompletion, actual: okTask.Status);
        }

        [Test]
        public void UnwrapOrAsync_WhenPreviousTaskThrewArgumentException_ThenArgumentExceptionIsThrown()
        {
            // arrange
            var failingTask = Task.FromException<Result<string, int>>(new ArgumentException());
            
            // act
            async Task Act() => await failingTask.UnwrapOrAsync("Arthur Dent");

            // assert
            Assert.ThrowsAsync<ArgumentException>(Act);
        }

        [Test]
        public async Task UnwrapOrAsyncWithTaskParam_WhenOk_ThenResultIsOkValue()
        {
            // arrange
            var ok = Task.FromResult("hello".ToOk<string, int>());
            
            // act
            var hello = await ok.UnwrapOrAsync(Task.FromResult("world"));
            
            // assert
            Assert.AreEqual("hello", hello);
        }

        [Test]
        public async Task UnwrapOrAsyncWithTaskParam_WhenErr_ThenResultIsParameterValue()
        {
            // arrange
            var err = Task.FromResult(99.ToErr<string, int>());
            
            // act
            var world = await err.UnwrapOrAsync(Task.FromResult("world"));
            
            // assert
            Assert.AreEqual("world", world);
        }

        [Test]
        public async Task UnwrapOrAsyncWithTaskParam_WhenSynchronousContinuation_ThenReturnsSuccessTask()
        {
            // arrange
            var okTask = Task.FromResult("Ford Prefect".ToOk<string, int>());
            
            // act
            await okTask.UnwrapOrAsync(Task.FromResult("Arthur Dent"));
            
            // assert
            Assert.AreEqual(expected: TaskStatus.RanToCompletion, actual: okTask.Status);
        }

        [Test]
        public void UnwrapOrAsyncWithTaskParam_WhenPreviousTaskThrewArgumentException_ThenArgumentExceptionIsThrown()
        {
            // arrange
            var failingTask = Task.FromException<Result<string, int>>(new ArgumentException());
            
            // act
            async Task Act() => await failingTask.UnwrapOrAsync(Task.FromResult("Arthur Dent"));

            // assert
            Assert.ThrowsAsync<ArgumentException>(Act);
        }
        
        [Test]
        public async Task UnwrapOrElseAsync_WhenOk_ThenResultIsOkValue()
        {
            // arrange
            var ok = Task.FromResult("hello".ToOk<string, int>());
            
            // act
            var hello = await ok.UnwrapOrElseAsync(() => "world");
            
            // arrange
            Assert.AreEqual("hello", hello);
        }

        [Test]
        public async Task UnwrapOrElseAsync_WhenErr_ThenResultIsParameterValue()
        {
            // arrange
            var err = Task.FromResult(99.ToErr<string, int>());
            
            // act
            var world = await err.UnwrapOrElseAsync(() => "world");
            
            // assert
            Assert.AreEqual("world", world);
        }
        
        [Test]
        public async Task UnwrapOrElseAsync_WhenSynchronousContinuation_ThenReturnsSuccessTask()
        {
            // arrange
            var okTask = Task.FromResult("Ford Prefect".ToOk<string, int>());
            
            // act
            await okTask.UnwrapOrElseAsync(() => "Arthur Dent");
            
            // assert
            Assert.AreEqual(expected: TaskStatus.RanToCompletion, actual: okTask.Status);
        }

        [Test]
        public void UnwrapOrElseAsync_WhenPreviousTaskThrewArgumentException_ThenArgumentExceptionIsThrown()
        {
            // arrange
            var failingTask = Task.FromException<Result<string, int>>(new ArgumentException());
            
            // act
            async Task Act() => await failingTask.UnwrapOrElseAsync(() => "Arthur Dent");

            // assert
            Assert.ThrowsAsync<ArgumentException>(Act);
        }
        
        [Test]
        public async Task UnwrapOrElseAsync_WhenUnwrapOkWithCallBack_ThenNoCallbackInvoked()
        {
            // arrange
            var ok = Task.FromResult("hello".ToOk<string, int>());
            var invoked = false;
            
            // act
            var hello = await ok.UnwrapOrElseAsync(() =>
            {
                invoked = true;
                return "world";
            });
            
            // assert
            Assert.AreEqual("hello", hello);
            Assert.IsFalse(invoked);
        }

        [Test]
        public async Task UnwrapOrElseAsync_WhenUnwrapErrWithCallBack_ThenCallbackInvoked()
        {
            // arrange
            var err = Task.FromResult(77.ToErr<string, int>());
            var invoked = false;
            
            // act
            var world = await err.UnwrapOrElseAsync(() =>
            {
                invoked = true;
                return "world";
            });
            
            // assert
            Assert.AreEqual("world", world);
            Assert.IsTrue(invoked);
        }


        [Test]
        public void UnwrapOrElse_WhenUnwrapErrWithFunctionIsNull_ThenArgumentExceptionIsThrown()
        {
            // arrange
            Func<string> nullFunc = null;

            // act
            // ReSharper disable once ExpressionIsAlwaysNull
            async Task Act() => await Task.FromResult(88.ToErr<string, int>()).UnwrapOrElseAsync(nullFunc);

            // assert
            Assert.ThrowsAsync<ArgumentNullException>(Act);
        }
        
        [Test]
        public async Task UnwrapOrElseAsyncWithTaskParam_WhenOk_ThenResultIsOkValue()
        {
            // arrange
            var ok = Task.FromResult("hello".ToOk<string, int>());
            static async Task<string> Continuation() => await Task.FromResult("world");
            
            // act
            var hello = await ok.UnwrapOrElseAsync((Func<Task<string>>) Continuation);
            
            // assert
            Assert.AreEqual("hello", hello);
        }
        
        [Test]
        public async Task UnwrapOrElseAsyncWithTaskParam_WhenErr_ThenResultIsParameterValue()
        {
            // arrange
            var err = Task.FromResult(99.ToErr<string, int>());
            static async Task<string> Continuation() => await Task.FromResult("world");
            
            // act
            var world = await err.UnwrapOrElseAsync((Func<Task<string>>) Continuation);
            
            // assert
            Assert.AreEqual("world", world);
        }
        
        [Test]
        public async Task UnwrapOrElseAsyncWithTaskParam_WhenSynchronousContinuation_ThenReturnsSuccessTask()
        {
            // arrange
            var okTask = Task.FromResult("Ford Prefect".ToOk<string, int>());
            static async  Task<string> Continuation() => await Task.FromResult("Arthur Dent");
            
            // act
            await okTask.UnwrapOrElseAsync((Func<Task<string>>) Continuation);
            
            // assert
            Assert.AreEqual(expected: TaskStatus.RanToCompletion, actual: okTask.Status);
        }

        [Test]
        public void UnwrapOrElseAsyncWithTaskParam_WhenPreviousTaskThrewArgumentException_ThenArgumentExceptionIsThrown()
        {
            // arrange
            var failingTask = Task.FromException<Result<string, int>>(new ArgumentException());
            static async Task<string> Continuation() => await Task.FromResult("Arthur Dent");
            
            // act
            async Task Act() => await failingTask.UnwrapOrElseAsync((Func<Task<string>>) Continuation);

            // assert
            Assert.ThrowsAsync<ArgumentException>(Act);
        }
        
        [Test]
        public async Task UnwrapOrElseAsyncWithTaskParam_WhenUnwrapOkWithCallBack_ThenNoCallbackInvoked()
        {
            // arrange
            var ok = Task.FromResult("hello".ToOk<string, int>());
            var invoked = false;
            async Task<string> Continuation() =>
                await Task.Run(() =>
                {
                    invoked = true;
                    return "world";
                });

            // act
            var hello = await ok.UnwrapOrElseAsync((Func<Task<string>>) Continuation);
            
            // assert
            Assert.AreEqual("hello", hello);
            Assert.IsFalse(invoked);
        }

        [Test]
        public async Task UnwrapOrElseAsyncWithTaskParam_WhenUnwrapErrWithCallBack_ThenCallbackInvoked()
        {
            // arrange
            var err = Task.FromResult(11.ToErr<string, int>());
            var invoked = false;
            async Task<string> Continuation() =>
                await Task.Run(() =>
                {
                    invoked = true;
                    return "world";
                });
            
            // act
            var world = await err.UnwrapOrElseAsync((Func<Task<string>>) Continuation);

            // assert
            Assert.AreEqual("world", world);
            Assert.IsTrue(invoked);
        }

        [Test]
        public void UnwrapOrElseAsyncWithTaskParam_WhenUnwrapOkWithFunctionTaskIsNull_ThenArgumentExceptionIsThrown()
        {
            // arrange
            Func<Task<string>> nullFunc = null;
            
            // act
            // ReSharper disable once ExpressionIsAlwaysNull
            Task Act() => Task.FromResult(12.ToErr<string, int>()).UnwrapOrElseAsync(nullFunc);

            // assert
            Assert.ThrowsAsync<ArgumentNullException>(Act);
        }
        
        [Test]
        public async Task UnwrapOrDefaultAsync_WhenOk_ThenResultIsOkValue()
        {
            // arrange
            var ok = Task.FromResult("hello".ToOk<string, int>());
            
            // act
            var hello = await ok.UnwrapOrDefaultAsync();
            // assert
            Assert.AreEqual("hello", hello);
        }

        [Test]
        public async Task UnwrapOrDefaultAsync_WhenErr_ThenResultIsDefaultValue()
        {
            // arrange
            var err = Task.FromResult(11.ToErr<string, int>());
            
            // act
            var defaultString = await err.UnwrapOrDefaultAsync();
            
            // assert
            Assert.AreEqual(null, defaultString);
        }
        
        [Test]
        public async Task UnwrapOrDefaultAsync_WhenSynchronousContinuation_ThenReturnsSuccessTask()
        {
            // arrange
            var ok = Task.FromResult("Ford Prefect".ToOk<string, int>());
            
            // act
            await ok.UnwrapOrDefaultAsync();
            
            // assert
            Assert.AreEqual(expected: TaskStatus.RanToCompletion, actual: ok.Status);
        }
        
        [Test]
        public async Task UnwrapOrDefaultAsync_WhenSynchronousContinuationForErr_ThenReturnsSuccessTask()
        {
            // arrange
            var okTask = Task.FromResult(99.ToErr<string, int>());
            
            // act
            await okTask.UnwrapOrDefaultAsync();
            
            // assert
            Assert.AreEqual(expected: TaskStatus.RanToCompletion, actual: okTask.Status);
        }
        
        [Test]
        public void UnwrapOrDefaultAsync_WhenPreviousTaskThrewArgumentException_ThenArgumentExceptionIsThrown()
        {
            // arrange
            var failingTask = Task.FromException<Result<string, int>>(new ArgumentException());
            
            // act
            async Task Act() => await failingTask.UnwrapOrDefaultAsync();

            // assert
            Assert.ThrowsAsync<ArgumentException>(Act);
        }
    }
}