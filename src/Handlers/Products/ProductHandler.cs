using BugStore.Data;
using BugStore.Models;

using Microsoft.EntityFrameworkCore;

namespace BugStore.Handlers.Products;

public class ProductHandler
{
    private readonly AppDbContext _context;

    public ProductHandler(AppDbContext context)
    {
        _context = context;
    }

    public async Task<Responses.Products.Get> GetAllAsync(Requests.Products.Get request)
    {
        var products = await _context.Products.ToListAsync();

        return new Responses.Products.Get
        {
            Products = products.Select(c => new Responses.Products.GetById
            {
                Id = c.Id,
                Title = c.Title,
                Description = c.Description,
                Slug = c.Slug,
                Price = c.Price
            })
            .ToList()
        };
    }

    public async Task<Responses.Products.GetById?> GetByIdAsync(Requests.Products.GetById request)
    {
        var product = await _context.Products.FindAsync(request.Id);
        if (product is null)
        {
            return null;
        }
        return new Responses.Products.GetById
        {
            Id = product.Id,
            Title = product.Title,
            Description = product.Description,
            Slug = product.Slug,
            Price = product.Price
        };
    }

    public async Task<Responses.Products.Create> CreateAsync(Requests.Products.Create request)
    {
        var product = new Product
        {
            Title = request.Title,
            Description = request.Description,
            Slug = GenerateSlug(request.Title),
            Price = request.Price
        };
        await _context.Products.AddAsync(product);
        await _context.SaveChangesAsync();
        return new Responses.Products.Create
        {
            Id = product.Id,
            Title = product.Title,
            Description = product.Description,
            Slug = product.Slug,
            Price = product.Price
        };
    }

    public async Task<Responses.Products.Update?> UpdateAsync(Requests.Products.Update request)
    {
        var product = await _context.Products.FindAsync(request.Id);
        if (product is null)
        {
            return null;
        }
        product.Title = request.Title;
        product.Description = request.Description;
        product.Slug = GenerateSlug(request.Title);
        product.Price = request.Price;
        _context.Products.Update(product);
        await _context.SaveChangesAsync();
        return new Responses.Products.Update
        {
            Id = product.Id,
            Title = product.Title,
            Description = product.Description,
            Slug = product.Slug,
            Price = product.Price
        };
    }

    public async Task<Responses.Products.Delete?> DeleteAsync(Requests.Products.Delete request)
    {
        var product = await _context.Products.FindAsync(request.Id);
        if (product is null)
        {
            return null;
        }
        _context.Products.Remove(product);
        await _context.SaveChangesAsync();
        return new Responses.Products.Delete
        {
            Id = product.Id,
            Title = product.Title,
            Description = product.Description,
            Slug = product.Slug,
            Price = product.Price
        };
    }


    private string GenerateSlug(string title)
    {
        return title.ToLower().Replace(' ', '-');
    }
}

            
