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
    }
}
