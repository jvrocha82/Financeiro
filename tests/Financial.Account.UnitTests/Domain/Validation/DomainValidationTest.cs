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
        string fieldName = Faker.Commerce.ProductName().Replace(" ", "");
        Action action =
            () => DomainValidation.NotNull(nullValue, fieldName);
        action.Should()
            .Throw<EntityValidationException>()
            .WithMessage($"{fieldName} should not be null");
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
        string fieldName = Faker.Commerce.ProductName().Replace(" ", "");

        Action action = 
            () => DomainValidation.NotNullOrEmpty(target, fieldName);

        action.Should()
            .Throw<EntityValidationException>()
            .WithMessage($"{fieldName} should not be empty or null");
    }

    [Fact(DisplayName = nameof(NotNullOrEmptyOk))]
    [Trait("Domain", "DomainValidation - Validation")]
    public void NotNullOrEmptyOk()
    {
        string fieldName = Faker.Commerce.ProductName().Replace(" ", "");

        var target = Faker.Commerce.ProductName();

        Action action =
            () => DomainValidation.NotNullOrEmpty(target, fieldName);

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
        string fieldName = Faker.Commerce.ProductName().Replace(" ", "");

        Action action = 
            () => DomainValidation.MinLength(target, minLength, fieldName);

        action.Should()
            .Throw<EntityValidationException>()
            .WithMessage($"{fieldName} should be at leats {minLength} characters long");
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
        string fieldName = Faker.Commerce.ProductName().Replace(" ", "");

        Action action =
            () => DomainValidation.MinLength(target, minLength, fieldName);

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
    [Theory(DisplayName = nameof (MaxLengthThrowWhenGrater))]
    [Trait("Domain", "DomainValidation - Validation")]
    [MemberData(nameof(GetValuesGreaterThanTheMax), 10)]

    public void MaxLengthThrowWhenGrater(string target, int maxLength)
    {
        string fieldName = Faker.Commerce.ProductName().Replace(" ", "");

        Action action = 
            () => DomainValidation.MaxLength(target, maxLength, fieldName);
        action.Should()
            .Throw<EntityValidationException>()
            .WithMessage($"{fieldName} should be less or equal {maxLength} characters long");
    }
    public static IEnumerable<object[]> GetValuesGreaterThanTheMax(int numberOfTests = 5)
    {
        yield return new object[] { "123456", 5 };
        var faker = new Faker();
        for (int i = 0; i < (numberOfTests - 1); i++)
        {
            string stringTarget = faker.Commerce.ProductName();
            var maxLength = stringTarget.Length - (new Random()).Next(1, 5);
            yield return new object[] { stringTarget, maxLength };
        }
    }

    [Theory(DisplayName = nameof(MaxLenghtOk))]
    [Trait("Domain", "DomainValidation - Validation")]
    [MemberData(nameof(GetValuesLessThanTheMax), 10)]
    public void MaxLenghtOk(string target, int maxLength)
    {
        Action action =
            () => DomainValidation.MaxLength(target, maxLength, "fieldName");

        action.Should()
            .NotThrow<EntityValidationException>();
    }

    public static IEnumerable<object[]> GetValuesLessThanTheMax(int numberOfTests = 5)
    {
        yield return new object[] { "123456", 6 };
        var faker = new Faker();
        for (int i = 0; i < (numberOfTests - 1); i++)
        {
            string stringTarget = faker.Commerce.ProductName();
            var maxLength = stringTarget.Length + (new Random()).Next(0, 5);
            yield return new object[] { stringTarget, maxLength };
        }
    }

    [Theory(DisplayName = nameof(IsNegativeValueOk))]
    [Trait("Domain", "DomainValidation - Validation")]
    [MemberData(nameof(GetNotNegativeValues), 10)]
    public void IsNegativeValueOk(int target, string fieldName)
    {


        Action action =
            () => DomainValidation.IsNegativeDecimalValue(target, fieldName);
        action.Should()
                  .NotThrow<EntityValidationException>();
    }
    public static IEnumerable<object[]> GetNotNegativeValues(int numberOfTests = 5)
    { 
      
        var faker = new Faker();
        for (int i = 0; i < (numberOfTests - 1); i++)
        {
            string stringTarget = faker.Commerce.ProductName();
            var notNegativeValue = (new Random()).Next(0, 5);
            yield return new object[] { notNegativeValue, stringTarget };
        }
    }

    [Theory(DisplayName = nameof(IsNegativeThrowWhenLessThanZeroValue))]
    [Trait("Domain", "DomainValidation - Validation")]
    [MemberData(nameof(GetNegativeValues), 10)]
    public void IsNegativeThrowWhenLessThanZeroValue(int target, string fieldName)
    {


        Action action =
            () => DomainValidation.IsNegativeDecimalValue(target, fieldName);
        action.Should()
                  .Throw<EntityValidationException>()
                  .WithMessage($"{fieldName} cannot be negative value");
    }
    public static IEnumerable<object[]> GetNegativeValues(int numberOfTests = 5)
    {

        var faker = new Faker();
        for (int i = 0; i < (numberOfTests - 1); i++)
        {
            string stringTarget = faker.Commerce.ProductName();
            var negativeValue = (new Random()).Next(1, 5) * -1;
            yield return new object[] { negativeValue, stringTarget };
        }
    }


}
