using Dapr.Client;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddDaprClient();
builder.Services.AddSingleton<HttpClient>(
    DaprClient.CreateInvokeHttpClient(appId: "app-b"));
var app = builder.Build();

app.MapPost("/serviceinvocation", async (
    SocialProfileDetails profileDetails,
    HttpClient httpClient) => {
        await httpClient.PostAsJsonAsync("/profile", profileDetails);
        Console.WriteLine($"Profile {profileDetails.Id} sent to AppB via service invocation.");
        return Results.Created(null as string, profileDetails.Id);
    }
);

app.MapPost("/pubsub", async (
    SocialProfileDetails profileDetails,
    DaprClient daprClient) => {
        await daprClient.PublishEventAsync(
            "mypubsub",
            "profiles",
            profileDetails);
        Console.WriteLine($"Profile {profileDetails.Id} sent to AppB via pubsub.");
        return Results.Created(null as string, profileDetails.Id);
    }
);

app.Run();

record SocialProfileDetails(string Id, string Name, string TwitterHandle, string GitHubHandle);
