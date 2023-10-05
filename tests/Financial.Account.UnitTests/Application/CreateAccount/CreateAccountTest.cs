using Financial.Application.UseCases.Account.CreateAccount;
using Financial.Domain.Entity;
using Financial.Domain.Exceptions;
using FluentAssertions;
using Moq;
using Xunit;
using UseCases = Financial.Application.UseCases.Account.CreateAccount;

namespace Financial.UnitTests.Application.CreateAccount;
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
        var repositoryMock = _fixture.GetRepositoryMock();
        var unitOfWorkMock = _fixture.GetUnitOfWorkMock();

        var useCase = new UseCases.CreateAccount(
                repositoryMock.Object,
                unitOfWorkMock.Object
            );
        var input = _fixture.GetInput();

        var output = await useCase.Handle(input, CancellationToken.None);

        repositoryMock.Verify(
            repository => repository.Insert(
                It.IsAny<Account>(),
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
        (output.CreatedAt != default(DateTime)).Should().BeTrue();

    }
    [Theory(DisplayName = nameof(ThrowWhenCantInstantiateAggregate))]
    [Trait("Application", "CreateAccount - Use Cases")]
    [MemberData(nameof(GetInvalidInputs))]


    public async void ThrowWhenCantInstantiateAggregate(
        CreateAccountInput input,
        string exceptionMessage)
    {
        var useCase = new UseCases.CreateAccount(
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
    public static IEnumerable<object[]> GetInvalidInputs()
    {
        var fixture = new CreateAccountTestFixture();
        var invalidInputsList = new List<object[]>();
        var invalidInputShortName = fixture.GetInput();
        invalidInputShortName.Name = invalidInputShortName.Name[..2];
        invalidInputsList.Add(new object[]
        {
            invalidInputShortName,
            $"Name should be at leats 3 characters long"
        });

      
        var invalidInputTooLongName = fixture.GetInput();
        invalidInputTooLongName.Name = fixture.Faker.Lorem.Letter(256);
        invalidInputsList.Add(new object[]
        {
            invalidInputTooLongName,
            $"Name should be less or equal 255 characters long"
        });

        var invalidInputNegativeOpeningBalance = fixture.GetInput();
        invalidInputNegativeOpeningBalance.OpeningBalance= invalidInputNegativeOpeningBalance.OpeningBalance * -1;
        invalidInputsList.Add(new object[]
        {
            invalidInputNegativeOpeningBalance,
            $"OpeningBalance cannot be negative value"
        });

        

        return invalidInputsList;
      

    }
}
