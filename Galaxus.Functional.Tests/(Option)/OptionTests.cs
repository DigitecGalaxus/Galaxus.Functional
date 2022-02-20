using System;
using NUnit.Framework;

namespace Galaxus.Functional.Tests
{
    public class OptionTests
    {
        [Test]
        public void Option_ConstructSomeFromValue()
        {
            {
                var val = Option<int>.Some(666);
                Assert.IsTrue(condition: val.IsSome);
                Assert.AreEqual(666, val.Unwrap());
            }

            {
                var val = Option<string>.Some("hello");
                Assert.IsTrue(condition: val.IsSome);
                Assert.AreEqual("hello", val.Unwrap());
            }
        }

        [Test]
        public void Option_ConstructSomeOrNoneFromNull()
        {
            {
                var val = Option<int?>.Some(null);
                Assert.IsTrue(condition: val.IsSome);
            }

            {
                var val = Option<string>.Some(null);
                Assert.IsTrue(condition: val.IsNone);
            }
        }

        [Test]
        public void Option_ConstructNoneWithDefaultConstructor()
        {
            {
                var val = new Option<int>();
                Assert.IsTrue(condition: val.IsNone);
            }

            {
                var val = new Option<string>();
                Assert.IsTrue(condition: val.IsNone);
            }
        }

        [Test]
        public void Option_StaticNoneIsNone()
        {
            Assert.IsTrue(condition: Option<int>.None.IsNone);
            Assert.IsTrue(condition: Option<string>.None.IsNone);
        }

        [Test]
        public void Option_MatchSome_Works()
        {
            var some = "hello".ToOption();

            {
                var called = false;
                some.Match(s => called = true, () => Assert.Fail());
                Assert.IsTrue(condition: called);
            }

            {
                var called = false;

                var number = some.Match(s =>
                    {
                        called = true;
                        return 666;
                    },
                    () =>
                    {
                        Assert.Fail();
                        throw new InvalidOperationException();
                    });

                Assert.AreEqual(666, actual: number);
                Assert.IsTrue(condition: called);
            }

            {
                var called = false;
                some.IfSome(s => called = true);
                Assert.IsTrue(condition: called);
            }
        }

        [Test]
        public void Option_MatchNone_Works()
        {
            var none = Option<string>.None;

            {
                var called = false;
                none.Match(s => Assert.Fail(), () => called = true);
                Assert.IsTrue(condition: called);
            }

            {
                var called = false;

                var number = none.Match(s =>
                    {
                        Assert.Fail();
                        throw new InvalidOperationException();
                    },
                    () =>
                    {
                        called = true;
                        return 666;
                    });

                Assert.AreEqual(666, actual: number);
                Assert.IsTrue(condition: called);
            }

            {
                var called = false;
                none.IfSome(s => called = true);
                Assert.IsFalse(condition: called);
            }
        }

        [Test]
        public void Option_MatchThrowsIfMatchArmIsNull()
        {
            // SOME
            Assert.Throws<ArgumentNullException>(() => { 0.ToOption().Match(null, () => { }); });
            Assert.Throws<ArgumentNullException>(() => { 0.ToOption().IfSome(null); });

            // NONE
            Assert.Throws<ArgumentNullException>(() => { Option<int>.None.Match(v => { }, null); });

            // SOME and NONE with return value
            Assert.Throws<ArgumentNullException>(() => { _ = 0.ToOption().Match(null, () => 0); });
            Assert.Throws<ArgumentNullException>(() => { _ = Option<int>.None.Match(v => v, null); });
        }

        [Test]
        public void Option_And()
        {
            {
                var some = "hello".ToOption();
                var none = Option<string>.None;

                Assert.AreEqual(expected: Option<string>.None, some.And(fallback: none));
                Assert.AreEqual(expected: Option<string>.None, none.And(fallback: some));
            }

            {
                var some1 = "hello".ToOption();
                var some2 = 123.ToOption();

                Assert.AreEqual(123.ToOption(), some1.And(fallback: some2));
            }

            {
                var none1 = Option<string>.None;
                var none2 = Option<string>.None;

                Assert.AreEqual(expected: Option<string>.None, none1.And(fallback: none2));
            }
        }

