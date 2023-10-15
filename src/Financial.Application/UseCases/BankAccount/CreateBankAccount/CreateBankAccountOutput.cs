using DomainEntity = Financial.Domain.Entity;

namespace Financial.Application.UseCases.BankAccount.CreateBankAccount;
public class CreateBankAccountOutput
{
    public Guid Id { get; set; }
    public Guid UserId { get; set; }
    public decimal OpeningBalance { get; set; }
    public string Name { get; set; }
    public bool IsActive { get; set; }
    public bool OpeningBalanceIsNegative { get; set; }
    public DateTime CreatedAt { get; set; }
    public CreateBankAccountOutput(
        Guid id,
        Guid userId,
        DateTime createdAt,
        string name,
        decimal openingBalance,
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

    public static CreateBankAccountOutput FromAccount(DomainEntity.BankAccount account)
        => new CreateBankAccountOutput(
            account.Id,
            account.UserId,
            account.CreatedAt,
            account.Name,
            account.OpeningBalance,
            account.OpeningBalanceIsNegative,
            account.IsActive
        );
}
