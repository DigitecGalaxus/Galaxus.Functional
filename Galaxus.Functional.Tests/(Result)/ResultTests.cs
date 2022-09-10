using System;
using NUnit.Framework;

namespace Galaxus.Functional.Tests;

public class ResultTests
{
    [Test]
    public void Result_From_OkAndErrAreDifferentTypes()
    {
        {
            var ok = Result<int, string>.FromOk(666);
            Assert.IsTrue(ok.IsOk);
            Assert.IsTrue(ok.Ok.IsSome);
            Assert.AreEqual(666, ok.Unwrap());
        }

        {
            var err = Result<int, string>.FromErr("err");
            Assert.IsTrue(err.IsErr);
            Assert.IsTrue(err.Err.IsSome);
            Assert.AreEqual("err", err.Err.Unwrap());
        }
    }

    [Test]
    public void Result_From_OkAndErrAreSameTypes()
    {
        {
            var ok = Result<string, string>.FromOk("ok");
            Assert.IsTrue(ok.IsOk);
            Assert.IsTrue(ok.Ok.IsSome);
            Assert.AreEqual("ok", ok.Unwrap());
        }

        {
            var err = Result<string, string>.FromErr("err");
            Assert.IsTrue(err.IsErr);
            Assert.IsTrue(err.Err.IsSome);
            Assert.AreEqual("err", err.Err.Unwrap());
        }
    }

    [Test]
    public void Result_ConstructFromNull()
    {
        {
            var ok = Result<int?, string>.FromOk(null);
            Assert.IsTrue(ok.IsOk);
        }

        {
            var ok = Result<string, int?>.FromErr(null);
            Assert.IsTrue(ok.IsErr);
        }

        Assert.Throws<ArgumentNullException>(() => Result<string, int?>.FromOk(null));
        Assert.Throws<ArgumentNullException>(() => Result<int?, string>.FromErr(null));
    }

    [Test]
    public void Result_MatchOk()
    {
        var result = "hello".ToOk<string, int>();

        {
            var called = false;
            result.Match(ok =>
            {
                called = true;
                Assert.AreEqual("hello", ok);
            }, _ => Assert.Fail());
            Assert.IsTrue(called);
        }

        {
            var called = false;

            var number = result.Match(ok =>
            {
                called = true;
                Assert.AreEqual("hello", ok);
                return 666;
            }, _ =>
            {
                Assert.Fail();
                throw new InvalidOperationException();
            });

            Assert.AreEqual(666, number);
            Assert.IsTrue(called);
        }

        {
            var called = false;
            result.IfOk(ok =>
            {
                Assert.AreEqual("hello", ok);
                called = true;
            });
            Assert.IsTrue(called);
        }

        {
            var called = false;
            result.IfErr(_ => called = true);
            Assert.IsFalse(called);
        }
    }

    [Test]
    public void Result_MatchErr()
    {
        var result = Result<string, int>.FromErr(99);

        {
            var called = false;
            result.Match(_ => Assert.Fail(), err =>
            {
                Assert.AreEqual(99, err);
                called = true;
            });
            Assert.IsTrue(called);
        }

        {
            var called = false;

            var number = result.Match(_ =>
                {
                    Assert.Fail();
                    throw new InvalidOperationException();
                },
                err =>
                {
                    called = true;
                    Assert.AreEqual(99, err);
                    return 666;
                });

            Assert.AreEqual(666, number);
            Assert.IsTrue(called);
        }

        {
            var called = false;
            result.IfErr(err =>
            {
                Assert.AreEqual(99, err);
                called = true;
            });
            Assert.IsTrue(called);
        }

        {
            var called = false;
            result.IfOk(_ => called = true);
            Assert.IsFalse(called);
        }
    }

