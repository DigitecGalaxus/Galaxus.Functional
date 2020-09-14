using System;
using System.Threading.Tasks;
using NUnit.Framework;

namespace Galaxus.Functional.Tests.OptionExtensions
{
    public class UnwrapAsyncTests
    {
        [Test]
        public async Task UnwrapAsync_WhenOptionIsSome_ThenCorrectValueReturned()
        {
            var some = Task.FromResult("hello".ToOption());
            Assert.AreEqual("hello", await some.UnwrapAsync());
        }
        
        [Test]
        public void UnwrapAsync_WhenOptionIsNone_ThenExceptionIsThrown()
        {
            var none = Task.FromResult(Option<string>.None);
            Assert.ThrowsAsync<AttemptToUnwrapNoneWhenOptionContainedSomeException>(() => none.UnwrapAsync());
        }
        
        [Test]
        public async Task UnwrapAsync_SynchronousContinuation_ReturnsSuccessTask()
        {
            var someTask = Task.FromResult("Ford Prefect".ToOption());
            await someTask.UnwrapAsync();
            Assert.AreEqual(expected: TaskStatus.RanToCompletion, actual: someTask.Status);
        }

        [Test]
        public void UnwrapAsync_PreviousTaskThrewArgumentException_ArgumentExceptionIsThrown()
        {
            Assert.ThrowsAsync<ArgumentException>(async () =>
            {
                await Task.FromException<Option<string>>(new ArgumentException())
                    .UnwrapAsync();
            });
        }
        
        [Test]
        public async Task UnwrapAsyncWithErrorMessage_WhenOptionIsSome_ThenCorrectValueReturned()
        {
            var some = Task.FromResult("hello".ToOption());
            Assert.AreEqual("hello", await some.UnwrapAsync("World"));
        }
        
        [Test]
        public void UnwrapAsyncWithErrorMessage_WhenOptionIsNone_ThenExceptionIsThrown()
        {
            var none = Task.FromResult(Option<string>.None);
            Assert.ThrowsAsync<AttemptToUnwrapNoneWhenOptionContainedSomeException>(() => none.UnwrapAsync("World"));
        }

        [Test]
        public async Task UnwrapAsyncWithErrorMessage_WhenSynchronousContinuation_ThenReturnsSuccessTask()
        {
            var someTask = Task.FromResult("Ford Prefect".ToOption());
            await someTask.UnwrapAsync("Arthur Dent");
            Assert.AreEqual(expected: TaskStatus.RanToCompletion, actual: someTask.Status);
        }

        [Test]
        public void UnwrapAsyncWithErrorMessage_WhenPreviousTaskThrewArgumentException_ThenArgumentExceptionIsThrown()
        {
            Assert.ThrowsAsync<ArgumentException>(async () =>
            {
                await Task.FromException<Option<string>>(new ArgumentException())
                    .UnwrapAsync("Arthur Dent");
            });
        }

        [Test]
        public void UnwrapAsync_WhenErrorFunctionIsNull_ThenArgumentException()
        {
            var none = Task.FromResult(Option<string>.None);
            Assert.ThrowsAsync<ArgumentNullException>(() => none.UnwrapAsync((Func<string>) null));
        }

        [Test]
        public void UnwrapAsync_WhenUnwrapOnNoneWithCustomError_ThenExceptionMessageIsCustomErrorMessage()
        {
            var none = Task.FromResult(Option<string>.None);
            Assert.ThrowsAsync<AttemptToUnwrapNoneWhenOptionContainedSomeException>(async () =>
            {
                try
                {
                    await none.UnwrapAsync("YOLO");
                }
                catch (AttemptToUnwrapNoneWhenOptionContainedSomeException ex)
                {
                    Assert.AreEqual("YOLO", ex.Message);
                    throw;
                }
            });
        }
        
        [Test]
        public async Task UnwrapAsyncWithErrorFunction_WhenSynchronousContinuation_ThenReturnsSuccessTask()
        {
            var someTask = Task.FromResult("Ford Prefect".ToOption());
            await someTask.UnwrapAsync(() => "Arthur Dent");
            Assert.AreEqual(expected: TaskStatus.RanToCompletion, actual: someTask.Status);
        }

        [Test]
        public void UnwrapAsyncWithErrorFunction_WhenPreviousTaskThrewArgumentException_ThenArgumentExceptionIsThrown()
        {
            Assert.ThrowsAsync<ArgumentException>(async () =>
            {
                await Task.FromException<Option<string>>(new ArgumentException())
                    .UnwrapAsync(() => "Arthur Dent");
            });
        }
        
