using System;
using System.Threading.Tasks;

namespace Galaxus.Functional;

/*
 * Overloads whose only purpose is to turn an accidentally nested Task into a compiler warning.
 *
 * MatchAsync's result type is an unconstrained type parameter, so a callback that returns a
 * Task makes T infer as Task and the whole call return Task<Task>. Awaiting that only awaits the
 * outer task; the inner one runs as fire-and-forget.
 *
 * Not covered: callbacks that nest a generic task (Task<Task<T>>), and a throwing callback next to a nesting
 * one, because the throwing one keeps a generic overload applicable.
 */
public static partial class AsyncEitherExtensions
{
    private const string NestedTaskMessage =
        "Awaiting this call does not await the callbacks' work: it binds an overload returning Task<Task>, so the "
        + "inner Task runs unobserved and its exceptions are swallowed. Await the work inside each callback instead "
        + "of returning a Task from it; a callback that only throws needs an explicit delegate type.";

    /// <summary>
    ///     Do not use. Awaiting this overload leaves an inner <see cref="Task" /> unawaited; see the obsolete message.
    /// </summary>
    [Obsolete(NestedTaskMessage)]
    public static Task<Task> MatchAsync<A, B>(this Either<A, B> self, Func<A, Task<Task>> onA, Func<B, Task<Task>> onB)
    {
        return MatchAsync<A, B, Task>(self, onA, onB);
    }

    /// <summary>
    ///     Do not use. Awaiting this overload leaves an inner <see cref="Task" /> unawaited; see the obsolete message.
    /// </summary>
    [Obsolete(NestedTaskMessage)]
    public static Task<Task> MatchAsync<A, B>(this Either<A, B> self, Func<A, Task<Task>> onA, Func<B, Task> onB)
    {
        return MatchAsync<A, B, Task>(self, onA, onB);
    }

    /// <summary>
    ///     Do not use. Awaiting this overload leaves an inner <see cref="Task" /> unawaited; see the obsolete message.
    /// </summary>
    [Obsolete(NestedTaskMessage)]
    public static Task<Task> MatchAsync<A, B>(this Either<A, B> self, Func<A, Task> onA, Func<B, Task<Task>> onB)
    {
        return MatchAsync<A, B, Task>(self, onA, onB);
    }

    /// <summary>
    ///     Do not use. Awaiting this overload leaves an inner <see cref="Task" /> unawaited; see the obsolete message.
    /// </summary>
    [Obsolete(NestedTaskMessage)]
    public static Task<Task> MatchAsync<A, B, C>(this Either<A, B, C> self, Func<A, Task<Task>> onA, Func<B, Task<Task>> onB, Func<C, Task<Task>> onC)
    {
        return MatchAsync<A, B, C, Task>(self, onA, onB, onC);
    }

    /// <summary>
    ///     Do not use. Awaiting this overload leaves an inner <see cref="Task" /> unawaited; see the obsolete message.
    /// </summary>
    [Obsolete(NestedTaskMessage)]
    public static Task<Task> MatchAsync<A, B, C>(this Either<A, B, C> self, Func<A, Task<Task>> onA, Func<B, Task<Task>> onB, Func<C, Task> onC)
    {
        return MatchAsync<A, B, C, Task>(self, onA, onB, onC);
    }

    /// <summary>
    ///     Do not use. Awaiting this overload leaves an inner <see cref="Task" /> unawaited; see the obsolete message.
    /// </summary>
    [Obsolete(NestedTaskMessage)]
    public static Task<Task> MatchAsync<A, B, C>(this Either<A, B, C> self, Func<A, Task<Task>> onA, Func<B, Task> onB, Func<C, Task<Task>> onC)
    {
        return MatchAsync<A, B, C, Task>(self, onA, onB, onC);
    }

    /// <summary>
    ///     Do not use. Awaiting this overload leaves an inner <see cref="Task" /> unawaited; see the obsolete message.
    /// </summary>
    [Obsolete(NestedTaskMessage)]
    public static Task<Task> MatchAsync<A, B, C>(this Either<A, B, C> self, Func<A, Task<Task>> onA, Func<B, Task> onB, Func<C, Task> onC)
    {
        return MatchAsync<A, B, C, Task>(self, onA, onB, onC);
    }

    /// <summary>
    ///     Do not use. Awaiting this overload leaves an inner <see cref="Task" /> unawaited; see the obsolete message.
    /// </summary>
    [Obsolete(NestedTaskMessage)]
    public static Task<Task> MatchAsync<A, B, C>(this Either<A, B, C> self, Func<A, Task> onA, Func<B, Task<Task>> onB, Func<C, Task<Task>> onC)
    {
        return MatchAsync<A, B, C, Task>(self, onA, onB, onC);
    }

    /// <summary>
    ///     Do not use. Awaiting this overload leaves an inner <see cref="Task" /> unawaited; see the obsolete message.
    /// </summary>
    [Obsolete(NestedTaskMessage)]
    public static Task<Task> MatchAsync<A, B, C>(this Either<A, B, C> self, Func<A, Task> onA, Func<B, Task<Task>> onB, Func<C, Task> onC)
    {
        return MatchAsync<A, B, C, Task>(self, onA, onB, onC);
    }

    /// <summary>
    ///     Do not use. Awaiting this overload leaves an inner <see cref="Task" /> unawaited; see the obsolete message.
    /// </summary>
    [Obsolete(NestedTaskMessage)]
    public static Task<Task> MatchAsync<A, B, C>(this Either<A, B, C> self, Func<A, Task> onA, Func<B, Task> onB, Func<C, Task<Task>> onC)
    {
        return MatchAsync<A, B, C, Task>(self, onA, onB, onC);
    }
}
