namespace Financial.Application.UseCases.Account.CreateAccount;
public class CreateAccountInput
{

    public int OpeningBalance { get; set; }
    public string Name { get; set; }
    public bool IsActive { get; set; }
    public bool OpeningBalanceIsNegative { get; set; }
    public CreateAccountInput(string name, int openingBalance, bool openingBalanceIsNegative = false, bool isActive = true)
    {
        Name = name;
        OpeningBalance = openingBalance;
        OpeningBalanceIsNegative = openingBalanceIsNegative;
        IsActive = isActive;
    }
}
