using CDR.Entities.Dtos;
using CDR.Shared.Utilities.Results.ComplexTypes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CDR.Services.Utilities
{
    public static class PostgreSqlList
    {
        public static class CompanyPhonebook
        {
            public static string CreateCompanyPhonebook(CompanyPhonebookAddDto data)
            {
                return "select public.cdrphonebook_insert ('" +
                            data.FirstName + "', '" +
                            data.LastName + "', '" +
                            data.MobileNumber + "', '" +
                            data.Company + "','" +
                            data.Email + "')";
            }

            public static string UpdateCompanyPhonebook(CompanyPhonebookAddDto data)
            {
                return "select public.cdrphonebook_update ('" +
                    "" + data.idphonebook + "', '" +
                            data.FirstName + "', '" +
                            data.LastName + "', '" +
                            data.MobileNumber + "', '" +
                            data.Company + "','" +
                            data.Email + "')";
            }

            public static string CompanyPhonebookDetail(int Id)
            {
                return "select idphonebook, firstname, lastname, phonenumber, company, email from public.cdr_phonebookcdr where idphonebook = " + Id;
            }

            public static string DeleteCompanyPhonebook(int Id)
            {
                return "select public.cdrphonebook_clear (" + Id + ")";
            }

            public static string CompanyPhonebookList(int skip, int take, string NameSurname, string Email, string Phone)
            {
                var _sqlSearchTextNameSurname = string.IsNullOrWhiteSpace(NameSurname) ? "" : "(LOWER(firstname) LIKE '%" + NameSurname.ToLower() + "%' OR LOWER(lastname) LIKE '%" + NameSurname.ToLower() + "%')";
                var _sqlSearchTextEmail = string.IsNullOrWhiteSpace(Email) ? "" : "(LOWER(email) LIKE '%" + Email.ToLower() + "%')";
                var _sqlSearchTextPhone = string.IsNullOrWhiteSpace(Phone) ? "" : "(LOWER(phonenumber) LIKE '%" + (Phone.ToLower()) + "%')";

                var temp = _sqlSearchTextNameSurname + " " + (string.IsNullOrWhiteSpace(_sqlSearchTextEmail) ? _sqlSearchTextEmail : (string.IsNullOrWhiteSpace(_sqlSearchTextNameSurname) ? "" : " AND ") + _sqlSearchTextEmail) + " " + (string.IsNullOrWhiteSpace(_sqlSearchTextPhone) ? _sqlSearchTextPhone : (string.IsNullOrWhiteSpace(_sqlSearchTextEmail) ? "" : " AND ") + _sqlSearchTextPhone);

                return "select idphonebook, firstname, lastname, phonenumber, company, email FROM  public.cdr_phonebookcdr" + (string.IsNullOrWhiteSpace(temp) ? "" : " where " + temp) + " OFFSET " + skip + " ROWS FETCH NEXT " + take + " ROWS ONLY";
            }

            public static string CompanyPhonebookCount(string NameSurname, string Email, string Phone)
            {
                var _sqlSearchTextNameSurname = string.IsNullOrWhiteSpace(NameSurname) ? "" : "(LOWER(firstname) LIKE '%" + NameSurname.ToLower() + "%' OR LOWER(lastname) LIKE '%" + NameSurname.ToLower() + "%')";
                var _sqlSearchTextEmail = string.IsNullOrWhiteSpace(Email) ? "" : "(LOWER(email) LIKE '%" + Email.ToLower() + "%')";
                var _sqlSearchTextPhone = string.IsNullOrWhiteSpace(Phone) ? "" : "(LOWER(phonenumber) LIKE '%" + (Phone.ToLower()) + "%')";

                var temp = _sqlSearchTextNameSurname + " " + (string.IsNullOrWhiteSpace(_sqlSearchTextEmail) ? _sqlSearchTextEmail : (string.IsNullOrWhiteSpace(_sqlSearchTextNameSurname) ? "" : " AND ") + _sqlSearchTextEmail) + " " + (string.IsNullOrWhiteSpace(_sqlSearchTextPhone) ? _sqlSearchTextPhone : (string.IsNullOrWhiteSpace(_sqlSearchTextEmail) ? "" : " AND ") + _sqlSearchTextPhone);

                return "SELECT COUNT(*) FROM public.cdr_phonebookcdr " + (string.IsNullOrWhiteSpace(temp) ? "" : " where " + temp);
            }
        }

        public static class CompanyUserDetail
        {
            public static string Detail(string no)
            {
                return "select public.cdr_getdisplayname('" + no + "')";
            }

            public static string Top(string no, string StartDate, string EndDate)
            {
                return "select * from public.cdr_get_extension_statistics('" + StartDate + "', '" + EndDate + "', '" + no + "')";
            }

            public static string NumberConversationTopSix(string no, string StartDate, string EndDate)
            {
                return "select * from public.cdr_getextention_top6_Calls('" + StartDate + "', '" + EndDate + "', '" + no + "');";
            }

            public static string TimeConversationTopSix(string no, string StartDate, string EndDate)
            {
                return "select * from public.cdr_getextention_top6_talktime_Calls('" + StartDate + "', '" + EndDate + "', '" + no + "');";
            }

            public static string NumberAnsweredTopSix(string no, string StartDate, string EndDate)
            {
                return "select * from public.cdr_getextention_top6_answered_Calls('" + StartDate + "', '" + EndDate + "', '" + no + "');";
            }

            public static string NumberMissedTopSix(string no, string StartDate, string EndDate)
            {
                return "select * from public.cdr_getextention_top6_missed_Calls('" + StartDate + "', '" + EndDate + "', '" + no + "');";
            }

            public static string CallInfo(string no, Enums.CallInfoFilter filter)
            {
                string _filterType = string.Empty;

                switch (filter)
                {
                    case Enums.CallInfoFilter.ALL:
                        _filterType = "'" + no + "', 0, 0, 0, 0";
                        break;
                    case Enums.CallInfoFilter.INBOUND:
                        _filterType = "'" + no + "', " + ((int)Enums.CallInfoFilter.INBOUND - 1) + ", 0, 0, 0";
                        break;
                    case Enums.CallInfoFilter.OUTBOUND:
                        _filterType = "'" + no + "', 0, " + ((int)Enums.CallInfoFilter.OUTBOUND - 1) + ", 0, 0";
                        break;
                    case Enums.CallInfoFilter.MISSED:
                        _filterType = "'" + no + "', 0, 0, " + ((int)Enums.CallInfoFilter.MISSED - 1) + ", 0";
                        break;
                    case Enums.CallInfoFilter.EXT2EXT:
                        _filterType = "'" + no + "', 0, 0, 0, " + ((int)Enums.CallInfoFilter.EXT2EXT - 1) + "";
                        break;
                    default:
                        _filterType = "'" + no + "', 0, 0, 0, 0";
                        break;
                }

                return "select * from public.cdr_extention_call_reports_all(" + _filterType + ");";
            }
        }

        public static class Report
        {
            public static string ReportCalls(int skip, int take, CompanyReportFilterDto filter, decimal UserGMT,DateTime now)
            {
                var whereDates = Shared.Utilities.Extensions.ReportFilterExtensions.WhereDatesSql(filter.Dates, filter.DatesStart, filter.DatesEnd, UserGMT, now);
                var whereStatues = Shared.Utilities.Extensions.ReportFilterExtensions.WhereStatuesSql(filter.Status);
                var whereInboundOutbound = Shared.Utilities.Extensions.ReportFilterExtensions.WhereInboundOutboundSql(filter.Source, filter.Target, filter.SourceCriteria, filter.TargetCriteria);
                var whereNumberExtNumber = Shared.Utilities.Extensions.ReportFilterExtensions.WhereNumberExtNumber(filter.SourceCriteria, filter.TargetCriteria, filter.SourceCriteriaInput, filter.TargetCriteriaInput);

                var _sql = @"
SELECT * FROM  cdr_reports_param_all_2(
_-{SS}-_, _-{SSC}-_, _-{TT}-_, _-{TTC}-_, _-{SCC}-_,
_-{take}-_, _-{skip}-_,
_-{startdate}-_, 
_-{enddate}-_,
_-{psidn}-_, 
_-{pdidn}-_,
'{_-{pparam}-_}',
'{_-{psidntype}-_}'::int[],
'{_-{pdidntype}-_}'::int[],
'{_-{psactionid}-_}'::int[], 
'{_-{posactionid}-_}'::int[], 
'{_-{podidntype}-_}'::int[],
'{_-{pnoactionid}-_}'::int[],
'_-{dicallernumber}-_',
'_-{nnumber}-_',
null,
'{_-{poparam}-_}',
'{_-{pstatactionid}-_}',
'{_-{pstatactionid2}-_}',
'{_-{pstatdidntype}-_}',
'{_-{pstatedidntype}-_}'
);
";
                string _dynamicSql = _sql
                    .Replace("_-{SS}-_", filter.Source.ToString())
                    .Replace("_-{SSC}-_", filter.SourceCriteria.ToString())
                    .Replace("_-{TT}-_", filter.Target.ToString())
                    .Replace("_-{TTC}-_", filter.TargetCriteria.ToString())
                    .Replace("_-{SCC}-_", filter.Status.ToString())
                    .Replace("_-{take}-_", take.ToString())
                    .Replace("_-{skip}-_", skip.ToString())
                    .Replace("_-{startdate}-_", whereDates.StartDateStringV2)
                    .Replace("_-{enddate}-_", whereDates.EndDateStringV2)
                    .Replace("_-{psidn}-_", "NULL")
                    .Replace("_-{pdidn}-_", "NULL")
                    .Replace("_-{pparam}-_",
                    !string.IsNullOrWhiteSpace(whereNumberExtNumber.SourceCriteria) ? (whereNumberExtNumber.SourceCriteria.Contains("**number**") ? "" : whereNumberExtNumber.SourceCriteria) : "")
                    .Replace("_-{psidntype}-_", whereInboundOutbound.SourceDn)
                    .Replace("_-{pdidntype}-_", whereInboundOutbound.TargetDn)
                    .Replace("_-{psactionid}-_", whereStatues.Psactionid)
                    .Replace("_-{posactionid}-_", whereStatues.Posactionid)
                    .Replace("_-{podidntype}-_", !string.IsNullOrWhiteSpace(whereInboundOutbound.OtherSourceDn) ? whereInboundOutbound.OtherSourceDn : whereStatues.Podidntype)
                    .Replace("_-{pnoactionid}-_", whereInboundOutbound.OtherTargetDn)
                    .Replace("_-{dicallernumber}-_",
                    ((filter.SourceCriteria == (int)Enums.ReportFilterSourceCriteria.NUMBERS || filter.SourceCriteria == (int)Enums.ReportFilterSourceCriteria.NUMBERSCONTAINS) && filter.TargetCriteria == (int)Enums.ReportFilterSourceCriteria.EXTENSIONORRANGEOFEXTENSION) ? whereNumberExtNumber.SourceCriteria.Replace("**number**", "") : (
                    !string.IsNullOrWhiteSpace(whereNumberExtNumber.SourceCriteria) && string.IsNullOrWhiteSpace(whereNumberExtNumber.TargetCriteria) ? (whereNumberExtNumber.SourceCriteria.Contains("**number**") ? whereNumberExtNumber.SourceCriteria.Replace("**number**", "") : "") :
                    !string.IsNullOrWhiteSpace(whereNumberExtNumber.TargetCriteria) ? (whereNumberExtNumber.TargetCriteria.Contains("**number**") ? whereNumberExtNumber.TargetCriteria.Replace("**number**", "") : "") :
                    ""))
                    .Replace("_-{poparam}-_",
                    ((filter.SourceCriteria == (int)Enums.ReportFilterSourceCriteria.NUMBERS || filter.SourceCriteria == (int)Enums.ReportFilterSourceCriteria.NUMBERSCONTAINS) && filter.TargetCriteria == (int)Enums.ReportFilterSourceCriteria.EXTENSIONORRANGEOFEXTENSION) ? whereNumberExtNumber.TargetCriteria : (
                    (!string.IsNullOrWhiteSpace(whereNumberExtNumber.TargetCriteria) && string.IsNullOrWhiteSpace(whereNumberExtNumber.SourceCriteria)) || (filter.TargetCriteria == (int)Enums.ReportFilterSourceCriteria.EXTENSIONORRANGEOFEXTENSION && filter.SourceCriteria == (int)Enums.ReportFilterSourceCriteria.EXTENSIONORRANGEOFEXTENSION) ?
                    (whereNumberExtNumber.TargetCriteria.Contains("**number**") ? whereNumberExtNumber.SourceCriteria.Replace("**number**", "") : whereNumberExtNumber.TargetCriteria) : ""
                    )
                    )
                    .Replace("_-{nnumber}-_", "EndCall")
                    .Replace("_-{pstatactionid}-_", filter.Status == (int)Enums.ReportFilterStatue.UNANSWEREDCALLS ? whereStatues.Posactionid : "")
                    .Replace("_-{pstatactionid2}-_", whereStatues.Psactionid)
                    .Replace("_-{pstatdidntype}-_", filter.Status == (int)Enums.ReportFilterStatue.UNANSWEREDCALLS ? whereStatues.Podidntype : "")
                    .Replace("_-{pstatedidntype}-_", filter.Status == (int)Enums.ReportFilterStatue.ANSWEREDCALLS ? whereStatues.Posactionid : "")
                    ;

                _dynamicSql = _dynamicSql.Replace("\r\n", " ");
                _dynamicSql = _dynamicSql.Replace("\t", " ");

                return _dynamicSql;
            }
            public static string ReportCallsCount(CompanyReportFilterDto filter, decimal UserGMT, DateTime now)
            {
                var whereDates = Shared.Utilities.Extensions.ReportFilterExtensions.WhereDatesSql(filter.Dates, filter.DatesStart, filter.DatesEnd, UserGMT, now);
                var whereStatues = Shared.Utilities.Extensions.ReportFilterExtensions.WhereStatuesSql(filter.Status);
                var whereInboundOutbound = Shared.Utilities.Extensions.ReportFilterExtensions.WhereInboundOutboundSql(filter.Source, filter.Target, filter.SourceCriteria, filter.TargetCriteria);
                var whereNumberExtNumber = Shared.Utilities.Extensions.ReportFilterExtensions.WhereNumberExtNumber(filter.SourceCriteria, filter.TargetCriteria, filter.SourceCriteriaInput, filter.TargetCriteriaInput);

                var _sqlCount = @"
SELECT * FROM  cdr_reports_param_all_2_cnt(
_-{SS}-_, _-{SSC}-_, _-{TT}-_, _-{TTC}-_, _-{SCC}-_,
_-{take}-_, _-{skip}-_,
_-{startdate}-_, 
_-{enddate}-_,
_-{psidn}-_, 
_-{pdidn}-_,
'{_-{pparam}-_}',
'{_-{psidntype}-_}'::int[],
'{_-{pdidntype}-_}'::int[],
'{_-{psactionid}-_}'::int[], 
'{_-{posactionid}-_}'::int[], 
'{_-{podidntype}-_}'::int[],
'{_-{pnoactionid}-_}'::int[],
'_-{dicallernumber}-_',
'_-{nnumber}-_',
null,
'{_-{poparam}-_}',
'{_-{pstatactionid}-_}',
'{_-{pstatactionid2}-_}',
'{_-{pstatdidntype}-_}',
'{_-{pstatedidntype}-_}'
);
";

                string _dynamicSql = _sqlCount
                    .Replace("_-{SS}-_", filter.Source.ToString())
                    .Replace("_-{SSC}-_", filter.SourceCriteria.ToString())
                    .Replace("_-{TT}-_", filter.Target.ToString())
                    .Replace("_-{TTC}-_", filter.TargetCriteria.ToString())
                    .Replace("_-{SCC}-_", filter.Status.ToString())
                    .Replace("_-{take}-_", "10")
                    .Replace("_-{skip}-_", "20")
                    .Replace("_-{startdate}-_", whereDates.StartDateStringV2)
                    .Replace("_-{enddate}-_", whereDates.EndDateStringV2)
                    .Replace("_-{psidn}-_", "NULL")
                    .Replace("_-{pdidn}-_", "NULL")
                    .Replace("_-{pparam}-_",
                    !string.IsNullOrWhiteSpace(whereNumberExtNumber.SourceCriteria) ? (whereNumberExtNumber.SourceCriteria.Contains("**number**") ? "" : whereNumberExtNumber.SourceCriteria) : "")
                    .Replace("_-{psidntype}-_", whereInboundOutbound.SourceDn)
                    .Replace("_-{pdidntype}-_", whereInboundOutbound.TargetDn)
                    .Replace("_-{psactionid}-_", whereStatues.Psactionid)
                    .Replace("_-{posactionid}-_", whereStatues.Posactionid)
                    .Replace("_-{podidntype}-_", !string.IsNullOrWhiteSpace(whereInboundOutbound.OtherSourceDn) ? whereInboundOutbound.OtherSourceDn : whereStatues.Podidntype)
                    .Replace("_-{pnoactionid}-_", whereInboundOutbound.OtherTargetDn)
                    .Replace("_-{dicallernumber}-_",
                    ((filter.SourceCriteria == (int)Enums.ReportFilterSourceCriteria.NUMBERS || filter.SourceCriteria == (int)Enums.ReportFilterSourceCriteria.NUMBERSCONTAINS) && filter.TargetCriteria == (int)Enums.ReportFilterSourceCriteria.EXTENSIONORRANGEOFEXTENSION) ? whereNumberExtNumber.SourceCriteria.Replace("**number**", "") : (
                    !string.IsNullOrWhiteSpace(whereNumberExtNumber.SourceCriteria) && string.IsNullOrWhiteSpace(whereNumberExtNumber.TargetCriteria) ? (whereNumberExtNumber.SourceCriteria.Contains("**number**") ? whereNumberExtNumber.SourceCriteria.Replace("**number**", "") : "") :
                    !string.IsNullOrWhiteSpace(whereNumberExtNumber.TargetCriteria) ? (whereNumberExtNumber.TargetCriteria.Contains("**number**") ? whereNumberExtNumber.TargetCriteria.Replace("**number**", "") : "") :
                    ""))
                    .Replace("_-{poparam}-_",
                    ((filter.SourceCriteria == (int)Enums.ReportFilterSourceCriteria.NUMBERS || filter.SourceCriteria == (int)Enums.ReportFilterSourceCriteria.NUMBERSCONTAINS) && filter.TargetCriteria == (int)Enums.ReportFilterSourceCriteria.EXTENSIONORRANGEOFEXTENSION) ? whereNumberExtNumber.TargetCriteria : (
                    (!string.IsNullOrWhiteSpace(whereNumberExtNumber.TargetCriteria) && string.IsNullOrWhiteSpace(whereNumberExtNumber.SourceCriteria)) || (filter.TargetCriteria == (int)Enums.ReportFilterSourceCriteria.EXTENSIONORRANGEOFEXTENSION && filter.SourceCriteria == (int)Enums.ReportFilterSourceCriteria.EXTENSIONORRANGEOFEXTENSION) ?
                    (whereNumberExtNumber.TargetCriteria.Contains("**number**") ? whereNumberExtNumber.SourceCriteria.Replace("**number**", "") : whereNumberExtNumber.TargetCriteria) : ""
                    )
                    )
                    .Replace("_-{nnumber}-_", "EndCall")
                    .Replace("_-{pstatactionid}-_", filter.Status == (int)Enums.ReportFilterStatue.UNANSWEREDCALLS ? whereStatues.Posactionid : "")
                    .Replace("_-{pstatactionid2}-_", whereStatues.Psactionid)
                    .Replace("_-{pstatdidntype}-_", filter.Status == (int)Enums.ReportFilterStatue.UNANSWEREDCALLS ? whereStatues.Podidntype : "")
                    .Replace("_-{pstatedidntype}-_", filter.Status == (int)Enums.ReportFilterStatue.ANSWEREDCALLS ? whereStatues.Posactionid : "")
                    ;

                _dynamicSql = _dynamicSql.Replace("\r\n", " ");
                _dynamicSql = _dynamicSql.Replace("\t", " ");

                return _dynamicSql;
            }
        }

        public static class Call
        {
            public static string CallDetail(long CallId)
            {
                return "select * from public.cdr_extention_call_details(" + CallId + ")";
            }

            public static string CallInformation(long CallId)
            {
                return "select * from  public.cdr_extention_call_informations(" + CallId + ")";
            }
        }
    }
}