    [Test]
    public void ResultMatchWithNullMatchPatternThrows()
    {
        // OK
        Assert.Throws<ArgumentNullException>(() => { 0.ToOk<int, string>().Match(null, _ => { }); });
        Assert.Throws<ArgumentNullException>(() => { 0.ToOk<int, string>().IfOk(null); });

        // ERR
        Assert.Throws<ArgumentNullException>(() => { "hello".ToErr<int, string>().Match(_ => { }, null); });
        Assert.Throws<ArgumentNullException>(() => { "hello".ToErr<int, string>().IfErr(null); });

        // OK and ERR with return value
        Assert.Throws<ArgumentNullException>(() => { _ = 0.ToOk<int, string>().Match(null, _ => 0); });
        Assert.Throws<ArgumentNullException>(() => { _ = "hello".ToErr<int, string>().Match(v => v, null); });
    }

    [Test]
    public void Result_Or()
    {
        {
            var ok = Result<int, string>.FromOk(2);
            var err = Result<int, string>.FromErr("late error");

            Assert.AreEqual(2.ToOk<int, string>(), ok.Or(err));
        }
        {
            var err = Result<int, string>.FromErr("early error");
            var ok = Result<int, string>.FromOk(2);

            Assert.AreEqual(2.ToOk<int, string>(), err.Or(ok));
        }
        {
            var err1 = Result<int, string>.FromErr("not a 2");
            var err2 = Result<int, string>.FromErr("late error");

            Assert.AreEqual("late error".ToErr<int, string>(), err1.Or(err2));
        }
        {
            var ok1 = Result<int, string>.FromOk(2);
            var ok2 = Result<int, string>.FromOk(100);

            Assert.AreEqual(2.ToOk<int, string>(), ok1.Or(ok2));
        }
    }

    [Test]
    public void Result_OrElse()
    {
        Result<int, int> Square(int i)
        {
            return Result<int, int>.FromOk(i * i);
        }

        Result<int, int> Error(int i)
        {
            return Result<int, int>.FromErr(i);
        }

        Assert.AreEqual(Result<int, int>.FromOk(2),
            Result<int, int>.FromOk(2).OrElse(Square).OrElse(Square));
        Assert.AreEqual(Result<int, int>.FromOk(2),
            Result<int, int>.FromOk(2).OrElse(Error).OrElse(Square));
        Assert.AreEqual(Result<int, int>.FromOk(9),
            Result<int, int>.FromErr(3).OrElse(Square).OrElse(Error));
        Assert.AreEqual(Result<int, int>.FromErr(3),
            Result<int, int>.FromErr(3).OrElse(Error).OrElse(Error));
    }

    [Test]
    public void Result_UnwrapOr()
    {
        var ok = "hello".ToOk<string, int>();
        var err = 99.ToErr<string, int>();

        Assert.AreEqual("hello", ok.UnwrapOr("world"));
        Assert.AreEqual("world", err.UnwrapOr("world"));
    }

    [Test]
    public void Result_UnwrapOrWithCallback()
    {
        var ok = "hello".ToOk<string, int>();
        var err = 99.ToErr<string, int>();

        {
            var invoked = false;

            Assert.AreEqual("hello", ok.UnwrapOrElse(() =>
            {
                invoked = true;
                return "world";
            }));

            Assert.IsFalse(invoked);
        }

        {
            var invoked = false;

            Assert.AreEqual("world", err.UnwrapOrElse(() =>
            {
                invoked = true;
                return "world";
            }));

            Assert.IsTrue(invoked);
        }
    }

    [Test]
    public void Result_Unwrap()
    {
        var ok = "hello".ToOk<string, int>();
        var err = 99.ToErr<string, int>();

        Assert.AreEqual("hello", ok.Unwrap());
        Assert.Throws<TriedToUnwrapErrException>(() => err.Unwrap());
    }

    [Test]
    public void Result_UnwrapWithCustomError()
    {
        var ok = "hello".ToOk<string, int>();
        var err = 99.ToErr<string, int>();

        Assert.AreEqual("hello", ok.Unwrap("YOLO"));
        Assert.Throws<TriedToUnwrapErrException>(() =>
        {
            try
            {
                err.Unwrap("YOLO");
            }
            catch (TriedToUnwrapErrException ex)
            {
                Assert.AreEqual("YOLO", ex.Message);
                throw;
            }
        });
    }

