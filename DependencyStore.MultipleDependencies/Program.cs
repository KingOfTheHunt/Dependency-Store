using DependencyStore.MultipleDependencies.Services;
using DependencyStore.MultipleDependencies.Services.Contracts;
using Microsoft.Extensions.DependencyInjection.Extensions;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// O TryAdd só vai adicionar a dependência caso a interface não está sendo resolvida por ninguém.
// builder.Services.TryAddTransient<IService, PrimaryService>();
// builder.Services.TryAddTransient<IService, SecondaryService>();

// Já o TryAddEnumerable permite adicionar mais de uma implementação por interface, desde que 
// as implementações sejam diferentes.
builder.Services.TryAddEnumerable(ServiceDescriptor.Scoped<IService, PrimaryService>());
builder.Services.TryAddEnumerable(ServiceDescriptor.Transient<IService, PrimaryService>());
builder.Services.TryAddEnumerable(ServiceDescriptor.Scoped<IService, SecondaryService>());
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.MapGet("/", (IEnumerable<IService> services) => 
    Results.Ok(services.Select(x => x.GetType().Name)));

app.Run();