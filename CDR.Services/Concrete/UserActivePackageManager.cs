using CDR.Data.Abstract;
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
    public class UserActivePackageManager : IUserActivePackagesService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IStaticService _staticService;

        public UserActivePackageManager(IUnitOfWork unitOfWork, IStaticService staticService)
        {
            _unitOfWork = unitOfWork;
            _staticService = staticService;
        }

        public async Task<IDataResult<DateTime>> CreateAsync(int PackageId, int UserId)
        {
            var package = await _unitOfWork.Packages.GetAsync(a => a.Id == PackageId);
            if (package == null)
            {
                return new DataResult<DateTime>(ResultStatus.Error, _staticService.GetLocalization("DBO_Yetki").Data, DateTime.Now);
            }

            var activePackages = await _unitOfWork.UserActivePackages.GetAllAsync(a => a.UserId == UserId);
            if (activePackages.Count == 0) //Trial
            {
                var endDate = DateTime.Now.AddMonths(package.Month);

                var addedActivePackage = await _unitOfWork.UserActivePackages.AddAsync(new Entities.Concrete.UserActivePackages
                {
                    PackageId = PackageId,
                    EndDate = endDate,
                    UserId = UserId,
                    StartDate = DateTime.Now
                });

                await _unitOfWork.SaveAsync();

                return new DataResult<DateTime>(ResultStatus.Success, "", endDate);
            }

            return new DataResult<DateTime>(ResultStatus.Error, _staticService.GetLocalization("DBO_Yetki").Data, DateTime.Now);
        }

        public async Task<IDataResult<UserActivePackageListDto>> GetAllAsync(int UserId)
        {
            var activePackages = await _unitOfWork.UserActivePackages.GetAllAsync(x=>x.UserId == UserId, c => c.Package);
            if (activePackages.Count > -1)
            {
                return new DataResult<UserActivePackageListDto>(ResultStatus.Success, new UserActivePackageListDto
                {
                    ActivePackages = activePackages
                });
            }
            return new DataResult<UserActivePackageListDto>(ResultStatus.Error, "", new UserActivePackageListDto
            {
                ActivePackages = null,
            });
        }
    }
}
