using Financial.Domain.Exceptions;
using Financial.Domain.SeedWork;

namespace Financial.Domain.Entity;
public class Account : AggregateRoot
{
    public int OpeningBalance { get; private set; }
    public string Name { get; private set; }                
    public DateTime CreatedAt { get; private set; }
    public bool IsActive { get; private set; }
    public bool OpeningBalanceIsNegative { get; private set; }

    public Account(string name, int openingBalance, bool openingBalanceIsNegative = false, bool isActive = true)
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
        if (String.IsNullOrWhiteSpace(Name))
            throw new EntityValidationException($"{nameof(Name)} should not be empty or null");
        if(Name.Length < 3)
            throw new EntityValidationException($"{nameof(Name)} should be at leats 3 characters long");
        if(Name.Length > 255)
            throw new EntityValidationException($"{nameof(Name)} should be less or equal 255 characters long");
        if (OpeningBalance < 0)
            throw new EntityValidationException($"{nameof(OpeningBalance)} cannot be negative value");
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
    public void Update(string name, int? openingBalance = null)
    {
        Name = name;
        OpeningBalance = openingBalance ?? OpeningBalance;
        Validate();
    }

}

