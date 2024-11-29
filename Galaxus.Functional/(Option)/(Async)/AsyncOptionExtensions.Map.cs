using System;
using System.Threading.Tasks;

namespace Galaxus.Functional;

/// <summary>
///     Extensions to map operations for <see cref="Option{T}" /> using async methods or <see cref="Task" />s.
/// </summary>
public static partial class AsyncOptionExtensions
{
    /// <inheritdoc cref="Option{T}.MapOrElse{TTo}" />
    public static async Task<TTo> MapOrElseAsync<T, TTo>(this Task<Option<T>> self, Func<T, TTo> map, Func<TTo> fallback)
    {
        var option = await self.ConfigureAwait(false);
        return option.MapOrElse(map, fallback);
    }

    /// <inheritdoc cref="Option{T}.MapOrElse{TTo}" />
    public static async Task<TTo> MapOrElseAsync<T, TTo>(this Task<Option<T>> self, Func<T, Task<TTo>> map, Func<TTo> fallback)
    {
        var option = await self.ConfigureAwait(false);
        return await option.MapOrElseAsync(map, fallback).ConfigureAwait(false);
    }

    /// <inheritdoc cref="Option{T}.MapOrElse{TTo}" />
    public static async Task<TTo> MapOrElseAsync<T, TTo>(this Task<Option<T>> self, Func<T, TTo> map, Func<Task<TTo>> fallback)
    {
        var option = await self.ConfigureAwait(false);
        return await option.MapOrElseAsync(map, fallback).ConfigureAwait(false);
    }

    /// <inheritdoc cref="Option{T}.MapOrElse{TTo}" />
    public static async Task<TTo> MapOrElseAsync<T, TTo>(this Task<Option<T>> self, Func<T, Task<TTo>> map, Func<Task<TTo>> fallback)
    {
        var option = await self.ConfigureAwait(false);
        return await option.MapOrElseAsync(map, fallback).ConfigureAwait(false);
    }

    /// <inheritdoc cref="Option{T}.MapOrElse{TTo}" />
    public static async Task<TTo> MapOrElseAsync<T, TTo>(this Option<T> self, Func<T, Task<TTo>> map, Func<TTo> fallback)
    {
        return await self.MapOrElseAsync(
            map,
            () =>
            {
                if (fallback == null)
                {
                    throw new ArgumentException(nameof(fallback));
                }

                return Task.FromResult(fallback());
            }).ConfigureAwait(false);
    }

    /// <inheritdoc cref="Option{T}.MapOrElse{TTo}" />
    public static async Task<TTo> MapOrElseAsync<T, TTo>(this Option<T> self, Func<T, TTo> map, Func<Task<TTo>> fallback)
    {
        return await self.MapOrElseAsync(
            t =>
            {
                if (map == null)
                {
                    throw new ArgumentNullException(nameof(map));
                }

                return Task.FromResult(map(t));
            },
            fallback).ConfigureAwait(false);
    }

    /// <inheritdoc cref="Option{T}.MapOrElse{TTo}" />
    public static async Task<TTo> MapOrElseAsync<T, TTo>(this Option<T> self, Func<T, Task<TTo>> map, Func<Task<TTo>> fallback)
    {
        return await self.MapOrElse(map, fallback).ConfigureAwait(false);
    }

    /// <inheritdoc cref="Option{T}.Map{TTo}"/>
    public static Task<Option<TTo>> MapAsync<T, TTo>(this Option<T> self, Func<T, Task<Option<TTo>>> map)
        => self.MapOrElseAsync(map, () => Option<TTo>.None);
}
