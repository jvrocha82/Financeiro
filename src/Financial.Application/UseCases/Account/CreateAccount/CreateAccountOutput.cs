namespace Financial.Application.UseCases.Account;
public class CreateAccountOutput
{
    public Guid Id { get; set; }
    public Guid UserId { get; set; }
    public Decimal OpeningBalance { get; set; }
    public string Name { get; set; }
    public bool IsActive { get; set; }
    public bool OpeningBalanceIsNegative { get; set; }
    public DateTime CreatedAt { get; set; }
   public CreateAccountOutput(
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
}
