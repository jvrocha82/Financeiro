namespace Financial.Domain.Entity;
public class User
{
    
    public Guid Id { get; private set; }
    public string Name { get; private set; }
    public DateTime CreatedAt { get; private set; }
    public bool IsActive { get; private set; }

    public User(string name, bool isActive = true)
    {
        Id = Guid.NewGuid();
        Name = name;
        CreatedAt = DateTime.Now;
        IsActive = isActive;
    }

}
