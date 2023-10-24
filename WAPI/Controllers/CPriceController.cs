using DA;
using Microsoft.AspNetCore.Mvc;
using MODEL;

namespace WebApplication1.Controllers;

public class CPriceController : ControllerBase
{
    private readonly IRepository _repository;

    public CPriceController(IRepository repository)
    {
        _repository = repository;
    }

    
    [HttpGet("getProductData")]
    public async Task<ActionResult<List<SQuery>>> GetProductDataAsync(string tablaCompra, string tipoCompra, string folioCompra, int codigoProducto)
    {
        try
        {
            var result = await _repository.GetProductDataAsync(tablaCompra, tipoCompra, folioCompra, codigoProducto);
            return Ok(result);
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Internal server error: {ex.Message}");
        }
    }

    [HttpGet("GetCodigoProductos")]
    public async Task<ActionResult<List<int>>> GetCodigoProductos(string tablaCompra, string tipoCompra,
        string folioCompra)
    {
        try
        {
            var result = await _repository.GetCodigoproductoAsync(tablaCompra, tipoCompra, folioCompra);
            return Ok(result);
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Internal server error: {ex.Message}");
        }
    }
    
    
}