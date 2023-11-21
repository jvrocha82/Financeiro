using DomainEntity = Financial.Domain.Entity;

namespace Financial.Application.UseCases.User.GetUser;
public class GetUserOutput
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public bool IsActive { get; set; }
    public DateTime CreatedAt { get; set; }


    public GetUserOutput(
        Guid id,
        string name,
        DateTime createdAt,
        bool isActive

        )
    {
        Id = id;
        CreatedAt = createdAt;
        Name = name;
        IsActive = isActive;
    }

    public static GetUserOutput FromUser(DomainEntity.User user)
    => new(
           user.Id,
           user.Name,
           user.CreatedAt,
           user.IsActive

       );
}
