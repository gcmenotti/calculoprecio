
SELECT gpc.codigoProducto  
FROM dbo.genProductosCat AS gpc  
         INNER JOIN " + this.lblTablaCompra.Text + " AS cdd ON gpc.codigoProducto = cdd.codigoProducto  
         INNER JOIN dbo.invControlExistenciasReg AS ice ON gpc.codigoProducto = ice.codigoProducto  
         INNER JOIN dbo.genProductosCodigosRelacionadosCat AS gpcr ON gpc.codigoProducto = gpcr.codigoProducto  
         INNER JOIN dbo.cmrCambioPreciosCnf AS ccp  
                    ON gpc.codigoFamiliaUno = ccp.codigoFamilia1 AND gpc.codigoFamiliaDos = ccp.codigoFamilia2 AND  
                       gpc.codigoFamiliaTres = ccp.codigoFamilia3 AND gpc.codigoFamiliaCuatro = ccp.codigoFamilia4  
WHERE (cdd.folioCompra = '" + this.txtCodigo.Text + "')  
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
WHERE cdpr.tipoCompra = '" + this.TipoCompra + "'  
  AND cdpr.folioCompra = '" + this.txtCodigo.Text + "'  
  AND (gpcr.pedir = 1)  
group by gpc.codigoProducto  
ORDER BY gpc.codigoProducto
