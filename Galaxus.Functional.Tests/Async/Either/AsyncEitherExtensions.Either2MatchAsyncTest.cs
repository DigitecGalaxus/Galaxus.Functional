using System.Threading.Tasks;
using Galaxus.Functional.Async;
using NUnit.Framework;

namespace Galaxus.Functional.Tests.Async.Either;

[TestFixture]
internal class Either2MatchAsyncTest
{
    public class BothContinuationsAreAsync : Either2MatchAsyncTest
    {
        [Test]
        public async Task AppliesOnA_WhenSelfIsA()
        {
            var value = await CreateA("a").MatchAsync(async x => AppendA(x), async x => AppendB(x));
            Assert.AreEqual("aA", value);
        }

        [Test]
        public async Task AppliesOnB_WhenSelfIsB()
        {
            var value = await CreateB("b").MatchAsync(async x => AppendA(x), async x => AppendB(x));
            Assert.AreEqual("bB", value);
        }
    }

    public class OnAIsAsync : Either2MatchAsyncTest
    {
        [Test]
        public async Task AppliesOnA_WhenSelfIsA()
        {
            var value = await CreateA("a").MatchAsync(async x => AppendA(x), AppendB);
            Assert.AreEqual("aA", value);
        }

        [Test]
        public async Task AppliesOnB_WhenSelfIsB()
        {
            var value = await CreateB("b").MatchAsync(async x => AppendA(x), AppendB);
            Assert.AreEqual("bB", value);
        }
    }

    public class OnBIsAsync : Either2MatchAsyncTest
    {
        [Test]
        public async Task AppliesOnA_WhenSelfIsA()
        {
            var value = await CreateA("a").MatchAsync(AppendA, async x => AppendB(x));
            Assert.AreEqual("aA", value);
        }

        [Test]
        public async Task AppliesOnB_WhenSelfIsB()
        {
            var value = await CreateB("b").MatchAsync(AppendA, async x => AppendB(x));
            Assert.AreEqual("bB", value);
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

    private static Either<string, string> CreateA(string value)
    {
        return new Either<string, string>(a: value);
    }

    private static Either<string, string> CreateB(string value)
    {
        return new Either<string, string>(b: value);
    }
}
