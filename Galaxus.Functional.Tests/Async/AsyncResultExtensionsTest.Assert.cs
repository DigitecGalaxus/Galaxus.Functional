using NUnit.Framework;

namespace Galaxus.Functional.Tests.Async;

internal partial class AsyncResultExtensionsTest
{
    private static void AssertOk(Result<string, string> result, string value)
    {
        Assert.That(result.Ok, Is.EqualTo(Option<string>.Some(value)));
    }

    private static void AssertErr(Result<string, string> result, string value)
    {
        Assert.That(result.Err, Is.EqualTo(Option<string>.Some(value)));
    }
}
