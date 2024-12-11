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
    public class LocalizationManager : ILocalizationService
    {
        private readonly IUnitOfWork _unitOfWork;

        public LocalizationManager(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<IDataResult<LocalizationListDto>> GetAllAsync()
        {
            var localizations = await _unitOfWork.Localizations.GetAllAsync(x=>x.IsActive);
            if (localizations.Count > -1)
            {
                return new DataResult<LocalizationListDto>(ResultStatus.Success, new LocalizationListDto
                {
                    Localizations = localizations
                });
            }
            return new DataResult<LocalizationListDto>(ResultStatus.Error, Messages.Localizations.NotFound(isPlural: true), new LocalizationListDto
            {
                Localizations = null
            });
        }

        public async Task<IDataResult<LocalizationListDto>> GetAllByCultureAsync(int cultureId)
        {
            var localizations = await _unitOfWork.Localizations.GetAllAsync(x=> x.IsActive && x.CultureId == cultureId);
            if (localizations.Count > 0)
            {
                return new DataResult<LocalizationListDto>(ResultStatus.Success, new LocalizationListDto
                {
                    Localizations = localizations
                });
            }
            return new DataResult<LocalizationListDto>(ResultStatus.Error, Messages.Localizations.NotFound(isPlural: true), new LocalizationListDto
            {
                Localizations = null
            });
        }

        public async Task<IDataResult<LocalizationDto>> GetAsync(int localizationId)
        {
            var localization = await _unitOfWork.Localizations.GetAsync(x => x.Id == localizationId);
            if (localization != null)
            {
                return new DataResult<LocalizationDto>(ResultStatus.Success, new LocalizationDto
                {
                    Localization = localization
                });
            }
            return new DataResult<LocalizationDto>(ResultStatus.Error, Messages.Localizations.NotFound(isPlural: false), null);
        }
    }
}
