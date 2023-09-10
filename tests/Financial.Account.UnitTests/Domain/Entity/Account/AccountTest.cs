using Financial.Domain.Exceptions;
using FluentAssertions;
using Microsoft.VisualStudio.TestPlatform.ObjectModel;
using System.ComponentModel;
using System.Xml.Linq;
using Xunit;

using DomainEntity = Financial.Domain.Entity;
namespace Financial.UnitTests.Domain.Entity.Account;
[Collection(nameof(AccountTestFixture))]
public class AccountTest
{
    private readonly AccountTestFixture _accountTestFixture;

    public AccountTest(AccountTestFixture accountTestFixture)
        => _accountTestFixture = accountTestFixture;
    
    #region[InstantiateValidation]
    [Fact(DisplayName = nameof(Instantiate))]
    [Trait("Domain", "Account - Aggregates")]
    public void Instantiate()
    {
        //Arrange
        var validAccount = _accountTestFixture.GetValidAccount();
      
        //Act
        var datetimeBefore = DateTime.Now;
        var account = new DomainEntity.Account(validAccount.Name, validAccount.OpeningBalance);
        var datetimeAfter = DateTime.Now;
        //Assert
        account.Should().NotBeNull();
        account.Name.Should().Be(validAccount.Name);
        account.OpeningBalance.Should().Be(validAccount.OpeningBalance);
        account.Id.Should().NotBeEmpty();
        account.CreatedAt.Should().NotBeSameDateAs(default(DateTime));
        account.OpeningBalanceIsNegative.Should().BeFalse();
        account.CreatedAt.Should().BeAfter(datetimeBefore);
        account.CreatedAt.Should().BeBefore(datetimeAfter);
        account.IsActive.Should().BeTrue();

    }
    #endregion[InstantiateValidation]

    #region[OpeningBalanceIsNegativeValidation]

    [Theory(DisplayName = nameof(InstantiateWithOpeningBalanceIsNegative))]
    [Trait("Domain", "Account - Aggregates")]
    [InlineData(true)]
    [InlineData(false)]
    public void InstantiateWithOpeningBalanceIsNegative(bool isNegative)
    {
        var validAccount = _accountTestFixture.GetValidAccount();

        var datetimeBefore = DateTime.Now;
        var account = new DomainEntity.Account(validAccount.Name, validAccount.OpeningBalance, isNegative);
        var datetimeAfter = DateTime.Now;

        account.Should().NotBeNull();
        account.Name.Should().Be(validAccount.Name);
        account.OpeningBalance.Should().Be(validAccount.OpeningBalance);
        account.Id.Should().NotBeEmpty();
        account.CreatedAt.Should().NotBeSameDateAs(default(DateTime));
        account.CreatedAt.Should().BeAfter(datetimeBefore);
        account.CreatedAt.Should().BeBefore(datetimeAfter);
        account.IsActive.Should().BeTrue();
        account.OpeningBalanceIsNegative.Should().Be(isNegative);
    }


    #endregion[OpeningBalanceIsNegativeValidation]

    #region[IsActiveValidation]
    [Theory(DisplayName = nameof(Instantiate))]
    [Trait("Domain", "Account - Aggregates")]
    [InlineData(true)]
    [InlineData(false)]
    public void InstantiateWithIsActive(bool isActive)
    {
        //Arrange
        var validAccount = _accountTestFixture.GetValidAccount();

        //Act
        var datetimeBefore = DateTime.Now;
        var account = new DomainEntity.Account(validAccount.Name, validAccount.OpeningBalance, validAccount.OpeningBalanceIsNegative, isActive);
        var datetimeAfter = DateTime.Now;
        //Assert

        account.Should().NotBeNull();
        account.Name.Should().Be(validAccount.Name);
        account.OpeningBalance.Should().Be(validAccount.OpeningBalance);
        account.Id.Should().NotBeEmpty();
        account.CreatedAt.Should().NotBeSameDateAs(default(DateTime));
        account.OpeningBalanceIsNegative.Should().BeFalse();
        account.CreatedAt.Should().BeAfter(datetimeBefore);
        account.CreatedAt.Should().BeBefore(datetimeAfter);
        account.IsActive.Should().Be(isActive);
    }
    #endregion[IsActiveValidation]

    #region[NameValidation]
    [Theory(DisplayName = nameof(InstantiateErrorWhenNameIsEmpty))]
    [Trait("Domain", "Account - Aggregates")]
    [InlineData("")]
    [InlineData(null)]
    [InlineData("    ")]
    
    public void InstantiateErrorWhenNameIsEmpty(string? name)
    {
        var validAccount = _accountTestFixture.GetValidAccount();

        Action action =
            () => new DomainEntity.Account(name!, validAccount.OpeningBalance);
        action.Should()
            .Throw<EntityValidationException>()
            .WithMessage("Name should not be empty or null");
    }


    [Theory(DisplayName = nameof(InstantiateErrorWhenNameIsLessThan3Characters))]
    [Trait("Domain", "Account - Aggregates")]
    [InlineData("1")]
    [InlineData("12")]
    [InlineData("a")]
    [InlineData("ca")]

    public void InstantiateErrorWhenNameIsLessThan3Characters(string name)
    {
        var validAccount = _accountTestFixture.GetValidAccount();

        Action action =
            () => new DomainEntity.Account(name!, validAccount.OpeningBalance);
            action.Should()
                .Throw<EntityValidationException>()
                .WithMessage("Name should be at leats 3 characters long");
    }

