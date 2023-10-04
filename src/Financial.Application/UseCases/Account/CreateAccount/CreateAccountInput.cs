namespace Financial.Application.UseCases.Account.CreateAccount;
public class CreateAccountInput
{
    public Guid UserId { get; set; }
    public int OpeningBalance { get; set; }
    public string Name { get; set; }
    public bool IsActive { get; set; }
    public bool OpeningBalanceIsNegative { get; set; }
    public CreateAccountInput(Guid userId, string name, int openingBalance, bool openingBalanceIsNegative = false, bool isActive = true)
    {
        UserId = userId;
        Name = name;
        OpeningBalance = openingBalance;
        OpeningBalanceIsNegative = openingBalanceIsNegative;
        IsActive = isActive;
    }
}
