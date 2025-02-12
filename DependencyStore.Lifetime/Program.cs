using DependencyStore.Lifetime.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddSingleton<PrimaryService>();
builder.Services.AddScoped<SecondaryService>();
builder.Services.AddTransient<TertiaryService>();

var app = builder.Build();

app.MapGet("/", (PrimaryService primaryService, SecondaryService secondaryService,
    TertiaryService tertiaryService) => new
{
    Id = Guid.NewGuid(),
    PrimaryService = primaryService.Id,
    Message = "Eu sou único durante toda a execução da aplicação",
    SecondaryService = new
    {
        Id = secondaryService.Id,
        PrimaryService = primaryService.Id,
        Message = "Eu sou único em toda a requisição."
    },
    TertiaryService = new
    {
        Id = tertiaryService.Id,
        PrimaryService = primaryService.Id,
        SecondaryService = secondaryService.Id,
        SecondaryServiceNewInstance = secondaryService.Id,
        Message = "Eu sou diferente a cada vez que sou requisitado."
    }
});

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.Run();