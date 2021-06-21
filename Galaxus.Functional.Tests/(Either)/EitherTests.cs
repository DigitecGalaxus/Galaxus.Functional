using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading.Tasks;

namespace Galaxus.Functional.Tests
{
    [TestFixture]
    public class EitherTests
    {
        [Test]
        [SuppressMessage("ReSharper", "ObjectCreationAsStatement", Justification = "The 'new' throws anyways - at least it should")]
        public void CtorThrowsWhenNull()
        {
            Assert.Throws<ArgumentNullException>(() => new Either<string, object>(default(string)));
            Assert.Throws<ArgumentNullException>(() => new Either<string, object>(default(object)));
        }

        [Test]
        public void UnionEqualityAndHashCodeWithValueTypeVariant()
        {
            var union1 = new Either<string, int>(117);
            var union2 = new Either<string, int>(666);
            var union3 = new Either<string, int>(117);
            Assert.AreNotEqual(union1, union2);
            Assert.AreEqual(union1, union3);
            Assert.AreNotEqual(union1.GetHashCode(), union2.GetHashCode());
            Assert.AreEqual(union1.GetHashCode(), union3.GetHashCode());
        }

        [Test]
        public void UnionEqualityAndHashCodeWithReferenceTypeVariant()
        {
            var union1 = new Either<string, int>("hello");
            var union2 = new Either<string, int>("bye");
            var union3 = new Either<string, int>("hello");
            Assert.AreNotEqual(union1, union2);
            Assert.AreEqual(union1, union3);
            Assert.AreNotEqual(union1.GetHashCode(), union2.GetHashCode());
            Assert.AreEqual(union1.GetHashCode(), union3.GetHashCode());
        }

        [Test]
        public void UnionEqualityAndHashCodeWithMixedTypeVariants()
        {
            var union1 = new Either<string, int>(117);
            var union2 = new Either<string, int>("hello");
            Assert.AreNotEqual(union1, union2);
            Assert.AreNotEqual(union1.GetHashCode(), union2.GetHashCode());
        }

        [Test]
        public void UnionEqualityWithMixedValueTypeVariantsBothZero()
        {
            var union1 = new Either<byte, int>(default(byte));
            var union2 = new Either<byte, int>(default(int));
            Assert.AreNotEqual(union1, union2);
        }
        
        [Test]
        public void IsAIsBAreEvaluatedCorrectly()
        {
            var someA = new Either<string, int>("hello");
            var someB = new Either<string, int>(117);

            Assert.IsTrue(someA.IsA);
            Assert.IsFalse(someA.IsB);
            Assert.IsFalse(someB.IsA);
            Assert.IsTrue(someB.IsB);
        }
        
        [Test]
        public void IsAIsBIsCAreEvaluatedCorrectly()
        {
            var someA = new Either<string, int, bool>("hello");
            var someB = new Either<string, int, bool>(117);
            var someC = new Either<string, int, bool>(true);

            Assert.IsTrue(someA.IsA);
            Assert.IsFalse(someA.IsB);
            Assert.IsFalse(someA.IsC);
            Assert.IsFalse(someB.IsA);
            Assert.IsTrue(someB.IsB);
            Assert.IsFalse(someB.IsC);
            Assert.IsFalse(someC.IsA);
            Assert.IsFalse(someC.IsB);
            Assert.IsTrue(someC.IsC);
        }

        [Test]
        public async Task Either2MatchAsyncForAReturnsA()
        {
            Func<string, Task<string>> continuationA = async s => await Task.FromResult(s);
            Func<bool, Task<string>> continuationB = async _ =>  await Task.FromResult("B");

            var someA = new Either<string, bool>("A");

            var result = await someA.MatchAsync(
                async a => await continuationA(a),
                async b => await continuationB(b));

            Assert.That(result, Is.EqualTo("A"));
        }
        
        [Test]
        public async Task Either2MatchAsyncForBReturnsB()
        {
            Func<bool, Task<string>> continuationA = async _ => await Task.FromResult("A");
            Func<string, Task<string>> continuationB = async s =>  await Task.FromResult(s);

            var someA = new Either<bool, string>("B");

            var result = await someA.MatchAsync(
                async a => await continuationA(a),
                async b => await continuationB(b));

            Assert.That(result, Is.EqualTo("B"));
        }
        
        [Test]
        public async Task Either3MatchAsyncForAReturnsA()
        {
            Func<string, Task<string>> continuationA = async s => await Task.FromResult(s);
            Func<bool, Task<string>> continuationB = async _ =>  await Task.FromResult("B");
            Func<int, Task<string>> continuationC = async _ =>  await Task.FromResult("C");

            var someA = new Either<string, bool, int>("A");

            var result = await someA.MatchAsync(
                async a => await continuationA(a),
                async b => await continuationB(b),
                async c => await continuationC(c));

            Assert.That(result, Is.EqualTo("A"));
        }
        
        [Test]
        public async Task Either3MatchAsyncForBReturnsB()
        {
            Func<bool, Task<string>> continuationA = async _ => await Task.FromResult("A");
            Func<string, Task<string>> continuationB = async s =>  await Task.FromResult(s);
            Func<int, Task<string>> continuationC = async _ =>  await Task.FromResult("C");

            var someA = new Either<bool, string, int>("B");

            var result = await someA.MatchAsync(
                async a => await continuationA(a),
                async b => await continuationB(b),
                async c => await continuationC(c));

            Assert.That(result, Is.EqualTo("B"));
        }
        
        [Test]
        public async Task Either3MatchAsyncForCReturnsC()
        {
            Func<bool, Task<string>> continuationA = async _ => await Task.FromResult("A");
            Func<int, Task<string>> continuationB = async _ =>  await Task.FromResult("B");
            Func<string, Task<string>> continuationC = async s =>  await Task.FromResult(s);

            var someA = new Either<bool, int, string>("C");

            var result = await someA.MatchAsync(
                async a => await continuationA(a),
                async b => await continuationB(b),
                async c => await continuationC(c));

            Assert.That(result, Is.EqualTo("C"));
        }

        [Test]
        [TestCaseSource(nameof(IEitherReturnsCorrectTypeTestCases))]
        public void IEitherReturnsCorrectType(IEither givenInterface, Type expectedType)
        {
            Assert.AreEqual(expectedType, givenInterface.ToObject().GetType());
        }

        [Test]
        [TestCase(3)]
        [TestCase("Foo")]
        [TestCase(default(byte))]
        public void ToStringDelegates<T>(T arg)
        {
            var eitherLeft = new Either<T, Unit>(arg);
            var toStringLeftResult = eitherLeft.ToString();
            Assert.IsNotNull(toStringLeftResult);
            Assert.AreEqual(arg.ToString(), toStringLeftResult);

            var eitherRight = new Either<Unit, T>(arg);
            var toStringRightResult = eitherRight.ToString();
            Assert.IsNotNull(toStringRightResult);
            Assert.AreEqual(arg.ToString(), toStringRightResult);
        }

        private static IEnumerable<object[]> IEitherReturnsCorrectTypeTestCases()
        {
            return IEitherReturnsCorrectTypeTestCasesExplicit()
                .Select(testObj => new object[] {testObj.GivenInterface, testObj.ExpectedType});
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
            Assert.AreEqual(expectedObject, givenInterface.ToObject());
        }

        private static IEnumerable<object[]> IEitherReturnsCorrectObjectTestCases()
        {
            return IEitherReturnsCorrectObjectTestCasesExplicit()
                .Select(testObj => new [] {testObj.GivenInterface, testObj.ExpectedObject});
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
