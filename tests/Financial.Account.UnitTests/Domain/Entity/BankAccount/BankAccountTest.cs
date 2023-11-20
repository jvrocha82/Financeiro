using Financial.Domain.Exceptions;
using FluentAssertions;
using Microsoft.VisualStudio.TestPlatform.ObjectModel;
using Xunit;

using DomainEntity = Financial.Domain.Entity;
namespace Financial.UnitTests.Domain.Entity.BankAccount;
[Collection(nameof(BankAccountTestFixture))]
public class BankAccountTest
{
    private readonly BankAccountTestFixture _bankAccountTestFixture;

    public BankAccountTest(BankAccountTestFixture bankAccountTestFixture)
        => _bankAccountTestFixture = bankAccountTestFixture;
    
    #region[InstantiateValidation]
    [Fact(DisplayName = nameof(Instantiate))]
    [Trait("Domain", "Bank Account - Aggregates")]
    public void Instantiate()
    {
        var validUser = _bankAccountTestFixture.GetValidUser(); 
        //Arrange
        var validBankAccount = _bankAccountTestFixture.GetValidBankAccount();
        
        //Act
        var datetimeBefore = DateTime.Now.AddSeconds(-1);
        var bankAccount = new DomainEntity.BankAccount(validUser.Id, validBankAccount.Name, validBankAccount.OpeningBalance);
        var datetimeAfter = DateTime.Now.AddSeconds(1);

        Guid GuidExampleValue = new Guid();
        var typeGuidName = GuidExampleValue.GetType().Name;
        //Assert
 
        bankAccount.Should().NotBeNull();
       
        bankAccount.Id.GetType().Name.Should().Be(typeGuidName);
        bankAccount.UserId.GetType().Name.Should().Be(typeGuidName);
        bankAccount.UserId.Should().Be(validUser.Id);
        bankAccount.Id.Should().NotBe(default(Guid));
        bankAccount.Name.Should().Be(validBankAccount.Name);
        bankAccount.OpeningBalance.Should().Be(validBankAccount.OpeningBalance);
        bankAccount.Id.Should().NotBeEmpty();
        bankAccount.CreatedAt.Should().NotBeSameDateAs(default(DateTime));
        bankAccount.OpeningBalanceIsNegative.Should().BeFalse();
        bankAccount.CreatedAt.Should().BeAfter(datetimeBefore);
        bankAccount.CreatedAt.Should().BeBefore(datetimeAfter);
        bankAccount.IsActive.Should().BeTrue();

    }
    #endregion[InstantiateValidation]

    #region[OpeningBalanceTypeValidation]
    [Fact(DisplayName = nameof(OpeningBalanceTypeOk))]
    [Trait("Domain", "Bank Account - Aggregates")]
    public void OpeningBalanceTypeOk()
    {
        var validUser = _bankAccountTestFixture.GetValidUser();
        var ValidBankAccount = _bankAccountTestFixture.GetValidBankAccount();

        var BankAccount = new DomainEntity.BankAccount(validUser.Id, ValidBankAccount.Name, ValidBankAccount.OpeningBalance);
        Decimal DecimalVar = 0;
        var typeDecimalName = DecimalVar.GetType().Name;

        BankAccount.OpeningBalance.GetType().Name.Should().Be(typeDecimalName);
    }
    #endregion

    #region[OpeningBalanceIsNegativeValidation]

    [Theory(DisplayName = nameof(InstantiateWithOpeningBalanceIsNegative))]
    [Trait("Domain", "Bank Account - Aggregates")]
    [InlineData(true)]
    [InlineData(false)]
    public void InstantiateWithOpeningBalanceIsNegative(bool isNegative)
    {
        var validBankAccount = _bankAccountTestFixture.GetValidBankAccount();
        var validUser = _bankAccountTestFixture.GetValidUser();

        var datetimeBefore = DateTime.Now.AddSeconds(-1);
        var bankAccount = new DomainEntity.BankAccount(validUser.Id, validBankAccount.Name, validBankAccount.OpeningBalance, isNegative);
        var datetimeAfter = DateTime.Now.AddSeconds(1);

        bankAccount.Should().NotBeNull();
        bankAccount.Name.Should().Be(validBankAccount.Name);
        bankAccount.OpeningBalance.Should().Be(validBankAccount.OpeningBalance);
        bankAccount.Id.Should().NotBeEmpty();
        bankAccount.CreatedAt.Should().NotBeSameDateAs(default(DateTime));
        bankAccount.CreatedAt.Should().BeAfter(datetimeBefore);
        bankAccount.CreatedAt.Should().BeBefore(datetimeAfter);
        bankAccount.IsActive.Should().BeTrue();
        bankAccount.OpeningBalanceIsNegative.Should().Be(isNegative);
    }


    #endregion[OpeningBalanceIsNegativeValidation]f

