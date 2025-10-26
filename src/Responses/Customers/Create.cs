namespace BugStore.Responses.Customers;

public class Create
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string Message { get; set; } = "Cliente criado com sucesso!";
}