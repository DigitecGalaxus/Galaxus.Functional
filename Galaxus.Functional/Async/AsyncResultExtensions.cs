using System;
using System.Threading.Tasks;

namespace Galaxus.Functional.Async
{
    /// <summary>
    ///     Extensions to common operations for <see cref="Result{TOk,TErr}" /> using async methods or <see cref="Task" />s.
    /// </summary>
    public static class AsyncResultExtensions
    {
        /// <inheritdoc cref="Result{TOk,TErr}.IfOk"/>
        public static async Task IfOkAsync<TOk, TErr>(this Result<TOk, TErr> self, Func<TOk, Task> onOk)
        {
            await self.Match(onOk, _ => Task.CompletedTask);
        }

        /// <inheritdoc cref="Result{TOk,TErr}.IfErr"/>
        public static async Task IfErrAsync<TOk, TErr>(this Result<TOk, TErr> self, Func<TErr, Task> onErr)
        {
            await self.Match(_ => Task.CompletedTask, onErr);
        }

        /// <inheritdoc cref="Result{TOk,TErr}.AndThen"/>
        public static async Task<Result<TContinuation, TErr>> AndThenAsync<TOk, TErr, TContinuation>(
            this Result<TOk, TErr> self,
            Func<TOk, Task<Result<TContinuation, TErr>>> continuation)
        {
            return await self.Match(continuation, err => Task.FromResult(Result<TContinuation, TErr>.FromErr(err)));
        }

        /// <inheritdoc cref="Result{TOk,TErr}.AndThen"/>
        public static async Task<Result<TContinuation, TErr>> AndThenAsync<TOk, TErr, TContinuation>(
            this Task<Result<TOk, TErr>> self,
            Func<TOk, Result<TContinuation, TErr>> continuation)
        {
            return (await self).AndThen(continuation);
        }

        /// <inheritdoc cref="Result{TOk,TErr}.AndThen"/>
        public static async Task<Result<TContinuation, TErr>> AndThenAsync<TOk, TErr, TContinuation>(
            this Task<Result<TOk, TErr>> self,
            Func<TOk, Task<Result<TContinuation, TErr>>> continuation)
        {
            return await (await self).AndThenAsync(continuation);
        }

        /// <inheritdoc cref="Result{TOk,TErr}.Map{TOkTo}"/>
        public static async Task<Result<TOkTo, TErr>> MapAsync<TOk, TErr, TOkTo>(
            this Result<TOk, TErr> self,
            Func<TOk, Task<TOkTo>> continuation)
        {
            return await self.Match(
                async ok => (await continuation(ok)).ToOk<TOkTo, TErr>(),
                err => Task.FromResult(err.ToErr<TOkTo, TErr>()));
        }

        /// <inheritdoc cref="Result{TOk,TErr}.Map{TOkTo}"/>
        public static async Task<Result<TOkTo, TErr>> MapAsync<TOk, TErr, TOkTo>(
            this Task<Result<TOk, TErr>> self,
            Func<TOk, TOkTo> continuation)
        {
            return (await self).Map(continuation);
        }

        /// <inheritdoc cref="Result{TOk,TErr}.Map{TOkTo}"/>
        public static async Task<Result<TOkTo, TErr>> MapAsync<TOk, TErr, TOkTo>(
            this Task<Result<TOk, TErr>> self,
            Func<TOk, Task<TOkTo>> continuation)
        {
            return await (await self).MapAsync(continuation);
        }

        /// <inheritdoc cref="Result{TOk,TErr}.MapErr{TErrTo}"/>
        public static async Task<Result<TOk, TErrTo>> MapErrAsync<TOk, TErr, TErrTo>(
            this Result<TOk, TErr> self,
            Func<TErr, Task<TErrTo>> continuation)
        {
            return await self.Match(
                ok => Task.FromResult(ok.ToOk<TOk, TErrTo>()),
                async err => (await continuation(err)).ToErr<TOk, TErrTo>());
        }

