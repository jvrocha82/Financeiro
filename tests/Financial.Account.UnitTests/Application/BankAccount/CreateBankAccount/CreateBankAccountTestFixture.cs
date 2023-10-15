using Financial.Application.Interfaces;
using Financial.Application.UseCases.BankAccount.CreateBankAccount;
using Financial.Domain.Repository;
using Financial.UnitTests.Common;
using Moq;
using Xunit;

namespace Financial.UnitTests.Application.BankAccount.CreateBankAccount;

[CollectionDefinition(nameof(CreateBankAccountTestFixture))]
public class CreateAccountTestFixtureCollection
    : ICollectionFixture<CreateBankAccountTestFixture>
{ }
public class CreateBankAccountTestFixture : BaseFixture
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
    public static int GetValidOpeningBalance() => new Random().Next();

    public static bool GetRandomBoolean() => new Random().NextDouble() < 0.5;
    public static Guid GetValidUserId() => Guid.NewGuid();

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

    public static Mock<IBankAccountRepository> GetRepositoryMock()
        => new();

    public static Mock<IUnitOfWork> GetUnitOfWorkMock()
        => new();


}
