using AuthenticationAp.Infrastructure.DependencyInjection;
namespace AuthenticationAp.Representation
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddControllers();
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();
            builder.Services.AddinfrastructureServices(builder.Configuration);
            var app = builder.Build();
            app.UserinfrastructurePoliciy();
            app.UseSwagger();
             app.UseSwaggerUI();
            //app.UseHttpsRedirection();
            app.UseAuthorization();
            app.MapControllers();
            app.Run();
        }
    }
}
