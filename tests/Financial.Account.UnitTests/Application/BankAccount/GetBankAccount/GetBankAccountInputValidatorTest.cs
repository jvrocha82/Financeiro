using Financial.Application.UseCases.BankAccount.GetBankAccount;
using FluentAssertions;
using Xunit;

namespace Financial.UnitTests.Application.BankAccount.GetBankAccount;

[Collection(nameof(GetBankAccountTestFixture))]
public class GetBankAccountInputValidatorTest
{
    private readonly GetBankAccountTestFixture fixture;

    public GetBankAccountInputValidatorTest(GetBankAccountTestFixture fixture)
    => this.fixture = fixture;

    [Fact(DisplayName = nameof(ValidationOk))]
    [Trait("Application", "GetBankAccountInputValidation - UseCases")]


    public void ValidationOk()
    {
        var validInpuit = new GetBankAccountInput(Guid.NewGuid());

        var validator = new GetBankAccountInputValidator();
        var validationResult = validator.Validate(validInpuit);

        validationResult.Should().NotBeNull();
        validationResult.IsValid.Should().BeTrue();
        validationResult.Errors.Should().HaveCount(0);

    }
    [Fact(DisplayName = nameof(InvalidWhenEmptyGuidId))]
    [Trait("Application", "GetBankAccountInputValidation - UseCases")]


    public void InvalidWhenEmptyGuidId()
    {
        var invalidInpuit = new GetBankAccountInput(Guid.Empty);

        var validator = new GetBankAccountInputValidator();
        var validationResult = validator.Validate(invalidInpuit);

        validationResult.Should().NotBeNull();
        validationResult.IsValid.Should().BeFalse();
        validationResult.Errors.Should().HaveCount(1);
        validationResult.Errors[0].ErrorMessage
            .Should().Be("'Id' must not be empty.");

    }
}
