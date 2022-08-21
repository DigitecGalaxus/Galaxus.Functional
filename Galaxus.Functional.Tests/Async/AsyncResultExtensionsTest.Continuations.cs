using System.Threading.Tasks;

namespace Galaxus.Functional.Tests.Async;

internal partial class AsyncResultExtensionsTest
{
    private void StoreValue(string value)
    {
        _storedValue = value;
    }

    private Task StoreValueAsync(string value)
    {
        StoreValue(value);

        return Task.CompletedTask;
    }

    private Result<string, string> StoreValueAndReturnWrapped(string value)
    {
        StoreValue(value);

        return Result<string, string>.FromOk(value);
    }

    private Task<Result<string, string>> StoreValueAndReturnWrappedAsync(string value)
    {
        StoreValue(value);

        return Task.FromResult(Result<string, string>.FromOk(value));
    }

    private static string AppendPeriod(string value)
    {
        return value + ".";
    }

    private static Task<string> AppendPeriodAsync(string value)
    {
        return Task.FromResult(AppendPeriod(value));
    }
}
