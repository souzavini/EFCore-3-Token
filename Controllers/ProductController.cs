
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Shop.Data;
using Shop.Models;

namespace Shop.Controllers 
{
    [Route("products")]
    public class ProductController : ControllerBase
    {

    [HttpGet]
    [Route("")]
    public async Task<ActionResult<List<Product>>> Get(
        [FromServices] DataContext context)
        {
            var products = await context.Products
                .Include(x => x.Category)
                .AsNoTracking()
                .ToListAsync();
                
            return Ok(products);
        }

    [HttpGet]
    [Route("{id:int}")]
    [AllowAnonymous]
    [ResponseCache(VaryByHeader = "User-Agent", Location = ResponseCacheLocation.Any, Duration = 30)]
    public async Task<ActionResult<Product>> Get(
        [FromServices] DataContext context , int id)
        {
            var products = await context.Products
                .Include(x => x.Category)
                .AsNoTracking()
                .FirstOrDefaultAsync(x=> x.Id == id);
            return products;
        }

    [HttpGet]
    [Route("categories/{id:int}")]
    [AllowAnonymous]
    public async Task<ActionResult<List<Product>>> GetByCategory(
        [FromServices] DataContext context , int id)
        {
            var products = await context.Products
                .Include(x => x.Category)
                .AsNoTracking()
                .Where(x=> x.CategoryId == id)
                .ToListAsync();
               
            return products;
        }


    [HttpPost]
    [Route("")]
    [Authorize(Roles = "employee")]
    public async Task<ActionResult<Product>> Post(
        [FromBody] Product model,
        [FromServices] DataContext context)

    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        try
        {
            context.Products.Add(model);
            await context.SaveChangesAsync();

            return Ok(model);
        }
        catch
        {

            return BadRequest(new { message = "Não foi possível criar o produto"});
        }


    }

    
}}