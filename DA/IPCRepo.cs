using MODEL;

namespace DA;

public interface IPCRepo
{
    Task<List<SQuery>> GetProductDataAsync(string tablaCompra, string tipoCompra, string folioCompra, int codigoProducto);
    Task<List<int>> GetCodigoproductoAsync(string tablaCompra, string tipoCompra, string folioCompra);
}