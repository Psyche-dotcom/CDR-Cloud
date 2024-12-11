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
    public class SupportManager : ISupportService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IStaticService _staticService;

        public SupportManager(IUnitOfWork unitOfWork, IStaticService staticService)
        {
            _unitOfWork = unitOfWork;
            _staticService = staticService;
        }

        public async Task<IDataResult<SupportAddReturnDto>> AddAsync(SupportAddDto supportAddDto, User User)
        {
            var categories = await _unitOfWork.SupportCategories.GetAsync(a => a.Id == supportAddDto.SupportCategoryId);
            if (categories == null)
                return new DataResult<SupportAddReturnDto>(ResultStatus.Error, _staticService.GetLocalization("DBO_GecerliDestekKategorisiSeciniz").Data, null);

            if (await _unitOfWork.Supports.AnyAsync(x => x.UserId == User.Id && x.IsActive && x.Statue == (byte)Enums.SupportStatue.WAITING))
                return new DataResult<SupportAddReturnDto>(ResultStatus.Error, _staticService.GetLocalization("DBO_DestekBekliyorMesaji").Data, null);

            var added = await _unitOfWork.Supports.AddAsync(new Support
            {
                PublicId = Guid.NewGuid(),
                UserId = User.Id,
                CategoryId = supportAddDto.SupportCategoryId ?? 0,
                Statue = (byte)Enums.SupportStatue.WAITING
            });

            await _unitOfWork.SaveAsync();

            var addedMessage = await _unitOfWork.SupportMessages.AddAsync(new SupportMessages
            {
                UserId = User.Id,
                IsAdmin = false,
                PublicId = Guid.NewGuid(),
                SupportId = added.Id,
                Text = supportAddDto.SupportMessage,
                IsSeenAdmin = false,
                IsSeenUser = true
            });

            await _unitOfWork.SaveAsync();

            return new DataResult<SupportAddReturnDto>(ResultStatus.Success, _staticService.GetLocalization("DBO_DestekOlusturuldu").Data, new SupportAddReturnDto
            {
                SupportNumber = added.Id + 100000,
                CategoryName = categories.Name,
                Message = supportAddDto.SupportMessage
            });
        }

        public async Task<IDataResult<SupportListDto>> GetAllAsync(int UserId)
        {
            var supports = await _unitOfWork.Supports.GetAllAsync(x => x.UserId == UserId, x => x.SupportMessages, x => x.Category);
            if (supports.Count > 0)
            {
                supports = supports.OrderByDescending(x => x.CreatedDate).ToList();

                return new DataResult<SupportListDto>(ResultStatus.Success, new SupportListDto
                {
                    Supports = supports,
                });
            }
            return new DataResult<SupportListDto>(ResultStatus.Error, "", new SupportListDto
            {
                Supports = null,
            });
        }

        public async Task<IDataResult<Support>> GetAsync(Guid PublicId)
        {
            var support = await _unitOfWork.Supports.GetAsync(x => x.PublicId == PublicId, x => x.Category);
            if (support != null)
            {
                return new DataResult<Support>(ResultStatus.Success, support);
            }
            return new DataResult<Support>(ResultStatus.Error, _staticService.GetLocalization("DBO_DestekBilgiBulunamadi").Data, null);
        }

        public async Task<IDataResult<SupportMessageListDto>> GetAllMessagesAsync(Guid PublicId)
        {
            var support = await _unitOfWork.Supports.GetAsync(x => x.PublicId == PublicId);

            if (support == null)
                return new DataResult<SupportMessageListDto>(ResultStatus.Error, _staticService.GetLocalization("DBO_DestekBilgiBulunamadi").Data, null);

            var supportMessages = await _unitOfWork.SupportMessages.GetAllAsync(x => x.SupportId == support.Id);
            if (supportMessages.Count > 0)
            {
                var newMessages = new List<int>();

                if (support.Statue == (byte)Enums.SupportStatue.WAITING)
                {
                    var notSeenUser = supportMessages.Where(x => !x.IsSeenUser).ToList();

                    if (notSeenUser.Count > 0)
                    {
                        foreach (var item in notSeenUser)
                        {
                            newMessages.Add(item.Id);
                            item.IsSeenUser = true;
                            await _unitOfWork.SupportMessages.UpdateAsync(item);
                        }

                        await _unitOfWork.SaveAsync();
                    }
                }

                return new DataResult<SupportMessageListDto>(ResultStatus.Success, new SupportMessageListDto
                {
                    SupportMessages = supportMessages.OrderByDescending(x => x.CreatedDate).ToList(),
                    NewMessages = newMessages
                });
            }
            return new DataResult<SupportMessageListDto>(ResultStatus.Error, "", new SupportMessageListDto
            {
                SupportMessages = null,
            });
        }

        public async Task<IResult> AddMessageAsync(Guid PublicId, string Text, int UserId)
        {
            var support = await _unitOfWork.Supports.GetAsync(x => x.PublicId == PublicId);

            if (support == null)
                return new Result(ResultStatus.Error, _staticService.GetLocalization("DBO_DestekBilgiBulunamadi").Data);

            if (support.Statue != (byte)Enums.SupportStatue.WAITING)
                return new Result(ResultStatus.Error, _staticService.GetLocalization("CDR_SupportFinished").Data);

            var added = await _unitOfWork.SupportMessages.AddAsync(new SupportMessages
            {
                UserId = UserId,
                IsAdmin = false,
                SupportId = support.Id,
                Text = Text,
                IsSeenAdmin = false,
                IsSeenUser = true
            });

            await _unitOfWork.SaveAsync();

            return new Result(ResultStatus.Success, _staticService.GetLocalization("DBO_DestegeMesajGonderildi").Data);
        }

        public async Task<IResult> GetBadgeAsync(int UserId)
        {
            var support = await _unitOfWork.Supports.GetAsync(x => x.UserId == UserId && x.Statue == (byte)Enums.SupportStatue.WAITING);

            if (support != null)
            {
                var supportMessagesNoSeen = await _unitOfWork.SupportMessages.AnyAsync(x => !x.IsSeenUser);

                return new Result(supportMessagesNoSeen ? ResultStatus.Success : ResultStatus.Error, "");
            }

            return new Result(ResultStatus.Error, "");
        }

        public async Task<IResult> AnyWaitingAsync(int UserId)
        {
            var isThereWaitingSupport = await _unitOfWork.Supports.AnyAsync(x => x.UserId == UserId && x.Statue == (byte)Enums.SupportStatue.WAITING);

            return new Result(isThereWaitingSupport ? ResultStatus.Success : ResultStatus.Error, "");
        }
    }
}
