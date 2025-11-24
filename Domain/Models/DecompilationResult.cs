namespace ILSpy.Mcp.Domain.Models;

/// <summary>
/// Result of a decompilation operation.
/// </summary>
public sealed record DecompilationResult
{
    public string SourceCode { get; init; }
    public string TypeName { get; init; }
    public string AssemblyName { get; init; }
    public DateTimeOffset Timestamp { get; init; }

    public DecompilationResult(string sourceCode, string typeName, string assemblyName)
    {
        SourceCode = sourceCode ?? throw new ArgumentNullException(nameof(sourceCode));
        TypeName = typeName ?? throw new ArgumentNullException(nameof(typeName));
        AssemblyName = assemblyName ?? throw new ArgumentNullException(nameof(assemblyName));
        Timestamp = DateTimeOffset.UtcNow;
    }
}
