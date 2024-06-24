namespace assignment_six.Exceptions;

public abstract class NotFoundException(string message) : Exception(message);

public class ProductNotFound(int id) : NotFoundException($"Product with id {id} not found");

public class WarehouseNotFound(int id) : NotFoundException($"Warehouse with id {id} not found");

public class OrderWithProductIdNotFound(int id) : NotFoundException($"No Order matching Product id {id} and/or amount found");

public class OrderNotFound(int id) : NotFoundException($"Order with id {id} not found in warehouse.");
