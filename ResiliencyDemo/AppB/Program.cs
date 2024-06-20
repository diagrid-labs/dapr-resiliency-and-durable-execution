using Dapr.Client;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddDaprClient();
var app = builder.Build();
app.UseCloudEvents();

const string StateStoreComponentName = "mystatestore";

app.MapPost("/profile", async (
    SocialProfileDetails profileDetails,
    DaprClient daprClient) => {
    await daprClient.SaveStateAsync<SocialProfileDetails>(
        StateStoreComponentName,
        profileDetails.Id,
        profileDetails);
    Console.WriteLine($"Profile {profileDetails.Id} saved to state store.");

    return Results.Created();
});

app.MapGet("/profile/{id}", async (
    string id,
    DaprClient daprClient) => {
    Console.WriteLine($"Getting profile with id {id} from state store.");
    var details =  await daprClient.GetStateAsync<SocialProfileDetails>(
        StateStoreComponentName,
        id);

    return details is not null ? Results.Ok(details) : Results.NotFound();
});

app.Run();

record SocialProfileDetails(string Id, string Name, string TwitterHandle, string GitHubHandle);
