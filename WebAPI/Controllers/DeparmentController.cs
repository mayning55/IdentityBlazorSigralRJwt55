﻿using DateClassLibrary;
using DateClassLibrary.Data;
using Microsoft.AspNetCore.Mvc;

namespace WebAPI.Controllers
{
    [Route("[controller]/[action]")]
    [ApiController]
    public class DeparmentController(IWebApiDataInterface<Department> dateInterface)
        : DateController<Department>(dateInterface)
    {
    }
}
