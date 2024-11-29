using System.Threading.Tasks;
using NUnit.Framework;

namespace Galaxus.Functional.Tests.Option.Async;

[TestFixture]
internal class AsyncOptionExtensions_MapTest
{
    public sealed class MapAsyncTest
    {
        [Test]
        public async Task TestMapAsync_WhenSelfIsSome()
        {
            var mapInvoked = false;

            var subject = "value".ToOption();
            var value = await subject.MapAsync(
                s => Task.Run(() =>
                {
                    mapInvoked = true;
                    return s.Length.ToOption();
                }));

            Assert.AreEqual(5.ToOption(), value);
            Assert.IsTrue(mapInvoked);
        }

        [Test]
        public async Task TestMapAsync_WhenSelfIsNone()
        {
            var mapInvoked = false;

            var subject = Option<string>.None;
            var value = await subject.MapAsync(
                s => Task.Run(() =>
                {
                    mapInvoked = true;
                    return s.Length.ToOption();
                }));

            Assert.AreEqual(Option<int>.None, value);
            Assert.IsFalse(mapInvoked);
        }
    }
}
