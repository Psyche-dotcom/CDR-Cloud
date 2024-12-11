using CDR.Data.Abstract;
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
    public class HelpManager : IHelpService
    {
        private readonly IUnitOfWork _unitOfWork;

        public HelpManager(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<IDataResult<HelpListDto>> GetAllAsync()
        {
            var helps = await _unitOfWork.Helps.GetAllAsync(x=>x.IsActive, x => x.HelpDetails);
            if (helps.Count > 0)
            {
                var list = helps.Select(x => new HelpDto
                {
                    PageType = x.PageType,
                    LocalizationKey = x.LocalizationKey,
                    HelpDetails = x.HelpDetails.Select(y => new HelpDetailDto
                    {
                        Element = y.Element,
                        LocalizationKey = y.LocalizationKey,
                        JsText = y.JsText,
                        Type = y.Type
                    }).ToList()
                }).ToList();

                return new DataResult<HelpListDto>(ResultStatus.Success, new HelpListDto
                {
                    Helps = list
                });
            }
            return new DataResult<HelpListDto>(ResultStatus.Error, "", new HelpListDto
            {
                Helps = null,
            });
        }

        public async Task<IDataResult<HelpListDto>> GetAllByPageTypeAsync(byte PageType)
        {
            var helps = await _unitOfWork.Helps.GetAllAsync(x => x.PageType == PageType && x.IsActive, x => x.HelpDetails);
            if (helps.Count > 0)
            {
                var list = helps.Select(x => new HelpDto
                {
                    PageType = x.PageType,
                    LocalizationKey = x.LocalizationKey,
                    HelpDetails = x.HelpDetails.Select(y => new HelpDetailDto
                    {
                        Element = y.Element,
                        LocalizationKey = y.LocalizationKey,
                        JsText = y.JsText,
                        Type = y.Type
                    }).ToList()
                }).ToList();

                return new DataResult<HelpListDto>(ResultStatus.Success, new HelpListDto
                {
                    Helps = list
                });
            }
            return new DataResult<HelpListDto>(ResultStatus.Error, "", new HelpListDto
            {
                Helps = null,
            });
        }
    }
}
