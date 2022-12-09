using NUnit.Framework;

namespace Galaxus.Functional.Tests.Option.OptionExtensions;

[TestFixture]
public class AutoMapTests
{
    [Test]
    public void Map_Some_InnerValueRemains()
    {
        Assert.AreEqual(true, true.ToOption().Map<bool, object>().Unwrap());
        Assert.AreEqual(false, false.ToOption().Map<bool, object>().Unwrap());
        Assert.AreEqual("hello", "hello".ToOption().Map<string, object>().Unwrap());
    }

    [Test]
    public void Map_None_RemainsNone()
    {
        Assert.IsTrue(Option<string>.None.Map<string, object>().IsNone);
    }
}
