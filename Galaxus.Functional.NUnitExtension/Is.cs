namespace Galaxus.Functional.NUnitExtension;

public class Is : NUnit.Framework.Is
{
    public static NoneConstraint None => new();

    public static SomeConstraint Some => new();

    public static ResultInStateConstraint Ok => new(true);

    public static ResultInStateConstraint Err => new(false);
}
