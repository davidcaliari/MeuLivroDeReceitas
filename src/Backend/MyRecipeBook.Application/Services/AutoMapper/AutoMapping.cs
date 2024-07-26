using AutoMapper;
using MyRecipeBook.Communication.Enums;
using MyRecipeBook.Communication.Requests;
using MyRecipeBook.Communication.Response;
using Sqids;

namespace MyRecipeBook.Application.Services.AutoMapper;

public class AutoMapping : Profile
{
    //private readonly SqidsEncoder<long> _idEncoder;

    public AutoMapping(/*SqidsEncoder<long> idEncoder*/)
    {
        RequestToDomain();
        DomainToResponse();
        //_idEncoder = idEncoder;
    }

    private void RequestToDomain()
    {
        CreateMap<RequestsRegisterUser, Domain.Entities.User>()
            .ForMember(dest => dest.Password, opt => opt.Ignore());

        CreateMap<RequestRecipe, Domain.Entities.Recipe>()
            .ForMember(dest => dest.Instructions, opt => opt.Ignore())
            .ForMember(dest => dest.Ingredients, opt => opt.MapFrom(source => source.Ingredients.Distinct()))
            .ForMember(dest => dest.DishTypes, opt => opt.MapFrom(source => source.DishTypes.Distinct()));

        CreateMap<string, Domain.Entities.Ingredient>()
            .ForMember(dest => dest.Item, opt => opt.MapFrom(source => source));

        CreateMap<DishType, Domain.Entities.DishType>()
            .ForMember(dest => dest.Type, opt => opt.MapFrom(source => source));

        CreateMap<RequestInstruction, Domain.Entities.Instruction>();
    }

    private void DomainToResponse()
    {
        var _idEncoder = new SqidsEncoder<long>();
        CreateMap<Domain.Entities.User, ResponseUserProfile>();

        CreateMap<Domain.Entities.Recipe, ResponseRegisteredRecipe>()
            .ForMember(dest => dest.Id, opt => opt.MapFrom(source => _idEncoder.Encode(source.Id)));

        CreateMap<Domain.Entities.Recipe, ResponseShortRecipe>()
            .ForMember(dest => dest.Id, opt => opt.MapFrom(source => _idEncoder.Encode(source.Id)))
            .ForMember(dest => dest.AmountIngredients, opt => opt.MapFrom(source => source.Ingredients.Count));



        CreateMap<Domain.Entities.Recipe, ResponseRecipe>()
            .ForMember(dest => dest.Id, opt => opt.MapFrom(source => _idEncoder.Encode(source.Id)))
            .ForMember(dest => dest.DishTypes, opt => opt.MapFrom(source => source.DishTypes.Select(r => r.Type)));

        CreateMap<Domain.Entities.Ingredient, ResponseIngredient>()
            .ForMember(dest => dest.Id, opt => opt.MapFrom(source => _idEncoder.Encode(source.Id)));

        CreateMap<Domain.Entities.Instruction, ResponseInstruction>()
        .ForMember(dest => dest.Id, opt => opt.MapFrom(source => _idEncoder.Encode(source.Id)));
    }
}
