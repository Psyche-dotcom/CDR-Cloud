using CDR.Data.Abstract;
using CDR.Entities.Concrete;
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
    public class QueriesManager : IQueriesService
    {
        private readonly IUnitOfWork _unitOfWork;
        public QueriesManager(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<IDataResult<QueriesDto>> GetAsync(User UserDetail, Enums.ConsoleQueryType Type)
        {
            var query = await _unitOfWork.Queriess
                .GetAsync(x =>
                x.IpAddress == UserDetail.IpAddress && 
                x.Port == UserDetail.Port && 
                x.DbName == UserDetail.DbName && 
                x.DbUsername == UserDetail.DbUsername && 
                x.DbPassword == UserDetail.DbPassword && 
                x.Type == (int)Type);

            if (query != null)
            {
                return new DataResult<QueriesDto>(ResultStatus.Success, new QueriesDto
                {
                    Queries = query,
                    ResultStatus = ResultStatus.Success
                });
            }
            return new DataResult<QueriesDto>(ResultStatus.Error, Messages.Queries.NotFound(isPlural: false), null);
        }
    }
}
