using System;
using System.Threading.Tasks;

namespace Galaxus.Functional;

/// <summary>
///     Extensions to common operations for <see cref="Either{A,B}" /> and <see cref="Either{A,B,C}" /> using async methods or <see cref="Task" />s.
/// </summary>
public static class AsyncEitherExtensions
{
    /// <inheritdoc cref="Either{A,B}.Match" />
    public static async Task<T> MatchAsync<A, B, T>(this Either<A, B> self, Func<A, Task<T>> onA, Func<B, Task<T>> onB)
    {
        return await self.Match(onA, onB);
    }

    /// <inheritdoc cref="Either{A,B}.Match" />
    public static async Task<T> MatchAsync<A, B, T>(this Either<A, B> self, Func<A, Task<T>> onA, Func<B, T> onB)
    {
        return await self.Match(onA, b => Task.FromResult(onB(b)));
    }

    /// <inheritdoc cref="Either{A,B}.Match" />
    public static async Task<T> MatchAsync<A, B, T>(this Either<A, B> self, Func<A, T> onA, Func<B, Task<T>> onB)
    {
        return await self.Match(a => Task.FromResult(onA(a)), onB);
    }

    /// <inheritdoc cref="Either{A,B,C}.Match" />
    public static async Task<T> MatchAsync<A, B, C, T>(this Either<A, B, C> self, Func<A, Task<T>> onA, Func<B, Task<T>> onB, Func<C, Task<T>> onC)
    {
        return await self.Match(onA, onB, onC);
    }

    /// <inheritdoc cref="Either{A,B,C}.Match" />
    public static async Task<T> MatchAsync<A, B, C, T>(this Either<A, B, C> self, Func<A, Task<T>> onA, Func<B, Task<T>> onB, Func<C, T> onC)
    {
        return await self.Match(onA, onB, c => Task.FromResult(onC(c)));
    }

    /// <inheritdoc cref="Either{A,B,C}.Match" />
    public static async Task<T> MatchAsync<A, B, C, T>(this Either<A, B, C> self, Func<A, Task<T>> onA, Func<B, T> onB, Func<C, Task<T>> onC)
    {
        return await self.Match(onA, b => Task.FromResult(onB(b)), onC);
    }

    /// <inheritdoc cref="Either{A,B,C}.Match" />
    public static async Task<T> MatchAsync<A, B, C, T>(this Either<A, B, C> self, Func<A, Task<T>> onA, Func<B, T> onB, Func<C, T> onC)
    {
        return await self.Match(onA, b => Task.FromResult(onB(b)), c => Task.FromResult(onC(c)));
    }

    /// <inheritdoc cref="Either{A,B,C}.Match" />
    public static async Task<T> MatchAsync<A, B, C, T>(this Either<A, B, C> self, Func<A, T> onA, Func<B, Task<T>> onB, Func<C, Task<T>> onC)
    {
        return await self.Match(a => Task.FromResult(onA(a)), onB, onC);
    }

    /// <inheritdoc cref="Either{A,B,C}.Match" />
    public static async Task<T> MatchAsync<A, B, C, T>(this Either<A, B, C> self, Func<A, T> onA, Func<B, Task<T>> onB, Func<C, T> onC)
    {
        return await self.Match(a => Task.FromResult(onA(a)), onB, c => Task.FromResult(onC(c)));
    }

    /// <inheritdoc cref="Either{A,B,C}.Match" />
    public static async Task<T> MatchAsync<A, B, C, T>(this Either<A, B, C> self, Func<A, T> onA, Func<B, T> onB, Func<C, Task<T>> onC)
    {
        return await self.Match(a => Task.FromResult(onA(a)), b => Task.FromResult(onB(b)), onC);
    }
}
