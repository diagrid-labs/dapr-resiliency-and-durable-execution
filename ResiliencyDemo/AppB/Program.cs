using Dapr.Client;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddSingleton<DaprClient>(new DaprClientBuilder().Build());
var app = builder.Build();
app.UseCloudEvents();

const string StateStoreName = "mystatestore";

app.MapPost("/profile", async (
    SocialProfileDetails details,
    DaprClient daprClient) => {
    await daprClient.SaveStateAsync<SocialProfileDetails>(
        StateStoreName,
        details.GitHubHandle,
        details);

    return Results.Created();
});

app.MapGet("/profile/{key}", async (
    string key,
    DaprClient daprClient) => {
    var details =  await daprClient.GetStateAsync<SocialProfileDetails>(
        StateStoreName,
        key);

    return details is not null ? Results.Ok(details) : Results.NotFound();
});

app.Run();

record SocialProfileDetails(string Name, string TwitterHandle, string GitHubHandle);
