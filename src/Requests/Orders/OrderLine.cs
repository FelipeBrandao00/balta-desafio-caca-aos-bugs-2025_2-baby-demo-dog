namespace BugStore.Requests.Orders;

public class OrderLine
{
    public Guid OrderId { get; set; }
    public int Quantity { get; set; }
    public decimal Total { get; set; }
    public Guid ProductId { get; set; }
}
