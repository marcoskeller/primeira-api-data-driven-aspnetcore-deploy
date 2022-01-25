using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Primeira_api_data_driven_asp.Data;
using Primeira_api_data_driven_asp.Models;


namespace Primeira_api_data_driven_asp.Controllers
{
    [Route("v1")]
    public class HomeController : Controller
    {
        [HttpGet]
        [Route("")]
        public async Task<ActionResult<dynamic>> Get([FromServices] DataContext context)
        {
            var employee = new User { Id = 1, Username = "teste", Password = "teste", Role = "employee" };
            var manager = new User { Id = 2, Username = "master", Password = "master", Role = "manager" };
            var category = new Category { Id = 1, Title = "Categoria 1"};
            var product = new Product { Id = 1, Category = category, Title = "Product 1", Price = 100, Description = "Descrição do Produto"};

            context.Users.Add(employee);
            context.Users.Add(manager);
            context.Categories.Add(category);
            context.Products.Add(product);

            await context.SaveChangesAsync();

            return Ok(new 
            {
                message = "Dados Configurados."
            });
        }
    }
}