
using Xunit;
using UseCase = Financial.Application.UseCases.BankAccount.DeleteBankAccount;
using Moq;
using FluentValidation;
using FluentAssertions;

namespace Financial.UnitTests.Application.BankAccount.DeleteBankAccount;

[Collection(nameof(DeleteBankAccountTestFixture))]
public class DeleteBankAccountTest
{
    private readonly DeleteBankAccountTestFixture _fixture;

    public DeleteBankAccountTest(DeleteBankAccountTestFixture fixture)
    => _fixture = fixture;


    [Fact(DisplayName = nameof(DeleteBankAccount))]
    [Trait("Application", "DeleteBankAccunt - Use Cases")]

    public async Task DeleteBankAccunt()
    {
        //throw new NotImplementedException();
        var repositoryMock = _fixture.GetRepositoryMock();
        var unitOfWorkMock = _fixture.GetUnitOfWorkMock();
        var bankAccuntExample = _fixture.GetValidBankAccount();

        repositoryMock.Setup(x => x.Get(
            bankAccuntExample.Id, 
            It.IsAny<CancellationToken>())
        ).ReturnsAsync(bankAccuntExample);

        var input = new UseCase.DeleteBankAccountInput(bankAccuntExample.Id);

        var useCase = new UseCase.DeleteBankAccount(
            repositoryMock.Object,
            unitOfWorkMock.Object
        );

        await useCase.Handle(input, CancellationToken.None);

        repositoryMock.Verify(x => x.Get(
            bankAccuntExample.Id,
            It.IsAny<CancellationToken>()
            ), Times.Once);

        repositoryMock.Verify(x => x.Delete(
            bankAccuntExample,
            It.IsAny<CancellationToken>()
            ),Times.Once);

        unitOfWorkMock.Verify(x => x.Commit(
            It.IsAny<CancellationToken>()
            ), Times.Once);
    }
    
}
