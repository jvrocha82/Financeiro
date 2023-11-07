using Financial.Application.UseCases.BankAccount.Common;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Financial.Application.UseCases.BankAccount.UpdateBankAccount;
public interface IUpdateBankAccount
    : IRequestHandler<UpdateBankAccountInput, BankAccountModelOutput>
{
}
