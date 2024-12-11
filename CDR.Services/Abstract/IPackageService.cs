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
    public interface IPackageService
    {
        Task<IDataResult<PackageListDto>> GetAllWithoutTrialAsync(int Simultaneous);
        Task<IDataResult<PackageDto>> GetTrialAsync();
        Task<IDataResult<PackageDto>> GetAsync(int PackageId);
        Task<IDataResult<PackageListDto>> GetAllMonthlyAsync();
        Task<IDataResult<PackageListDto>> GetAllAnnualAsync();
    }
}
