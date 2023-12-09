using Financial.Application.UseCases.BankAccount.CreateBankAccount;
using Financial.IntegrationTests.Application.UseCase.BankAccount.Common;
using Xunit;

namespace Financial.IntegrationTests.Application.UseCase.BankAccount.CreateBankAccount;

[CollectionDefinition(nameof(CreateBankAccountTestFixture))]
public class CreateBankAccountTestFixtureCollection
    : ICollectionFixture<CreateBankAccountTestFixture>
{ }
public class CreateBankAccountTestFixture
    : BankAccountUseCaseBaseFixture
{
    public CreateBankAccountInput GetInput()
    {
        var bankAccount = GetExampleBankAccount();
        return new CreateBankAccountInput(bankAccount.UserId, bankAccount.Name, bankAccount.OpeningBalance);
    }

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
