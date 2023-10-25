using Dapper;
using MODEL;

namespace DA;

public class PCRepo : IPCRepo
{
    private readonly SqlCnnFLee _cnnFLee;
    public PCRepo(SqlCnnFLee cnnFLee)
    {
        _cnnFLee = cnnFLee;
    }
 
    public async Task<List<SQuery>> GetProductDataAsync(string tablaCompra, string tipoCompra, string folioCompra, int codigoProducto)
    {
        var query = $@"
            SELECT gpc.codigoProducto,  
                   gpc.descripcion,  
                   gpc.precioFarmacia              [precioFarmaciaCat],  
                   ice.codigoSucursal,  
                   convert(int, ice.existencia) as existencia,  
                   ice.precioFarmacia,  
                   ccp.margen,  
                   gpcr.codigoRelacionado,  
                   0                            AS cantidadPastillero,  
                   gpc.precioPublico               [PrecioActual]  
            FROM dbo.genProductosCat AS gpc  
                     INNER JOIN {tablaCompra} AS cdd ON gpc.codigoProducto = cdd.codigoProducto  
                     INNER JOIN dbo.invControlExistenciasReg AS ice ON gpc.codigoProducto = ice.codigoProducto  
                     INNER JOIN dbo.genProductosCodigosRelacionadosCat AS gpcr ON gpc.codigoProducto = gpcr.codigoProducto  
                     INNER JOIN dbo.cmrCambioPreciosCnf AS ccp  
                                ON gpc.codigoFamiliaUno = ccp.codigoFamilia1 AND gpc.codigoFamiliaDos = ccp.codigoFamilia2 AND  
                                   gpc.codigoFamiliaTres = ccp.codigoFamilia3 AND gpc.codigoFamiliaCuatro = ccp.codigoFamilia4  
            WHERE (cdd.folioCompra = '{folioCompra}')  
              AND (ice.existencia > 0)  
              AND gpc.codigoProducto = {codigoProducto} 
              AND (gpcr.pedir = 1)  
            UNION ALL  
            SELECT gpc.codigoProducto,  
                   gpc.descripcion,  
                   gpc.precioFarmacia              [precioFarmaciaCat],  
                   ice.codigoSucursal,  
                   convert(int, ice.existencia) as existencia,  
                   ice.precioFarmacia,  
                   ccp.margen,  
                   gpcr.codigoRelacionado,  
                   CAST(gpasc.cantidad as int)  as cantidadPastillero,  
                   gpc.precioPublico               [PrecioActual]  
            FROM dbo.genProductosCat AS gpc  
                     INNER JOIN dbo.cmpComprasPastillerosReg AS cdpr ON gpc.codigoProducto = cdpr.codigoProducto  
                     INNER JOIN dbo.genPastillerosCat AS gpasc ON gpc.codigoProducto = gpasc.codigoPastillero  
                     INNER JOIN dbo.invControlExistenciasReg AS ice ON gpc.codigoProducto = ice.codigoProducto  
                     INNER JOIN dbo.genProductosCodigosRelacionadosCat AS gpcr ON gpc.codigoProducto = gpcr.codigoProducto  
                     INNER JOIN dbo.cmrCambioPreciosCnf AS ccp  
                                ON gpc.codigoFamiliaUno = ccp.codigoFamilia1 AND gpc.codigoFamiliaDos = ccp.codigoFamilia2 AND  
                                   gpc.codigoFamiliaTres = ccp.codigoFamilia3 AND gpc.codigoFamiliaCuatro = ccp.codigoFamilia4  
            WHERE cdpr.tipoCompra = '{tipoCompra}'  
              AND cdpr.folioCompra = '{folioCompra}'  
              AND gpc.codigoProducto = {codigoProducto}
              AND (gpcr.pedir = 1)  
            ORDER BY gpc.codigoProducto, ice.codigoSucursal";
        try
        {
            await using var con = _cnnFLee.GetConnection();
            con.Open();
            var result = await con.QueryAsync<SQuery>(query);

            con.Close();
            return result.ToList();
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }

    public Task<List<int>> GetCodigoproductoAsync(string tablaCompra, string tipoCompra, string folioCompra)
    {
        var query = $@"
            SELECT gpc.codigoProducto  
            FROM dbo.genProductosCat AS gpc  
                     INNER JOIN {tablaCompra} AS cdd ON gpc.codigoProducto = cdd.codigoProducto  
                     INNER JOIN dbo.invControlExistenciasReg AS ice ON gpc.codigoProducto = ice.codigoProducto  
                     INNER JOIN dbo.genProductosCodigosRelacionadosCat AS gpcr ON gpc.codigoProducto = gpcr.codigoProducto  
                     INNER JOIN dbo.cmrCambioPreciosCnf AS ccp  
                                ON gpc.codigoFamiliaUno = ccp.codigoFamilia1 AND gpc.codigoFamiliaDos = ccp.codigoFamilia2 AND  
                                   gpc.codigoFamiliaTres = ccp.codigoFamilia3 AND gpc.codigoFamiliaCuatro = ccp.codigoFamilia4  
            WHERE (cdd.folioCompra = '{folioCompra}')  
              AND (ice.existencia > 0)  
              AND (gpcr.pedir = 1)  
            group by gpc.codigoProducto  
                        UNION ALL  
                        SELECT gpc.codigoProducto  
                        FROM dbo.genProductosCat AS gpc  
                                 INNER JOIN dbo.cmpComprasPastillerosReg AS cdpr ON gpc.codigoProducto = cdpr.codigoProducto  
                                 INNER JOIN dbo.invControlExistenciasReg AS ice ON gpc.codigoProducto = ice.codigoProducto  
                                 INNER JOIN dbo.genProductosCodigosRelacionadosCat AS gpcr ON gpc.codigoProducto = gpcr.codigoProducto  
                                 INNER JOIN dbo.cmrCambioPreciosCnf AS ccp  
                                            ON gpc.codigoFamiliaUno = ccp.codigoFamilia1 AND gpc.codigoFamiliaDos = ccp.codigoFamilia2 AND  
                                               gpc.codigoFamiliaTres = ccp.codigoFamilia3 AND gpc.codigoFamiliaCuatro = ccp.codigoFamilia4  
                        WHERE cdpr.tipoCompra = '{tipoCompra}'  
                          AND cdpr.folioCompra = '{folioCompra}'  
                          AND (gpcr.pedir = 1)  
                        group by gpc.codigoProducto  
                        ORDER BY gpc.codigoProducto";
        try
        {
            using var con = _cnnFLee.GetConnection();
            con.Open();
            var result = con.Query<int>(query);
            con.Close();
            return Task.FromResult(result.ToList());
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }
}