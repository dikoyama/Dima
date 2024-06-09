﻿using Dima.Api.Common.Api;
using Dima.Core.Handlers;
using Dima.Core.Models;
using Dima.Core.Requests.Categories;
using Dima.Core.Responses;

namespace Dima.Api.Endpoints.Categories
{
    public class CreateCategoryEndpoint : IEndpoint
    {
        public static void Map(IEndpointRouteBuilder app)
            => app.MapPost("/", HandleAsync)
                  .WithName("Catogories: Create")
                  .WithSummary("Cria uma nova categoria")
                  .WithDescription("Cria uma nova categoria")
                  .WithOrder(1)
                  .Produces<Response<Category?>>();

        private static async Task<IResult> HandleAsync(
            ICategoryHandler handler,
            CreateCategoryRequest request)
        {
            request.UserId = "diego@teste.com.br";
            var result = await handler.CreateAsync(request);
            return result.Sucess 
                ? TypedResults.Created($"/{result.Data?.Id}", result) 
                : TypedResults.BadRequest(result);
        }
    }
}
