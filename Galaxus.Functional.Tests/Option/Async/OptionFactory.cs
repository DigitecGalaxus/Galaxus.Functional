using System.Threading.Tasks;

namespace Galaxus.Functional.Tests.Option.Async;

internal static class OptionFactory
{
    public static Task<Option<string>> CreateSomeTask(string value)
    {
        return Task.FromResult(Option<string>.Some(value));
    }

    public static Task<Option<string>> CreateNoneTask()
    {
        return Task.FromResult(Option<string>.None);
    }
}
