﻿using Financial.Application.UseCases.BankAccount.Common;
using MediatR;

namespace Financial.Application.UseCases.BankAccount.CreateBankAccount;
public class CreateBankAccountInput : IRequest<BankAccountModelOutput>
{
    public Guid UserId { get; set; }
    public decimal OpeningBalance { get; set; }
    public string Name { get; set; }
    public bool IsActive { get; set; }
    public bool OpeningBalanceIsNegative { get; set; }
    public CreateBankAccountInput(Guid userId, string name, decimal openingBalance = 0, bool openingBalanceIsNegative = false, bool isActive = true)
    {
        UserId = userId;
        Name = name;
        OpeningBalance = openingBalance;
        OpeningBalanceIsNegative = openingBalanceIsNegative;
        IsActive = isActive;
    }
}
