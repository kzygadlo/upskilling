using Azure.Identity;
using Azure.Security.KeyVault.Secrets;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Identity.Web;
using System;
using System.Web;

var builder = WebApplication.CreateBuilder(args);

// 1. CORS (Cross-Origin Resource Sharing)
builder.Services.AddCors(options =>
{
    options.AddPolicy("FrontendApp", policy =>
    {
        policy.WithOrigins("https://trustedfrontend.com")
              .WithMethods("GET", "POST")
              .WithHeaders("Content-Type", "Authorization");
    });
});

// 2. SECRETS MANAGEMENT (Azure Key Vault)
if (!builder.Environment.IsDevelopment())
{
    var keyVaultUri = new Uri("https://my-secure-vault.vault.azure.net/");
    var secretClient = new SecretClient(keyVaultUri, new DefaultAzureCredential());
    
    KeyVaultSecret dbSecret = secretClient.GetSecret("SqlConnectionString");
    builder.Configuration["ConnectionStrings:DefaultConnection"] = dbSecret.Value;
}

// 3. OAUTH2 & IDENTITY SYSTEMS (Azure Entra ID)
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddMicrosoftIdentityWebApi(builder.Configuration.GetSection("AzureAd"));

builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("RequireAdminRole", policy => 
        policy.RequireClaim("http://schemas.microsoft.com/ws/2008/06/identity/claims/role", "Admin"));
});

var app = builder.Build();

app.UseCors("FrontendApp");
app.UseHttpsRedirection(); // Enforce HTTPS/TLS
app.UseAuthentication();
app.UseAuthorization();

// 4. BASIC INPUT SANITIZATION (XSS Protection)
app.MapPost("/api/comments", (CommentRequest request) =>
{
    // Encode special HTML characters
    string safeComment = HttpUtility.HtmlEncode(request.RawContent);
    return Results.Ok(new { CleanContent = safeComment });
});

// 5. SECURE API DESIGN (Protected Endpoints)
app.MapGet("/api/admin-dashboard", () => Results.Ok("Admin Data"))
   .RequireAuthorization("RequireAdminRole");

app.Run();

public record CommentRequest(string RawContent);