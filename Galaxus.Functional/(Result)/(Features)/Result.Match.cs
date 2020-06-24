using System;
using System.Threading.Tasks;

namespace Galaxus.Functional
{
    public sealed partial class Result<TOk, TErr>
    {
        /// <summary>
        /// Provides access to <b>self</b>'s content by calling <paramref name="onOk"/> and passing in <b>Ok</b>
        /// or calling <paramref name="onErr"/> and passing in <b>Err</b>.
        /// </summary>
        /// <param name="onOk">Called when <b>self</b> contains <b>Ok</b>. The argument to this action is never the <b>null</b> reference.</param>
        /// <param name="onErr">Called when <b>self</b> contains <b>Err</b>. The argument to this action is never the <b>null</b> reference.</param>
        public void Match(Action<TOk> onOk, Action<TErr> onErr)
        {
            if (IsOk)
            {
                if (onOk is null)
                    throw new ArgumentNullException(nameof(onOk));

                onOk(_ok);
            }
            else
            {
                if (onErr is null)
                    throw new ArgumentNullException(nameof(onErr));

                onErr(_err);
            }
        }

        /// <summary>
        /// Provides access to <b>self</b>'s content by calling <paramref name="onOk"/> and passing in <b>Ok</b>
        /// or calling <paramref name="onErr"/> and passing in <b>Err</b>.
        /// </summary>
        /// <param name="onOk">Called when <b>self</b> contains <b>Ok</b>. The argument to this function is never the <b>null</b> reference.</param>
        /// <param name="onErr">Called when <b>self</b> contains <b>Err</b>. The argument to this function is never the <b>null</b> reference.</param>
        public T Match<T>(Func<TOk, T> onOk, Func<TErr, T> onErr)
        {
            if (IsOk)
            {
                if (onOk is null)
                    throw new ArgumentNullException(nameof(onOk));

                return onOk(_ok);
            }

            if (onErr is null)
                throw new ArgumentNullException(nameof(onErr));

            return onErr(_err);
        }

        /// <summary>
        /// An overload for <see cref="Match"/> using async functions.
        /// </summary>
        /// <param name="onOk">The async function to be called on an <b>Ok</b>.</param>
        /// <param name="onErr">The async function to be called on an <b>Ok</b>.</param>
        /// <typeparam name="TResult">The resulting <see cref="Result{TOk,TErr}"/> type.</typeparam>
        /// <returns></returns>
        public Task<TResult> MatchAsync<TResult>(Func<TOk, Task<TResult>> onOk, Func<TErr, Task<TResult>> onErr)
        {
            return Match(
                async ok => await onOk(ok),
                async err => await onErr(err));
        }

        /// <summary>
        /// An overload for <see cref="Match"/> using async functions.
        /// </summary>
        /// <param name="onOk">The async function to be called on an <b>Ok</b>.</param>
        /// <param name="onErr">The non-async function to be called on an <b>Ok</b>.</param>
        /// <typeparam name="TResult">The resulting <see cref="Result{TOk,TErr}"/> type.</typeparam>
        /// <returns></returns>
        public Task<TResult> MatchAsync<TResult>(Func<TOk, Task<TResult>> onOk, Func<TErr, TResult> onErr)
        {
            return Match(
                async ok => await onOk(ok),
                err => Task.FromResult(onErr(err)));
        }

        /// <summary>
        /// An overload for <see cref="Match"/> using async functions.
        /// </summary>
        /// <param name="onOk">The non-async function to be called on an <b>Ok</b>.</param>
        /// <param name="onErr">The async function to be called on an <b>Ok</b>.</param>
        /// <typeparam name="TResult">The resulting <see cref="Result{TOk,TErr}"/> type.</typeparam>
        /// <returns></returns>
        public Task<TResult> MatchAsync<TResult>(Func<TOk, TResult> onOk, Func<TErr, Task<TResult>> onErr)
        {
            return Match(
                ok => Task.FromResult(onOk(ok)),
                async err => await onErr(err));
        }
    }
}
