using ecommrece.sharedliberary.Logs;
using ecommrece.sharedliberary.Responses;
using Microsoft.EntityFrameworkCore;
using OrderApi.Application.Interfaces;
using OrderApi.Domain.Entites;
using OrderApi.Infrastructure.Data;
using System.Linq.Expressions;

namespace OrderApi.Infrastructure.Repository
{
    public class OrderRepository(OrderDBContext context) : IOrder
    {
        public async Task<Response> CreateAsync(Order entity)
        {
            try
            {
                var created = context.Orders.Add(entity).Entity;
                await context.SaveChangesAsync();

                return created.Id > 0 ? new Response(true, "Order created successfully") :
                        new Response(false, "Failed to create order");
            }
            catch (Exception ex)
            {

                LogException.LogExceptions(ex);
                return new Response(false, $"Error creating order: {ex.Message}");
            }
        }
        public async Task<Response> DeleteAsync(Order entity)
        {
            try
            {
                var deleted = await FindByIdAsync(entity.Id);
                if (deleted == null)
                {
                    return new Response(false, "Order not found");
                }
                await context.SaveChangesAsync();
                return new Response(true, "Order deleted successfully");

            }
            catch (Exception ex)
            {
                LogException.LogExceptions(ex);
                return new Response(false, $"Error deleting order: {ex.Message}");
            }

        }

        public async Task<Order> FindByIdAsync(int id)
        {
            try
            {
                var order = await context.Orders!.FindAsync(id);
                if (order == null)
                {
                    LogException.LogToConsole($"Order with ID {id} not found.");
                    return null!;
                }
                return order;
            }
            catch (Exception ex)
            {
                LogException.LogExceptions(ex);
                return null!;
            }
        }
        public async Task<IEnumerable<Order>> GetAllAsync()
        {
            try
            {
                var getorder = await context.Orders.AsNoTracking().ToListAsync();
                return getorder is not null ? getorder : null!;
            }
            catch (Exception ex)
            {
                LogException.LogExceptions(ex);
                throw new Exception("Error during get all order");
            }
        }

        public async Task<Order> GetByAsync(Expression<Func<Order, bool>> predicte)
        {
            try
            {
                var getorder = await context.Orders!.Where(predicte).FirstOrDefaultAsync();
                return getorder is not null ? getorder : null!;
            }
            catch (Exception ex)
            {
                LogException.LogExceptions(ex);
                throw new Exception("Error during get order by predicate");
            }

        }

        public async Task<IEnumerable<Order>> GetOrderAsync(Expression<Func<Order, bool>> predicte)
        {
            try
            {
                var getorder = await context.Orders!.Where(predicte).ToListAsync();
                return getorder is not null ? getorder : null!;
            }
            catch (Exception ex)
            {
                LogException.LogExceptions(ex);
                throw new Exception("Error during get order by predicate");
            }

        }

        public async Task<Response> UpdateAsync(Order entity)
        {
            try
            {
                var getproduct = await FindByIdAsync(entity.Id);
                if (getproduct == null)
                {
                    return new Response(false, "Order not found");
                }
                context.Entry(getproduct).State = EntityState.Detached;
                context.Orders.Update(entity);
                await context.SaveChangesAsync();
                return new Response(true, "Order updated successfully");
            }
            catch (Exception ex)
            {
                LogException.LogExceptions(ex);
                return new Response(false, $"Error updating order: {ex.Message}");
            }
        }
    }
}
