namespace BugStore.Responses.Customers;

public class GetById
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string Phone { get; set; } = string.Empty;
    public DateTime BirthDate { get; set; }
    public string Message { get; set; } = "Cliente encontrado com sucesso!";
}