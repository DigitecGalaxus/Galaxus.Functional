using System.Threading.Tasks;
using NUnit.Framework;

namespace Galaxus.Functional.Tests.Async;

internal partial class AsyncResultExtensionsTest
{
    private string _storedValue;

    [SetUp]
    public void ClearStoredValue()
    {
        _storedValue = null;
    }

    private static Result<string, string> CreateOk(string value)
    {
        return Result<string, string>.FromOk(value);
    }

    private static Task<Result<string, string>> CreateOkTask(string value)
    {
        return Task.FromResult(CreateOk(value));
    }

    private static Result<string, string> CreateErr(string value)
    {
        return Result<string, string>.FromErr(value);
    }

    private static Task<Result<string, string>> CreateErrTask(string value)
    {
        return Task.FromResult(CreateErr(value));
    }
}
