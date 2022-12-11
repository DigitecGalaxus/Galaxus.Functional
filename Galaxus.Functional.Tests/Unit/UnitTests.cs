using NUnit.Framework;

namespace Galaxus.Functional.Tests.Unit;

public class UnitTests
{
    [Test]
    public void Unit_AreEqual()
    {
        Assert.AreEqual(Functional.Unit.Value, new Functional.Unit());
        Assert.AreEqual(new Functional.Unit(), new Functional.Unit());
    }

    [Test]
    public void Unit_AreNotEqual()
    {
        Assert.AreNotEqual(Functional.Unit.Value, null);
        Assert.AreNotEqual(Functional.Unit.Value, 0);
    }

    [Test]
    public void Unit_Hash()
    {
        Assert.AreEqual(
            Functional.Unit.Value.GetHashCode(),
            new Functional.Unit().GetHashCode());
    }

    [Test]
    public void Unit_ToString()
    {
        Assert.AreEqual(
            "()",
            Functional.Unit.Value.ToString());
    }
}
