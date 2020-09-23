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
            // arrange
            var some = Task.FromResult("hello".ToOption());

            // act
            var hello = await some.UnwrapAsync();

            // assert
            Assert.AreEqual("hello", hello);
        }

        [Test]
        public void UnwrapAsync_WhenOptionIsNone_ThenExceptionIsThrown()
        {
            // arrange
            var none = Task.FromResult(Option<string>.None);

            //act
            async Task Act() => await none.UnwrapAsync();

            // assert
            Assert.ThrowsAsync<AttemptToUnwrapNoneWhenOptionContainedSomeException>(Act);
        }

        [Test]
        public async Task UnwrapAsync_WhenSynchronousContinuation_ThenReturnsSuccessTask()
        {
            // arrange
            var someTask = Task.FromResult("Ford Prefect".ToOption());

            // act
            await someTask.UnwrapAsync();

            // assert
            Assert.AreEqual(expected: TaskStatus.RanToCompletion, actual: someTask.Status);
        }

        [Test]
        public void UnwrapAsync_WhenPreviousTaskThrewArgumentException_ThenArgumentExceptionIsThrown()
        {
            // arrange
            var failingTask = Task.FromException<Option<string>>(new ArgumentException());

            // act
            async Task Act() => await failingTask.UnwrapAsync();

            // assert
            Assert.ThrowsAsync<ArgumentException>(Act);
        }

        [Test]
        public async Task UnwrapAsyncWithErrorMessage_WhenOptionIsSome_ThenCorrectValueReturned()
        {
            // arrange
            var some = Task.FromResult("hello".ToOption());

            // act
            var hello = await some.UnwrapAsync("World");

            // assert
            Assert.AreEqual("hello", hello);
        }

        [Test]
        public void UnwrapAsyncWithErrorMessage_WhenOptionIsNone_ThenExceptionIsThrown()
        {
            // arrange
            var none = Task.FromResult(Option<string>.None);

            // act
            async Task Act() => await none.UnwrapAsync("World");

            // assert
            Assert.ThrowsAsync<AttemptToUnwrapNoneWhenOptionContainedSomeException>(Act);
        }

        [Test]
        public async Task UnwrapAsyncWithErrorMessage_WhenSynchronousContinuation_ThenReturnsSuccessTask()
        {
            // arrange
            var someTask = Task.FromResult("Ford Prefect".ToOption());

            // act
            await someTask.UnwrapAsync("Arthur Dent");

            // assert
            Assert.AreEqual(expected: TaskStatus.RanToCompletion, actual: someTask.Status);
        }

        [Test]
        public void UnwrapAsyncWithErrorMessage_WhenPreviousTaskThrewArgumentException_ThenArgumentExceptionIsThrown()
        {
            // arrange
            var failingTask = Task.FromException<Option<string>>(new ArgumentException());

            // act
            async Task Act() => await failingTask.UnwrapAsync("Arthur Dent");

            // assert
            Assert.ThrowsAsync<ArgumentException>(Act);
        }

        [Test]
        public void UnwrapAsync_WhenErrorFunctionIsNull_ThenArgumentException()
        {
            // arrange
            var none = Task.FromResult(Option<string>.None);

            // act
            async Task Act() => await none.UnwrapAsync((Func<string>) null);

            // assert
            Assert.ThrowsAsync<ArgumentNullException>(Act);
        }

        [Test]
        public void UnwrapAsync_WhenUnwrapOnNoneWithCustomError_ThenExceptionMessageIsCustomErrorMessage()
        {
            // arrange
            var none = Task.FromResult(Option<string>.None);

            // act
            async Task Act()
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
            }

            // assert
            Assert.ThrowsAsync<AttemptToUnwrapNoneWhenOptionContainedSomeException>(Act);
        }

        [Test]
        public async Task UnwrapAsyncWithErrorFunction_WhenSynchronousContinuation_ThenReturnsSuccessTask()
        {
            // arrange
            var someTask = Task.FromResult("Ford Prefect".ToOption());

            // act
            await someTask.UnwrapAsync(() => "Arthur Dent");

            // assert
            Assert.AreEqual(expected: TaskStatus.RanToCompletion, actual: someTask.Status);
        }

        [Test]
        public void UnwrapAsyncWithErrorFunction_WhenPreviousTaskThrewArgumentException_ThenArgumentExceptionIsThrown()
        {
            // arrange
            var failingTask = Task.FromException<Option<string>>(new ArgumentException());

            // act
            async Task Act() => await failingTask.UnwrapAsync(() => "Arthur Dent");

            // assert
            Assert.ThrowsAsync<ArgumentException>(Act);
        }

        [Test]
        public async Task UnwrapAsyncWhitErrorFunction_WhenUnwrapWithCustomInvokableErrorOnSome_ThenNothingIsInvoked()
        {
            // arrange
            var some = Task.FromResult("hello".ToOption());
            var invoked = false;

            // act
            var hello = await some.UnwrapAsync(() =>
            {
                invoked = true;
                return "YOLO";
            });

            // assert
            Assert.AreEqual("hello", hello);
            Assert.IsFalse(invoked);
        }

        [Test]
        public void UnwrapAsyncWithErrorFunction_WhenUnwrapWithCustomInvokableErrorOnNone_ThenFunctionIsInvoked()
        {
            // arrange
            var none = Task.FromResult(Option<string>.None);
            var invoked = false;

            // act
            async Task Act()
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
            }

            // assert
            Assert.ThrowsAsync<AttemptToUnwrapNoneWhenOptionContainedSomeException>(Act);
            Assert.IsTrue(invoked);
        }

        [Test]
        public async Task UnwrapAsyncWithErrorFunctionTask_WhenSynchronousContinuation_ThenReturnsSuccessTask()
        {
            // arrange
            var someTask = Task.FromResult("Ford Prefect".ToOption());
            
            // act
            await someTask.UnwrapAsync(async () => await Task.FromResult("Arthur Dent"));
            
            // assert
            Assert.AreEqual(expected: TaskStatus.RanToCompletion, actual: someTask.Status);
        }

        [Test]
        public void UnwrapAsyncWithErrorFunctionTask_WhenPreviousTaskThrewArgumentException_ThenArgumentExceptionIsThrown()
        {
            // assert
            var failingTask = Task.FromException<Option<string>>(new ArgumentException());
            
            // act
            async Task Act() => await failingTask.UnwrapAsync(async () => await Task.FromResult("Arthur Dent"));

            // assert
            Assert.ThrowsAsync<ArgumentException>(Act);
        }

        [Test]
        public async Task UnwrapAsyncWhitErrorFunctionTask_WhenUnwrapWithCustomInvokableErrorOnSome_ThenNothingIsInvoked()
        {
            // arrange
            var some = Task.FromResult("hello".ToOption());
            var invoked = false;

            // act
            var hello = await some.UnwrapAsync(async () =>
            {
                invoked = true;
                return await Task.FromResult("YOLO");
            });

            // assert
            Assert.AreEqual("hello", hello);
            Assert.IsFalse(invoked);
        }

        [Test]
        public void UnwrapAsyncWithErrorFunctionTask_WhenUnwrapWithCustomInvokableErrorOnNone_ThenFunctionIsInvoked()
        {
            // arrange
            var none = Task.FromResult(Option<string>.None);
            var invoked = false;

            // act
            async Task Act()
            {
                try
                {
                    await none.UnwrapAsync(async () =>
                    {
                        invoked = true;
                        return await Task.FromResult("YOLO");
                    });
                }
                catch (AttemptToUnwrapNoneWhenOptionContainedSomeException ex)
                {
                    Assert.AreEqual("YOLO", ex.Message);
                    throw;
                }
            }

            // assert
            Assert.ThrowsAsync<AttemptToUnwrapNoneWhenOptionContainedSomeException>(Act);
            Assert.IsTrue(invoked);
        }

        [Test]
        public async Task UnwrapOrAsync_WhenSome_ThenResultIsSomeValue()
        {
            // arrange
            var some = Task.FromResult("hello".ToOption());

            // act
            var hello = await some.UnwrapOrAsync("world");

            // assert
            Assert.AreEqual("hello", hello);
        }

        [Test]
        public async Task UnwrapOrAsync_WhenNone_ThenResultIsParameterValue()
        {
            // arrange
            var none = Task.FromResult(Option<string>.None);

            // act
            var world = await none.UnwrapOrAsync("world");

            // assert
            Assert.AreEqual("world", world);
        }

        [Test]
        public async Task UnwrapOrAsync_WhenSynchronousContinuation_ThenReturnsSuccessTask()
        {
            // arrange
            var someTask = Task.FromResult("Ford Prefect".ToOption());

            // act
            await someTask.UnwrapOrAsync("Arthur Dent");

            // assert
            Assert.AreEqual(expected: TaskStatus.RanToCompletion, actual: someTask.Status);
        }

        [Test]
        public void UnwrapOrAsync_WhenPreviousTaskThrewArgumentException_ThenArgumentExceptionIsThrown()
        {
            // arrange
            var failingTask = Task.FromException<Option<string>>(new ArgumentException());

            // act
            async Task Act() => await failingTask.UnwrapOrAsync("Arthur Dent");

            // assert
            Assert.ThrowsAsync<ArgumentException>(Act);
        }

        [Test]
        public async Task UnwrapOrAsyncWithTaskParam_WhenSome_ThenResultIsSomeValue()
        {
            // arrange
            var some = Task.FromResult("hello".ToOption());

            // act
            var hello = await some.UnwrapOrAsync(Task.FromResult("world"));

            // assert
            Assert.AreEqual("hello", hello);
        }

        [Test]
        public async Task UnwrapOrAsyncWithTaskParam_WhenNone_ThenResultIsParameterValue()
        {
            // arrange
            var none = Task.FromResult(Option<string>.None);

            // act
            var world = await none.UnwrapOrAsync(Task.FromResult("world"));

            // assert
            Assert.AreEqual("world", world);
        }

        [Test]
        public async Task UnwrapOrAsyncWithTaskParam_WhenSynchronousContinuation_ThenReturnsSuccessTask()
        {
            // arrange
            var someTask = Task.FromResult("Ford Prefect".ToOption());

            // act
            await someTask.UnwrapOrAsync(Task.FromResult("Arthur Dent"));

            // assert
            Assert.AreEqual(expected: TaskStatus.RanToCompletion, actual: someTask.Status);
        }

        [Test]
        public void UnwrapOrAsyncWithTaskParam_WhenPreviousTaskThrewArgumentException_ThenArgumentExceptionIsThrown()
        {
            // arrange
            var failingTask = Task.FromException<Option<string>>(new ArgumentException());

            // act
            async Task Act() => await failingTask.UnwrapOrAsync(Task.FromResult("Arthur Dent"));

            // assert
            Assert.ThrowsAsync<ArgumentException>(Act);
        }

        [Test]
        public async Task UnwrapOrElseAsync_WhenSome_ThenResultIsSomeValue()
        {
            // arrange
            var some = Task.FromResult("hello".ToOption());

            // act
            var hello = await some.UnwrapOrElseAsync(() => "world");

            // assert
            Assert.AreEqual("hello", hello);
        }

        [Test]
        public async Task UnwrapOrElseAsync_WhenNone_ThenResultIsParameterValue()
        {
            // arrange
            var none = Task.FromResult(Option<string>.None);

            // act
            var world = await none.UnwrapOrElseAsync(() => "world");

            // assert
            Assert.AreEqual("world", world);
        }

        [Test]
        public async Task UnwrapOrElseAsync_WhenSynchronousContinuation_ThenReturnsSuccessTask()
        {
            // arrange
            var someTask = Task.FromResult("Ford Prefect".ToOption());

            // act
            await someTask.UnwrapOrElseAsync(() => "Arthur Dent");

            // assert
            Assert.AreEqual(expected: TaskStatus.RanToCompletion, actual: someTask.Status);
        }

        [Test]
        public void UnwrapOrElseAsync_WhenPreviousTaskThrewArgumentException_ThenArgumentExceptionIsThrown()
        {
            // arrange
            var failingTask = Task.FromException<Option<string>>(new ArgumentException());

            // act
            async Task Act() => await failingTask.UnwrapOrElseAsync(() => "Arthur Dent");

            // assert
            Assert.ThrowsAsync<ArgumentException>(Act);
        }

        [Test]
        public async Task UnwrapOrElseAsync_WhenUnwrapSomeWithCallBack_ThenNoCallbackInvoked()
        {
            // arrange 
            var some = Task.FromResult(Option<string>.Some("hello"));
            var invoked = false;

            // act
            var hello = await some.UnwrapOrElseAsync(() =>
            {
                invoked = true;
                return "world";
            });

            // assert
            Assert.AreEqual("hello", hello);
            Assert.IsFalse(invoked);
        }

        [Test]
        public async Task UnwrapOrElseAsync_WhenUnwrapNoneWithCallBack_ThenCallbackInvoked()
        {
            // arrange
            var none = Task.FromResult(Option<string>.None);
            var invoked = false;

            // act
            var world = await none.UnwrapOrElseAsync(() =>
            {
                invoked = true;
                return "world";
            });

            // assert
            Assert.AreEqual("world", world);
            Assert.IsTrue(invoked);
        }


        [Test]
        public void UnwrapOrElseAsync_WhenUnwrapNoneWithFunctionIsNull_ThenArgumentExceptionIsThrown()
        {
            // arrange
            Func<string> nullFunc = null;

            // act
            // ReSharper disable once ExpressionIsAlwaysNull
            async Task Act() => await Task.FromResult(Option<string>.None).UnwrapOrElseAsync(nullFunc);

            // assert
            Assert.ThrowsAsync<ArgumentNullException>(Act);
        }

        [Test]
        public async Task UnwrapOrElseAsyncWithTaskParam_WhenSome_ThenResultIsSomeValue()
        {
            // arrange
            var some = Task.FromResult("hello".ToOption());
            static async Task<string> Continuation() => await Task.FromResult("world");

            // act
            var hello = await some.UnwrapOrElseAsync((Func<Task<string>>) Continuation);

            // assert
            Assert.AreEqual("hello", hello);
        }

        [Test]
        public async Task UnwrapOrElseAsyncWithTaskParam_WhenNone_ThenResultIsParameterValue()
        {
            // arrange
            var none = Task.FromResult(Option<string>.None);
            static async Task<string> Continuation() => await Task.FromResult("world");

            // act
            var world = await none.UnwrapOrElseAsync((Func<Task<string>>) Continuation);

            // assert
            Assert.AreEqual("world", world);
        }

        [Test]
        public async Task UnwrapOrElseAsyncWithTaskParam_WhenSynchronousContinuation_ThenReturnsSuccessTask()
        {
            // arrange
            var someTask = Task.FromResult("Ford Prefect".ToOption());
            static async Task<string> Continuation() => await Task.FromResult("Arthur Dent");

            // act
            await someTask.UnwrapOrElseAsync((Func<Task<string>>) Continuation);

            // assert
            Assert.AreEqual(expected: TaskStatus.RanToCompletion, actual: someTask.Status);
        }

        [Test]
        public void
            UnwrapOrElseAsyncWithTaskParam_WhenPreviousTaskThrewArgumentException_ThenArgumentExceptionIsThrown()
        {
            // arrange
            static async Task<string> Continuation() => await Task.FromResult("Arthur Dent");

            // act
            async Task Act() =>
                await Task.FromException<Option<string>>(new ArgumentException())
                    .UnwrapOrElseAsync((Func<Task<string>>) Continuation);

            // assert
            Assert.ThrowsAsync<ArgumentException>(Act);
        }

        [Test]
        public async Task UnwrapOrElseAsyncWithTaskParam_WhenUnwrapSomeWithCallBack_ThenNoCallbackInvoked()
        {
            // arrange
            var some = Task.FromResult(Option<string>.Some("hello"));
            var invoked = false;

            async Task<string> Continuation() =>
                await Task.Run(() =>
                {
                    invoked = true;
                    return "world";
                });

            // act
            var hello = await some.UnwrapOrElseAsync((Func<Task<string>>) Continuation);

            // assert
            Assert.AreEqual("hello", hello);
            Assert.IsFalse(invoked);
        }

        [Test]
        public async Task UnwrapOrElseAsyncWithTaskParam_WhenUnwrapNoneWithCallBack_ThenCallbackInvoked()
        {
            // arrange
            var none = Task.FromResult(Option<string>.None);
            var invoked = false;

            async Task<string> Continuation() =>
                await Task.Run(() =>
                {
                    invoked = true;
                    return "world";
                });

            // act
            var world = await none.UnwrapOrElseAsync((Func<Task<string>>) Continuation);

            // assert
            Assert.AreEqual("world", world);
            Assert.IsTrue(invoked);
        }

        [Test]
        public void UnwrapOrElseAsyncWithTaskParam_WhenUnwrapNoneWithFunctionTaskIsNull_ThenArgumentExceptionIsThrown()
        {
            // arrange
            Func<Task<string>> nullFunc = null;

            // act
            // ReSharper disable once ExpressionIsAlwaysNull
            async Task Act() => await Task.FromResult(Option<string>.None).UnwrapOrElseAsync(nullFunc);

            // assert
            Assert.ThrowsAsync<ArgumentNullException>(Act);
        }

        [Test]
        public async Task UnwrapOrDefaultAsync_WhenSome_ThenResultIsSomeValue()
        {
            // arrange
            var some = Task.FromResult("hello".ToOption());

            // act
            var hello = await some.UnwrapOrDefaultAsync();

            // assert
            Assert.AreEqual("hello", hello);
        }

        [Test]
        public async Task UnwrapOrDefaultAsync_WhenNone_ThenResultIsDefaultValue()
        {
            // arrange
            var none = Task.FromResult(Option<string>.None);

            // act
            var defaultString = await none.UnwrapOrDefaultAsync();

            // assert
            Assert.AreEqual(null, defaultString);
        }

        [Test]
        public async Task UnwrapOrDefaultAsync_WhenSynchronousContinuation_ThenReturnsSuccessTask()
        {
            // arrange
            var someTask = Task.FromResult("Ford Prefect".ToOption());

            // act
            await someTask.UnwrapOrDefaultAsync();

            // assert
            Assert.AreEqual(expected: TaskStatus.RanToCompletion, actual: someTask.Status);
        }

        [Test]
        public async Task UnwrapOrDefaultAsync_WhenSynchronousContinuationForNone_ThenReturnsSuccessTask()
        {
            // arrange
            var someTask = Task.FromResult(Option<string>.None);

            // act
            await someTask.UnwrapOrDefaultAsync();

            // assert
            Assert.AreEqual(expected: TaskStatus.RanToCompletion, actual: someTask.Status);
        }

        [Test]
        public void UnwrapOrDefaultAsync_WhenPreviousTaskThrewArgumentException_ThenArgumentExceptionIsThrown()
        {
            // arrange
            var failingTask = Task.FromException<Option<string>>(new ArgumentException());

            // act
            async Task Act() => await failingTask.UnwrapOrDefaultAsync();

            // assert
            Assert.ThrowsAsync<ArgumentException>(Act);
        }
    }
}