using System.Collections;

namespace Galaxus.Functional
{
    /// <summary>
    ///     General interface for objects that represent an optional value.
    /// </summary>
    public interface IOption : IEnumerable
    {

        /// <summary>
        ///     True if the option contains "Some".
        /// </summary>
        bool IsSome { get; }

        /// <summary>
        ///     True if the option contains "None".
        /// </summary>
        bool IsNone { get; }

        /// <summary>
        ///     Returns <b>Some</b> as an <see cref="object" /> if <b>self</b> contains <b>Some</b>.
        ///     Returns <b>null</b> otherwise.
        /// </summary>
        object ToObject();
    }
}
