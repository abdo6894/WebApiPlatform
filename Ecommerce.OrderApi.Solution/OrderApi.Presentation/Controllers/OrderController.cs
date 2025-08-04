using Azure;
using ecommrece.sharedliberary.Logs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using OrderApi.Application.DTOs;
using OrderApi.Application.DTOs.Conversion;
using OrderApi.Application.Interfaces;
using OrderApi.Application.Services;

namespace OrderApi.Presentation.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class OrderController(IOrder orderinterface, IOrderService orderservice) : ControllerBase
    {
        [HttpGet]
        public async Task<ActionResult<IEnumerable<OrderDTO>>> GetallOrders()
        {
            var orders = await orderinterface.GetAllAsync();
            if (!orders.Any())
                return NotFound("No orders found.");
            var (_, _list) = OrderConversion.FromEntity(null, orders);
            return !_list!.Any() ? NotFound() : Ok(_list);
        }
        [HttpGet("{id}")]
        public async Task<ActionResult<OrderDTO>> GetOrderById(int id)
        {
            var order = await orderinterface.FindByIdAsync(id);
            if (order is null)
                return NotFound("No order found.");
            var (_order, _) = OrderConversion.FromEntity(order, null);
            return _order is null ? NotFound() : Ok(_order);
        }
        [HttpPost]
        public async Task<ActionResult<Response>> Createorder(OrderDTO orderdto)
        {

            if (!ModelState.IsValid)
                BadRequest("IN complete data submitted");

            var convert = OrderConversion.ToEntity(orderdto);
            var response = await orderinterface.CreateAsync(convert);
            return response.flag ? Ok(response) : BadRequest(response);

        }
        [HttpPut()]
        public async Task<ActionResult<OrderDTO>> Updateorder(OrderDTO orderdto)
        {

            var convert = OrderConversion.ToEntity(orderdto);
            var response = await orderinterface.UpdateAsync(convert);
            return response.flag ? Ok(response) : BadRequest(response);

        }
        [HttpDelete]
        public async Task<ActionResult<OrderDTO>> Deleteorder(OrderDTO orderdto)
        {

            var convert = OrderConversion.ToEntity(orderdto);
            var response = await orderinterface.DeleteAsync(convert);
            return response.flag ? Ok(response) : BadRequest(response);

        }
        [HttpGet("ClientOrders/{clientId}")]
        public async Task<ActionResult<IEnumerable<OrderDTO>>> GetOrdersByClientId(int clientId)
        {
            if (clientId <= 0)
                return BadRequest("Invalid client ID.");
            var orders = await orderservice.GetOrderByClientId(clientId);
            return !orders.Any() ? NotFound("No orders found for this client.") : Ok(orders);


        }
        [HttpGet("OrderDetail/{orderid}")]
        public async Task<ActionResult<OrderDetailsDTO>> GetOrderDetail(int orderid)
        {
            if (orderid <= 0)
                return BadRequest("Invalid order ID.");
            try
            {
                var orderdetail = await orderservice.GetOrderDetail(orderid);
                return Ok(orderdetail);
            }
            catch (Exception ex)
            {
                LogException.LogToConsole(ex.Message);
                return BadRequest($"Error retrieving order details: {ex.Message}");
            }
        }

    }
}
