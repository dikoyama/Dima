﻿using Dima.Api.Common.Api;
using Dima.Core;
using Dima.Core.Handlers;
using Dima.Core.Models;
using Dima.Core.Requests.Categories;
using Dima.Core.Responses;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Dima.Api.Endpoints.Categories
{
    public class GetAllCategoriesEndpoint : IEndpoint
    {
        public static void Map(IEndpointRouteBuilder app)
            => app.MapGet("/", HandleAsync)
                  .WithName("Catogories: Get All")
                  .WithSummary("Recupera todas as categoria")
                  .WithDescription("Recupera todas as categoria")
                  .WithOrder(5)
                  .Produces<PagedResponse<List<Category>?>>();

        private static async Task<IResult> HandleAsync(
            ClaimsPrincipal user,
            ICategoryHandler handler,
            [FromQuery]int pageNumber = Configuration.DefaultPageNumber,
            [FromQuery]int pageSize = Configuration.DefaultPageSize)
        {
            var request = new GetAllCategoriesRequest
            {
                UserId = user.Identity?.Name ?? string.Empty,
                PageNumber = pageNumber,
                PageSize = pageSize
                
            };

            var result = await handler.GetAllAsync(request);
            return result.Sucess
                ? TypedResults.Ok(result)
                : TypedResults.BadRequest(result);
        }
    }
}