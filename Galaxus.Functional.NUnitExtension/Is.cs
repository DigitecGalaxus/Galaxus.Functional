namespace Galaxus.Functional.NUnitExtension;

public class Is : NUnit.Framework.Is
{
    public static NoneConstraint OptionOfNone => new NoneConstraint();

    public static SomeConstraint OptionOfSome => new SomeConstraint();
    public static ResultConstraint Result => new ResultConstraint();
}
