using AutoMapper;
using CDR.API.Api.Model;
using CDR.API.PartialViewGeneration;
using CDR.Entities.Concrete;
using CDR.Entities.Dtos;
using CDR.Services.Abstract;
using CDR.Shared.Utilities.Results.ComplexTypes;
using CDR.Shared.Utilities.Results.Concrete;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System.IdentityModel.Tokens.Jwt;
using System.Text.Json;

namespace CDR.API.Controllers
{
    [Authorize(AuthenticationSchemes = "Bearer")]
    [Route("api/support")]
    [ApiController]
    public class SupportController : ControllerBase
    {

        private readonly UserManager<User> _userManager;
        private readonly IMapper _mapper;
        private readonly IStaticService _staticService;
        private readonly ISupportService _supportService;
        private readonly IMailService _mailService;
        private readonly ILogger<SupportController> _logger;
        private readonly SmtpSettings _smtpSettings;

        public SupportController(IOptions<SmtpSettings> smtpSettings ,ILogger<SupportController> logger,
            IMailService mailService, ISupportService supportService, IStaticService staticService, IMapper mapper, UserManager<User> userManager)
        {
            _smtpSettings = smtpSettings.Value;
            _logger = logger;
            _mailService = mailService;
            _supportService = supportService;
            _staticService = staticService;
            _mapper = mapper;
            _userManager = userManager;
        }

        [HttpGet("GetSupportBagde")]
        public async Task<IActionResult> GetSupportBagde()
        {
            var userId = User.Claims.FirstOrDefault(c => c.Type == JwtRegisteredClaimNames.Jti).Value;
            var supportBadge = await _supportService.GetBadgeAsync(LoggedInUser(userId).Id);


            return Ok(JsonSerializer.Serialize(new SupportAddAjaxModel
            {
                ResultStatus = supportBadge.ResultStatus
            }));
        }
        [HttpGet("GetSupports")]
        public async Task<IActionResult> GetSupports()
        {
            var userId = User.Claims.FirstOrDefault(c => c.Type == JwtRegisteredClaimNames.Jti).Value;
            var supports = await _supportService.GetAllAsync(LoggedInUser(userId).Id);
            var htmlContent = PartialViewGen.GenerateSupportListHtml(supports.Data, _staticService);
            return Ok(htmlContent);

        }
        [HttpGet("GetSupportMessages")]
        public async Task<IActionResult> GetSupportMessages(string PublicId)
        {
            var userId = User.Claims.FirstOrDefault(c => c.Type == JwtRegisteredClaimNames.Jti).Value;
            Guid SupportPublicId = new Guid();

            try
            {
                SupportPublicId = new Guid(PublicId);
            }
            catch (Exception ex)
            {
                _logger.LogError("GetSupportMessages -> " + ex.Message);
            }

            var supportMessages = await _supportService.GetAllMessagesAsync(SupportPublicId);

            if (supportMessages.ResultStatus == ResultStatus.Success)
            {
                var htmlContent = PartialViewGen.GenerateSupportMessagesHtml(new SupportMessageListModel
                {
                    SupportMessages = supportMessages.Data.SupportMessages,
                    User = LoggedInUser(userId),
                    NewMessages = supportMessages.Data.NewMessages
                }, _staticService);
                return Ok(htmlContent);
            }

            else
            {
                var htmlContent = PartialViewGen.GenerateSupportMessagesHtml(new SupportMessageListModel
                {
                    SupportMessages = null,
                    User = LoggedInUser(userId),
                    NewMessages = null
                }, _staticService);
                return Ok(htmlContent);

            }


        }

