# ILSpy MCP Server

A Model Context Protocol (MCP) server that provides .NET assembly decompilation and analysis capabilities.

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

