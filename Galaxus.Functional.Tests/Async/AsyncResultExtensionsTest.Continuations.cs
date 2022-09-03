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

    private Result<string, string> StoreValueAndReturnOk(string value)
    {
        StoreValue(value);

        return Result<string, string>.FromOk(value);
    }

    private Task<Result<string, string>> StoreValueAndReturnOkAsync(string value)
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

    private static Result<string, string> AppendPeriodAndReturnOk(string value)
    {
        return Result<string, string>.FromOk(AppendPeriod(value));
    }

    private static Task<Result<string, string>> AppendPeriodAndReturnOkAsync(string value)
    {
        return Task.FromResult(AppendPeriodAndReturnOk(value));
    }
}
