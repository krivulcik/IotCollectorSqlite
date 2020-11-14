using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace IotCollectorSqlite
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            InitDatabase(@"URI=file:./test.db");

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {

                //endpoints.MapControllers();
                endpoints.MapDefaultControllerRoute();
            });
        }

        public void InitDatabase(string connectionString)
        {
            using var con = new SQLiteConnection(connectionString);
            con.Open();

            using var cmd = new SQLiteCommand(con);
            cmd.CommandText = "CREATE TABLE IF NOT EXISTS dht_data (id INTEGER PRIMARY KEY, timestamp INTEGER, station STRING, temperature REAL, humidity INTEGER);";
            cmd.ExecuteNonQuery();

            con.Close();
        }
    }
}
