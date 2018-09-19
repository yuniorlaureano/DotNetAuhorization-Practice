using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WebApiJwt.Models;

namespace WebApiJwt.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ResourcesController : ControllerBase
    {
        [Authorize(Policy = "TrainedStaffOnly")]
        [HttpGet("employess")]
        public IActionResult Employees()
        {
            var userName = User.Identity.Name;

            return Ok(new[]
            {
                new {Name = "Franklin", LastName = "Almeida" },
                new {Name = "Danilo", LastName = "Medina" },
                new {Name = "Doctor", LastName = "Fadul" },
            });
        }
    }
}