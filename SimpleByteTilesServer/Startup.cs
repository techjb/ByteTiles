using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.Collections.Generic;
using ByteTilesReaderWriter;
using Microsoft.AspNetCore.ResponseCompression;
using System.Linq;
using Microsoft.Extensions.FileProviders;
using System.IO;
using Microsoft.AspNetCore.StaticFiles;

namespace SimpleByteTilesServer
{
    public class Startup
    {
        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc();
            services.AddResponseCompression();
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

            app.UseStaticFiles(new StaticFileOptions
            {
                FileProvider = new PhysicalFileProvider(
                    Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "static")),
                RequestPath = "/static",
                ContentTypeProvider = new FileExtensionContentTypeProvider()
            });

            app.UseResponseCompression();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

            CacheTilesDictionaries(memoryCache);
        }

        private static void CacheTilesDictionaries(IMemoryCache memoryCache)
        {
            CacheTilesDictionary(memoryCache, "countries-vector");
            CacheTilesDictionary(memoryCache, "countries-raster");
            CacheTilesDictionary(memoryCache, "europolis");
            CacheTilesDictionary(memoryCache, "satellite-lowres");
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
