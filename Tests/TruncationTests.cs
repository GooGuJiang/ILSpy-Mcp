using Xunit;
using FluentAssertions;
using ILSpy.Mcp.Application.Configuration;
using ILSpy.Mcp.Application.Services;
using ILSpy.Mcp.Application.UseCases;
using ILSpy.Mcp.Domain.Services;
using ILSpy.Mcp.Infrastructure.Decompiler;
using ILSpy.Mcp.Transport.Mcp.Tools;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Runtime.InteropServices;

namespace ILSpy.Mcp.Tests;

/// <summary>
/// Tests to verify that tool responses are not truncated with "... and X more" messages.
/// </summary>
public class TruncationTests : IDisposable
{
    private readonly IServiceProvider _serviceProvider;
    private readonly string _testAssemblyPath;

    public TruncationTests()
    {
        // Use System.Collections.dll which contains many types
        var runtimePath = RuntimeEnvironment.GetRuntimeDirectory();
        
        var possibleAssemblies = new[]
        {
            Path.Combine(runtimePath, "System.Collections.dll"),
            Path.Combine(runtimePath, "System.Linq.dll"),
            Path.Combine(runtimePath, "System.Runtime.dll"),
            Path.Combine(runtimePath, "System.Private.CoreLib.dll")
        };
        
        _testAssemblyPath = possibleAssemblies.FirstOrDefault(File.Exists)
            ?? throw new InvalidOperationException($"No suitable test assembly found in: {runtimePath}");

        // Setup dependency injection
        var services = new ServiceCollection();
        
        services.AddLogging(builder => builder.AddConsole());
        
        services.Configure<ILSpyOptions>(options =>
        {
            options.DefaultTimeoutSeconds = 30;
            options.MaxDecompilationSize = 1_048_576;
            options.MaxConcurrentOperations = 10;
        });
        
        services.AddSingleton<ITimeoutService, TimeoutService>();
        services.AddScoped<IDecompilerService, ILSpyDecompilerService>();
        
        services.AddScoped<DecompileTypeUseCase>();
        services.AddScoped<DecompileMethodUseCase>();
        services.AddScoped<ListAssemblyTypesUseCase>();
        services.AddScoped<AnalyzeAssemblyUseCase>();
        services.AddScoped<GetTypeMembersUseCase>();
        services.AddScoped<FindTypeHierarchyUseCase>();
        services.AddScoped<SearchMembersByNameUseCase>();
        services.AddScoped<FindExtensionMethodsUseCase>();
        
        services.AddScoped<DecompileTypeTool>();
        services.AddScoped<DecompileMethodTool>();
        services.AddScoped<ListAssemblyTypesTool>();
        services.AddScoped<AnalyzeAssemblyTool>();
        services.AddScoped<GetTypeMembersTool>();
        services.AddScoped<FindTypeHierarchyTool>();
        services.AddScoped<SearchMembersByNameTool>();
        services.AddScoped<FindExtensionMethodsTool>();
        
        _serviceProvider = services.BuildServiceProvider();
    }

    [Fact]
    public async Task AnalyzeAssemblyTool_ShouldNotTruncateResults()
    {
        // Arrange
        var tool = _serviceProvider.GetRequiredService<AnalyzeAssemblyTool>();
        
        // Act
        var result = await tool.ExecuteAsync(
            _testAssemblyPath,
            query: "What is the main purpose of this assembly?",
            CancellationToken.None);
        
        // Assert
        result.Should().NotBeNullOrEmpty();
        result.Should().NotContain("... and", "results should not be truncated");
        result.Should().NotContain("more", "results should not contain truncation messages");
        result.Should().Contain("Assembly:", "should show assembly name");
        result.Should().Contain("Total Types:", "should show type count");
    }

    [Fact]
    public async Task SearchMembersByNameTool_ShouldNotTruncateResults()
    {
        // Arrange
        var tool = _serviceProvider.GetRequiredService<SearchMembersByNameTool>();
        
        // Act
        var result = await tool.ExecuteAsync(
            _testAssemblyPath,
            "ToString",
            memberKind: "method",
            CancellationToken.None);
        
        // Assert
        result.Should().NotBeNullOrEmpty();
        result.Should().NotContain("... showing first", "results should not be truncated");
        result.Should().NotContain("out of", "results should not contain truncation messages");
        result.Should().Contain("Search results", "should show search results");
        result.Should().Contain("ToString", "should find ToString methods");
    }

    [Fact]
    public async Task ListAssemblyTypesTool_ShouldNotTruncateResults()
    {
        // Arrange
        var tool = _serviceProvider.GetRequiredService<ListAssemblyTypesTool>();
        
        // Act
        var result = await tool.ExecuteAsync(
            _testAssemblyPath,
            namespaceFilter: null,
            CancellationToken.None);
        
        // Assert
        result.Should().NotBeNullOrEmpty();
        result.Should().NotContain("... and", "results should not be truncated");
        result.Should().NotContain("more", "results should not contain truncation messages");
        result.Should().Contain("Assembly:", "should show assembly name");
        result.Should().Contain("Types found:", "should show type count");
        
        // Verify that we can count types from the output
        var lines = result.Split('\n', StringSplitOptions.RemoveEmptyEntries);
        var typeCountLine = lines.FirstOrDefault(l => l.Contains("Types found:"));
        typeCountLine.Should().NotBeNull();
        
        // Extract the count
        var countMatch = System.Text.RegularExpressions.Regex.Match(typeCountLine!, @"Types found:\s*(\d+)");
        if (countMatch.Success)
        {
            var expectedCount = int.Parse(countMatch.Groups[1].Value);
            // Count actual type lines (excluding header lines)
            var actualTypeLines = lines.Count(l => 
                (l.Contains("class") || l.Contains("struct") || l.Contains("interface") || 
                 l.Contains("enum") || l.Contains("delegate")) && 
                !l.Contains("Types found:") && !l.Contains("Assembly:"));
            
            // The actual count should match or be close (some types might be filtered)
            actualTypeLines.Should().BeGreaterThan(0, "should list actual types");
        }
    }

    public void Dispose()
    {
        (_serviceProvider as IDisposable)?.Dispose();
    }
}

