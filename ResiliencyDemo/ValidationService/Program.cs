using Dapr.Client;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddDaprClient();
builder.Services.AddSingleton<HttpClient>(
    DaprClient.CreateInvokeHttpClient(appId: "profile"));
var app = builder.Build();

app.MapPost("/validate", async (
    SocialProfileDetails profileDetails,
    HttpClient httpClient) => {
        var validator = new SocialProfileDetailsValidator();
        var validationResult = await validator.ValidateAsync(profileDetails);
        if (!validationResult.IsValid)
        {
            return Results.ValidationProblem(validationResult.ToDictionary());
        }

        var response = await httpClient.PostAsJsonAsync("/profile", profileDetails);
        var location = response.Headers.Location?.ToString();
        Console.WriteLine($"Profile {profileDetails.Id} sent to the ProfileService via service invocation.");

        return Results.Created(location, value: null);
    }
);

app.Run();

record SocialProfileDetails(string Id, string Name, string Discord, string Bluesky, string Linkedin);