using Dima.Api.Data;
using Dima.Api.Endpoints;
using Dima.Api.Handlers;
using Dima.Core.Handlers;
using Dima.Core.Models;
using Dima.Core.Requests.Categories;
using Dima.Core.Responses;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Swashbuckle.AspNetCore.Swagger;
using Swashbuckle.AspNetCore.SwaggerGen;

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
    .AddTransient<ICategoryHandler, CategoryHandler>();

builder
    .Services
    .AddTransient<ITransactionHandler, TransactionHandler>();

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();

app.MapGet("/", () => new { message = "OK"});
app.MapEndpoints();

app.Run();

// user-secrets
// dotnet user-secrets set "ConnectionStrings:DefaultConnection" "Server=NBKOYAMA\MSSQLSERVER01;Database=dima-dev;User ID=sa;Password=dp;Trusted_Connection=False;TrustServerCertificate=True;"