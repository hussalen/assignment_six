using assignment_six.Exceptions;
using assignment_six.Model;
using assignment_six.Repositories;

namespace assignment_six.Services;

public class WarehouseService : IWarehouseService
{
    private readonly IWarehouseRepo _warehouseRepo;

    public WarehouseService(IWarehouseRepo warehouseRepo)
    {
        _warehouseRepo = warehouseRepo;
    }

    public async Task<int> InsertProductInWarehouse(InsertProductRequest insertProductReq)
    {
        if (!await _warehouseRepo.IdProductExists(insertProductReq))
        {
            throw new ProductNotFound(insertProductReq.IdProduct);
        }

        if (!await _warehouseRepo.IdWarehouseExists(insertProductReq))
        {
            throw new WarehouseNotFound(insertProductReq.IdWarehouse);
        }

        if (!await _warehouseRepo.ProductIsPurchased(insertProductReq))
        {
            throw new OrderWithProductIdNotFound(insertProductReq.IdProduct);
        }
        
        if (await _warehouseRepo.CreatedAtRequestIsTooNew(insertProductReq))
        {
            throw new RequestedOrderDateTooNew(insertProductReq.CreatedAt);
        }

        int orderId = await _warehouseRepo.FulfilledOrderId(insertProductReq);
        if (orderId < 1)
        {
            throw new OrderIsNotFulfilled(orderId);
        }

        if (!await _warehouseRepo.RequestedOrderIdExists(orderId))
        {
            throw new OrderNotFound(orderId);
        }

        await _warehouseRepo.UpdateFulfilledAtOnOrder(orderId, insertProductReq);

        int productPrice = await _warehouseRepo.GetProductPrice(insertProductReq);

        return await _warehouseRepo.InsertProductInWarehouse(insertProductReq, orderId, productPrice);
    }
}