# ILSpy MCP Server Architecture

This document describes the architecture of the ILSpy MCP server, which follows clean architecture principles with clear separation of concerns.

## Architecture Overview

The server is organized into four main layers:

```
┌─────────────────────────────────────┐
│      Transport (MCP Protocol)       │  ← MCP tool handlers, protocol DTOs
├─────────────────────────────────────┤
│      Application (Use Cases)        │  ← Business logic orchestration
├─────────────────────────────────────┤
│         Domain (Core)               │  ← Business entities, value objects, ports
├─────────────────────────────────────┤
│      Infrastructure (Adapters)      │  ← ILSpy integration, external services
└─────────────────────────────────────┘
```

## Layer Responsibilities

### Domain Layer (`Domain/`)

**Purpose**: Core business logic and domain model, independent of external frameworks.

**Components**:
- **Models**: Value objects and entities (`AssemblyPath`, `TypeName`, `TypeInfo`, etc.)
- **Services (Ports)**: Interfaces defining capabilities (`IDecompilerService`)
- **Errors**: Domain-specific exceptions (`TypeNotFoundException`, `AssemblyLoadException`)

**Principles**:
- No dependencies on external frameworks
- Pure business logic
- Stable contracts (interfaces)

### Application Layer (`Application/`)

**Purpose**: Orchestrates use cases and coordinates domain services.

**Components**:
- **UseCases**: Single-purpose operations (e.g., `DecompileTypeUseCase`, `ListAssemblyTypesUseCase`)
- **Configuration**: Application settings (`ILSpyOptions`)
- **Services**: Application-level services (`ITimeoutService`)

**Principles**:
- One use case per operation
- Coordinates domain services
- Handles timeouts and cancellation
- Logs operations with correlation context

### Infrastructure Layer (`Infrastructure/`)

**Purpose**: Implements adapters for external systems and frameworks.

**Components**:
- **Decompiler**: ILSpy adapter (`ILSpyDecompilerService`)

**Principles**:
- Encapsulates external API details
- Implements domain service interfaces
- Handles external system failures gracefully

### Transport Layer (`Transport/Mcp/`)

**Purpose**: MCP protocol implementation and tool handlers.

**Components**:
- **Tools**: MCP tool handlers (e.g., `DecompileTypeTool`, `ListAssemblyTypesTool`)
- **Errors**: MCP-specific exceptions (`McpToolException`)

**Principles**:
- Maps MCP requests to use cases
- Transforms domain errors to MCP errors
- Provides stable public API contracts
- Handles timeouts and cancellation

## Key Design Patterns

### Ports and Adapters (Hexagonal Architecture)

- **Ports**: Interfaces in the domain layer (`IDecompilerService`)
- **Adapters**: Implementations in infrastructure layer (`ILSpyDecompilerService`)

This allows swapping implementations without changing domain or application code.

### Dependency Injection

All dependencies are injected via constructors:
- Use cases depend on domain service interfaces
- Tool handlers depend on use cases
- Infrastructure services implement domain interfaces

### Error Handling Strategy

1. **Domain Errors**: Thrown by domain layer (`TypeNotFoundException`, `AssemblyLoadException`)
2. **Application Errors**: Wrapped or transformed in use cases
3. **Transport Errors**: Mapped to `McpToolException` with stable error codes

Error codes are part of the public contract and should be versioned.

### Timeout and Cancellation

- All operations support `CancellationToken`
- Default timeout configured via `ILSpyOptions`
- Timeout service creates linked cancellation tokens
- Timeouts are logged and mapped to appropriate exceptions

### Concurrency

- All operations are async/await
- No shared mutable state
- Stateless design (state stored externally if needed)
- Thread-safe value objects

## Configuration

Configuration is managed through:
- `appsettings.json`: Default settings
- Environment variables: Override defaults
- `ILSpyOptions`: Strongly-typed configuration class

## Security Considerations

1. **Read-Only Operations**: All operations are read-only (no file modifications)
2. **Path Validation**: `AssemblyPath` validates file existence and extension
3. **Input Sanitization**: Type names and paths are validated
4. **Timeout Protection**: Prevents resource exhaustion
5. **Error Message Sanitization**: Avoids leaking sensitive information

## Testing Strategy

### Unit Tests
- Domain models and value objects
- Use case logic
- Error handling

### Integration Tests
- ILSpy adapter with real assemblies
- End-to-end tool execution
- Timeout and cancellation scenarios

## Extension Points

### Adding a New Tool

1. Create a use case in `Application/UseCases/`
2. Create a tool handler in `Transport/Mcp/Tools/`
3. Register both in `Program.cs`
4. Add tool description and parameter documentation

### Adding a New Decompiler

1. Implement `IDecompilerService` interface
2. Create adapter in `Infrastructure/Decompiler/`
3. Register in `Program.cs`
4. No changes needed to domain or application layers

### Adding Configuration Options

1. Add property to `ILSpyOptions`
2. Update `appsettings.json`
3. Use `IOptions<ILSpyOptions>` in services

## Dependencies

- **Domain**: None (pure C#)
- **Application**: Domain, Microsoft.Extensions.*
- **Infrastructure**: Domain, ILSpy, MCP SDK
- **Transport**: Application, Domain, MCP SDK

## Future Improvements

- [ ] Add metrics/telemetry
- [ ] Add health checks
- [ ] Add request correlation IDs
- [ ] Add rate limiting
- [ ] Add caching for frequently accessed assemblies
- [ ] Add support for multiple decompiler backends
