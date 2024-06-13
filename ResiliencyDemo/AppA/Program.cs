using Dapr.Client;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddSingleton<HttpClient>(DaprClient.CreateInvokeHttpClient(appId: "app-b"));
builder.Services.AddSingleton<DaprClient>(new DaprClientBuilder().Build());
var app = builder.Build();

app.MapPost("/post", async (
    SocialProfileDetails profileDetails,
    HttpClient httpClient) => {
        await httpClient.PostAsJsonAsync("/profile", profileDetails);
        return Results.Created();
    }
);

app.MapPost("/pubsub", async (
    SocialProfileDetails profileDetails,
    DaprClient daprClient) => {
        await daprClient.PublishEventAsync("mypubsub", "profiles" , profileDetails);
        return Results.Created();
    }
);

app.Run();

record SocialProfileDetails(string Name, string TwitterHandle, string GitHubHandle);
