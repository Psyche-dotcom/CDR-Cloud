using CDR.Data.Abstract;
using CDR.Entities.Concrete;
using CDR.Entities.Dtos;
using CDR.Services.Abstract;
using CDR.Shared.Utilities.Results.Abstract;
using CDR.Shared.Utilities.Results.ComplexTypes;
using CDR.Shared.Utilities.Results.Concrete;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Caching.Memory;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Log = Serilog.Log;

namespace CDR.Services.Concrete
{
    public class StaticManager : IStaticService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMemoryCache _memoryCache;
        private readonly MemoryCacheEntryOptions memoryCacheEntryOptions;
        public IList<Localization> Localizations { get; set; }
        public IList<LocalizationCulture> LocalizationCultures { get; set; }
        public IList<Country> Countries { get; set; }
        public IList<City> Cities { get; set; }
        public IList<SupportCategories> SupportCategories { get; set; }

        public StaticManager(IUnitOfWork unitOfWork, IMemoryCache memoryCache)
        {
            _unitOfWork = unitOfWork;
            _memoryCache = memoryCache;
            memoryCacheEntryOptions = new MemoryCacheEntryOptions
            {
                SlidingExpiration = TimeSpan.FromHours(2),
                AbsoluteExpirationRelativeToNow = TimeSpan.FromHours(2),
            };
        }

        public IDataResult<IList<Localization>> GetAllLocalization()
        {
            var cachedData = _memoryCache.Get("Loc:List:List");

            if (cachedData == null)
            {
                var data = SetLocalizationAsync();

                Localizations = data.Result.Data;

                return data.Result;
            }

            Localizations = JsonConvert.DeserializeObject<IList<Localization>>(cachedData.ToString());

            return new DataResult<IList<Localization>>(ResultStatus.Success, Localizations);
        }
        public async Task<IDataResult<IList<Localization>>> SetLocalizationAsync()
        {
            var data = await _unitOfWork.Localizations.GetAllAsync(x=>x.IsActive);

            var _clear = data.Select(x => new
            {
                ResourceKey = x.ResourceKey.Replace("\r","").Replace("\n",""),
                Text = x.Text.Replace("\r", "").Replace("\n", ""),
                x.CultureId,
                x.Id
            }).ToList();

            var byteData = JsonConvert.SerializeObject(_clear);
            _memoryCache.Set("Loc:List:List", byteData, memoryCacheEntryOptions);

            var _group = (from x in _clear
                          group x by x.ResourceKey into A
                          select new LocalizationRedisDto
                          {
                              ResourceKey = A.Key.Replace("\r", "").Replace("\n", ""),
                              DataList = A.Select(x => new Localization
                              {
                                  Text = x.Text.Replace("\r", "").Replace("\n", ""),
                                  CultureId = x.CultureId,
                              }).ToList()
                          }).ToList();

            for (int i = 0; i < _group.Count; i++)
            {
                var byteDataForItem = JsonConvert.SerializeObject(_group[i]);
              
                _memoryCache.Set("Loc:Det:" + _group[i].ResourceKey, byteDataForItem, memoryCacheEntryOptions);
            }

            return new DataResult<IList<Localization>>(ResultStatus.Success, data);
        }
        public IDataResult<string> GetLocalization(string ResourceKey)
        {
            string data = string.Empty;

            var culture = System.Globalization.CultureInfo.CurrentCulture;

            if (LocalizationCultures == null || LocalizationCultures.Count == 0)
                GetAllLocalizationCulture();

            var _localizationCultureId = LocalizationCultures.Where(x => x.Culture.Equals(culture.Name)).Select(x => x.Id).FirstOrDefault();

            var _localizationCache = _memoryCache.Get("Loc:Det:" + ResourceKey);

            if (_localizationCache == null)
            {
                GetAllLocalization();

                return new DataResult<string>(ResultStatus.Success, Localizations.Where(x => x.ResourceKey == ResourceKey && x.CultureId == _localizationCultureId).Select(x => x.Text).FirstOrDefault());
            }
            else
            {
                var localization = JsonConvert.DeserializeObject<LocalizationRedisDto>(_localizationCache.ToString());

                return new DataResult<string>(ResultStatus.Success, localization.DataList.Where(x => x.CultureId == _localizationCultureId).Select(x => x.Text).FirstOrDefault());
            }
        }

        public IDataResult<IList<LocalizationCulture>> GetAllLocalizationCulture()
        {
            var cachedData = _memoryCache.Get("LocCultures");

            if (cachedData == null)
            {
                var data = SetLocalizationCultureAsync();

                LocalizationCultures = data.Result.Data;

                return data.Result;
            }

            LocalizationCultures = JsonConvert.DeserializeObject<IList<LocalizationCulture>>(cachedData.ToString());

            return new DataResult<IList<LocalizationCulture>>(ResultStatus.Success, LocalizationCultures);
        }
        public async Task<IDataResult<IList<LocalizationCulture>>> SetLocalizationCultureAsync()
        {
            var data = await _unitOfWork.LocalizationCultures.GetAllAsync();

            var _clear = data.Select(x => new
            {
                x.Id,
                x.Title,
                x.Culture,
                x.FlagIcon
            });

            var byteData = JsonConvert.SerializeObject(_clear);
            _memoryCache.Set("LocCultures", byteData, memoryCacheEntryOptions);

            return new DataResult<IList<LocalizationCulture>>(ResultStatus.Success, data);
        }
        public IDataResult<LocalizationCulture> GetLocalizationCulture(int Id)
        {
            if (LocalizationCultures.Count == 0)
                GetAllLocalizationCulture();

            if (Localizations.Count > 0)
                return new DataResult<LocalizationCulture>(ResultStatus.Success, LocalizationCultures.Where(x => x.Id == Id).FirstOrDefault() ?? new LocalizationCulture());
            else
                return new DataResult<LocalizationCulture>(ResultStatus.Error, new LocalizationCulture());
        }

