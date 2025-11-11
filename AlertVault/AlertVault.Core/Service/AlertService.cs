using AlertVault.Core.Entities;
using AlertVault.Core.Entities.Dto;
using AlertVault.Core.Infrastructure.Database;
using FluentValidation;

namespace AlertVault.Core.Service;

public class AlertService(AlertRepository alertRepository, AlertNotificationQueueRepository alertNotificationQueueRepository, IValidator<Alert> validator)
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

    public async Task<List<Request>> GetRequests(Guid uuid)
    {
        var alert = await alertRepository.Get(uuid);
        if (alert is null)
        {
            return [];
        }

        return alert.Requests;
    }
    
    public void Delete(Alert alert)
    {
        alertRepository.Delete(alert);
    }
    
    public async Task Save() => await alertRepository.Save();

}