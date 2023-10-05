namespace Financial.Application.UseCases.User.CreateUser;
public class CreateUserOutput
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public bool IsActive { get; set; }
    public DateTime CreatedAt { get; set; }


    public CreateUserOutput(
        Guid id,
        string name,
        bool isActive,
        DateTime createdAt
        )
    {
        Id = id;
        CreatedAt = createdAt;
        Name = name;
        IsActive = isActive;
    }
}
