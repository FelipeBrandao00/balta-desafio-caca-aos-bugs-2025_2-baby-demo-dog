namespace BugStore.Endpoints;

public static class OrderEndpoints
{
    public static void MapOrderEndpoints(this WebApplication app)
    {
        var orders = app.MapGroup("/v1/orders").WithTags("Orders");

        orders.MapGet("/v1/orders/{id}", async(Guid id, Handlers.Orders.OrderHandler handler) =>
        {
            var result = await handler.GetByIdAsync(new Requests.Orders.GetById { Id = id });
            return result is not null ? Results.Ok(result) : Results.NotFound();
        })
        .WithName("GetById")
        .Produces<Responses.Orders.GetById>(200)
        .Produces(404); ;

        orders.MapPost("/v1/orders", async(Requests.Orders.Create request, Handlers.Orders.OrderHandler handler) =>
        {
            var result =  await handler.CreateAsync(request);
            return Results.Created($"/v1/orders/{result.Id}", result);
        })
        .WithName("CreateOrder")
        .Produces<Responses.Orders.Create>(201); ;
    }
}
