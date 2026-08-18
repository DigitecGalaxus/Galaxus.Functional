using System;
using System.Threading.Tasks;

namespace Galaxus.Functional;

/// <summary>
///     Extensions to common operations for <see cref="Either{A,B}" /> and <see cref="Either{A,B,C}" /> using async methods or <see cref="Task" />s.
/// </summary>
public static partial class AsyncEitherExtensions
{
    /// <inheritdoc cref="Either{A,B}.Match" />
    public static async Task MatchAsync<A, B>(this Either<A, B> self, Func<A, Task> onA, Func<B, Task> onB)
    {
        await self.Match(onA, onB).ConfigureAwait(false);
    }

    /// <inheritdoc cref="Either{A,B,C}.Match" />
    public static async Task MatchAsync<A, B, C>(this Either<A, B, C> self, Func<A, Task> onA, Func<B, Task> onB, Func<C, Task> onC)
    {
        await self.Match(onA, onB, onC).ConfigureAwait(false);
    }

    /// <inheritdoc cref="Either{A,B}.Match" />
    public static async Task MatchAsync<A, B>(this Either<A, B> self, Action<A> onA, Func<B, Task> onB)
    {
        await self.Match(Awaited(onA, nameof(onA)), onB).ConfigureAwait(false);
    }

    /// <inheritdoc cref="Either{A,B}.Match" />
    public static async Task MatchAsync<A, B>(this Either<A, B> self, Func<A, Task> onA, Action<B> onB)
    {
        await self.Match(onA, Awaited(onB, nameof(onB))).ConfigureAwait(false);
    }

    /// <inheritdoc cref="Either{A,B,C}.Match" />
    public static async Task MatchAsync<A, B, C>(this Either<A, B, C> self, Action<A> onA, Func<B, Task> onB, Func<C, Task> onC)
    {
        await self.Match(Awaited(onA, nameof(onA)), onB, onC).ConfigureAwait(false);
    }

    /// <inheritdoc cref="Either{A,B,C}.Match" />
    public static async Task MatchAsync<A, B, C>(this Either<A, B, C> self, Func<A, Task> onA, Action<B> onB, Func<C, Task> onC)
    {
        await self.Match(onA, Awaited(onB, nameof(onB)), onC).ConfigureAwait(false);
    }

    /// <inheritdoc cref="Either{A,B,C}.Match" />
    public static async Task MatchAsync<A, B, C>(this Either<A, B, C> self, Func<A, Task> onA, Func<B, Task> onB, Action<C> onC)
    {
        await self.Match(onA, onB, Awaited(onC, nameof(onC))).ConfigureAwait(false);
    }

    /// <inheritdoc cref="Either{A,B,C}.Match" />
    public static async Task MatchAsync<A, B, C>(this Either<A, B, C> self, Action<A> onA, Action<B> onB, Func<C, Task> onC)
    {
        await self.Match(Awaited(onA, nameof(onA)), Awaited(onB, nameof(onB)), onC).ConfigureAwait(false);
    }

    /// <inheritdoc cref="Either{A,B,C}.Match" />
    public static async Task MatchAsync<A, B, C>(this Either<A, B, C> self, Action<A> onA, Func<B, Task> onB, Action<C> onC)
    {
        await self.Match(Awaited(onA, nameof(onA)), onB, Awaited(onC, nameof(onC))).ConfigureAwait(false);
    }

    /// <inheritdoc cref="Either{A,B,C}.Match" />
    public static async Task MatchAsync<A, B, C>(this Either<A, B, C> self, Func<A, Task> onA, Action<B> onB, Action<C> onC)
    {
        await self.Match(onA, Awaited(onB, nameof(onB)), Awaited(onC, nameof(onC))).ConfigureAwait(false);
    }

    private static Func<T, Task> Awaited<T>(Action<T> continuation, string parameterName)
    {
        return value =>
        {
            if (continuation is null)
            {
                throw new ArgumentNullException(parameterName);
            }

            continuation(value);
            return Task.CompletedTask;
        };
    }

    /// <inheritdoc cref="Either{A,B}.Match" />
    public static async Task<T> MatchAsync<A, B, T>(this Either<A, B> self, Func<A, Task<T>> onA, Func<B, Task<T>> onB)
    {
        return await self.Match(onA, onB).ConfigureAwait(false);
    }

    /// <inheritdoc cref="Either{A,B}.Match" />
    public static async Task<T> MatchAsync<A, B, T>(this Either<A, B> self, Func<A, Task<T>> onA, Func<B, T> onB)
    {
        return await self.Match(onA, b => Task.FromResult(onB(b))).ConfigureAwait(false);
    }

    /// <inheritdoc cref="Either{A,B}.Match" />
    public static async Task<T> MatchAsync<A, B, T>(this Either<A, B> self, Func<A, T> onA, Func<B, Task<T>> onB)
    {
        return await self.Match(a => Task.FromResult(onA(a)), onB).ConfigureAwait(false);
    }

    /// <inheritdoc cref="Either{A,B,C}.Match" />
    public static async Task<T> MatchAsync<A, B, C, T>(this Either<A, B, C> self, Func<A, Task<T>> onA, Func<B, Task<T>> onB, Func<C, Task<T>> onC)
    {
        return await self.Match(onA, onB, onC).ConfigureAwait(false);
    }

    /// <inheritdoc cref="Either{A,B,C}.Match" />
    public static async Task<T> MatchAsync<A, B, C, T>(this Either<A, B, C> self, Func<A, Task<T>> onA, Func<B, Task<T>> onB, Func<C, T> onC)
    {
        return await self.Match(onA, onB, c => Task.FromResult(onC(c))).ConfigureAwait(false);
    }

    /// <inheritdoc cref="Either{A,B,C}.Match" />
    public static async Task<T> MatchAsync<A, B, C, T>(this Either<A, B, C> self, Func<A, Task<T>> onA, Func<B, T> onB, Func<C, Task<T>> onC)
    {
        return await self.Match(onA, b => Task.FromResult(onB(b)), onC).ConfigureAwait(false);
    }

    /// <inheritdoc cref="Either{A,B,C}.Match" />
    public static async Task<T> MatchAsync<A, B, C, T>(this Either<A, B, C> self, Func<A, Task<T>> onA, Func<B, T> onB, Func<C, T> onC)
    {
        return await self.Match(onA, b => Task.FromResult(onB(b)), c => Task.FromResult(onC(c))).ConfigureAwait(false);
    }

    /// <inheritdoc cref="Either{A,B,C}.Match" />
    public static async Task<T> MatchAsync<A, B, C, T>(this Either<A, B, C> self, Func<A, T> onA, Func<B, Task<T>> onB, Func<C, Task<T>> onC)
    {
        return await self.Match(a => Task.FromResult(onA(a)), onB, onC).ConfigureAwait(false);
    }

    /// <inheritdoc cref="Either{A,B,C}.Match" />
    public static async Task<T> MatchAsync<A, B, C, T>(this Either<A, B, C> self, Func<A, T> onA, Func<B, Task<T>> onB, Func<C, T> onC)
    {
        return await self.Match(a => Task.FromResult(onA(a)), onB, c => Task.FromResult(onC(c))).ConfigureAwait(false);
    }

    /// <inheritdoc cref="Either{A,B,C}.Match" />
    public static async Task<T> MatchAsync<A, B, C, T>(this Either<A, B, C> self, Func<A, T> onA, Func<B, T> onB, Func<C, Task<T>> onC)
    {
        return await self.Match(a => Task.FromResult(onA(a)), b => Task.FromResult(onB(b)), onC).ConfigureAwait(false);
    }
}
