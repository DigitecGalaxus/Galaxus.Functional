using System;
using System.Threading.Tasks;

namespace Galaxus.Functional;

/// <summary>
///     Extensions to common operations for <see cref="Option{T}" /> using async methods or <see cref="Task" />s.
/// </summary>
public static partial class AsyncOptionExtensions
{
    /// <inheritdoc cref="Option{T}.Unwrap()" />
    public static async Task<T> UnwrapAsync<T>(this Task<Option<T>> self)
    {
        return (await self.ConfigureAwait(false)).Unwrap();
    }

    /// <inheritdoc cref="Option{T}.Unwrap(string)" />
    public static async Task<T> UnwrapAsync<T>(this Task<Option<T>> self, string error)
    {
        return (await self.ConfigureAwait(false)).Unwrap(error);
    }

    /// <inheritdoc cref="Option{T}.Unwrap(Func{string})" />
    public static async Task<T> UnwrapAsync<T>(this Task<Option<T>> self, Func<string> error)
    {
        return (await self.ConfigureAwait(false)).Unwrap(error);
    }

    /// <inheritdoc cref="Option{T}.Unwrap(string)" />
    public static async Task<T> UnwrapAsync<T>(this Task<Option<T>> self, Func<Task<string>> error)
    {
        var option = await self.ConfigureAwait(false);
        if (option.IsNone)
        {
            if (error is null)
            {
                throw new ArgumentNullException(nameof(error));
            }

            throw new TriedToUnwrapNoneException(await error());
        }

        return option.Unwrap();
    }

    /// <inheritdoc cref="Option{T}.UnwrapOr" />
    public static async Task<T> UnwrapOrAsync<T>(this Task<Option<T>> self, T fallback)
    {
        return (await self.ConfigureAwait(false)).UnwrapOr(fallback);
    }

    /// <inheritdoc cref="Option{T}.UnwrapOr" />
    public static async Task<T> UnwrapOrAsync<T>(this Task<Option<T>> self, Task<T> fallback)
    {
        return (await self.ConfigureAwait(false)).UnwrapOr(await fallback);
    }

    /// <inheritdoc cref="Option{T}.UnwrapOrElse" />
    public static async Task<T> UnwrapOrElseAsync<T>(this Task<Option<T>> self, Func<T> fallback)
    {
        return (await self.ConfigureAwait(false)).UnwrapOrElse(fallback);
    }

    /// <inheritdoc cref="Option{T}.UnwrapOrElse" />
    public static async Task<T> UnwrapOrElseAsync<T>(this Task<Option<T>> self, Func<Task<T>> fallback)
    {
        var option = await self.ConfigureAwait(false);
        if (option.IsNone)
        {
            if (fallback is null)
            {
                throw new ArgumentNullException(nameof(fallback));
            }

            return await fallback().ConfigureAwait(false);
        }

        return option.Unwrap();
    }

    /// <inheritdoc cref="Option{T}.Match" />
    public static async Task MatchAsync<T>(this Option<T> self, Func<T, Task> onSome, Func<Task> onNone)
    {
        await self.Match(onSome, onNone).ConfigureAwait(false);
    }

    /// <inheritdoc cref="Option{T}.Match" />
    public static async Task MatchAsync<T>(this Task<Option<T>> task, Func<T, Task> onSome, Func<Task> onNone)
    {
        var option = await task.ConfigureAwait(false);
        await option.MatchAsync(onSome, onNone).ConfigureAwait(false);
    }

    /// <inheritdoc cref="Option{T}.Match" />
    public static async Task MatchAsync<T>(this Option<T> self, Action<T> onSome, Func<Task> onNone)
    {
        await self.MatchAsync(t =>
        {
            onSome(t);
            return Task.CompletedTask;
        }, onNone).ConfigureAwait(false);
    }

    /// <inheritdoc cref="Option{T}.Match" />
    public static async Task MatchAsync<T>(this Task<Option<T>> task, Action<T> onSome, Func<Task> onNone)
    {
        var option = await task.ConfigureAwait(false);
        await option.MatchAsync(onSome, onNone).ConfigureAwait(false);
    }

    /// <inheritdoc cref="Option{T}.Match" />
    public static async Task MatchAsync<T>(this Option<T> self, Func<T, Task> onSome, Action onNone)
    {
        await self.MatchAsync(onSome, async () =>
        {
            onNone();
            await Task.CompletedTask;
        }).ConfigureAwait(false);
    }

    /// <inheritdoc cref="Option{T}.Match" />
    public static async Task MatchAsync<T>(this Task<Option<T>> task, Func<T, Task> onSome, Action onNone)
    {
        var option = await task.ConfigureAwait(false);
        await option.MatchAsync(onSome, onNone).ConfigureAwait(false);
    }

    /// <inheritdoc cref="Option{T}.Match" />
    public static async Task MatchAsync<T>(this Task<Option<T>> task, Action<T> onSome, Action onNone)
    {
        var option = await task.ConfigureAwait(false);
        option.Match(onSome, onNone);
    }

    /// <inheritdoc cref="Option{T}.Match{U}" />
    public static async Task<U> MatchAsync<T, U>(this Option<T> self, Func<T, Task<U>> onSome, Func<Task<U>> onNone)
    {
        return await self.Match(onSome, onNone).ConfigureAwait(false);
    }

    /// <inheritdoc cref="Option{T}.Match{U}" />
    public static async Task<U> MatchAsync<T, U>(this Task<Option<T>> self, Func<T, Task<U>> onSome, Func<Task<U>> onNone)
    {
        var option = await self.ConfigureAwait(false);
        return await option.MatchAsync(onSome, onNone).ConfigureAwait(false);
    }

    /// <inheritdoc cref="Option{T}.Match{U}" />
    public static async Task<U> MatchAsync<T, U>(this Option<T> self, Func<T, U> onSome, Func<Task<U>> onNone)
    {
        return await self.MatchAsync(t => Task.FromResult(onSome(t)), onNone).ConfigureAwait(false);
    }

    /// <inheritdoc cref="Option{T}.Match{U}" />
    public static async Task<U> MatchAsync<T, U>(this Task<Option<T>> self, Func<T, U> onSome, Func<Task<U>> onNone)
    {
        var option = await self.ConfigureAwait(false);
        return await option.MatchAsync(onSome, onNone).ConfigureAwait(false);
    }

    /// <inheritdoc cref="Option{T}.Match{U}" />
    public static async Task<U> MatchAsync<T, U>(this Option<T> self, Func<T, Task<U>> onSome, Func<U> onNone)
    {
        return await self.MatchAsync(onSome, () => Task.FromResult(onNone())).ConfigureAwait(false);
    }

    /// <inheritdoc cref="Option{T}.Match{U}" />
    public static async Task<U> MatchAsync<T, U>(this Task<Option<T>> self, Func<T, Task<U>> onSome, Func<U> onNone)
    {
        var option = await self.ConfigureAwait(false);
        return await option.MatchAsync(onSome, onNone).ConfigureAwait(false);
    }

    /// <inheritdoc cref="Option{T}.Match{U}" />
    public static async Task<U> MatchAsync<T, U>(this Task<Option<T>> self, Func<T, U> onSome, Func<U> onNone)
    {
        var option = await self.ConfigureAwait(false);
        return option.Match(onSome, onNone);
    }
}
