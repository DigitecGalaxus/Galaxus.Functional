using System;
using System.Threading.Tasks;

namespace Galaxus.Functional.Async
{
    /// <summary>
    ///     Extensions to common operations for <see cref="Result{TOk,TErr}" /> using async methods or <see cref="Task" />s.
    /// </summary>
    public static class AsyncResultExtensions
    {
        /// <summary>
        ///     Pass <paramref name="self" />'s <b>Ok</b> to <paramref name="onOk"/> or do nothing if <paramref name="self" /> is <b>Err</b>.
        /// </summary>
        /// <param name="self">The result to act on</param>
        /// <param name="onOk">Function to call if <paramref name="self" /> contains <b>Ok</b></param>
        public static async Task IfOkAsync<TOk, TErr>(this Result<TOk, TErr> self, Func<TOk, Task> onOk)
        {
            await self.Match(onOk, _ => Task.CompletedTask);
        }

        /// <summary>
        ///     Pass <paramref name="self" />'s <b>Err</b> to <paramref name="onErr"/> or do nothing if <paramref name="self" /> is <b>Ok</b>.
        /// </summary>
        /// <param name="self">The result to act on</param>
        /// <param name="onErr">Function to call if <paramref name="self" /> contains <b>Err</b></param>
        public static async Task IfErrAsync<TOk, TErr>(this Result<TOk, TErr> self, Func<TErr, Task> onErr)
        {
            await self.Match(_ => Task.CompletedTask, onErr);
        }

        /// <summary>
        ///     Calls <paramref name="continuation" /> if <paramref name="self" /> contains <b>Ok</b>, otherwise returns the <b>Err</b> value
        ///     contained in <paramref name="self" />.
        /// </summary>
        /// <param name="self">The result to act on</param>
        /// <param name="continuation">The function to call if <paramref name="self" /> contains <b>Ok</b></param>
        public static async Task<Result<TContinuation, TErr>> AndThenAsync<TOk, TErr, TContinuation>(
            this Result<TOk, TErr> self,
            Func<TOk, Task<Result<TContinuation, TErr>>> continuation)
        {
            return await self.Match(continuation, err => Task.FromResult(Result<TContinuation, TErr>.FromErr(err)));
        }

        /// <inheritdoc cref="AndThenAsync{TOk,TErr,TContinuation}(Galaxus.Functional.Result{TOk,TErr},System.Func{TOk,System.Threading.Tasks.Task{Galaxus.Functional.Result{TContinuation,TErr}}})"/>
        public static async Task<Result<TContinuation, TErr>> AndThenAsync<TOk, TErr, TContinuation>(
            this Task<Result<TOk, TErr>> self,
            Func<TOk, Result<TContinuation, TErr>> continuation)
        {
            return (await self).AndThen(continuation);
        }

        /// <inheritdoc cref="AndThenAsync{TOk,TErr,TContinuation}(Galaxus.Functional.Result{TOk,TErr},System.Func{TOk,System.Threading.Tasks.Task{Galaxus.Functional.Result{TContinuation,TErr}}})"/>
        public static async Task<Result<TContinuation, TErr>> AndThenAsync<TOk, TErr, TContinuation>(
            this Task<Result<TOk, TErr>> self,
            Func<TOk, Task<Result<TContinuation, TErr>>> continuation)
        {
            return await (await self).AndThenAsync(continuation);
        }

    }
}