        [Test]
        public void Option_AndThen()
        {
            Option<int> Square(int i)
            {
                return Option<int>.Some(i * i);
            }

            Option<int> Nope(int i)
            {
                return Option<int>.None;
            }

            Assert.AreEqual(Option<int>.Some(16),
                Option<int>.Some(2).AndThen(continuation: Square).AndThen(continuation: Square));
            Assert.AreEqual(expected: Option<int>.None,
                Option<int>.Some(2).AndThen(continuation: Square).AndThen(continuation: Nope));
            Assert.AreEqual(expected: Option<int>.None,
                Option<int>.Some(2).AndThen(continuation: Nope).AndThen(continuation: Square));
            Assert.AreEqual(expected: Option<int>.None,
                Option<int>.None.AndThen(continuation: Square).AndThen(continuation: Square));
        }

        [Test]
        public void Option_AndThenWithAction()
        {
            {
                var some = "hello".ToOption();
                var none = Option<string>.None;

                {
                    var invoked = false;

                    Assert.AreEqual(Option<string>.Some("hello"), some.AndThen(_ => invoked = true));
                    Assert.IsTrue(condition: invoked);
                }

                {
                    var invoked = false;

                    Assert.AreEqual(expected: Option<string>.None, none.AndThen(_ => invoked = true));
                    Assert.IsFalse(condition: invoked);
                }
            }
        }

        [Test]
        public void Option_UnwrapOr()
        {
            var some = "hello".ToOption();
            var none = Option<string>.None;

            Assert.AreEqual("hello", some.UnwrapOr("world"));
            Assert.AreEqual("world", none.UnwrapOr("world"));
        }

        [Test]
        public void Option_UnwrapOrWithCallback()
        {
            var some = Option<string>.Some("hello");
            var none = Option<string>.None;

            {
                var invoked = false;

                Assert.AreEqual("hello", some.UnwrapOrElse(() =>
                {
                    invoked = true;
                    return "world";
                }));

                Assert.IsFalse(condition: invoked);
            }

            {
                var invoked = false;

                Assert.AreEqual("world", none.UnwrapOrElse(() =>
                {
                    invoked = true;
                    return "world";
                }));

                Assert.IsTrue(condition: invoked);
            }
        }


        [Test]
        public void Option_UnwrapOrWithNullFunc()
        {
            Assert.Throws<ArgumentNullException>(() => Option<string>.None.UnwrapOrElse(null));
        }

        [Test]
        public void Option_OkOr()
        {
            var some = "hello".ToOption();
            var none = Option<string>.None;
            Assert.AreEqual(Result<string, int>.FromOk("hello"), some.OkOr(0));
            Assert.AreEqual(Result<string, int>.FromErr(0), none.OkOr(0));
        }

        [Test]
        public void Option_OkOrElse()
        {
            var some = "hello".ToOption();
            var none = Option<string>.None;
            Assert.AreEqual(Result<string, int>.FromOk("hello"), some.OkOrElse(() => 0));
            Assert.AreEqual(Result<string, int>.FromErr(0), none.OkOrElse(() => 0));
        }

        [Test]
        public void Option_Or()
        {
            var some = "hello".ToOption();
            var none = Option<string>.None;

            Assert.AreEqual("hello".ToOption(), some.Or("world".ToOption()));
            Assert.AreEqual("world".ToOption(), none.Or("world".ToOption()));
        }

        [Test]
        public void Option_OrWithCallback()
        {
            var some = Option<string>.Some("hello");
            var none = Option<string>.None;

            {
                var invoked = false;

                Assert.AreEqual("hello".ToOption(), some.OrElse(() =>
                {
                    invoked = true;
                    return "world".ToOption();
                }));

                Assert.IsFalse(condition: invoked);
            }

            {
                var invoked = false;

                Assert.AreEqual("world".ToOption(), none.OrElse(() =>
                {
                    invoked = true;
                    return "world".ToOption();
                }));

                Assert.IsTrue(condition: invoked);
            }
        }

        [Test]
        public void Option_Xor()
        {
            {
                var some = 2.ToOption();
                var none = Option<int>.None;

                Assert.AreEqual(2.ToOption(), some.Xor(fallback: none));
                Assert.AreEqual(2.ToOption(), none.Xor(fallback: some));
            }

            {
                var some1 = 2.ToOption();
                var some2 = 2.ToOption();

                Assert.AreEqual(expected: Option<int>.None, some1.Xor(fallback: some2));
            }

            {
                var none1 = Option<int>.None;
                var none2 = Option<int>.None;

                Assert.AreEqual(expected: Option<int>.None, none1.Xor(fallback: none2));
            }
        }

        [Test]
        public void Option_Map()
        {
            var some = "hello".ToOption();

            Assert.AreEqual(5.ToOption(), some.Map(s => s.Length));
        }

