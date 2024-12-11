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
    public class ContentManager : IContentService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILocalizationCultureService _localizationCultureService;

        public ContentManager(IUnitOfWork unitOfWork, ILocalizationCultureService localizationCultureService)
        {
            _unitOfWork = unitOfWork;
            _localizationCultureService = localizationCultureService;
        }

        public async Task<IDataResult<ContentDto>> GetAsync(Enums.Content Type)
        {
            var getCulture = System.Globalization.CultureInfo.CurrentCulture.Name;

            var culture = await _localizationCultureService.GetByCultureAsync(getCulture);

            var content = await _unitOfWork.Contents.GetAsync(c => c.ContentType == (byte)Type && c.LocalizationCultureId == culture.Data.LocalizationCulture.Id);

            if (content != null)
            {
                return new DataResult<ContentDto>(ResultStatus.Success, new ContentDto
                {
                    Content = content,
                });
            }
            return new DataResult<ContentDto>(ResultStatus.Error, "", new ContentDto
            {
                Content = null,
            });
        }

        public async Task<IDataResult<ContentDto>> GetGlobalAsync(Enums.Content Type)
        {
            var culture = await _localizationCultureService.GetByCultureAsync("en-US");

            var content = await _unitOfWork.Contents.GetAsync(c => c.ContentType == (byte)Type && c.LocalizationCultureId == culture.Data.LocalizationCulture.Id);

            if (content != null)
            {
                return new DataResult<ContentDto>(ResultStatus.Success, new ContentDto
                {
                    Content = content,
                });
            }
            return new DataResult<ContentDto>(ResultStatus.Error, "", new ContentDto
            {
                Content = null,
            });
        }
    }
}
