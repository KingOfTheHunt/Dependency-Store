using DependencyStore.MultipleDependencies.Services.Contracts;

namespace DependencyStore.MultipleDependencies.Services;

public class SecondaryService : IService
{
    public Guid Id { get; set; }
}