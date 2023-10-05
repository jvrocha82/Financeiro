namespace Financial.Application.UseCases.User.CreateUser;
public class CreateUserInput
{
    
    public string Name { get; set; }
    public bool IsActive { get; set; }
    public CreateUserInput(string name, bool isActive = true)
    {
        Name = name;
        IsActive = isActive;

    }
}
