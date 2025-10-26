namespace BugStore.Responses.Products;

public class Get
{
    public List<GetById> Products { get; set; } = new List<GetById>();
}