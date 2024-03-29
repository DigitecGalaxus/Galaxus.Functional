using NUnit.Framework;

namespace Galaxus.Functional.Tests.Option;

public class OptionTests_Filter
{
    [Test]
    public void Option_FilterValueOnNone_ReturnsNone()
    {
        var option = Option<string>.None;
        option = option.Filter(val => val == "hello");
        Assert.IsTrue(option.IsNone);
    }

    [Test]
    public void Option_FilterValueOnSome_ReturnsSome()
    {
        var option = "hello".ToOption();
        option = option.Filter(val => val == "hello");
        Assert.IsTrue(option.IsSome);
    }

    [Test]
    public void Option_FilterValueOnSome_ReturnsNone()
    {
        var option = "world".ToOption();
        option = option.Filter(val => val == "hello");
        Assert.IsTrue(option.IsNone);
    }
}
