using CDR.Entities.Concrete;
using CDR.Entities.Dtos;
using CDR.Shared.Utilities.Results.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CDR.Services.Abstract
{
    public interface IApi3cxService
    {
        Task<IDataResult<UserConnectionDetailDto>> GetApiConnectionDetails(User User);
        Task<IDataResult<OrderApiDto>> GetSimultaneousCalls(User User);
        Task<IResult> TriggerHangfire(string IpAddress);
        Task<IDataResult<string>> GetLicenseKeyForUser(User User);
        Task<IDataResult<string>> CheckLicenseKeyFromOrderAPI(string PromotionCode, string LicenseKey);
    }
}
