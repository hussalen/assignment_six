using assignment_six.Model;

namespace assignment_six.Repositories;

public interface IWarehouseRepo
{
    Task<int> InsertProductInWarehouse(InsertProductRequest insertProductReq, int idOrder,
        int productPrice);
    Task<bool> IdProductExists(InsertProductRequest insertProductReq);
    Task<bool> IdWarehouseExists(InsertProductRequest insertProductReq);
    Task<bool> ProductIsPurchased(InsertProductRequest insertProductReq);
    Task<bool> CreatedAtRequestIsTooNew(InsertProductRequest insertProductReq);
    Task<int> FulfilledOrderId(InsertProductRequest insertProductReq);
    Task<bool> RequestedOrderIdExists(int idOrder);
    public Task<int> GetProductPrice(InsertProductRequest insertProductReq);
    Task UpdateFulfilledAtOnOrder(int idOrder, InsertProductRequest insertProductReq);

   
}