using ProductApi.Domain.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductApi.Application.DTOs.Conversion
{
    public static class ProductConversion
    {
        public static Product ToEntity(ProductDTO productDTO) => new()
        {
            Id = productDTO.Id,
            Name = productDTO.Name,
            Price = productDTO.Price,
            Quntity = productDTO.Quntity
        };
        public static (ProductDTO?,IEnumerable<ProductDTO>?) FromEntity (Product? product, IEnumerable<Product>? products)
        {
            if (product is null && products is null) return (null, null);

            if (product is not null && products is null)
                return (new ProductDTO(product.Id, product.Name!, product.Quntity, product.Price), null);

            if (product is null && products is not null)
                return (null, products.Select(p => new ProductDTO(p.Id, p.Name!, p.Quntity, p.Price)));

            return (new ProductDTO(product.Id, product.Name!, product.Quntity, product.Price),
                products.Select(p => new ProductDTO(p.Id, p.Name!, p.Quntity, p.Price)));
        }


    }
}
