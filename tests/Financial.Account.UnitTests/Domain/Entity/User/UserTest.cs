using Xunit;
using FluentAssertions;
using DomainEntity = Financial.Domain.Entity;
using Financial.Domain.Exceptions;


namespace Financial.UnitTests.Domain.Entity.User;

[Collection(nameof(UserTestFixture))]

public class UserTest
{
    private readonly UserTestFixture _userTestFixture;

    public UserTest(UserTestFixture userTestFixture)
        => _userTestFixture = userTestFixture;

    [Fact(DisplayName =nameof(Instantiate))]
    [Trait("Domain", "User - Aggregate")]
    public void Instantiate()
    {
        var validUser = _userTestFixture.GetValidUser();
        var datetimeBefore = DateTime.Now.AddSeconds(-1);
        var user = new DomainEntity.User(validUser.Name, true);
        var datetimeAfter = DateTime.Now.AddSeconds(1);

        Guid GuidExampleValue = new Guid();
        var typeGuidName = GuidExampleValue.GetType().Name;



        user.Should().NotBeNull();

        user.Id.GetType().Name.Should().Be(typeGuidName);
        user.Id.Should().NotBe(default(Guid));
        user.Name.Should().Be(validUser.Name);
        user.CreatedAt.Should().NotBeSameDateAs(default(DateTime));
        user.CreatedAt.Should().BeAfter(datetimeBefore);
        user.CreatedAt.Should().BeBefore(datetimeAfter);
        user.IsActive.Should().BeTrue();
    }

    [Theory(DisplayName = nameof(InstantiateErrorWhenNameIsEmpty))]
    [Trait("Domain", "User - Aggregate")]
    [InlineData("")]
    [InlineData(null)]
    [InlineData("    ")]

    public void InstantiateErrorWhenNameIsEmpty(string? name)
    {
  
        Action action = () 
            => new DomainEntity.User(name!, true);

        action.Should()
            .Throw<EntityValidationException>()
            .WithMessage("Name should not be empty or null");
    }
    [Theory(DisplayName = nameof(InstantiateErrorWhenNameIsLessThan3Characters))]
    [Trait("Domain", "User - Aggregate")]
    [MemberData(nameof(GetNamesWhenNameIsLessThan3Characters), parameters: 10)]
    public void InstantiateErrorWhenNameIsLessThan3Characters(string name)
    {
        Action action = ()
            => new DomainEntity.User(name!, true);
        action.Should()
            .Throw<EntityValidationException>()
            .WithMessage("Name should be at leats 3 characters long");
    }
    public static IEnumerable<object[]> GetNamesWhenNameIsLessThan3Characters(int numberOfTests = 6)
    {
        var fixture = new UserTestFixture();
        for (int i = 0; i < numberOfTests; i++)
        {
            var isOdd =  1 % 2 == 1;
            yield return new object[] {
                fixture.GetValidName()[..(isOdd ? 1 : 2)]
            };
        }
       
   

    }
    [Fact(DisplayName = nameof(InstantiateErrorWhenNameIsGreaterThan255Characters))]
    [Trait("Domain", "User - Aggregate")]

    public void InstantiateErrorWhenNameIsGreaterThan255Characters()
    {
        var invalidName = String.Join(null, Enumerable.Range(0, 256).Select(_ => "a").ToArray());
       
        Action action = ()
            => new DomainEntity.User(invalidName, true);
        
        action.Should()
            .Throw<EntityValidationException>()
            .WithMessage("Name should be less or equal 255 characters long");
    }

    [Fact(DisplayName = nameof(UpdateOnlyName))]
    [Trait("Domain", "User - Aggregate")]

    public void UpdateOnlyName()
    {
        var user = _userTestFixture.GetValidUser();
        var newUserName = _userTestFixture.GetValidName();

        user.Update(newUserName);

        
        user.Name.Should().Be(newUserName);
    }

    [Theory(DisplayName = nameof(UpdateErrorWhenEmptyValue))]
    [Trait("Domain", "User - Aggregate")]
    [InlineData("     ")]
    [InlineData("")]
    [InlineData(null)]

    public void UpdateErrorWhenEmptyValue(string? wrongName)
    {
        var user = _userTestFixture.GetValidUser();

        Action action = () => user.Update(wrongName!);

        action.Should().Throw<EntityValidationException>()
            .WithMessage("Name should not be empty or null");
    }

    [Fact(DisplayName = nameof(UpdateActiveUser))]
    [Trait("Domain", "User - Aggregate")]

    public void UpdateActiveUser()
    {
        var user = _userTestFixture.GetValidUser(false);

        user.Activate();
        
        user.Should().NotBeNull();
        user.IsActive.Should().BeTrue();

    }
    [Fact(DisplayName = nameof(UpdateDeactivateUser))]
    [Trait("Domain", "User - Aggregate")]

    public void UpdateDeactivateUser()
    {
        var user = _userTestFixture.GetValidUser();

        user.Deactivate();

        user.Should().NotBeNull();
        user.IsActive.Should().BeFalse();
    }




}
