using CDR.Entities.Concrete;

namespace CDR.API.Api.Model
{
    public class MembershipBuyPackageModel
    {
        public IList<Package> PackageList { get; set; }
        public User User { get; set; }
        public string DistanceSellingContract { get; set; }
        public string PrivacyAgreement { get; set; }
        public IList<Country> Countries { get; set; }
    }
}
