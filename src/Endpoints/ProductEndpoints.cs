namespace BugStore.Endpoints;

public static class ProductEndpoints
{
    public static void MapProductEndpoints(this WebApplication app)
    {
        var group = app.MapGroup("/v1/products").WithTags("Products");

        group.MapGet("", async (Handlers.Products.ProductHandler handler) =>
        {
            var request = new Requests.Products.Get();
            var products = await handler.GetAllAsync(request);
            return Results.Ok(products);
        })
        .WithName("GetAllProducts")
        .Produces<List<Responses.Products.Get>>(200);

        group.MapGet("{id}", async (Guid id, Handlers.Products.ProductHandler handler) =>
        {
            var request = new Requests.Products.GetById { Id = id };
            var product = await handler.GetByIdAsync(request);
            return product is not null ? Results.Ok(product) : Results.NotFound();
        })
        .WithName("GetProductById")
        .Produces<Responses.Products.GetById>(200)
        .Produces(404);

        group.MapPost("", async (Requests.Products.Create request, Handlers.Products.ProductHandler handler) =>
        {
            var response = await handler.CreateAsync(request);
            return Results.Created($"/v1/products/{response.Id}", response);
        })
        .WithName("CreateProduct")
        .Produces<Responses.Products.Create>(201);

        group.MapPut("{id}", async (Guid id, Requests.Products.Update request, Handlers.Products.ProductHandler handler) =>
        {
            if (id != request.Id)
            {
                return Results.BadRequest("Id na url é diferente do Id no corpo da requisição.");
            }
            var response = await handler.UpdateAsync(request);
            return response is not null ? Results.Ok(response) : Results.NotFound();
        })
        .WithName("UpdateProduct")
        .Produces<Responses.Products.Update>(200)
        .Produces(404);

        group.MapDelete("{id}", async (Guid id, Handlers.Products.ProductHandler handler) =>
        {
            var request = new Requests.Products.Delete { Id = id };
            var deleted = await handler.DeleteAsync(request);
            return deleted!= null ? Results.NoContent() : Results.NotFound();
        })
        .WithName("DeleteProduct")
        .Produces(204)
        .Produces(404);
    }
}
