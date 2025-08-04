using ProductApi.Application.Interfaces;
using ProductApi.Domain.Entity;
using ProductApi.Infrastructure.Data;
using ecommrece.sharedliberary.Responses;
using System.Linq.Expressions;
using ecommrece.sharedliberary.Logs;
using Microsoft.EntityFrameworkCore;

namespace ProductApi.Infrastructure.Repositories
{
    public class ProductRepository(ProductDbContext context) : IProduct
    {
        public async Task<Response> CreateAsync(Product entity)
        {
            try
            {
                var Getproduct= await GetByAsync(_=> _.Name!.Equals(entity.Name));
                if (Getproduct is not null && !string.IsNullOrEmpty(Getproduct.Name))
                {
                    return new Response(false, "Product already exists");
                }
                var newProduct=  context.Products.Add(entity).Entity;
                await context.SaveChangesAsync();
                if(newProduct != null && newProduct.Id>0)
                {
                    return new Response(true, "Product created successfully");

                }
                else
                {
                     return new Response(false, $"Error Ocurred When Created{entity.Name}");
                }
            }
            catch(Exception ex)
            {
                LogException.LogExceptions(ex);
                return new Response(false, "An error occurred while creating the product");
            }
        }

        public async Task<Response> DeleteAsync(Product entity)
        {
            try
            {
                var productToDelete = await context.Products.FindAsync(entity.Id);
                if (productToDelete == null)
                {
                    return new Response(false, "Product not found");
                }
                context.Products.Remove(productToDelete);
                await context.SaveChangesAsync();
                return new Response(true, "Product deleted successfully");
            }
            catch (Exception ex)
            {
                LogException.LogExceptions(ex);
                return new Response(false, "An error occurred while creating the product");
            }
        }

        public async Task<Product> FindByIdAsync(int id)
        {
            try
            {
                var product = await context.Products.FindAsync(id);
                return product is not null ? product : null!;
            }
            catch (Exception ex)
            {
                LogException.LogExceptions(ex);
                throw new InvalidOperationException("An error occurred while finding the product");
            }
        }

        public async Task<IEnumerable<Product>> GetAllAsync()
        {
            try
            {
                var products = await context.Products.AsNoTracking().ToListAsync();
                return products is not null ? products : null!;
            }
            catch (Exception ex)
            {
                LogException.LogExceptions(ex);
                throw new InvalidOperationException("An error occurred while finding the product");
            }

        }

        public async Task<Product> GetByAsync(Expression<Func<Product, bool>> predicte)
        {
            var product = await context.Products.Where(predicte).FirstOrDefaultAsync();
            return product is not null ? product : null!;
        }

        public async Task<Response> UpdateAsync(Product entity)
        {
            try
            {
                var product = await FindByIdAsync(entity.Id);
                if (product is null) return new Response(false, $"Product {entity.Name} Not Found");
                context.Entry(product).State = EntityState.Detached;
                context.Products.Update(entity);
                  await context.SaveChangesAsync();
                return new Response(true, "Product pdatedd Sucessfulley");


            }
            catch (Exception ex)
            {
                LogException.LogExceptions(ex);
                throw new InvalidOperationException("An error occurred while finding the product");
            }
        }
    }
}
