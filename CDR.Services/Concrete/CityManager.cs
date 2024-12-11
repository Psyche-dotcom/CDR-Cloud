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
    public class CityManager : ICityService
    {
        private readonly IUnitOfWork _unitOfWork;

        public CityManager(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<IDataResult<CityListDto>> GetAllAsync()
        {
            var cities = await _unitOfWork.Cities.GetAllAsync(null);
            if (cities.Count > -1)
            {
                return new DataResult<CityListDto>(ResultStatus.Success, new CityListDto
                {
                    Cities = cities
                });
            }
            return new DataResult<CityListDto>(ResultStatus.Error, Messages.Content.NotFound(isPlural: true), new CityListDto
            {
                Cities = null
            });
        }

        public async Task<IDataResult<CityListDto>> GetAllByCountryAsync(int countryId)
        {
            var cities = await _unitOfWork.Cities.GetAllAsync(x => x.CountryId == countryId);
            if (cities.Count > 0)
            {
                return new DataResult<CityListDto>(ResultStatus.Success, new CityListDto
                {
                    Cities = cities
                });
            }
            return new DataResult<CityListDto>(ResultStatus.Error, Messages.Content.NotFound(isPlural: true), new CityListDto
            {
                Cities = null
            });
        }

        public async Task<IDataResult<CityDto>> GetAsync(int cityId)
        {
            var city = await _unitOfWork.Cities.GetAsync(x => x.Id == cityId);
            if (city != null)
            {
                return new DataResult<CityDto>(ResultStatus.Success, new CityDto
                {
                    City = city
                });
            }
            return new DataResult<CityDto>(ResultStatus.Error, Messages.Content.NotFound(isPlural: false), null);
        }
    }
}
