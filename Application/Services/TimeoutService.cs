using Microsoft.Extensions.Options;

namespace ILSpy.Mcp.Application.Services;

/// <summary>
/// Service for managing timeouts and cancellation tokens.
/// </summary>
public interface ITimeoutService
{
    CancellationToken CreateTimeoutToken(CancellationToken cancellationToken = default);
    TimeSpan GetDefaultTimeout();
}

public sealed class TimeoutService : ITimeoutService
{
    private readonly ILSpy.Mcp.Application.Configuration.ILSpyOptions _options;

    public TimeoutService(IOptions<ILSpy.Mcp.Application.Configuration.ILSpyOptions> options)
    {
        _options = options.Value;
    }

    public CancellationToken CreateTimeoutToken(CancellationToken cancellationToken = default)
    {
        var timeout = TimeSpan.FromSeconds(_options.DefaultTimeoutSeconds);
        var timeoutCts = new CancellationTokenSource(timeout);
        
        if (cancellationToken != default)
        {
            return CancellationTokenSource.CreateLinkedTokenSource(
                cancellationToken, 
                timeoutCts.Token).Token;
        }
        
        return timeoutCts.Token;
    }

    public TimeSpan GetDefaultTimeout() => TimeSpan.FromSeconds(_options.DefaultTimeoutSeconds);
}
