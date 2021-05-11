using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.ResponseCompression;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.DependencyInjection;
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

            CacheTilesDictionaries(env, memoryCache);
        }

        private static void CacheTilesDictionaries(IWebHostEnvironment env, IMemoryCache memoryCache)
        {
            var contentRoot = Directory.GetParent(env.ContentRootPath)
                + @"\ByteTilesReaderWriter_Test\files\";

            string file1 = contentRoot + "countries-vector.bytetiles";
            string file2 = contentRoot + "countries-raster.bytetiles";
            string file3 = contentRoot + "europolis.bytetiles";
            string file4 = contentRoot + "satellite-lowres.bytetiles";

            ByteTilesCache byteTilesCache = new(memoryCache);
            byteTilesCache.SetTilesDictionary(file1);
            byteTilesCache.SetTilesDictionary(file2);
            byteTilesCache.SetTilesDictionary(file3);
            byteTilesCache.SetTilesDictionary(file4);
        }
    }
}