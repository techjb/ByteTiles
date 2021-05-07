using Microsoft.AspNetCore.Mvc;

namespace SimpleByteTilesServer.Controler
{
    public class ExamplesController : Controller
    {
        [Route("countries-vector")]
        public IActionResult countries_vector()
        {
            return View();
        }

        [Route("countries-raster")]
        public IActionResult countries_raster()
        {
            return View();
        }

        [Route("satellite-lowres")]
        public IActionResult satellite_lowres()
        {
            return View();
        }

        [Route("europolis")]
        public IActionResult europolis()
        {
            return View();
        }

        [Route("buildings")]
        public IActionResult buildings()
        {
            return View();
        }
    }
}