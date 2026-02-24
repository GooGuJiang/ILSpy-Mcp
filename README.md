# ILSpy MCP Server

A Model Context Protocol (MCP) server that provides .NET assembly decompilation and analysis capabilities.

## 🎯 What is this?

ILSpy MCP Server enables AI assistants (like Claude Desktop, Cursor) to decompile and analyze .NET assemblies directly through natural language commands. It integrates [ILSpy](https://github.com/icsharpcode/ILSpy) to provide powerful reverse-engineering capabilities.

## 🚀 Quick Start

### Prerequisites

- .NET 8.0 or higher
- ILSpy installed or ILSpy CLI available
- MCP-compatible client (Cursor, Claude Desktop, etc.)

### Installation

1. **Clone the repository:**
   ```bash
   git clone https://github.com/bivex/ILSpy-Mcp.git
   cd ILSpy-Mcp
   ```

2. **Build the project:**
   ```bash
   dotnet build -c Release
   ```

3. **Configure MCP Client:**

   For **Claude Desktop**, add to `claude_desktop_config.json`:
   ```json
   {
     "mcpServers": {
       "ilspy-mcp": {
         "command": "dotnet",
         "args": ["/path/to/ILSpy-Mcp.dll"]
       }
     }
   }
   ```

   For **Cursor**, configure in MCP settings:
   ```json
   {
     "servers": {
       "ilspy-mcp": {
         "command": "dotnet",
         "args": ["/path/to/ILSpy-Mcp.dll"]
       }
     }
   }
   ```

## Usage Examples

### Decompile an Assembly
```
Decompile the assembly /path/to/MyAssembly.dll and show me the Main method
```

### List All Types
```
List all types in the assembly /path/to/MyLibrary.dll
```

### Find a Specific Method
```
Find the CalculateTotal method in the assembly /path/to/Calculator.dll
```

### Analyze Type Hierarchy
```
Show me the type hierarchy for ProductService in /path/to/ECommerce.dll
```

### Search Members
```
Search for members containing "Authenticate" in /path/to/Auth.dll
```

## Architecture

This server follows a clean architecture with clear separation of concerns:

- **Domain**: Core business logic and entities
- **Application**: Use cases and application services
- **Infrastructure**: External system adapters (ILSpy, file system)
- **Transport**: MCP protocol layer

## Capabilities

- Decompile types, methods, and assemblies
- List and search types in assemblies
- Analyze assembly structure and architecture
- Find type hierarchies and relationships
- Discover extension methods
- Search members by name

## Configuration

Configuration is managed through environment variables and `appsettings.json`.

## Security

- All operations are read-only (no file modifications)
- Assembly path validation
- Timeout and cancellation support
- Request context propagation

## Requirements

- .NET 8.0+
- ILSpy or ILSpy CLI
- MCP-compatible client

## License

See LICENSE file for details.
