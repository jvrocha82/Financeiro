using Financial.Domain.Exceptions;
using Financial.Domain.SeedWork;
using Financial.Domain.Validation;

namespace Financial.Domain.Entity;
public class Account : AggregateRoot
{
    public Decimal OpeningBalance { get; private set; }
    public string Name { get; private set; }                
    public DateTime CreatedAt { get; private set; }
    public bool IsActive { get; private set; }
    public bool OpeningBalanceIsNegative { get; private set; }

    public Account(string name, Decimal openingBalance, bool openingBalanceIsNegative = false, bool isActive = true)
        : base()
    {
        
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

