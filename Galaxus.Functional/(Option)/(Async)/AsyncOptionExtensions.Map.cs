using System;
using System.Threading.Tasks;

namespace Galaxus.Functional;

/// <summary>
///     Extensions to map operations for <see cref="Option{T}" /> using async methods or <see cref="Task" />s.
/// </summary>
public static class AsyncOptionExtensions_Map
{
    /// <inheritdoc cref="Option{T}.MapOrElse{TTo}" />
    public static async Task<TTo> MapOrElseAsync<T, TTo>(this Task<Option<T>> self, Func<T, TTo> map, Func<TTo> fallback)
    {
        throw new NotImplementedException();
    }

    /// <inheritdoc cref="Option{T}.MapOrElse{TTo}" />
    public static Task<TTo> MapOrElseAsync<T, TTo>(this Task<Option<T>> self, Func<T, Task<TTo>> map, Func<TTo> fallback)
    {
        throw new NotImplementedException();
    }

    /// <inheritdoc cref="Option{T}.MapOrElse{TTo}" />
    public static Task<TTo> MapOrElseAsync<T, TTo>(this Task<Option<T>> self, Func<T, TTo> map, Func<Task<TTo>> fallback)
    {
        throw new NotImplementedException();
    }

    /// <inheritdoc cref="Option{T}.MapOrElse{TTo}" />
    public static Task<TTo> MapOrElseAsync<T, TTo>(this Task<Option<T>> self, Func<T, Task<TTo>> map, Func<Task<TTo>> fallback)
    {
        throw new NotImplementedException();
    }

    /// <inheritdoc cref="Option{T}.MapOrElse{TTo}" />
    public static Task<TTo> MapOrElseAsync<T, TTo>(this Option<T> self, Func<T, Task<TTo>> map, Func<TTo> fallback)
    {
        throw new NotImplementedException();
    }
}
