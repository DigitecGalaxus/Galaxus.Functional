using NUnit.Framework;
using System;
using System.Threading.Tasks;

namespace Galaxus.Functional.Tests
{
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
                bool called = false;
                result.Match(ok =>
                {
                    called = true;
                    Assert.AreEqual("hello", ok);
                }, err => Assert.Fail());
                Assert.IsTrue(called);
            }

            {
                bool called = false;

                var number = result.Match(ok =>
                {
                    called = true;
                    Assert.AreEqual("hello", ok);
                    return 666;
                }, err => {
                    Assert.Fail();
                    throw new InvalidOperationException();
                });

                Assert.AreEqual(666, number);
                Assert.IsTrue(called);
            }

            {
                bool called = false;
                result.IfOk(ok =>
                {
                    Assert.AreEqual("hello", ok);
                    called = true;
                });
                Assert.IsTrue(called);
            }

            {
                bool called = false;
                result.IfErr(err => called = true);
                Assert.IsFalse(called);
            }
        }

        [Test]
        public void Result_MatchErr()
        {
            var result = Result<string, int>.FromErr(99);

            {
                bool called = false;
                result.Match(ok => Assert.Fail(), err =>
                {
                    Assert.AreEqual(99, err);
                    called = true;
                });
                Assert.IsTrue(called);
            }

            {
                bool called = false;

                var number = result.Match(ok =>
                {
                    Assert.Fail();
                    throw new InvalidOperationException();
                },
                err => {
                    called = true;
                    Assert.AreEqual(99, err);
                    return 666;
                });

                Assert.AreEqual(666, number);
                Assert.IsTrue(called);
            }

            {
                bool called = false;
                result.IfErr(err =>
                {
                    Assert.AreEqual(99, err);
                    called = true;
                });
                Assert.IsTrue(called);
            }

            {
                bool called = false;
                result.IfOk(ok => called = true);
                Assert.IsFalse(called);
            }
        }

        [Test]
        public void ResultMatchWithNullMatchPatternThrows()
        {
            // OK
            Assert.Throws<ArgumentNullException>(() => { 0.ToOk<int, string>().Match(null, err => { }); });
            Assert.Throws<ArgumentNullException>(() => { 0.ToOk<int, string>().IfOk(null); });

            // ERR
            Assert.Throws<ArgumentNullException>(() => { "hello".ToErr<int, string>().Match(v => { }, null); });
            Assert.Throws<ArgumentNullException>(() => { "hello".ToErr<int, string>().IfErr(null); });

            // OK and ERR with return value
            Assert.Throws<ArgumentNullException>(() => { _ = 0.ToOk<int, string>().Match(null, err => 0); });
            Assert.Throws<ArgumentNullException>(() => { _ = "hello".ToErr<int, string>().Match(v => v, null); });
        }

        [Test]
        public async Task MatchAsync_PreviousResultWasSuccess_ReturnsOk()
        {
            const string successString = "success";

            Func<string, Task<string>> continuationOk = async s => await Task.FromResult(s);

            Func<string, Task<string>> continuationErr = async _ =>  await Task.FromResult("error");

            var result = await Result<string, string>.FromOk(successString)
                .MatchAsync(
                    async ok => await continuationOk(ok),
                    async err => await continuationErr(err));

            Assert.That(result, Is.EqualTo(successString));
        }

        [Test]
        public async Task MatchAsync_PreviousResultWasError_ReturnsError()
        {
            const string errorString = "error";

            Func<string, Task<string>> continuationOk = async s => await Task.FromResult(s);

            Func<string, Task<string>> continuationErr = async _ =>  await Task.FromResult(errorString);

            var result = await Result<string, string>.FromErr("any error message")
                .MatchAsync(
                    async ok => await continuationOk(ok),
                    async err => await continuationErr(err));

            Assert.That(result, Is.EqualTo(errorString));
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
            Result<int, int> Square(int i) => Result<int, int>.FromOk(i * i);
            Result<int, int> Error(int i) => Result<int, int>.FromErr(i);

            Assert.AreEqual(Result<int, int>.FromOk(2), Result<int, int>.FromOk(2).OrElse(Square).OrElse(Square));
            Assert.AreEqual(Result<int, int>.FromOk(2), Result<int, int>.FromOk(2).OrElse(Error).OrElse(Square));
            Assert.AreEqual(Result<int, int>.FromOk(9), Result<int, int>.FromErr(3).OrElse(Square).OrElse(Error));
            Assert.AreEqual(Result<int, int>.FromErr(3), Result<int, int>.FromErr(3).OrElse(Error).OrElse(Error));
        }

        [Test]
        public async Task OrElseAsync_PreviousResultWasSuccess_ContinuationIsApplied()
        {
            const string initialResult = "a";
            const string continuationResult = "b";
            Func<string, Task<Result<string, string>>> continuation = s => Task.FromResult(Result<string, string>.FromErr(continuationResult));
            var result = await Result<string, string>.FromErr(initialResult)
                .OrElseAsync(continuation);

            result.Match(
                ok => Assert.Fail(),
                err => Assert.AreEqual(continuationResult, err));
        }

        [Test]
        public async Task OrElseAsync_PreviousResultWasSuccess_ContinuationIsNotApplied()
        {
            const string initialResult = "a";
            const string continuationResult = "b";
            Func<string, Task<Result<string, string>>> continuation = s => Task.FromResult(Result<string, string>.FromErr(continuationResult));

            var result = await Result<string, string>.FromOk(initialResult)
                .OrElseAsync(continuation);

            result.Match(
                ok => Assert.AreEqual(initialResult, ok),
                err => Assert.Fail());
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
                bool invoked = false;

                Assert.AreEqual("hello", ok.UnwrapOrElse(() =>
                {
                    invoked = true;
                    return "world";
                }));

                Assert.IsFalse(invoked);
            }

            {
                bool invoked = false;

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
            Assert.Throws<AttemptToUnwrapErrWhenResultWasOkException>(() => err.Unwrap());
        }

        [Test]
        public void Result_UnwrapWithCustomError()
        {
            var ok = "hello".ToOk<string, int>();
            var err = 99.ToErr<string, int>();

            Assert.AreEqual("hello", ok.Unwrap("YOLO"));
            Assert.Throws<AttemptToUnwrapErrWhenResultWasOkException>(() => {
                try
                {
                    err.Unwrap("YOLO");
                }
                catch (AttemptToUnwrapErrWhenResultWasOkException ex)
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
                bool invoked = false;
                Assert.AreEqual("hello", ok.Unwrap(err => { invoked = true; return "YOLO"; }));
                Assert.IsFalse(invoked);
            }

            {
                var err = 0.ToErr<string, int>();
                bool invoked = false;
                Assert.Throws<AttemptToUnwrapErrWhenResultWasOkException>(() =>
                {
                    try
                    {
                        err.Unwrap(err_ => { invoked = true; return "YOLO"; });
                    }
                    catch (AttemptToUnwrapErrWhenResultWasOkException ex)
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

                Assert.AreEqual( Result<string, string>.FromErr("early error"), err.And(ok));
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
            Result<int, int> Square(int i) => Result<int, int>.FromOk(i * i);
            Result<int, int> Error(int i) => Result<int, int>.FromErr(i);

            Assert.AreEqual(Result<int, int>.FromOk(16), Result<int, int>.FromOk(2).AndThen(Square).AndThen(Square));
            Assert.AreEqual(Result<int, int>.FromErr(4), Result<int, int>.FromOk(2).AndThen(Square).AndThen(Error));
            Assert.AreEqual(Result<int, int>.FromErr(2), Result<int, int>.FromOk(2).AndThen(Error).AndThen(Square));
            Assert.AreEqual(Result<int, int>.FromErr(3), Result<int, int>.FromErr(3).AndThen(Square).AndThen(Square));
        }

        [Test]
        public void Result_AndThenWithAction()
        {
            Result<int, int> Square(int i) => Result<int, int>.FromOk(i * i);
            Result<int, int> Error(int i) => Result<int, int>.FromErr(i);

            {
                var invoked = false;
                Assert.AreEqual(Result<int, int>.FromOk(9), Result<int, int>.FromOk(3).AndThen(i => invoked = true).AndThen(Square));
                Assert.IsTrue(invoked);
            }
            {
                var invoked = false;
                Assert.AreEqual(Result<int, int>.FromErr(3), Result<int, int>.FromOk(3).AndThen(i => invoked = true).AndThen(Error));
                Assert.IsTrue(invoked);
            }
            {
                var invoked = false;
                Assert.AreEqual(Result<int, int>.FromErr(3), Result<int, int>.FromOk(3).AndThen(Error).AndThen(i => invoked = true));
                Assert.IsFalse(invoked);
            }
            {
                var invoked = false;
                Assert.AreEqual(Result<int, int>.FromErr(5), Result<int, int>.FromErr(5).AndThen(Square).AndThen(i => invoked = true));
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
                return x.Match(ok => Result<Unit, int>.FromOk(Unit.Value), err => err);
            }

            var nextResult = success.AndThen(s => Nop(s));
            Assert.IsTrue(nextResult.IsErr);
            Assert.AreEqual(5, nextResult.Err.Unwrap());
        }

        [Test]
        public async Task AndThenAsync_PreviousResultWasSuccess_ContinuationIsApplied()
        {
            const string initialResult = "a";
            const string continuationResult = "b";
            Func<string, Task<Result<string, string>>> continuation = s => Task.FromResult(Result<string, string>.FromOk(continuationResult));
            var result = await Result<string, string>.FromOk(initialResult)
                .AndThenAsync(continuation);
            Assert.AreEqual(expected: continuationResult, actual: result.Ok.UnwrapOrDefault());
        }

        [Test]
        public async Task AndThenAsync_PreviousResultWasError_ContinuationIsNotApplied()
        {
            const string initialError = "a";
            const string continuationResult = "b";
            Func<string, Task<Result<string, string>>> continuation = s => Task.FromResult(Result<string, string>.FromOk(continuationResult));
            var result = await Result<string, string>.FromErr(initialError)
                .AndThenAsync(continuation);
            Assert.IsTrue(result.IsErr);
        }

        [Test]
        public async Task MapAsync_PreviousResultWasSuccess_ContinuationIsApplied()
        {
            const string initialResult = "a";
            const string continuationResult = "b";
            Func<string, Task<string>> continuation = s => Task.FromResult(continuationResult);
            var result = await Result<string, string>.FromOk(initialResult)
                .MapAsync(continuation);
            Assert.AreEqual(expected: continuationResult, actual: result.Ok.UnwrapOrDefault());
        }

        [Test]
        public async Task MapAsync_PreviousResultWasError_ContinuationIsNotApplied()
        {
            const string initialError = "a";
            const string continuationResult = "b";
            Func<string, Task<string>> continuation = s => Task.FromResult(continuationResult);
            var result = await Result<string, string>.FromErr(initialError)
                .MapAsync(continuation);
            Assert.IsTrue(result.IsErr);
        }

        [Test]
        public void MapAsync_ContinuationThrowsException_CorrectExceptionIsPropagated()
        {
            const string initialResult = "a";
            Func<string, Task<string>> continuation = s => Task.FromException<string>(new ArgumentException());

            Assert.ThrowsAsync<ArgumentException>(async () =>
            {
                await Result<string, string>.FromOk(initialResult)
                    .MapAsync(continuation);
            });
        }

        [Test]
        public async Task MapErrAsync_PreviousResultWasError_ContinuationIsApplied()
        {
            const string initialError = "a";
            const string continuationResult = "b";
            Func<string, Task<string>> continuation = s => Task.FromResult(continuationResult);
            var result = await Result<string, string>.FromErr(initialError)
                .MapErrAsync(continuation);
            Assert.AreEqual(expected: continuationResult, actual: result.Err.UnwrapOrDefault());
        }

        [Test]
        public async Task MapErrAsync_PreviousResultWasSuccess_ContinuationIsNotApplied()
        {
            const string initialResult = "a";
            const string continuationResult = "b";
            Func<string, Task<string>> continuation = s => Task.FromResult(continuationResult);
            var result = await Result<string, string>.FromOk(initialResult)
                .MapErrAsync(continuation);
            Assert.IsTrue(result.IsOk);
        }

        [Test]
        public void MapErrAsync_ContinuationThrowsException_CorrectExceptionIsPropagated()
        {
            const string initialResult = "a";
            Func<string, Task<string>> continuation = s => Task.FromException<string>(new ArgumentException());

            Assert.ThrowsAsync<ArgumentException>(async () =>
            {
                await Result<string, string>.FromErr(initialResult)
                    .MapErrAsync(continuation);
            });
        }

        [Test]
        public void MatchAsync_ContinuationThrowsException_CorrectExceptionIsPropagated()
        {
            const string initialResult = "a";
            Func<string, Task<string>> continuationOk = s => Task.FromException<string>(new ArgumentException());
            Func<string, Task<string>> continuationErr = s => Task.FromResult("b");

            Assert.ThrowsAsync<ArgumentException>(async () =>
            {
                await Result<string, string>.FromOk(initialResult)
                    .MatchAsync(continuationOk, continuationErr);
            });
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

        [Test]
        public async Task IfOkAsync_PreviousResultWasSuccess_ContinuationIsApplied()
        {
            const string continuationString = "b";

            var result = string.Empty;

            Func<string, Task> continuation = async s =>
            {
                await Task.Delay(1000);
                result = s;
            };

            await Result<string, string>.FromOk(continuationString).IfOkAsync(async s => await continuation(s));

            Assert.That(result, Is.EqualTo(continuationString));
        }

        [Test]
        public async Task IfOkAsync_PreviousResultWasError_ContinuationIsNotApplied()
        {
            var result = string.Empty;

            Func<string, Task> continuation = async s =>
            {
                await Task.Delay(1000);
                result = s;
            };

            await Result<string, string>.FromErr("err").IfOkAsync(async s => await continuation(s));

            Assert.That(result, Is.EqualTo(string.Empty));
        }

        [Test]
        public async Task IfErrAsync_PreviousResultWasError_ContinuationIsApplied()
        {
            const string continuationString = "b";

            var result = string.Empty;

            Func<string, Task> continuation = async s =>
            {
                await Task.Delay(1000);
                result = s;
            };

            await Result<string, string>.FromErr(continuationString).IfErrAsync(async s => await continuation(s));

            Assert.That(result, Is.EqualTo(continuationString));
        }

        [Test]
        public async Task IfErrAsync_PreviousResultWasSuccess_ContinuationIsNotApplied()
        {
            var result = string.Empty;

            Func<string, Task> continuation = async s =>
            {
                await Task.Delay(1000);
                result = s;
            };

            await Result<string, string>.FromOk("success").IfErrAsync(async s => await continuation(s));

            Assert.That(result, Is.EqualTo(string.Empty));
        }
    }
}
