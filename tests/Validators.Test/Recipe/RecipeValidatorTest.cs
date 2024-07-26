using CommonTestUtilities.Requests;
using FluentAssertions;
using MyRecipeBook.Application.UseCases.Recipe;
using MyRecipeBook.Communication.Enums;
using MyRecipeBook.Exceptions;

namespace Validators.Test.Recipe;

public class FilterRecipeValidatorTest
{
    [Fact]
    public void Success_Cooking_Time_Null()
    {
        var validator = new RecipeValidator();

        var request = RequestRecipeBuilder.Build();
        request.CookingTime = null;

        var result = validator.Validate(request);

        result.IsValid.Should().BeTrue();
    }

    [Fact]
    public void Success_Difficult_Null()
    {
        var validator = new RecipeValidator();

        var request = RequestRecipeBuilder.Build();
        request.Difficulty = null;

        var result = validator.Validate(request);

        result.IsValid.Should().BeTrue();
    }

    [Fact]
    public void Success()
    {
        var validator = new RecipeValidator();

        var request = RequestRecipeBuilder.Build();

        var result = validator.Validate(request);

        result.IsValid.Should().BeTrue();
    }

    [Fact]
    public void Success_DishTypes_Empty()
    {
        var validator = new RecipeValidator();

        var request = RequestRecipeBuilder.Build();
        request.DishTypes.Clear();

        var result = validator.Validate(request);

        result.IsValid.Should().BeTrue();
    }

    [Theory]
    [InlineData(null)]
    [InlineData("         ")]
    [InlineData("")]
    public void Error_Empty_Title(string title)
    {
        var validator = new RecipeValidator();

        var request = RequestRecipeBuilder.Build();
        request.Title = title;

        var result = validator.Validate(request);

        result.IsValid.Should().BeFalse();
        result.Errors.Should().ContainSingle().And.Contain(e => e.ErrorMessage.Equals(ResourceMessagesException.Recipe_Title_Empty));
    }

    [Theory]
    [InlineData(null)]
    [InlineData("         ")]
    [InlineData("")]
    public void Error_Empty_Value_Ingredients(string ingredient)
    {
        var validator = new RecipeValidator();

        var request = RequestRecipeBuilder.Build();
        request.Ingredients.Add(ingredient);

        var result = validator.Validate(request);

        result.IsValid.Should().BeFalse();
        result.Errors.Should().ContainSingle();
    }

    [Theory]
    [InlineData(null)]
    [InlineData("         ")]
    [InlineData("")]
    public void Error_Empty_Value_Instructions(string instruction)
    {
        var validator = new RecipeValidator();

        var request = RequestRecipeBuilder.Build();
        request.Instructions.First().Text = instruction;

        var result = validator.Validate(request);

        result.IsValid.Should().BeFalse();
        result.Errors.Should().ContainSingle();
    }

    [Fact]
    public void Error_Invalid_DishTypes()
    {
        var validator = new RecipeValidator();

        var request = RequestRecipeBuilder.Build();
        request.DishTypes.Add((DishType)1000);

        var result = validator.Validate(request);

        result.IsValid.Should().BeFalse();
        result.Errors.Should().ContainSingle();
    }

    [Fact]
    public void Error_Empty_Ingredients()
    {
        var validator = new RecipeValidator();

        var request = RequestRecipeBuilder.Build();
        request.Ingredients.Clear();

        var result = validator.Validate(request);

        result.IsValid.Should().BeFalse();
        result.Errors.Should().ContainSingle();
    }

    [Fact]
    public void Error_Empty_Instructions()
    {
        var validator = new RecipeValidator();

        var request = RequestRecipeBuilder.Build();
        request.Instructions.Clear();

        var result = validator.Validate(request);

        result.IsValid.Should().BeFalse();
        result.Errors.Should().ContainSingle();
    }

    [Fact]
    public void Error_Same_Step_Instructions()
    {
        var validator = new RecipeValidator();

        var request = RequestRecipeBuilder.Build();
        request.Instructions.First().Step = request.Instructions.Last().Step;

        var result = validator.Validate(request);

        result.IsValid.Should().BeFalse();
        result.Errors.Should().ContainSingle();
    }

    [Fact]
    public void Error_Negative_Step_Instructions()
    {
        var validator = new RecipeValidator();

        var request = RequestRecipeBuilder.Build();
        request.Instructions.First().Step = -1;

        var result = validator.Validate(request);

        result.IsValid.Should().BeFalse();
        result.Errors.Should().ContainSingle();
    }

    [Fact]
    public void Error_Invalid_Cooking_Time()
    {
        var validator = new RecipeValidator();

        var request = RequestRecipeBuilder.Build();
        request.CookingTime = (CookingTime?)1000;

        var result = validator.Validate(request);

        result.IsValid.Should().BeFalse();
    }

    [Fact]
    public void Error_Invalid_Difficult()
    {
        var validator = new RecipeValidator();

        var request = RequestRecipeBuilder.Build();
        request.Difficulty = (MyRecipeBook.Communication.Enums.Difficulty?)1000;

        var result = validator.Validate(request);

        result.IsValid.Should().BeFalse();
    }

    [Fact]
    public void Error_Instruction_Too_Long()
    {
        var validator = new RecipeValidator();

        var request = RequestRecipeBuilder.Build();
        request.Instructions.First().Text = RequestStringGenerator.Paragraphs(2001);

        var result = validator.Validate(request);

        result.IsValid.Should().BeFalse();
        result.Errors.Should().ContainSingle();
    }
}
