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

        [Test]
        public void Compose_GivenTwoFunctions_ExecutesThemChained()
        {
            // Arrange
            Func<string, int> first = x => x.Length;
            Func<int, double> second = x => Math.Sqrt(x);

            // Act
            var func = first.Compose(second);
            var result = func.Invoke("Hell");

            // Assert
            Assert.That(result, Is.EqualTo(2)); // Square Root of 4, which is length of "Hell"
        }

        internal class BindTests
        {
            [Test]
            public void Bind1_GivenParam_ReturnsUnaryFunctionReturningResult()
            {
                // Arrange
                var func = Identity<int>();

                // Act
                var boundFunc = func.Bind(1);
                var result = boundFunc.Invoke();

                // Assert
                Assert.That(result, Is.EqualTo(1));
            }

            [Test]
            public void Bind2_GivenParam_ReturnsUnaryFunctionReturningResult()
            {
                // Arrange
                Func<int, int, int> func = (x, y) => x + y;

                // Act
                var boundFunc = func.Bind(1);
                var result = boundFunc.Invoke(2);

                // Assert
                Assert.That(result, Is.EqualTo(3));
            }

            [Test]
            public void Bind3_GivenParam_ReturnsUnaryFunctionReturningResult()
            {
                // Arrange
                Func<int, int, int, int> func = (x, y, z) => x + y + z;

                // Act
                var boundFunc = func.Bind(1);
                var result = boundFunc.Invoke(2, 3);

                // Assert
                Assert.That(result, Is.EqualTo(6));
            }

            [Test]
            public void Bind4_GivenParam_ReturnsUnaryFunctionReturningResult()
            {
                // Arrange
                Func<int, int, int, int, int> func = (x, y, z, w) => x + y + z + w;

                // Act
                var boundFunc = func.Bind(1);
                var result = boundFunc.Invoke(2, 3, 4);

                // Assert
                Assert.That(result, Is.EqualTo(10));
            }

            [Test]
            public void Bind5_GivenParam_ReturnsUnaryFunctionReturningResult()
            {
                // Arrange
                Func<int, int, int, int, int, int> func = (x, y, z, w, v) => x + y + z + w + v;

                // Act
                var boundFunc = func.Bind(1);
                var result = boundFunc.Invoke(2, 3, 4, 5);

                // Assert
                Assert.That(result, Is.EqualTo(15));
            }

            [Test]
            public void Bind6_GivenParam_ReturnsUnaryFunctionReturningResult()
            {
                // Arrange
                Func<int, int, int, int, int, int, int> func = (x, y, z, w, v, u) => x + y + z + w + v + u;

                // Act
                var boundFunc = func.Bind(1);
                var result = boundFunc.Invoke(2, 3, 4, 5, 6);

                // Assert
                Assert.That(result, Is.EqualTo(21));
            }
        }

    }
}
