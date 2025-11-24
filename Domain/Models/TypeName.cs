namespace ILSpy.Mcp.Domain.Models;

/// <summary>
/// Value object representing a fully qualified .NET type name.
/// </summary>
public sealed record TypeName
{
    public string FullName { get; }
    public string? Namespace { get; }
    public string ShortName { get; }

    private TypeName(string fullName, string? @namespace, string shortName)
    {
        FullName = fullName;
        Namespace = @namespace;
        ShortName = shortName;
    }

    public static TypeName Create(string fullName)
    {
        if (string.IsNullOrWhiteSpace(fullName))
            throw new ArgumentException("Type name cannot be null or empty.", nameof(fullName));

        var lastDot = fullName.LastIndexOf('.');
        var @namespace = lastDot >= 0 ? fullName.Substring(0, lastDot) : null;
        var shortName = lastDot >= 0 ? fullName.Substring(lastDot + 1) : fullName;

        return new TypeName(fullName, @namespace, shortName);
    }
}
