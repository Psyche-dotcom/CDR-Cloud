using CDR.Data.Abstract;
using CDR.Entities.Dtos;
using CDR.Services.Abstract;
using CDR.Services.Utilities;
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
    public class CountryManager : ICountryService
    {
        private readonly IUnitOfWork _unitOfWork;

        public CountryManager(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<IDataResult<CountryListDto>> GetAllAsync()
        {
            var countries = await _unitOfWork.Countries.GetAllAsync(null);
            if (countries.Count > -1)
            {
                return new DataResult<CountryListDto>(ResultStatus.Success, new CountryListDto
                {
                    Countries = countries
                });
            }
            return new DataResult<CountryListDto>(ResultStatus.Error, Messages.Content.NotFound(isPlural: true), new CountryListDto
            {
                Countries = null
            });
        }

        public async Task<IDataResult<CountryDto>> GetAsync(int countryId)
        {
            var country = await _unitOfWork.Countries.GetAsync(x => x.Id == countryId);
            if (country != null)
            {
                return new DataResult<CountryDto>(ResultStatus.Success, new CountryDto
                {
                    Country = country
                });
            }
            return new DataResult<CountryDto>(ResultStatus.Error, Messages.Content.NotFound(isPlural: false), null);
        }
    }
}
