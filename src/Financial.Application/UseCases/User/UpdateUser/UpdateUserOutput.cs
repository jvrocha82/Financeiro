
using DomainEntity = Financial.Domain.Entity;
namespace Financial.Application.UseCases.User.UpdateUser;
public class GetUserOutput
{
    

    public Guid Id { get; set; }
    public string Name { get; set; }
    public bool IsActive { get; set; }
    public DateTime CreatedAt { get; set; }

    public GetUserOutput(
        Guid id,
        string name,
        bool isActive,
        DateTime createdAt)
    {
        Id = id;
        Name = name;
        IsActive = isActive;
        CreatedAt = createdAt;
    }

        public static GetUserOutput FromUser(DomainEntity.User user)
        => new(
                user.Id,
                user.Name,
                user.IsActive,
                user.CreatedAt
            );


}
