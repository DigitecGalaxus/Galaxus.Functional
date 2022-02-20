using System;

namespace Galaxus.Functional
{
    public readonly partial struct Option<T>
    {
        /// <summary>
        ///     Provides access to <b>self</b>'s <b>Some</b> value by calling <paramref name="onSome" /> if <b>self</b> contains
        ///     <b>Some</b>.
        /// </summary>
        /// <param name="onSome">
        ///     Called if <b>self</b> contains <b>Some</b>. The argument to this action is never the <b>null</b>
        ///     reference.
        /// </param>
        public void IfSome(Action<T> onSome)
        {
            if (IsSome)
            {
                if (onSome is null)
                {
                    throw new ArgumentNullException(nameof(onSome));
                }

                onSome(obj: _some);
            }
        }

        // "IfNone" is not implemented because it can just as easly be written like this:
        // if(option.IsNone) { }
    }
}
