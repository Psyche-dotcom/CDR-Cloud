using CDR.Services.Abstract;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CDR.API.Controllers
{
    [Route("api/home")]
    [ApiController]
    public class HomeController : ControllerBase
    {
        private readonly IPackageService _packageService;

        public HomeController(IPackageService packageService)
        {
            _packageService = packageService;
        }
        [HttpGet("subcription/all")]
        public async Task<IActionResult> GetAllSubType() 
        {
            var packagesAnnual = await _packageService.GetAllAnnualAsync();
            var packagesMonth = await _packageService.GetAllMonthlyAsync();

            return Ok( new Entities.Dtos.PagePriceDto
            {
                Annual = packagesAnnual.Data,
                Monthly = packagesMonth.Data,
                Type = true
            });
        }
    }
}
