namespace ILSpy.Mcp.Application.Configuration;

/// <summary>
/// Configuration options for ILSpy operations.
/// </summary>
public sealed class ILSpyOptions
{
    public const string SectionName = "ILSpy";

    /// <summary>
    /// Maximum size of decompiled code in bytes before truncation.
    /// </summary>
    public int MaxDecompilationSize { get; set; } = 1_048_576; // 1 MB

    /// <summary>
    /// Default timeout for operations in seconds.
    /// </summary>
    public int DefaultTimeoutSeconds { get; set; } = 30;

    /// <summary>
    /// Maximum number of concurrent decompilation operations.
    /// </summary>
    public int MaxConcurrentOperations { get; set; } = 10;
}
