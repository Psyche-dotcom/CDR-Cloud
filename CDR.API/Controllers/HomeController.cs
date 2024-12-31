using CDR.Entities.Concrete;
using CDR.Services.Abstract;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace CDR.API.Controllers
{
    [Route("api/home")]
    [ApiController]
    public class HomeController : ControllerBase
    {
        private readonly IPackageService _packageService;
        private readonly string _setUpfolder = "Setup";
        private readonly DownloadFiles _downloadFiles;

        public HomeController(IPackageService packageService,
            IOptions<DownloadFiles> downloadFiles
           )
        {
            _downloadFiles = downloadFiles.Value;
            _packageService = packageService;
            _setUpfolder = Environment.GetEnvironmentVariable("SETUP_FOLDER") ?? Path.Combine(Directory.GetCurrentDirectory(), "Setup");

        }
        [HttpGet("subcription/all")]
        public async Task<IActionResult> GetAllSubType()
        {
            var packagesAnnual = await _packageService.GetAllAnnualAsync();
            var packagesMonth = await _packageService.GetAllMonthlyAsync();

            return Ok(new Entities.Dtos.PagePriceDto
            {
                Annual = packagesAnnual.Data,
                Monthly = packagesMonth.Data,
                Type = true
            });
        }
       // [Authorize(AuthenticationSchemes = "Bearer")]
        [HttpGet("DownloadWindows")]
        public IActionResult DownloadWindows()
        {
            string sWebRootFolder = _setUpfolder;

            var path = Path.Combine(sWebRootFolder, _downloadFiles.WindowsName);
            var fs = new FileStream(path, FileMode.Open);

            return File(fs, "application/octet-stream", _downloadFiles.WindowsName);
        }
        //[Authorize(AuthenticationSchemes = "Bearer")]
        [HttpGet("DownloadDebian")]
        public IActionResult DownloadDebian()
        {


            string sWebRootFolder = _setUpfolder;

            var path = Path.Combine(sWebRootFolder, _downloadFiles.DebianName);
            var fs = new FileStream(path, FileMode.Open);

            return File(fs, "application/octet-stream", _downloadFiles.DebianName);
        }


    }
}
