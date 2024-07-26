using CommonTestUtilities.Requests;
using FluentAssertions;
using MyRecipeBook.Application.UseCases.Recipe;
using MyRecipeBook.Application.UseCases.Recipe.Filter;
using MyRecipeBook.Communication.Enums;
using MyRecipeBook.Communication.Requests;
using MyRecipeBook.Exceptions;

namespace Validators.Test.Recipe.Filter;

public class FilterRecipeValidatorTest
{


    [Fact]
    public void Success()
    {
        var validator = new FilterRecipeValidator();

        var request = RequestFilterRecipeBuilder.Build();

        var result = validator.Validate(request);

        result.IsValid.Should().BeTrue();
    }


    [Fact]
    public void Error_Invalid_Cooking_Time()
    {
        var validator = new FilterRecipeValidator();

        var request = RequestFilterRecipeBuilder.Build();
        request.CookingTimes.Add((CookingTime)1000);

        var result = validator.Validate(request);

        result.IsValid.Should().BeFalse();
        result.Errors.Should().ContainSingle();
    }

    [Fact]
    public void Error_Invalid_Difficult()
    {
        var validator = new FilterRecipeValidator();

        var request = RequestFilterRecipeBuilder.Build();
        request.Difficulties.Add((Difficulty)1000);

        var result = validator.Validate(request);

        result.IsValid.Should().BeFalse();
        result.Errors.Should().ContainSingle();
    }

    [Fact]
    public void Error_Invalid_DishTypes()
    {
        var validator = new FilterRecipeValidator();

        var request = RequestFilterRecipeBuilder.Build();
        request.DishTypes.Add((DishType)1000);

        var result = validator.Validate(request);

        result.IsValid.Should().BeFalse();
        result.Errors.Should().ContainSingle();
    }
}
