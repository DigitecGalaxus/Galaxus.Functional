using NUnit.Framework;

namespace Galaxus.Functional.Tests.Async;

internal static class ResultAssert
{
    public static void IsOk(Result<string, string> result, string value)
    {
        Assert.That(result.Ok.UnwrapOr(null), Is.EqualTo(value));
    }

    public static void IsErr(Result<string, string> result, string value)
    {
        Assert.That(result.Err.UnwrapOr(null), Is.EqualTo(value));
    }
}
