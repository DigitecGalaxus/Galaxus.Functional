using System;
using System.Threading.Tasks;

namespace Galaxus.Functional;

/// <summary>
///     Extensions to common operations for <see cref="Result{TOk,TErr}" /> using async methods or <see cref="Task" />s.
/// </summary>
public static class AsyncResultExtensions
{
    /// <inheritdoc cref="Result{TOk,TErr}.IfOk" />
    public static async Task IfOkAsync<TOk, TErr>(this Result<TOk, TErr> self, Func<TOk, Task> onOk)
    {
        await self.Match(onOk, _ => Task.CompletedTask).ConfigureAwait(false);
    }

    /// <inheritdoc cref="Result{TOk,TErr}.IfErr" />
    public static async Task IfErrAsync<TOk, TErr>(this Result<TOk, TErr> self, Func<TErr, Task> onErr)
    {
        await self.Match(_ => Task.CompletedTask, onErr).ConfigureAwait(false);
    }

    /// <inheritdoc cref="Result{TOk,TErr}.AndThen" />
    public static async Task<Result<TContinuation, TErr>> AndThenAsync<TOk, TErr, TContinuation>(
        this Result<TOk, TErr> self,
        Func<TOk, Task<Result<TContinuation, TErr>>> continuation)
    {
        return await self.Match(continuation, err => Task.FromResult(Result<TContinuation, TErr>.FromErr(err))).ConfigureAwait(false);
    }

    /// <inheritdoc cref="Result{TOk,TErr}.AndThen" />
    public static async Task<Result<TContinuation, TErr>> AndThenAsync<TOk, TErr, TContinuation>(
        this Task<Result<TOk, TErr>> self,
        Func<TOk, Result<TContinuation, TErr>> continuation)
    {
        return (await self.ConfigureAwait(false)).AndThen(continuation);
    }

    /// <inheritdoc cref="Result{TOk,TErr}.AndThen" />
    public static async Task<Result<TContinuation, TErr>> AndThenAsync<TOk, TErr, TContinuation>(
        this Task<Result<TOk, TErr>> self,
        Func<TOk, Task<Result<TContinuation, TErr>>> continuation)
    {
        return await (await self.ConfigureAwait(false)).AndThenAsync(continuation).ConfigureAwait(false);
    }

    /// <inheritdoc cref="Result{TOk,TErr}.Map{TOkTo}" />
    public static async Task<Result<TOkTo, TErr>> MapAsync<TOk, TErr, TOkTo>(
        this Result<TOk, TErr> self,
        Func<TOk, Task<TOkTo>> continuation)
    {
        return await self.Match(
            async ok => (await continuation(ok).ConfigureAwait(false)).ToOk<TOkTo, TErr>(),
            err => Task.FromResult(err.ToErr<TOkTo, TErr>())).ConfigureAwait(false);
    }

    /// <inheritdoc cref="Result{TOk,TErr}.Map{TOkTo}" />
    public static async Task<Result<TOkTo, TErr>> MapAsync<TOk, TErr, TOkTo>(
        this Task<Result<TOk, TErr>> self,
        Func<TOk, TOkTo> continuation)
    {
        return (await self.ConfigureAwait(false)).Map(continuation);
    }

    /// <inheritdoc cref="Result{TOk,TErr}.Map{TOkTo}" />
    public static async Task<Result<TOkTo, TErr>> MapAsync<TOk, TErr, TOkTo>(
        this Task<Result<TOk, TErr>> self,
        Func<TOk, Task<TOkTo>> continuation)
    {
        return await (await self.ConfigureAwait(false)).MapAsync(continuation).ConfigureAwait(false);
    }

    /// <inheritdoc cref="Result{TOk,TErr}.MapErr{TErrTo}" />
    public static async Task<Result<TOk, TErrTo>> MapErrAsync<TOk, TErr, TErrTo>(
        this Result<TOk, TErr> self,
        Func<TErr, Task<TErrTo>> continuation)
    {
        return await self.Match(
            ok => Task.FromResult(ok.ToOk<TOk, TErrTo>()),
            async err => (await continuation(err).ConfigureAwait(false)).ToErr<TOk, TErrTo>()).ConfigureAwait(false);
    }

