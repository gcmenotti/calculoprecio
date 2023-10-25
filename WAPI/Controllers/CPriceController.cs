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
           
            var sumMargen = productData.Average(data => data.Margen);
            var sumPrecioFarmacia = productData.Average(data => data.PrecioFarmacia);
            var sumExistencia = productData.Sum(data => data.Existencia);
            var sumPrecioActual = productData.Average(data => data.PrecioActual);
            var PorcentajeFinal = 0M;
            var sumPorcentajeFinal = 0M;

            foreach (var plist in productData)
            {
                PorcentajeFinal = (Convert.ToDecimal(plist.Existencia) * plist.PrecioFarmacia) / sumExistencia;
                sumPorcentajeFinal += PorcentajeFinal;
            }
            
            var Adjustment = 0M;
            var PrecioFinal = 0M;
            var PrecioVenta = 0M;
            
            
            foreach (var pro in productData)
            {
                 Adjustment = sumPorcentajeFinal * 100M / pro.PrecioFarmaciaCat - 100M;
                 PrecioFinal = !(Adjustment > 0.6M) ? pro.PrecioFarmaciaCat : sumPorcentajeFinal;
                 PrecioVenta = PrecioFinal / (1M - Convert.ToDecimal(pro.Margen) / 100M);
            }
            
            var productInfo = new ProductInfo
            {
                CodigoProducto = productData[0].CodigoProducto,
                CodigoRelacionado = productData[0].CodigoRelacionado,
                Descripcion = productData[0].Descripcion,
                PrecioVentaActual = sumPrecioActual, 
                PrecioFarmaciaNuevo = PrecioFinal,
                PrecioVentaNuevo = PrecioVenta,
                Margen = sumMargen,
                CantidadPastillero = productData[0].CantidadPastillero,
                PrecioVentaCajaActual = sumPrecioActual * productData[0].CantidadPastillero,
                CostoCaja = PrecioFinal * productData[0].CantidadPastillero,
                PrecioVentaCajaNuevo = PrecioVenta * productData[0].CantidadPastillero,
                
                
            };

            pCalc.Add(productInfo);
        }


        return pCalc;

    }





}