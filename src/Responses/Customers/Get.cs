namespace BugStore.Responses.Customers;

public class Get
{
    public List<Responses.Customers.GetById> Customers { get; set; } = new List<Responses.Customers.GetById>();
}