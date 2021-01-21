using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;

namespace Galaxus.Functional.Tests
{
    [TestFixture]
    public class EitherExtensionTests
    {
        [Test]
        public void SelectASelectBAreEvaluatedCorrectly()
        {
            var eithers = new List<Either<string, int>>
            {
                new Either<string, int>(1),
                new Either<string, int>("Hello"),
                new Either<string, int>(2),
                new Either<string, int>("World")
            };

            var aEntries = eithers.SelectA().ToList();
            var bEntries = eithers.SelectB().ToList();

            Assert.That(aEntries[0], Is.EqualTo("Hello"));
            Assert.That(aEntries[1], Is.EqualTo("World"));
            Assert.That(bEntries[0], Is.EqualTo(1));
            Assert.That(bEntries[1], Is.EqualTo(2));
        }

        [Test]
        public void SelectASelectBSelectCAreEvaluatedCorrectly()
        {
            var eithers = new List<Either<string, int, bool>>
            {
                new Either<string, int, bool>(1),
                new Either<string, int, bool>("Hello"),
                new Either<string, int, bool>(true),
                new Either<string, int, bool>(2),
                new Either<string, int, bool>("World")
            };

            var aEntries = eithers.SelectA().ToList();
            var bEntries = eithers.SelectB().ToList();
            var cEntries = eithers.SelectC().ToList();

            Assert.That(aEntries[0], Is.EqualTo("Hello"));
            Assert.That(aEntries[1], Is.EqualTo("World"));
            Assert.That(bEntries[0], Is.EqualTo(1));
            Assert.That(bEntries[1], Is.EqualTo(2));
            Assert.That(cEntries[0], Is.EqualTo(true));
        }
    }
}
