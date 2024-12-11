using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CDR.Shared.Utilities.Results.ComplexTypes
{
    public class Enums
    {
        public enum FilterTimes
        {
            DAILY = 1,
            WEEKLY,
            MONTHLY,
            ALL
        }

        public enum GraphFilter
        {
            WEEKLY = 1,
            MONTHLY,
            YEARLY,
            DAILY,
        }

        public enum DashboardFilter
        {
            DAILY = 1,
            MONTHLY,
            ALL,
            WEEKLY
        }

        public enum CallSummaryFilter
        {
            HOURLY = 1,
            DAILY
        }

        public enum CallInfoFilter
        {
            ALL = 1,
            INBOUND,
            OUTBOUND,
            MISSED,
            EXT2EXT
        }

        public enum ReportFilterDate
        {
            ALL = 1,
            LASTDAY,
            LAST7DAYS,
            LAST30DAYS,
            LAST3MONTHS,
            LAST6MONTHS,
            FROMTHEBEGENNINGOFTHISDAY,
            FROMTHEBEGENNINGOFTHISWEEK,
            FROMTHEBEGENNINGOFTHISMONTH,
            FROMTHEBEGENNINGOFTHISYEAR,
            CUSTOM
        }

        public enum ReportFilterSource
        {
            ALL = 1,
            EXTERNALNUMBER,
            INTERNALNUMBER
        }

        public enum ReportFilterSourceCriteria
        {
            ALL = 1,
            EXTENSIONORRANGEOFEXTENSION,
            NUMBERS,
            NUMBERSCONTAINS
        }

        public enum ReportFilterTarget
        {
            ALL = 1,
            EXTERNALNUMBER,
            INTERNALNUMBER
        }

        public enum ReportFilterTargetCriteria
        {
            ALL = 1,
            EXTENSIONORRANGEOFEXTENSION,
            NUMBERS,
            NUMBERSCONTAINS
        }

        public enum ReportFilterStatue
        {
            ANSWEREDANDUNANSWEREDCALLS = 1,
            ANSWEREDCALLS,
            UNANSWEREDCALLS
        }

        public enum SupportStatue
        {
            WAITING = 0,
            SOLVED,
            NOT_SOLVED,
            CANCELLED
        }

        public enum ConsoleQueryType
        {
            [Description("Dashboard Total Günlük Sorgusu")]
            DASHBOARD_TOTALS_DAILY = 1,
            [Description("Dashboard Total Haftalık Sorgusu")]
            DASHBOARD_TOTALS_WEEKLY, /*2*/
            [Description("Dashboard Total Aylık Sorgusu")]
            DASHBOARD_TOTALS_MONTHLY, /*3*/
            [Description("Dashboard Total Yıllık Sorgusu")]
            DASHBOARD_TOTALS_YEARLY, /*4*/
            /*---------------------------------------------*/
            [Description("Dashboard Total Bar Sorgusu")]
            DASHBOARD_BARS, /*5*/
            /*---------------------------------------------*/
            [Description("Dashboard Total Calls Haftalık Sorgusu")]
            DASHBOARD_TOTAL_CALLS_WEEKLY, /*6*/
            [Description("Dashboard Total Calls Aylık Sorgusu")]
            DASHBOARD_TOTAL_CALLS_MONTHLY, /*7*/
            [Description("Dashboard Total Calls Yıllık Sorgusu")]
            DASHBOARD_TOTAL_CALLS_YEARLY, /*8*/
            /*---------------------------------------------*/
            [Description("Dashboard Top 5 Talk Time Günlük Sorgusu")]
            DASHBOARD_TALK_TIME_DAILY, /*9*/
            [Description("Dashboard Top 5 Talk Time Haftalık Sorgusu")]
            DASHBOARD_TALK_TIME_WEEKLY, /*10*/
            [Description("Dashboard Top 5 Talk Time Aylık Sorgusu")]
            DASHBOARD_TALK_TIME_MONTHLY, /*11*/
            [Description("Dashboard Top 5 Talk Time Yıllık Sorgusu")]
            DASHBOARD_TALK_TIME_YEARLY, /*12*/
            /*---------------------------------------------*/
            [Description("Dashboard Top 5 Answered Calls Günlük Sorgusu")]
            DASHBOARD_ANSWERED_CALLS_DAILY, /*13*/
            [Description("Dashboard Top 5 Answered Calls Haftalık Sorgusu")]
            DASHBOARD_ANSWERED_CALLS_WEEKLY, /*14*/
            [Description("Dashboard Top 5 Answered Calls Aylık Sorgusu")]
            DASHBOARD_ANSWERED_CALLS_MONTHLY, /*15*/
            [Description("Dashboard Top 5 Answered Calls Yıllık Sorgusu")]
            DASHBOARD_ANSWERED_CALLS_YEARLY, /*16*/
            /*---------------------------------------------*/
            [Description("Dashboard Top 5 Inbound Günlük Sorgusu")]
            DASHBOARD_INBOUND_DAILY, /*17*/
            [Description("Dashboard Top 5 Inbound Haftalık Sorgusu")]
            DASHBOARD_INBOUND_WEEKLY, /*18*/
            [Description("Dashboard Top 5 Inbound Aylık Sorgusu")]
            DASHBOARD_INBOUND_MONTHLY, /*19*/
            [Description("Dashboard Top 5 Inbound Yıllık Sorgusu")]
            DASHBOARD_INBOUND_YEARLY, /*20*/
            /*---------------------------------------------*/
            [Description("Dashboard Top 5 Outbound Günlük Sorgusu")]
            DASHBOARD_OUTBOUND_DAILY, /*21*/
            [Description("Dashboard Top 5 Outbound Haftalık Sorgusu")]
            DASHBOARD_OUTBOUND_WEEKLY, /*22*/
            [Description("Dashboard Top 5 Outbound Aylık Sorgusu")]
            DASHBOARD_OUTBOUND_MONTHLY, /*23*/
            [Description("Dashboard Top 5 Outbound Yıllık Sorgusu")]
            DASHBOARD_OUTBOUND_YEARLY, /*24*/
            /*---------------------------------------------*/
            [Description("Company Profile Monthly Total Calls Sorgusu")]
            COMPANY_PROFILE_MONTHLY_TOTAL_CALLS, /*25*/
            /*---------------------------------------------*/
            [Description("Company Profile Most Contacted Günlük Sorgusu")]
            COMPANY_PROFILE_MOST_CONTACTED_DAILY_CALLS, /*25*/
            [Description("Company Profile Most Contacted Haftalık Sorgusu")]
            COMPANY_PROFILE_MOST_CONTACTED_WEEKLY_CALLS, /*26*/
            [Description("Company Profile Most Contacted Aylık Sorgusu")]
            COMPANY_PROFILE_MOST_CONTACTED_MONTHLY_CALLS, /*27*/
            [Description("Company Profile Most Contacted Yıllık Sorgusu")]
            COMPANY_PROFILE_MOST_CONTACTED_YEARLY_CALLS, /*28*/
            /*---------------------------------------------*/
            [Description("Company Profile Call Summary Saatlik Sorgusu")]
            COMPANY_PROFILE_CALL_SUMMARY_HOURLY_CALLS, /*29*/
            [Description("Company Profile Call Summary Günlük Sorgusu")]
            COMPANY_PROFILE_CALL_SUMMARY_DAILY_CALLS, /*30*/
            /*---------------------------------------------*/
            [Description("Company Profile General Statistic Günlük Sorgusu")]
            COMPANY_PROFILE_GENERAL_STATISTIC_DAILY, /*31*/
            [Description("Company Profile General Statistic Haftalık Sorgusu")]
            COMPANY_PROFILE_GENERAL_STATISTIC_WEEKLY, /*32*/
            [Description("Company Profile General Statistic Aylık Sorgusu")]
            COMPANY_PROFILE_GENERAL_STATISTIC_MONTHLY, /*33*/
            [Description("Company Profile General Statistic Yıllık Sorgusu")]
            COMPANY_PROFILE_GENERAL_STATISTIC_YEARLY, /*34*/
            /*---------------------------------------------*/
            [Description("Company Profile Inbound Statistic Günlük Sorgusu")]
            COMPANY_PROFILE_INBOUND_STATISTIC_DAILY, /*35*/
            [Description("Company Profile Inbound Statistic Haftalık Sorgusu")]
            COMPANY_PROFILE_INBOUND_STATISTIC_WEEKLY, /*36*/
            [Description("Company Profile Inbound Statistic Aylık Sorgusu")]
            COMPANY_PROFILE_INBOUND_STATISTIC_MONTHLY, /*37*/
            [Description("Company Profile Inbound Statistic Yıllık Sorgusu")]
            COMPANY_PROFILE_INBOUND_STATISTIC_YEARLY, /*38*/
            /*---------------------------------------------*/
            [Description("Company Profile Outbound Statistic Günlük Sorgusu")]
            COMPANY_PROFILE_OUTBOUND_STATISTIC_DAILY, /*39*/
            [Description("Company Profile Outbound Statistic Haftalık Sorgusu")]
            COMPANY_PROFILE_OUTBOUND_STATISTIC_WEEKLY, /*40*/
            [Description("Company Profile Outbound Statistic Aylık Sorgusu")]
            COMPANY_PROFILE_OUTBOUND_STATISTIC_MONTHLY, /*41*/
            [Description("Company Profile Outbound Statistic Yıllık Sorgusu")]
            COMPANY_PROFILE_OUTBOUND_STATISTIC_YEARLY, /*42*/
            /*---------------------------------------------*/
            [Description("Company Profile Ext2Ext Statistic Günlük Sorgusu")]
            COMPANY_PROFILE_EXT2EXT_STATISTIC_DAILY, /*43*/
            [Description("Company Profile Ext2Ext Statistic Haftalık Sorgusu")]
            COMPANY_PROFILE_EXT2EXT_STATISTIC_WEEKLY, /*44*/
            [Description("Company Profile Ext2Ext Statistic Aylık Sorgusu")]
            COMPANY_PROFILE_EXT2EXT_STATISTIC_MONTHLY, /*45*/
            [Description("Company Profile Ext2Ext Statistic Yıllık Sorgusu")]
            COMPANY_PROFILE_EXT2EXT_STATISTIC_YEARLY, /*46*/
            /*---------------------------------------------*/
            [Description("Company Users Top Statistic")]
            COMPANY_USERS_STATISTIC, /*47*/
            [Description("Company Users")]
            COMPANY_USERS /*48*/
        }

        public enum Content
        {
            [Description("CDR_MesafeliSatisSozlesmesi")]
            DISTANCE_SALES_AGREEMENT = 1,
            [Description("CDR_GizlilikSozlesmesi")]
            PRIVACY_AGREEMENT,
            [Description("CDR_UyelikSozlesmesi")]
            MEMBERSHIP_AGREEMENT
        }

        public enum Currency
        {
            [Description("TL (₺)")]
            TL = 1,
            [Description("DOLAR ($)")]
            DOLAR,
            [Description("EURO (€)")]
            EURO
        }

        public enum PageTypes
        {
            [Description("DASHBOARD")]
            DASHBOARD = 1,
            [Description("TRUNK")]
            TRUNK,
            [Description("COMPANY_PROFILE")]
            COMPANY_PROFILE,
            [Description("REPORTS")]
            REPORTS,
            [Description("EXTENSIONS")]
            EXTENSIONS,
            [Description("EXTENSION_DETAIL")]
            EXTENSION_DETAIL,
            [Description("PHONEBOOK")]
            PHONEBOOK,
            [Description("PROFILE")]
            PROFILE,
            [Description("MEMBERSHIP")]
            MEMBERSHIP,
            [Description("SUPPORT")]
            SUPPORT,
            [Description("OTHER")]
            OTHER
        }

        public enum HelpDetailType
        {
            NORMAL = 1,
            BEFORE_CLICK
        }
    }
}
