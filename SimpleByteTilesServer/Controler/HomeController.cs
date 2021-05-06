using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Caching.Memory;

namespace SimpleByteTilesServer.Controler
{
    [Route("Home/Index")]
    public class HomeController : Controller
    {
        private readonly IMemoryCache MemoryCache;
        
        public HomeController(IMemoryCache memoryCache)
        {
            MemoryCache = memoryCache;
        }
        public IActionResult Index()
        {
            bool AlreadyExist = MemoryCache.TryGetValue("CachedTime", out DateTime currenTime);
            if (!AlreadyExist)
            {
                currenTime = DateTime.Now;

                MemoryCache.Set("CachedTime", currenTime, new MemoryCacheEntryOptions());
            }
            return View(currenTime);

        }
    }
}
