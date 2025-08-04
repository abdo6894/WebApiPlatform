using Microsoft.EntityFrameworkCore;
using OrderApi.Domain.Entites;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderApi.Infrastructure.Data
{
    public class OrderDBContext(DbContextOptions<OrderDBContext> option):DbContext(option)
    {   

         public DbSet<Order> Orders { get; set; }
    }
}
