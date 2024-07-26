﻿using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;

namespace MyRecipeBook.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class MyRecipeBookBaseController : ControllerBase
{
    protected static bool IsNotAuthenticated(AuthenticateResult authenticate)
    {
        return
               !authenticate.Succeeded
            || authenticate.Principal is null
            || !authenticate.Principal.Identities.Any(id => id.IsAuthenticated);
    }
}
