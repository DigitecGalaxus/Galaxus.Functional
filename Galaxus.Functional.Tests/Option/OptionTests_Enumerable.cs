using System.Linq;
using NUnit.Framework;

namespace Galaxus.Functional.Tests.Option;

public class OptionTests_Enumerable
{
    [Test]
    public void Option_EnumerateNone_ContainsNoItems()
    {
        Assert.IsEmpty(Option<string>.None.ToArray());
    }

    [Test]
    public void Option_EnumerateSome_ContainsOneItem()
    {
        Assert.AreEqual(1, "hello".ToOption().ToArray().Length);
    }

    [Test]
    public void Option_EnumerateSome_ContainsCorrectItem()
    {
        Assert.AreEqual("hello", "hello".ToOption().ToArray()[0]);
    }
}
