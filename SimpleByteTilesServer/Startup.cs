using ByteTilesReaderWriter;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.ResponseCompression;
using Microsoft.AspNetCore.StaticFiles;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Hosting;
using System.IO;
using System.Linq;

namespace SimpleByteTilesServer
{
    public class Startup
    {
        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc();
            services.AddResponseCompression(options =>
            {
                options.EnableForHttps = true;
                options.Providers.Add<GzipCompressionProvider>();
                options.MimeTypes = ResponseCompressionDefaults.MimeTypes.Concat(new[]
                    {
                        "application/x-protobuf",
                        "image/png",
                        "image/jpeg",
                    });
            });
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

            app.UseDefaultFiles();

            app.UseStaticFiles();

            app.UseResponseCompression();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

            CacheTilesDictionaries(memoryCache);
        }

        private static void CacheTilesDictionaries(IMemoryCache memoryCache)
        {
            ByteTilesCache byteTilesCache = new(memoryCache);
            byteTilesCache.SetTilesDictionary(GetFile("countries-vector"));
            byteTilesCache.SetTilesDictionary(GetFile("countries-raster"));
            byteTilesCache.SetTilesDictionary(GetFile("europolis"));
            byteTilesCache.SetTilesDictionary(GetFile("satellite-lowres"));            
        }

        private static string GetFile(string id)
        {
            return @"C:\Users\Chus\source\repos\ByteTiles\ByteTilesReaderWriter_Test\files\" + id + ".bytetiles";            
        }
    }
}