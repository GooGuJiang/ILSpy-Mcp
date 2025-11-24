# ILSpy MCP Server - Connection Guide

This guide explains how to connect the ILSpy MCP server to Cursor.

## Prerequisites

- .NET 8.0 SDK installed
- Cursor IDE with MCP support

## Building the Server

1. Navigate to the project directory:
   ```bash
   cd C:\Users\Admin\Desktop\Dev\ILSpy-Mcp
   ```

2. Build the project in Release mode:
   ```bash
   dotnet build -c Release
   ```

3. The compiled executable will be located at:
   ```
   bin\Release\net8.0\ILSpy.Mcp.dll
   ```

## Adding to Cursor MCP Configuration

1. Open your Cursor MCP configuration file:
   ```
   C:\Users\Admin\.cursor\mcp.json
   ```

2. Add the following entry to the `mcpServers` object:

```json
{
  "mcpServers": {
    "ilspy": {
      "command": "dotnet",
      "args": [
        "C:\\Users\\Admin\\Desktop\\Dev\\ILSpy-Mcp\\bin\\Release\\net8.0\\ILSpy.Mcp.dll"
      ],
      "cwd": "C:\\Users\\Admin\\Desktop\\Dev\\ILSpy-Mcp",
      "env": {
        "ILSpy__MaxDecompilationSize": "1048576",
        "ILSpy__DefaultTimeoutSeconds": "30",
        "ILSpy__MaxConcurrentOperations": "10"
      },
      "disabled": false,
      "autoApprove": [
        "decompile_type",
        "decompile_method",
        "list_assembly_types",
        "analyze_assembly",
        "get_type_members",
        "find_type_hierarchy",
        "search_members_by_name",
        "find_extension_methods"
      ]
    }
  }
}
```

## Configuration Options

The server can be configured via environment variables (as shown above) or via `appsettings.json`:

- `ILSpy__MaxDecompilationSize`: Maximum size of decompiled code in bytes (default: 1048576 = 1 MB)
- `ILSpy__DefaultTimeoutSeconds`: Default timeout for operations in seconds (default: 30)
- `ILSpy__MaxConcurrentOperations`: Maximum number of concurrent operations (default: 10)

## Alternative: Using Executable (Self-Contained)

If you prefer to publish as a self-contained executable:

1. Publish the application:
   ```bash
   dotnet publish -c Release -r win-x64 --self-contained true -p:PublishSingleFile=true
   ```

2. Use the executable path in `mcp.json`:
   ```json
   {
     "mcpServers": {
       "ilspy": {
         "command": "C:\\Users\\Admin\\Desktop\\Dev\\ILSpy-Mcp\\bin\\Release\\net8.0\\win-x64\\publish\\ILSpy.Mcp.exe",
         "args": [],
         "env": {}
       }
     }
   }
   ```

## Available Tools

Once connected, the following tools will be available:

- **decompile_type**: Decompile and analyze a .NET type
- **decompile_method**: Decompile and analyze a specific method
- **list_assembly_types**: List all types in an assembly
- **analyze_assembly**: Get architectural overview of an assembly
- **get_type_members**: Get complete API surface of a type
- **find_type_hierarchy**: Find inheritance relationships
- **search_members_by_name**: Search for members by name
- **find_extension_methods**: Find extension methods for a type

## Usage Example

After adding the server to your configuration:

1. Restart Cursor to load the new MCP server
2. The server will be available in the MCP tools panel
3. You can use tools like:
   ```
   decompile_type(
     assemblyPath: "C:\\path\\to\\assembly.dll",
     typeName: "System.String",
     query: "What methods are available?"
   )
   ```

## Troubleshooting

### Server Not Starting

- Verify .NET 8.0 SDK is installed: `dotnet --version`
- Check the build output directory exists
- Review Cursor's MCP logs for error messages

### Tools Not Available

- Ensure the server is not disabled in `mcp.json`
- Check that `autoApprove` includes the tool names
- Verify the server started successfully in Cursor's MCP panel

### Timeout Errors

- Increase `ILSpy__DefaultTimeoutSeconds` for large assemblies
- Consider increasing `ILSpy__MaxDecompilationSize` if decompilation fails

## Security Notes

- All operations are **read-only** (no file modifications)
- Assembly paths are validated before processing
- Operations have timeout protection to prevent resource exhaustion
- Consider restricting access to specific assembly directories if needed

## Logging

Logs are written to stderr and can be viewed in Cursor's MCP server logs. To adjust log levels, you can modify the logging configuration in `appsettings.json` or via environment variables.

