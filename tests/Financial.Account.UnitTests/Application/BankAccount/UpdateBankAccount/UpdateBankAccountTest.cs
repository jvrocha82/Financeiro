using Financial.Application.Exceptions;
using Financial.Application.UseCases.BankAccount.Common;
using Financial.Application.UseCases.BankAccount.UpdateBankAccount;
using Financial.Domain.Exceptions;
using FluentAssertions;
using Moq;
using Xunit;
using DomainEntity = Financial.Domain.Entity;

using UseCase = Financial.Application.UseCases.BankAccount.UpdateBankAccount;
namespace Financial.UnitTests.Application.BankAccount.UpdateBankAccount;
[Collection(nameof(UpdateBankAccountTestFixture))]
public class UpdateBankAccountTest
{
    private readonly UpdateBankAccountTestFixture _fixture;
    public UpdateBankAccountTest(UpdateBankAccountTestFixture fixture)
        => _fixture = fixture;


    [Theory(DisplayName = nameof(UpdateBankAccount))]
    [Trait("Application", "UpdateBankAccount - Use Cases")]
    [MemberData(
        nameof(UpdateBankAccountTestDataGenerator.GetBankAccountToUpdate),
        parameters:10,
        MemberType = typeof(UpdateBankAccountTestDataGenerator)
    )]
    public async Task UpdateBankAccount(
        DomainEntity.BankAccount exampleBankAccount,
        UpdateBankAccountInput input)
    {
        var repositoryMock = _fixture.GetRepositoryMock();
        var unitOfWorkMock = _fixture.GetUnitOfWorkMock();

        repositoryMock.Setup(x => x.Get(
            exampleBankAccount.Id,
            It.IsAny<CancellationToken>())
        ).ReturnsAsync(exampleBankAccount);

        var useCase = new UseCase.UpdateBankAccount(
                repositoryMock.Object,
                unitOfWorkMock.Object
        );

        BankAccountModelOutput output  = await useCase.Handle(input, CancellationToken.None);

        output.Should().NotBeNull();
        output.Name.Should().Be(input.Name);
        output.OpeningBalance.Should().Be(input.OpeningBalance);
        output.IsActive.Should().Be((bool)input.IsActive!);

        repositoryMock.Verify(x => x.Get(
              exampleBankAccount.Id,
            It.IsAny<CancellationToken>())
        , Times.Once);
        repositoryMock.Verify(x => x.Update(
            exampleBankAccount
            ,
            It.IsAny<CancellationToken>())
        , Times.Once);

        unitOfWorkMock.Verify(x => x.Commit(
            It.IsAny<CancellationToken>())
        , Times.Once());

    }

    [Fact(DisplayName = nameof(ThrowWhenBankAccountNotFound))]
    [Trait("Application", "UpdateBankAccount - Use Cases")]
   
    public async Task ThrowWhenBankAccountNotFound()
    {
        var repositoryMock = _fixture.GetRepositoryMock();
        var unitOfWorkMock = _fixture.GetUnitOfWorkMock();
        var input = _fixture.GetValidBankAccountInput();

        repositoryMock.Setup(x => x.Get(
            input.Id,
            It.IsAny<CancellationToken>())
        ).ThrowsAsync(new NotFoundException($"bankAccount '{input.Id}' not found"));

        var useCase = new UseCase.UpdateBankAccount(
                repositoryMock.Object,
                unitOfWorkMock.Object
        );

       var task = async () 
            => await useCase.Handle(input, CancellationToken.None);

        await task.Should().ThrowAsync<NotFoundException>();


        repositoryMock.Verify(x => x.Get(
              input.Id,
            It.IsAny<CancellationToken>())
        , Times.Once);
   

    }

