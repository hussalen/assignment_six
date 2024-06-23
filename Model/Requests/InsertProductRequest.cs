using System.ComponentModel.DataAnnotations;
namespace assignment_six.Model;

public class InsertProductRequest
{
    [Required]
    public int IdProduct { get; set; }
    [Required]
    public int IdWarehouse { get; set; }
    
    [Required]
    [Range(1, Int32.MaxValue, ErrorMessage = "The field {0} must be greater than 0.")]
    public int Amount { get; set; }
    [Required]
    public DateTime CreatedAt { get; set; }
}