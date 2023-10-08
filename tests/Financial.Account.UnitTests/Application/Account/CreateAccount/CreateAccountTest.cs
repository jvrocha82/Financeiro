using Financial.Application.UseCases.Account.CreateAccount;
using Financial.Domain.Exceptions;
using Financial.UnitTests.Application.CreateAccount;
using FluentAssertions;
using Moq;
using Xunit;

using UseCases = Financial.Application.UseCases.Account.CreateAccount;
using DomainEntity = Financial.Domain.Entity;

namespace Financial.UnitTests.Application.Account.CreateAccount;

[Collection(nameof(CreateAccountTestFixture))]
public class CreateAccountTest
{
    private readonly CreateAccountTestFixture _fixture;

    public CreateAccountTest(CreateAccountTestFixture fixture)
    => _fixture = fixture;

    [Fact(DisplayName = nameof(CreateAccount))]
    [Trait("Application", "CreateAccount - Use Cases")]
    public async void CreateAccount()
    {
        var repositoryMock = CreateAccountTestFixture.GetRepositoryMock();
        var unitOfWorkMock = CreateAccountTestFixture.GetUnitOfWorkMock();

        var useCase = new UseCases.CreateAccount(
                repositoryMock.Object,
                unitOfWorkMock.Object
            );
        var input = _fixture.GetInput();

        var output = await useCase.Handle(input, CancellationToken.None);

        repositoryMock.Verify(
            repository => repository.Insert(
                It.IsAny<DomainEntity.Account>(),
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
    [Theory(DisplayName = nameof(ThrowWhenCantInstantiateAccount))]
    [Trait("Application", "CreateAccount - Use Cases")]
    [MemberData(
        nameof(CreateAccountTestDataGenerator.GetInvalidInputs),
        parameters: 12,
        MemberType = typeof(CreateAccountTestDataGenerator)
        )]


    public async void ThrowWhenCantInstantiateAccount(
        CreateAccountInput input,
        string exceptionMessage)
    {
        var useCase = new UseCases.CreateAccount(
            CreateAccountTestFixture.GetRepositoryMock().Object,
            CreateAccountTestFixture.GetUnitOfWorkMock().Object
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