        [Test]
        public async Task UnwrapAsyncWhitErrorFunction_WhenUnwrapWithCustomInvokableErrorOnSome_ThenNothingIsInvoked()
        {
            var some = Task.FromResult("hello".ToOption());
            bool invoked = false;
            Assert.AreEqual("hello", await some.UnwrapAsync(() =>
            {
                invoked = true;
                return "YOLO";
            }));
            Assert.IsFalse(invoked);
        }

        [Test]
        public void UnwrapAsyncWithErrorFunction_WhenUnwrapWithCustomInvokableErrorOnNone_ThenFunctionIsInvoked()
        {
            var none = Task.FromResult(Option<string>.None);
            bool invoked = false;
            Assert.ThrowsAsync<AttemptToUnwrapNoneWhenOptionContainedSomeException>(async () =>
            {
                try
                {
                    await none.UnwrapAsync(() =>
                    {
                        invoked = true;
                        return "YOLO";
                    });
                }
                catch (AttemptToUnwrapNoneWhenOptionContainedSomeException ex)
                {
                    Assert.AreEqual("YOLO", ex.Message);
                    throw;
                }
            });

            Assert.IsTrue(invoked);
        }

        [Test]
        public async Task UnwrapOrAsync_WhenSome_ThenResultIsSomeValue()
        {
            var some = Task.FromResult("hello".ToOption());
            Assert.AreEqual("hello", await some.UnwrapOrAsync("world"));
        }

        [Test]
        public async Task UnwrapOrAsync_WhenNone_ThenResultIsParameterValue()
        {
            var none = Task.FromResult(Option<string>.None);
            Assert.AreEqual("world", await none.UnwrapOrAsync("world"));
        }
        
        [Test]
        public async Task UnwrapOrAsync_WhenSynchronousContinuation_ThenReturnsSuccessTask()
        {
            var someTask = Task.FromResult("Ford Prefect".ToOption());
            await someTask.UnwrapOrAsync("Arthur Dent");
            Assert.AreEqual(expected: TaskStatus.RanToCompletion, actual: someTask.Status);
        }

        [Test]
        public void UnwrapOrAsync_WhenPreviousTaskThrewArgumentException_ThenArgumentExceptionIsThrown()
        {
            Assert.ThrowsAsync<ArgumentException>(async () =>
            {
                await Task.FromException<Option<string>>(new ArgumentException())
                    .UnwrapOrAsync("Arthur Dent");
            });
        }
        
        [Test]
        public async Task UnwrapOrAsyncWithTaskParam_WhenSome_ThenResultIsSomeValue()
        {
            var some = Task.FromResult("hello".ToOption());
            Assert.AreEqual("hello", await some.UnwrapOrAsync(Task.FromResult("world")));
        }

        [Test]
        public async Task UnwrapOrAsyncWithTaskParam_WhenNone_ThenResultIsParameterValue()
        {
            var none = Task.FromResult(Option<string>.None);
            Assert.AreEqual("world", await none.UnwrapOrAsync(Task.FromResult("world")));
        }
        
        [Test]
        public async Task UnwrapOrAsyncWithTaskParam_WhenSynchronousContinuation_ThenReturnsSuccessTask()
        {
            var someTask = Task.FromResult("Ford Prefect".ToOption());
            await someTask.UnwrapOrAsync(Task.FromResult("Arthur Dent"));
            Assert.AreEqual(expected: TaskStatus.RanToCompletion, actual: someTask.Status);
        }

        [Test]
        public void UnwrapOrAsyncWithTaskParam_WhenPreviousTaskThrewArgumentException_ThenArgumentExceptionIsThrown()
        {
            Assert.ThrowsAsync<ArgumentException>(async () =>
            {
                await Task.FromException<Option<string>>(new ArgumentException())
                    .UnwrapOrAsync(Task.FromResult("Arthur Dent"));
            });
        }

        [Test]
        public async Task UnwrapOrElseAsync_WhenSome_ThenResultIsSomeValue()
        {
            var some = Task.FromResult("hello".ToOption());
            Assert.AreEqual("hello", await some.UnwrapOrElseAsync(() => "world"));
        }

        [Test]
        public async Task UnwrapOrElseAsync_WhenNone_ThenResultIsParameterValue()
        {
            var none = Task.FromResult(Option<string>.None);
            Assert.AreEqual("world", await none.UnwrapOrElseAsync(() => "world"));
        }
        
        [Test]
        public async Task UnwrapOrElseAsync_WhenSynchronousContinuation_ThenReturnsSuccessTask()
        {
            var someTask = Task.FromResult("Ford Prefect".ToOption());
            await someTask.UnwrapOrElseAsync(() => "Arthur Dent");
            Assert.AreEqual(expected: TaskStatus.RanToCompletion, actual: someTask.Status);
        }

