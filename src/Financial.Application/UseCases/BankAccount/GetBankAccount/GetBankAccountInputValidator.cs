using FluentValidation;

namespace Financial.Application.UseCases.BankAccount.GetBankAccount;
public class GetBankAccountInputValidator
    : AbstractValidator<GetBankAccountInput>
{
    public GetBankAccountInputValidator()
        => RuleFor(x => x.Id).NotEmpty();
    
}
