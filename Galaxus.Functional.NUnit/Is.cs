using OriginalIs = NUnit.Framework.Is;

namespace Galaxus.Functional.NUnit;

public class Is : OriginalIs
{
    public static NoneConstraint None => new();

    public static SomeConstraint Some => new();

    public static ResultInStateConstraint Ok => new(true);

    public static ResultInStateConstraint Err => new(false);
}
