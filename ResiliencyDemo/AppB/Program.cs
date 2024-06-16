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
        details.Id,
        details);
    Console.WriteLine("Profile saved to state store.");

    return Results.Created();
});

app.MapGet("/profile/{id}", async (
    string id,
    DaprClient daprClient) => {
    Console.WriteLine($"Getting profile with id {id} from state store.");
    var details =  await daprClient.GetStateAsync<SocialProfileDetails>(
        StateStoreName,
        id);

    return details is not null ? Results.Ok(details) : Results.NotFound();
});

app.Run();

record SocialProfileDetails(string Id, string Name, string TwitterHandle, string GitHubHandle);
