using AuthServer.Core.DTOs;
using AuthServer.Core.Entities;
using AuthServer.Core.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace AuthServer.API.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : CustomBaseController
    {
        private readonly IGenericService<Product,ProductDto> _productService;

        public ProductController(IGenericService<Product, ProductDto> productService)
        {
            _productService = productService;
        }
        [HttpGet]
        public async Task<IActionResult> GetListAll()
        {
            var result = await _productService.TGetListAllAsync();
            return ActionResultInstance(result);
        }
        [HttpGet("GetProductById/{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var result = await _productService.TGetByIdAsync(id);
            return ActionResultInstance(result);
        }
        [HttpPost]
        public async Task<IActionResult> CreateProduct(ProductDto p)
        {
            var result = await _productService.TCreateAsync(p);
            return ActionResultInstance(result);
        }
        [HttpPut]
        public async Task<IActionResult> UpdateProduct(ProductDto p)
        {
            var result = await _productService.TUpdateAsync(p, p.Id);
            return ActionResultInstance(result);
        }
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProduct(int id)
        {
            var result=await _productService.TDeleteAsync(id);
            return ActionResultInstance(result);
        }
    }
}
