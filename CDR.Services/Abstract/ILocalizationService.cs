using CDR.Entities.Dtos;
using CDR.Shared.Utilities.Results.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CDR.Services.Abstract
{
    public interface ILocalizationService
    {
        Task<IDataResult<LocalizationDto>> GetAsync(int localizationId);
        Task<IDataResult<LocalizationListDto>> GetAllAsync();
        Task<IDataResult<LocalizationListDto>> GetAllByCultureAsync(int cultureId);
    }
}
