using DomainEntity = Financial.Domain.Entity;

namespace Financial.Application.UseCases.Account.GetAccount;
public class GetAccountOutput
{
    public Guid Id { get; set; }
    public Guid UserId { get; set; }
    public Decimal OpeningBalance { get; set; }
    public string Name { get; set; }
    public bool IsActive { get; set; }
    public bool OpeningBalanceIsNegative { get; set; }
    public DateTime CreatedAt { get; set; }
    public GetAccountOutput(
        Guid id,
        Guid userId,
        DateTime createdAt,
        string name,
        Decimal openingBalance,
        bool openingBalanceIsNegative,
        bool isActive
        )
    {
        Id = id;
        UserId = userId;
        CreatedAt = createdAt;
        Name = name;
        OpeningBalance = openingBalance;
        OpeningBalanceIsNegative = openingBalanceIsNegative;
        IsActive = isActive;
    }

    public static GetAccountOutput FromAccount(DomainEntity.Account account)
        => new (
            account.Id,
            account.UserId,
            account.CreatedAt,
            account.Name,
            account.OpeningBalance,
            account.OpeningBalanceIsNegative,
            account.IsActive
        );
}
