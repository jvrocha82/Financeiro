using Xunit;
using FluentAssertions;
using Financial.Application.UseCases.BankAccount.UpdateBankAccount;
namespace Financial.UnitTests.Application.BankAccount.UpdateBankAccount;
[Collection(nameof(UpdateBankAccountTestFixture))]
public class UpdateBankAccountInputValidatorTest
{
    private readonly UpdateBankAccountTestFixture _fixture;

    public UpdateBankAccountInputValidatorTest(UpdateBankAccountTestFixture fixture)
    => _fixture = fixture;

    [Fact(DisplayName = nameof(DontValidateWhenEmptyGuid))]
    [Trait("Application", "UpdateBankAccountInputValidator - Use Cases")]
    public void DontValidateWhenEmptyGuid()
    {
        var input = _fixture.GetValidBankAccountInput(Guid.Empty);
        var validator = new UpdateBankAccountInputValidator();

        var validateResult = validator.Validate(input);

        validateResult.Should().NotBeNull();
        validateResult.IsValid.Should().BeFalse();
        validateResult.Errors.Should().HaveCount(1);
        validateResult.Errors[0].ErrorMessage.Should().Be("'Id' must not be empty.");

    }
    [Fact(DisplayName = nameof(DontValidateWhenEmptyGuid))]
    [Trait("Application", "UpdateBankAccountInputValidator - Use Cases")]
    public void ValidateWhenValid()
    {
        var input = _fixture.GetValidBankAccountInput();
        var validator = new UpdateBankAccountInputValidator();

        var validateResult = validator.Validate(input);

        validateResult.Should().NotBeNull();
        validateResult.IsValid.Should().BeTrue();
        validateResult.Errors.Should().HaveCount(0);

    }

}
