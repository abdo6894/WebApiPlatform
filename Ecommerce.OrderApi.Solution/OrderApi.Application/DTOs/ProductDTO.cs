using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderApi.Application.DTOs 
{
    public record ProductDTO
        (
        int Id,
        [Required(ErrorMessage = "Name is required.")]
        string Name,
        [Required ,Range(1,int.MaxValue)] int Quntity,
        [Required ,DataType(DataType.Currency)] Decimal Price

        );

}
