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
                await Task.CompletedTask;
            });
            Assert.IsTrue(called);
        }

        {
            var called = false;
            await some.MatchAsync(_ =>
            {
                called = true;
            }, async () => { await Task.CompletedTask; });
            Assert.IsTrue(called);
        }

        {
            var called = false;
            await some.MatchAsync(async _ =>
                {
                    called = true;
                    await Task.CompletedTask;
                },
                () => { });
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
                () => Task.FromResult(-1));

            Assert.AreEqual(42, number);
            Assert.IsTrue(called);
        }

        {
            var called = false;
            var number = await some.MatchAsync(_ =>
                {
                    called = true;
                    return 42;
                },
                () => Task.FromResult(-1));

            Assert.AreEqual(42, number);
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
                () => -1);

            Assert.AreEqual(42, number);
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
            var number = await none.MatchAsync(async _ => Task.FromResult(-1),
                async () =>
                {
                    called = true;
                    await Task.CompletedTask;
                    return 42;
                });

            Assert.AreEqual(42, number);
            Assert.IsTrue(called);
        }
    }

    [Test]
    public void Option_MatchAsyncThrowsIfMatchArmIsNull()
    {
        Assert.ThrowsAsync<ArgumentNullException>(async () => { await OptionFactory.CreateSome(0).MatchAsync(null, async () => { await Task.CompletedTask; }); });

        Assert.ThrowsAsync<ArgumentNullException>(async () => { await Option<int>.None.MatchAsync(async _ => { await Task.CompletedTask; }, null); });

        Assert.ThrowsAsync<ArgumentNullException>(async () => {_ = await OptionFactory.CreateSome(0).MatchAsync((Func<int, Task<int>>)null, () => Task.FromResult(0)); });
        Assert.ThrowsAsync<ArgumentNullException>(async () => { _ = await Option<int>.None.MatchAsync(Task.FromResult, (Func<Task<int>>)null); });
    }

}
