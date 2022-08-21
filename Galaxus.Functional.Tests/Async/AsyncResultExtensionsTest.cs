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
            Assert.That(LoadValue(), Is.EqualTo(ok.Unwrap()));
        }

        [Test]
        public async Task ContinuationIsNotExecuted_WhenResultIsErr()
        {
            var err = Result<string, string>.FromErr("err");
            await err.IfOkAsync(StoreValue);
            Assert.That(LoadValue(), Is.EqualTo(string.Empty));
        }

        private Task StoreValue(string value)
        {
            _storedValue = value;

            return Task.CompletedTask;
        }

        private string LoadValue()
        {
            return _storedValue;
        }
    }
}
