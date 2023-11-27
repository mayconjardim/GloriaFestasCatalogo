﻿using GloriaFestasCatalogo.Server.Services.ProductService;
using GloriaFestasCatalogo.Shared.Dtos.Products;
using GloriaFestasCatalogo.Shared.Utils;
using Microsoft.AspNetCore.Mvc;

namespace GloriaFestasCatalogo.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {

        private readonly IProductService _productService;

        public ProductController(IProductService productService)
        {
            _productService = productService;
        }

        [HttpGet("all")]
        public async Task<ActionResult<ServiceResponse<List<ProductDto>>>> GetProducts()
        {
            var result = await _productService.GetProductsAsync();
            return Ok(result);
        }

        [HttpGet("page/{page}")]
        public async Task<ActionResult<ServiceResponse<ProductResponse>>> GetProductsPageable(int page, int pageSize = 20)
        {
            var result = await _productService.GetProductsPageableAsync(page, pageSize);

            if (result == null || result.Data == null || result.Data.Products == null || result.Data.Products.Count == 0)
            {
                return NotFound("Desculpe, produto não encontrado.");
            }

            return Ok(result);
        }

        [HttpGet("{productId}")]
        public async Task<ActionResult<ServiceResponse<ProductDto>>> GetProduct(int productId)
        {
            var result = await _productService.GetProductAsync(productId);
            return Ok(result);
        }

        [HttpGet("category/{categoryId}")]
        public async Task<ActionResult<ServiceResponse<List<ProductDto>>>> GetProductsByCategory(int categoryId)
        {
            var result = await _productService.GetProductsByCategory(categoryId);
            return Ok(result);
        }

    }
}