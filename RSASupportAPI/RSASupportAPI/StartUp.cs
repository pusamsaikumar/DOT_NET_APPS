using Amazon.S3;
using Newtonsoft.Json;
using RSASupportAPI.Models;
using RSASupportAPI.RSASupportBLL;
using RSASupportAPI.RSASupportDAL;

namespace RSASupportAPI
{
    public class StartUp
    {

        //private readonly IConfiguration _configuration;
        public IConfiguration configRoot { get; set; }

        // constructor
        public StartUp(IConfiguration configuration)
        {
            configRoot = configuration;

        }
        // configure services
        public void ConfigureServices(IServiceCollection services)
        {
            var path = Path.Combine(Environment.CurrentDirectory, "AppConfigurations.json");
            var json = File.ReadAllText(path);
            var appConfigurations = JsonConvert.DeserializeObject<List<AppConfigurations>>(json);
            services.AddSingleton(appConfigurations);
            services.AddOptions();
            services.Configure<ConnectionStrings>(configRoot.GetSection("ConnectionStrings"));
          
            services.AddScoped<Helpers>();
            services.AddScoped<IClientService,ClientService>();
            services.AddScoped<IClientRepo,ClientRepo>();
            services.AddScoped<S3BucketHelpers>();
           // services.AddScoped<SqlHelpers>();   
          
            services.AddDataProtection();
            services.AddMvc();
            services.AddControllers();
            services.AddSwaggerGen();
            services.AddHttpContextAccessor();
            // AWS API GETWAY:
            services.AddAWSLambdaHosting(LambdaEventSource.RestApi);

            // AWS S3 Bucket:
            services.AddDefaultAWSOptions(configRoot.GetAWSOptions());
            services.AddAWSService<IAmazonS3>();



            // add cors for frontend:
            services.AddCors(options =>
            {
                options.AddDefaultPolicy(
                    policy =>
                    {
                        policy.AllowAnyOrigin()
                              .AllowAnyHeader()
                              .AllowAnyMethod();
                    });
            });



        }

        public void Configure(WebApplication app, IWebHostEnvironment env)
        {
            app.UseCors();

            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }
            app.UseRouting();
            app.UseHttpsRedirection();
            app.UseAuthentication();
            app.UseAuthorization();
            app.MapControllers();
            app.Run();
        }
    }

}