    [Theory(DisplayName = nameof(UpdateBankAccount))]
    [Trait("Application", "UpdateBankAccount - Use Cases")]
    [MemberData(
       nameof(UpdateBankAccountTestDataGenerator.GetBankAccountToUpdate),
       parameters: 10,
       MemberType = typeof(UpdateBankAccountTestDataGenerator)
   )]
    public async Task UpdateBankAccountWhithoutProvidingIsActive(
        DomainEntity.BankAccount exampleBankAccount,
        UpdateBankAccountInput exampleInput)
    {

        var input = new UpdateBankAccountInput(
            exampleInput.Id,
            exampleInput.Name,
            exampleInput.OpeningBalance
        );
        var repositoryMock = _fixture.GetRepositoryMock();
        var unitOfWorkMock = _fixture.GetUnitOfWorkMock();

        repositoryMock.Setup(x => x.Get(
            exampleBankAccount.Id,
            It.IsAny<CancellationToken>())
        ).ReturnsAsync(exampleBankAccount);

        var useCase = new UseCase.UpdateBankAccount(
                repositoryMock.Object,
                unitOfWorkMock.Object
        );

        BankAccountModelOutput output = await useCase.Handle(input, CancellationToken.None);

        output.Should().NotBeNull();
        output.Name.Should().Be(input.Name);
        output.OpeningBalance.Should().Be(input.OpeningBalance);
        output.IsActive.Should().Be((bool)exampleBankAccount.IsActive!);

        repositoryMock.Verify(x => x.Get(
              exampleBankAccount.Id,
            It.IsAny<CancellationToken>())
        , Times.Once);
        repositoryMock.Verify(x => x.Update(
            exampleBankAccount
            ,
            It.IsAny<CancellationToken>())
        , Times.Once);

        unitOfWorkMock.Verify(x => x.Commit(
            It.IsAny<CancellationToken>())
        , Times.Once());

    }

    [Theory(DisplayName = nameof(UpdateBankAccountOnlyName))]
    [Trait("Application", "UpdateBankAccount - Use Cases")]
    [MemberData(
      nameof(UpdateBankAccountTestDataGenerator.GetBankAccountToUpdate),
      parameters: 10,
      MemberType = typeof(UpdateBankAccountTestDataGenerator)
  )]
    public async Task UpdateBankAccountOnlyName(
       DomainEntity.BankAccount exampleBankAccount,
       UpdateBankAccountInput exampleInput)
    {

        var input = new UpdateBankAccountInput(
            exampleInput.Id,
            exampleInput.Name
        );
        var repositoryMock = _fixture.GetRepositoryMock();
        var unitOfWorkMock = _fixture.GetUnitOfWorkMock();

        repositoryMock.Setup(x => x.Get(
            exampleBankAccount.Id,
            It.IsAny<CancellationToken>())
        ).ReturnsAsync(exampleBankAccount);

        var useCase = new UseCase.UpdateBankAccount(
                repositoryMock.Object,
                unitOfWorkMock.Object
        );

        BankAccountModelOutput output = await useCase.Handle(input, CancellationToken.None);

        output.Should().NotBeNull();
        output.Name.Should().Be(input.Name);
        output.OpeningBalance.Should().Be(exampleBankAccount.OpeningBalance);
        output.IsActive.Should().Be((bool)exampleBankAccount.IsActive!);

        repositoryMock.Verify(x => x.Get(
              exampleBankAccount.Id,
            It.IsAny<CancellationToken>())
        , Times.Once);
        repositoryMock.Verify(x => x.Update(
            exampleBankAccount
            ,
            It.IsAny<CancellationToken>())
        , Times.Once);

        unitOfWorkMock.Verify(x => x.Commit(
            It.IsAny<CancellationToken>())
        , Times.Once());

    }

    [Theory(DisplayName = "")]
    [Trait("Application", "UpdateBankAccount - Use")]
    [MemberData(
        nameof(UpdateBankAccountTestDataGenerator.GetInvalidInputs),
        parameters: 12,
        MemberType = typeof(UpdateBankAccountTestDataGenerator)
        )]
    public async Task ThrowWhenCantUpdateBankAccount(
        UpdateBankAccountInput input,
        string expectedExceptionMessage)
    {
        var exampleBankAccount = _fixture.GetValidBankAccount();
        input.Id = exampleBankAccount.Id;
        var repositoryMock = _fixture.GetRepositoryMock();
        var unitOfWorkMock = _fixture.GetUnitOfWorkMock();

        repositoryMock.Setup(x => x.Get(
            exampleBankAccount.Id,
            It.IsAny<CancellationToken>())
        ).ReturnsAsync(exampleBankAccount);

        var useCase = new UseCase.UpdateBankAccount(
            repositoryMock.Object,
            unitOfWorkMock.Object
        );
        var task = async () 
            => await useCase.Handle(input, CancellationToken.None);

        await task.Should().ThrowAsync<EntityValidationException>()
            .WithMessage(expectedExceptionMessage);
            
        repositoryMock.Verify(x => x.Get(
            exampleBankAccount.Id,
            It.IsAny<CancellationToken>()),
            Times.Once());

    }
}
