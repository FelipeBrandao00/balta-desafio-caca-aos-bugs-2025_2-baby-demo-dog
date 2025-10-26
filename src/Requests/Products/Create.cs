namespace BugStore.Requests.Products;

public class Create
{
    public string Title { get; set; }
    public string Description { get; set; }
    public decimal Price { get; set; }
}