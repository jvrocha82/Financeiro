using MediatR;

namespace Financial.Application.UseCases.BankAccount.CreateBankAccount;
public class CreateBankAccountInput : IRequest<CreateBankAccountOutput>
{
    public Guid UserId { get; set; }
    public int OpeningBalance { get; set; }
    public string Name { get; set; }
    public bool IsActive { get; set; }
    public bool OpeningBalanceIsNegative { get; set; }
    public CreateBankAccountInput(Guid userId, string name, int openingBalance, bool openingBalanceIsNegative = false, bool isActive = true)
    {
        UserId = userId;
        Name = name;
        OpeningBalance = openingBalance;
        OpeningBalanceIsNegative = openingBalanceIsNegative;
        IsActive = isActive;
    }
}