    #region[IsActiveValidation]
    [Theory(DisplayName = nameof(Instantiate))]
    [Trait("Domain", "Bank Account - Aggregates")]
    [InlineData(true)]
    [InlineData(false)]
    public void InstantiateWithIsActive(bool isActive)
    {
        //Arrange
        var validBankAccount = _bankAccountTestFixture.GetValidBankAccount();
        var validUser = _bankAccountTestFixture.GetValidUser();


        //Act
        var datetimeBefore = DateTime.Now.AddSeconds(-1);
        var bankAccount = new DomainEntity.BankAccount(validUser.Id, validBankAccount.Name, validBankAccount.OpeningBalance, validBankAccount.OpeningBalanceIsNegative, isActive);
        var datetimeAfter = DateTime.Now.AddSeconds(1);
        //Assert

        bankAccount.Should().NotBeNull();
        bankAccount.Name.Should().Be(validBankAccount.Name);
        bankAccount.OpeningBalance.Should().Be(validBankAccount.OpeningBalance);
        bankAccount.Id.Should().NotBeEmpty();
        bankAccount.CreatedAt.Should().NotBeSameDateAs(default(DateTime));
        bankAccount.OpeningBalanceIsNegative.Should().BeFalse();
        bankAccount.CreatedAt.Should().BeAfter(datetimeBefore);
        bankAccount.CreatedAt.Should().BeBefore(datetimeAfter);
        bankAccount.IsActive.Should().Be(isActive);
    }
    #endregion[IsActiveValidation]

    #region[NameValidation]
    [Theory(DisplayName = nameof(InstantiateErrorWhenNameIsEmpty))]
    [Trait("Domain", "BankAccount - Aggregates")]
    [InlineData("")]
    [InlineData(null)]
    [InlineData("    ")]
    
    public void InstantiateErrorWhenNameIsEmpty(string? name)
    {
        var validBankAccount = _bankAccountTestFixture.GetValidBankAccount();
        var validUser = _bankAccountTestFixture.GetValidUser();

        Action action =
            () => new DomainEntity.BankAccount(validUser.Id, name!, validBankAccount.OpeningBalance);
        action.Should()
            .Throw<EntityValidationException>()
            .WithMessage("Name should not be empty or null");
    }


    [Theory(DisplayName = nameof(InstantiateErrorWhenNameIsLessThan3Characters))]
    [Trait("Domain", "Bank Account - Aggregates")]
    [MemberData(nameof(GetNamesWhenNameIsLessThan3Characters), parameters:10)]
   

    public void InstantiateErrorWhenNameIsLessThan3Characters(string name)
    {
        var validBankAccount = _bankAccountTestFixture.GetValidBankAccount();
        var validUser = _bankAccountTestFixture.GetValidUser();

        Action action =
            () => new DomainEntity.BankAccount(validUser.Id, name!, validBankAccount.OpeningBalance);
            action.Should()
                .Throw<EntityValidationException>()
                .WithMessage("Name should be at leats 3 characters long");
    }

    public static IEnumerable<object[]> GetNamesWhenNameIsLessThan3Characters(int numberOfTests = 6)
    {
        var fixture = new BankAccountTestFixture();
        for(int i = 0; i < numberOfTests; i++)
        {
            var isOdd = i % 2 == 1;
            yield return new object[] { 
                fixture.GetValidName()[..(isOdd ? 1 : 2)]
            };
        }
    }

    [Fact(DisplayName = nameof(InstantiateErrorWhenNameIsGreaterThan255Characters))]
    [Trait("Domain", "Bank Account - Aggregates")]
    public void InstantiateErrorWhenNameIsGreaterThan255Characters()
    {
        var validBankAccount = _bankAccountTestFixture.GetValidBankAccount();
        var validUser = _bankAccountTestFixture.GetValidUser();

        var invalidName = String.Join(null, Enumerable.Range(0, 256).Select(_ => "a").ToArray());
        Action action =
            () => new DomainEntity.BankAccount(validUser.Id, invalidName, validBankAccount.OpeningBalance);

        action.Should()
            .Throw<EntityValidationException>()
            .WithMessage("Name should be less or equal 255 characters long");
    }

    #endregion[NameValidation]

    #region[OpeningBalanceIsNegativeValidation]

    [Theory(DisplayName = nameof(InstantiateErrorWhenOpeningBalanceIsNegative))]
    [Trait("Domain", "Bank Account - Aggregates")]
    [InlineData(-10)]
    [InlineData(-1)]
    [InlineData(-100)]
    [InlineData(-300)]

    public void InstantiateErrorWhenOpeningBalanceIsNegative(int openingBalanceNegative)
    {
        var validBankAccount = _bankAccountTestFixture.GetValidBankAccount();
        var validUser = _bankAccountTestFixture.GetValidUser();

        Action action =
            () => new DomainEntity.BankAccount(validUser.Id, validBankAccount.Name, openingBalanceNegative);

        action.Should()
            .Throw<EntityValidationException>()
            .WithMessage("OpeningBalance cannot be negative value");
    }
    #endregion[OpeningBalanceIsNegativeValidation]

    #region[Activate/DesactivateValidation]
    [Fact(DisplayName = nameof(Activate))]
    [Trait("Domain", "Bank Account - Aggregates")]
    public void Activate()
    {
        var validBankAccount = _bankAccountTestFixture.GetValidBankAccount();
        var validUser = _bankAccountTestFixture.GetValidUser();

        var bankAccount = new DomainEntity.BankAccount(validUser.Id, validBankAccount.Name, validBankAccount.OpeningBalance, validBankAccount.OpeningBalanceIsNegative, false);
        bankAccount.Activate();

        bankAccount.IsActive.Should().BeTrue();
    }