        /// <inheritdoc cref="Result{TOk,TErr}.Map{TOkTo}"/>
        public static async Task<Result<TOk, TErrTo>> MapErrAsync<TOk, TErr, TErrTo>(
            this Task<Result<TOk, TErr>> self,
            Func<TErr, TErrTo> continuation)
        {
            return (await self).MapErr(continuation);
        }

        /// <inheritdoc cref="Result{TOk,TErr}.Map{TOkTo}"/>
        public static async Task<Result<TOk, TErrTo>> MapErrAsync<TOk, TErr, TErrTo>(
            this Task<Result<TOk, TErr>> self,
            Func<TErr, Task<TErrTo>> continuation)
        {
            return await (await self).MapErrAsync(continuation);
        }

        /// <inheritdoc cref="Result{TOk,TErr}.OrElse{TContinuationErr}"/>
        public static Task<Result<TOk, TContinuationErr>> OrElseAsync<TOk, TErr, TContinuationErr>(
            this Result<TOk, TErr> self,
            Func<TErr, Task<Result<TOk, TContinuationErr>>> continuation)
        {
            return self.MatchAsync(ok => Result<TOk, TContinuationErr>.FromOk(ok), continuation);
        }

        /// <inheritdoc cref="Result{TOk,TErr}.OrElse{TContinuationErr}"/>
        public static async Task<Result<TOk, TContinuationErr>> OrElseAsync<TOk, TContinuationErr>(
            this Task<Result<TOk, TContinuationErr>> self,
            Func<TContinuationErr, Result<TOk, TContinuationErr>> continuation)
        {
            return (await self).OrElse(continuation);
        }

        /// <inheritdoc cref="Result{TOk,TErr}.OrElse{TContinuationErr}"/>
        public static async Task<Result<TOk, TContinuationErr>> OrElseAsync<TOk, TContinuationErr>(
            this Task<Result<TOk, TContinuationErr>> self,
            Func<TContinuationErr, Task<Result<TOk, TContinuationErr>>> continuation)
        {
            return await (await self).OrElseAsync(continuation);
        }

        /// <inheritdoc cref="Result{TOk,TErr}.Match{TResult}"/>
        public static async Task<TResult> MatchAsync<TOk, TErr, TResult>(
            this Result<TOk, TErr> self,
            Func<TOk, Task<TResult>> onOk,
            Func<TErr, Task<TResult>> onErr)
        {
            return await self.Match(onOk, onErr);
        }

        /// <inheritdoc cref="Result{TOk,TErr}.Match{TResult}"/>
        public static async Task<TResult> MatchAsync<TOk, TErr, TResult>(
            this Result<TOk, TErr> self,
            Func<TOk, TResult> onOk,
            Func<TErr, Task<TResult>> onErr)
        {
            return await self.Match(ok => Task.FromResult(onOk(ok)), onErr);
        }

        /// <inheritdoc cref="Result{TOk,TErr}.Match{TResult}"/>
        public static async Task<TResult> MatchAsync<TOk, TErr, TResult>(
            this Result<TOk, TErr> self,
            Func<TOk, Task<TResult>> onOk,
            Func<TErr, TResult> onErr)
        {
            return await self.Match(onOk, err => Task.FromResult(onErr(err)));
        }

        /// <inheritdoc cref="Result{TOk,TErr}.Match{TResult}"/>
        public static async Task<TResult> MatchAsync<TOk, TErr, TResult>(
            this Task<Result<TOk, TErr>> self,
            Func<TOk, Task<TResult>> onOk,
            Func<TErr, Task<TResult>> onErr)
        {
            return await (await self).Match(onOk, onErr);
        }

        /// <inheritdoc cref="Result{TOk,TErr}.Match{TResult}"/>
        public static async Task<TResult> MatchAsync<TOk, TErr, TResult>(
            this Task<Result<TOk, TErr>> self,
            Func<TOk, TResult> onOk,
            Func<TErr, Task<TResult>> onErr)
        {
            return await (await self).Match(ok => Task.FromResult(onOk(ok)), onErr);
        }

