using System;
using System.Threading.Tasks;
using NUnit.Framework;

namespace Galaxus.Functional.Tests.OptionExtensions
{
    public class UnwrapAsyncTests
    {
        [Test]
        public async Task Option_UnwrapAsync()
        {
            var some = Task.FromResult("hello".ToOption());
            var none = Task.FromResult(Option<string>.None);

            Assert.AreEqual("hello", await some.UnwrapAsync());
            Assert.ThrowsAsync<AttemptToUnwrapNoneWhenOptionContainedSomeException>(() => none.UnwrapAsync());
        }
        
        [Test]
        public void Option_UnwrapNullFunc()
        {
            var none = Task.FromResult(Option<string>.None);
            Assert.ThrowsAsync<ArgumentNullException>(() => none.UnwrapAsync((Func<string>) null));
        }

        [Test]
        public async Task Option_UnwrapWithCustomError()
        {
            var some = Task.FromResult("hello".ToOption());
            var none = Task.FromResult(Option<string>.None);

            Assert.AreEqual("hello", await some.UnwrapAsync("YOLO"));
            Assert.ThrowsAsync<AttemptToUnwrapNoneWhenOptionContainedSomeException>(async () => {
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
        public async Task Option_UnwrapWithCustomInvokableError()
        {
            {
                var some = Task.FromResult("hello".ToOption());
                bool invoked = false;
                Assert.AreEqual("hello", await some.UnwrapAsync(() => { invoked = true; return "YOLO"; }));
                Assert.IsFalse(invoked);
            }

            {
                var none = Task.FromResult(Option<string>.None);
                bool invoked = false;
                Assert.ThrowsAsync<AttemptToUnwrapNoneWhenOptionContainedSomeException>(async () =>
                {
                    try
                    {
                        await none.UnwrapAsync(() => { invoked = true; return "YOLO"; });
                    }
                    catch (AttemptToUnwrapNoneWhenOptionContainedSomeException ex)
                    {
                        Assert.AreEqual("YOLO", ex.Message);
                        throw;
                    }
                });

                Assert.IsTrue(invoked);
            }
        }
    }
}