    [Fact(DisplayName = nameof(Deactivate))]
    [Trait("Domain", "Bank Account - Aggregates")]
    public void Deactivate()
    {
        var validBankAccount = _bankAccountTestFixture.GetValidBankAccount();
        var validUser = _bankAccountTestFixture.GetValidUser();

        var bankAccount = new DomainEntity.BankAccount(validUser.Id, validBankAccount.Name, validBankAccount.OpeningBalance, validBankAccount.OpeningBalanceIsNegative, true);
        bankAccount.Deactivate();
        bankAccount.IsActive.Should().BeFalse();
    }

    #endregion[Activate/DesactivateValidation]

    #region[UpdateValidation]
    [Fact(DisplayName = nameof(Update))]
    [Trait("Domain", "Bank Account - Aggregates")]

    public void Update()
    {
        var bankAccount = _bankAccountTestFixture.GetValidBankAccount();
        var bankAccountWithNewValues = _bankAccountTestFixture.GetValidBankAccount();
  

        bankAccount.Update(bankAccountWithNewValues.Name, bankAccountWithNewValues.OpeningBalance);

        bankAccount.Name.Should().Be(bankAccountWithNewValues.Name);
        bankAccount.OpeningBalance.Should().Be(bankAccountWithNewValues.OpeningBalance);

    }
    [Fact(DisplayName = nameof(UpdateOnlyName))]
    [Trait("Domain", "Bank Account - Aggregates")]
    public void UpdateOnlyName()
    {
        var bankAccount = _bankAccountTestFixture.GetValidBankAccount();
        var bankAccountWithNewValues = _bankAccountTestFixture.GetValidBankAccount();

        var currentOpeningBalance = bankAccount.OpeningBalance;

        bankAccount.Update(bankAccountWithNewValues.Name);


        bankAccount.Name.Should().Be(bankAccountWithNewValues.Name);
        bankAccount.OpeningBalance.Should().Be(currentOpeningBalance);
    }

    [Theory(DisplayName = nameof(UpdateErrorWhenNameIsEmpty))]
    [Trait("Domain", "Bank Account - Aggregates")]
    [InlineData("")]
    [InlineData(null)]
    [InlineData("    ")]
    public void UpdateErrorWhenNameIsEmpty(string? name)
    {
        var validOpeningBalance = _bankAccountTestFixture.GetValidOpeningBalance();

        var bankAccount = _bankAccountTestFixture.GetValidBankAccount();
        Action action =
            () => bankAccount.Update(name!, validOpeningBalance);

        action.Should()
            .Throw<EntityValidationException>()
            .WithMessage("Name should not be empty or null");
    }

    [Theory(DisplayName = nameof(UpdateErrorWhenNameIsLessThan3Characters))]
    [Trait("Domain", "Bank Account - Aggregates")]
    [InlineData("1")]
    [InlineData("12")]
    [InlineData("a")]
    [InlineData("ca")]

    public void UpdateErrorWhenNameIsLessThan3Characters(string name)
    {
        var bankAccount = _bankAccountTestFixture.GetValidBankAccount();
        var validOpeningBalance = _bankAccountTestFixture.GetValidOpeningBalance();

        Action action =
            () => bankAccount.Update(name!, validOpeningBalance);


        action.Should()
            .Throw<EntityValidationException>()
            .WithMessage("Name should be at leats 3 characters long");
    }

    [Fact(DisplayName = nameof(UpdateErrorWhenNameIsGreaterThan255Characters))]
    [Trait("Domain", "Bank Account - Aggregates")]
    public void UpdateErrorWhenNameIsGreaterThan255Characters()
    {
        var bankAccount = _bankAccountTestFixture.GetValidBankAccount();
     
        var invalidName = _bankAccountTestFixture.Faker.Lorem.Letter(256);
        var validOpeningBalance = _bankAccountTestFixture.GetValidOpeningBalance();
        Action action =
            () => bankAccount.Update(invalidName, validOpeningBalance);

        action.Should()
              .Throw<EntityValidationException>()
              .WithMessage("Name should be less or equal 255 characters long");
    }


    [Theory(DisplayName = nameof(UpdateErrorWhenOpeningBalanceIsNegative))]
    [Trait("Domain", "Bank Account - Aggregates")]
    [InlineData(-10)]
    [InlineData(-1)]
    [InlineData(-100)]
    [InlineData(-300)]

    public void UpdateErrorWhenOpeningBalanceIsNegative(int openingBalanceNegative)
    {
        var bankAccount = _bankAccountTestFixture.GetValidBankAccount();
        var validName = _bankAccountTestFixture.GetValidName();
        Action action =
            () => bankAccount.Update(validName, openingBalanceNegative);

        action.Should()
              .Throw<EntityValidationException>()
              .WithMessage("OpeningBalance cannot be negative value");
    }
    #endregion
}
