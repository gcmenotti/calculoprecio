namespace MODEL;

public class ProductInfo
{
    public string CodigoProducto { get; set; }
    public string CodigoRelacionado { get; set; }
    public string Descripcion { get; set; }
    public decimal PrecioVentaActual { get; set; }
    public decimal PrecioFarmaciaNuevo { get; set; }
    public decimal PrecioVentaNuevo { get; set; }
    public decimal Margen { get; set; }
    public int CantidadPastillero { get; set; }
    public decimal PrecioVentaCajaActual { get; set; }
    public decimal CostoCaja { get; set; }
    public decimal PrecioVentaCajaNuevo { get; set; }
    
}