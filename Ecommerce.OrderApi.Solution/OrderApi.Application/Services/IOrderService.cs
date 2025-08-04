using OrderApi.Application.DTOs;
using OrderApi.Domain.Entites;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderApi.Application.Services
{
    public interface IOrderService
    {
        Task<IEnumerable<OrderDTO>> GetOrderByClientId(int ClientId);
        Task<OrderDetailsDTO> GetOrderDetail(int orderid);

    }
}