        [Test]
        public void UnwrapOrElseAsync_WhenPreviousTaskThrewArgumentException_ThenArgumentExceptionIsThrown()
        {
            Assert.ThrowsAsync<ArgumentException>(async () =>
            {
                await Task.FromException<Option<string>>(new ArgumentException())
                    .UnwrapOrElseAsync(() => "Arthur Dent");
            });
        }
        
        [Test]
        public async Task UnwrapOrElseAsync_WhenUnwrapSomeWithCallBack_ThenNoCallbackInvoked()
        {
            var some = Task.FromResult(Option<string>.Some("hello"));
            bool invoked = false;
            Assert.AreEqual("hello", await some.UnwrapOrElseAsync(() =>
            {
                invoked = true;
                return "world";
            }));
            Assert.IsFalse(invoked);
        }

        [Test]
        public async Task UnwrapOrElseAsync_WhenUnwrapNoneWithCallBack_ThenCallbackInvoked()
        {
            var none = Task.FromResult(Option<string>.None);
            bool invoked = false;
            Assert.AreEqual("world", await none.UnwrapOrElseAsync(() =>
            {
                invoked = true;
                return "world";
            }));
            Assert.IsTrue(invoked);
        }


        [Test]
        public void UnwrapOrElse_WhenUnwrapNoneWithFunctionIsNull_ThenArgumentExceptionIsThrown()
        {
            Func<string> nullFunc = null;
            Assert.ThrowsAsync<ArgumentNullException>(() =>
                Task.FromResult(Option<string>.None).UnwrapOrElseAsync(nullFunc));
        }
        
        [Test]
        public async Task UnwrapOrElseAsyncWithTaskParam_WhenSome_ThenResultIsSomeValue()
        {
            var some = Task.FromResult("hello".ToOption());
            Func<Task<string>> continuation = () => Task.FromResult("world");
            Assert.AreEqual("hello", await some.UnwrapOrElseAsync(continuation));
        }

        [Test]
        public async Task UnwrapOrElseAsyncWithTaskParam_WhenNone_ThenResultIsParameterValue()
        {
            var none = Task.FromResult(Option<string>.None);
            Func<Task<string>> continuation = () => Task.FromResult("world");
            Assert.AreEqual("world", await none.UnwrapOrElseAsync(continuation));
        }
        
        [Test]
        public async Task UnwrapOrElseAsyncWithTaskParam_WhenSynchronousContinuation_ThenReturnsSuccessTask()
        {
            var someTask = Task.FromResult("Ford Prefect".ToOption());
            Func<Task<string>> continuation = () => Task.FromResult("Arthur Dent");
            await someTask.UnwrapOrElseAsync(continuation);
            Assert.AreEqual(expected: TaskStatus.RanToCompletion, actual: someTask.Status);
        }

        [Test]
        public void UnwrapOrElseAsyncWithTaskParam_WhenPreviousTaskThrewArgumentException_ThenArgumentExceptionIsThrown()
        {
            Assert.ThrowsAsync<ArgumentException>(async () =>
            {
                Func<Task<string>> continuation = () => Task.FromResult("Arthur Dent");
                await Task.FromException<Option<string>>(new ArgumentException())
                    .UnwrapOrElseAsync(continuation);
            });
        }
        
        [Test]
        public async Task UnwrapOrElseAsyncWithTaskParam_WhenUnwrapSomeWithCallBack_ThenNoCallbackInvoked()
        {
            var some = Task.FromResult(Option<string>.Some("hello"));
            bool invoked = false;
            Func<Task<string>> continuation = () => Task.Run(() =>
            {
                invoked = true;
                return "world";
            });
            Assert.AreEqual("hello", await some.UnwrapOrElseAsync(continuation));
            Assert.IsFalse(invoked);
        }

        [Test]
        public async Task UnwrapOrElseAsyncWithTaskParam_WhenUnwrapNoneWithCallBack_ThenCallbackInvoked()
        {
            var none = Task.FromResult(Option<string>.None);
            bool invoked = false;
            Func<Task<string>> continuation = () => Task.Run(() =>
            {
                invoked = true;
                return "world";
            });
            Assert.AreEqual("world", await none.UnwrapOrElseAsync(continuation));
            Assert.IsTrue(invoked);
        }

        [Test]
        public void UnwrapOrElse_WhenUnwrapNoneWithFunctionTaskIsNull_ThenArgumentExceptionIsThrown()
        {
            Func<Task<string>> nullFunc = null;
            Assert.ThrowsAsync<ArgumentNullException>(() =>
                Task.FromResult(Option<string>.None).UnwrapOrElseAsync(nullFunc));
        }
    }
}