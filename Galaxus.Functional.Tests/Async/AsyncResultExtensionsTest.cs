using System.Threading.Tasks;
using Galaxus.Functional.Async;
using NUnit.Framework;

namespace Galaxus.Functional.Tests.Async;

[TestFixture]
internal class AsyncResultExtensionsTest
{
    public sealed class IfOkAsyncTest : AsyncResultExtensionsTest
    {
        private string _storedValue;

        [SetUp]
        public void SetUp()
        {
            _storedValue = string.Empty;
        }

        [Test]
        public async Task ContinuationIsExecuted_WhenResultIsOk()
        {
            var ok = Result<string, string>.FromOk("ok");
            await ok.IfOkAsync(StoreValue);
            Assert.That(_storedValue, Is.EqualTo("ok"));
        }

        [Test]
        public async Task ContinuationIsNotExecuted_WhenResultIsErr()
        {
            var err = Result<string, string>.FromErr("err");
            await err.IfOkAsync(StoreValue);
            Assert.That(_storedValue, Is.EqualTo(string.Empty));
        }

        private Task StoreValue(string value)
        {
            _storedValue = value;

            return Task.CompletedTask;
        }
    }

    public sealed class IfErrAsyncTest : AsyncResultExtensionsTest
    {
        private string _storedValue;

        [SetUp]
        public void SetUp()
        {
            _storedValue = string.Empty;
        }

        [Test]
        public async Task ContinuationIsExecuted_WhenResultIsErr()
        {
            var err = Result<string, string>.FromErr("err");
            await err.IfErrAsync(StoreValue);
            Assert.That(_storedValue, Is.EqualTo("err"));
        }

        [Test]
        public async Task ContinuationIsNotExecuted_WhenResultIsOk()
        {
            var ok = Result<string, string>.FromOk("ok");
            await ok.IfErrAsync(StoreValue);
            Assert.That(_storedValue, Is.EqualTo(string.Empty));
        }

        private Task StoreValue(string value)
        {
            _storedValue = value;

            return Task.CompletedTask;
        }
    }
}
