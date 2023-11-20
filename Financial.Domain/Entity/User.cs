using Financial.Domain.SeedWork;
using Financial.Domain.Validation;
using System.Xml.Linq;

namespace Financial.Domain.Entity;
public class User : AggregateRoot
{

    public string Name { get; private set; }
    public DateTime CreatedAt { get; private set; }
    public bool IsActive { get; private set; }

    public User(string name, bool isActive = true)
    : base()
    {
        Name = name;
        CreatedAt = DateTime.Now;
        IsActive = isActive;

        Validate();
    }
    public void Validate()
    {
        DomainValidation.NotNullOrEmpty(Name, nameof(Name));
        DomainValidation.MinLength(Name, 3, nameof(Name));
        DomainValidation.MaxLength(Name, 255, nameof(Name));
    }
    public void Update(string name)
    {
        Name = name;
        Validate();
    }

}
