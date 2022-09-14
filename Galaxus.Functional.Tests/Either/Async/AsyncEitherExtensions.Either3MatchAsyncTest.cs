using System.Threading.Tasks;
using NUnit.Framework;

namespace Galaxus.Functional.Tests.Either.Async;

[TestFixture]
internal class Either3MatchAsyncTest
{
    public sealed class AllContinuationsAreAsync : Either3MatchAsyncTest
    {
        [Test]
        public async Task AppliesOnA_WhenSelfIsA()
        {
            var result = await CreateA("a").MatchAsync(async x => AppendA(x), async x => AppendB(x), async x => AppendC(x));
            Assert.AreEqual("aA", result);
        }

        [Test]
        public async Task AppliesOnB_WhenSelfIsB()
        {
            var result = await CreateB("b").MatchAsync(async x => AppendA(x), async x => AppendB(x), async x => AppendC(x));
            Assert.AreEqual("bB", result);
        }

        [Test]
        public async Task AppliesOnC_WhenSelfIsC()
        {
            var result = await CreateC("c").MatchAsync(async x => AppendA(x), async x => AppendB(x), async x => AppendC(x));
            Assert.AreEqual("cC", result);
        }
    }

    public sealed class OnAAndOnBAreAsync : Either3MatchAsyncTest
    {
        [Test]
        public async Task AppliesOnA_WhenSelfIsA()
        {
            var result = await CreateA("a").MatchAsync(async x => AppendA(x), async x => AppendB(x), AppendC);
            Assert.AreEqual("aA", result);
        }

        [Test]
        public async Task AppliesOnB_WhenSelfIsB()
        {
            var result = await CreateB("b").MatchAsync(async x => AppendA(x), async x => AppendB(x), AppendC);
            Assert.AreEqual("bB", result);
        }

        [Test]
        public async Task AppliesOnC_WhenSelfIsC()
        {
            var result = await CreateC("c").MatchAsync(async x => AppendA(x), async x => AppendB(x), AppendC);
            Assert.AreEqual("cC", result);
        }
    }

    public sealed class OnAAndOnCAreAsync : Either3MatchAsyncTest
    {
        [Test]
        public async Task AppliesOnA_WhenSelfIsA()
        {
            var result = await CreateA("a").MatchAsync(async x => AppendA(x), AppendB, async x => AppendC(x));
            Assert.AreEqual("aA", result);
        }

        [Test]
        public async Task AppliesOnB_WhenSelfIsB()
        {
            var result = await CreateB("b").MatchAsync(async x => AppendA(x), AppendB, async x => AppendC(x));
            Assert.AreEqual("bB", result);
        }

        [Test]
        public async Task AppliesOnC_WhenSelfIsC()
        {
            var result = await CreateC("c").MatchAsync(async x => AppendA(x), AppendB, async x => AppendC(x));
            Assert.AreEqual("cC", result);
        }
    }

    public sealed class OnBAndOnCAreAsync : Either3MatchAsyncTest
    {
        [Test]
        public async Task AppliesOnA_WhenSelfIsA()
        {
            var result = await CreateA("a").MatchAsync(AppendA, async x => AppendB(x), async x => AppendC(x));
            Assert.AreEqual("aA", result);
        }

        [Test]
        public async Task AppliesOnB_WhenSelfIsB()
        {
            var result = await CreateB("b").MatchAsync(AppendA, async x => AppendB(x), async x => AppendC(x));
            Assert.AreEqual("bB", result);
        }

        [Test]
        public async Task AppliesOnC_WhenSelfIsC()
        {
            var result = await CreateC("c").MatchAsync(AppendA, async x => AppendB(x), async x => AppendC(x));
            Assert.AreEqual("cC", result);
        }
    }

    public sealed class OnAIsAsync : Either3MatchAsyncTest
    {
        [Test]
        public async Task AppliesOnA_WhenSelfIsA()
        {
            var result = await CreateA("a").MatchAsync(async x => AppendA(x), AppendB, AppendC);
            Assert.AreEqual("aA", result);
        }

        [Test]
        public async Task AppliesOnB_WhenSelfIsB()
        {
            var result = await CreateB("b").MatchAsync(async x => AppendA(x), AppendB, AppendC);
            Assert.AreEqual("bB", result);
        }

        [Test]
        public async Task AppliesOnC_WhenSelfIsC()
        {
            var result = await CreateC("c").MatchAsync(async x => AppendA(x), AppendB, AppendC);
            Assert.AreEqual("cC", result);
        }
    }

    public sealed class OnBIsAsync : Either3MatchAsyncTest
    {
        [Test]
        public async Task AppliesOnA_WhenSelfIsA()
        {
            var result = await CreateA("a").MatchAsync(AppendA, async x => AppendB(x), AppendC);
            Assert.AreEqual("aA", result);
        }

        [Test]
        public async Task AppliesOnB_WhenSelfIsB()
        {
            var result = await CreateB("b").MatchAsync(AppendA, async x => AppendB(x), AppendC);
            Assert.AreEqual("bB", result);
        }

        [Test]
        public async Task AppliesOnC_WhenSelfIsC()
        {
            var result = await CreateC("c").MatchAsync(AppendA, async x => AppendB(x), AppendC);
            Assert.AreEqual("cC", result);
        }
    }

    public sealed class OnCIsAsync : Either3MatchAsyncTest
    {
        [Test]
        public async Task AppliesOnA_WhenSelfIsA()
        {
            var result = await CreateA("a").MatchAsync(AppendA, AppendB, async x => AppendC(x));
            Assert.AreEqual("aA", result);
        }

        [Test]
        public async Task AppliesOnB_WhenSelfIsB()
        {
            var result = await CreateB("b").MatchAsync(AppendA, AppendB, async x => AppendC(x));
            Assert.AreEqual("bB", result);
        }

        [Test]
        public async Task AppliesOnC_WhenSelfIsC()
        {
            var result = await CreateC("c").MatchAsync(AppendA, AppendB, async x => AppendC(x));
            Assert.AreEqual("cC", result);
        }
    }

    private static string AppendA(string value)
    {
        return value + "A";
    }

    private static string AppendB(string value)
    {
        return value + "B";
    }

    private static string AppendC(string value)
    {
        return value + "C";
    }

    private static Either<string, string, string> CreateA(string value)
    {
        return new Either<string, string, string>(a: value);
    }

    private static Either<string, string, string> CreateB(string value)
    {
        return new Either<string, string, string>(b: value);
    }

    private static Either<string, string, string> CreateC(string value)
    {
        return new Either<string, string, string>(c: value);
    }
}
