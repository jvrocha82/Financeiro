
using FluentValidation;

namespace Financial.Application.UseCases.BankAccount.UpdateBankAccount;
public class UpdateBankAccountInputValidator
    : AbstractValidator<UpdateBankAccountInput>
{
    public UpdateBankAccountInputValidator()
    => RuleFor(x => x.Id).NotEmpty();
    
}
