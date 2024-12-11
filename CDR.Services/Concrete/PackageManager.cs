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
    public class PackageManager : IPackageService
    {
        private readonly IUnitOfWork _unitOfWork;

        public PackageManager(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<IDataResult<PackageListDto>> GetAllWithoutTrialAsync(int Simultaneous)
        {
            var packages = await _unitOfWork.Packages.GetAllAsync(x => x.SimultaneousCalls == Simultaneous && x.IsActive && x.IsShown);
            if (packages.Count > 0)
            {
                return new DataResult<PackageListDto>(ResultStatus.Success, new PackageListDto
                {
                    Packages = packages.OrderBy(x => x.Month).ToList(),
                });
            }
            return new DataResult<PackageListDto>(ResultStatus.Error, "", new PackageListDto
            {
                Packages = null,
            });
        }

        public async Task<IDataResult<PackageDto>> GetAsync(int PackageId)
        {
            var package = await _unitOfWork.Packages.GetAsync(c => c.Id == PackageId);
            if (package != null)
            {
                return new DataResult<PackageDto>(ResultStatus.Success, new PackageDto
                {
                    Package = package,
                });
            }
            return new DataResult<PackageDto>(ResultStatus.Error, "", new PackageDto
            {
                Package = null,
            });
        }

        public async Task<IDataResult<PackageDto>> GetTrialAsync()
        {
            var package = await _unitOfWork.Packages.GetAsync(c => c.IsTrial && c.IsActive);
            if (package != null)
            {
                return new DataResult<PackageDto>(ResultStatus.Success, new PackageDto
                {
                    Package = package,
                });
            }
            return new DataResult<PackageDto>(ResultStatus.Error, "", new PackageDto
            {
                Package = null,
            });
        }

        public async Task<IDataResult<PackageListDto>> GetAllMonthlyAsync()
        {
            var packages = await _unitOfWork.Packages.GetAllAsync(x => x.Month == 1 && x.IsShown && x.IsActive);
            if (packages.Count > 0)
            {
                return new DataResult<PackageListDto>(ResultStatus.Success, new PackageListDto
                {
                    Packages = packages.OrderBy(x => x.SimultaneousCalls).ToList(),
                });
            }
            return new DataResult<PackageListDto>(ResultStatus.Error, "", new PackageListDto
            {
                Packages = null,
            });
        }

        public async Task<IDataResult<PackageListDto>> GetAllAnnualAsync()
        {
            var packages = await _unitOfWork.Packages.GetAllAsync(x => x.Month == 12 && x.IsShown && x.IsActive);
            if (packages.Count > 0)
            {
                return new DataResult<PackageListDto>(ResultStatus.Success, new PackageListDto
                {
                    Packages = packages.OrderBy(x => x.SimultaneousCalls).ToList(),
                });
            }
            return new DataResult<PackageListDto>(ResultStatus.Error, "", new PackageListDto
            {
                Packages = null,
            });
        }
    }
}
