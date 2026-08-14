using System.Collections.Generic;
using System.Collections.Immutable;
using System.IO;
using System.Linq;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using NUnit.Framework;

namespace Galaxus.Functional.Tests.NestedTaskGuards;

[TestFixture]
internal class NestedTaskGuardDiagnosticsTest
{
    private const string ObsoleteDiagnosticId = "GF0001";

    // Cases returning nested Tasks where the guards should get bound and cause warnings
    private static IEnumerable<TestCaseData> NestedTestcases()
    {
        yield return Case("option, onSome nested", "await option.MatchAsync(async _ => { await Task.Yield(); return OnNone(); }, OnNone);");
        yield return Case("option, onNone nested", "await option.MatchAsync(OnInt, async () => { await Task.Yield(); return OnNone(); });");
        yield return Case(
            "option, both nested",
            "await option.MatchAsync(async _ => { await Task.Yield(); return OnNone(); }, async () => { await Task.Yield(); return OnNone(); });");
        yield return Case("option in task, onSome nested", "await optionTask.MatchAsync(async _ => { await Task.Yield(); return OnNone(); }, OnNone);");
        yield return Case("option, onSome nested via method group", "await option.MatchAsync(NestedOnInt, OnNone);");

        yield return Case("result, onOk nested", "await result.MatchAsync(async _ => { await Task.Yield(); return OnNone(); }, OnString);");
        yield return Case("result, onErr nested", "await result.MatchAsync(OnInt, async _ => { await Task.Yield(); return OnNone(); });");
        yield return Case(
            "result, both nested",
            "await result.MatchAsync(async _ => { await Task.Yield(); return OnNone(); }, async _ => { await Task.Yield(); return OnNone(); });");
        yield return Case("result in task, onOk nested", "await resultTask.MatchAsync(async _ => { await Task.Yield(); return OnNone(); }, OnString);");

        yield return Case("either2, onA nested", "await either2.MatchAsync(async _ => { await Task.Yield(); return OnNone(); }, OnString);");
        yield return Case("either2, onB nested", "await either2.MatchAsync(OnInt, async _ => { await Task.Yield(); return OnNone(); });");
        yield return Case("either3, onC nested", "await either3.MatchAsync(OnInt, OnString, async _ => { await Task.Yield(); return OnNone(); });");
    }

    // Normal cases (not returning nested Tasks) where guards should not get bound
    private static IEnumerable<TestCaseData> CorrectTestcases()
    {
        yield return Case("option, lambdas", "await option.MatchAsync(x => OnInt(x), () => OnNone());");
        yield return Case("option, method groups", "await option.MatchAsync(OnInt, OnNone);");
        yield return Case("option in task, method groups", "await optionTask.MatchAsync(OnInt, OnNone);");
        yield return Case("option, onSome is an action", "await option.MatchAsync(_ => { }, OnNone);");
        yield return Case("option, onNone is an action", "await option.MatchAsync(OnInt, () => { });");
        yield return Case("option, callbacks return a value", "var u = await option.MatchAsync(IntAsync, () => Task.FromResult(0));");
        yield return Case("option, null callback in a typed variable", "Func<int, Task> onSome = null; await option.MatchAsync(onSome, OnNone);");

        yield return Case("result, method groups", "await result.MatchAsync(OnInt, OnString);");
        yield return Case("result in task, method groups", "await resultTask.MatchAsync(OnInt, OnString);");
        yield return Case("result in task, both are actions", "await resultTask.MatchAsync(_ => { }, _ => { });");
        yield return Case("result, callbacks return a value", "var u = await result.MatchAsync(IntAsync, _ => 0);");

        yield return Case("either2, method groups", "await either2.MatchAsync(OnInt, OnString);");
        yield return Case("either3, method groups", "await either3.MatchAsync(OnInt, OnString, _ => Task.CompletedTask);");
        yield return Case("either2, callbacks return a value", "var u = await either2.MatchAsync(IntAsync, _ => 0);");
        yield return Case("either2, onA is an action", "await either2.MatchAsync(_ => { }, OnString);");
        yield return Case("either2, onB is an action", "await either2.MatchAsync(OnInt, _ => { });");
        yield return Case("either3, onA is an action", "await either3.MatchAsync(_ => { }, OnString, OnBool);");
        yield return Case("either3, onB is an action", "await either3.MatchAsync(OnInt, _ => { }, OnBool);");
        yield return Case("either3, onC is an action", "await either3.MatchAsync(OnInt, OnString, _ => { });");
        yield return Case("either3, onA and onB are actions", "await either3.MatchAsync(_ => { }, _ => { }, OnBool);");
        yield return Case("either3, onA and onC are actions", "await either3.MatchAsync(_ => { }, OnString, _ => { });");
        yield return Case("either3, onB and onC are actions", "await either3.MatchAsync(OnInt, _ => { }, _ => { });");
    }

