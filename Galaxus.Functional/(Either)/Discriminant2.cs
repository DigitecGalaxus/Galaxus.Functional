namespace Galaxus.Functional
{
    /// <summary>
    ///     A discriminant indicates which field of a union is in use.
    /// </summary>
    /// <remarks>
    ///     The order of a union's fields might change. For this reason:
    ///     Do not serialize discriminants, do not store them in databases and
    ///     do not send them across application domains or processes.
    /// </remarks>
    public enum Discriminant2 : byte
    {
        /// <summary>
        ///     The first field of an <see cref="Either{A, B}"/>
        /// </summary>
        A,

        /// <summary>
        ///     The second field of an <see cref="Either{A, B}"/>
        /// </summary>
        B
    }
}
