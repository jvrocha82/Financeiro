using Financial.Application.UseCases.BankAccount.UpdateBankAccount;
using Financial.IntegrationTests.Application.UseCase.BankAccount.Common;
using Xunit;
using DomainEntity = Financial.Domain.Entity;

namespace Financial.IntegrationTests.Application.UseCase.BankAccount.UpdateBankAccount;
[CollectionDefinition(nameof(UpdateBankAccountTestFixture))]
public class UpdateBankAccountTestFixtureCollection
    : ICollectionFixture<UpdateBankAccountTestFixture>
{ }

public class UpdateBankAccountTestFixture
    : BankAccountUseCaseBaseFixture
{
    public UpdateBankAccountInput GetValidBankAccountInput(Guid? id = null) => new(
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

}
