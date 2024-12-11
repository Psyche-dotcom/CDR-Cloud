using CDR.Data.Abstract;
using CDR.Data.Concrete;
using CDR.Data.Concrete.EntityFramework.Contexts;
using CDR.Entities.Concrete;
using CDR.Services.Abstract;
using CDR.Services.Concrete;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace CDR.Services.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection LoadServices(this IServiceCollection serviceCollection, IConfiguration configuration)
        {
            var connectionString = configuration.GetConnectionString("DefaultConnection");

            serviceCollection.AddDbContext<CdrContext>(options => options.UseSqlServer(connectionString).UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking));
            serviceCollection.AddIdentity<User, Role>(options =>
            {
                // User Password Options
                options.Password.RequireDigit = false;
                options.Password.RequiredLength = 5;
                options.Password.RequiredUniqueChars = 0;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequireLowercase = false;
                options.Password.RequireUppercase = false;
                // User Username and Email Options
                options.User.AllowedUserNameCharacters = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789-._@+$";
                options.User.RequireUniqueEmail = true;
                options.SignIn.RequireConfirmedEmail = true;
            }).AddEntityFrameworkStores<CdrContext>().AddDefaultTokenProviders();
            serviceCollection.Configure<SecurityStampValidatorOptions>(options =>
            {
                options.ValidationInterval = TimeSpan.FromMinutes(15);
            });
            serviceCollection.AddMemoryCache();
            serviceCollection.AddScoped<IUnitOfWork, UnitOfWork>();
            serviceCollection.AddScoped<IQueriesService, QueriesManager>();
            serviceCollection.AddScoped<ILocalizationService, LocalizationManager>();
            serviceCollection.AddScoped<ILocalizationCultureService, LocalizationCultureManager>();
            serviceCollection.AddScoped<ICountryService, CountryManager>();
            serviceCollection.AddScoped<ICityService, CityManager>();
            serviceCollection.AddScoped<IStaticService, StaticManager>();
            serviceCollection.AddScoped<IPostgreSqlService, PostgreSqlManager>();
            serviceCollection.AddScoped<IReportFilterService, ReportFilterManager>();
            serviceCollection.AddScoped<IPackageService, PackageManager>();
            serviceCollection.AddScoped<IUserActivePackagesService, UserActivePackageManager>();
            serviceCollection.AddScoped<IApi3cxService, Api3cxManager>();
            serviceCollection.AddScoped<IContentService, ContentManager>();
            serviceCollection.AddScoped<IDepositService, DepositManager>();
            serviceCollection.AddScoped<ITransactionService, TransactionManager>();
            serviceCollection.AddScoped<ISupportService, SupportManager>();
            serviceCollection.AddScoped<IMailService, MailManager>();
            serviceCollection.AddScoped<IHelpService, HelpManager>();

          
          

            return serviceCollection;
        }
    }
}
