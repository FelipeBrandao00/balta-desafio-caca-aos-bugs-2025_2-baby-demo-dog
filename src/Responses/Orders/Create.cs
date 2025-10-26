
using BugStore.Models;

namespace BugStore.Responses.Orders;

public class Create
{
    public Guid Id { get; set; }
    public Guid CustomerId { get; set; }
    public Responses.Customers.GetById Customer { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }

    public List<OrderLine> Lines { get; set; } = null;
}