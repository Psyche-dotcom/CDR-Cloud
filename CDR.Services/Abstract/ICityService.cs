using CDR.Entities.Dtos;
using CDR.Shared.Utilities.Results.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CDR.Services.Abstract
{
    public interface ICityService
    {
        Task<IDataResult<CityDto>> GetAsync(int cityId);
        Task<IDataResult<CityListDto>> GetAllAsync();
        Task<IDataResult<CityListDto>> GetAllByCountryAsync(int countryId);
    }
}
