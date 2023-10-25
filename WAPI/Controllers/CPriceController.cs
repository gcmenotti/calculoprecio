using DA;
using Microsoft.AspNetCore.Mvc;
using MODEL;

namespace WebApplication1.Controllers;

public class CPriceController : ControllerBase
{
    private readonly IPCRepo _repository;
    public CPriceController(IPCRepo repository)
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

    [HttpGet("GetProductInfo")]
    public async Task<List<ProductInfo>> GetProductInfo(string tablaCompra, string tipoCompra, string folioCompra)
    {
        var pCalc = new List<ProductInfo>();

        var productCodes = await _repository.GetCodigoproductoAsync(tablaCompra, tipoCompra, folioCompra);

        foreach (var cod in productCodes)
        {
            var productData = await _repository.GetProductDataAsync(tablaCompra, tipoCompra, folioCompra, cod);


            var sumPrecioFarmacia = productData.Average(data => data.PrecioFarmacia);

            var productInfo = new ProductInfo
            {

                PrecioFarmaciaNuevo = sumPrecioFarmacia,
            };

            pCalc.Add(productInfo);
        }


        return pCalc;

    }





}