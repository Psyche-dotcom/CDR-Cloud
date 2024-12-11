using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CDR.Entities.Dtos
{
    public class CompanyPhonebookUserDto
    {
        public int idphonebook { get; set; }
        public string firstname { get; set; }
        public string lastname { get; set; }
        public string phonenumber { get; set; }
        public string company { get; set; }
        public string email { get; set; }

        public string tableemail
        {
            get
            {
                if (string.IsNullOrWhiteSpace(this.email))
                    return "-";
                else
                    return "<img src=\"/app-assets/images/icons/ext-mail.svg\" class=\"mr-1\">" + this.email;
            }
        }

        public string tablephone
        {
            get
            {
                if (string.IsNullOrWhiteSpace(this.phonenumber))
                    return "-";
                else
                    return "<img src=\"/app-assets/images/icons/ext-phone.svg\" class=\"mr-1\">" + this.phonenumber;
            }
        }
        public string tablecompany
        {
            get
            {
                if (string.IsNullOrWhiteSpace(this.company))
                    return "-";
                else
                    return "<img src=\"/app-assets/images/icons/bag-menu.svg\" class=\"mr-1\">" + this.company;
            }
        }
    }
}
