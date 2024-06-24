using assignment_six.Exceptions;
using assignment_six.Model;
using assignment_six.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;

namespace assignment_six.Controllers;

[Route("api/[controller]")]
[ApiController]
public class WarehouseController : ControllerBase
{
    private IWarehouseService _warehouseService;
    
    public WarehouseController(IWarehouseService warehouseService)
    {
        _warehouseService = warehouseService;
    }
    
    /// <summary>
    /// Endpoint used to add a product in the Warehouse.
    /// </summary>
    /// <param name="insertProduct">New Product data</param>
    /// <returns>201 Created</returns>
    [HttpPost]
    [Route("products")]
    public async Task<IActionResult> InsertProductInWarehouse(InsertProductRequest insertProduct)
    {
        try
        {
            var insertedProductId = await _warehouseService.InsertProductInWarehouse(insertProduct);
            return Created("/api/warehouses/products/id/", new
            {
                Id = insertedProductId
            });
        }
        catch (BadRequestException ex)
        {
            return BadRequest(ex.Message);
        }
        catch (NotFoundException ex)
        {
            return NotFound(ex.Message);
        }
        
        catch (Exception ex) when (ex is InvalidOperationException or SqlException)
        {
            return StatusCode(StatusCodes.Status500InternalServerError);
        }
    }
}