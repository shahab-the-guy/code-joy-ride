using System.Collections.Immutable;
using System.Threading.Tasks;
using CodeJoyRide.Fx.Analyzer;
using Microsoft.CodeAnalysis.CSharp.Testing.XUnit;
using Microsoft.CodeAnalysis.Testing;
using Microsoft.CodeAnalysis.Testing.Verifiers;
using Xunit;

namespace CodeJoyRide.Fx.Analyzer.Tests;

public class MaybeSemanticAnalyzerTests
{
    [Fact]
    public async Task Detects_diagnostic_for_throwing_error_when_a_method_returns_Maybe_of_T()
    {
        // Arrange
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

        var analyserTest = CSharpAnalyzerTestHelper.GetAnalyzerForOption<MaybeSemanticAnalyzer, XUnitVerifier>(
            source, [AnalyzerVerifier<MaybeSemanticAnalyzer>.Diagnostic().WithLocation(8, 8)]);

        // Assert
        await analyserTest.RunAsync();
    }
    
    [Fact]
    public async Task Detects_diagnostic_for_throwing_error_on_a_nested_level_when_a_method_returns_Maybe_of_T()
    {
        // Arrange
        const string source = """
                              using System;
                              using CodeJoyRide.Fx;

                              public class Program
                              {
                                  public Maybe<int> GetValue(string number)
                                  {
                                      var parsed = int.TryParse(number, out var n);
                                      if (!parsed)
                                          throw new InvalidOperationException("Could not parse the number");
                              
                                      return n;
                                  }
                              }
                              """;

        var analyserTest = CSharpAnalyzerTestHelper.GetAnalyzerForOption<MaybeSemanticAnalyzer, XUnitVerifier>(
            source, [AnalyzerVerifier<MaybeSemanticAnalyzer>.Diagnostic().WithLocation(10, 13)]);

        // Assert
        await analyserTest.RunAsync();
    }
    
    [Fact]
    public async Task Detects_NO_diagnostic_for_throwing_error_when_a_method_returns_normal_values()
    {
        // Arrange
        const string source = """
                              using System;
                              using CodeJoyRide.Fx;

                              public class Program
                              {
                                  public int GetValue(string number)
                                  {
                                      var parsed = int.TryParse(number, out var n);
                                      if (!parsed)
                                          throw new InvalidOperationException("Could not parse the number");
                              
                                      return n;
                                  }
                              }
                              """;

        var analyserTest = CSharpAnalyzerTestHelper.GetAnalyzerForOption<MaybeSemanticAnalyzer, XUnitVerifier>(
            source, ImmutableArray<DiagnosticResult>.Empty);

        // Assert
        await analyserTest.RunAsync();
    }
}