    [Test]
    public void Result_UnwrapWithCustomInvokableError()
    {
        {
            var ok = "hello".ToOk<string, int>();
            var invoked = false;
            Assert.AreEqual("hello", ok.Unwrap(_ =>
            {
                invoked = true;
                return "YOLO";
            }));
            Assert.IsFalse(invoked);
        }

        {
            var err = 0.ToErr<string, int>();
            var invoked = false;
            Assert.Throws<TriedToUnwrapErrException>(() =>
            {
                try
                {
                    err.Unwrap(_ =>
                    {
                        invoked = true;
                        return "YOLO";
                    });
                }
                catch (TriedToUnwrapErrException ex)
                {
                    Assert.AreEqual("YOLO", ex.Message);
                    throw;
                }
            });

            Assert.IsTrue(invoked);
        }
    }

    [Test]
    public void Result_EqualsSameTypeOkVariant()
    {
        var ok1 = "hello".ToOk<string, string>();
        var ok2 = "hello".ToOk<string, string>();
        Assert.AreEqual(ok1.GetHashCode(), ok2.GetHashCode());
        Assert.AreEqual(ok1, ok2);
    }

    [Test]
    public void Result_EqualsSameTypeErrVariant()
    {
        var err1 = "hello".ToErr<string, string>();
        var err2 = "hello".ToErr<string, string>();
        Assert.AreEqual(err1.GetHashCode(), err2.GetHashCode());
        Assert.AreEqual(err1, err2);
    }

    [Test]
    public void Result_EqualsSameTypeDifferentVariants()
    {
        var ok = "hello".ToOk<string, string>();
        var err = "hello".ToErr<string, string>();
        Assert.AreNotEqual(ok, err);
    }

    [Test]
    public void Result_EqualsSameTypeOkVariantDifferentValue()
    {
        var ok1 = "hello".ToOk<string, string>();
        var ok2 = "world".ToOk<string, string>();
        Assert.AreNotEqual(ok1, ok2);
    }

    [Test]
    public void Result_EqualsSameTypeErrVariantDifferentValue()
    {
        var err1 = "hello".ToErr<string, string>();
        var err2 = "world".ToErr<string, string>();
        Assert.AreNotEqual(err1, err2);
    }

    [Test]
    public void Result_EqualsDifferentType()
    {
        var ok1 = "hello".ToOk<string, string>();
        var ok2 = "hello".ToOk<string, int>();
        Assert.AreNotEqual(ok1, ok2);
    }

    [Test]
    public void Result_And()
    {
        {
            var ok = Result<int, string>.FromOk(2);
            var err = Result<int, string>.FromErr("late error");

            Assert.AreEqual(Result<int, string>.FromErr("late error"), ok.And(err));
        }

        {
            var err = Result<int, string>.FromErr("early error");
            var ok = Result<string, string>.FromOk("hello");

            Assert.AreEqual(Result<string, string>.FromErr("early error"), err.And(ok));
        }

        {
            var err1 = Result<int, string>.FromErr("not a 2");
            var err2 = Result<string, string>.FromErr("late error");

            Assert.AreEqual(Result<string, string>.FromErr("not a 2"), err1.And(err2));
        }

        {
            var ok1 = Result<int, string>.FromOk(2);
            var ok2 = Result<string, string>.FromOk("different result type");

            Assert.AreEqual(Result<string, string>.FromOk("different result type"), ok1.And(ok2));
        }
    }

    [Test]
    public void Result_AndThen()
    {
        Result<int, int> Square(int i)
        {
            return Result<int, int>.FromOk(i * i);
        }

        Result<int, int> Error(int i)
        {
            return Result<int, int>.FromErr(i);
        }

        Assert.AreEqual(Result<int, int>.FromOk(16),
            Result<int, int>.FromOk(2).AndThen(Square).AndThen(Square));
        Assert.AreEqual(Result<int, int>.FromErr(4),
            Result<int, int>.FromOk(2).AndThen(Square).AndThen(Error));
        Assert.AreEqual(Result<int, int>.FromErr(2),
            Result<int, int>.FromOk(2).AndThen(Error).AndThen(Square));
        Assert.AreEqual(Result<int, int>.FromErr(3),
            Result<int, int>.FromErr(3).AndThen(Square).AndThen(Square));
    }

