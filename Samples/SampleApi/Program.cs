using PipeFilterCore;

namespace WebApplication1
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            builder.Services.AddPipeAndFilter(
                     PipeAndFilter.New<WeatherForecast>()
                        .AddPipe(ExecPipe)
                        .Build("opc1"));

            builder.Services.AddPipeAndFilter(
                     PipeAndFilter.New<WeatherForecast>()
                        .AddPipe(ExecPipe)
                        .AddPipe(ExecPipe)
                        .Build("opc2"));

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }

        private static Task ExecPipe(EventPipe<WeatherForecast> pipe, CancellationToken token)
        {
            pipe.ThreadSafeAccess((contract) =>
            {
                contract.TemperatureC += 10;
            });
            return Task.CompletedTask;
        }
    }
}