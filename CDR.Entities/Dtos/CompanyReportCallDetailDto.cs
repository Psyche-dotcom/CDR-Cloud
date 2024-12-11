using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CDR.Entities.Dtos
{
    public class CompanyReportCallDetailDto
    {
        public int call_id { get; set; }
        public string from { get; set; }
        public string to { get; set; }
        public TimeSpan duration { get; set; }
        public TimeSpan talktime { get; set; }
        public TimeSpan ringtime { get; set; }
        public int year { get; set; }
        public int month { get; set; }
        public int day { get; set; }
        public TimeSpan starttime { get; set; }
        public TimeSpan stoptime { get; set; }
        public string inorout { get; set; }
        public string status { get; set; }

        public DateTime Date
        {
            get
            {
                return new DateTime(year, month, day);
            }
        }

        public string AudioUrl { get; set; }

        private string _tablecolumninorout;
        public string tablecolumninorout
        {
            get
            {
                string _result = string.Empty;

                if (string.IsNullOrWhiteSpace(_tablecolumninorout))
                {

                    if (this.inorout.Equals("inbound"))
                    {
                        _result = "<span class=\"badge badge-success\">CDR_ReportInbound</span>";
                    }
                    else if (this.inorout.Equals("outbound"))
                    {
                        _result = "<span class=\"badge badge-danger\">CDR_ReportOutbound</span>";
                    }
                    else if (this.inorout.Equals("internal"))
                    {
                        _result = "<span class=\"badge badge-warning\">CDR_ReportInternal</span>";
                    }

                    _tablecolumninorout = _result;
                }

                return _tablecolumninorout;
            }
            set
            {
                this._tablecolumninorout = value;
            }

        }

        private string _tablecolumnstatus;
        public string tablecolumnstatus
        {
            get
            {
                string _result = string.Empty;

                if (string.IsNullOrWhiteSpace(_tablecolumnstatus))
                {

                    if (this.status.Equals("completed"))
                    {
                        _result = "<span class=\"badge badge-completed\"><i class=\"la la-check\" style=\"font-size: unset;\"></i> CDR_ReportCompleted</span>";
                    }
                    else if (this.status.Equals("missed"))
                    {
                        _result = "<span class=\"badge badge-missed\"><i class=\"las la-phone-slash\" style=\"font-size: unset;\"></i> CDR_ReportMissed</span>";
                    }
                    else if (this.status.Equals("transferred"))
                    {
                        _result = "<span class=\"badge badge-transferred\"><i class=\"las la-exchange-alt\" style=\"font-size: unset;\"></i> CDR_ReportTransferred</span>";
                    }

                    _tablecolumnstatus = _result;
                }

                return _tablecolumnstatus;
            }
            set
            {
                this._tablecolumnstatus = value;
            }
        }

        public string tablecolumnduration
        {
            get
            {
                string _result = "<div class=\"table-column-duration\">" + "<i class=\"la la-clock\"></i> " + this.duration.ToString() + "</div>";

                return _result;
            }

        }

        public string tablecolumntalktime
        {
            get
            {
                string _result = "<div class=\"table-column-talktime\">" + "<i class=\"la la-clock\"></i> " + this.talktime.ToString() + "</div>";

                return _result;
            }

        }

        public string tablecolumnringtime
        {
            get
            {
                string _result = "<div class=\"table-column-ringtime\">" + "<i class=\"la la-clock\"></i> " + this.ringtime.ToString() + "</div>";

                return _result;
            }

        }

        public string tablecolumndate
        {
            get
            {
                string _day = this.day < 10 ? "0" + this.day : this.day.ToString();
                string _month = this.month < 10 ? "0" + this.month : this.month.ToString();

                string _compare = _day + "." + _month + "." + this.year;

                string _result = "<div class=\"table-column-date\">" + "<img src=\"/app-assets/images/icons/filter-calendar.svg\"> " + _compare + "</div>";

                return _result;
            }

        }

        public string tablecolumnstarttime
        {
            get
            {
                string _result = "<div class=\"table-column-starttime\">" + "<i class=\"la la-clock\"></i> " + (this.starttime.ToString().Contains(".") ? this.starttime.ToString().Split(".")[1].ToString() : this.starttime.ToString()) + "</div>";

                return _result;
            }

        }

        public string tablecolumnstoptime
        {
            get
            {
                string _result = "<div class=\"table-column-stoptime\">" + "<i class=\"la la-clock\"></i> " + (this.stoptime.ToString().Contains(".") ? this.stoptime.ToString().Split(".")[1].ToString() : this.stoptime.ToString()) + "</div>";

                return _result;
            }

        }

        public string tablecolumnfrom
        {
            get
            {
                string _result = "<div class=\"table-column-from\">" + "<img src=\"/app-assets/images/icons/user-from.svg\"> " + this.from + "</div>";

                return _result;
            }

        }

        public string tablecolumnto
        {
            get
            {
                string _result = "<div class=\"table-column-to\">" + "<img src=\"/app-assets/images/icons/user-to.svg\"> " + this.to + "</div>";

                return _result;
            }

        }
    }
}