    [TestCaseSource(nameof(NestedTestcases))]
    public void ACallbackReturningANestedTaskIsReported(string testcase)
    {
        var diagnostics = Compile(testcase);

        Assert.IsEmpty(Errors(diagnostics), "the test case must compile");
        Assert.IsNotEmpty(
            diagnostics.Where(d => d.Id == ObsoleteDiagnosticId),
            $"expected {ObsoleteDiagnosticId}, but the call bound a non-obsolete overload and the nested task went unreported");
    }

    [TestCaseSource(nameof(CorrectTestcases))]
    public void ACorrectUsageIsNotReported(string testcase)
    {
        var diagnostics = Compile(testcase);

        Assert.IsEmpty(Errors(diagnostics), "the test case must compile");
        Assert.IsEmpty(
            diagnostics.Where(d => d.Id == ObsoleteDiagnosticId),
            $"the call bound a guard, so this previously clean test case now warns: {string.Join("; ", diagnostics.Select(d => d.GetMessage()))}");
    }

    private static TestCaseData Case(string name, string testcase)
    {
        return new TestCaseData(testcase).SetName($"{{m}}({name})");
    }

    private static IEnumerable<Diagnostic> Errors(ImmutableArray<Diagnostic> diagnostics)
    {
        return diagnostics.Where(d => d.Severity == DiagnosticSeverity.Error);
    }

    private static ImmutableArray<Diagnostic> Compile(string testcase)
    {
        var source = $$"""
                       using System;
                       using System.Threading.Tasks;
                       using Galaxus.Functional;

                       internal static class TestClass
                       {
                           private static Task OnInt(int value) => Task.CompletedTask;
                           private static Task OnString(string value) => Task.CompletedTask;
                           private static Task OnBool(bool value) => Task.CompletedTask;
                           private static Task OnNone() => Task.CompletedTask;
                           private static Task<int> IntAsync(int value) => Task.FromResult(0);
                           private static Task<Task> NestedOnInt(int value) => Task.FromResult(Task.CompletedTask);

                           private static async Task Run()
                           {
                               var option = 1.ToOption();
                               var optionTask = Task.FromResult(1.ToOption());
                               var result = 1.ToOk<int, string>();
                               var resultTask = Task.FromResult(1.ToOk<int, string>());
                               var either2 = new Either<int, string>(a: 1);
                               var either3 = new Either<int, string, bool>(a: 1);

                               {{testcase}}
                           }
                       }
                       """;

        var compilation = CSharpCompilation.Create(
            "NestedTaskGuardTestcase",
            [CSharpSyntaxTree.ParseText(source)],
            References,
            new CSharpCompilationOptions(OutputKind.DynamicallyLinkedLibrary));

        return compilation.GetDiagnostics();
    }

    private static readonly IReadOnlyList<MetadataReference> References =
    [
        MetadataReference.CreateFromFile(typeof(Option<>).Assembly.Location),
        MetadataReference.CreateFromFile(typeof(object).Assembly.Location),
        MetadataReference.CreateFromFile(Path.Combine(Path.GetDirectoryName(typeof(object).Assembly.Location)!, "System.Runtime.dll")),
        MetadataReference.CreateFromFile(Path.Combine(Path.GetDirectoryName(typeof(object).Assembly.Location)!, "netstandard.dll")),
    ];
}
