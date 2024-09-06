﻿using Dima.Api.Common.Api;
using Dima.Core.Handlers;
using Dima.Core.Models;
using Dima.Core.Requests.Categories;
using Dima.Core.Responses;
using System.Security.Claims;

namespace Dima.Api.Endpoints.Categories
{
    public class UpdateCategoryEndpoint : IEndpoint
    {
        public static void Map(IEndpointRouteBuilder app)
            => app.MapPut("/{id}", HandleAsync)
                      .WithName("Catogories: Update")
                      .WithSummary("Atualiza uma nova categoria")
                      .WithDescription("Atualiza uma nova categoria")
                      .WithOrder(2)
                      .Produces<Response<Category?>>();

        private static async Task<IResult> HandleAsync(
            ClaimsPrincipal user,
            ICategoryHandler handler,
            UpdateCategoryRequest request,
            long id)
        {
            request.UserId = user.Identity?.Name ?? string.Empty;
            request.Id = id;

            var result = await handler.UpdateAsync(request);
            return result.Sucess
                ? TypedResults.Created($"/{result.Data?.Id}", result)
                : TypedResults.BadRequest(result);
        }
    }
}
