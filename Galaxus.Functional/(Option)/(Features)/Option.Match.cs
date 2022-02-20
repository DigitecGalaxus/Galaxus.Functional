using System;

namespace Galaxus.Functional
{
    public readonly partial struct Option<T>
    {
        /// <summary>
        ///     Provides access to <b>self</b>'s content by calling <paramref name="onSome" /> and passing in <b>Some</b>
        ///     or calling <paramref name="onNone" />.
        /// </summary>
        /// <param name="onSome">
        ///     Called when <b>self</b> contains <b>Some</b>. The argument to this action is never the <b>null</b>
        ///     reference.
        /// </param>
        /// <param name="onNone">Called when <b>self</b> contains <b>None</b>.</param>
        public void Match(Action<T> onSome, Action onNone)
        {
            if (IsSome)
            {
                if (onSome is null)
                {
                    throw new ArgumentNullException(nameof(onSome));
                }

                onSome(obj: _some);
            }
            else
            {
                if (onNone is null)
                {
                    throw new ArgumentNullException(nameof(onNone));
                }

                onNone();
            }
        }

        /// <summary>
        ///     Provides access to <b>self</b>'s content by calling <paramref name="onSome" /> and passing in <b>Some</b>
        ///     or calling <paramref name="onNone" />.
        /// </summary>
        /// <param name="onSome">
        ///     Called when <b>self</b> contains <b>Some</b>. The argument to this action is never the <b>null</b>
        ///     reference.
        /// </param>
        /// <param name="onNone">Called when <b>self</b> contains <b>None</b>.</param>
        public U Match<U>(Func<T, U> onSome, Func<U> onNone)
        {
            if (IsSome)
            {
                if (onSome is null)
                {
                    throw new ArgumentNullException(nameof(onSome));
                }

                return onSome(arg: _some);
            }

            if (onNone is null)
            {
                throw new ArgumentNullException(nameof(onNone));
            }

            return onNone();
        }
    }
}
