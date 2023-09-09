using Financial.Domain.Exceptions;
using FluentAssertions;
using Microsoft.VisualStudio.TestPlatform.ObjectModel;
using System.ComponentModel;
using System.Xml.Linq;
using Xunit;

using DomainEntity = Financial.Domain.Entity;
namespace Financial.UnitTests.Domain.Entity.Account;

public class AccountTest
{
    #region[InstantiateValidation]
    [Fact(DisplayName = nameof(Instantiate))]
    [Trait("Domain", "Account - Aggregates")]
    public void Instantiate()
    {
        //Arrange
        var validData = new
        {
            Name = "Account Name",
            OpeningBalance = 100,
        };
        //Act
        var datetimeBefore = DateTime.Now;
        var account = new DomainEntity.Account(validData.Name, validData.OpeningBalance);
        var datetimeAfter = DateTime.Now;
        //Assert

        Assert.NotNull(account);
        Assert.Equal(validData.Name, account.Name);
        Assert.Equal(validData.OpeningBalance, account.OpeningBalance);
        Assert.NotEqual(default(Guid), account.Id);
        Assert.NotEqual(default(DateTime), account.CreatedAt);
        Assert.True(account.CreatedAt > datetimeAfter);
        Assert.True(account.CreatedAt < datetimeBefore);
        Assert.True(account.IsActive);
        Assert.False(account.OpeningBalanceIsNegative);

    }
    #endregion[InstantiateValidation]

    #region[OpeningBalanceIsNegativeValidation]

    [Theory(DisplayName = nameof(InstantiateWithOpeningBalanceIsNegative))]
    [Trait("Domain", "Account - Aggregates")]
    [InlineData(true)]
    [InlineData(false)]
    public void InstantiateWithOpeningBalanceIsNegative(bool isNegative)
    {
        var validData = new
        {
            Name = "Account Name",
            OpeningBalance = 100
        };

        var datetimeBefore = DateTime.Now;
        var account = new DomainEntity.Account(validData.Name, validData.OpeningBalance, isNegative);
        var datetimeAfter = DateTime.Now;


        Assert.NotNull(account);
        Assert.Equal(validData.Name, account.Name);
        Assert.Equal(validData.OpeningBalance, account.OpeningBalance);
        Assert.NotEqual(default(Guid), account.Id);
        Assert.NotEqual(default(DateTime), account.CreatedAt);
        Assert.True(account.CreatedAt > datetimeBefore);
        Assert.True(account.CreatedAt < datetimeAfter);
        Assert.True(account.IsActive);
        Assert.Equal(isNegative, account.OpeningBalanceIsNegative);
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
        var validData = new
        {
            Name = "Account Name",
            OpeningBalance = 100,
            OpeningBalanceIsNegative = false
        };
        //Act
        var datetimeBefore = DateTime.Now;
        var account = new DomainEntity.Account(validData.Name, validData.OpeningBalance, validData.OpeningBalanceIsNegative, isActive);
        var datetimeAfter = DateTime.Now;
        //Assert

        account.Should().NotBeNull();
        account.Name.Should().Be(validData.Name);
        account.OpeningBalance.Should().Be(validData.OpeningBalance);
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
        Action action =
            () => new DomainEntity.Account(name!, 0);
        var exception = Assert.Throws<EntityValidationException>(action);
        Assert.Equal("Name should not be empty or null", exception.Message);
    }
    [Theory(DisplayName = nameof(InstantiateErrorWhenNameIsLessThan3Characters))]
    [Trait("Domain", "Account - Aggregates")]
    [InlineData("1")]
    [InlineData("12")]
    [InlineData("a")]
    [InlineData("ca")]

    public void InstantiateErrorWhenNameIsLessThan3Characters(string name)
    {
        Action action =
            () => new DomainEntity.Account(name!, 0);
        var exception = Assert.Throws<EntityValidationException>(action);
        Assert.Equal("Name should be at leats 3 characters long", exception.Message);
    }

    [Fact(DisplayName = nameof(InstantiateErrorWhenNameIsGreaterThan255Characters))]
    [Trait("Domain", "Account - Aggregates")]
    public void InstantiateErrorWhenNameIsGreaterThan255Characters()
    {
        var invalidName = String.Join(null, Enumerable.Range(0, 256).Select(_ => "a").ToArray());
        Action action =
            () => new DomainEntity.Account(invalidName, 0);
        var exception = Assert.Throws<EntityValidationException>(action);
        Assert.Equal("Name should be less or equal 255 characters long", exception.Message);
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
        Action action =
            () => new DomainEntity.Account("AccountName", openingBalanceNegative);
        var exception = Assert.Throws<EntityValidationException>(action);
        Assert.Equal("OpeningBalance cannot be negative value", exception.Message);
    }
    #endregion[OpeningBalanceIsNegativeValidation]

