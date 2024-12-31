using AutoMapper;
using CDR.API.Api.Model;
using CDR.API.Api.Service.Interface;
using CDR.API.PartialViewGeneration;
using CDR.Entities.Concrete;
using CDR.Entities.Dtos;
using CDR.Entities.Dtos.WebApi;
using CDR.Entities.Dtos.WebApi.ViewModel;
using CDR.Services.Abstract;
using CDR.Shared.Utilities.Extensions;
using CDR.Shared.Utilities.Results.ComplexTypes;
using CDR.Shared.Utilities.Results.Concrete;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.IdentityModel.Tokens.Jwt;
using System.Net.Sockets;
using System.Text.Json;

namespace CDR.API.Controllers
{
    [Route("api/user")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly SignInManager<User> _signInManager;
        private readonly IConfiguration _configuration;
        private readonly IMailService _mailService;
        private readonly ICityService _cityService;
        private readonly IUserActivePackagesService _userActivePackageService;
        private readonly IPackageService _packageService;
        private readonly IApi3cxService _api3CxService;
        private readonly UserManager<User> _userManager;
        private readonly IMapper _mapper;
        private readonly ILogger<UserController> _logger;
        private readonly IStaticService _staticService;
        private readonly IGenerateJwt _generateJwt;

        public UserController(SignInManager<User> signInManager,
            IConfiguration configuration,
            IMailService mailService,
            ICityService cityService,
            IUserActivePackagesService userActivePackageService,
            IPackageService packageService,
            IApi3cxService api3CxService,
            UserManager<User> userManager,
            IMapper mapper,
            ILogger<UserController> logger,
            IStaticService staticService,
            IGenerateJwt generateJwt)
        {
            _signInManager = signInManager;
            _configuration = configuration;
            _mailService = mailService;
            _cityService = cityService;
            _userActivePackageService = userActivePackageService;
            _packageService = packageService;
            _api3CxService = api3CxService;
            _userManager = userManager;
            _mapper = mapper;
            _logger = logger;
            _staticService = staticService;
            _generateJwt = generateJwt;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register(UserRegisterDto userRegisterDto)
        {

            var response = new ResponseDto<string>();
            var user = _mapper.Map<User>(userRegisterDto);
            user.CreatedDate = DateTime.Now;


            var result = await _userManager.CreateAsync(user, userRegisterDto.Password);
            if (result.Succeeded)
            {
                var token = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                var callback = $"{_configuration["Callbacks"]}/confirm_email?token={token}&&email={user.Email}";


                var _result = _mailService.SendEmailConfirmEmailByResendClient(new EmailSendDto
                {
                    Email = userRegisterDto.Email,
                    Name = userRegisterDto.FirstName,
                    Subject = _staticService.GetLocalization("CDR_CDREmailAktivasyonu").Data,
                    Token = callback
                });

                response.StatusCode = 200;
                response.DisplayMessage = "Success";
                response.Result = "successfully registered user";
                return Ok(response);
            }
            else
            {/*
                foreach (var item in result.Errors)
                {
                    Console.WriteLine("error::",item.Description);
                    response.ErrorMessages.Add(item.Description.ToString());
                }
                response.StatusCode = 400;
                response.DisplayMessage = "Error";*/
                return BadRequest(result.Errors);
            }




        }


        [HttpPost("confirm_email")]
        public async Task<IActionResult> ConfirmEmail(ConfirmEmailRequestModel req)
        {

            var model = new UserEmailConfirmationModel
            {
                Result = false,
                Message = _staticService.GetLocalization("DBO_BirHataOlustu").Data
            };

            var user = await _userManager.FindByEmailAsync(req.Email);

            if (user == null)
            {
                model.Message = "Invalid Email Address";
                return NotFound(model);
            }

            var emailConfirmResult = await _userManager.ConfirmEmailAsync(user, req.Token);

            if (!emailConfirmResult.Succeeded)
                return BadRequest(emailConfirmResult.Errors);

            var trial = await _packageService.GetTrialAsync();

            if (trial.ResultStatus == ResultStatus.Success)
            {
                var addPackage = await _userActivePackageService.CreateAsync(trial.Data.Package.Id, user.Id);

                if (addPackage.ResultStatus == ResultStatus.Success)
                {
                    user.PackageFinishDate = addPackage.Data;

                    await _userManager.UpdateAsync(user);
                }
            }

            model.Result = true;
            model.Message = _staticService.GetLocalization("DBO_EmailAktivasyonBasarili").Data;

            return Ok(model);
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(UserLoginDto userLoginDto)
        {
            var response = new ResponseDto<string>();
            if (ModelState.IsValid)
            {
                var user = await _userManager.FindByEmailAsync(userLoginDto.Email);
                if (user != null)
                {
                    var result = await _signInManager.PasswordSignInAsync(user, userLoginDto.Password,
                        true, false);

                    if (result.Succeeded)
                    {

                        response.StatusCode = 200;
                        response.DisplayMessage = "Success";
                        response.Result = await _generateJwt.GenerateToken(user);
                        return Ok(response);

                    }
                    else
                    {
                        ModelState.AddModelError("", _staticService.GetLocalization("DBO_GirilenBilgilerdeKullaniciBulunamadi").Data);
                        return BadRequest(ModelState);
                    }
                }
                else
                {
                    var user2 = await _userManager.FindByNameAsync(userLoginDto.Email);
                    if (user2 != null)
                    {
                        var result = await _signInManager.PasswordSignInAsync(user2, userLoginDto.Password,
                        true, false);
                        if (result.Succeeded)
                        {
                            response.StatusCode = 200;
                            response.DisplayMessage = "Success";
                            response.Result = await _generateJwt.GenerateToken(user2);
                            return Ok(response);
                        }
                        else
                        {
                            ModelState.AddModelError("", _staticService.GetLocalization("DBO_GirilenBilgilerdeKullaniciBulunamadi").Data);
                            return Ok(ModelState);
                        }
                    }
                    else
                    {
                        ModelState.AddModelError("", _staticService.GetLocalization("DBO_GirilenBilgilerdeKullaniciBulunamadi").Data);
                        return BadRequest(ModelState);
                    }
                }
            }
            else
            {
                return Ok("UserLogin");
            }

        }

        [HttpGet("PasswordChange")]
        public async Task<IActionResult> PasswordChange()
        {
            return Ok(PartialViewGen.GeneratePasswordChangeHtml(new UserPasswordChangeDto(), _staticService, false));
        }
        [Authorize(AuthenticationSchemes = "Bearer")]
        [HttpPost("PasswordChange")]
        public async Task<IActionResult> PasswordChangeRequest(UserPasswordChangeDto userPasswordChangeDto)
        {
            if (ModelState.IsValid)
            {
                var userId = User.Claims.FirstOrDefault(c => c.Type == JwtRegisteredClaimNames.Jti).Value;
                var user = await _userManager.FindByIdAsync(userId);
                var isVerified = await _userManager.CheckPasswordAsync(user, userPasswordChangeDto.CurrentPassword);
                if (isVerified)
                {
                    var result = await _userManager.ChangePasswordAsync(user, userPasswordChangeDto.CurrentPassword,
                        userPasswordChangeDto.NewPassword);
                    if (result.Succeeded)
                    {
                        await _userManager.UpdateSecurityStampAsync(user);
                        await _signInManager.SignOutAsync();
                        await _signInManager.PasswordSignInAsync(user, userPasswordChangeDto.NewPassword, true, false);

                        var userPasswordChangeAjaxModel = JsonSerializer.Serialize(new UserPasswordChangeAjaxViewModel
                        {
                            UserPasswordChangePartial = PartialViewGen.GeneratePasswordChangeHtml(userPasswordChangeDto, _staticService, false),
                            ResultStatus = ResultStatus.Success,
                            Message = _staticService.GetLocalization("DBO_ProfilSifreBilgileriDuzenlendi").Data,
                        });
                        return Ok(userPasswordChangeAjaxModel);
                    }
                    else
                    {
                        foreach (var error in result.Errors)
                        {
                            ModelState.AddModelError("", error.Description);
                            userPasswordChangeDto.ModelError = error.Description;
                            _logger.LogError("PasswordChange -> " + error.Description);
                        }

                        var userPasswordChangeAjaxErrorModel = JsonSerializer.Serialize(new UserPasswordChangeAjaxViewModel
                        {
                            UserPasswordChangeDto = userPasswordChangeDto,
                            UserPasswordChangePartial = PartialViewGen.GeneratePasswordChangeHtml(userPasswordChangeDto, _staticService, true)
                        });
                        return Ok(userPasswordChangeAjaxErrorModel);
                    }
                }
                else
                {
                    userPasswordChangeDto.ModelError = _staticService.GetLocalization("DBO_GirdiginizSifreUyusmuyor").Data;
                    ModelState.AddModelError("", _staticService.GetLocalization("DBO_GirdiginizSifreUyusmuyor").Data);

                    var userPasswordChangeAjaxModelStateErrorModel = JsonSerializer.Serialize(new UserPasswordChangeAjaxViewModel
                    {
                        UserPasswordChangeDto = userPasswordChangeDto,
                        UserPasswordChangePartial = PartialViewGen.GeneratePasswordChangeHtml(userPasswordChangeDto, _staticService, true)
                    });

                    return Ok(userPasswordChangeAjaxModelStateErrorModel);
                }
            }
            else
            {
                userPasswordChangeDto.ModelError = "Error in request sent";
                var userPasswordChangeAjaxModelStateErrorModel = JsonSerializer.Serialize(new UserPasswordChangeAjaxViewModel
                {
                    UserPasswordChangeDto = userPasswordChangeDto,
                    UserPasswordChangePartial = PartialViewGen.GeneratePasswordChangeHtml(userPasswordChangeDto, _staticService, true)
                });
                return Ok(userPasswordChangeAjaxModelStateErrorModel);
            }

        }
        [HttpGet("Timezone")]
        public async Task<IActionResult> Timezone()
        {
            return Ok(PartialViewGen.GenerateUserTimezoneHtml(new UserTimezoneDto(), _staticService, false));
        }

        [Authorize(AuthenticationSchemes = "Bearer")]
        [HttpPost("Timezone")]
        public async Task<IActionResult> Timezone(UserTimezoneDto userTimezoneDto)
        {
            if (ModelState.IsValid)
            {
                var gmt = userTimezoneDto.GMT.Replace("+", "").Replace(":", ".");

                decimal gmtValue = Convert.ToDecimal(gmt, new System.Globalization.CultureInfo("en-US"));
                var userId = User.Claims.FirstOrDefault(c => c.Type == JwtRegisteredClaimNames.Jti).Value;

                var oldUser = await _userManager.FindByIdAsync(userId);

                oldUser.GMT = gmtValue;
                oldUser.Timezone = userTimezoneDto.Timezone;

                var result = await _userManager.UpdateAsync(oldUser);
                if (result.Succeeded)
                {
                    var userTimezoneAjaxModel = JsonSerializer.Serialize(new UserTimezoneAjaxViewModel
                    {
                        UserTimezonePartial = PartialViewGen.GenerateUserTimezoneHtml(userTimezoneDto, _staticService, false),
                        ResultStatus = ResultStatus.Success,
                        Message = _staticService.GetLocalization("DBO_TimezoneBilgileriDuzenlendi").Data,
                    });
                    return Ok(userTimezoneAjaxModel);
                }
                else
                {
                    foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError("", error.Description);
                        userTimezoneDto.ModelError = error.Description;
                    }

                    var userTimezoneAjaxErrorModel = JsonSerializer.Serialize(new UserTimezoneAjaxViewModel
                    {
                        UserTimezoneDto = userTimezoneDto,
                        UserTimezonePartial = PartialViewGen.GenerateUserTimezoneHtml(userTimezoneDto, _staticService, true)
                    });
                    return Ok(userTimezoneAjaxErrorModel);
                }

            }
            else
            {
                userTimezoneDto.ModelError = "Error in request sent";
                var userTimezoneAjaxModelStateErrorModel = JsonSerializer.Serialize(new UserTimezoneAjaxViewModel
                {
                    UserTimezoneDto = userTimezoneDto,
                    UserTimezonePartial = PartialViewGen.GenerateUserTimezoneHtml(userTimezoneDto, _staticService, true)
                });
                return Ok(userTimezoneAjaxModelStateErrorModel);
            }
        }
        [Authorize(AuthenticationSchemes = "Bearer")]
        [HttpGet("ProfileInformation")]
        public async Task<IActionResult> ProfileInformation()
        {
            var userId = User.Claims.FirstOrDefault(c => c.Type == JwtRegisteredClaimNames.Jti).Value;
            var user = await _userManager.FindByIdAsync(userId);
            return Ok(
                PartialViewGen.GenerateUserProfileHtml(new UserProfileInformationDto

                {
                    ZipCode = user.ZipCode,
                    Address = user.Address,
                    CityId = user.CityId,
                    CountryId = user.CountryId,
                    Email = user.Email,
                    PhoneNumber = user.PhoneNumber,
                    FirstName = user.FirstName,
                    LastName = user.LastName,
                    CompanyName = user.CompanyName
                }, _staticService, false));


        }

        [Authorize(AuthenticationSchemes = "Bearer")]
        [HttpPost("ProfileInformation")]
        public async Task<IActionResult> ProfileInformation(UserProfileInformationDto userProfileInformatioDto)
        {
            if (ModelState.IsValid)
            {
                var userId = User.Claims.FirstOrDefault(c => c.Type == JwtRegisteredClaimNames.Jti).Value;

                var oldUser = await _userManager.FindByIdAsync(userId);
                oldUser.FirstName = userProfileInformatioDto.FirstName;
                oldUser.LastName = userProfileInformatioDto.LastName;
                oldUser.Email = userProfileInformatioDto.Email;
                oldUser.PhoneNumber = userProfileInformatioDto.PhoneNumber;
                oldUser.CountryId = userProfileInformatioDto.CountryId;
                oldUser.CityId = userProfileInformatioDto.CityId;
                oldUser.Address = userProfileInformatioDto.Address;
                oldUser.ZipCode = userProfileInformatioDto.ZipCode;
                oldUser.CompanyName = userProfileInformatioDto.CompanyName;

                var result = await _userManager.UpdateAsync(oldUser);
                if (result.Succeeded)
                {
                    var userProfileInformationAjaxModel = JsonSerializer.Serialize(new UserProfileInformationAjaxViewModel
                    {
                        UserProfileInformationPartial = PartialViewGen.GenerateUserProfileHtml(userProfileInformatioDto, _staticService, false),
                        ResultStatus = ResultStatus.Success,
                        Message = _staticService.GetLocalization("DBO_ProfilBilgileriDuzenlendi").Data,
                    });
                    return Ok(userProfileInformationAjaxModel);
                }
                else
                {
                    foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError("", error.Description);
                        userProfileInformatioDto.ModelError = error.Description;
                    }

                    var userProfileInformationAjaxErrorModel = JsonSerializer.Serialize(new UserProfileInformationAjaxViewModel
                    {
                        UserProfileInformationDto = userProfileInformatioDto,
                        UserProfileInformationPartial = PartialViewGen.GenerateUserProfileHtml(userProfileInformatioDto, _staticService, true)
                    });
                    return Ok(userProfileInformationAjaxErrorModel);
                }

            }
            else
            {
                userProfileInformatioDto.ModelError = "Error in request sent";

                var userProfileInformationAjaxModelStateErrorModel = JsonSerializer.Serialize(new UserProfileInformationAjaxViewModel
                {
                    UserProfileInformationDto = userProfileInformatioDto,
                    UserProfileInformationPartial = PartialViewGen.GenerateUserProfileHtml(userProfileInformatioDto, _staticService, true)
                });
                return Ok(userProfileInformationAjaxModelStateErrorModel);
            }
        }

        [Authorize(AuthenticationSchemes = "Bearer")]
        [HttpPost("GetCityListByCountryId")]
        public async Task<IActionResult> GetCityListByCountryId(int CountryId)
        {
            var cities = await _cityService.GetAllByCountryAsync(CountryId);

            return Ok(cities.Data.Cities.Select(x => new CityListAjaxModel
            {
                Id = x.Id,
                Name = x.Name
            }).ToList());
        }

        [Authorize(AuthenticationSchemes = "Bearer")]
        [HttpGet("ConnectionDetail")]
        public async Task<IActionResult> ConnectionDetail()
        {
            var userId = User.Claims.FirstOrDefault(c => c.Type == JwtRegisteredClaimNames.Jti).Value;
            var user = await _userManager.FindByIdAsync(userId);

            return Ok(
                PartialViewGen.GenerateUserConnectionDetailHtml(new UserConnectionDetailDto
                {
                    DbName = user.DbName,
                    Version = user.Version,
                    DbPassword = user.DbPassword,
                    DbUsername = user.DbUsername,
                    IpAddress = user.IpAddress,
                    Port = user.Port
                }, _staticService, false
                ));



        }

        [Authorize(AuthenticationSchemes = "Bearer")]
        [HttpPost("ConnectionDetail")]
        public async Task<IActionResult> ConnectionDetail(UserConnectionDetailDto userConnectionDetailDto)
        {
            if (ModelState.IsValid)
            {
                Result connectionResult = CheckPortIsOpen(userConnectionDetailDto.IpAddress, Convert.ToInt32(userConnectionDetailDto.Port));

                if (connectionResult.ResultStatus == ResultStatus.Error)
                {
                    ModelState.AddModelError("", connectionResult.Message + " /" + userConnectionDetailDto.IpAddress + ":" + userConnectionDetailDto.Port);
                    userConnectionDetailDto.ModelError = connectionResult.Message + " /" + userConnectionDetailDto.IpAddress + ":" + userConnectionDetailDto.Port;
                    var userConnectionDetailAjaxModel = JsonSerializer.Serialize(new UserConnectionDetailAjaxViewModel
                    {
                        UserConnectionDetailPartial = PartialViewGen.GenerateUserConnectionDetailHtml(userConnectionDetailDto, _staticService, true),
                        ResultStatus = ResultStatus.Error,
                        Message = connectionResult.Message,
                    });
                    return Ok(userConnectionDetailAjaxModel);
                }
                var userId = User.Claims.FirstOrDefault(c => c.Type == JwtRegisteredClaimNames.Jti).Value;

                var oldUser = await _userManager.FindByIdAsync(userId);

                var simultaneous = await _api3CxService.GetSimultaneousCalls(oldUser);

                oldUser.IpAddress = !string.IsNullOrWhiteSpace(userConnectionDetailDto.IpAddress) ? userConnectionDetailDto.IpAddress : "";
                oldUser.Port = !string.IsNullOrWhiteSpace(userConnectionDetailDto.Port) ? userConnectionDetailDto.Port : oldUser.Port;
                oldUser.DbName = !string.IsNullOrWhiteSpace(userConnectionDetailDto.DbName) ? userConnectionDetailDto.DbName : "";
                oldUser.DbUsername = !string.IsNullOrWhiteSpace(userConnectionDetailDto.DbUsername) ? userConnectionDetailDto.DbUsername : "";
                oldUser.DbPassword = !string.IsNullOrWhiteSpace(userConnectionDetailDto.DbPassword) ? BaseExtensions.EncryptString(userConnectionDetailDto.DbPassword) : oldUser.DbPassword;

                if (string.IsNullOrWhiteSpace(userConnectionDetailDto.Port) ||
                     string.IsNullOrWhiteSpace(userConnectionDetailDto.DbName) ||
                     string.IsNullOrWhiteSpace(userConnectionDetailDto.DbUsername) ||
                     string.IsNullOrWhiteSpace(userConnectionDetailDto.DbPassword)
                     )
                {
                    var obj = await _api3CxService.GetApiConnectionDetails(oldUser);

                    if (obj.ResultStatus == ResultStatus.Success)
                    {
                        oldUser.Port = string.IsNullOrWhiteSpace(obj.Data.Port) ? oldUser.Port : obj.Data.Port;
                        oldUser.DbName = obj.Data.DbName;
                        oldUser.DbUsername = obj.Data.DbUsername;
                        oldUser.DbPassword = BaseExtensions.EncryptString(obj.Data.DbPassword);
                    }
                }

                oldUser.Version = userConnectionDetailDto.Version;
                oldUser.SimultaneousCalls = simultaneous.Data.SimultaneousCalls;
                oldUser.Edition = simultaneous.Data.Edition;

                var result = await _userManager.UpdateAsync(oldUser);
                if (result.Succeeded)
                {
                    await _api3CxService.TriggerHangfire(oldUser.IpAddress);

                    var userConnectionDetailAjaxModel = JsonSerializer.Serialize(new UserConnectionDetailAjaxViewModel
                    {
                        UserConnectionDetailPartial = PartialViewGen.GenerateUserConnectionDetailHtml(userConnectionDetailDto, _staticService, false),
                        ResultStatus = ResultStatus.Success,
                        Message = _staticService.GetLocalization("DBO_ProfilBaglantiBilgileriDuzenlendi").Data,
                    });
                    return Ok(userConnectionDetailAjaxModel);
                }
                else
                {
                    foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError("", error.Description);
                        userConnectionDetailDto.ModelError = error.Description;
                        /* Log.Error("ConnectionDetail -> " + error.Description);*/
                    }

                    var userConnectionDetailAjaxErrorModel = JsonSerializer.Serialize(new UserConnectionDetailAjaxViewModel
                    {
                        UserConnectionDetailDto = userConnectionDetailDto,
                        UserConnectionDetailPartial = PartialViewGen.GenerateUserConnectionDetailHtml(userConnectionDetailDto, _staticService, true)
                    });
                    return Ok(userConnectionDetailAjaxErrorModel);
                }

            }
            else
            {
                userConnectionDetailDto.ModelError = "Error in request sent";
                var userConnectionDetailAjaxModelStateErrorModel = JsonSerializer.Serialize(new UserConnectionDetailAjaxViewModel
                {
                    UserConnectionDetailDto = userConnectionDetailDto,
                    UserConnectionDetailPartial = PartialViewGen.GenerateUserConnectionDetailHtml(userConnectionDetailDto, _staticService, true)
                });
                return Ok(userConnectionDetailAjaxModelStateErrorModel);
            }
        }
        [HttpPost("ForgotPassword")]
        public async Task<IActionResult> ForgotPassword(UserForgotPasswordDto forgotPasswordModel)
        {
            var response = new ResponseDto<string>();
            if (ModelState.IsValid)
            {
                var user = await _userManager.FindByEmailAsync(forgotPasswordModel.Email);
                if (user == null)
                {
                    response.StatusCode = 400;
                    response.DisplayMessage = "Error";
                    response.ErrorMessages = new List<string>() { _staticService.GetLocalization("DBO_EmaileAitKullaniciBulunamadi").Data };

                    return BadRequest(response);
                }

                var token = await _userManager.GeneratePasswordResetTokenAsync(user);
                var callback = $"{_configuration["Callbacks"]}/reset_password?token={token}&&email={user.Email}";



                var _result = _mailService.SendForgotPasswordEmailByResendClient(new EmailSendDto
                {
                    Email = forgotPasswordModel.Email,
                    Name = user.FirstName,
                    Subject = _staticService.GetLocalization("CDR_CdrSifreDegisikligi").Data,
                    Token = callback
                });

                ModelState.Clear();

                if (_result.ResultStatus == ResultStatus.Error)
                {
                    response.StatusCode = 400;
                    response.DisplayMessage = "Error";
                    response.ErrorMessages = new List<string>() { _staticService.GetLocalization("CDR_SistemselHata").Data };
                    return BadRequest(response);

                }
                response.StatusCode = 200;
                response.DisplayMessage = "Success";
                response.Result = _staticService.GetLocalization("CDR_SifreDegistirmeIstekOlustu").Data;

                return Ok(response);

            }
            else
            {
                response.StatusCode = 400;
                response.DisplayMessage = "Error";
                response.ErrorMessages = new List<string>() { "Please provide a valid email address" };
                return BadRequest(response);
            }
        }

