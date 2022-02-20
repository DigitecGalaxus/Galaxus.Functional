using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading.Tasks;
using NUnit.Framework;

namespace Galaxus.Functional.Tests
{
    [TestFixture]
    public class EitherTests
    {
        [Test]
        [SuppressMessage("ReSharper", "ObjectCreationAsStatement",
            Justification = "The 'new' throws anyways - at least it should")]
        public void CtorThrowsWhenNull()
        {
            Assert.Throws<ArgumentNullException>(() => new Either<string, object>(default));
            Assert.Throws<ArgumentNullException>(() => new Either<string, object>(default(object)));
        }

        [Test]
        public void UnionEqualityAndHashCodeWithValueTypeVariant()
        {
            var union1 = new Either<string, int>(117);
            var union2 = new Either<string, int>(666);
            var union3 = new Either<string, int>(117);
            Assert.AreNotEqual(expected: union1, actual: union2);
            Assert.AreEqual(expected: union1, actual: union3);
            Assert.AreNotEqual(union1.GetHashCode(), union2.GetHashCode());
            Assert.AreEqual(union1.GetHashCode(), union3.GetHashCode());
        }

        [Test]
        public void UnionEqualityAndHashCodeWithReferenceTypeVariant()
        {
            var union1 = new Either<string, int>("hello");
            var union2 = new Either<string, int>("bye");
            var union3 = new Either<string, int>("hello");
            Assert.AreNotEqual(expected: union1, actual: union2);
            Assert.AreEqual(expected: union1, actual: union3);
            Assert.AreNotEqual(union1.GetHashCode(), union2.GetHashCode());
            Assert.AreEqual(union1.GetHashCode(), union3.GetHashCode());
        }

        [Test]
        public void UnionEqualityAndHashCodeWithMixedTypeVariants()
        {
            var union1 = new Either<string, int>(117);
            var union2 = new Either<string, int>("hello");
            Assert.AreNotEqual(expected: union1, actual: union2);
            Assert.AreNotEqual(union1.GetHashCode(), union2.GetHashCode());
        }

        [Test]
        public void UnionEqualityWithMixedValueTypeVariantsBothZero()
        {
            var union1 = new Either<byte, int>(default);
            var union2 = new Either<byte, int>(default(int));
            Assert.AreNotEqual(expected: union1, actual: union2);
        }

        [Test]
        public void IsAIsBAreEvaluatedCorrectly()
        {
            var someA = new Either<string, int>("hello");
            var someB = new Either<string, int>(117);

            Assert.IsTrue(condition: someA.IsA);
            Assert.IsFalse(condition: someA.IsB);
            Assert.IsFalse(condition: someB.IsA);
            Assert.IsTrue(condition: someB.IsB);
        }

        [Test]
        public void IsAIsBIsCAreEvaluatedCorrectly()
        {
            var someA = new Either<string, int, bool>("hello");
            var someB = new Either<string, int, bool>(117);
            var someC = new Either<string, int, bool>(true);

            Assert.IsTrue(condition: someA.IsA);
            Assert.IsFalse(condition: someA.IsB);
            Assert.IsFalse(condition: someA.IsC);
            Assert.IsFalse(condition: someB.IsA);
            Assert.IsTrue(condition: someB.IsB);
            Assert.IsFalse(condition: someB.IsC);
            Assert.IsFalse(condition: someC.IsA);
            Assert.IsFalse(condition: someC.IsB);
            Assert.IsTrue(condition: someC.IsC);
        }

        [Test]
        public async Task Either2MatchAsyncForAReturnsA()
        {
            Func<string, Task<string>> continuationA = async s => await Task.FromResult(result: s);
            Func<bool, Task<string>> continuationB = async _ => await Task.FromResult("B");

            var someA = new Either<string, bool>("A");

            var result = await someA.MatchAsync(
                async a => await continuationA(arg: a),
                async b => await continuationB(arg: b));

            Assert.That(actual: result, Is.EqualTo("A"));
        }

        [Test]
        public async Task Either2MatchAsyncForBReturnsB()
        {
            Func<bool, Task<string>> continuationA = async _ => await Task.FromResult("A");
            Func<string, Task<string>> continuationB = async s => await Task.FromResult(result: s);

            var someB = new Either<bool, string>("B");

            var result = await someB.MatchAsync(
                async a => await continuationA(arg: a),
                async b => await continuationB(arg: b));

            Assert.That(actual: result, Is.EqualTo("B"));
        }