    /// <inheritdoc cref="Result{TOk,TErr}.Map{TOkTo}" />
    public static async Task<Result<TOk, TErrTo>> MapErrAsync<TOk, TErr, TErrTo>(
        this Task<Result<TOk, TErr>> self,
        Func<TErr, TErrTo> continuation)
    {
        return (await self.ConfigureAwait(false)).MapErr(continuation);
    }

    /// <inheritdoc cref="Result{TOk,TErr}.Map{TOkTo}" />
    public static async Task<Result<TOk, TErrTo>> MapErrAsync<TOk, TErr, TErrTo>(
        this Task<Result<TOk, TErr>> self,
        Func<TErr, Task<TErrTo>> continuation)
    {
        return await (await self.ConfigureAwait(false)).MapErrAsync(continuation).ConfigureAwait(false);
    }

    /// <inheritdoc cref="Result{TOk,TErr}.OrElse{TContinuationErr}" />
    public static async Task<Result<TOk, TContinuationErr>> OrElseAsync<TOk, TErr, TContinuationErr>(
        this Result<TOk, TErr> self,
        Func<TErr, Task<Result<TOk, TContinuationErr>>> continuation)
    {
        return await self.MatchAsync(ok => Result<TOk, TContinuationErr>.FromOk(ok), continuation).ConfigureAwait(false);
    }

    /// <inheritdoc cref="Result{TOk,TErr}.OrElse{TContinuationErr}" />
    public static async Task<Result<TOk, TContinuationErr>> OrElseAsync<TOk, TContinuationErr>(
        this Task<Result<TOk, TContinuationErr>> self,
        Func<TContinuationErr, Result<TOk, TContinuationErr>> continuation)
    {
        return (await self.ConfigureAwait(false)).OrElse(continuation);
    }

    /// <inheritdoc cref="Result{TOk,TErr}.OrElse{TContinuationErr}" />
    public static async Task<Result<TOk, TContinuationErr>> OrElseAsync<TOk, TContinuationErr>(
        this Task<Result<TOk, TContinuationErr>> self,
        Func<TContinuationErr, Task<Result<TOk, TContinuationErr>>> continuation)
    {
        return await (await self.ConfigureAwait(false)).OrElseAsync(continuation).ConfigureAwait(false);
    }

    /// <inheritdoc cref="Result{TOk,TErr}.Match{TResult}" />
    public static async Task<TResult> MatchAsync<TOk, TErr, TResult>(
        this Result<TOk, TErr> self,
        Func<TOk, Task<TResult>> onOk,
        Func<TErr, Task<TResult>> onErr)
    {
        return await self.Match(onOk, onErr).ConfigureAwait(false);
    }

    /// <inheritdoc cref="Result{TOk,TErr}.Match{TResult}" />
    public static async Task<TResult> MatchAsync<TOk, TErr, TResult>(
        this Result<TOk, TErr> self,
        Func<TOk, TResult> onOk,
        Func<TErr, Task<TResult>> onErr)
    {
        return await self.Match(ok => Task.FromResult(onOk(ok)), onErr).ConfigureAwait(false);
    }

    /// <inheritdoc cref="Result{TOk,TErr}.Match{TResult}" />
    public static async Task<TResult> MatchAsync<TOk, TErr, TResult>(
        this Result<TOk, TErr> self,
        Func<TOk, Task<TResult>> onOk,
        Func<TErr, TResult> onErr)
    {
        return await self.Match(onOk, err => Task.FromResult(onErr(err))).ConfigureAwait(false);
    }

    /// <inheritdoc cref="Result{TOk,TErr}.Match{TResult}" />
    public static async Task<TResult> MatchAsync<TOk, TErr, TResult>(
        this Task<Result<TOk, TErr>> self,
        Func<TOk, Task<TResult>> onOk,
        Func<TErr, Task<TResult>> onErr)
    {
        return await (await self.ConfigureAwait(false)).Match(onOk, onErr).ConfigureAwait(false);
    }

