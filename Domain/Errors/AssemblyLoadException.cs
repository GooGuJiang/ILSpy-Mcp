namespace ILSpy.Mcp.Domain.Errors;

public sealed class AssemblyLoadException : DomainException
{
    public string AssemblyPath { get; }

    public AssemblyLoadException(string assemblyPath, Exception innerException)
        : base("ASSEMBLY_LOAD_FAILED", $"Failed to load assembly '{assemblyPath}'", innerException)
    {
        AssemblyPath = assemblyPath;
    }
}
