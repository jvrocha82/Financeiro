using Financial.Application.UseCases.BankAccount.Common;
using MediatR;

namespace Financial.Application.UseCases.BankAccount.UpdateBankAccount;
public class UpdateBankAccountInput : IRequest<BankAccountModelOutput>
{


    public Guid Id { get; set; }
    public string Name { get; set; }
    public int? OpeningBalance { get; set; }
    public bool? IsActive { get; set; }

    public UpdateBankAccountInput(
        Guid id,
        string name,
        int? openingBalance = null,
        bool? isActive = null
    )
    {
        Id = id;
        Name = name;
        OpeningBalance = openingBalance;
        IsActive = isActive;
    }
}
