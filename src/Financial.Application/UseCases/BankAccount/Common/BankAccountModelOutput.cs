using DomainEntity = Financial.Domain.Entity;

namespace Financial.Application.UseCases.BankAccount.Common;
public class BankAccountModelOutput
{
    public Guid Id { get; set; }
    public Guid UserId { get; set; }
    public decimal OpeningBalance { get; set; }
    public string Name { get; set; }
    public bool IsActive { get; set; }
    public bool OpeningBalanceIsNegative { get; set; }
    public DateTime CreatedAt { get; set; }
    public BankAccountModelOutput(
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

    public static BankAccountModelOutput FromBankAccount(DomainEntity.BankAccount bankAccount)
        => new(
            bankAccount.Id,
            bankAccount.UserId,
            bankAccount.CreatedAt,
            bankAccount.Name,
            bankAccount.OpeningBalance,
            bankAccount.OpeningBalanceIsNegative,
            bankAccount.IsActive
        );
}
