using BugStore.Handlers.Customers;

namespace BugStore.Endpoints;

public static class CustomerEndpoints
{
    public static void MapCustomerEndpoints(this WebApplication app)
    {
        var group = app.MapGroup("/v1/customers").WithTags("Customers");

        group.MapGet("", async (CustomerHandler handler) =>
        {
            var request = new Requests.Customers.Get();
            var customers = await handler.GetAllAsync(request);
            return Results.Ok(customers);
        })
        .WithName("GetAllCustomers")
        .Produces<List<Responses.Customers.Get>>(200);

        group.MapGet("{id}", async (Guid id, CustomerHandler handler) =>
        {
            var request = new Requests.Customers.GetById { Id = id };
            var customer = await handler.GetByIdAsync(request);
            return customer is not null ? Results.Ok(customer) : Results.NotFound();
        })
        .WithName("GetCustomerById")
        .Produces<Responses.Customers.GetById>(200)
        .Produces(404);

        group.MapPost("", async (Requests.Customers.Create request, CustomerHandler handler) =>
        {
            var response = await handler.CreateAsync(request);
            return Results.Created($"/v1/customers/{response.Id}", response);
        })
        .WithName("CreateCustomer")
        .Produces<Responses.Customers.Create>(201);

        group.MapPut("{id}", async (Guid id, Requests.Customers.Update request, CustomerHandler handler) =>
        {
            if (id != request.Id)
            {
                return Results.BadRequest("Id na url é diferente do Id no corpo da requisição.");
            }
            var response = await handler.UpdateAsync(request);
            return response is not null ? Results.Ok(response) : Results.NotFound();
        })
        .WithName("UpdateCustomer")
        .Produces<Responses.Customers.Update>(200)
        .Produces(404);

        group.MapDelete("{id}", async (Guid id, CustomerHandler handler) =>
        {
            var request = new Requests.Customers.Delete { Id = id };
            var deleted = await handler.DeleteAsync(request);
            return deleted!= null ? Results.NoContent() : Results.NotFound();
        })
        .WithName("DeleteCustomer")
        .Produces(204)
        .Produces(404);
    }
}
