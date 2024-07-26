using CommonTestUtilities.Requests;
using FluentAssertions;
using MyRecipeBook.Application.UseCases.User.Update;
using MyRecipeBook.Exceptions;

namespace Validators.Test.User.Update;

public class UpdateUserValidatorTest
{
    [Fact]
    public void Success()
    {
        var validator = new UpdateUserValidator();

        var request = RequestUpdateUserBuilder.Build();

        var result = validator.Validate(request);

        result.IsValid.Should().BeTrue();
        Assert.True(result.IsValid);
    }

    [Fact]
    public void Error_Name_Empty()
    {
        var validator = new UpdateUserValidator();

        var request = RequestUpdateUserBuilder.Build();
        request.Name = string.Empty;

        var result = validator.Validate(request);

        result.IsValid.Should().BeFalse();
        result.Errors.Should().ContainSingle()
            .And.Contain(e => e.ErrorMessage.Equals(ResourceMessagesException.Name_Empty));
        Assert.False(result.IsValid);
    }

    [Fact]
    public void Error_Email_Empty()
    {
        var validator = new UpdateUserValidator();

        var request = RequestUpdateUserBuilder.Build();
        request.Email = string.Empty;

        var result = validator.Validate(request);

        result.IsValid.Should().BeFalse();

        result.Errors.Should().ContainSingle()
            .And.Contain(e => e.ErrorMessage.Equals(ResourceMessagesException.Email_Empty));
        Assert.False(result.IsValid);
    }

    [Fact]
    public void Error_Email_Invalid()
    {
        var validator = new UpdateUserValidator();

        var request = RequestUpdateUserBuilder.Build();
        request.Email = "email.com";

        var result = validator.Validate(request);

        result.IsValid.Should().BeFalse();

        result.Errors.Should().ContainSingle();
        Assert.False(result.IsValid);
    }
}
