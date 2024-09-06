using Dima.Api.Data;
using Dima.Api.Endpoints;
using Dima.Api.Handlers;
using Dima.Api.Models;
using Dima.Core.Handlers;
using Dima.Core.Models;
using Dima.Core.Requests.Categories;
using Dima.Core.Responses;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Swashbuckle.AspNetCore.Swagger;
using Swashbuckle.AspNetCore.SwaggerGen;
using System.Security.Claims;

var builder = WebApplication.CreateBuilder(args);

var connectionString = builder
    .Configuration
    .GetConnectionString("DefaultConnection") ?? string.Empty;

builder
    .Services.
    AddEndpointsApiExplorer();

builder
    .Services
    .AddSwaggerGen(options =>
    {
        options.CustomSchemaIds(n => n.FullName);
    });

builder
    .Services
    .AddAuthentication(IdentityConstants.ApplicationScheme)
    .AddIdentityCookies();

builder
    .Services
    .AddAuthorization();

builder
    .Services
    .AddDbContext<AppDbContext>(x =>
        {
            x.UseSqlServer(connectionString);
        });

builder
    .Services
    .AddIdentityCore<User>()
    .AddRoles<IdentityRole<long>>()
    .AddEntityFrameworkStores<AppDbContext>()
    .AddApiEndpoints();

builder
    .Services
    .AddTransient<ICategoryHandler, CategoryHandler>();

builder
    .Services
    .AddTransient<ITransactionHandler, TransactionHandler>();

var app = builder.Build();

app.UseAuthentication();
app.UseAuthorization();

app.UseSwagger();
app.UseSwaggerUI();

app.MapGet("/", () => new { message = "OK"});
app.MapEndpoints();

app.MapGroup("v1/identity")
    .WithTags("Identity")
    .MapIdentityApi<User>();

app.MapGroup("v1/identity")
    .WithTags("Identity")
    .MapPost("/logout", async (SignInManager<User> signInManager) =>
    {
        await signInManager.SignOutAsync();
        return Results.Ok();
    })
    .RequireAuthorization();

app.MapGroup("v1/identity")
    .WithTags("Identity")
    .MapPost("/roles", async (ClaimsPrincipal user) =>
    {
        if (user.Identity is null || !user.Identity.IsAuthenticated)
            return Results.Unauthorized();

        var identity = (ClaimsIdentity)user.Identity;
        var roles = identity.FindAll(identity.RoleClaimType).Select(x => new
        {
            x.Issuer,
            x.OriginalIssuer,
            x.Type,
            x.Value,
            x.ValueType
        });

        return TypedResults.Json(roles);
    })
    .RequireAuthorization();

app.Run();

// user-secrets
// dotnet user-secrets set "ConnectionStrings:DefaultConnection" "Server=NBKOYAMA\MSSQLSERVER01;Database=dima-dev;User ID=sa;Password=dp;Trusted_Connection=False;TrustServerCertificate=True;"