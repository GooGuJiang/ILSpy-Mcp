namespace ILSpy.Mcp.Transport.Mcp.Errors;

/// <summary>
/// Exception thrown by MCP tool handlers. These are part of the public contract.
/// Error codes are stable and documented for the MCP client.
/// </summary>
public sealed class McpToolException : Exception
{
    public string ErrorCode { get; }

    public McpToolException(string errorCode, string message) : base(message)
    {
        ErrorCode = errorCode;
    }

    public McpToolException(string errorCode, string message, Exception innerException) 
        : base(message, innerException)
    {
        ErrorCode = errorCode;
    }
}
