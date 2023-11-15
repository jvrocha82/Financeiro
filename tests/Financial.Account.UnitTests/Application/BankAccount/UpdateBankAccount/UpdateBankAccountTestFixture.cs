using Financial.Application.Interfaces;
using Financial.Application.UseCases.BankAccount.UpdateBankAccount;
using Financial.Domain.Repository;
using Financial.UnitTests.Common;
using Moq;
using Xunit;
using DomainEntity = Financial.Domain.Entity;

namespace Financial.UnitTests.Application.BankAccount.UpdateBankAccount;

[CollectionDefinition(nameof(UpdateBankAccountTestFixture))]

public class UpdateBankAccountTestFixtureCollection 
    : ICollectionFixture<UpdateBankAccountTestFixture>
{ }
public class UpdateBankAccountTestFixture
    : BaseFixture
{

    public string GetValidName()
    {
        var randomName = "";

        while (randomName.Length < 3)
            randomName = Faker.Commerce.Categories(1)[0];

        if (randomName.Length > 255)
            randomName = randomName[..255];

        return randomName;
    }
    public int GetValidOpeningBalance() => new Random().Next();

    public bool GetRandomBoolean() => new Random().NextDouble() < 0.5;
    public Guid GetValidUserId() => Guid.NewGuid();
    public UpdateBankAccountInput GetValidBankAccountInput(Guid? id = null) => new (
                id ?? Guid.NewGuid(),
                GetValidName(),
                GetValidOpeningBalance(),
                GetRandomBoolean()
            );

    public DomainEntity.BankAccount GetValidBankAccount() => new(
    GetValidUserId(),
    GetValidName(),
    GetValidOpeningBalance()
    );


    public UpdateBankAccountInput GetInvalidInputShortName()
    {
        var invalidInputShortName = GetValidBankAccountInput();
        invalidInputShortName.Name = invalidInputShortName.Name[..2];

        return invalidInputShortName;

    }
    public UpdateBankAccountInput GetInvalidInputLongName()
    {
        var invalidInputTooLongName = GetValidBankAccountInput();
        invalidInputTooLongName.Name = Faker.Lorem.Letter(256);
        return invalidInputTooLongName;
    }
    public UpdateBankAccountInput GetInvalidInputWithNegativeOpeningBalance()
    {

        var invalidInputNegativeOpeningBalance = GetValidBankAccountInput();
        invalidInputNegativeOpeningBalance.OpeningBalance *= -1;
        return invalidInputNegativeOpeningBalance;
    }
    public Mock<IBankAccountRepository> GetRepositoryMock()
        => new();

    public Mock<IUnitOfWork> GetUnitOfWorkMock()
        => new();

}
