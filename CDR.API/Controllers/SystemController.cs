using AutoMapper;
using CDR.Entities.Concrete;
using CDR.Entities.Dtos;
using CDR.Services.Abstract;
using CDR.Shared.Utilities.Results.ComplexTypes;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.IdentityModel.Tokens.Jwt;

namespace CDR.API.Controllers
{
    [Route("api/system")]
    [ApiController]
    public class SystemController : ControllerBase
    {
        private readonly IPostgreSqlService _postgreSqlService;
        private readonly UserManager<User> _userManager;
        private readonly IMapper _mapper;
        private readonly ILogger<UserController> _logger;
        private readonly IStaticService _staticService;

        public SystemController(IStaticService staticService, ILogger<UserController> logger, IMapper mapper, UserManager<User> userManager, IPostgreSqlService postgreSqlService)
        {
            _staticService = staticService;
            _logger = logger;
            _mapper = mapper;
            _userManager = userManager;
            _postgreSqlService = postgreSqlService;
        }

        [Authorize(AuthenticationSchemes = "Bearer")]
        [HttpPost("GetDbControl")]
        public async Task<IActionResult> GetDbControl()
        {
            var userId = User.Claims.FirstOrDefault(c => c.Type == JwtRegisteredClaimNames.Jti).Value;
            var control = await _postgreSqlService.GetConnectDB(LoggedInUser(userId));

            if (control.ResultStatus == ResultStatus.Success)
                return Ok(true);
            else
                return Ok(false);

        }

            protected User LoggedInUser(string id) => _userManager.FindByIdAsync(id).Result;
    }
}
