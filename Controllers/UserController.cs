using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Primeira_api_data_driven_asp.Data;
using Primeira_api_data_driven_asp.Models;
using Primeira_api_data_driven_asp.Services;

namespace Primeira_api_data_driven_asp.Controllers
{
    [Route("v1/users")]
    public class UserController : ControllerBase
    {
        [HttpGet]
        [Route("")]
        [Authorize(Roles = "manager")]
        public async Task<ActionResult<List<User>>> Get([FromServices] DataContext context)
        {
            var users = await context
                        .Users
                        .AsNoTracking()
                        .ToListAsync();
            return Ok(users);                  
        }

        [HttpPost]
        [Route("")]
        [AllowAnonymous]
        public async Task<ActionResult<User>> Post([FromServices] DataContext context, [FromBody] User model)
        {
            if(!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            else
            {
                try
                {
                    //Força o usuário a ser sempre um funcionário
                    model.Role = "employee";

                    context.Users.Add(model);
                    await context.SaveChangesAsync();

                    //Esconde a senha cadastrada
                    model.Password = "";
                    
                    return Ok(model);
                }
                catch(Exception)
                {
                    return BadRequest( new { message = "Não foi possível criar o usuário." } );
                }                
            }       
        }

        [HttpPut]
        [Route("{id:int}")]
        [Authorize(Roles = "manager")]
        public async Task<ActionResult<User>> Put([FromServices] DataContext context,int id, [FromBody] User model)
        {
            if(!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            else if(id != model.Id)
            {
                return NotFound(new { message = "Usuário não encontrado."});
            }
            else
            {
                try
                {
                    context.Entry(model).State = EntityState.Modified;
                    await context.SaveChangesAsync();
                    return Ok(model);
                }
                catch(Exception)
                {
                    return BadRequest( new { message = "Não foi possível criar o usuário." } );
                }                
            }       
        }

        [HttpPost]
        [Route("login")]
        public async Task<ActionResult<dynamic>> Authenticate([FromServices] DataContext context, [FromBody] User model)
        {
            // Recupera o usuário
            var user = await context
                .Users.AsNoTracking()
                .Where(x => x.Username == model.Username && x.Password == model.Password)
                .FirstOrDefaultAsync();

            // Verifica se o usuário existe
            try
            {
                if (user == null)
                {
                    return NotFound(new { message = "Usuário ou senha inválidos" });
                }
                else
                {
                    // Gera o Token
                    var token = TokenService.GenerateToken(user);

                    // Oculta a senha
                    user.Password = "";
                
                    // Retorna os dados
                    return new
                    {
                        user = user,
                        token = token
                    };
                }
            }catch(Exception)
            {
                return BadRequest( new { message = "Não foi possível gerar o token." } );
            }                                  
        }
    }  
}

