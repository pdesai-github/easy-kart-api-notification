
using MassTransit;

namespace EasyKart.Notification
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            var allowedOrigins = builder.Configuration.GetSection("CorsSettings:AllowedOrigins").Get<string[]>();

            builder.Services.AddSignalR();
            builder.Services.AddCors(options =>
            {
                options.AddPolicy("AllowCors", builder =>
                {
                    builder.WithOrigins(allowedOrigins)
                           .AllowAnyMethod()
                           .AllowAnyHeader()
                           .AllowCredentials();
                });
            });

            builder.Services.AddMassTransit(x =>
            {
                x.AddConsumer<NotificationConsumer>();
                x.UsingAzureServiceBus((context, cfg) =>
                {
                    cfg.Host(builder.Configuration.GetConnectionString("azservicebusconnstr"));

                    cfg.SubscriptionEndpoint("notificationsubscriber", "notificationtopic", e => 
                    {
                        e.ConfigureConsumer<NotificationConsumer>(context);

                    });
                });
            });

            // Add services to the container.

            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseAuthorization();
            app.UseCors("AllowCors");

            app.MapControllers();
            app.MapHub<NotificationHub>("/notificationhub");

            app.Run();
        }
    }
}
