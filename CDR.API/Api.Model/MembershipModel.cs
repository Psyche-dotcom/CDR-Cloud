using CDR.Entities.Concrete;

namespace CDR.API.Api.Model
{
    public class MembershipModel
    {
        public User User { get; set; }
        public IList<UserActivePackages> UserPackages { get; set; }
    }
}
