using Azure.Identity;
using Microsoft.Azure.Cosmos;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddRazorPages(); // razor needs for ./Pages/_Host.cshtml page
builder.Services.AddServerSideBlazor(); //и blazor

if (builder.Environment.IsDevelopment())
{
    builder.Services.AddSingleton<CosmosClient>((_) =>
    {
        // <create_client>
        CosmosClient client = new(
            accountEndpoint: builder.Configuration["AZURE_COSMOS_DB_NOSQL_ENDPOINT"]!,
            tokenCredential: new DefaultAzureCredential() // рекомендуют такую штуку как универсальную
        );
        // </create_client>
        return client;
    });
}
else
{// для release
    builder.Services.AddSingleton<CosmosClient>((_) =>
    {
        // <create_client_client_id>
        CosmosClient client = new(
            accountEndpoint: builder.Configuration["AZURE_COSMOS_DB_NOSQL_ENDPOINT"]!,
            tokenCredential: new DefaultAzureCredential(
                new DefaultAzureCredentialOptions()
                {
                    ManagedIdentityClientId = builder.Configuration["AZURE_MANAGED_IDENTITY_CLIENT_ID"]!
                }
            )
        );
        // </create_client_client_id>
        return client;
    });
}

builder.Services.AddTransient<ICosmosDbService, CosmosDbService>();

var app = builder.Build();

app.UseDeveloperExceptionPage();

app.UseHttpsRedirection();

app.UseStaticFiles();

app.UseRouting();

app.MapBlazorHub(); // blazor
app.MapFallbackToPage("/_Host"); // refers to ./Pages/_Host.cshtml

app.Run();