    /// <inheritdoc cref="Result{TOk,TErr}.Match{TResult}" />
    public static async Task<TResult> MatchAsync<TOk, TErr, TResult>(
        this Task<Result<TOk, TErr>> self,
        Func<TOk, TResult> onOk,
        Func<TErr, Task<TResult>> onErr)
    {
        return await (await self.ConfigureAwait(false)).Match(ok => Task.FromResult(onOk(ok)), onErr).ConfigureAwait(false);
    }

    /// <inheritdoc cref="Result{TOk,TErr}.Match{TResult}" />
    public static async Task<TResult> MatchAsync<TOk, TErr, TResult>(
        this Task<Result<TOk, TErr>> self,
        Func<TOk, Task<TResult>> onOk,
        Func<TErr, TResult> onErr)
    {
        return await (await self.ConfigureAwait(false)).Match(onOk, err => Task.FromResult(onErr(err))).ConfigureAwait(false);
    }

    /// <inheritdoc cref="Result{TOk,TErr}.Match{TResult}" />
    public static async Task<TResult> MatchAsync<TOk, TErr, TResult>(
        this Task<Result<TOk, TErr>> self,
        Func<TOk, TResult> onOk,
        Func<TErr, TResult> onErr)
    {
        return (await self.ConfigureAwait(false)).Match(onOk, onErr);
    }

    /// <inheritdoc cref="Result{TOk,TErr}.Unwrap()" />
    public static async Task<TOk> UnwrapAsync<TOk, TErr>(this Task<Result<TOk, TErr>> self)
    {
        return (await self.ConfigureAwait(false)).Unwrap();
    }

    /// <inheritdoc cref="Result{TOk,TErr}.Unwrap(string)" />
    public static async Task<TOk> UnwrapAsync<TOk, TErr>(this Task<Result<TOk, TErr>> self, string error)
    {
        return (await self.ConfigureAwait(false)).Unwrap(error);
    }

    /// <inheritdoc cref="Result{TOk,TErr}.Unwrap(Func{TErr, string})" />
    public static async Task<TOk> UnwrapAsync<TOk, TErr>(this Task<Result<TOk, TErr>> self, Func<TErr, string> error)
    {
        return (await self.ConfigureAwait(false)).Unwrap(error);
    }

    /// <inheritdoc cref="Result{TOk,TErr}.Unwrap(Func{TErr, string})" />
    public static async Task<TOk> UnwrapAsync<TOk, TErr>(this Task<Result<TOk, TErr>> self, Func<TErr, Task<string>> error)
    {
        error ??= _ => throw new ArgumentNullException(nameof(error));
        return await self.MatchAsync(
            ok => ok,
            async err => throw new TriedToUnwrapErrException(await error(err))).ConfigureAwait(false);
    }

    /// <inheritdoc cref="Result{TOk,TErr}.UnwrapOr" />
    public static async Task<TOk> UnwrapOrAsync<TOk, TErr>(this Task<Result<TOk, TErr>> self, TOk fallback)
    {
        return (await self.ConfigureAwait(false)).UnwrapOr(fallback);
    }

    /// <inheritdoc cref="Result{TOk,TErr}.UnwrapOr" />
    public static async Task<TOk> UnwrapOrAsync<TOk, TErr>(this Task<Result<TOk, TErr>> self, Task<TOk> fallback)
    {
        return (await self.ConfigureAwait(false)).UnwrapOr(await fallback);
    }

    /// <inheritdoc cref="Result{TOk,TErr}.UnwrapOrElse" />
    public static async Task<TOk> UnwrapOrElseAsync<TOk, TErr>(this Task<Result<TOk, TErr>> self, Func<TOk> fallback)
    {
        return (await self.ConfigureAwait(false)).UnwrapOrElse(fallback);
    }

    /// <inheritdoc cref="Result{TOk,TErr}.UnwrapOrElse" />
    public static async Task<TOk> UnwrapOrElseAsync<TOk, TErr>(this Task<Result<TOk, TErr>> self, Func<Task<TOk>> fallback)
    {
        fallback ??= () => throw new ArgumentNullException(nameof(fallback));
        return await self.MatchAsync(ok => ok, async _ => await fallback().ConfigureAwait(false)).ConfigureAwait(false);
    }
}
