using Bogus;
using Xunit;
using FluentAssertions;
using Financial.Domain.Validation;
using Financial.Domain.Exceptions;

namespace Financial.UnitTests.Domain.Validation;
public class DomainValidationTest
{
    public Faker Faker { get; set; } = new Faker();

    #region[NotNull Validation]

    [Fact(DisplayName = nameof(NotNullOk))]
    [Trait("Domain", "DomainValidation - Validation")]
    public void NotNullOk()
    {
        var notNullValue = Faker.Commerce.ProductName();
        Action action = 
            () => DomainValidation.NotNull(notNullValue, "Value");
        action.Should().NotThrow();
    }

    [Fact(DisplayName = nameof(NotNullThrowWhenNull))]
    [Trait("Domain", "DomainValidation - Validation")]
    public void NotNullThrowWhenNull()
    {
        string? nullValue = null;
        Action action =
            () => DomainValidation.NotNull(nullValue, "FieldName");
        action.Should()
            .Throw<EntityValidationException>()
            .WithMessage("FieldName should not be null");
    }

    #endregion[NotNull Validation]

    #region[NotNullOrEmpty Validation]
    [Theory(DisplayName = nameof(NotNullOrEmptyThrowWhenEmpty))]
    [Trait("Domain", "DomainValidation - Validation")]
    [InlineData("")]
    [InlineData("  ")]
    [InlineData(null)]
    public void NotNullOrEmptyThrowWhenEmpty(string? target)
    {
        
        Action action = 
            () => DomainValidation.NotNullOrEmpty(target, "fieldName");

        action.Should()
            .Throw<EntityValidationException>()
            .WithMessage("FieldName should not be null or empty");
    }

    [Fact(DisplayName = nameof(NotNullOrEmptyOk))]
    [Trait("Domain", "DomainValidation - Validation")]
    public void NotNullOrEmptyOk()
    {

        var target = Faker.Commerce.ProductName();

        Action action =
            () => DomainValidation.NotNullOrEmpty(target, "fieldName");

        action.Should()
            .NotThrow<EntityValidationException>();
    }
    #endregion[NotNullOrEmpty Validation]

    #region[MinLength Validation]
    [Theory(DisplayName = nameof(MinLenghtThrowWhenLess))]
    [Trait("Domain", "DomainValidation - Validation")]
    [MemberData(nameof(GetValuesSmallerThanTheMin), 10)]

    public void MinLenghtThrowWhenLess(string target, int minLength)
    {
        Action action = 
            () => DomainValidation.MinLength(target, minLength, "fieldName");

        action.Should()
            .Throw<EntityValidationException>()
            .WithMessage($"fieldName should not be less than {minLength}");
    }

    public static IEnumerable<object[]> GetValuesSmallerThanTheMin(int numberOfTests = 5) 
    {
        yield return new object[] { "123456", 10 };
        var faker = new Faker();
        for (int i = 0; i < (numberOfTests - 1); i++)
        {
            string stringTarget = faker.Commerce.ProductName();
            var minLength = stringTarget.Length + (new Random()).Next(1,20);
            yield return new object[] { stringTarget, minLength };
        }
    }

    [Theory(DisplayName = nameof(MinLenghtOk))]
    [Trait("Domain", "DomainValidation - Validation")]
    [MemberData(nameof(GetValuesGreaterThanTheMin), 10)]
    public void MinLenghtOk(string target, int minLength)
    {
        Action action =
            () => DomainValidation.MinLength(target, minLength, "fieldName");

        action.Should()
            .NotThrow<EntityValidationException>();
    }

    public static IEnumerable<object[]> GetValuesGreaterThanTheMin(int numberOfTests = 5)
    {
        yield return new object[] { "123456", 6 };
        var faker = new Faker();
        for (int i = 0; i < (numberOfTests - 1); i++)
        {
            string stringTarget = faker.Commerce.ProductName();
            var minLength = stringTarget.Length - (new Random()).Next(1, 5);
            yield return new object[] { stringTarget, minLength };
        }
    }
    #endregion[MinLength Validation]
}
