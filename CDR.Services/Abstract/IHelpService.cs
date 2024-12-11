using CDR.Entities.Dtos;
using CDR.Shared.Utilities.Results.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CDR.Services.Abstract
{
    public interface IHelpService
    {
        Task<IDataResult<HelpListDto>> GetAllAsync();
        Task<IDataResult<HelpListDto>> GetAllByPageTypeAsync(byte PageType);
    }
}
