using CDR.Entities.Concrete;
using CDR.Entities.Dtos;
using CDR.Shared.Utilities.Results.Abstract;
using CDR.Shared.Utilities.Results.ComplexTypes;
using Microsoft.Extensions.Caching.Memory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CDR.Services.Abstract
{
    public interface IStaticService
    {
        public IList<Localization> Localizations { get; set; }

        IDataResult<IList<Localization>> GetAllLocalization();
        Task<IDataResult<IList<Localization>>> SetLocalizationAsync();
        IDataResult<string> GetLocalization(string ResourceKey);

        IDataResult<IList<LocalizationCulture>> GetAllLocalizationCulture();
        Task<IDataResult<IList<LocalizationCulture>>> SetLocalizationCultureAsync();
        IDataResult<LocalizationCulture> GetLocalizationCulture(int Id);

        IDataResult<IList<Country>> GetAllCountry();
        Task<IDataResult<IList<Country>>> SetCountryAsync();
        IDataResult<Country> GetCountry(int Id);

        IDataResult<IList<SupportCategories>> GetAllSupportCategories();
        Task<IDataResult<IList<SupportCategories>>> SetSupportCategoriesAsync();
        IDataResult<SupportCategories> GetSupportCategories(int Id);

        Task<IDataResult<IList<Queries>>> SetQueriesAsync();
        IDataResult<string> GetQueries(User User, Enums.ConsoleQueryType consoleQueryType);
    }
}
