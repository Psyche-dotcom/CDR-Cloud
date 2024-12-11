using CDR.Data.Abstract;
using CDR.Data.Concrete.EntityFramework.Contexts;
using CDR.Data.Concrete.EntityFramework.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CDR.Data.Concrete
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly CdrContext _context;
        private EfCityRepository _cityRepository;
        private EfContentRepository _contentRepository;
        private EfCountryRepository _countryRepository;
        private EfDepositRepository _depositRepository;
        private EfHelpRepository _helpRepository;
        private EfLocalizationRepository _localizationRepository;
        private EfLocalizationCultureRepository _localizationCultureRepository;
        private EfPackageRepository _packageRepository;
        private EfPermissionsRepository _permissionsRepository;
        private EfQueriesRepository _queriesRepository;
        private EfRatingRepository _ratingRepository;
        private EfReportFilterRepository _reportFilterRepository;
        private EfSupportRepository _supportRepository;
        private EfSupportCategoriesRepository _supportCategoriesRepository;
        private EfSupportMessagesRepository _supportMessagesRepository;
        private EfTransactionRepository _transactionRepository;
        private EfTransactionDetailRepository _transactionDetailRepository;
        private EfUserActivePackagesRepository _userActivePackagesRepository;
        private EfUserPermissionsRepository _userPermissionsRepository;
        private EfPromotionUsageRepository _promotionUsageRepository;

        public UnitOfWork(CdrContext context)
        {
            _context = context;
        }

        public ICityRepository Cities => _cityRepository ??= new EfCityRepository(_context);
        public IContentRepository Contents => _contentRepository ??= new EfContentRepository(_context);
        public ICountryRepository Countries => _countryRepository ??= new EfCountryRepository(_context);
        public IDepositRepository Deposits => _depositRepository ??= new EfDepositRepository(_context);
        public IHelpRepository Helps => _helpRepository ??= new EfHelpRepository(_context);
        public ILocalizationRepository Localizations => _localizationRepository ??= new EfLocalizationRepository(_context);
        public ILocalizationCultureRepository LocalizationCultures => _localizationCultureRepository ??= new EfLocalizationCultureRepository(_context);
        public IPackageRepository Packages => _packageRepository ??= new EfPackageRepository(_context);
        public IPermissionsRepository Permissions => _permissionsRepository ??= new EfPermissionsRepository(_context);
        public IQueriesRepository Queriess => _queriesRepository ??= new EfQueriesRepository(_context);
        public IRatingRepository Ratings => _ratingRepository ??= new EfRatingRepository(_context);
        public IReportFilterRepository ReportFilters => _reportFilterRepository ??= new EfReportFilterRepository(_context);
        public ISupportRepository Supports => _supportRepository ??= new EfSupportRepository(_context);
        public ISupportCategoriesRepository SupportCategories => _supportCategoriesRepository ??= new EfSupportCategoriesRepository(_context);
        public ISupportMessagesRepository SupportMessages => _supportMessagesRepository ??= new EfSupportMessagesRepository(_context);
        public ITransactionRepository Transactions => _transactionRepository ??= new EfTransactionRepository(_context);
        public ITransactionDetailRepository TransactionDetails => _transactionDetailRepository ??= new EfTransactionDetailRepository(_context);
        public IUserActivePackagesRepository UserActivePackages => _userActivePackagesRepository ??= new EfUserActivePackagesRepository(_context);
        public IUserPermissionsRepository UserPermissions => _userPermissionsRepository ??= new EfUserPermissionsRepository(_context);
        public IPromotionUsageRepository PromotionUsage => _promotionUsageRepository ??= new EfPromotionUsageRepository(_context);

        public async Task<int> SaveAsync()
        {
            return await _context.SaveChangesAsync();
        }

        public async ValueTask DisposeAsync()
        {
            await _context.DisposeAsync();
        }
    }
}
