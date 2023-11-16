using Financial.Application.UseCases.BankAccount.CreateBankAccount;
using Financial.UnitTests.Application.BankAccount.Common;
using Xunit;

namespace Financial.UnitTests.Application.BankAccount.CreateBankAccount;

[CollectionDefinition(nameof(CreateBankAccountTestFixture))]
public class CreateAccountTestFixtureCollection
    : ICollectionFixture<CreateBankAccountTestFixture>
{ }
public class CreateBankAccountTestFixture : BankAccountUseCasesBaseFixture
{
    public CreateBankAccountInput GetInput()
        => new(
            GetValidUserId(),
            GetValidName(),
            GetValidOpeningBalance(),
            GetRandomBoolean(),
            GetRandomBoolean()
        );

    public CreateBankAccountInput GetInvalidInputShortName()
    {
        var invalidInputShortName = GetInput();
        invalidInputShortName.Name = invalidInputShortName.Name[..2];

        return invalidInputShortName;

    }
    public CreateBankAccountInput GetInvalidInputLongName()
    {
        var invalidInputTooLongName = GetInput();
        invalidInputTooLongName.Name = Faker.Lorem.Letter(256);
        return invalidInputTooLongName;
    }
    public CreateBankAccountInput GetInvalidInputWithNegativeOpeningBalance()
    {

        var invalidInputNegativeOpeningBalance = GetInput();
        invalidInputNegativeOpeningBalance.OpeningBalance *= -1;
        return invalidInputNegativeOpeningBalance;
    }




}
