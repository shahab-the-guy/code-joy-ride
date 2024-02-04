using System.Threading.Tasks;
using CodeJoyRide.Fx.Analyzer;
using Microsoft.CodeAnalysis.Testing.Verifiers;
using Xunit;
using Verifier =
    Microsoft.CodeAnalysis.CSharp.Testing.XUnit.CodeFixVerifier<CodeJoyRide.Fx.Analyzer.MaybeSemanticAnalyzer,
        CodeJoyRide.Fx.Analyzer.MaybeCodeFixProvider>;

namespace CodeJoyRide.Fx.Analyzer.Tests;

public class MaybeCodeFixProviderTests
{
    [Fact]
    public async Task Suggests_code_fix_for_throwing_exception_when_method_returns_Maybe_T()
    {
        const string source = """
                              using System;
                              using CodeJoyRide.Fx;

                              public class Program
                              {
                                  public Maybe<int> GetValue(string number)
                                  {
                                     throw new InvalidOperationException("Could not parse the number");
                                  }
                              }
                              """;

        const string newSource = """
                                 using System;
                                 using CodeJoyRide.Fx;

                                 public class Program
                                 {
                                     public Maybe<int> GetValue(string number)
                                     {
                                         return CodeJoyRide.Fx.Maybe.None;
                                     }
                                 }
                                 """;

        var expected = Verifier.Diagnostic()
            .WithLocation(8, 8)
            .WithMessage("Use Maybe.None instead of throw exception");

        var codeFixTester = CSharpAnalyzerTestHelper
            .GetCodeFixAnalyzerForOption<MaybeSemanticAnalyzer, MaybeCodeFixProvider, XUnitVerifier>(
                source, [expected], newSource);

        await codeFixTester.RunAsync();
    }
}
