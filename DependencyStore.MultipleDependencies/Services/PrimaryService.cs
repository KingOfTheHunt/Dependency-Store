using DependencyStore.MultipleDependencies.Services.Contracts;

namespace DependencyStore.MultipleDependencies.Services;

public class PrimaryService : IService
{
    public Guid Id { get; set; } = Guid.NewGuid();
}