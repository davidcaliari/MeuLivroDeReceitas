using Microsoft.AspNetCore.Mvc.ModelBinding;
using Sqids;

namespace MyRecipeBook.API.Binders;

public class MyRecipeBookIdBinder : IModelBinder
{
    //private readonly SqidsEncoder<long> _idEncoder;

    //public MyRecipeBookIdBinder(SqidsEncoder<long> idEncoder) => _idEncoder = idEncoder;

    public Task BindModelAsync(ModelBindingContext bindingContext)
    {
        ArgumentNullException.ThrowIfNull(bindingContext);

        var sqids = new SqidsEncoder<long>();

        var modelName = bindingContext.ModelName;

        var valueProviderResult = bindingContext.ValueProvider.GetValue(modelName);

        if (valueProviderResult == ValueProviderResult.None)
            return Task.CompletedTask;

        bindingContext.ModelState.SetModelValue(modelName, valueProviderResult);

        var value = valueProviderResult.FirstValue;

        if (string.IsNullOrWhiteSpace(value))
            return Task.CompletedTask;

        var id = sqids.Decode(value).Single();
        //var id = _idEncoder.Decode(value).Single();

        bindingContext.Result = ModelBindingResult.Success(id);

        return Task.CompletedTask;
    }
}
