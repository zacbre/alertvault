namespace AlertVault.Core.Jobs;

public interface IJobInstance
{
    Task Execute();
}