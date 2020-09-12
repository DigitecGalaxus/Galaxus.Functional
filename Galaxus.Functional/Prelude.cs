using System;

namespace Galaxus.Functional
{
    /// <summary>
    /// Miscellaneous helper methods to ease a more function-oriented style of working.
    /// </summary>
    public static class Prelude
    {
        /// <summary>
        /// The identity function returns its input without modification.
        /// </summary>
        /// <typeparam name="T">The type to accept.</typeparam>
        /// <returns>The identity function for type <c>T</c>.</returns>
        public static Func<T, T> Identity<T>() => x => x;

        /// <summary>
        /// Builds a function to always return the given value, no matter what is given into it.
        /// </summary>
        /// <param name="value">The value to return.</param>
        /// <typeparam name="T">Type of the value to return.</typeparam>
        /// <returns>A function always returning the given value.</returns>
        public static Func<object, T> Const<T>(T value) => _ => value;

        /// <summary>
        /// Modifies a function to flip the order of parameters.
        /// </summary>
        /// <param name="func">The function to modify.</param>
        /// <typeparam name="T1">Type of the first parameter.</typeparam>
        /// <typeparam name="T2">Type of the second parameter.</typeparam>
        /// <typeparam name="T3">Type of the result.</typeparam>
        /// <returns>A modified function taking the second parameter as first and vice-versa.</returns>
        public static Func<T2, T1, T3> Flip<T1, T2, T3>(this Func<T1, T2, T3> func)
        {
            return (y, x) => func(x, y);
        }

        /// <summary>
        /// Turns a void-returning action to a <c>Unit</c>-returning function.
        /// </summary>
        /// <param name="action">The action to modify.</param>
        /// <typeparam name="T">Type of the parameter to the action.</typeparam>
        /// <returns>A function executing the given action and returning <c>Unit</c>.</returns>
        public static Func<T, Unit> Do<T>(this Action<T> action)
        {
            return x =>
            {
                action.Invoke(x);
                return Unit.Value;
            };
        }
    }
}
