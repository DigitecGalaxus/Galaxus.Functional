using System;
using System.Threading.Tasks;

namespace Galaxus.Functional
{
    public sealed partial class Result<TOk, TErr>
    {
        /// <summary>
        ///     Provides access to <b>self</b>'s content by calling <paramref name="onOk" /> and passing in <b>Ok</b>
        ///     or calling <paramref name="onErr" /> and passing in <b>Err</b>.
        /// </summary>
        /// <param name="onOk">
        ///     Called when <b>self</b> contains <b>Ok</b>. The argument to this action is never the <b>null</b>
        ///     reference.
        /// </param>
        /// <param name="onErr">
        ///     Called when <b>self</b> contains <b>Err</b>. The argument to this action is never the <b>null</b>
        ///     reference.
        /// </param>
        public void Match(Action<TOk> onOk, Action<TErr> onErr)
        {
            if (IsOk)
            {
                if (onOk is null)
                {
                    throw new ArgumentNullException(nameof(onOk));
                }

                onOk(obj: _ok);
            }
            else
            {
                if (onErr is null)
                {
                    throw new ArgumentNullException(nameof(onErr));
                }

                onErr(obj: _err);
            }
        }

        /// <summary>
        ///     Provides access to <b>self</b>'s content by calling <paramref name="onOk" /> and passing in <b>Ok</b>
        ///     or calling <paramref name="onErr" /> and passing in <b>Err</b>.
        /// </summary>
        /// <param name="onOk">
        ///     Called when <b>self</b> contains <b>Ok</b>. The argument to this function is never the <b>null</b>
        ///     reference.
        /// </param>
        /// <param name="onErr">
        ///     Called when <b>self</b> contains <b>Err</b>. The argument to this function is never the <b>null</b>
        ///     reference.
        /// </param>
        public T Match<T>(Func<TOk, T> onOk, Func<TErr, T> onErr)
        {
            if (IsOk)
            {
                if (onOk is null)
                {
                    throw new ArgumentNullException(nameof(onOk));
                }

                return onOk(arg: _ok);
            }

            if (onErr is null)
            {
                throw new ArgumentNullException(nameof(onErr));
            }

            return onErr(arg: _err);
        }

        /// <summary>
        ///     An overload for <see cref="Match" /> using async functions.
        /// </summary>
        /// <inheritdoc cref="Result{TOk,TErr}.Match{TResult}"/>
        public async Task<TResult> MatchAsync<TResult>(Func<TOk, Task<TResult>> onOk, Func<TErr, Task<TResult>> onErr)
        {
            return await Match(
                async ok => await onOk(arg: ok),
                async err => await onErr(arg: err));
        }

        /// <summary>
        ///     An overload for <see cref="Match" /> using async functions.
        /// </summary>
        /// <inheritdoc cref="Result{TOk,TErr}.Match{TResult}"/>
        public async Task<TResult> MatchAsync<TResult>(Func<TOk, Task<TResult>> onOk, Func<TErr, TResult> onErr)
        {
            return await Match(
                async ok => await onOk(arg: ok),
                err => Task.FromResult(onErr(arg: err)));
        }

        /// <summary>
        ///     An overload for <see cref="Match" /> using async functions.
        /// </summary>
        /// <inheritdoc cref="Result{TOk,TErr}.Match{TResult}"/>
        public async Task<TResult> MatchAsync<TResult>(Func<TOk, TResult> onOk, Func<TErr, Task<TResult>> onErr)
        {
            return await Match(
                ok => Task.FromResult(onOk(arg: ok)),
                async err => await onErr(arg: err));
        }
    }
}
