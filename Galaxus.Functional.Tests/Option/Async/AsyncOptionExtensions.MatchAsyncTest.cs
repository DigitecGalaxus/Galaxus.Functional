using System;
using System.Threading.Tasks;
using NUnit.Framework;

namespace Galaxus.Functional.Tests.Option.Async;

[TestFixture]
public class MatchAsyncTest
{
  [Test]
    public async Task Option_MatchAsyncSome_Works()
    {
        var some = "hello".ToOption();

        {
            var called = false;
            await some.MatchAsync(async _ =>
            {
                called = true;
                await Task.CompletedTask;
            }, async () =>
            {
                Assert.Fail();
                await Task.CompletedTask;
            });
            Assert.IsTrue(called);
        }

        {
            var called = false;

            var number = await some.MatchAsync(async _ =>
                {
                    called = true;
                    await Task.CompletedTask;
                    return 42;
                },
                () =>
                {
                    Assert.Fail();
                    throw new InvalidOperationException();
                });

            Assert.AreEqual(42, number);
            Assert.IsTrue(called);
        }

        {
            var called = false;
            some.IfSome(_ => called = true);
            Assert.IsTrue(called);
        }
    }

    [Test]
    public async Task Option_MatchAsyncNone_Works()
    {
        var none = Option<string>.None;

        {
            var called = false;
            await none.MatchAsync(async _ =>
            {
                Assert.Fail();
                await Task.CompletedTask;
            }, async () =>
            {
                called = true;
                await Task.CompletedTask;
            });
            Assert.IsTrue(called);
        }

        {
            var called = false;

            var number = await none.MatchAsync(_ =>
                {
                    Assert.Fail();
                    throw new InvalidOperationException();
                },
                async () =>
                {
                    called = true;
                    await Task.CompletedTask;
                    return 42;
                });

            Assert.AreEqual(42, number);
            Assert.IsTrue(called);
        }

        {
            var called = false;
            none.IfSome(_ => called = true);
            Assert.IsFalse(called);
        }
    }

    [Test]
    public void Option_MatchAsyncThrowsIfMatchArmIsNull()
    {
        // SOME
        Assert.ThrowsAsync<ArgumentNullException>(async () => { await 0.ToOption().MatchAsync(null, async () => { await Task.CompletedTask; }); });

        // NONE
        Assert.ThrowsAsync<ArgumentNullException>(async () => { await Option<int>.None.MatchAsync(async _ => { await Task.CompletedTask; }, null); });

        // SOME and NONE with return value
        Assert.ThrowsAsync<ArgumentNullException>(async () => {_ = await 0.ToOption().MatchAsync(null, () => Task.FromResult(0)); });
        Assert.ThrowsAsync<ArgumentNullException>(async () => { _ = await Option<int>.None.MatchAsync(Task.FromResult, null); });
    }

}