        /// <inheritdoc cref="Result{TOk,TErr}.Match{TResult}"/>
        public static async Task<TResult> MatchAsync<TOk, TErr, TResult>(
            this Task<Result<TOk, TErr>> self,
            Func<TOk, Task<TResult>> onOk,
            Func<TErr, TResult> onErr)
        {
            return await (await self).Match(onOk, err => Task.FromResult(onErr(err)));
        }
        /// <inheritdoc cref="Result{TOk,TErr}.Match{TResult}"/>
        public static async Task<TResult> MatchAsync<TOk, TErr, TResult>(
            this Task<Result<TOk, TErr>> self,
            Func<TOk, TResult> onOk,
            Func<TErr, TResult> onErr)
        {
            return (await self).Match(onOk, onErr);
        }

        /// <summary>
        ///     Alternative for <see cref="Result{TOk,TErr}.Match"/>, producing another result.
        /// </summary>
        /// <inheritdoc cref="Result{TOk,TErr}.Match"/>
        [Obsolete("Use 'MatchAsync' instead and create the Result yourself")]
        public static async Task<Result<TOkResult, TErrResult>> MatchResultAsync<TOk, TErr, TOkResult, TErrResult>(
            this Task<Result<TOk, TErr>> self,
            Func<TOk, Task<TOkResult>> onOk,
            Func<TErr, Task<TErrResult>> onErr)
        {
            return await (await self).Match(
                async ok => Result<TOkResult, TErrResult>.FromOk(await onOk(ok)),
                async err => Result<TOkResult, TErrResult>.FromErr(await onErr(err)));
        }

        /// <summary>
        ///     Alternative for <see cref="Result{TOk,TErr}.Match"/>, producing another result.
        /// </summary>
        /// <inheritdoc cref="Result{TOk,TErr}.Match"/>
        [Obsolete("Use 'MatchAsync' instead and create the Result yourself")]
        public static async Task<Result<TOkResult, TErrResult>> MatchResultAsync<TOk, TErr, TOkResult, TErrResult>(
            this Task<Result<TOk, TErr>> self,
            Func<TOk, Task<Result<TOkResult, TErrResult>>> onOk,
            Func<TErr, TErrResult> onErr)
        {
            return await (await self).Match(
                async ok => await onOk(ok),
                err => Task.FromResult(Result<TOkResult, TErrResult>.FromErr(onErr(err))));
        }

        /// <inheritdoc cref="Result{TOk,TErr}.Unwrap()"/>
        public static async Task<TOk> UnwrapAsync<TOk, TErr>(this Task<Result<TOk, TErr>> self)
        {
            return (await self).Unwrap();
        }

        /// <inheritdoc cref="Result{TOk,TErr}.Unwrap(string)"/>
        public static async Task<TOk> UnwrapAsync<TOk, TErr>(this Task<Result<TOk, TErr>> self, string error)
        {
            return (await self).Unwrap(error);
        }

        /// <inheritdoc cref="Result{TOk,TErr}.Unwrap(Func{TErr, string})"/>
        public static async Task<TOk> UnwrapAsync<TOk, TErr>(this Task<Result<TOk, TErr>> self, Func<TErr, string> error)
        {
            return (await self).Unwrap(error);
        }

        /// <inheritdoc cref="Result{TOk,TErr}.Unwrap(Func{TErr, string})"/>
        public static async Task<TOk> UnwrapAsync<TOk, TErr>(this Task<Result<TOk, TErr>> self, Func<TErr, Task<string>> error)
        {
            error ??= _ => throw new ArgumentNullException(nameof(error));
            return await self.MatchAsync(
                onOk: ok => ok,
                onErr: async err => throw new TriedToUnwrapErrException(await error(err)));
        }
    }
}