    [Test]
    public void Result_AndThenWithAction()
    {
        Result<int, int> Square(int i)
        {
            return Result<int, int>.FromOk(i * i);
        }

        Result<int, int> Error(int i)
        {
            return Result<int, int>.FromErr(i);
        }

        {
            var invoked = false;
            Assert.AreEqual(Result<int, int>.FromOk(9),
                Result<int, int>.FromOk(3).AndThen(_ => invoked = true).AndThen(Square));
            Assert.IsTrue(invoked);
        }
        {
            var invoked = false;
            Assert.AreEqual(Result<int, int>.FromErr(3),
                Result<int, int>.FromOk(3).AndThen(_ => invoked = true).AndThen(Error));
            Assert.IsTrue(invoked);
        }
        {
            var invoked = false;
            Assert.AreEqual(Result<int, int>.FromErr(3),
                Result<int, int>.FromOk(3).AndThen(Error).AndThen(_ => invoked = true));
            Assert.IsFalse(invoked);
        }
        {
            var invoked = false;
            Assert.AreEqual(Result<int, int>.FromErr(5),
                Result<int, int>.FromErr(5).AndThen(Square).AndThen(_ => invoked = true));
            Assert.IsFalse(invoked);
        }
    }

    [Test]
    public void Result_AndThenAction_ForwardsCurrentValue()
    {
        var result = Result<string, int>.FromOk("Success");
        var nextResult = result.AndThen(Console.WriteLine);

        Assert.AreSame(nextResult, result);
    }

    [Test]
    public void Result_AndThenFunc_IsOk()
    {
        var success = Result<string, int>.FromOk("Success");

        Result<Unit, int> Nop(string s)
        {
            return Unit.Value;
        }

        var nextResult = success.AndThen(Nop);
        Assert.IsTrue(nextResult.IsOk);
        Assert.AreEqual(Unit.Value, nextResult.Ok.Unwrap());
    }


    [Test]
    public void Result_AndThenFunc_IsErr()
    {
        var success = Result<string, int>.FromErr(5);

        Result<Unit, int> Nop(Result<string, int> x)
        {
            return x.Match(_ => Result<Unit, int>.FromOk(Unit.Value), err => err);
        }

        var nextResult = success.AndThen(s => Nop(s));
        Assert.IsTrue(nextResult.IsErr);
        Assert.AreEqual(5, nextResult.Err.Unwrap());
    }

    [Test]
    public void Result_MapOk_NullFunc()
    {
        var ok = Result<string, string>.FromOk("Success");
        Assert.Throws<ArgumentNullException>(() => ok.Map<int>(null));
    }

    [Test]
    public void Result_MapOk_IsOk()
    {
        var ok = Result<string, string>.FromOk("Success");
        var mappingResult = ok.Map(s => s.Length);
        Assert.IsTrue(mappingResult.IsOk);
        Assert.AreEqual("Success".Length, mappingResult.Ok.Unwrap());
    }

    [Test]
    public void Result_MapOk_IsErrStaysTheSame()
    {
        var ok = Result<string, string>.FromErr("Failure");
        var mappingResult = ok.Map(s => s.Length);
        Assert.IsTrue(mappingResult.IsErr);
        Assert.AreEqual("Failure", mappingResult.Err.Unwrap());
    }

    [Test]
    public void Result_MapErr_NullFunc()
    {
        var ok = Result<string, string>.FromErr("Failure");
        Assert.Throws<ArgumentNullException>(() => ok.MapErr<int>(null));
    }

    [Test]
    public void Result_MapErr_IsErr()
    {
        var ok = Result<string, string>.FromErr("Failure");
        var mappingResult = ok.MapErr(s => s.Length);
        Assert.IsTrue(mappingResult.IsErr);
        Assert.AreEqual("Failure".Length, mappingResult.Err.Unwrap());
    }

    [Test]
    public void Result_MapErr_IsOkStaysTheSame()
    {
        var ok = Result<string, string>.FromOk("Success");
        var mappingResult = ok.MapErr(s => s.Length);
        Assert.IsTrue(mappingResult.IsOk);
        Assert.AreEqual("Success", mappingResult.Ok.Unwrap());
    }
}
