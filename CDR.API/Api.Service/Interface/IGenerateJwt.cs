using CDR.Entities.Concrete;

namespace CDR.API.Api.Service.Interface
{
    public interface IGenerateJwt
    {
        Task<string> GenerateToken(User user);
    }
}
