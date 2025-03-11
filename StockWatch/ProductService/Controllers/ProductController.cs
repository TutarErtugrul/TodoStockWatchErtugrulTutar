using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.WebUtilities;
using ProductServiceAPI.DTOs;
using ProductServiceAPI.Models;
using ProductServiceAPI.Services;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

[Route("api/products")]
[ApiController]
public class ProductController : ControllerBase
{
    private readonly IProductService _productService;

    public ProductController(IProductService productService)
    {
        _productService = productService;
    }

    [HttpGet("AllProducts")]
    public async Task<IActionResult> GetAll()
    {
        var products = await _productService.GetAllProductsAsync();
        return Ok(products);
    }

    [HttpGet("OneProduct/{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        var product = await _productService.GetProductByIdAsync(id);
        if (product == null)
            return NotFound($"Ürün bulunamadı! ID: {id}");

        return Ok(product);
    }

    [HttpPost("CreateProduct")]
    public async Task<IActionResult> Create([FromBody] CreateProductDto productDto)
    {
        if (productDto == null)
            return BadRequest("Ürün bilgileri eksik!");

        var newProductId = await _productService.AddProductAsync(productDto);
        return CreatedAtAction(nameof(GetById), new { id = newProductId }, newProductId);
    }

    [HttpPut("UpdateProduct/{id}")]
    public async Task<IActionResult> Update(int id, [FromBody] UpdateProductDto productDto)
    {
        var result = await _productService.UpdateProductAsync(id, productDto);
        if (result == 0)
            return NotFound($"Güncellenecek ürün bulunamadı! ID: {id}");

        return Ok($"Ürün başarıyla güncellendi! ID: {id}");
    }

    [HttpPut("UpdateStock/{id}")]
    public async Task<IActionResult> UpdateStock(int id, [FromBody] UpdateStock updateStock)
    {
        var token = Request.Headers["Authorization"].ToString();
        token = token.Substring("Bearer ".Length);
        
        var handler = new JwtSecurityTokenHandler();
        var jsonToken = handler.ReadToken(token) as JwtSecurityToken;
        var userIdtmp = jsonToken?.Claims.First(claim => claim.Type == "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier")?.Value;
        int userId = int.Parse(userIdtmp);
        var result = await _productService.UpdateStockAsync(userId, updateStock);

        return Ok($"Ürün başarıyla güncellendi! ID: {id}");
    }

    [HttpDelete("Delete/{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var result = await _productService.DeleteProductAsync(id);
        if (result == 0)
            return NotFound($"Silinecek ürün bulunamadı! ID: {id}");

        return Ok($"Ürün başarıyla silindi! ID: {id}");
    }

    [HttpGet("GetSumReport")]
    public async Task<IActionResult> GetSumReport(DateTime start, DateTime end)
    {
        var products = await _productService.SumQuantitySoldAsync(start,end);
        return Ok(products);
    }

    [HttpGet("GetAvgReport")]
    public async Task<IActionResult> GetAvgReport(DateTime start, DateTime end)
    {
        var products = await _productService.StockTurnoverRateAsync(start, end);
        return Ok(products);
    }
}
