using System;
using System.Threading.Tasks;

namespace Galaxus.Functional
{
    public sealed partial class Result<TOk, TErr>
    {
        /// <summary>
        ///     Provides access to <b>self</b>'s <b>Ok</b> value by calling <paramref name="onOk" /> if <b>self</b> contains
        ///     <b>Ok</b>.
        /// </summary>
        /// <param name="onOk">
        ///     Called when <b>self</b> contains <b>Ok</b>. The argument to this action is never the <b>null</b>
        ///     reference.
        /// </param>
        public void IfOk(Action<TOk> onOk)
        {
            if (IsOk)
            {
                if (onOk is null)
                {
                    throw new ArgumentNullException(nameof(onOk));
                }

                onOk(_ok);
            }
        }

        /// <summary>
        ///     Provides access to <b>self</b>'s <b>Err</b> value by calling <paramref name="onErr" /> if <b>self</b> contains
        ///     <b>Err</b>.
        /// </summary>
        /// <param name="onErr">
        ///     Called when <b>self</b> contains <b>Err</b>. The argument to this action is never the <b>null</b>
        ///     reference.
        /// </param>
        public void IfErr(Action<TErr> onErr)
        {
            if (IsErr)
            {
                if (onErr is null)
                {
                    throw new ArgumentNullException(nameof(onErr));
                }

                onErr(_err);
            }
        }

        /// <summary>
        ///     Provides access to <b>self</b>'s <b>Err</b> value by calling <paramref name="onErr" /> if <b>self</b> contains
        ///     <b>Err</b>.
        /// </summary>
        /// <param name="onErr">
        ///     Called when <b>self</b> contains <b>Err</b>. The argument to this function is never the <b>null</b>
        ///     reference.
        /// </param>
        public Task IfErrAsync(Func<TErr, Task> onErr)
        {
            if (IsErr)
            {
                if (onErr is null)
                {
                    throw new ArgumentNullException(nameof(onErr));
                }

                return onErr(_err);
            }

            return Task.CompletedTask;
        }
    }
}
