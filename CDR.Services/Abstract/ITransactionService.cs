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
    public interface ITransactionService
    {
        Task<IDataResult<DateTime>> AddAsync(Deposit deposit, string paymentId, decimal price, byte currency,TransactionDetails trancationDetails);
        Task<IDataResult<MembershipTransactionListDto>> GetAllAsync(int UserId);
        Task<IDataResult<bool>> AddPromotionUsage(PromotionUsageDto promotionUsageDto, string LicenseKeyForUser);
        Task<IDataResult<bool>> IsPromotionCodeUsed(string PromotionCode, string LicenseKeyForUser);
    }
}
