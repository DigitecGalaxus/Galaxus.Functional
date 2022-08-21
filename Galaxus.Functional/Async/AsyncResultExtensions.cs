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
        ///     Provides access to <paramref name="self" />'s <b>Ok</b> value by calling <paramref name="onOk" />
        ///     if <paramref name="self" /> contains <b>Ok</b>.
        /// </summary>
        /// <param name="self">The result to act on.</param>
        /// <param name="onOk">
        ///     Called when <paramref name="self" /> contains <b>Ok</b>. The argument to this function may not be <b>null</b>.
        /// </param>
        public static Task IfOkAsync<TOk, TErr>(this Result<TOk, TErr> self, Func<TOk, Task> onOk)
        {
            return self.Match(onOk, _ => Task.CompletedTask);
        }

        /// <summary>
        ///     Provides access to <paramref name="self" />'s <b>Err</b> value by calling <paramref name="onErr" />
        ///     if <paramref name="self" /> contains <b>Err</b>.
        /// </summary>
        /// <param name="self">The result to act on.</param>
        /// <param name="onErr">
        ///     Called when <paramref name="self" /> contains <b>Err</b>. The argument to this function may not be <b>null</b>.
        /// </param>
        public static Task IfErrAsync<TOk, TErr>(this Result<TOk, TErr> self, Func<TErr, Task> onErr)
        {
            return self.Match(_ => Task.CompletedTask, onErr);
        }
    }
}
