using NUnit.Framework;

namespace Galaxus.Functional.Tests.Option;

public class OptionTests_Contains
{
    [Test]
    public void Option_NoneContainsValue_ReturnsFalse()
    {
        var option = Option<string>.None;
        var contains = option.Contains("hello");
        Assert.IsFalse(contains);
    }

    [Test]
    public void Option_SomeContainsValue_ReturnsFalse()
    {
        var option = "world".ToOption();
        var contains = option.Contains("hello");
        Assert.IsFalse(contains);
    }

    [Test]
    public void Option_SomeContainsValue_ReturnsTrue()
    {
        var option = "hello".ToOption();
        var contains = option.Contains("hello");
        Assert.IsTrue(contains);
    }
}
