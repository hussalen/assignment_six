using assignment_six.Model;

namespace assignment_six.Services;

public interface IWarehouseService
{
    Task<int> InsertProductInWarehouse(InsertProductRequest insertProductReq);
}