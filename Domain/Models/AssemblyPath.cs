namespace ILSpy.Mcp.Domain.Models;

/// <summary>
/// Value object representing a validated assembly file path.
/// </summary>
public sealed record AssemblyPath
{
    public string Value { get; }

    private AssemblyPath(string value)
    {
        Value = value;
    }

    public static AssemblyPath Create(string path)
    {
        if (string.IsNullOrWhiteSpace(path))
            throw new ArgumentException("Assembly path cannot be null or empty.", nameof(path));

        if (!File.Exists(path))
            throw new FileNotFoundException($"Assembly file not found: {path}", path);

        if (!Path.GetExtension(path).Equals(".dll", StringComparison.OrdinalIgnoreCase) &&
            !Path.GetExtension(path).Equals(".exe", StringComparison.OrdinalIgnoreCase))
            throw new ArgumentException($"Invalid assembly file extension: {path}", nameof(path));

        return new AssemblyPath(Path.GetFullPath(path));
    }

    public string FileName => Path.GetFileName(Value);
}