        [Test]
        public async Task Either3MatchAsyncForAReturnsA()
        {
            Func<string, Task<string>> continuationA = async s => await Task.FromResult(result: s);
            Func<bool, Task<string>> continuationB = async _ => await Task.FromResult("B");
            Func<int, Task<string>> continuationC = async _ => await Task.FromResult("C");

            var someA = new Either<string, bool, int>("A");

            var result = await someA.MatchAsync(
                async a => await continuationA(arg: a),
                async b => await continuationB(arg: b),
                async c => await continuationC(arg: c));

            Assert.That(actual: result, Is.EqualTo("A"));
        }

        [Test]
        public async Task Either3MatchAsyncForBReturnsB()
        {
            Func<bool, Task<string>> continuationA = async _ => await Task.FromResult("A");
            Func<string, Task<string>> continuationB = async s => await Task.FromResult(result: s);
            Func<int, Task<string>> continuationC = async _ => await Task.FromResult("C");

            var someB = new Either<bool, string, int>("B");

            var result = await someB.MatchAsync(
                async a => await continuationA(arg: a),
                async b => await continuationB(arg: b),
                async c => await continuationC(arg: c));

            Assert.That(actual: result, Is.EqualTo("B"));
        }

        [Test]
        public async Task Either3MatchAsyncForCReturnsC()
        {
            Func<bool, Task<string>> continuationA = async _ => await Task.FromResult("A");
            Func<int, Task<string>> continuationB = async _ => await Task.FromResult("B");
            Func<string, Task<string>> continuationC = async s => await Task.FromResult(result: s);

            var someC = new Either<bool, int, string>("C");

            var result = await someC.MatchAsync(
                async a => await continuationA(arg: a),
                async b => await continuationB(arg: b),
                async c => await continuationC(arg: c));

            Assert.That(actual: result, Is.EqualTo("C"));
        }

        [Test]
        [TestCaseSource(nameof(IEitherReturnsCorrectTypeTestCases))]
        public void IEitherReturnsCorrectType(IEither givenInterface, Type expectedType)
        {
            Assert.AreEqual(expected: expectedType, givenInterface.ToObject().GetType());
        }

        [Test]
        [TestCase(3)]
        [TestCase("Foo")]
        [TestCase(default(byte))]
        public void ToStringDelegates<T>(T arg)
        {
            var eitherLeft = new Either<T, Unit>(a: arg);
            var toStringLeftResult = eitherLeft.ToString();
            Assert.IsNotNull(anObject: toStringLeftResult);
            Assert.AreEqual(arg.ToString(), actual: toStringLeftResult);

            var eitherRight = new Either<Unit, T>(b: arg);
            var toStringRightResult = eitherRight.ToString();
            Assert.IsNotNull(anObject: toStringRightResult);
            Assert.AreEqual(arg.ToString(), actual: toStringRightResult);
        }

        private static IEnumerable<object[]> IEitherReturnsCorrectTypeTestCases()
        {
            return IEitherReturnsCorrectTypeTestCasesExplicit()
                .Select(testObj => new object[] { testObj.GivenInterface, testObj.ExpectedType });
        }

        private static IEnumerable<(IEither GivenInterface, Type ExpectedType)>
            IEitherReturnsCorrectTypeTestCasesExplicit()
        {
            yield return (new Either<string, char>("hello"), typeof(string));
            yield return (new Either<string, char>('B'), typeof(char));
            yield return (new Either<string, char, bool>("World"), typeof(string));
            yield return (new Either<string, char, bool>('B'), typeof(char));
            yield return (new Either<string, char, bool>(false), typeof(bool));
        }

        [Test]
        [TestCaseSource(nameof(IEitherReturnsCorrectObjectTestCases))]
        public void IEitherReturnsCorrectObject(IEither givenInterface, object expectedObject)
        {
            Assert.AreEqual(expected: expectedObject, givenInterface.ToObject());
        }

        private static IEnumerable<object[]> IEitherReturnsCorrectObjectTestCases()
        {
            return IEitherReturnsCorrectObjectTestCasesExplicit()
                .Select(testObj => new[] { testObj.GivenInterface, testObj.ExpectedObject });
        }

        private static IEnumerable<(IEither GivenInterface, object ExpectedObject)>
            IEitherReturnsCorrectObjectTestCasesExplicit()
        {
            yield return (new Either<string, char>("Digitec"), "Digitec");
            yield return (new Either<string, char>('X'), 'X');
            yield return (new Either<string, char, bool>("Galaxus"), "Galaxus");
            yield return (new Either<string, char, bool>('Y'), 'Y');
            yield return (new Either<string, char, bool>(true), true);
        }
    }
}