        public IDataResult<IList<Country>> GetAllCountry()
        {
            var cachedData = _memoryCache.Get("Countries");

            if (cachedData == null)
            {
                var data = SetCountryAsync();

                Countries = data.Result.Data;

                return data.Result;
            }

            Countries = JsonConvert.DeserializeObject<IList<Country>>(cachedData.ToString());

            return new DataResult<IList<Country>>(ResultStatus.Success, Countries);
        }
        public async Task<IDataResult<IList<Country>>> SetCountryAsync()
        {
            var data = await _unitOfWork.Countries.GetAllAsync();

            var byteData = JsonConvert.SerializeObject(data);
            _memoryCache.Set("Countries", byteData, memoryCacheEntryOptions);

            return new DataResult<IList<Country>>(ResultStatus.Success, data);
        }
        public IDataResult<Country> GetCountry(int Id)
        {
            if (Countries.Count > 0)
                return new DataResult<Country>(ResultStatus.Success, Countries.Where(x => x.Id == Id).FirstOrDefault() ?? new Country());
            else
                return new DataResult<Country>(ResultStatus.Error, new Country());
        }

        public IDataResult<IList<SupportCategories>> GetAllSupportCategories()
        {
            var cachedData = _memoryCache.Get("SupportCategories");

            if (cachedData == null)
            {
                var data = SetSupportCategoriesAsync();

                SupportCategories = data.Result.Data;

                return data.Result;
            }

            SupportCategories = JsonConvert.DeserializeObject<IList<SupportCategories>>(cachedData.ToString());

            return new DataResult<IList<SupportCategories>>(ResultStatus.Success, SupportCategories);
        }
        public async Task<IDataResult<IList<SupportCategories>>> SetSupportCategoriesAsync()
        {
            var data = await _unitOfWork.SupportCategories.GetAllAsync(x=>x.IsActive && x.IsShown);

            var byteData = JsonConvert.SerializeObject(data);
        
            _memoryCache.Set("SupportCategories", byteData, memoryCacheEntryOptions);

            return new DataResult<IList<SupportCategories>>(ResultStatus.Success, data);
        }
        public IDataResult<SupportCategories> GetSupportCategories(int Id)
        {
            if (SupportCategories.Count > 0)
                return new DataResult<SupportCategories>(ResultStatus.Success, SupportCategories.Where(x => x.Id == Id).FirstOrDefault() ?? new SupportCategories());
            else
                return new DataResult<SupportCategories>(ResultStatus.Error, new SupportCategories());
        }

        public async Task<IDataResult<IList<Queries>>> SetQueriesAsync()
        {
            var data = await _unitOfWork.Queriess.GetAllAsync();

            if (data is null)
            {
                return new DataResult<IList<Queries>>(ResultStatus.Error, null);
            }

            var _group = (from x in data
                          group x by x.IpAddress into A
                          select new QueriesRedisDto
                          {
                              IpAddress = A.Key,
                              DataList = A.Select(x => new QueriesRedisDataListDto
                              {
                                  DbName = x.DbName,
                                  DbPassword = x.DbPassword,
                                  DbUsername = x.DbUsername,
                                  JsonData = x.JsonData,
                                  Port = x.Port,
                                  Type = x.Type
                              }).ToList()
                          }).ToList();

            for (int i = 0; i < _group.Count; i++)
            {
                var itemData = JsonConvert.SerializeObject(_group[i]);
                _memoryCache.Set("Queries:Detail:" + _group[i].IpAddress, itemData, memoryCacheEntryOptions);
            }

            return new DataResult<IList<Queries>>(ResultStatus.Success, data);
        }
        public IDataResult<string> GetQueries(User User, Enums.ConsoleQueryType consoleQueryType)
        {
            string data = string.Empty;

            var queriesCache = _memoryCache.Get("Queries:Detail:" + User.Id);

            if (queriesCache == null)
            {
                var queries = SetQueriesAsync().Result;

                if (queries.ResultStatus == ResultStatus.Error)
                {
                    return new DataResult<string>(ResultStatus.Error,null);
                }

                var result = new DataResult<string>(ResultStatus.Success,
                    queries.Data.Where(x => x.IpAddress == User.IpAddress &&
                    x.Port == User.Port &&
                    x.DbName == User.DbName &&
                    x.DbUsername == User.DbUsername &&
                    x.DbPassword == User.DbPassword &&
                    x.Type == (int)consoleQueryType).Select(x => x.JsonData).FirstOrDefault());

                return result;
            }
            else
            {
                var queries = JsonConvert.DeserializeObject<QueriesRedisDto>(queriesCache.ToString());

                var result = new DataResult<string>(ResultStatus.Success,
                    queries.DataList.Where(x => x.Port == User.Port &&
                    x.DbName == User.DbName &&
                    x.DbUsername == User.DbUsername &&
                    x.DbPassword == User.DbPassword &&
                    x.Type == (int)consoleQueryType).Select(x => x.JsonData).FirstOrDefault());

                return result;
            }
        }
    }
}