        [Test]
        public void Option_MapOr()
        {
            var some = "hello".ToOption();
            var none = Option<string>.None;

            Assert.AreEqual(5.ToOption(), some.MapOr(s => s.Length.ToOption(), 2.ToOption()));
            Assert.AreEqual(42.ToOption(), none.MapOr(s => s.Length.ToOption(), 42.ToOption()));
        }

        [Test]
        public void Option_ToObject()
        {
            var some = (IOption)"hello".ToOption();
            var none = (IOption)Option<string>.None;

            Assert.AreEqual("hello", some.ToObject());
            Assert.IsNull(none.ToObject());
        }

        [Test]
        public void Option_Unwrap()
        {
            var some = "hello".ToOption();
            var none = Option<string>.None;

            Assert.AreEqual("hello", some.Unwrap());
            Assert.Throws<AttemptToUnwrapNoneWhenOptionContainedSomeException>(() => none.Unwrap());
        }

        [Test]
        public void Option_UnwrapNullFunc()
        {
            Assert.Throws<ArgumentNullException>(() => Option<string>.None.Unwrap((Func<string>)null));
        }

        [Test]
        public void Option_UnwrapWithCustomError()
        {
            var some = "hello".ToOption();
            var none = Option<string>.None;

            Assert.AreEqual("hello", some.Unwrap("YOLO"));
            Assert.Throws<AttemptToUnwrapNoneWhenOptionContainedSomeException>(() =>
            {
                try
                {
                    none.Unwrap("YOLO");
                }
                catch (AttemptToUnwrapNoneWhenOptionContainedSomeException ex)
                {
                    Assert.AreEqual("YOLO", actual: ex.Message);
                    throw;
                }
            });
        }

        [Test]
        public void Option_UnwrapWithCustomInvokableError()
        {
            {
                var some = "hello".ToOption();
                var invoked = false;
                Assert.AreEqual("hello", some.Unwrap(() =>
                {
                    invoked = true;
                    return "YOLO";
                }));
                Assert.IsFalse(condition: invoked);
            }

            {
                var none = Option<string>.None;
                var invoked = false;
                Assert.Throws<AttemptToUnwrapNoneWhenOptionContainedSomeException>(() =>
                {
                    try
                    {
                        none.Unwrap(() =>
                        {
                            invoked = true;
                            return "YOLO";
                        });
                    }
                    catch (AttemptToUnwrapNoneWhenOptionContainedSomeException ex)
                    {
                        Assert.AreEqual("YOLO", actual: ex.Message);
                        throw;
                    }
                });

                Assert.IsTrue(condition: invoked);
            }
        }

        [Test]
        public void Option_EqualsSameType()
        {
            var one = 1.ToOption();
            var oneClone = 1.ToOption();
            var two = 2.ToOption();
            var none = new Option<int>();

            Assert.AreEqual(expected: one, actual: oneClone);
            Assert.AreEqual(expected: oneClone, actual: one);

            Assert.AreNotEqual(expected: one, actual: two);
            Assert.AreNotEqual(expected: two, actual: one);

            Assert.AreNotEqual(expected: none, actual: one);
            Assert.AreNotEqual(expected: one, actual: none);
        }

        [Test]
        public void Option_EqualsOtherType()
        {
            var one = 1.ToOption();
            var oneAsObject = 1.ToOption<object>();
            var none = new Option<object>();

            Assert.AreNotEqual(expected: one, actual: oneAsObject);
            Assert.AreNotEqual(expected: oneAsObject, actual: one);

            Assert.AreNotEqual(expected: none, actual: one);
            Assert.AreNotEqual(expected: one, actual: none);
        }

        [Test]
        public void Option_HashAreEqualIfInstancesAreEqual()
        {
            Assert.AreEqual("hello".ToOption().GetHashCode(), "hello".ToOption().GetHashCode());
            Assert.AreEqual(1.ToOption().GetHashCode(), 1.ToOption().GetHashCode());
            Assert.AreEqual(new Option<int>().GetHashCode(), new Option<int>().GetHashCode());
        }

        [Test]
        public void Option_Map_AutoMap()
        {
            Assert.AreEqual(true.ToString(), true.ToOption().Map(b => b.ToString()).Unwrap());

            Assert.AreEqual(expected: Option<string>.None, Option<string>.None.Map(b => "foobar"));

            Assert.Throws<ArgumentNullException>(() => "Foobar".ToOption().Map<string>(null));
        }
    }
}
