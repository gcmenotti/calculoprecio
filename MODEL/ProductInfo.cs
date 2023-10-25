namespace MODEL;

public class ProductInfo
{
    public string CodigoProducto { get; set; }
    public string CodigoRelacionado { get; set; }
    public string Descripcion { get; set; }
    public double PrecioVentaActual { get; set; }
    public double PrecioFarmaciaNuevo { get; set; }
    public double PrecioVentaNuevo { get; set; }
    public double Margen { get; set; }
    public int CantidadPastillero { get; set; }
    public double PrecioVentaCajaActual { get; set; }
    public double CostoCaja { get; set; }
    public double PrecioVentaCajaNuevo { get; set; }
}