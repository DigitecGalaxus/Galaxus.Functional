using System.Threading.Tasks;
using Galaxus.Functional.Async;
using NUnit.Framework;

namespace Galaxus.Functional.Tests.Async;

[TestFixture]
internal class AsyncResultExtensionsTest
{
    public sealed class IfOkAsyncTest : AsyncResultExtensionsTest
    {
        [Test]
        public async Task ContinuationIsExecuted_WhenResultIsOk()
        {
            var storedValue = string.Empty;

            Task StoreValue(string value)
            {
                storedValue = value;
                return Task.CompletedTask;
            }

            var ok = Result<string, string>.FromOk("ok");
            await ok.IfOkAsync(StoreValue);
            Assert.That(storedValue, Is.EqualTo("ok"));
        }

        [Test]
        public async Task ContinuationIsNotExecuted_WhenResultIsErr()
        {
            var storedValue = string.Empty;

            Task StoreValue(string value)
            {
                storedValue = value;
                return Task.CompletedTask;
            }

            var err = Result<string, string>.FromErr("err");
            await err.IfOkAsync(StoreValue);
            Assert.That(storedValue, Is.EqualTo(string.Empty));
        }
    }

    public sealed class IfErrAsyncTest : AsyncResultExtensionsTest
    {
        [Test]
        public async Task ContinuationIsExecuted_WhenResultIsErr()
        {
            var storedValue = string.Empty;

            Task StoreValue(string value)
            {
                storedValue = value;
                return Task.CompletedTask;
            }

            var err = Result<string, string>.FromErr("err");
            await err.IfErrAsync(StoreValue);
            Assert.That(storedValue, Is.EqualTo("err"));
        }

        [Test]
        public async Task ContinuationIsNotExecuted_WhenResultIsOk()
        {
            var storedValue = string.Empty;

            Task StoreValue(string value)
            {
                storedValue = value;
                return Task.CompletedTask;
            }

            var ok = Result<string, string>.FromOk("ok");
            await ok.IfErrAsync(StoreValue);
            Assert.That(storedValue, Is.EqualTo(string.Empty));
        }
    }
}
