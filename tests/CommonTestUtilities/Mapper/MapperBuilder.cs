﻿using AutoMapper;
using CommonTestUtilities.IdEncryption;
using MyRecipeBook.Application.Services.AutoMapper;

namespace CommonTestUtilities.Mapper;

public class MapperBuilder
{
    public static IMapper Build()
    {
        var idEncripter = IdEncripterBuilder.Build();

        return new MapperConfiguration(option =>
        {
            option.AddProfile(new AutoMapping(idEncripter));
        }).CreateMapper();
    }
}
