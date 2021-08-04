using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Galaxus.Functional.Linq;
using Galaxus.Functional.Tests.Helpers;
using NUnit.Framework;

namespace Galaxus.Functional.Tests.OptionExtensions
{
    [TestFixture]
    public class OptionLinqExtensionsTests
    {
        [Test]
        public void ElementAtOrNone_IndexIsZero_ReturnsSome()
        {
            var list = new List<int> { 0, 1, 2, 3, 4 };

            var some = list.ElementAtOrNone(0);

            Assert.IsTrue(some.IsSome);
            Assert.AreEqual(0, some.Unwrap());
        }

        [Test]
        public void ElementAtOrNone_IndexIsCount_ReturnsNone()
        {
            var list = new List<int> { 0, 1, 2, 3, 4 };

            var none = list.ElementAtOrNone(5);

            Assert.IsTrue(none.IsNone);
        }

        [Test]
        public void ElementAtOrNone_IndexIsNegativeOne_ReturnsNone()
        {
            var list = new List<int> { 0, 1, 2, 3, 4 };

            var none = list.ElementAtOrNone(-1);

            Assert.IsTrue(none.IsNone);
        }

        [Test]
        public void ElementAtOrNone_AppliedToFailingEnumerator_DoesNotThrow()
        {
            var index = 5;
            var sequence = new YieldElementsThenFail<string>("Hello world", index + 1);

            var composition = sequence.ElementAtOrNone(index);

            Assert.IsTrue(composition.IsSome);
            Assert.Throws<AssertionException>(() => sequence.ToList());
        }

        [Test]
        public void FirstOrNone_NoPredicate_ReturnsSome()
        {
            var list = new List<int> { 0, 1, 2, 3, 4 };

            var some = list.FirstOrNone();

            Assert.IsTrue(some.IsSome);
            Assert.AreEqual(0, some.Unwrap());
        }

        [Test]
        public void FirstOrNone_WithPredicate_ReturnsSome()
        {
            var list = new List<int> { 0, 1, 2, 3, 4 };
            var some = list.FirstOrNone(x => x > 2);

            Assert.IsTrue(some.IsSome);
            Assert.AreEqual(3, some.Unwrap());
        }

        [Test]
        public void FirstOrNone_PredicateMatchesNone_ReturnsNone()
        {
            var list = new List<int> { 0, 1, 2, 3, 4 };

            var none = list.FirstOrNone(x => x == 10);

            Assert.IsTrue(none.IsNone);
        }

        [Test]
        public void FirstOrNone_AppliedToFailingEnumerator_DoesNotThrow()
        {
            var sequence = new YieldElementsThenFail<string>("Hello world", 1);

            var composition = sequence.FirstOrNone();

            Assert.IsTrue(composition.IsSome);
            Assert.Throws<AssertionException>(() => sequence.ToList());
        }

        [Test]
        public void LastOrNone_GetLast_ReturnsSome()
        {
            var list = new List<int> { 0, 1, 2, 3, 4 };

            var some1 = list.LastOrNone();

            Assert.IsTrue(some1.IsSome);
            Assert.AreEqual(4, some1.Unwrap());
        }

        [Test]
        public void LastOrNone_WithPredicate_ReturnsSome()
        {
            var list = new List<int> { 0, 1, 2, 3, 4 };

            var some2 = list.LastOrNone(x => x > 2);

            Assert.IsTrue(some2.IsSome);
            Assert.AreEqual(4, some2.Unwrap());
        }

        [Test]
        public void LastOrNon_PredicateMatchesNon_ReturnsNone()
        {
            var list = new List<int> { 0, 1, 2, 3, 4 };

            var none = list.FirstOrNone(x => x == 10);

            Assert.IsTrue(none.IsNone);
        }

        [Test]
        public void SingleOrNone_SingleMatchingElement_ReturnsSome()
        {
            var list1 = new List<int> { 0, 1, 2, 3, 4 };

            var some1 = list1.SingleOrNone(x => x == 2);

            Assert.IsTrue(some1.IsSome);
            Assert.AreEqual(2, some1.Unwrap());
        }

        [Test]
        public void SingleOrNone_SingleElementInCollection_ReturnsSome()
        {
            var list3 = new List<int> { 0 };

            var some2 = list3.SingleOrNone();

            Assert.IsTrue(some2.IsSome);
            Assert.AreEqual(0, some2.Unwrap());
        }

        [Test]
        public void SingleOrNone_MultipleMatchingElements_ThrowsException()
        {
            var list2 = new List<int> { 0, 1, 2, 2, 3, 4 };

            Assert.Throws<InvalidOperationException>(() => list2.SingleOrNone(x => x == 2));
        }

        [Test]
        public void SingleOrNone_MultipleElements_ThrowsException()
        {
            var list2 = new List<int> { 0, 1, 2, 2, 3, 4 };

            Assert.Throws<InvalidOperationException>(() => list2.SingleOrNone());
        }

        [Test]
        public void SingleOrNone_EmptyList_ReturnsNone()
        {
            // ReSharper disable once CollectionNeverUpdated.Local
            // This is intentional
            var list4 = new List<int>();

            var none = list4.SingleOrNone();

            Assert.IsTrue(none.IsNone);
        }

        [Test]
        public void SingleOrNone_AppliedToFailingEnumerator_DoesNotThrow()
        {
            var sequence = new YieldElementsThenFail<string>("Hello world", 2);

            Assert.Throws<InvalidOperationException>(() => sequence.SingleOrNone());
            Assert.Throws<AssertionException>(() => sequence.ToList());
        }

        [Test]
        public void GetValueOrNone_WithValue_ReturnsValue()
        {
            // Arrange
            const string key = "Asterix";
            const string expectedValue = "Obelix";
            var bestFriends = new Dictionary<string, string> { { key, expectedValue }, };

            // Act
            var bestFriendOrNone = bestFriends.GetValueOrNone(key);

            // Assert
            Assert.That(bestFriendOrNone.Unwrap(), Is.EqualTo(expectedValue));
        }

        [Test]
        public void GetValueOrNone_WithoutValue_ReturnsNone()
        {
            // Arrange
            var bestFriends = new Dictionary<string, string>();

            // Act
            var bestFriendOrNone = bestFriends.GetValueOrNone("The Grinch");

            // Assert
            Assert.That(bestFriendOrNone, Is.EqualTo(Option<string>.None));
        }
        
        [Test]
        public void GetValueOrNone_WorksForReadOnlyDictionary()
        {
            // Arrange
            const string key = "Asterix";
            const string expectedValue = "Obelix";
            var bestFriends = new ReadOnlyDictionary<string, string>(new Dictionary<string, string> { { key, expectedValue }, });

            // Act
            var bestFriendOrNone = bestFriends.GetValueOrNone(key);

            // Assert
            Assert.That(bestFriendOrNone.Unwrap(), Is.EqualTo(expectedValue));
        } 
    }
}
