using CDR.Data.Abstract;
using CDR.Entities.Concrete;
using CDR.Entities.Dtos;
using CDR.Services.Abstract;
using CDR.Shared.Utilities.Results.Abstract;
using CDR.Shared.Utilities.Results.ComplexTypes;
using CDR.Shared.Utilities.Results.Concrete;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CDR.Services.Concrete
{
    public class TransactionManager : ITransactionService
    {
        private readonly IStaticService _staticService;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IDepositService _depositService;

        public TransactionManager(IStaticService staticService, IUnitOfWork unitOfWork, IDepositService depositService)
        {
            _staticService = staticService;
            _unitOfWork = unitOfWork;
            _depositService = depositService;
        }

        public async Task<IDataResult<DateTime>> AddAsync(Deposit deposit, string paymentId, decimal price, byte currency, TransactionDetails trancationDetails)
        {
            var addTransaction = await _unitOfWork.Transactions.AddAsync(new Transaction
            {
                TransactionId = paymentId,
                Currency = currency,
                DepositPublicId = deposit.PublicId,
                Price = price,
                PublicId = Guid.NewGuid()
            });

            await _unitOfWork.SaveAsync();

            var addTransactionDetail = await _unitOfWork.TransactionDetails.AddAsync(trancationDetails);

            await _unitOfWork.SaveAsync();

            var packageList = await _unitOfWork.UserActivePackages.GetAllAsync(x => x.IsActive && x.UserId == deposit.UserId);

            var activePackage = packageList.LastOrDefault();

            var date = DateTime.Now;

            if (activePackage == null || activePackage.EndDate < DateTime.Now)
            {
                var addUserPackage = await _unitOfWork.UserActivePackages.AddAsync(new UserActivePackages
                {
                    EndDate = DateTime.Now.AddMonths(deposit.Package.Month),
                    PackageId = deposit.Package.Id,
                    StartDate = DateTime.Now,
                    UserId = deposit.UserId
                });

                await _unitOfWork.SaveAsync();

                date = addUserPackage.EndDate;
            }
            else
            {
                var addUserPackage = await _unitOfWork.UserActivePackages.AddAsync(new UserActivePackages
                {
                    EndDate = activePackage.EndDate.AddMonths(deposit.Package.Month),
                    PackageId = deposit.Package.Id,
                    StartDate = activePackage.StartDate,
                    UserId = deposit.UserId
                });

                await _unitOfWork.SaveAsync();

                date = addUserPackage.EndDate;
            }

            return new DataResult<DateTime>(ResultStatus.Success, _staticService.GetLocalization("DBO_PaketinizTanimlandi").Data, date);
        }

        public async Task<IDataResult<MembershipTransactionListDto>> GetAllAsync(int UserId)
        {
            var deposit = await _unitOfWork.Deposits.GetAllAsync(x => x.UserId == UserId, x => x.User, x => x.Package);
            var depositPublicIds = deposit.Select(x => x.PublicId).ToList();

            var transaction = await _unitOfWork.Transactions.GetAllAsync(x => depositPublicIds.Contains(x.DepositPublicId));

            if (transaction == null || transaction.Count == 0)
                return new DataResult<MembershipTransactionListDto>(ResultStatus.Error, "", new MembershipTransactionListDto
                {
                    Transactions = null,
                });

            var list = transaction.Select(x => new MembershipTransactionDto
            {
                CreatedDate = x.CreatedDate,
                Currency = x.Currency,
                Price = x.Price,
                TransactionId = x.TransactionId,
                PackageName = deposit.Where(y => y.PublicId == x.DepositPublicId).Select(y => y.Package.Name).FirstOrDefault()
            }).ToList();

            return new DataResult<MembershipTransactionListDto>(ResultStatus.Success, "", new MembershipTransactionListDto
            {
                Transactions = list,
            });
        }

        public async Task<IDataResult<bool>> AddPromotionUsage(PromotionUsageDto promotionUsageDto, string LicenseKeyForUser)
        {
            PromotionUsage newPromotionUsageEntity = new PromotionUsage
            {
                PromotionCode = promotionUsageDto.PromotionCode,
                UserId = promotionUsageDto.UserId,
                LicenseKey = LicenseKeyForUser
            };

            var addedPromotionUsage = await _unitOfWork.PromotionUsage.AddAsync(newPromotionUsageEntity);

            if (addedPromotionUsage is null)
            {
                return new DataResult<bool>(ResultStatus.Success, false);
            }

            await _unitOfWork.SaveAsync();

            return new DataResult<bool>(ResultStatus.Success,true);
        }

        public async Task<IDataResult<bool>> IsPromotionCodeUsed(string PromotionCode, string LicenseKeyForUser)
        {
            var result = await _unitOfWork.PromotionUsage.AnyAsync(q => q.LicenseKey.Equals(LicenseKeyForUser));

            if (!result)
            {
                return new DataResult<bool>(ResultStatus.Success, true);
            }

            return new DataResult<bool>(ResultStatus.Error, false);
        }
    }
}
