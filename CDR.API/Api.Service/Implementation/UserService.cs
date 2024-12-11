using CDR.API.Api.Service.Interface;
using CDR.Entities.Concrete;
using CDR.Entities.Dtos.WebApi;
using Microsoft.AspNetCore.Identity;

namespace CDR.API.Api.Service.Implementation
{
    public class UserService : IUserService
    {
        private readonly UserManager<User> _userManager;
        private readonly ILogger<UserService> _logger;

        public UserService(UserManager<User> userManager, 
            ILogger<UserService> logger)
        {
            _userManager = userManager;
            _logger = logger;
        }
        public async Task<ResponseDto<User>> UserInfoAsync(string userid)
        {
            var response = new ResponseDto<User>();
            try
            {
                var user = await _userManager.FindByIdAsync(userid);
                if (user == null)
                {
                    response.ErrorMessages = new List<string>() { "Invalid user"};
                    response.DisplayMessage = "Error";
                    response.StatusCode = 400;
                    return response;
                }
                response.Result = user;
                response.DisplayMessage = "Succes";
                response.StatusCode = 200;
                return response;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, ex);
                response.ErrorMessages = new List<string>() { "Error in getting user info" };
                response.StatusCode = 500;
                response.DisplayMessage = "Error";
                return response;
            }
        }
    }
}
