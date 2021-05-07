using ByteTilesReaderWriter;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.ResponseCompression;
using Microsoft.AspNetCore.StaticFiles;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Hosting;
using System.Collections.Generic;
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
            CacheTilesDictionaryId(memoryCache, "countries-vector");
            CacheTilesDictionaryId(memoryCache, "countries-raster");
            CacheTilesDictionaryId(memoryCache, "europolis");
            CacheTilesDictionaryId(memoryCache, "satellite-lowres");
            CacheTilesDictionaryId(memoryCache, "buildings");
        }

        private static void CacheTilesDictionaryId(IMemoryCache memoryCache, string id)
        {
            string file = @"C:\Users\Chus\source\repos\ByteTiles\ByteTilesReaderWriter_Test\files\" + id + ".bytetiles";
            CacheTilesDictionaryFile(memoryCache, id, file);
        }

        private static void CacheTilesDictionaryFile(IMemoryCache memoryCache, string id, string file)
        {
            ByteTilesReader byteTilesReader = new(file);
            Dictionary<string, string> dictionary = byteTilesReader.GetTilesDictionary();
            memoryCache.Set(id, dictionary);
        }
    }
}