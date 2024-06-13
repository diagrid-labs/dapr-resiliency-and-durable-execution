using Dapr.Client;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddSingleton<HttpClient>(DaprClient.CreateInvokeHttpClient(appId: "app-b"));
var app = builder.Build();

app.MapPost("/social", async (
    SocialProfileDetails profileDetails,
    HttpClient httpClient) => {
        await httpClient.PostAsJsonAsync("/profile", profileDetails);
        return Results.Created();
    }
);

app.Run();

record SocialProfileDetails(string Name, string TwitterHandle, string GitHubHandle);