    #region[Activate/DesactivateValidation]
    [Fact(DisplayName = nameof(Activate))]
    [Trait("Domain", "Account - Aggregates")]
    [InlineData(true)]
    [InlineData(false)]
    public void Activate()
    {
        var validData = new
        {
            Name = "Account Name",
            OpeningBalance = 100
        };

        var account = new DomainEntity.Account(validData.Name, validData.OpeningBalance, false, false);
        account.Activate();

        Assert.True(account.IsActive);
    }
    [Fact(DisplayName = nameof(Deactivate))]
    [Trait("Domain", "Account - Aggregates")]
    [InlineData(true)]
    [InlineData(false)]
    public void Deactivate()
    {
        var validData = new
        {
            Name = "Account Name",
            OpeningBalance = 100
        };

        var account = new DomainEntity.Account(validData.Name, validData.OpeningBalance, false, true);
        account.Deactivate();

        Assert.False(account.IsActive);
    }

    #endregion[Activate/DesactivateValidation]

    #region[UpdateValidation]
    [Fact(DisplayName = nameof(Update))]
    [Trait("Domain", "Account - Aggregates")]

    public void Update()
    {
        var account = new DomainEntity.Account("Category Name", 0);
        var newValues = new
        {
            Name = "New Name",
            OpeningBalance = 0
        };
        account.Update(newValues.Name, newValues.OpeningBalance);

        Assert.Equal(newValues.Name, account.Name);
        Assert.Equal(newValues.OpeningBalance, account.OpeningBalance);
    }
    [Fact(DisplayName = nameof(UpdateOnlyName))]
    [Trait("Domain", "Account - Aggregates")]
    public void UpdateOnlyName()
    {
        var account = new DomainEntity.Account("Account Name", 0);
        var newValues = new { Name = "New Name" };
        var currentOpeningBalance = account.OpeningBalance;

        account.Update(newValues.Name);

        Assert.Equal(newValues.Name, account.Name);
        Assert.Equal(currentOpeningBalance, account.OpeningBalance);

    }
    [Theory(DisplayName = nameof(UpdateErrorWhenNameIsEmpty))]
    [Trait("Domain", "Account - Aggregates")]
    [InlineData("")]
    [InlineData(null)]
    [InlineData("    ")]
    public void UpdateErrorWhenNameIsEmpty(string? name)
    {
        var account = new DomainEntity.Account("Account Name", 0);
        Action action =
            () => account.Update(name!, 0);
        var exception = Assert.Throws<EntityValidationException>(action);
        Assert.Equal("Name should not be empty or null", exception.Message);
    }

    [Theory(DisplayName = nameof(UpdateErrorWhenNameIsLessThan3Characters))]
    [Trait("Domain", "Account - Aggregates")]
    [InlineData("1")]
    [InlineData("12")]
    [InlineData("a")]
    [InlineData("ca")]

    public void UpdateErrorWhenNameIsLessThan3Characters(string name)
    {
        var account = new DomainEntity.Account("Account Name", 0);
        Action action =
            () => account.Update(name!, 0);
        var exception = Assert.Throws<EntityValidationException>(action);
        Assert.Equal("Name should be at leats 3 characters long", exception.Message);
    }

    [Fact(DisplayName = nameof(UpdateErrorWhenNameIsGreaterThan255Characters))]
    [Trait("Domain", "Account - Aggregates")]
    public void UpdateErrorWhenNameIsGreaterThan255Characters()
    {
        var account = new DomainEntity.Account("Account Name", 0);

        var invalidName = String.Join(null, Enumerable.Range(0, 256).Select(_ => "a").ToArray());
        Action action =
            () => account.Update(invalidName, 0);
        var exception = Assert.Throws<EntityValidationException>(action);
        Assert.Equal("Name should be less or equal 255 characters long", exception.Message);
    }
    [Theory(DisplayName = nameof(UpdateErrorWhenOpeningBalanceIsNegative))]
    [Trait("Domain", "Account - Aggregates")]
    [InlineData(-10)]
    [InlineData(-1)]
    [InlineData(-100)]
    [InlineData(-300)]

    public void UpdateErrorWhenOpeningBalanceIsNegative(int openingBalanceNegative)
    {
        var account = new DomainEntity.Account("Account Name", 0);

        Action action =
            () => account.Update("AccountName", openingBalanceNegative);
        var exception = Assert.Throws<EntityValidationException>(action);
        Assert.Equal("OpeningBalance cannot be negative value", exception.Message);
    }
    #endregion
}
