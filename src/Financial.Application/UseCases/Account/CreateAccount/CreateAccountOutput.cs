namespace Financial.Application.UseCases.Account;
public class CreateAccountOutput
{
    public Guid Id { get; set; }
    public int OpeningBalance { get; set; }
    public string Name { get; set; }
    public bool IsActive { get; set; }
    public bool OpeningBalanceIsNegative { get; set; }
    public DateTime CreatedAt { get; set; }
   public CreateAccountOutput(
       Guid id,
       DateTime createdAt,
       string name,
       int openingBalance,
       bool openingBalanceIsNegative,
       bool isActive
       )
    {
        Id = id;
        CreatedAt = createdAt;
        Name = name;
        OpeningBalance = openingBalance;
        OpeningBalanceIsNegative = openingBalanceIsNegative;
        IsActive = isActive;
    }
}
