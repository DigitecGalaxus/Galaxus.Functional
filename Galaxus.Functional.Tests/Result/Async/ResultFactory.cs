using System.Threading.Tasks;

namespace Galaxus.Functional.Tests.Result.Async;

internal static class ResultFactory
{
    public static Result<string, string> CreateOk(string value)
    {
        return Result<string, string>.FromOk(value);
    }

    public static Result<string, string> CreateErr(string value)
    {
        return Result<string, string>.FromErr(value);
    }

    // Don't change the suffixes here to Async - IDEs will nag you about using the async variant of e.g. CreateOk.
    public static Task<Result<string, string>> CreateOkTask(string value)
    {
        return Task.FromResult(Result<string, string>.FromOk(value));
    }

    public static Task<Result<string, string>> CreateErrTask(string value)
    {
        return Task.FromResult(Result<string, string>.FromErr(value));
    }
}
