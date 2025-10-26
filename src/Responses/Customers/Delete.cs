namespace BugStore.Responses.Customers;

public class Delete
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string Message { get; set; } = "Cliente excluído com sucesso!";
}