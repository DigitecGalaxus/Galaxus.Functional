using System;

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

                onOk(_ok);
            }
            else
            {
                if (onErr is null)
                {
                    throw new ArgumentNullException(nameof(onErr));
                }

                onErr(_err);
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

                return onOk(_ok);
            }

            if (onErr is null)
            {
                throw new ArgumentNullException(nameof(onErr));
            }

            return onErr(_err);
        }
    }
}
