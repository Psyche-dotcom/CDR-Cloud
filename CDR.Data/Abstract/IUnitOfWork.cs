using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CDR.Data.Abstract
{
    public interface IUnitOfWork : IAsyncDisposable
    {
        ICityRepository Cities { get; }
        IContentRepository Contents { get; }
        ICountryRepository Countries { get; }
        IDepositRepository Deposits { get; }
        IHelpRepository Helps { get; }
        ILocalizationRepository Localizations { get; }
        ILocalizationCultureRepository LocalizationCultures { get; }
        IPackageRepository Packages { get; }
        IPermissionsRepository Permissions { get; }
        IQueriesRepository Queriess { get; }
        IRatingRepository Ratings { get; }
        IReportFilterRepository ReportFilters { get; }
        ISupportRepository Supports { get; }
        ISupportCategoriesRepository SupportCategories { get; }
        ISupportMessagesRepository SupportMessages { get; }
        ITransactionRepository Transactions { get; }
        ITransactionDetailRepository TransactionDetails { get; }
        IUserActivePackagesRepository UserActivePackages { get; }
        IUserPermissionsRepository UserPermissions { get; }
        IPromotionUsageRepository PromotionUsage { get; }


        Task<int> SaveAsync();
    }
}
