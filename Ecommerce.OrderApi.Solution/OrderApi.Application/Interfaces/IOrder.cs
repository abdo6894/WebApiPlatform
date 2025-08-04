using ecommrece.sharedliberary.Interface;
using OrderApi.Domain.Entites;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace OrderApi.Application.Interfaces
{
    public   interface IOrder : IgenericInterface<Order>
    {
        Task<IEnumerable<Order>> GetOrderAsync(Expression<Func<Order, bool>> predicte);

    }
}
