namespace ILSpy.Mcp.Domain.Errors;

public sealed class TypeNotFoundException : DomainException
{
    public string TypeName { get; }
    public string AssemblyPath { get; }

    public TypeNotFoundException(string typeName, string assemblyPath)
        : base("TYPE_NOT_FOUND", $"Type '{typeName}' not found in assembly '{assemblyPath}'")
    {
        TypeName = typeName;
        AssemblyPath = assemblyPath;
    }
}
