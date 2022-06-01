var builder = WebApplication.CreateBuilder(args);
builder.Services.AddHealthChecks();

var app = builder.Build();
app.MapHealthChecks("/health");

var random = new Random();
app.MapGet("/account", () =>
{
    var accounts = Enumerable.Range(1, 5).Select(index =>
        new Account(Guid.NewGuid(), $"Account {index}", random.Next(1, 2000))
    ).ToArray();

    return accounts;
});

app.Run();

internal record Account(Guid Id, string Name, decimal Balance);
