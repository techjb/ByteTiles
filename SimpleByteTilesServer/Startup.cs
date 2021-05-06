using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.Collections.Generic;
using ByteTilesReaderWriter;

namespace SimpleByteTilesServer
{
    public class Startup
    {
        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc();

            services.AddMemoryCache();

        }


        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, IMemoryCache memoryCache)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseRouting();

            app.UseStaticFiles();            

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

            CacheData(memoryCache);
        }

        private void CacheData(IMemoryCache memoryCache)
        {
            CacheTilesDictionary(memoryCache, "countries");
        }

        private static void CacheTilesDictionary(IMemoryCache memoryCache, string fileName)
        {
            string filePath = @"C:\Users\Chus\source\repos\ByteTiles\ByteTilesReaderWriter_Test\files\" + fileName + ".bytetiles";
            ByteTilesReader byteTilesReader = new(filePath);
            Dictionary<string, string> dictionary = byteTilesReader.GetTilesDictionary();
            memoryCache.Set(fileName, dictionary);
        }
    }
}
