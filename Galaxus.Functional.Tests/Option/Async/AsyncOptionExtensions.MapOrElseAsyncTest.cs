using System.Threading.Tasks;
using NUnit.Framework;
using static Galaxus.Functional.Tests.Option.Async.OptionFactory;

namespace Galaxus.Functional.Tests.Option.Async;

[TestFixture]
internal class MapOrElseAsyncTest
{
    public sealed class AsyncOption_BothSyncArguments : MapOrElseAsyncTest
    {
        [Test]
        public async Task CallsMap_WhenSelfIsSome()
        {
            var mapInvoked = false;
            var fallbackInvoked = false;

            var value = await CreateSomeTask("value").MapOrElseAsync(
                s =>
                {
                    mapInvoked = true;
                    return s.Length.ToOption();
                },
                () =>
                {
                    fallbackInvoked = true;
                    return 2.ToOption();
                });

            Assert.AreEqual(5.ToOption(), value);
            Assert.IsTrue(mapInvoked);
            Assert.IsFalse(fallbackInvoked);
        }

        [Test]
        public async Task CallsFallback_WhenSelfIsNone()
        {
            var mapInvoked = false;
            var fallbackInvoked = false;

            Assert.AreEqual(42.ToOption(), await CreateNoneTask().MapOrElseAsync(
                s =>
                {
                    mapInvoked = true;
                    return s.Length.ToOption();
                },
                () =>
                {
                    fallbackInvoked = true;
                    return 42.ToOption();
                }));

            Assert.IsFalse(mapInvoked);
            Assert.IsTrue(fallbackInvoked);
        }
    }

    public sealed class AsyncOption_AsyncMapArguments : MapOrElseAsyncTest
    {
        [Test]
        public async Task CallsMap_WhenSelfIsSome()
        {
            var mapInvoked = false;
            var fallbackInvoked = false;

            var value = await CreateSomeTask("value").MapOrElseAsync(
                s =>
                {
                    mapInvoked = true;
                    return Task.FromResult(s.Length.ToOption());
                },
                () =>
                {
                    fallbackInvoked = true;
                    return 2.ToOption();
                });

            Assert.AreEqual(5.ToOption(), value);
            Assert.IsTrue(mapInvoked);
            Assert.IsFalse(fallbackInvoked);
        }

        [Test]
        public async Task CallsFallback_WhenSelfIsNone()
        {
            var mapInvoked = false;
            var fallbackInvoked = false;

            Assert.AreEqual(42.ToOption(), await CreateNoneTask().MapOrElseAsync(
                s =>
                {
                    mapInvoked = true;
                    return Task.FromResult(s.Length.ToOption());
                },
                () =>
                {
                    fallbackInvoked = true;
                    return 42.ToOption();
                }));

            Assert.IsFalse(mapInvoked);
            Assert.IsTrue(fallbackInvoked);
        }
    }

    public sealed class AsyncOption_AsyncFallbackArguments : MapOrElseAsyncTest
    {
        [Test]
        public async Task CallsMap_WhenSelfIsSome()
        {
            var mapInvoked = false;
            var fallbackInvoked = false;

            var value = await CreateSomeTask("value").MapOrElseAsync(
                s =>
                {
                    mapInvoked = true;
                    return s.Length.ToOption();
                },
                () =>
                {
                    fallbackInvoked = true;
                    return Task.FromResult(2.ToOption());
                });

            Assert.AreEqual(5.ToOption(), value);
            Assert.IsTrue(mapInvoked);
            Assert.IsFalse(fallbackInvoked);
        }

        [Test]
        public async Task CallsFallback_WhenSelfIsNone()
        {
            var mapInvoked = false;
            var fallbackInvoked = false;

            Assert.AreEqual(42.ToOption(), await CreateNoneTask().MapOrElseAsync(
                s =>
                {
                    mapInvoked = true;
                    return s.Length.ToOption();
                },
                () =>
                {
                    fallbackInvoked = true;
                    return Task.FromResult(42.ToOption());
                }));

            Assert.IsFalse(mapInvoked);
            Assert.IsTrue(fallbackInvoked);
        }
    }

    public sealed class AsyncOption_BothAsyncArguments : MapOrElseAsyncTest
    {
        [Test]
        public async Task CallsMap_WhenSelfIsSome()
        {
            var mapInvoked = false;
            var fallbackInvoked = false;

            var value = await CreateSomeTask("value").MapOrElseAsync(
                s =>
                {
                    mapInvoked = true;
                    return Task.FromResult(s.Length.ToOption());
                },
                () =>
                {
                    fallbackInvoked = true;
                    return Task.FromResult(2.ToOption());
                });

            Assert.AreEqual(5.ToOption(), value);
            Assert.IsTrue(mapInvoked);
            Assert.IsFalse(fallbackInvoked);
        }

        [Test]
        public async Task CallsFallback_WhenSelfIsNone()
        {
            var mapInvoked = false;
            var fallbackInvoked = false;

            Assert.AreEqual(42.ToOption(), await CreateNoneTask().MapOrElseAsync(
                s =>
                {
                    mapInvoked = true;
                    return Task.FromResult(s.Length.ToOption());
                },
                () =>
                {
                    fallbackInvoked = true;
                    return Task.FromResult(42.ToOption());
                }));

            Assert.IsFalse(mapInvoked);
            Assert.IsTrue(fallbackInvoked);
        }
    }

    public sealed class SyncOption_AsyncMapArguments : MapOrElseAsyncTest
    {
        [Test]
        public async Task CallsMap_WhenSelfIsSome()
        {
            var mapInvoked = false;
            var fallbackInvoked = false;

            var value = await CreateSome("value").MapOrElseAsync(
                s =>
                {
                    mapInvoked = true;
                    return Task.FromResult(s.Length.ToOption());
                },
                () =>
                {
                    fallbackInvoked = true;
                    return 2.ToOption();
                });

            Assert.AreEqual(5.ToOption(), value);
            Assert.IsTrue(mapInvoked);
            Assert.IsFalse(fallbackInvoked);
        }

        [Test]
        public async Task CallsFallback_WhenSelfIsNone()
        {
            var mapInvoked = false;
            var fallbackInvoked = false;

            Assert.AreEqual(42.ToOption(), await Option<string>.None.MapOrElseAsync(
                s =>
                {
                    mapInvoked = true;
                    return Task.FromResult(s.Length.ToOption());
                },
                () =>
                {
                    fallbackInvoked = true;
                    return 42.ToOption();
                }));

            Assert.IsFalse(mapInvoked);
            Assert.IsTrue(fallbackInvoked);
        }
    }
}
