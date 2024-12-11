using CDR.Entities.Concrete;
using CDR.Entities.Dtos;
using CDR.Shared.Utilities.Results.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CDR.Services.Abstract
{
    public interface IDepositService
    {
        Task<IDataResult<Deposit>> GetAsync(string publicId);
        Task<IDataResult<Guid>> AddAsync(DepositAddDto depositAddDto);
        Task<IDataResult<IList<Deposit>>> GetAllWithUserAsync(int userId);
    }
}
