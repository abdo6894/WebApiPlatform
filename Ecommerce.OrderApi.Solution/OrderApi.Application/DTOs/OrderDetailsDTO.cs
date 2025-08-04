
using System.ComponentModel.DataAnnotations;

namespace OrderApi.Application.DTOs
{
    public record OrderDetailsDTO(
        [Required] int OrderId,
        [Required] int ClientId,
        [Required] int ProductId,
        [Required] string TelephoneNumber,
        [Required] string Address,
        [Required] string ProductName,
        [Required, EmailAddress] string Email,
        [Required, Range(1, int.MaxValue)] int PurchaseQuntity,
        [Required,DataType(DataType.Currency)] decimal UnitPrice,
        [Required, DataType(DataType.Currency)] decimal TotalPrice,
        [Required] DateTime OrderDate



        );

}
