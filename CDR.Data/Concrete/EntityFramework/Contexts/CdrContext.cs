using CDR.Data.Concrete.EntityFramework.Mappings;
using CDR.Entities.Concrete;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CDR.Data.Concrete.EntityFramework.Contexts
{
    public class CdrContext : IdentityDbContext<User, Role, int, UserClaim, UserRole, UserLogin, RoleClaim, UserToken>
    {
        public DbSet<City> City { get; set; }
        public DbSet<Content> Content { get; set; }
        public DbSet<Country> Country { get; set; }
        public DbSet<Deposit> Deposit { get; set; }
        public DbSet<Help> Help { get; set; }
        public DbSet<HelpDetails> HelpDetails { get; set; }
        public DbSet<Localization> Localization { get; set; }
        public DbSet<LocalizationCulture> LocalizationCulture { get; set; }
        public DbSet<Package> Package { get; set; }
        public DbSet<Permissions> Permissions { get; set; }
        public DbSet<Queries> Queries { get; set; }
        public DbSet<Rating> Rating { get; set; }
        public DbSet<ReportFilter> ReportFilter { get; set; }
        public DbSet<Support> Support { get; set; }
        public DbSet<SupportCategories> SupportCategories { get; set; }
        public DbSet<SupportMessages> SupportMessages { get; set; }
        public DbSet<Transaction> Transaction { get; set; }
        public DbSet<TransactionDetails> TransactionDetails { get; set; }
        public DbSet<UserActivePackages> UserActivePackages { get; set; }
        public DbSet<UserPermissions> UserPermissions { get; set; }
        public DbSet<Log> Logs { get; set; }
        public DbSet<AuthenticationTicket> AuthenticationTicket { get; set; }
        public DbSet<PromotionUsage> PromotionUsages { get; set; }


        public CdrContext(DbContextOptions<CdrContext> options) : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new CityMap());
            modelBuilder.ApplyConfiguration(new ContentMap());
            modelBuilder.ApplyConfiguration(new CountryMap());
            modelBuilder.ApplyConfiguration(new DepositMap());
            modelBuilder.ApplyConfiguration(new HelpMap());
            modelBuilder.ApplyConfiguration(new HelpDetailsMap());
            modelBuilder.ApplyConfiguration(new LocalizationMap());
            modelBuilder.ApplyConfiguration(new LocalizationCultureMap());
            modelBuilder.ApplyConfiguration(new PackageMap());
            modelBuilder.ApplyConfiguration(new PermissionsMap());
            modelBuilder.ApplyConfiguration(new QueriesMap());
            modelBuilder.ApplyConfiguration(new RatingMap());
            modelBuilder.ApplyConfiguration(new ReportFilterMap());
            modelBuilder.ApplyConfiguration(new SupportMap());
            modelBuilder.ApplyConfiguration(new SupportCategoriesMap());
            modelBuilder.ApplyConfiguration(new SupportMessagesMap());
            modelBuilder.ApplyConfiguration(new TransactionMap());
            modelBuilder.ApplyConfiguration(new TransactionDetailsMap());
            modelBuilder.ApplyConfiguration(new UserActivePackagesMap());
            modelBuilder.ApplyConfiguration(new UserPermissionsMap());
            modelBuilder.ApplyConfiguration(new RoleMap());
            modelBuilder.ApplyConfiguration(new UserMap());
            modelBuilder.ApplyConfiguration(new RoleClaimMap());
            modelBuilder.ApplyConfiguration(new UserClaimMap());
            modelBuilder.ApplyConfiguration(new UserLoginMap());
            modelBuilder.ApplyConfiguration(new UserRoleMap());
            modelBuilder.ApplyConfiguration(new UserTokenMap());
            modelBuilder.ApplyConfiguration(new LogMap());

            foreach (var relationship in modelBuilder.Model.GetEntityTypes().SelectMany(e => e.GetForeignKeys()))
            {
                relationship.DeleteBehavior = DeleteBehavior.Restrict;
            }
        }
    }
}
