using MediatR;

namespace Financial.Application.UseCases.BankAccount.DeleteBankAccount;
public interface IDeleteBankAccount 
    : IRequestHandler<DeleteBankAccountInput>
{}
