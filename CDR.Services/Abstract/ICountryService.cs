using CDR.Entities.Dtos;
using CDR.Shared.Utilities.Results.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CDR.Services.Abstract
{
    public interface ICountryService
    {
        Task<IDataResult<CountryDto>> GetAsync(int countryId);
        Task<IDataResult<CountryListDto>> GetAllAsync();
    }
}
