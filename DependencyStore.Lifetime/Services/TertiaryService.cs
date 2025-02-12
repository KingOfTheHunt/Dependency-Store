namespace DependencyStore.Lifetime.Services;

public class TertiaryService(PrimaryService primaryService, SecondaryService secondaryService, 
    SecondaryService secondaryServiceNewInstance)
{
    private readonly PrimaryService _primaryService = primaryService;
    private readonly SecondaryService _secondaryService = secondaryService;
    private readonly SecondaryService _secondaryServiceNewInstance = secondaryServiceNewInstance;

    public Guid Id { get; set; } = Guid.NewGuid();
    public Guid PrimaryServiceId => _primaryService.Id;
    public Guid SecondaryServiceId => _secondaryService.Id;
    public Guid SecondaryServiceNewInstanceId => _secondaryServiceNewInstance.Id;
}