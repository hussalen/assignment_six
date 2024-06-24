using System.ComponentModel;
using assignment_six.Model;
using Microsoft.Data.SqlClient;

namespace assignment_six.Repositories;

public class WarehouseRepo : IWarehouseRepo
{
    private IConfiguration _configuration;

    public WarehouseRepo(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public async Task<bool> IdProductExists(InsertProductRequest insertProductReq)
    {
        await using (var con = new SqlConnection(_configuration["ConnectionStrings:DefaultConnection"]))
        {
            await using (var cmd = new SqlCommand())
            {
                cmd.Connection = con;
                await con.OpenAsync();

                cmd.CommandText = "SELECT COUNT(1) FROM Product WHERE IdProduct = @IdProduct ";
                cmd.Parameters.AddWithValue("@IdProduct", insertProductReq.IdProduct);
                var count = await cmd.ExecuteScalarAsync() ?? throw new InvalidOperationException();
                return (int)count > 0;
            }
        }
    }

    public async Task<bool> IdWarehouseExists(InsertProductRequest insertProductReq)
    {
        await using (var con = new SqlConnection(_configuration["ConnectionStrings:DefaultConnection"]))
        {
            await using (var cmd = new SqlCommand())
            {
                cmd.Connection = con;
                await con.OpenAsync();

                cmd.CommandText = "SELECT COUNT(1) FROM Warehouse WHERE IdWarehouse = @IdWarehouse ";
                cmd.Parameters.AddWithValue("@IdWarehouse", insertProductReq.IdWarehouse);

                var count = await cmd.ExecuteScalarAsync() ?? throw new InvalidOperationException();

                return (int)count > 0;
            }
        }
    }

    public async Task<bool> ProductIsPurchased(InsertProductRequest insertProductReq)
    {
        await using (var con = new SqlConnection(_configuration["ConnectionStrings:DefaultConnection"]))
        {
            await using (var cmd = new SqlCommand())
            {
                cmd.Connection = con;
                await con.OpenAsync();

                cmd.CommandText = "SELECT COUNT(1) FROM \"Order\" WHERE IdProduct = @IdProduct AND Amount = @Amount";
                cmd.Parameters.AddWithValue("@IdProduct", insertProductReq.IdProduct);
                cmd.Parameters.AddWithValue("@Amount", insertProductReq.Amount);

                var count = await cmd.ExecuteScalarAsync() ?? throw new InvalidOperationException();
                return (int)count > 0;
            }
        }
    }

    public async Task<bool> CreatedAtRequestIsTooNew(InsertProductRequest insertProductReq)
    {
        await using (var con = new SqlConnection(_configuration["ConnectionStrings:DefaultConnection"]))
        {
            await using (var cmd = new SqlCommand())
            {
                cmd.Connection = con;
                await con.OpenAsync();

                cmd.CommandText = "SELECT COUNT(1) FROM \"Order\" WHERE CreatedAt < @CreatedAt";
                cmd.Parameters.AddWithValue("@CreatedAt", insertProductReq.CreatedAt);

                var count = await cmd.ExecuteScalarAsync() ?? throw new InvalidOperationException();
                return (int)count > 0;
            }
        }
    }

    public async Task<int> FulfilledOrderId(InsertProductRequest insertProductReq)
    {
        await using (var con = new SqlConnection(_configuration["ConnectionStrings:DefaultConnection"]))
        {
            await using (var cmd = new SqlCommand())
            {
                cmd.Connection = con;
                await con.OpenAsync();

                cmd.CommandText =
                    "SELECT IdOrder FROM \"Order\" WHERE IdProduct = @IdProduct AND Amount = @Amount AND FulfilledAt IS NOT NULL";
                cmd.Parameters.AddWithValue("@IdProduct", insertProductReq.IdProduct);
                cmd.Parameters.AddWithValue("@Amount", insertProductReq.Amount);

                var orderId = await cmd.ExecuteScalarAsync() ?? 0;

                return (int)orderId;
            }
        }
    }

    public async Task<bool> RequestedOrderIdExists(int idOrder)
    {
        await using (var con = new SqlConnection(_configuration["ConnectionStrings:DefaultConnection"]))
        {
            await using (var cmd = new SqlCommand())
            {
                cmd.Connection = con;
                await con.OpenAsync();

                cmd.CommandText = "SELECT COUNT(1) FROM Product_Warehouse WHERE IdOrder = @IdOrder";
                cmd.Parameters.AddWithValue("@IdOrder", idOrder);

                var orderId = await cmd.ExecuteScalarAsync() ?? throw new InvalidOperationException();

                return (int)orderId > 0;
            }
        }
    }

    public async Task UpdateFulfilledAtOnOrder(int idOrder, InsertProductRequest insertProductReq)
    {
        await using (var con = new SqlConnection(_configuration["ConnectionStrings:DefaultConnection"]))
        {
            await using (var cmd = new SqlCommand())
            {
                cmd.Connection = con;
                await con.OpenAsync();

                cmd.CommandText = "UPDATE Order SET FulfilledAt = @FulfilledAt WHERE IdOrder = @IdOrder";
                cmd.Parameters.AddWithValue("@FulfilledAt", insertProductReq.CreatedAt);
                cmd.Parameters.AddWithValue("@IdOrder", idOrder);

                await cmd.ExecuteNonQueryAsync();
            }
        }
    }

    public async Task<int> GetProductPrice(InsertProductRequest insertProductReq)
    {
        await using (var con = new SqlConnection(_configuration["ConnectionStrings:DefaultConnection"]))
        {
            await using (var cmd = new SqlCommand())
            {
                cmd.Connection = con;
                await con.OpenAsync();

                cmd.CommandText = "SELECT Price FROM Product WHERE IdProduct = @IdProduct";
                cmd.Parameters.AddWithValue("@IdProduct", insertProductReq.IdProduct);

                var productPrice = await cmd.ExecuteScalarAsync() ?? throw new InvalidOperationException();
                return (int)productPrice;
            }
        }
    }

    public async Task<int> InsertProductInWarehouse(InsertProductRequest insertProductReq, int idOrder,
        int productPrice)
    {
        await using (var con = new SqlConnection(_configuration["ConnectionStrings:DefaultConnection"]))
        {
            await using (var cmd = new SqlCommand())
            {
                cmd.Connection = con;
                await con.OpenAsync();
                await con.OpenAsync();
                cmd.CommandText =
                    "INSERT INTO Product_Warehouse(IdWarehouse, IdProduct, IdOrder, Amount, Price, CreatedAt) OUTPUT INSERTED.IdProductWarehouse " +
                    "VALUES(@IdWarehouse, @IdProduct, @IdOrder, @Amount, @Price, @CreatedAt)";

                cmd.Parameters.AddWithValue("@IdWarehouse", insertProductReq.IdWarehouse);
                cmd.Parameters.AddWithValue("@IdProduct", insertProductReq.IdProduct);
                cmd.Parameters.AddWithValue("@IdOrder", idOrder);
                cmd.Parameters.AddWithValue("@Amount", insertProductReq.Amount);
                cmd.Parameters.AddWithValue("@Price", insertProductReq.Amount * productPrice);
                cmd.Parameters.AddWithValue("@CreatedAt", insertProductReq.CreatedAt);

                var newProductInWarehouseId = await cmd.ExecuteScalarAsync() ?? throw new InvalidOperationException();

                return (int)newProductInWarehouseId;
            }
        }
    }
}