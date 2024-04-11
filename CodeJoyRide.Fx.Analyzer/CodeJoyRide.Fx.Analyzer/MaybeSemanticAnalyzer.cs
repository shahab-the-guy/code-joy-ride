using System;
using System.Collections.Immutable;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Diagnostics;
using Microsoft.CodeAnalysis.Operations;

namespace CodeJoyRide.Fx.Analyzer;

/// <summary>
/// A sample analyzer that reports invalid values being used for the 'speed' parameter of the 'SetSpeed' function.
/// To make sure that we analyze the method of the specific class, we use semantic analysis instead of the syntax tree, so this analyzer will not work if the project is not compilable.
/// </summary>
[DiagnosticAnalyzer(LanguageNames.CSharp)]
public class MaybeSemanticAnalyzer : DiagnosticAnalyzer
{
    // Preferred format of DiagnosticId is Your Prefix + Number, e.g. CA1234.
    public const string DiagnosticId = "CJR01";

    // Feel free to use raw strings if you don't need localization.
    private static readonly LocalizableString Title = new LocalizableResourceString(nameof(Resources.CJR01Title),
        Resources.ResourceManager, typeof(Resources));

    // The message that will be displayed to the user.
    private static readonly LocalizableString MessageFormat =
        new LocalizableResourceString(nameof(Resources.CJR01MessageFormat), Resources.ResourceManager,
            typeof(Resources));

    private static readonly LocalizableString Description =
        new LocalizableResourceString(nameof(Resources.CJR01Description), Resources.ResourceManager,
            typeof(Resources));

    // The category of the diagnostic (Design, Naming etc.).
    private const string Category = "Usage";

    private static readonly DiagnosticDescriptor Rule = new(DiagnosticId, Title, MessageFormat, Category,
        DiagnosticSeverity.Error, isEnabledByDefault: true, description: Description
        , customTags: [WellKnownDiagnosticTags.NotConfigurable]
    );

    // Keep in mind: you have to list your rules here.
    public override ImmutableArray<DiagnosticDescriptor> SupportedDiagnostics { get; } =
        ImmutableArray.Create(Rule);

    public override void Initialize(AnalysisContext context)
    {
        // You must call this method to avoid analyzing generated code.
        context.ConfigureGeneratedCodeAnalysis(GeneratedCodeAnalysisFlags.None);

        // You must call this method to enable the Concurrent Execution.
        context.EnableConcurrentExecution();

        // Subscribe to semantic (compile time) action invocation, e.g. throw .
        context.RegisterOperationAction(AnalyzeThrowStatements, OperationKind.Throw);
    }

    private void AnalyzeThrowStatements(OperationAnalysisContext context)
    {
        if (context.Operation is not IThrowOperation || context.Operation.Syntax is not ThrowStatementSyntax)
            return;

        if (context.Operation.SemanticModel is null)
            return;

        var containingMethodSyntax = GetContainingMethodSyntax(context.Operation.Syntax);
        var containingMethodSymbol =
            context.Operation.SemanticModel.GetDeclaredSymbol(containingMethodSyntax) as IMethodSymbol;

        var returnTypeSymbol = containingMethodSymbol!.ReturnType;
        var maybeTypeSymbol = context.Compilation.GetTypeByMetadataName("CodeJoyRide.Fx.Maybe`1");

        if (!returnTypeSymbol.OriginalDefinition.Equals(maybeTypeSymbol, SymbolEqualityComparer.Default))
            return;

        var diagnostic = Diagnostic.Create(Rule, context.Operation.Syntax.GetLocation());
        context.ReportDiagnostic(diagnostic);
    }

    private MethodDeclarationSyntax GetContainingMethodSyntax(SyntaxNode syntaxNode)
    {
        while (true)
        {
            if (syntaxNode.Parent is MethodDeclarationSyntax mds) return mds;
            syntaxNode = syntaxNode.Parent!;
        }
    }
}
