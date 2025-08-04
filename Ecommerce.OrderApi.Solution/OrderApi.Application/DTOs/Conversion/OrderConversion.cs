
using OrderApi.Domain.Entites;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderApi.Application.DTOs.Conversion
{
    public static class OrderConversion
    {
        public static Order ToEntity(OrderDTO  orderDTO) => new()
        {
            Id = orderDTO.Id,
            ProductId = orderDTO.ProductId,
            ClientId = orderDTO.ClientId,
            PurchaseQuntity = orderDTO.PurchaseQuntity,
            OrderDate = orderDTO.OrderDate
        };
        


        public static (OrderDTO?,IEnumerable<OrderDTO>?) FromEntity (Order? order, IEnumerable<Order>? orders)
        {
           if(order is null &&  orders is not null)
            {
                return (null, orders.Select(p => new OrderDTO(p.Id, p.ProductId, p.ClientId,p.PurchaseQuntity, p.OrderDate)));
            }
           if( order is not null && orders is null)
            {
                return (new OrderDTO(order.Id, order.ProductId, order.ClientId, order.PurchaseQuntity, order.OrderDate), null);
            }
           if(order is null && orders is null)
            {
                 return (null, null);
            }
              return (new OrderDTO(order!.Id, order.ProductId, order.ClientId, order.PurchaseQuntity, order.OrderDate),
                orders!.Select(p => new OrderDTO(p.Id, p.ProductId, p.ClientId, p.PurchaseQuntity, p.OrderDate)));

        }


    }
}
