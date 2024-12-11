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
    public interface IReportFilterService
    {
        Task<IDataResult<ReportFilter>> GetAsync(Guid publicId);
        Task<IDataResult<ReportFavoriteFilterListDto>> GetAllAsync(int UserId);
        Task<IResult> AddAsync(ReportFavoriteFilterAddDto reportFavoriFilterAddDto);
        Task<IResult> DeleteAsync(int Id, int UserId);
        Task<IResult> HardDeleteAsync(int Id, int UserId);
    }
}
