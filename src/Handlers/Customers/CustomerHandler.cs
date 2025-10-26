using BugStore.Data;
using BugStore.Models;

using Microsoft.EntityFrameworkCore;

namespace BugStore.Handlers.Customers;

public class CustomerHandler
{
    private readonly AppDbContext _context;

    public CustomerHandler(AppDbContext context)
    {
        _context = context;
    }

    public async Task<Responses.Customers.Create> CreateAsync(Requests.Customers.Create request)
    {
        var customer = new Customer
        {
            Id = Guid.NewGuid(),
            Name = request.Name,
            Email = request.Email,
            Phone = request.Phone,
            BirthDate = request.BirthDate
        };

        await _context.Customers.AddAsync(customer);
        await _context.SaveChangesAsync();

        return new Responses.Customers.Create
        {
            Id = customer.Id,
            Name = customer.Name,
            Email = customer.Email
        };
    }

    public async Task<Responses.Customers.Delete?> DeleteAsync(Requests.Customers.Delete request)
    {
        var customer = await _context.Customers.FindAsync(request.Id);
        if (customer is null)
        {
            return null;
        }
        _context.Customers.Remove(customer);
        await _context.SaveChangesAsync();
        return new Responses.Customers.Delete
        {
            Id = customer.Id,
            Name = customer.Name,
            Email = customer.Email
        };
    }

    public async Task<Responses.Customers.GetById?> GetByIdAsync(Requests.Customers.GetById request)
    {
        var customer = await _context.Customers.FindAsync(request.Id);
        if (customer is null)
        {
            return null;
        }
        return new Responses.Customers.GetById
        {
            Id = customer.Id,
            Name = customer.Name,
            Email = customer.Email,
            Phone = customer.Phone,
            BirthDate = customer.BirthDate
        };
    }

    public async Task<Responses.Customers.Get> GetAllAsync(Requests.Customers.Get request)
    {
        var customers = await _context.Customers.ToListAsync();

        return new Responses.Customers.Get
        {
            Customers = customers.Select(c => new Responses.Customers.GetById
            {
                Id = c.Id,
                Name = c.Name,
                Email = c.Email,
                Phone = c.Phone,
                BirthDate = c.BirthDate
            })
            .ToList()
        };
    }

    public async Task<Responses.Customers.Update?> UpdateAsync(Requests.Customers.Update request)
    {
        var customer = await _context.Customers.FindAsync(request.Id);
        if (customer is null)
        {
            return null;
        }
        customer.Name = request.Name;
        customer.Email = request.Email;
        customer.Phone = request.Phone;
        customer.BirthDate = request.BirthDate;
        _context.Customers.Update(customer);
        await _context.SaveChangesAsync();
        return new Responses.Customers.Update
        {
            Id = customer.Id,
            Name = customer.Name,
            Email = customer.Email,
            Phone = customer.Phone,
            BirthDate = customer.BirthDate
        };
    }
}