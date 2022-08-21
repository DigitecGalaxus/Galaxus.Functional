using System.Threading.Tasks;

namespace Galaxus.Functional.Tests.Async;

internal partial class AsyncResultExtensionsTest
{
    private static Task StoreValueAsync(string value, out string result)
    {
        result = value;

        return Task.CompletedTask;
    }

    private static Result<string, string> StoreValueAndReturnWrapped(string value, out string result)
    {
        result = value;

        return Result<string, string>.FromOk(value);
    }

    private static Task<Result<string, string>> StoreValueAndReturnWrappedAsync(string value, out string result)
    {
        var returnValue = StoreValueAndReturnWrapped(value, out result);

        return Task.FromResult(returnValue);
    }
}
