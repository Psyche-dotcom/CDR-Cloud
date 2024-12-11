using CDR.Data.Abstract;
using CDR.Entities.Concrete;
using CDR.Entities.Dtos;
using CDR.Services.Abstract;
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
    public class DepositManager : IDepositService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IStaticService _staticService;

        public DepositManager(IUnitOfWork unitOfWork, IStaticService staticService)
        {
            _unitOfWork = unitOfWork;
            _staticService = staticService;
        }

        public async Task<IDataResult<Guid>> AddAsync(DepositAddDto depositAddDto)
        {
            Guid publiId = new Guid();
            try
            {
                publiId = new Guid(depositAddDto.PackagePublicId);
            }
            catch (Exception)
            {
            }

            var package = await _unitOfWork.Packages.GetAsync(a => a.PublicId == publiId);
            if (package == null)
                return new DataResult<Guid>(ResultStatus.Error, _staticService.GetLocalization("DBO_SatinAlmakIstediginizPaketHatasi").Data, Guid.NewGuid());

            if (package.SimultaneousCalls > depositAddDto.User.SimultaneousCalls)
                return new DataResult<Guid>(ResultStatus.Error, _staticService.GetLocalization("DBO_SecilenPaketiSatinAlamazsiniz").Data, Guid.NewGuid());

            var added = await _unitOfWork.Deposits.AddAsync(new Deposit
            {
                PackageId = package.Id,
                UserId = depositAddDto.User.Id,
                PublicId = Guid.NewGuid()
            });

            await _unitOfWork.SaveAsync();

            return new DataResult<Guid>(ResultStatus.Success, "", added.PublicId);
        }

        public async Task<IDataResult<Deposit>> GetAsync(string publicId)
        {
            Guid depositPublicId = new Guid();
            try
            {
                depositPublicId = new Guid(publicId);
            }
            catch (Exception)
            {
            }

            var deposit = await _unitOfWork.Deposits.GetAsync(c => c.PublicId == depositPublicId, c => c.Package, c => c.User);
            if (deposit != null)
            {
                return new DataResult<Deposit>(ResultStatus.Success, deposit);
            }
            return new DataResult<Deposit>(ResultStatus.Error, _staticService.GetLocalization("DBO_BirHataOlustu").Data, null);
        }

        public async Task<IDataResult<IList<Deposit>>> GetAllWithUserAsync(int userId)
        {
            var deposit = await _unitOfWork.Deposits.GetAllAsync(c => c.UserId == userId, c => c.Package, c => c.User);
            if (deposit != null)
            {
                return new DataResult<IList<Deposit>>(ResultStatus.Success, deposit);
            }
            return new DataResult<IList<Deposit>>(ResultStatus.Error, _staticService.GetLocalization("DBO_BirHataOlustu").Data, null);
        }
    }
}
