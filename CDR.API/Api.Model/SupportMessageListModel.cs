using CDR.Entities.Concrete;

namespace CDR.API.Api.Model
{
    public class SupportMessageListModel
    {
        public IList<SupportMessages> SupportMessages { get; set; }
        public User User { get; set; }
        public IList<int> NewMessages { get; set; }
    }
}
