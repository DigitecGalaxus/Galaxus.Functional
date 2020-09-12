using System;
using NUnit.Framework;
using static Galaxus.Functional.Prelude;

namespace Galaxus.Functional.Tests
{
    public class PreludeTests
    {
        [Test]
        public void Identity_GivenValue_ReturnsThatValue()
        {
            // Arrange
            var func = Identity<int>();

            // Act
            var result = func(12);

            // Assert
            Assert.That(result, Is.EqualTo(12));
        }

        [Test]
        public void Const_GivenValue_ReturnsConstantAndIgnoresParameter()
        {
            // Arrange
            var func = Const(12);

            // Act
            var result = func(5);

            // Assert
            Assert.That(result, Is.EqualTo(12));
        }

        [Test]
        public void Flip_GivenParameters_ReturnsCorrectResult()
        {
            // Arrange
            Func<int, int, int> func = (x, y) => x - y;

            // Act
            var result = func.Flip().Invoke(1, 6);

            // Assert
            Assert.That(result, Is.EqualTo(5));
        }

        [Test]
        public void Do_GivenAction_ExecutesActionAndReturnsUnit()
        {
            // Arrange
            var parameter = 0;
            Action<int> action = x => parameter = x;

            // Act
            var result = action.Do().Invoke(5);

            // Assert
            Assert.That(parameter, Is.EqualTo(5));
            Assert.That(result, Is.EqualTo(Unit.Value));
        }
    }
}
