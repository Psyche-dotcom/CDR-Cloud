using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CDR.Entities.Dtos
{
    public class UserConnectionDetailDto
    {
        [DisplayName("Ip Address / FQDN")]
        [Required(ErrorMessage = "RequiredMessage")]
        public string IpAddress { get; set; }
        public string Port { get; set; }
        public string DbName { get; set; } = "database_single";
        public string DbUsername { get; set; } = "phonesystem";
        public string DbPassword { get; set; }
        public string? ModelError { get; set; }

        [DisplayName("Version")]
        [Required(ErrorMessage = "RequiredMessage")]
        public byte Version { get; set; }

    }
}
