using System.Collections.Immutable;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Diagnostics;

namespace DiaryAssistance.Analyzers;

[DiagnosticAnalyzer(LanguageNames.CSharp)]
public class AsyncSuffixAnalyzer : DiagnosticAnalyzer
{
    public const string DiagnosticId = "DA001";

    private static readonly LocalizableString Title = "Async method should end with 'Async' suffix";
    private static readonly LocalizableString MessageFormat = "Async method '{0}' should end with 'Async' suffix";
    private static readonly LocalizableString Description = "Methods that are async or return Task/ValueTask should have names ending with 'Async'.";
    private const string Category = "Naming";

    private static readonly DiagnosticDescriptor Rule = new(
        DiagnosticId,
        Title,
        MessageFormat,
        Category,
        DiagnosticSeverity.Warning,
        isEnabledByDefault: true,
        description: Description);

    public override ImmutableArray<DiagnosticDescriptor> SupportedDiagnostics => ImmutableArray.Create(Rule);

    public override void Initialize(AnalysisContext context)
    {
        context.ConfigureGeneratedCodeAnalysis(GeneratedCodeAnalysisFlags.None);
        context.EnableConcurrentExecution();
        context.RegisterSyntaxNodeAction(AnalyzeMethod, SyntaxKind.MethodDeclaration);
    }

    private static void AnalyzeMethod(SyntaxNodeAnalysisContext context)
    {
        var methodDeclaration = (MethodDeclarationSyntax)context.Node;
        var methodName = methodDeclaration.Identifier.Text;

        if (methodName.EndsWith("Async"))
        {
            return;
        }

        var isAsync = methodDeclaration.Modifiers.Any(SyntaxKind.AsyncKeyword);
        var returnsTaskLike = IsTaskLikeReturnType(methodDeclaration, context.SemanticModel);

        if (isAsync || returnsTaskLike)
        {
            var diagnostic = Diagnostic.Create(Rule, methodDeclaration.Identifier.GetLocation(), methodName);
            context.ReportDiagnostic(diagnostic);
        }
    }

    private static bool IsTaskLikeReturnType(MethodDeclarationSyntax method, SemanticModel semanticModel)
    {
        var returnType = method.ReturnType;
        var typeInfo = semanticModel.GetTypeInfo(returnType);
        var typeSymbol = typeInfo.Type;

        if (typeSymbol == null)
        {
            return false;
        }

        var typeName = typeSymbol.ToDisplayString();

        return typeName.StartsWith("System.Threading.Tasks.Task") ||
               typeName.StartsWith("System.Threading.Tasks.ValueTask") ||
               typeName == "System.Threading.Tasks.Task" ||
               typeName == "System.Threading.Tasks.ValueTask";
    }
}
