using System;
using System.Collections.Immutable;
using System.Composition;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CodeActions;
using Microsoft.CodeAnalysis.CodeFixes;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using static Microsoft.CodeAnalysis.CSharp.SyntaxFactory;

namespace CodeJoyRide.Fx.Analyzer;

/// <summary>
/// A sample code fix provider that renames classes with the company name in their definition.
/// All code fixes must  be linked to specific analyzers.
/// </summary>
[ExportCodeFixProvider(LanguageNames.CSharp, Name = nameof(MaybeCodeFixProvider)), Shared]
public class MaybeCodeFixProvider : CodeFixProvider
{
    // Specify the diagnostic IDs of analyzers that are expected to be linked.
    public sealed override ImmutableArray<string> FixableDiagnosticIds { get; } =
        ImmutableArray.Create(MaybeSemanticAnalyzer.DiagnosticId);

    // If you don't need the 'fix all' behaviour, return null.
    public override FixAllProvider? GetFixAllProvider() => null;

    public sealed override async Task RegisterCodeFixesAsync(CodeFixContext context)
    {
        // We link only one diagnostic and assume there is only one diagnostic in the context.
        var diagnostic = context.Diagnostics.Single();

        // 'SourceSpan' of 'Location' is the highlighted area. We're going to use this area to find the 'SyntaxNode' to replace.
        var diagnosticSpan = diagnostic.Location.SourceSpan;

        // Get the root of Syntax Tree that contains the highlighted diagnostic.
        var root = await context.Document.GetSyntaxRootAsync(context.CancellationToken).ConfigureAwait(false);

        // Find SyntaxNode corresponding to the diagnostic.
        var diagnosticNode = root?.FindNode(diagnosticSpan);

        // To get the required metadata, we should match the Node to the specific type: 'ThrowStatementSyntax'.
        if (diagnosticNode is not ThrowStatementSyntax throwStatementSyntax)
            return;

        // Register a code action that will invoke the fix.
        context.RegisterCodeFix(CodeAction.Create(
            title: string.Format(Resources.CJR01CodeFixTitle, "throw", "Maybe.None"),
            token => ReplaceThrowWithReturnStatement(context.Document, throwStatementSyntax, token),
            equivalenceKey: nameof(Resources.CJR01CodeFixTitle)
        ), diagnostic);
    }

    private async Task<Document> ReplaceThrowWithReturnStatement(
        Document document, CSharpSyntaxNode throwSyntaxNode, CancellationToken token)
    {
        var returnStatement = ReturnStatement(
                MemberAccessExpression(
                    SyntaxKind.SimpleMemberAccessExpression,
                    MemberAccessExpression(
                        SyntaxKind.SimpleMemberAccessExpression,
                        MemberAccessExpression(
                            SyntaxKind.SimpleMemberAccessExpression,
                            IdentifierName("CodeJoyRide"),
                            IdentifierName("Fx")),
                        IdentifierName("Maybe")),
                    IdentifierName("None")))
            .NormalizeWhitespace()
            .WithLeadingTrivia(throwSyntaxNode.GetLeadingTrivia())
            .WithTrailingTrivia(throwSyntaxNode.GetTrailingTrivia());

        var currentRoot = await document.GetSyntaxRootAsync(token);
        var newRoot = currentRoot!.ReplaceNode(throwSyntaxNode, returnStatement);

        return document.WithSyntaxRoot(newRoot);
    }
}
