namespace DependencyStore.Lifetime.Services;

public class SecondaryService(PrimaryService primaryService)
{
    private readonly PrimaryService _primaryService = primaryService;

    public Guid Id { get; set; } = Guid.NewGuid();
    public Guid PrimaryServiceId => _primaryService.Id;
}