        [HttpPost("ResetPassword")]
        public async Task<IActionResult> ResetPassword(UserResetPasswordModel resetPasswordModel)
        {
            var response = new ResponseDto<string>();
            if (ModelState.IsValid)
            {
                var user = await _userManager.FindByEmailAsync(resetPasswordModel.Email);

                if (user == null)
                {
                    response.StatusCode = 400;
                    response.DisplayMessage = "Error";
                    response.ErrorMessages = new List<string>() { _staticService.GetLocalization("DBO_BirHataOlustu").Data };

                    return BadRequest(response);
                  
                }

                var resetPassResult = await _userManager.ResetPasswordAsync(user, resetPasswordModel.Token, resetPasswordModel.Password);
                if (!resetPassResult.Succeeded)
                {
                   

                    
                    foreach (var error in resetPassResult.Errors)
                    {
                        response.StatusCode = 400;
                        response.DisplayMessage = "Error";
                        response.ErrorMessages = new List<string>() { error.Description };
                    }
                    return BadRequest(response);
                }
                response.StatusCode = 200;
                response.DisplayMessage = "Success";
                response.Result ="Password Reset successfully, you can login with your new credential now";

                return Ok(response);
                
            }
            var errorMessages = ModelState.Values
            .SelectMany(v => v.Errors)
            .Select(e => e.ErrorMessage)
            .ToList();
            response.StatusCode = 400;
            response.DisplayMessage = "Error";
            response.ErrorMessages = new List<string>() { errorMessages[0] };
            return BadRequest(response);
        }

        private Result CheckPortIsOpen(string hostnameorIpAddress, int port)
        {
            try
            {
                using (var client = new TcpClient(hostnameorIpAddress, port))
                {
                    return new Result(ResultStatus.Success, "Connection Succeeded.");
                }
            }
            catch (SocketException ex)
            {
                Console.WriteLine($"Error connecting host:{ex.Message}");
                /* Log.Error(ex, "Error Connecting Host");*/
                return new Result(ResultStatus.Error, ex.Message);
            }
        }

    }
}
