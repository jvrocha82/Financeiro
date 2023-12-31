﻿using Financial.Domain.SeedWork;
using Financial.Domain.Validation;

namespace Financial.Domain.Entity;
public class BankAccount : AggregateRoot
{
    public Guid UserId { get; private set; }
    public Decimal OpeningBalance { get; private set; }
    public string Name { get; private set; }                
    public DateTime CreatedAt { get; private set; }
    public bool IsActive { get; private set; }
    public bool OpeningBalanceIsNegative { get; private set; }

    public BankAccount(Guid userId, string name, Decimal openingBalance, bool openingBalanceIsNegative = false, bool isActive = true)
        : base()
    {
        UserId = userId;
        CreatedAt = DateTime.Now;
        IsActive = isActive;
        Name = name;
        OpeningBalance = openingBalance;
        OpeningBalanceIsNegative = openingBalanceIsNegative;
        Validate();
    }
    private void Validate()
    {
        DomainValidation.NotNullOrEmpty(Name, nameof(Name));
        DomainValidation.MinLength(Name, 3, nameof(Name));
        DomainValidation.MaxLength(Name, 255, nameof(Name));
        DomainValidation.IsNegativeDecimalValue(OpeningBalance, nameof(OpeningBalance));

    }
    public void Activate()
    {
        IsActive = true;
        Validate();
    }
    public void Deactivate()
    {
        IsActive = false;
        Validate();
    }
    public void Update(string name, Decimal? openingBalance = null)
    {
        Name = name;
        OpeningBalance = openingBalance ?? OpeningBalance;
        Validate();
    }

}

