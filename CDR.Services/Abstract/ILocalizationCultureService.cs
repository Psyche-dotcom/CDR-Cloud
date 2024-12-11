using CDR.Entities.Dtos;
using CDR.Shared.Utilities.Results.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CDR.Services.Abstract
{
    public interface ILocalizationCultureService
    {
        Task<IDataResult<LocalizationCultureDto>> GetAsync(int localizationCultureId);
        Task<IDataResult<LocalizationCultureDto>> GetByCultureAsync(string localizationCulture);
        Task<IDataResult<LocalizationCultureListDto>> GetAllAsync();
    }
}
