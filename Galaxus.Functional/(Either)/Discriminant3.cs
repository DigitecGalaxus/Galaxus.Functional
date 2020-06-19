namespace Galaxus.Functional
{
    /// <summary>
    /// A discriminant indicates which field of a union is in use.
    /// The order of a union's fields might change. For this reason:
    /// Do not serialize discriminants, do not store them in databases and
    /// do not send them across application domains or processes.
    /// </summary>
    public enum Discriminant3 : byte
    {
        A,
        B,
        C
    }
}
