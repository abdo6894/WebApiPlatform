using OrderApi.Application.DTOs;
using OrderApi.Application.DTOs.Conversion;
using OrderApi.Application.Interfaces;
using OrderApi.Domain.Entites;
using Polly;
using Polly.Registry;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;

namespace OrderApi.Application.Services
{
    public class OrderServvice(HttpClient httpClient,
        ResiliencePipelineProvider<string> resiliencePipeline,IOrder orderinterface) : IOrderService
    {

        public async Task<ProductDTO> GetProduct(int productid)
        {
            var getproduct=await httpClient.GetAsync($"api/product/{productid}");
            if (getproduct.IsSuccessStatusCode)
            {
                var product = await getproduct.Content.ReadFromJsonAsync<ProductDTO>();
                return product;
            }
            else
            {
                throw new Exception("Product not found");
            }
        }
        public async Task<AppUserDTO> GetUser(int userid)
        {
            var getuser = await httpClient.GetAsync($"api/Authentication/{userid}");
            if (getuser.IsSuccessStatusCode)
            {
                var user = await getuser.Content.ReadFromJsonAsync<AppUserDTO>();
                return user;
            }
            else
            {
                throw new Exception("Product not found");
            }
        }

        public async Task<OrderDetailsDTO> GetOrderDetail(int orderid)
        {
            var order = await orderinterface.FindByIdAsync(orderid);
            if(order is null && order.Id<= 0)
            {
                throw new Exception("Order not found");
            }

            var retrypipline = resiliencePipeline.GetPipeline("my-retry-pipline");
            var Productdto= await retrypipline.ExecuteAsync(async token => await  GetProduct(order.ProductId));
            var AppUserdto= await retrypipline.ExecuteAsync(async token => await GetUser(order.ClientId));

           return new OrderDetailsDTO(
                order.Id,
                Productdto.Id,
                AppUserdto.Id,
                AppUserdto.Name,
                AppUserdto.Email,
                AppUserdto.Address,
                AppUserdto.TelephoneNumber,
                order.PurchaseQuntity,
                Productdto.Price,
                Productdto.Price * order.PurchaseQuntity,
                order.OrderDate
            );

        }

        public async Task<IEnumerable<OrderDTO>> GetOrderByClientId(int ClientId)
        {
            var orders = await orderinterface.GetOrderAsync(x => x.ClientId == ClientId);
            if (!orders.Any()) return null;
            var (_, _orders) = OrderConversion.FromEntity(null, orders);
            return _orders!;
        }
    }
}
