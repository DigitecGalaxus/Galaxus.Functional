using System;
using System.Threading.Tasks;

namespace Galaxus.Functional.Async;

/// <summary>
///     Extensions to common operations for <see cref="Option{T}" /> using async methods or <see cref="Task" />s.
/// </summary>
public static class AsyncOptionExtensions
{
        /// <inheritdoc cref="Option{T}.Unwrap()"/>
        public static async Task<T> UnwrapAsync<T>(this Task<Option<T>> self)
        {
            return (await self).Unwrap();
        }

        /// <inheritdoc cref="Option{T}.Unwrap(string)"/>
        public static async Task<T> UnwrapAsync<T>(this Task<Option<T>> self, string error)
        {
            return (await self).Unwrap(error);
        }

        /// <inheritdoc cref="Option{T}.Unwrap(Func{string})"/>
        public static async Task<T> UnwrapAsync<T>(this Task<Option<T>> self, Func<string> error)
        {
            return (await self).Unwrap(error);
        }

        /// <inheritdoc cref="Option{T}.Unwrap(string)"/>
        public static async Task<T> UnwrapAsync<T>(this Task<Option<T>> self, Func<Task<string>> error)
        {
            var option = await self;
            if (option.IsNone)
            {
                if (error is null)
                {
                    throw new ArgumentNullException(nameof(error));
                }

                throw new TriedToUnwrapNoneException(await error());
            }

            return option.Unwrap();
        }

        /// <inheritdoc cref="Option{T}.UnwrapOr"/>
        public static async Task<T> UnwrapOrAsync<T>(this Task<Option<T>> self, T fallback)
        {
            return (await self).UnwrapOr(fallback);
        }

        /// <inheritdoc cref="Option{T}.UnwrapOr"/>
        public static async Task<T> UnwrapOrAsync<T>(this Task<Option<T>> self, Task<T> fallback)
        {
            return (await self).UnwrapOr(await fallback);
        }

        /// <inheritdoc cref="Option{T}.UnwrapOrElse"/>
        public static async Task<T> UnwrapOrElseAsync<T>(this Task<Option<T>> self, Func<T> fallback)
        {
            return (await self).UnwrapOrElse(fallback);
        }

        /// <inheritdoc cref="Option{T}.UnwrapOrElse"/>
        public static async Task<T> UnwrapOrElseAsync<T>(this Task<Option<T>> self, Func<Task<T>> fallback)
        {
            var option = await self;
            if (option.IsNone)
            {
                if (fallback is null)
                {
                    throw new ArgumentNullException(nameof(fallback));
                }

                return await fallback();
            }

            return option.Unwrap();
        }
}
