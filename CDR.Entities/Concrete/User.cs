using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;

namespace CDR.Entities.Concrete
{
    public class User : IdentityUser<int>
    {
        public override int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string IpAddress { get; set; }
        public string Port { get; set; }
        public string DbName { get; set; }
        public string DbUsername { get; set; }
        public string DbPassword { get; set; }
        public byte Version { get; set; } = 2;
        public string Timezone { get; set; }
        public decimal GMT { get; set; }
        public bool EmailActivation { get; set; }
        public int? CountryId { get; set; }
        public long? CityId { get; set; }
        public string Address { get; set; }
        public string ZipCode { get; set; }
        public int? SimultaneousCalls { get; set; }
        public string Edition { get; set; }
        public string CompanyName { get; set; }
        public DateTime PackageFinishDate { get; set; }
        public DateTime CreatedDate { get; set; }
        public ICollection<UserPermissions> UserPermissions { get; set; }
        public ICollection<Deposit> Deposits { get; set; }
        public ICollection<UserActivePackages> ActivePackages { get; set; }

    }
}
