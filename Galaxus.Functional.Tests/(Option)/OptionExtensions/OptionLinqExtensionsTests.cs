using System;
using System.Collections.Generic;
using NUnit.Framework;

namespace Galaxus.Functional.Tests.OptionExtensions
{
    [TestFixture]
    public class OptionLinqExtensionsTests
    {
        [Test]
        public void ElementAtOrNoneTest()
        {
            // Arrange
            var list = new List<int> {0, 1, 2, 3, 4};

            // Act
            var some1 = list.ElementAtOrNone(0);
            var some2 = list.ElementAtOrNone(2);
            var none1 = list.ElementAtOrNone(5);
            var none2 = list.ElementAtOrNone(-1);

            // Assert
            Assert.IsTrue(some1.IsSome);
            Assert.AreEqual(0, some1.Unwrap());

            Assert.IsTrue(some2.IsSome);
            Assert.AreEqual(2, some2.Unwrap());

            Assert.IsTrue(none1.IsNone);
            Assert.IsTrue(none2.IsNone);
        }

        [Test]
        public void FirstOrNoneTest()
        {
            // Arrange
            var list = new List<int> {0, 1, 2, 3, 4};

            // Act
            var some1 = list.FirstOrNone();
            var some2 = list.FirstOrNone(x => x > 2);
            var none = list.FirstOrNone(x => x == 10);

            // Assert
            Assert.IsTrue(some1.IsSome);
            Assert.AreEqual(0, some1.Unwrap());

            Assert.IsTrue(some2.IsSome);
            Assert.AreEqual(3, some2.Unwrap());

            Assert.IsTrue(none.IsNone);
        }

        [Test]
        public void LastOrNoneTest()
        {
            // Arrange
            var list = new List<int> {0, 1, 2, 3, 4};

            // Act
            var some1 = list.LastOrNone();
            var some2 = list.LastOrNone(x => x > 2);
            var none = list.FirstOrNone(x => x == 10);

            // Assert
            Assert.IsTrue(some1.IsSome);
            Assert.AreEqual(4, some1.Unwrap());

            Assert.IsTrue(some2.IsSome);
            Assert.AreEqual(4, some2.Unwrap());

            Assert.IsTrue(none.IsNone);
        }

        [Test]
        public void SingleOrNone()
        {
            // Arrange
            var list1 = new List<int> {0, 1, 2, 3, 4};
            var list2 = new List<int> {0, 1, 2, 2, 3, 4};
            var list3 = new List<int> {0};
            
            // ReSharper disable once CollectionNeverUpdated.Local
            // This is intentional
            var list4 = new List<int>();

            // Act
            var some1 = list1.SingleOrNone(x => x == 2);
            var some2 = list3.SingleOrNone();
            var none = list4.SingleOrNone();

            try
            {
                // Act and assert failure
                list2.SingleOrNone(x => x == 2);
                Assert.Fail("Calling 'SingleOrNone' on a list with multiple instances of an element should throw an exception");
            }
            catch (InvalidOperationException)
            { }

            try
            {
                // Act and assert failure
                list2.SingleOrNone();
                Assert.Fail("Calling 'SingleOrNone' on a list with more than 1 element should throw an exception");
            }
            catch(InvalidOperationException)
            { }

            // Assert
            Assert.IsTrue(some1.IsSome);
            Assert.AreEqual(2, some1.Unwrap());

            Assert.IsTrue(some2.IsSome);
            Assert.AreEqual(0, some2.Unwrap());

            Assert.IsTrue(none.IsNone);
        }
    }
}
