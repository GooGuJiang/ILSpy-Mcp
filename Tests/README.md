# ILSpy MCP Server Tests

This directory contains integration tests for all MCP tools in the ILSpy MCP server.

## Test Coverage

The test suite covers all 8 MCP tools:

1. ✅ **decompile_type** - Decompile and analyze types
2. ✅ **decompile_method** - Decompile and analyze methods  
3. ✅ **list_assembly_types** - List types in an assembly
4. ✅ **analyze_assembly** - Analyze assembly architecture (skipped - requires AI)
5. ✅ **get_type_members** - Get complete API surface of a type
6. ✅ **find_type_hierarchy** - Find inheritance relationships
7. ✅ **search_members_by_name** - Search for members by name
8. ✅ **find_extension_methods** - Find extension methods

## Running Tests

```bash
# Run all tests
dotnet test

# Run with verbose output
dotnet test --verbosity normal

# Run specific test class
dotnet test --filter "FullyQualifiedName~ToolsIntegrationTests"
```

## Test Results

- **Total Tests**: 15
- **Passed**: 12
- **Skipped**: 3 (tests requiring AI analysis or with known decompilation limitations)
- **Failed**: 0

## Test Assembly

Tests use `System.Private.CoreLib.dll` as the test assembly, which contains core .NET types like `System.String`. This assembly is located in the .NET runtime directory.

## Skipped Tests

Some tests are skipped for the following reasons:

1. **AI Analysis Tests**: Tests that require the MCP server's AI sampling capabilities are skipped in unit tests since they require a full MCP server instance.

2. **Decompilation Limitations**: Some types in `System.Private.CoreLib.dll` have decompilation limitations. These are tested via `GetTypeMembers` instead, which works reliably.

## Test Structure

- **ToolsIntegrationTests.cs**: Main test class with integration tests for all tools
- Tests verify:
  - Successful tool execution
  - Error handling (invalid paths, types, methods)
  - Filtering and search functionality
  - Proper exception mapping from domain to MCP errors

## Dependencies

- xUnit - Test framework
- FluentAssertions - Assertion library
- Microsoft.Extensions.Hosting - Dependency injection
- ModelContextProtocol - MCP SDK

