using NUnit.Framework;

namespace Galaxus.Functional.Tests.OptionExtensions;

[TestFixture]
public class ToNullableTests
{
    [Test]
    public void SomeValueToNullable_ReturnsValue()
    {
        Assert.AreEqual(42, 42.ToOption().ToNullable());
    }

    [Test]
    public void NoneValueToNullable_ReturnsNull()
    {
        Assert.AreEqual(default(int?), Option<int>.None.ToNullable());
    }

    [Test]
    public void SomeReferenceToNullable_ReturnsNotNullReference()
    {
        Assert.AreEqual("hello", "hello".ToOption().ToNullable());
    }

    [Test]
    public void NoneReferenceToNullable_ReturnsNullReference()
    {
        Assert.AreEqual(null, Option<string>.None.ToNullable());
    }
}
