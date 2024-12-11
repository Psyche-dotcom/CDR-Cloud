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
    public interface ISupportService
    {
        Task<IDataResult<SupportAddReturnDto>> AddAsync(SupportAddDto supportAddDto, User User);
        Task<IDataResult<SupportListDto>> GetAllAsync(int UserId);
        Task<IDataResult<Support>> GetAsync(Guid PublicId);
        Task<IDataResult<SupportMessageListDto>> GetAllMessagesAsync(Guid PublicId);
        Task<IResult> AddMessageAsync(Guid PublicId,string Text,int UserId);
        Task<IResult> GetBadgeAsync(int UserId);
        Task<IResult> AnyWaitingAsync(int UserId);
    }
}
