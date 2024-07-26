using AutoMapper;
using MyRecipeBook.Communication.Requests;
using MyRecipeBook.Domain;
using MyRecipeBook.Domain.Repositories.Recipe;
using MyRecipeBook.Domain.Services.LoggedUser;
using MyRecipeBook.Exceptions;
using MyRecipeBook.Exceptions.ExceptionBase;

namespace MyRecipeBook.Application.UseCases.Recipe.Update;

public class UpdateRecipeUseCase : IUpdateRecipeUseCase
{
    private readonly IRecipeUpdateOnlyRepository _repository;
    private readonly ILoggedUser _loggedUser;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public UpdateRecipeUseCase(IRecipeUpdateOnlyRepository repository, ILoggedUser loggedUser, IUnitOfWork unitOfWork, IMapper mapper)
    {
        _repository = repository;
        _loggedUser = loggedUser;
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task Execute(long recipeId, RequestRecipe request)
    {
        Validate(request);

        var loggerUser = await _loggedUser.User();

        var recipe = await _repository.GetById(loggerUser, recipeId);
        if(recipe is null)
            throw new NotFoundException(ResourceMessagesException.Recipe_Not_Found);

        recipe.Ingredients.Clear();
        recipe.Instructions.Clear();
        recipe.DishTypes.Clear();

        _mapper.Map(request, recipe);

        var instructions = request.Instructions.OrderBy(i => i.Step).ToList();
        for (int index = 0; index < instructions.Count; index++)
            instructions[index].Step = index + 1;

        recipe.Instructions = _mapper.Map<IList<Domain.Entities.Instruction>>(instructions);

        _repository.Update(recipe);

        await _unitOfWork.Commit();
    }

    private static void Validate(RequestRecipe request)
    {
        var result = new RecipeValidator().Validate(request);

        if (!result.IsValid)
            throw new ErrorOnValidationException(result.Errors.Select(e => e.ErrorMessage).Distinct().ToList());
    }
}
