using Financial.Domain.Exceptions;
using FluentAssertions;
using Moq;
using Xunit;

using UseCases = Financial.Application.UseCases.BankAccount.CreateBankAccount;
using DomainEntity = Financial.Domain.Entity;
using Financial.Application.UseCases.BankAccount.CreateBankAccount;

namespace Financial.UnitTests.Application.BankAccount.CreateBankAccount;

[Collection(nameof(CreateBankAccountTestFixture))]
public class CreateBankAccountTest
{
    private readonly CreateBankAccountTestFixture _fixture;

    public CreateBankAccountTest(CreateBankAccountTestFixture fixture)
    => _fixture = fixture;

    [Fact(DisplayName = nameof(CreateBankAccount))]
    [Trait("Application", "CreateBankAccount - Use Cases")]
    public async void CreateBankAccount()
    {
        var repositoryMock = _fixture.GetRepositoryMock();
        var unitOfWorkMock = _fixture.GetUnitOfWorkMock();

        var useCase = new UseCases.CreateBankAccount(
                repositoryMock.Object,
                unitOfWorkMock.Object
            );
        var input = _fixture.GetInput();

        var output = await useCase.Handle(input, CancellationToken.None);

        repositoryMock.Verify(
            repository => repository.Insert(
                It.IsAny<DomainEntity.BankAccount>(),
                It.IsAny<CancellationToken>()
            ),
            Times.Once
        );
        unitOfWorkMock.Verify(
            uow => uow.Commit(It.IsAny<CancellationToken>()),
            Times.Once
        );


        output.Should().NotBeNull();
        output.Name.Should().Be(input.Name);
        output.OpeningBalance.Should().Be(input.OpeningBalance);
        output.OpeningBalanceIsNegative.Should().Be(input.OpeningBalanceIsNegative);
        output.IsActive.Should().Be(input.IsActive);
        (output.Id != Guid.Empty).Should().BeTrue();
        (output.CreatedAt != default).Should().BeTrue();

    }
    [Theory(DisplayName = nameof(ThrowWhenCantInstantiateBankAccount))]
    [Trait("Application", "CreateBankAccount - Use Cases")]
    [MemberData(
        nameof(CreateBankAccountTestDataGenerator.GetInvalidInputs),
        parameters: 12,
        MemberType = typeof(CreateBankAccountTestDataGenerator)
        )]


    public async void ThrowWhenCantInstantiateBankAccount(
        CreateBankAccountInput input,
        string exceptionMessage)
    {
        var useCase = new UseCases.CreateBankAccount(
            _fixture.GetRepositoryMock().Object,
            _fixture.GetUnitOfWorkMock().Object
        );

        Func<Task> task = async () => await useCase.Handle(
            input,
            CancellationToken.None
        );

        await task.Should()
             .ThrowAsync<EntityValidationException>()
             .WithMessage(exceptionMessage);
    }
}
