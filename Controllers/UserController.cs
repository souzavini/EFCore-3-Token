using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using Microsoft.AspNetCore.Authorization;
using System.Linq;
using Backoffice.Services;
using Shop.Models;
using Shop.Data;

namespace Backoffice.Controllers
{
    [Route("users")]
    public class UserController : Controller
    {
        [HttpGet]
        [Route("")]
        [Authorize(Roles = "manager")]
        public async Task<ActionResult<List<User>>> Get([FromServices] DataContext context)
        {
            var users = await context.Users
            .AsNoTracking()
            .ToListAsync();

            return users;
        }

        [HttpPost]
        [Route("")]
        [AllowAnonymous]
        // [Authorize(Roles = "manager")]
        public async Task<ActionResult<User>> Post(
            [FromServices] DataContext context,
            [FromBody]User model)
        {
           if(!ModelState.IsValid)
           {
               return BadRequest(ModelState);
           }

           try
           {
               // Força usuario ser sempre funcionario
               model.Role = "employee";

               context.Users.Add(model);
               await context.SaveChangesAsync();
               model.Password ="";
               return model;
           }
           catch (Exception)
           {
               
               return BadRequest(new {message = "Não foi possível criar o usuario"});
           }

        }

        [HttpPut]
        [Route("{id:int}")]
        [Authorize(Roles = "manager")]
        public async Task<ActionResult<User>> Put(
            [FromServices] DataContext context,
            int id,
            [FromBody]User model)
        {

           if(!ModelState.IsValid)
           {
               return BadRequest(ModelState);
           }

           if(id != model.Id)
           {
                return NotFound(new {message = "Usuario nao encontrado"});
           }

            try
            {
                context.Entry(model).State = EntityState.Modified;
                await context.SaveChangesAsync();

                return Ok(model);
            }
            catch (Exception)
            {
                
                return BadRequest(new {message = "Erro nao mapeado"});
            }
            
        }

        [HttpPost]
        [Route("login")]
        [AllowAnonymous]
        public async Task<ActionResult<dynamic>> Authenticate(
                    [FromServices] DataContext context,
                    [FromBody]User model)
        {
            var user = await context.Users
                .AsNoTracking()
                .Where(x=> x.Username == model.Username && x.Password == model.Password)
                .FirstOrDefaultAsync();

            if(user == null)
            {
                return NotFound(new {message = "Usuario ou senha invalidos"});
            }

            var token = TokenService.GenerateToken(user);
            //esconde senha 
            model.Password ="";
            return new
            {
                user = user,
                token= token
            };
        }
    }
}