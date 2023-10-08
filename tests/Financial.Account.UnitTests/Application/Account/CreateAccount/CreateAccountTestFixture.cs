using Financial.Application.Interfaces;
using Financial.Application.UseCases.Account.CreateAccount;
using Financial.Domain.Repository;
using Financial.UnitTests.Common;
using Moq;
using Xunit;

namespace Financial.UnitTests.Application.CreateAccount;

[CollectionDefinition(nameof(CreateAccountTestFixture))]
public class CreateAccountTestFixtureCollection 
    : ICollectionFixture<CreateAccountTestFixture>
{}
public class CreateAccountTestFixture : BaseFixture
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

    public static bool GetRandomBoolean() => (new Random()).NextDouble() < 0.5;
    public static Guid GetValidUserId() => Guid.NewGuid();

    public CreateAccountInput GetInput() 
        => new(
            GetValidUserId(),
            GetValidName(),
            GetValidOpeningBalance(),
            GetRandomBoolean(),
            GetRandomBoolean()
        );

    public CreateAccountInput GetInvalidInputShortName()
    {
        var invalidInputShortName = GetInput();
        invalidInputShortName.Name = invalidInputShortName.Name[..2];

        return invalidInputShortName;
         
    }
    public CreateAccountInput GetInvalidInputLongName()
    {
        var invalidInputTooLongName = GetInput();
        invalidInputTooLongName.Name = Faker.Lorem.Letter(256);
        return invalidInputTooLongName;
    }
    public CreateAccountInput GetInvalidInputWithNegativeOpeningBalance()
    {

        var invalidInputNegativeOpeningBalance = GetInput();
        invalidInputNegativeOpeningBalance.OpeningBalance *= -1;
        return invalidInputNegativeOpeningBalance;
    }

    public static Mock<IAccountRepository> GetRepositoryMock()
        => new();
    
    public static Mock<IUnitOfWork> GetUnitOfWorkMock() 
        => new();


}
