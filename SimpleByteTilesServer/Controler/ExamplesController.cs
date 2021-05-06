using Microsoft.AspNetCore.Mvc;

namespace SimpleByteTilesServer.Controler
{
    
    public class ExamplesController : Controller
    {
        [Route("mapbox")]
        public IActionResult Mapbox()
        {
            return View();
        }

        [Route("countries")]
        public IActionResult Countries()
        {
            return View();
        }
    }
}
