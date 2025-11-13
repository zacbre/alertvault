using System.Net;
using AlertVault.Core.Entities;
using AlertVault.Core.Entities.Dto;
using AlertVault.Core.Infrastructure.Database;
using AlertVault.Core.Infrastructure.Database.Repositories;
using FluentValidation;
using Hangfire;

namespace AlertVault.Core.Service;

public class AlertService(
    AlertRepository alertRepository, 
    AlertNotificationQueueRepository alertNotificationQueueRepository, 
    UserAgentRepository userAgentRepository, 
    IValidator<Alert> validator)
{
    public async Task<List<Alert>> All() => await alertRepository.All();
    
    public async Task<List<Alert>> AllExpired() => await alertRepository.AllExpired();
    
    public async Task<Alert?> Get(Guid uuid) => await alertRepository.Get(uuid);
    
    public async Task<Result<Alert?>> Add(int userId, TimeSpan interval)
    {
        var alert = new Alert
        {
            UserId = userId,
            CreatedUtc = DateTime.UtcNow,
            Uuid = Guid.NewGuid(),
            Interval = interval,
            LastCheckUtc = DateTime.UtcNow
        };

        var validationResult = await validator.ValidateAsync(alert);
        if (!validationResult.IsValid)
        {
            var errors = validationResult.Errors.Select(e => e.ErrorMessage).ToList();
            return Result<Alert?>.Failure(errors);
        }

        await alertRepository.Add(alert);

        return alert;
    }

    public async Task<Alert?> UpdateLastChecked(Guid uuid)
    {
        var alert = await alertRepository.Get(uuid);
        if (alert is null)
        {
            return null;
        }

        alert.LastCheckUtc = DateTime.UtcNow;
        alert.LastReported = null;
        await alertRepository.Save();
        
        return alert;
    }
    
    public async Task<string> AddRequest(int alertId, IPAddress? ipAddress, string? userAgent, string requestBody, string httpMethod)
    {
        var ipString = ipAddress?.ToString();
        var jobId = BackgroundJob.Enqueue<AlertService>(service => service.AddRequestInternal(alertId, ipString, userAgent, requestBody, httpMethod));
        return await Task.FromResult(jobId);
    }

    public async Task AddRequestInternal(int alertId, string? ipAddress, string? userAgent, string requestBody, string httpMethod)
    {
        var parsedIpAddress = ipAddress != null && IPAddress.TryParse(ipAddress, out var ip) ? ip : null;
        // First, locate the user agent.
        var foundUserAgent = userAgent == null ? null : await userAgentRepository.GetByUserAgentString(userAgent) ?? await userAgentRepository.Add(userAgent);
        var request = new Request
        {
            IpAddress = parsedIpAddress,
            UserAgentId = foundUserAgent?.Id,
            Method = Enum.Parse<RequestMethodTypeEnum>(httpMethod.ToUpperInvariant()),
            AlertId = alertId,
            Body = requestBody,
        };

        await alertRepository.AddRequest(request);
    }

    public async Task<List<Request>> GetRequests(Guid uuid) => await alertRepository.GetRequests(uuid);
    
    public void Delete(Alert alert) => alertRepository.Delete(alert);
    
    public async Task Save() => await alertRepository.Save();
}