using CDR.Entities.Dtos;
using CDR.Shared.Utilities.Results.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CDR.Services.Abstract
{
    public interface IUserActivePackagesService
    {
        Task<IDataResult<DateTime>> CreateAsync(int PackageId,int UserId);
        Task<IDataResult<UserActivePackageListDto>> GetAllAsync(int UserId);
    }
}
