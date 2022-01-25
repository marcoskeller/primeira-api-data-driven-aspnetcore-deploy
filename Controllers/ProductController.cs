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
    [Route("v1/products")]
    public class ProductController : ControllerBase
    {
        [HttpGet]
        [Route("")]
        [AllowAnonymous]
        public async Task<ActionResult<List<Product>>> Get([FromServices] DataContext context)
        {
            var products = await context
                .Products
                .Include(x => x.Category)
                .AsNoTracking()
                .ToListAsync();
            
            if(products.Count == 0 || products == null)
            {
                return NotFound(new { message = "Não existe produto(s) cadastrado(s)."});
            }
            else
            {
                return Ok(products);
            }       
        }

        [HttpGet]
        [Route("{id:int}")]
        [AllowAnonymous]
        public async Task<ActionResult<Product>> GetById(int id, [FromServices] DataContext context)
        {
            var product = await context
                .Products
                .Include(x => x.Category)
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.Id == id);

            if(product == null)
            {
                return NotFound(new { message = "Produto não encontrado."});
            }
            else
            {
                return Ok(product);
            }
            
        }

        [HttpGet]
        [Route("categories/{id:int}")]
        [AllowAnonymous]
        public async Task<ActionResult<List<Product>>> GetByCategory([FromServices] DataContext context, int id)
        {
            var products = await context
                .Products
                .Include(x => x.Category)
                .AsNoTracking()
                .Where(x => x.CategoryId == id)
                .ToListAsync();
            
            if(products.Count == 0 || products == null)
            {
                return NotFound(new { message = "Não existe produto(s) cadastrado(s) para a categoria informada."});
            }
            else
            {
                return Ok(products);
            }
            
        }

        [HttpPost]
        [Route("")]
        [Authorize(Roles = "employee")]
        public async Task<ActionResult<Product>> Post([FromServices] DataContext context, [FromBody] Product model)
        {
            if(!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            else
            {
                try
                {
                    context.Products.Add(model);
                    await context.SaveChangesAsync();
                    return Ok(model);
                }
                catch(Exception)
                {
                    return BadRequest( new { message = "Não foi possível inserir o produto." } );
                }
                
            }      
        }

    }
}