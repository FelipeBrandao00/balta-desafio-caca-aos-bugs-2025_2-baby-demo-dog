using BugStore.Data;
using BugStore.Handlers.Customers;

using Microsoft.EntityFrameworkCore;

namespace BugStore.Handlers.Orders;

public class OrderHandler
{
    private readonly AppDbContext _context;

    public OrderHandler(AppDbContext context, CustomerHandler customerHandler)
    {
        _context = context;
    }

    public async Task<Responses.Orders.GetById?> GetByIdAsync(Requests.Orders.GetById request)
    {
        var order = await _context.Orders
            .Where(o => o.Id == request.Id)
            .Include(o => o.Customer)
            .Include(o => o.Lines)
            .ThenInclude(l => l.Product)
            .FirstOrDefaultAsync();

        if (order == null) return null;

        return new Responses.Orders.GetById
        {
            Id = order.Id,
            CustomerId = order.CustomerId,
            CreatedAt = order.CreatedAt,
            UpdatedAt = order.UpdatedAt,
            Customer = new Responses.Customers.GetById
            {
                Id = order.Customer.Id,
                Name = order.Customer.Name,
                Email = order.Customer.Email
            },
            Lines = order.Lines.Select(line => new Responses.Orders.OrderLine
            {
                Id = line.Id,
                OrderId = line.OrderId,
                ProductId = line.ProductId,
                Quantity = line.Quantity,
                Total = line.Total,
                Product = new Responses.Products.GetById
                {
                    Id = line.Product.Id,
                    Title = line.Product.Title,
                    Description = line.Product.Description,
                    Slug = line.Product.Slug,
                    Price = line.Product.Price
                }
            }).ToList()
        };
    }

    public async Task<Responses.Orders.Create> CreateAsync(Requests.Orders.Create request)
    {
        var order = new Models.Order
        {
            Id = Guid.NewGuid(),
            CustomerId = request.CustomerId,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow,
            Lines = request.Lines.Select(line => new Models.OrderLine
            {
                Id = Guid.NewGuid(),
                ProductId = line.ProductId,
                Quantity = line.Quantity,
                Total = line.Total
            }).ToList()
        };
        _context.Orders.Add(order);
        await _context.SaveChangesAsync();
        return new Responses.Orders.Create
        {
            Id = order.Id,
            CustomerId = order.CustomerId,
            CreatedAt = order.CreatedAt,
            UpdatedAt = order.UpdatedAt,
            Lines = order.Lines.Select(line => new Responses.Orders.OrderLine
            {
                Id = line.Id,
                ProductId = line.ProductId,
                Quantity = line.Quantity,
                Total = line.Total,
                OrderId = line.OrderId,
                Product = new Responses.Products.GetById
                {
                    Id = line.Product.Id,
                    Title = line.Product.Title,
                    Description = line.Product.Description,
                    Slug = line.Product.Slug,
                    Price = line.Product.Price
                }
            }).ToList()
        };
    }
}