        [HttpPost("GetSupportDetail")]
        public async Task<IActionResult> GetSupportDetail(string PublicId)
        {
            var userId = User.Claims.FirstOrDefault(c => c.Type == JwtRegisteredClaimNames.Jti).Value;

            Guid SupportPublicId = new Guid();

            try
            {
                SupportPublicId = new Guid(PublicId);
            }
            catch (Exception ex)
            {
                _logger.LogError("GetSupportDetail -> " + ex.Message);
            }

            var support = await _supportService.GetAsync(SupportPublicId);

            if (support.ResultStatus == ResultStatus.Success)
            {
                return Ok(JsonSerializer.Serialize(new SupportDetailAjaxModel
                {
                    CreatedDate = support.Data.CreatedDate,
                    Id = support.Data.Id,
                    Statue = support.Data.Statue,
                    SupportPublicId = support.Data.PublicId,
                    SupportCategories = support.Data.Category.Name,
                    ResultStatus = (byte)ResultStatus.Success
                }));
            }
            else
            {
                return Ok(JsonSerializer.Serialize(new SupportDetailAjaxModel
                {
                    ResultStatus = (byte)ResultStatus.Error,
                    Message = support.Message
                }));
            }
        }
        [HttpPost("AddMessage")]
        public async Task<IActionResult> AddMessage(string PublicId, string Text)
        {
            var userId = User.Claims.FirstOrDefault(c => c.Type == JwtRegisteredClaimNames.Jti).Value;


            Guid SupportPublicId = new Guid();

            try
            {
                SupportPublicId = new Guid(PublicId);
            }
            catch (Exception ex)
            {
                _logger.LogError("AddMessage -> " + ex.Message);
            }

            if (string.IsNullOrWhiteSpace(Text))
            {
                var addMessageAjaxErrorModel = JsonSerializer.Serialize(new SupportAddAjaxModel
                {
                    Message = _staticService.GetLocalization("DBO_MesajGiriniz").Data,
                    ResultStatus = ResultStatus.Error
                });

                return Ok(addMessageAjaxErrorModel);
            }

            if (Text.Length > 10000)
            {
                var addMessageAjaxErrorModel = JsonSerializer.Serialize(new SupportAddAjaxModel
                {
                    Message = _staticService.GetLocalization("DBO_MaxMesaj").Data,
                    ResultStatus = ResultStatus.Error
                });

                return Ok(addMessageAjaxErrorModel);
            }

            var result = await _supportService.AddMessageAsync(SupportPublicId, Text, LoggedInUser(userId).Id);
            if (result.ResultStatus == ResultStatus.Success)
            {
                var addMessageAjaxModel = JsonSerializer.Serialize(new SupportAddAjaxModel
                {
                    ResultStatus = ResultStatus.Success,
                    Message = result.Message,
                });

                return Ok(addMessageAjaxModel);
            }
            else
            {
                var addMessageAjaxErrorModel = JsonSerializer.Serialize(new SupportAddAjaxModel
                {
                    Message = result.Message,
                    ResultStatus = ResultStatus.Error
                });
                return Ok(addMessageAjaxErrorModel);
            }
        }
        [HttpPost("AddSupport")]
        public async Task<IActionResult> AddSupport(SupportAddDto supportAddDto)
        {
            var userId = User.Claims.FirstOrDefault(c => c.Type == JwtRegisteredClaimNames.Jti).Value;

            if (ModelState.IsValid)
            {
                var result = await _supportService.AddAsync(supportAddDto, LoggedInUser(userId));
                if (result.ResultStatus == ResultStatus.Success)
                {
                    var htmlContent = PartialViewGen.GenerateSupportAddHtml(supportAddDto, _staticService,false);
                    var createSupportAjaxModel = JsonSerializer.Serialize(new SupportAddAjaxModel
                    {
                        SupportAddPartial = htmlContent,
                        ResultStatus = ResultStatus.Success,
                        Message = result.Message,
                    });

                    _mailService.SendSupportEmailByResendClient(new EmailSendDto
                    {
                        Email = _smtpSettings.SupportMailAddress,
                        Name = LoggedInUser(userId).FirstName + " " + LoggedInUser(userId).LastName,
                        Subject = _staticService.GetLocalization("CDR_CdrDestekOluşturuldu").Data,
                        Message = supportAddDto.SupportMessage
                    },
                    result.Data
                    );

                    return Ok(createSupportAjaxModel);
                }
                else
                {
                    supportAddDto.ModelError = result.Message;
  
                    var htmlContent = PartialViewGen.GenerateSupportAddHtml(supportAddDto, _staticService, true);
                    var createSupportAjaxErrorModel = JsonSerializer.Serialize(new SupportAddAjaxModel
                    {
                        SupportAddDto = supportAddDto,
                        SupportAddPartial = htmlContent
                    });
                    return Ok(createSupportAjaxErrorModel);
                }

            }
            else
            {
                supportAddDto.ModelError ="Please fill the required field";
                var htmlContent = PartialViewGen.GenerateSupportAddHtml(supportAddDto, _staticService, true);
                var createSupportAjaxModel = JsonSerializer.Serialize(new SupportAddAjaxModel
                {
                    SupportAddDto = supportAddDto,
                    SupportAddPartial =htmlContent
                });
                return Ok(createSupportAjaxModel);
            }

        }
        [HttpGet("AddSupport")]
        public async Task<IActionResult> AddSupportGet()
        {
            var htmlContent = PartialViewGen.GenerateSupportAddHtml(new SupportAddDto(), _staticService, false);
            return Ok(htmlContent);
        }

            protected User LoggedInUser(string id) => _userManager.FindByIdAsync(id).Result;
    }
}
