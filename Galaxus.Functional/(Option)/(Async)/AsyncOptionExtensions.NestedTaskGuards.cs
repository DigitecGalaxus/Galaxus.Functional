using System;
using System.Threading.Tasks;

namespace Galaxus.Functional;

/*
 * Overloads whose only purpose is to turn an accidentally nested Task into a compiler warning.
 *
 * MatchAsync's result type is an unconstrained type parameter, so a callback that returns a
 * Task makes U infer as Task and the whole call return Task<Task>. Awaiting that only awaits the
 * outer task; the inner one runs as fire-and-forget.
 *
 * Not covered: callbacks that nest a generic task (Task<Task<T>>), and a throwing callback next to a nesting
 * one, because the throwing one keeps a generic overload applicable.
 */
public static partial class AsyncOptionExtensions
{
    private const string NestedTaskMessage =
        "Awaiting this call does not await the callbacks' work: it binds an overload returning Task<Task>, so the "
        + "inner Task runs unobserved and its exceptions are swallowed. Await the work inside each callback instead "
        + "of returning a Task from it; a callback that only throws needs an explicit delegate type.";

    private const string NestedTaskDiagnosticId = "GF0001";

    /// <summary>
    ///     Do not use. Awaiting this overload leaves an inner <see cref="Task" /> unawaited; see the obsolete message.
    /// </summary>
#if NET5_0_OR_GREATER
    [Obsolete(NestedTaskMessage, DiagnosticId = NestedTaskDiagnosticId)]
#else
    [Obsolete(NestedTaskMessage)]
#endif
    public static Task<Task> MatchAsync<T>(this Option<T> self, Func<T, Task<Task>> onSome, Func<Task<Task>> onNone)
    {
        return MatchAsync<T, Task>(self, onSome, onNone);
    }

    /// <summary>
    ///     Do not use. Awaiting this overload leaves an inner <see cref="Task" /> unawaited; see the obsolete message.
    /// </summary>
#if NET5_0_OR_GREATER
    [Obsolete(NestedTaskMessage, DiagnosticId = NestedTaskDiagnosticId)]
#else
    [Obsolete(NestedTaskMessage)]
#endif
    public static Task<Task> MatchAsync<T>(this Option<T> self, Func<T, Task<Task>> onSome, Func<Task> onNone)
    {
        return MatchAsync<T, Task>(self, onSome, onNone);
    }

    /// <summary>
    ///     Do not use. Awaiting this overload leaves an inner <see cref="Task" /> unawaited; see the obsolete message.
    /// </summary>
#if NET5_0_OR_GREATER
    [Obsolete(NestedTaskMessage, DiagnosticId = NestedTaskDiagnosticId)]
#else
    [Obsolete(NestedTaskMessage)]
#endif
    public static Task<Task> MatchAsync<T>(this Option<T> self, Func<T, Task> onSome, Func<Task<Task>> onNone)
    {
        return MatchAsync<T, Task>(self, onSome, onNone);
    }

    /// <summary>
    ///     Do not use. Awaiting this overload leaves an inner <see cref="Task" /> unawaited; see the obsolete message.
    /// </summary>
#if NET5_0_OR_GREATER
    [Obsolete(NestedTaskMessage, DiagnosticId = NestedTaskDiagnosticId)]
#else
    [Obsolete(NestedTaskMessage)]
#endif
    public static Task<Task> MatchAsync<T>(this Task<Option<T>> self, Func<T, Task<Task>> onSome, Func<Task<Task>> onNone)
    {
        return MatchAsync<T, Task>(self, onSome, onNone);
    }

    /// <summary>
    ///     Do not use. Awaiting this overload leaves an inner <see cref="Task" /> unawaited; see the obsolete message.
    /// </summary>
#if NET5_0_OR_GREATER
    [Obsolete(NestedTaskMessage, DiagnosticId = NestedTaskDiagnosticId)]
#else
    [Obsolete(NestedTaskMessage)]
#endif
    public static Task<Task> MatchAsync<T>(this Task<Option<T>> self, Func<T, Task<Task>> onSome, Func<Task> onNone)
    {
        return MatchAsync<T, Task>(self, onSome, onNone);
    }

    /// <summary>
    ///     Do not use. Awaiting this overload leaves an inner <see cref="Task" /> unawaited; see the obsolete message.
    /// </summary>
#if NET5_0_OR_GREATER
    [Obsolete(NestedTaskMessage, DiagnosticId = NestedTaskDiagnosticId)]
#else
    [Obsolete(NestedTaskMessage)]
#endif
    public static Task<Task> MatchAsync<T>(this Task<Option<T>> self, Func<T, Task> onSome, Func<Task<Task>> onNone)
    {
        return MatchAsync<T, Task>(self, onSome, onNone);
    }
}
