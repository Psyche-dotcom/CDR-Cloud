using AutoMapper;
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
    public class ReportFilterManager : IReportFilterService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IStaticService _staticService;

        public ReportFilterManager(IUnitOfWork unitOfWork, IMapper mapper, IStaticService staticService)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _staticService = staticService;
        }

        public async Task<IResult> AddAsync(ReportFavoriteFilterAddDto reportFavoriFilterAddDto)
        {
            var reportFavoriteFilter = _mapper.Map<ReportFilter>(reportFavoriFilterAddDto);
            var isThere = await _unitOfWork.ReportFilters.AnyAsync(x => x.UserId == reportFavoriteFilter.UserId && x.Title.Equals(reportFavoriteFilter.Title));

            if (isThere)
                return new Result(ResultStatus.Success, _staticService.GetLocalization("DBO_FavoriBasligiBulunmaktadir").Data);

            var addedReportFavoriteFilter = await _unitOfWork.ReportFilters.AddAsync(reportFavoriteFilter);
            await _unitOfWork.SaveAsync();

            return new Result(ResultStatus.Success, _staticService.GetLocalization("DBO_FavoriEklendi").Data);
        }

        public async Task<IDataResult<ReportFavoriteFilterListDto>> GetAllAsync(int UserId)
        {
            var reportFavoriteFilters = await _unitOfWork.ReportFilters.GetAllAsync(x => x.UserId == UserId);
            if (reportFavoriteFilters.Count > -1)
            {
                return new DataResult<ReportFavoriteFilterListDto>(ResultStatus.Success, new ReportFavoriteFilterListDto
                {
                    ReportFilters = reportFavoriteFilters,
                });
            }

            return new DataResult<ReportFavoriteFilterListDto>(ResultStatus.Error, "", new ReportFavoriteFilterListDto
            {
                ReportFilters = null,
            });
        }

        public async Task<IDataResult<ReportFilter>> GetAsync(Guid publicId)
        {
            throw new NotImplementedException();
        }

        public async Task<IResult> DeleteAsync(int Id, int UserId)
        {
            throw new NotImplementedException();
        }

        public async Task<IResult> HardDeleteAsync(int Id, int UserId)
        {
            var reportFavoriteFilter = await _unitOfWork.ReportFilters.GetAsync(c => c.Id == Id && c.UserId == UserId);

            if (reportFavoriteFilter != null)
            {
                await _unitOfWork.ReportFilters.DeleteAsync(reportFavoriteFilter);
                await _unitOfWork.SaveAsync();
                return new Result(ResultStatus.Success, _staticService.GetLocalization("DBO_FavoriSilindi").Data);
            }

            return new Result(ResultStatus.Error, _staticService.GetLocalization("DBO_SilinecekFavoriFiltrelemesiBulunamadi").Data);
        }
    }
}
