using MediatR;
using Microsoft.EntityFrameworkCore;
using ShoppingCart.Data;
using ShoppingCart.Data.Contracts;
using ShoppingCart.Entities;
using System.Reflection;

namespace ShoppingCart
{
    public class StartUp
    {
        public StartUp(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();            

            //In Memory Database, we could usesqlserver options here to connect with SQL database
            services.AddDbContext<ShoppingCartDbContext>(options =>
            options.UseInMemoryDatabase("ShoppingCartDbContext"));

            services.AddMediatR(Assembly.GetExecutingAssembly());
            services.AddControllersWithViews()
                 .AddNewtonsoftJson(options =>
                         options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore);

            services.AddScoped<IOrderRepo, OrderRepo>();
            services.AddScoped<IOrderDetailsRepo, OrderDetailsRepo>();
            services.AddScoped<IProductRepo, ProductRepo>();
            services.AddScoped<ICustomerRepo, CustomerRepo>();
            services.AddSwaggerGen();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, IServiceProvider serviceProvider)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI();               
            }

            var context = serviceProvider.GetService<ShoppingCartDbContext>();
            AddDummyData(context);
            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }

        private void AddDummyData(ShoppingCartDbContext? context)
        {
            if(context != null)
            {
                context.Customer.Add(new Customer { FirstName = "Joseph", LastName = "John", AddressLine1 = " abc line", AddressLine2 = "xyz Line", City = "Dublin", PostCode = "123456" });
                context.Customer.Add(new Customer { FirstName = "Tom", LastName = "Smith", AddressLine1 = " xyz line", AddressLine2 = "abc Line", City = "LetterKenny", PostCode = "987654" });
                context.Order.Add(new Order { Date = DateTime.Now, CustomerId = 1, IsShipped=false, ShippingAddress="Address1"});
                context.Order.Add(new Order { Date = DateTime.Now, CustomerId = 2, IsShipped = false , ShippingAddress="Address2"});
                context.Order.Add(new Order { Date = DateTime.Now, CustomerId = 1, IsShipped = true, ShippingAddress = "Address1" });
                context.Product.Add(new Product { ProductId = 1, Price = 999, ItemsLeft = 3, Name = "TV" });
                context.Product.Add(new Product { ProductId = 2, Price = 1500, ItemsLeft = 5, Name = "Computer" });
                context.OrderDetails.Add(new OrderDetails { OrderDetailsId = 1, OrderId = 1, ProductId = 1, ProductQuantity = 2 });
                context.OrderDetails.Add(new OrderDetails { OrderDetailsId = 2, OrderId = 2, ProductId = 2, ProductQuantity = 3 });
            }
            context.SaveChanges();           
            
        }
    }
}
