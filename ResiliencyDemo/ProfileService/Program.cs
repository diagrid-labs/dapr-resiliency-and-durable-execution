using Dapr.Client;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddDaprClient();
var app = builder.Build();

const string StateStoreComponentName = "mystatestore";

app.MapPost("/profile", async (
    SocialProfileDetails profileDetails,
    DaprClient daprClient,
    HttpContext context) => {
    await daprClient.SaveStateAsync<SocialProfileDetails>(
        StateStoreComponentName,
        profileDetails.Id,
        profileDetails);

    Console.WriteLine($"Profile {profileDetails.Id} saved to state store.");
    var getUrl = $"{context.Request.Host}{context.Request.Path.Value}/{profileDetails.Id}";

    return Results.Created(getUrl, value: null);
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

record SocialProfileDetails(string Id, string Name, string Discord, string Bluesky, string Linkedin);