﻿using System;
using Microsoft.AspNetCore.Mvc;

namespace GeneratedWebService.Controllers
{
    public interface IUserRepository
    {
        IActionResult CreateUser(CreateUserCommand createUserCommand);
        IActionResult UpdateUserName(UpdateUserNameCommand updateUserNameCommand);
        IActionResult GetUser(Guid id);
    }
}