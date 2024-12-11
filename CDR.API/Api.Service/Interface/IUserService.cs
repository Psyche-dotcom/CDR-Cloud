using CDR.Entities.Concrete;
using CDR.Entities.Dtos.WebApi;

namespace CDR.API.Api.Service.Interface
{
    public interface IUserService
    {
        Task<ResponseDto<User>> UserInfoAsync(string userid);
    }
}
