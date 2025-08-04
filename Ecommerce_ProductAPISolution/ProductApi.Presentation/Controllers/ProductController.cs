using ecommrece.sharedliberary.Logs;
using ecommrece.sharedliberary.Responses;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ProductApi.Application.DTOs;
using ProductApi.Application.DTOs.Conversion;
using ProductApi.Application.Interfaces;
using ProductApi.Infrastructure.Data;

namespace ProductApi.Presentation.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [AllowAnonymous]
    public class ProductController(IProduct productInterface) : ControllerBase
    {
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ProductDTO>>> GetProducts()
        {
            var products = await productInterface.GetAllAsync();
            if (!products.Any())
            {
                return NotFound("NO Product detecated in database ");
            }
            var (_, List) = ProductConversion.FromEntity(null, products);
            return List is not null ? Ok(List) : NotFound("No Product is Found");

        }
        [HttpPost]
        [Authorize(Roles ="Admin")]
        public async Task<ActionResult<Response>> CreateProduct(ProductDTO productDTO)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                var product = ProductConversion.ToEntity(productDTO);
                var response = await productInterface.CreateAsync(product);
                return response.flag is true ? Ok(response) : BadRequest(response.message);
            }
            catch (Exception ex)
            {
               LogException.LogExceptions(ex);
                return new Response(false, $"Error Ocurred When Created");
            }
        }

        [HttpPut]
        [Authorize(Roles = "Admin")]

        public async Task<ActionResult<Response>> UpdateProduct(ProductDTO productDTO)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var product = ProductConversion.ToEntity(productDTO);
            var response = await productInterface.UpdateAsync(product);
            return response.flag is true ? Ok(response) : BadRequest(response.message);


        }
        [HttpDelete]
        [Authorize(Roles = "Admin")]

        public async Task<ActionResult<Response>> DeleteProduct(ProductDTO productDTO)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var product = ProductConversion.ToEntity(productDTO);
            var response = await productInterface.DeleteAsync(product);
            return response.flag is true ? Ok(response) : BadRequest(response.message);

        }
    }
}