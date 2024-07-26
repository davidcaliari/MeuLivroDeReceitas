using CommonTestUtilities.Requests;
using CommonTestUtilities.Tokens;
using FluentAssertions;
using MyRecipeBook.Communication.Requests;
using MyRecipeBook.Domain.Enums;
using MyRecipeBook.Exceptions;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Net;
using System.Text.Json;
using WebApi.Test.InlineData;

namespace WebApi.Test.Recipe.Filter;

public class FilterRecipeTest : MyRecipeBookClassFixture
{
    private readonly string method = "api/recipe/filter";

    private readonly Guid _userIdentifier;

    private string _recipeTitle;
    private Difficulty _recipedDifficultyLevel;
    private CookingTime _recipeCookingTime;
    private IList<DishType> _recipeDishTypes;

    public FilterRecipeTest(CustomWebApplicationFactory factory) : base(factory)
    {
        _userIdentifier = factory.GetUserIdentifier();

        _recipeTitle = factory.GetRecipeTitle();
        _recipedDifficultyLevel = factory.GetRecipedDifficulty();
        _recipeCookingTime = factory.GetRecipeCookingTime();
        _recipeDishTypes = factory.GetRecipeDishTypes();
    }

    [Fact]
    public async Task Success()
    {

        var request = new RequestFilterRecipe
        {
            RecipeTitle_Ingredient = _recipeTitle,
            Difficulties = [(MyRecipeBook.Communication.Enums.Difficulty)_recipedDifficultyLevel],
            CookingTimes = [(MyRecipeBook.Communication.Enums.CookingTime)_recipeCookingTime],
            DishTypes = _recipeDishTypes.Select(d => (MyRecipeBook.Communication.Enums.DishType)d).ToList(),
        };

        var token = JwtTokenGeneratorBuilder.Build().Generate(_userIdentifier);
        var response = await DoPost(method: method, request: request, token: token);

        response.StatusCode.Should().Be(HttpStatusCode.OK);

        await using var responseBody = await response.Content.ReadAsStreamAsync();
        var responseData = await JsonDocument.ParseAsync(responseBody);

        responseData.RootElement.GetProperty("recipes").EnumerateArray().Should().NotBeNullOrEmpty();
    }

    [Fact]
    public async Task Success_NoContent()
    {
        var request = RequestFilterRecipeBuilder.Build();
        request.RecipeTitle_Ingredient = "recipeDontExist";

        var token = JwtTokenGeneratorBuilder.Build().Generate(_userIdentifier);
        var response = await DoPost(method: method, request: request, token: token);

        response.StatusCode.Should().Be(HttpStatusCode.NoContent);
    }

    [Theory]
    [ClassData(typeof(CultureInlineDataTest))]
    public async Task Error_CookingTime_Invalid(string culture)
    {
        var request = RequestFilterRecipeBuilder.Build();
        request.CookingTimes.Add((MyRecipeBook.Communication.Enums.CookingTime)1000);

        var token = JwtTokenGeneratorBuilder.Build().Generate(_userIdentifier);

        var response = await DoPost(method: method, request: request, token: token, culture: culture);

        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);

        await using var responseBody = await response.Content.ReadAsStreamAsync();
        var responseData = await JsonDocument.ParseAsync(responseBody);

        var errors = responseData.RootElement.GetProperty("errors").EnumerateArray();

        errors.Should().ContainSingle();
    }
}
