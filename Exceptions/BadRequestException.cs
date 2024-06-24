namespace assignment_six.Exceptions;

public abstract class BadRequestException(string message) : Exception(message);

public class RequestedOrderDateTooNew(DateTime date) : BadRequestException($"The order date should be older than the CreatedAt date provided: {date}.");

public class OrderIsNotFulfilled(int id) : BadRequestException($"Order with id {id} is not fulfilled");
