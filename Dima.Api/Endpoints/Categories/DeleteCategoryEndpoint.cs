﻿using Dima.Api.Common.Api;
using Dima.Core.Handlers;
using Dima.Core.Models;
using Dima.Core.Requests.Categories;
using Dima.Core.Responses;
using System.Security.Claims;

namespace Dima.Api.Endpoints.Categories
{
    public class DeleteCategoryEndpoint : IEndpoint
    {
        public static void Map(IEndpointRouteBuilder app)
            => app.MapDelete("/", HandleAsync)
                  .WithName("Catogories: Delete")
                  .WithSummary("Exclui uma categoria")
                  .WithDescription("Exclui uma categoria")
                  .WithOrder(3)
                  .Produces<Response<Category?>>();

        private static async Task<IResult> HandleAsync(
            ClaimsPrincipal user,
            ICategoryHandler handler, 
            long id)
        {
            var request = new DeleteCategoryRequest 
            {
                UserId = user.Identity?.Name ?? string.Empty,
                Id = id
            };
            var result = await handler.DeleteAsync(request);
            return result.Sucess
                ? TypedResults.Ok(result)
                : TypedResults.BadRequest(result);
        }
    }
}
