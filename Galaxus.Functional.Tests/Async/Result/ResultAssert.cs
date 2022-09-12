using NUnit.Framework;

namespace Galaxus.Functional.Tests.Async.Result;

internal static class ResultAssert
{
    public static void IsOk(string expectedValue, Result<string, string> actualResult)
    {
        Assert.That(actualResult.Ok.UnwrapOr(null), Is.EqualTo(expectedValue));
    }

    public static void IsErr(string expectedValue, Result<string, string> actualResult)
    {
        Assert.That(actualResult.Err.UnwrapOr(null), Is.EqualTo(expectedValue));
    }
}
