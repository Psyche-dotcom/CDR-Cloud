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
    public class LocalizationCultureManager : ILocalizationCultureService
    {
        private readonly IUnitOfWork _unitOfWork;

        public LocalizationCultureManager(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<IDataResult<LocalizationCultureListDto>> GetAllAsync()
        {
            var localizationCultures = await _unitOfWork.LocalizationCultures.GetAllAsync(null);
            if (localizationCultures.Count > 0)
            {
                return new DataResult<LocalizationCultureListDto>(ResultStatus.Success, new LocalizationCultureListDto
                {
                    LocalizationCultures = localizationCultures
                });
            }
            return new DataResult<LocalizationCultureListDto>(ResultStatus.Error, Messages.Localizations.NotFound(isPlural: true), new LocalizationCultureListDto
            {
                LocalizationCultures = null
            });
        }

        public async Task<IDataResult<LocalizationCultureDto>> GetAsync(int localizationCultureId)
        {
            var localization = await _unitOfWork.LocalizationCultures.GetAsync(x => x.Id == localizationCultureId);
            if (localization != null)
            {
                return new DataResult<LocalizationCultureDto>(ResultStatus.Success, new LocalizationCultureDto
                {
                    LocalizationCulture = localization
                });
            }
            return new DataResult<LocalizationCultureDto>(ResultStatus.Error, Messages.Localizations.NotFound(isPlural: false), null);
        }

        public async Task<IDataResult<LocalizationCultureDto>> GetByCultureAsync(string localizationCulture)
        {
            var localization = await _unitOfWork.LocalizationCultures.GetAsync(x => x.Culture == localizationCulture);
            if (localization != null)
            {
                return new DataResult<LocalizationCultureDto>(ResultStatus.Success, new LocalizationCultureDto
                {
                    LocalizationCulture = localization
                });
            }
            return new DataResult<LocalizationCultureDto>(ResultStatus.Error, Messages.Localizations.NotFound(isPlural: false), null);
        }
    }
}
