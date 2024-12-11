using CDR.Entities.Concrete;
using CDR.Entities.Dtos;
using CDR.Shared.Utilities.Results.Abstract;
using CDR.Shared.Utilities.Results.ComplexTypes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CDR.Services.Abstract
{
    public interface IQueriesService
    {
        Task<IDataResult<QueriesDto>> GetAsync(User UserDetail, Enums.ConsoleQueryType Type);
    }
}