    [Fact(DisplayName = nameof(InstantiateErrorWhenNameIsGreaterThan255Characters))]
    [Trait("Domain", "Account - Aggregates")]
    public void InstantiateErrorWhenNameIsGreaterThan255Characters()
    {
        var validAccount = _accountTestFixture.GetValidAccount();

        var invalidName = String.Join(null, Enumerable.Range(0, 256).Select(_ => "a").ToArray());
        Action action =
            () => new DomainEntity.Account(invalidName, validAccount.OpeningBalance);

        action.Should()
            .Throw<EntityValidationException>()
            .WithMessage("Name should be less or equal 255 characters long");
    }

    #endregion[NameValidation]

    #region[OpeningBalanceIsNegativeValidation]

    [Theory(DisplayName = nameof(InstantiateErrorWhenOpeningBalanceIsNegative))]
    [Trait("Domain", "Account - Aggregates")]
    [InlineData(-10)]
    [InlineData(-1)]
    [InlineData(-100)]
    [InlineData(-300)]

    public void InstantiateErrorWhenOpeningBalanceIsNegative(int openingBalanceNegative)
    {
        var validAccount = _accountTestFixture.GetValidAccount();

        Action action =
            () => new DomainEntity.Account(validAccount.Name, openingBalanceNegative);

        action.Should()
            .Throw<EntityValidationException>()
            .WithMessage("OpeningBalance cannot be negative value");
    }
    #endregion[OpeningBalanceIsNegativeValidation]

    #region[Activate/DesactivateValidation]
    [Fact(DisplayName = nameof(Activate))]
    [Trait("Domain", "Account - Aggregates")]
    public void Activate()
    {
        var validAccount = _accountTestFixture.GetValidAccount();

        var account = new DomainEntity.Account(validAccount.Name, validAccount.OpeningBalance, validAccount.OpeningBalanceIsNegative, false);
        account.Activate();

        account.IsActive.Should().BeTrue();
    }

    [Fact(DisplayName = nameof(Deactivate))]
    [Trait("Domain", "Account - Aggregates")]
    public void Deactivate()
    {
        var validAccount = _accountTestFixture.GetValidAccount();

        var account = new DomainEntity.Account(validAccount.Name, validAccount.OpeningBalance, validAccount.OpeningBalanceIsNegative, true);
        account.Deactivate();
        account.IsActive.Should().BeFalse();
    }

    #endregion[Activate/DesactivateValidation]

    #region[UpdateValidation]
    [Fact(DisplayName = nameof(Update))]
    [Trait("Domain", "Account - Aggregates")]

    public void Update()
    {
        var account = _accountTestFixture.GetValidAccount();
        var newValues = new
        {
            Name = "New Name",
            OpeningBalance = 0
        };

        account.Update(newValues.Name, newValues.OpeningBalance);

        account.Name.Should().Be(newValues.Name);
        account.OpeningBalance.Should().Be(newValues.OpeningBalance);

    }
    [Fact(DisplayName = nameof(UpdateOnlyName))]
    [Trait("Domain", "Account - Aggregates")]
    public void UpdateOnlyName()
    {
        var account = _accountTestFixture.GetValidAccount();
        var newValues = new { Name = "New Name" };
        var currentOpeningBalance = account.OpeningBalance;

        account.Update(newValues.Name);


        account.Name.Should().Be(newValues.Name);
        account.OpeningBalance.Should().Be(currentOpeningBalance);
    }

    [Theory(DisplayName = nameof(UpdateErrorWhenNameIsEmpty))]
    [Trait("Domain", "Account - Aggregates")]
    [InlineData("")]
    [InlineData(null)]
    [InlineData("    ")]
    public void UpdateErrorWhenNameIsEmpty(string? name)
    {
        var account = _accountTestFixture.GetValidAccount();
        Action action =
            () => account.Update(name!, 0);

        action.Should()
            .Throw<EntityValidationException>()
            .WithMessage("Name should not be empty or null");
    }

    [Theory(DisplayName = nameof(UpdateErrorWhenNameIsLessThan3Characters))]
    [Trait("Domain", "Account - Aggregates")]
    [InlineData("1")]
    [InlineData("12")]
    [InlineData("a")]
    [InlineData("ca")]

    public void UpdateErrorWhenNameIsLessThan3Characters(string name)
    {
        var account = _accountTestFixture.GetValidAccount();
        Action action =
            () => account.Update(name!, 0);


        action.Should()
            .Throw<EntityValidationException>()
            .WithMessage("Name should be at leats 3 characters long");
    }

    [Fact(DisplayName = nameof(UpdateErrorWhenNameIsGreaterThan255Characters))]
    [Trait("Domain", "Account - Aggregates")]
    public void UpdateErrorWhenNameIsGreaterThan255Characters()
    {
        var account = _accountTestFixture.GetValidAccount();

        var invalidName = String.Join(null, Enumerable.Range(0, 256).Select(_ => "a").ToArray());
        Action action =
            () => account.Update(invalidName, 0);

        action.Should()
              .Throw<EntityValidationException>()
              .WithMessage("Name should be less or equal 255 characters long");
    }


    [Theory(DisplayName = nameof(UpdateErrorWhenOpeningBalanceIsNegative))]
    [Trait("Domain", "Account - Aggregates")]
    [InlineData(-10)]
    [InlineData(-1)]
    [InlineData(-100)]
    [InlineData(-300)]

    public void UpdateErrorWhenOpeningBalanceIsNegative(int openingBalanceNegative)
    {
        var account = _accountTestFixture.GetValidAccount();

        Action action =
            () => account.Update("AccountName", openingBalanceNegative);

        action.Should()
              .Throw<EntityValidationException>()
              .WithMessage("OpeningBalance cannot be negative value");
    }
    #endregion
}
