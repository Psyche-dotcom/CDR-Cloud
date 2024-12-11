using CDR.API.Api.Model;
using CDR.Entities.Concrete;
using CDR.Entities.Dtos;
using CDR.Services.Abstract;
using CDR.Shared.Utilities.Results.ComplexTypes;
using System.Text;
using static CDR.Shared.Utilities.Results.ComplexTypes.Enums;

namespace CDR.API.PartialViewGeneration
{
    public static class PartialViewGen
    {
        public static string GenerateDashboardOutboundHtml(List<Entities.Dtos.DashboardMostInboundOutboundDto> dataList)
        {
            var sb = new StringBuilder();
            long firstTotal = 0;
            int styleWidth = 0;
            var gradientClassList = new List<string>
    {
        "bg-gradient-x-success",
        "bg-gradient-x-info",
        "bg-gradient-x-primary",
        "bg-gradient-x-warning",
        "bg-gradient-x-danger"
    };

            if (dataList != null && dataList.Count > 0)
            {
                for (int i = 0; i < dataList.Count; i++)
                {
                    if (i == 0)
                    {
                        firstTotal = dataList[i].numberofcalls;
                        styleWidth = 100;
                    }
                    else
                    {
                        var max = firstTotal;
                        styleWidth = (int)(100 * dataList[i].numberofcalls / max);
                    }

                    // Append each row's HTML content
                    sb.AppendFormat("<p class=\"pt-1\">{0} <span class=\"float-right text-bold-600\">{1}</span></p>", dataList[i].to, dataList[i].numberofcalls);
                    sb.AppendFormat(
                        "<div class=\"progress progress-sm mt-1 mb-0 box-shadow-1\">" +
                        "<div class=\"progress-bar {0}\" role=\"progressbar\" style=\"width: {1}%\" aria-valuenow=\"{2}\" aria-valuemin=\"0\" aria-valuemax=\"{3}\"></div>" +
                        "</div>",
                        gradientClassList[i], styleWidth, dataList[i].numberofcalls, firstTotal);
                }
            }
            else
            {
                sb.Append("<p class=\"pt-1\">No data available for the selected time range.</p>");
            }

            return sb.ToString();
        }

        public static string GenerateDashboardInboundHtml(List<Entities.Dtos.DashboardMostInboundOutboundDto> dataList)
        {
            var sb = new StringBuilder();
            long firstTotal = 0;
            int styleWidth = 0;
            var gradientClassList = new List<string>
    {
        "bg-gradient-x-success",
        "bg-gradient-x-info",
        "bg-gradient-x-primary",
        "bg-gradient-x-warning",
        "bg-gradient-x-danger"
    };

            if (dataList != null && dataList.Count > 0)
            {
                for (int i = 0; i < dataList.Count; i++)
                {
                    if (i == 0)
                    {
                        firstTotal = dataList[i].numberofcalls;
                        styleWidth = 100;
                    }
                    else
                    {
                        var max = firstTotal;
                        styleWidth = (int)(100 * dataList[i].numberofcalls / max);
                    }

                    // Append each row's HTML content
                    sb.AppendFormat("<p class=\"pt-1\">{0} <span class=\"float-right text-bold-600\">{1}</span></p>", dataList[i].from, dataList[i].numberofcalls);
                    sb.AppendFormat(
                        "<div class=\"progress progress-sm mt-1 mb-0 box-shadow-1\">" +
                        "<div class=\"progress-bar {0}\" role=\"progressbar\" style=\"width: {1}%\" aria-valuenow=\"{2}\" aria-valuemin=\"0\" aria-valuemax=\"{3}\"></div>" +
                        "</div>",
                        gradientClassList[i], styleWidth, dataList[i].numberofcalls, firstTotal);
                }
            }
            else
            {
                sb.Append("<p class=\"pt-1\">No data available for the selected time range.</p>");
            }

            return sb.ToString();
        }

        public static string GenerateDashboardMostAnsweredHtml(List<Entities.Dtos.DashboardLeastAnsweredDto> dataList)
        {
            var sb = new StringBuilder();
            long firstTotal = 0;
            int styleWidth = 0;
            var gradientClassList = new List<string>
    {
        "bg-gradient-x-success",
        "bg-gradient-x-info",
        "bg-gradient-x-primary",
        "bg-gradient-x-warning",
        "bg-gradient-x-danger"
    };

            if (dataList != null && dataList.Count > 0)
            {
                for (int i = 0; i < dataList.Count; i++)
                {
                    if (i == 0)
                    {
                        firstTotal = dataList[i].calls;
                        styleWidth = 100;
                    }
                    else
                    {
                        var max = firstTotal;
                        styleWidth = (int)(100 * dataList[i].calls / max);
                    }

                    // Append each row's HTML content
                    sb.AppendFormat("<p class=\"pt-1\">{0} <span class=\"float-right text-bold-600\">{1}</span></p>", dataList[i].d_name, dataList[i].calls);
                    sb.AppendFormat(
                        "<div class=\"progress progress-sm mt-1 mb-0 box-shadow-1\">" +
                        "<div class=\"progress-bar {0}\" role=\"progressbar\" style=\"width: {1}%\" aria-valuenow=\"{2}\" aria-valuemin=\"0\" aria-valuemax=\"{3}\"></div>" +
                        "</div>",
                        gradientClassList[i % gradientClassList.Count], styleWidth, dataList[i].calls, firstTotal);
                }
            }
            else
            {
                sb.Append("<p class=\"pt-1\">No data available for the selected time range.</p>");
            }

            return sb.ToString();
        }

        public static string GenerateDashboardMostTalkHtml(List<Entities.Dtos.DashboardLeastTalkDto> dataList)
        {
            var sb = new StringBuilder();
            long firstTotal = 0;
            int styleWidth = 0;
            var gradientClassList = new List<string>
    {
        "bg-gradient-x-success",
        "bg-gradient-x-info",
        "bg-gradient-x-primary",
        "bg-gradient-x-warning",
        "bg-gradient-x-danger"
    };

            if (dataList != null && dataList.Count > 0)
            {
                for (int i = 0; i < dataList.Count; i++)
                {
                    if (i == 0)
                    {
                        firstTotal = (long)dataList[i].duration.TotalSeconds == 0 ? 60 * 60 * 24 : (long)dataList[i].duration.TotalSeconds;
                        styleWidth = 100;
                    }
                    else
                    {
                        var max = firstTotal;
                        styleWidth = (int)(100 * (long)dataList[i].duration.TotalSeconds / max);
                    }

                    // Format hours, minutes, and seconds for each row
                    string totalHours = ((int)dataList[i].duration.TotalHours).ToString("D2");
                    string minutes = dataList[i].duration.Minutes.ToString("D2");
                    string seconds = dataList[i].duration.Seconds.ToString("D2");

                    // Append each row's HTML content
                    sb.AppendFormat(
                        "<p class=\"pt-1\">{0} <span class=\"float-right text-bold-600 dashboard-topmosttalk-time\"><i class=\"ft-clock\"></i> {1}:{2}:{3}</span></p>",
                        dataList[i].d_name, totalHours, minutes, seconds);

                    sb.AppendFormat(
                        "<div class=\"progress progress-sm mt-1 mb-0 box-shadow-1\">" +
                        "<div class=\"progress-bar {0}\" role=\"progressbar\" style=\"width: {1}%\" aria-valuenow=\"{2}\" aria-valuemin=\"0\" aria-valuemax=\"{3}\"></div>" +
                        "</div>",
                        gradientClassList[i % gradientClassList.Count], styleWidth, (int)dataList[i].duration.TotalSeconds, firstTotal);
                }
            }
            else
            {
                sb.Append("<p class=\"pt-1\">No data available for the selected time range.</p>");
            }

            return sb.ToString();
        }


        public static string GenerateCompanyGeneralStatisticsHtml(CompanyGeneralStatisticModel model, IStaticService staticService)
        {
            var sb = new StringBuilder();

            sb.Append("<table class=\"melocom last-call-statistic titlesizeweight\"><tbody>");

            // Total Call Time
            sb.Append("<tr>");
            sb.AppendFormat("<td>{0}</td>", staticService.GetLocalization("CDR_TotalCallTime").Data);
            string totalCallTime = FormatTimeSpan(model.Data.totalcalltime);
            sb.AppendFormat("<td class=\"text-right\"><img src=\"/app-assets/images/icons/bi_clock-fill-green.svg\" /> <span class=\"badge badge-pill badge-secondary\">{0}</span></td>", totalCallTime);
            sb.Append("</tr>");

            // Average Call Time
            sb.Append("<tr>");
            sb.AppendFormat("<td>{0}</td>", staticService.GetLocalization("CDR_AverageCallTime").Data);
            string averageCallTime = FormatTimeSpan(model.Data.avrgcalltime);
            sb.AppendFormat("<td class=\"text-right\"><img src=\"/app-assets/images/icons/bi_clock-fill-green.svg\" /> <span class=\"badge badge-pill badge-secondary\">{0}</span></td>", averageCallTime);
            sb.Append("</tr>");

            // Total Talk Time
            sb.Append("<tr>");
            sb.AppendFormat("<td>{0}</td>", staticService.GetLocalization("CDR_TotalTalkTime").Data);
            string totalTalkTime = FormatTimeSpan(model.Data.totaltalkduration);
            sb.AppendFormat("<td class=\"text-right\"><img src=\"/app-assets/images/icons/bi_clock-fill-green.svg\" /> <span class=\"badge badge-pill badge-secondary\">{0}</span></td>", totalTalkTime);
            sb.Append("</tr>");

            // Average Talk Time
            sb.Append("<tr>");
            sb.AppendFormat("<td>{0}</td>", staticService.GetLocalization("CDR_AverageTalkTime").Data);
            string averageTalkTime = FormatTimeSpan(model.Data.avrgtalkduration);
            sb.AppendFormat("<td class=\"text-right\"><img src=\"/app-assets/images/icons/bi_clock-fill-green.svg\" /> <span class=\"badge badge-pill badge-secondary\">{0}</span></td>", averageTalkTime);
            sb.Append("</tr>");

            // Total Ring Time
            sb.Append("<tr>");
            sb.AppendFormat("<td>{0}</td>", staticService.GetLocalization("CDR_TotalRingTime").Data);
            string totalRingTime = FormatTimeSpan(model.Data.totalringtime);
            sb.AppendFormat("<td class=\"text-right\"><img src=\"/app-assets/images/icons/bi_clock-fill-green.svg\" /> <span class=\"badge badge-pill badge-secondary\">{0}</span></td>", totalRingTime);
            sb.Append("</tr>");

            // Average Ring Time
            sb.Append("<tr>");
            sb.AppendFormat("<td>{0}</td>", staticService.GetLocalization("CDR_AverageRingTime").Data);
            string averageRingTime = FormatTimeSpan(model.Data.avrgringtime);
            sb.AppendFormat("<td class=\"text-right\"><img src=\"/app-assets/images/icons/bi_clock-fill-green.svg\" /> <span class=\"badge badge-pill badge-secondary\">{0}</span></td>", averageRingTime);
            sb.Append("</tr>");

            // Total Abandoned Time
            sb.Append("<tr>");
            sb.AppendFormat("<td>{0}</td>", staticService.GetLocalization("CDR_TotalAbandonedTime").Data);
            string totalAbandonedTime = FormatTimeSpan(model.Data.totalabandonedtime);
            sb.AppendFormat("<td class=\"text-right\"><img src=\"/app-assets/images/icons/bi_clock-fill-green.svg\" /> <span class=\"badge badge-pill badge-secondary\">{0}</span></td>", totalAbandonedTime);
            sb.Append("</tr>");

            // Average Abandoned Time
            sb.Append("<tr>");
            sb.AppendFormat("<td>{0}</td>", staticService.GetLocalization("CDR_AverageAbandonedTime").Data);
            string averageAbandonedTime = FormatTimeSpan(model.Data.avrgabandonedtime);
            sb.AppendFormat("<td class=\"text-right\"><img src=\"/app-assets/images/icons/bi_clock-fill-green.svg\" /> <span class=\"badge badge-pill badge-secondary\">{0}</span></td>", averageAbandonedTime);
            sb.Append("</tr>");

            sb.Append("</tbody></table>");

            return sb.ToString();
        }


        public static string GenerateCompanyInternalStatisticHtml(CompanyInternalStatisticModel model, IStaticService staticService)
        {
            var sb = new StringBuilder();

            sb.Append("<table class=\"last-call-statistic titlesizeweight\">");
            sb.Append("<tbody>");

            // Total Calls
            sb.Append("<tr>");
            sb.AppendFormat("<td>{0}</td>", staticService.GetLocalization("CDR_TotalCalls").Data);
            sb.AppendFormat("<td><span class=\"badge-primary\">{0}</span></td>", model.Data.totalext2ext);
            sb.Append("</tr>");

            // Answered Calls
            sb.Append("<tr>");
            sb.AppendFormat("<td>{0}</td>", staticService.GetLocalization("CDR_AnsweredCalls").Data);
            sb.AppendFormat("<td><span class=\"badge-primary\">{0}</span></td>", model.Data.answeredext2ext);
            sb.Append("</tr>");

            // Missed Calls
            sb.Append("<tr>");
            sb.AppendFormat("<td>{0}</td>", staticService.GetLocalization("CDR_MissedCalls").Data);
            sb.AppendFormat("<td><span class=\"badge-primary\">{0}</span></td>", model.Data.missedext2ext);
            sb.Append("</tr>");

            sb.Append("</tbody>");
            sb.Append("</table>");

            return sb.ToString();
        }

        public static string GenerateCompanyInboundStatisticHtml(CompanyInboundStatisticModel model, IStaticService staticService)
        {
            var sb = new StringBuilder();

            sb.Append("<table class=\"last-call-statistic titlesizeweight\">");
            sb.Append("<tbody>");

            // Total Calls
            sb.Append("<tr>");
            sb.AppendFormat("<td>{0}</td>", staticService.GetLocalization("CDR_TotalCalls").Data);
            sb.AppendFormat("<td><span class=\"badge-primary\">{0}</span></td>", model.Data.totalinbound);
            sb.Append("</tr>");

            // Answered Calls
            sb.Append("<tr>");
            sb.AppendFormat("<td>{0}</td>", staticService.GetLocalization("CDR_AnsweredCalls").Data);
            sb.AppendFormat("<td><span class=\"badge-primary\">{0}</span></td>", model.Data.answeredinbound);
            sb.Append("</tr>");

            // Unanswered Calls
            sb.Append("<tr>");
            sb.AppendFormat("<td>{0}</td>", staticService.GetLocalization("CDR_UnansweredCalls").Data);
            sb.AppendFormat("<td><span class=\"badge-primary\">{0}</span></td>", model.Data.unansweredInbound);
            sb.Append("</tr>");

            /* Uncomment the following sections if you want to include abandoned and missed calls 
            // Abandoned Calls
            sb.Append("<tr>");
            sb.AppendFormat("<td>{0}</td>", staticService.GetLocalization("CDR_AbandonedCalls").Data);
            sb.AppendFormat("<td><span class=\"badge-primary\">{0}</span></td>", model.Data.abandonedinbound);
            sb.Append("</tr>");

            // Missed Calls
            sb.Append("<tr>");
            sb.AppendFormat("<td>{0}</td>", staticService.GetLocalization("CDR_MissedCalls").Data);
            sb.AppendFormat("<td><span class=\"badge-primary\">{0}</span></td>", model.Data.missedinbound);
            sb.Append("</tr>");
            */

            sb.Append("</tbody>");
            sb.Append("</table>");

            return sb.ToString();
        }

        public static string GenerateCompanyOutboundStatisticHtml(CompanyOutboundStatisticModel model, IStaticService staticService)
        {
            var sb = new StringBuilder();

            sb.Append("<table class=\"last-call-statistic titlesizeweight\">");
            sb.Append("<tbody>");

            // Total Calls
            sb.Append("<tr>");
            sb.AppendFormat("<td>{0}</td>", staticService.GetLocalization("CDR_TotalCalls").Data);
            sb.AppendFormat("<td><span class=\"badge-primary\">{0}</span></td>", model.Data.totaloutbound);
            sb.Append("</tr>");

            // Answered Calls
            sb.Append("<tr>");
            sb.AppendFormat("<td>{0}</td>", staticService.GetLocalization("CDR_AnsweredCalls").Data);
            sb.AppendFormat("<td><span class=\"badge-primary\">{0}</span></td>", model.Data.answeredoutbound);
            sb.Append("</tr>");

            // Missed Calls
            sb.Append("<tr>");
            sb.AppendFormat("<td>{0}</td>", staticService.GetLocalization("CDR_MissedCalls").Data);
            sb.AppendFormat("<td><span class=\"badge-primary\">{0}</span></td>", model.Data.outboundmissed);
            sb.Append("</tr>");

            sb.Append("</tbody>");
            sb.Append("</table>");

            return sb.ToString();
        }

        public static string GenerateCompanyCallDetailHtml(CompanyCallDetailModel model, IStaticService staticService)
        {
            var sb = new StringBuilder();
            int infoCount = 0;
            TimeSpan ringOrTalTime = new TimeSpan();

            if (model.ResultStatus == CDR.Shared.Utilities.Results.ComplexTypes.ResultStatus.Error)
            {
                sb.AppendFormat("<p>{0}</p>", model.Message);
                return sb.ToString();
            }

            sb.AppendLine("<style>");
            sb.AppendLine(".plyr { position: absolute; top: 50%; left: 50%; transform: translate(-50%,-50%); width: 480px; }");
            sb.AppendLine("</style>");

            sb.AppendLine("<div class=\"header\">");
            sb.AppendFormat("<input type=\"hidden\" id=\"RecordingFileName\" value=\"{0}\" />",
                CDR.Shared.Utilities.Extensions.BaseExtensions.PhotoTitle(model.Detail.from) + "-" +
                CDR.Shared.Utilities.Extensions.BaseExtensions.PhotoTitle(model.Detail.to));
            sb.AppendLine("<a class=\"close-button close-appointment\" onclick=\"closeNav()\"><img src=\"/app-assets/images/modal-close.svg\"></a>");
            sb.AppendFormat("<div class=\"header-tag\">{0}</div>", staticService.GetLocalization(model.Detail.tablecolumninorout).Data);
            sb.AppendLine("<div class=\"row header-back\">");
            sb.AppendLine("<div class=\"col-md-6 text-center\">");
            sb.AppendLine("<div class=\"from-avatar detail-avatar\">");
            sb.AppendLine("<img src=\"/app-assets/images/icons/call-detail-user.svg\" />");
            sb.AppendLine("</div>");
            sb.AppendFormat("<h3>{0}</h3>", model.Detail.from);
            sb.AppendLine("</div>");
            sb.AppendLine("<div class=\"col-md-6 text-center\">");
            sb.AppendLine("<div class=\"to-avatar detail-avatar\">");
            sb.AppendLine("<img src=\"/app-assets/images/icons/call-detail-user.svg\" />");
            sb.AppendLine("</div>");
            sb.AppendFormat("<h3>{0}</h3>", model.Detail.to);
            sb.AppendLine("</div></div></div>");

            sb.AppendLine("<div class=\"content call-details-scroll\">");
            sb.AppendLine("<div class=\"tabs-page mb-5\">");
            sb.AppendLine("<ul class=\"nav nav-tabs\" role=\"tablist\">");
            sb.AppendFormat("<li role=\"presentation\" class=\"active\"><a href=\"#call-information\" aria-controls=\"call-information\" role=\"tab\" data-toggle=\"tab\" aria-expanded=\"true\" class=\"active show\" aria-selected=\"true\"><h4>{0}</h4></a></li>",
                staticService.GetLocalization("CDR_CallInformation").Data);
            sb.AppendFormat("<li role=\"presentation\" class=\"\"><a href=\"#call-details\" aria-controls=\"call-details\" role=\"tab\" data-toggle=\"tab\" aria-expanded=\"false\" class=\"\" aria-selected=\"false\"><h4>{0}</h4></a></li>",
                staticService.GetLocalization("CDR_Details").Data);
            sb.AppendFormat("<li role=\"presentation\" class=\"\"><a href=\"#sound-recording\" aria-controls=\"sound-recording\" role=\"tab\" data-toggle=\"tab\" aria-expanded=\"false\" class=\"\" aria-selected=\"false\"><h4>{0}</h4></a></li>",
                staticService.GetLocalization("CDR_Recording").Data);
            sb.AppendLine("</ul></div>");

            sb.AppendLine("<div class=\"tabs-page\">");
            sb.AppendLine("<div class=\"tab-content\">");
            sb.AppendLine("<div role=\"tabpanel\" class=\"tab-pane active show\" id=\"call-information\">");

            sb.AppendLine("<div class=\"call-detail-message-list-top\">");
            sb.AppendFormat("<p><img src=\"/app-assets/images/icons/timeline-calender-green.svg\" /> {0} {1}</p>",
                String.Format("{0:dd.MM.yyyy}", model.Detail.Date),
                model.Information.Select(x => x.starttimestring).FirstOrDefault());
            sb.AppendLine("<div class=\"clearfix\"></div>");
            sb.AppendLine("<img src=\"/app-assets/images/timeline-circle.svg\" />");
            sb.AppendLine("</div>");

            sb.AppendLine("<div class=\"call-detail-message-list\">");
            int index = -1;

            foreach (var item in model.Information)
            {
                index++;

                if (item.durum.Equals("noneed"))
                {
                    continue;
                }

                if (item.durum.Contains("Picked up") && item.act == 2)
                {
                    sb.AppendLine("<div class=\"call-detail-message-content center\">");
                    sb.AppendLine("<div class=\"call-detail-message-item\">");

                    ringOrTalTime = item.ringortalktime;
                    string resultText = GetFormattedDuration(ringOrTalTime);

                    sb.AppendFormat("<div class=\"item-text\"><img src=\"/app-assets/images/icons/timeline-bell.svg\" /> {0} : {1}</div>",
                        staticService.GetLocalization("CDR_Ringing").Data, resultText);
                    sb.AppendLine("</div></div>");

                    sb.AppendLine("<div class=\"call-detail-message-content center\" style=\"margin-top: 15px;\">");
                    sb.AppendLine("<div class=\"call-detail-message-item\" style=\"background: #ff5c72;width: 325px;\">");
                    sb.AppendFormat("<div class=\"item-text\" style=\"color: #fff;font-weight: 500;\">{0}</div>",
                        item.durum);
                    sb.AppendLine("</div></div>");

                    continue;
                }

                if (item.durum.Equals("ringing"))
                {
                    var backData = index > 0 ? model.Information[index - 1] : null;
                    ringOrTalTime = backData != null && backData.durum.Contains("Picked up") && backData.act == 2 ? new TimeSpan() : item.ringortalktime;

                    sb.AppendLine("<div class=\"call-detail-message-content center\">");
                    sb.AppendLine("<div class=\"call-detail-message-item\">");

                    string resultText = GetFormattedDuration(ringOrTalTime);

                    sb.AppendFormat("<div class=\"item-text\"><img src=\"/app-assets/images/icons/timeline-bell.svg\" /> {0} : {1}</div>",
                        staticService.GetLocalization("CDR_Ringing").Data, resultText);
                    sb.AppendLine("</div></div>");
                }
                else
                {
                    sb.AppendLine("<div class=\"call-detail-message-content " + (infoCount % 2 == 0 ? "left" : "right") + "\">");
                    sb.AppendLine("<div class=\"call-detail-message-item\">");
                    sb.AppendFormat("<div class=\"item-text\">{0}</div>", item.durum);
                    sb.AppendLine("</div>");

                    sb.AppendLine("<div class=\"call-detail-clock-item\">");
                    sb.AppendFormat("<div class=\"start-time\"><img src=\"/app-assets/images/icons/timeline-clock-green.svg\" /> {0}</div>", item.starttimestring);
                    sb.AppendLine("<div class=\"clearfix\"></div>");
                    sb.AppendFormat("<div class=\"talk-time\"><img src=\"/app-assets/images/icons/timeline-speaking.svg\" /> {0} {1}</div>",
                        GetFormattedTalkTime(item.ringortalktime), item.stoptimestring);
                    sb.AppendLine("<div class=\"clearfix\"></div>");
                    sb.AppendFormat("<div class=\"end-time\"><img src=\"/app-assets/images/icons/timeline-clock-red.svg\" /> {0}</div>", item.stoptimestring);
                    sb.AppendLine("</div>");

                    sb.AppendLine("<div class=\"clearfix\"></div>");
                    sb.AppendLine("</div>");

                    infoCount++;
                }
            }

            sb.AppendLine("</div>");

            sb.AppendLine("<div class=\"call-detail-message-list-bottom\">");
            sb.AppendLine("<img src=\"/app-assets/images/timeline-circle.svg\" />");
            sb.AppendFormat("<p><img src=\"/app-assets/images/icons/timeline-calender-red.svg\" /> {0} {1}</p>",
                String.Format("{0:dd.MM.yyyy}", model.Detail.Date),
                model.Information.Select(x => x.stoptimestring).LastOrDefault());
            sb.AppendLine("</div>");

            sb.AppendLine("</div>"); // close call-information tab

            // Call Details
            sb.AppendLine("<div role=\"tabpanel\" class=\"tab-pane report-table\" id=\"call-details\">");
            sb.AppendLine("<div class=\"row\">");
            sb.AppendFormat("<div class=\"col-md-6\"><p class=\"title\">{0}:</p></div>", staticService.GetLocalization("CDR_CallId").Data);
            sb.AppendFormat("<div class=\"col-md-6\" style=\"font-size: 16px;\">{0}</div>", model.Detail.call_id);
            sb.AppendLine("</div>");

            sb.AppendFormat("<div class=\"row\"><div class=\"col-md-6\"><p class=\"title\">{0}:</p></div>", staticService.GetLocalization("CDR_From").Data);
            sb.AppendFormat("<div class=\"col-md-6\">{0}</div></div>", model.Detail.tablecolumnfrom);
            sb.AppendFormat("<div class=\"row\"><div class=\"col-md-6\"><p class=\"title\">{0}:</p></div>", staticService.GetLocalization("CDR_To").Data);
            sb.AppendFormat("<div class=\"col-md-6\">{0}</div></div>", model.Detail.tablecolumnto);
            sb.AppendFormat("<div class=\"row\"><div class=\"col-md-6\"><p class=\"title\">{0}:</p></div>", staticService.GetLocalization("CDR_Duration").Data);
            sb.AppendFormat("<div class=\"col-md-6\">{0}</div></div>", model.Detail.duration);

            sb.AppendLine("</div>"); // close call-details tab

            // Sound Recording
            sb.AppendLine("<div role=\"tabpanel\" class=\"tab-pane report-table\" id=\"sound-recording\">");
            sb.AppendLine("<div class=\"recording-plyr\">");
            sb.AppendLine("<div class=\"plyr\" id=\"player\" data-type=\"audio\">");
            sb.AppendLine("<audio controls crossorigin playsinline id=\"audio-player\">");
            sb.AppendFormat("<source src=\"/app-assets/audio/{0}\" type=\"audio/mpeg\" />",
                CDR.Shared.Utilities.Extensions.BaseExtensions.PhotoTitle(model.Detail.from) + "-" +
                CDR.Shared.Utilities.Extensions.BaseExtensions.PhotoTitle(model.Detail.to) + ".mp3");
            sb.AppendLine("</audio>");
            sb.AppendLine("</div></div></div>"); // close sound-recording tab

            sb.AppendLine("</div>"); // close tab-content
            sb.AppendLine("</div>"); // close content

            return sb.ToString();
        }

        public static string GenerateFavoriteFiltersHtml(CDR.Entities.Dtos.ReportFavoriteFilterListDto model, IStaticService staticService)
        {
            var sb = new StringBuilder();

            if (model.ReportFilters != null && model.ReportFilters.Count > 0)
            {
                sb.Append("<div class=\"row mt-2\">");
                sb.Append("<div class=\"col-md-8 fav-title\">");
                sb.AppendFormat("<p>{0}</p>", staticService.GetLocalization("CDR_FavoriteTitle").Data);
                sb.Append("<i class=\"la la-star\"></i>");
                sb.Append("</div>");
                sb.Append("<div class=\"col-md-4\"></div>");
                sb.Append("</div>");

                foreach (var item in model.ReportFilters)
                {
                    sb.Append("<div class=\"row favorite-filter-row\">");
                    sb.Append("<div class=\"col-md-8\">");
                    sb.AppendFormat("<p>{0}</p>", item.Title);
                    sb.Append("</div>");
                    sb.Append("<div class=\"col-md-4 pl-0 pr-0 text-center\">");
                    sb.AppendFormat(
                        "<a class=\"favorite-button-view\" data-item=\"{0}\" data-toggle=\"tooltip\" title=\"{1}\" data-placement=\"left\">" +
                        "<img src=\"/app-assets/images/icons/fav-eye.svg\" /></a>",
                        item.Id,
                        staticService.GetLocalization("CDR_Filter").Data
                    );
                    sb.AppendFormat(
                        "<a class=\"favorite-button-delete\" data-item=\"{0}\" data-toggle=\"tooltip\" title=\"{1}\" data-placement=\"right\">" +
                        "<img src=\"/app-assets/images/icons/fav-trash.svg\" /></a>",
                        item.Id,
                        staticService.GetLocalization("CDR_Sil").Data
                    );

                    // Instead of Html.Raw, just append the JSON string directly
                    sb.AppendFormat("<span id=\"FavoriteFilterDataList-{0}\" style=\"display:none\">{1}</span>",
                        item.Id,
                        item.Json
                    );

                    sb.Append("</div>");
                    sb.Append("</div>");
                }
            }
            else
            {
                sb.AppendFormat("<p>{0}</p>", staticService.GetLocalization("CDR_FavoriFiltrelerinizBulunamadi").Data);
            }

            return sb.ToString();
        }

        public static string GenerateCompanyPhonebookAddHtml(CompanyPhonebookAddDto model, IStaticService staticService)
        {
            var sb = new StringBuilder();

            // Modal header
            sb.AppendFormat("<div class=\"modal-header\">" +
                            "<label class=\"modal-title text-text-bold-600\" id=\"myModalLabel25\">{0}</label>" +
                            "<button type=\"button\" class=\"close\" data-dismiss=\"modal\" aria-label=\"Close\">" +
                            "<span aria-hidden=\"true\">×</span></button></div>",
                            staticService.GetLocalization("CDR_CreateUser").Data);

            // Modal body and form
            sb.Append("<div class=\"modal-body\">");
            sb.Append("<form id=\"form-phonebook\">");

            sb.Append("<div asp-validation-summary=\"All\" class=\"text-danger\" style=\"display:none\"></div>");
            sb.AppendFormat("<input type=\"hidden\" name=\"idphonebook\" value=\"{0}\" />", model.idphonebook);

            // Form fields
            sb.AppendFormat("<div class=\"form-group\">" +
                            "<label>{0}</label>" +
                            "<input type=\"text\" class=\"form-control\" name=\"FirstName\" placeholder=\"\" value=\"{1}\" />" +
                            "</div>", staticService.GetLocalization("CDR_Name").Data, model.FirstName);

            sb.AppendFormat("<div class=\"form-group\">" +
                            "<label>{0}</label>" +
                            "<input type=\"text\" class=\"form-control\" name=\"LastName\" placeholder=\"\" value=\"{1}\" />" +
                            "</div>", staticService.GetLocalization("CDR_Surname").Data, model.LastName);

            sb.AppendFormat("<div class=\"form-group\">" +
                            "<label>{0}</label>" +
                            "<input type=\"email\" class=\"form-control\" name=\"Email\" placeholder=\"\" value=\"{1}\" />" +
                            "</div>", staticService.GetLocalization("CDR_Email").Data, model.Email);

            sb.AppendFormat("<div class=\"form-group\">" +
                            "<label>{0}</label>" +
                            "<input type=\"text\" class=\"form-control\" name=\"MobileNumber\" placeholder=\"\" value=\"{1}\" />" +
                            "</div>", staticService.GetLocalization("CDR_Mobile").Data, model.MobileNumber);

            sb.AppendFormat("<div class=\"form-group\">" +
                            "<label>{0}</label>" +
                            "<input type=\"text\" class=\"form-control\" name=\"Company\" placeholder=\"\" value=\"{1}\" />" +
                            "</div>", staticService.GetLocalization("CDR_CompanyName").Data, model.Company);

            // Close form and modal body
            sb.Append("</form>");
            sb.Append("</div>");

            // Modal footer with buttons
            sb.AppendFormat("<div class=\"modal-footer\">" +
                            "<button type=\"button\" class=\"btn grey btn-outline-secondary\" data-dismiss=\"modal\">{0}</button>" +
                            "<button type=\"button\" class=\"btn btn-outline-primary\" id=\"CreateUserButton\">{1}</button>" +
                            "</div>",
                            staticService.GetLocalization("CDR_Cancel").Data, staticService.GetLocalization("CDR_SaveChanges").Data);

            return sb.ToString();
        }
        public static string GenerateCompanyPhonebookAddHtmlv2(CompanyPhonebookAddDto model, IStaticService staticService)
        {
            var sb = new StringBuilder();

            // Modal header
            sb.Append("<div class=\"modal-header\">");
            sb.AppendFormat("<label class=\"modal-title text-text-bold-600\" id=\"myModalLabel25\">{0}</label>", staticService.GetLocalization("CDR_CreateUser").Data);
            sb.Append("<button type=\"button\" class=\"close\" data-dismiss=\"modal\" aria-label=\"Close\">");
            sb.Append("<span aria-hidden=\"true\">×</span>");
            sb.Append("</button>");
            sb.Append("</div>");

            // Modal body
            sb.Append("<div class=\"modal-body\">");
            sb.Append("<form id=\"form-phonebook\">");
            sb.Append("<div class=\"text-danger\" style=\"display:none\"></div>");

            // Hidden input for ID
            sb.AppendFormat("<input type=\"hidden\" name=\"idphonebook\" value=\"{0}\" />", model.idphonebook);

            // First Name input
            sb.Append("<div class=\"form-group\">");
            sb.AppendFormat("<label>{0}</label>", staticService.GetLocalization("CDR_Name").Data);
            sb.AppendFormat("<input type=\"text\" class=\"form-control\" name=\"FirstName\" value=\"{0}\" placeholder=\"\" />", model.FirstName);
            sb.Append("</div>");

            // Last Name input
            sb.Append("<div class=\"form-group\">");
            sb.AppendFormat("<label>{0}</label>", staticService.GetLocalization("CDR_Surname").Data);
            sb.AppendFormat("<input type=\"text\" class=\"form-control\" name=\"LastName\" value=\"{0}\" placeholder=\"\" />", model.LastName);
            sb.Append("</div>");

            // Email input
            sb.Append("<div class=\"form-group\">");
            sb.AppendFormat("<label>{0}</label>", staticService.GetLocalization("CDR_Email").Data);
            sb.AppendFormat("<input type=\"email\" class=\"form-control\" name=\"Email\" value=\"{0}\" placeholder=\"\" />", model.Email);
            sb.Append("</div>");

            // Mobile Number input
            sb.Append("<div class=\"form-group\">");
            sb.AppendFormat("<label>{0}</label>", staticService.GetLocalization("CDR_Mobile").Data);
            sb.AppendFormat("<input type=\"text\" class=\"form-control\" name=\"MobileNumber\" value=\"{0}\" placeholder=\"\" />", model.MobileNumber);
            sb.Append("</div>");

            // Company input
            sb.Append("<div class=\"form-group\">");
            sb.AppendFormat("<label>{0}</label>", staticService.GetLocalization("CDR_CompanyName").Data);
            sb.AppendFormat("<input type=\"text\" class=\"form-control\" name=\"Company\" value=\"{0}\" placeholder=\"\" />", model.Company);
            sb.Append("</div>");

            sb.Append("</form>");
            sb.Append("</div>");

            // Modal footer
            sb.Append("<div class=\"modal-footer\">");
            sb.AppendFormat("<button type=\"button\" class=\"btn grey btn-outline-secondary\" data-dismiss=\"modal\">{0}</button>", staticService.GetLocalization("CDR_Cancel").Data);
            sb.AppendFormat("<button type=\"button\" class=\"btn btn-outline-primary\" id=\"CreateUserButton\">{0}</button>", staticService.GetLocalization("CDR_SaveChanges").Data);
            sb.Append("</div>");

            return sb.ToString();
        }

        public static string GenerateDownloadPageWindowHtml(IStaticService _staticService)
        {
            var sb = new StringBuilder();

            // Start Carousel Div
            sb.AppendLine("<div class=\"owl-carousel owl-theme\" id=\"OwlDownload\">");

            // First Item - Download Section Title
            sb.AppendLine("<div class=\"item\"><div class=\"row\"><div class=\"col-md-12\">");
            sb.AppendFormat("<h1 class=\"text-center\">{0}</h1>", _staticService.GetLocalization("CDR_Download_Win_Title").Data);
            sb.AppendLine("</div></div>");

            // Windows Downloads
            sb.AppendLine("<div class=\"row\"><div class=\"col-md-6\">");
            sb.AppendFormat("<h3>{0}</h3>", _staticService.GetLocalization("CDR_Download_Win_Subtitle").Data);
            sb.AppendLine("<ul>");
            sb.AppendFormat("<li>{0}</li>", _staticService.GetLocalization("CDR_Download_Win_2016").Data);
            sb.AppendFormat("<li>{0}</li>", _staticService.GetLocalization("CDR_Download_Win_2019").Data);
            sb.AppendFormat("<li>{0}</li>", _staticService.GetLocalization("CDR_Download_Win_10Pro").Data);
            sb.AppendFormat("<li>{0}</li>", _staticService.GetLocalization("CDR_Download_Win_10Enterprise").Data);
            sb.AppendFormat("<li>{0}</li>", _staticService.GetLocalization("CDR_Download_Win_11").Data);
            sb.AppendLine("</ul>");
            sb.AppendFormat("<p>{0}</p>", _staticService.GetLocalization("CDR_Download_Win_Note").Data);
            sb.AppendLine("</div>");

            // Windows Requirements
            sb.AppendLine("<div class=\"col-md-6\">");
            sb.AppendFormat("<h3>{0}</h3>", _staticService.GetLocalization("CDR_Download_Win_Requirements").Data);
            sb.AppendLine("<ul>");
            sb.AppendFormat("<li>{0}</li>", _staticService.GetLocalization("CDR_Download_Win_Requirements_Item1").Data);
            sb.AppendFormat("<li>{0}</li>", _staticService.GetLocalization("CDR_Download_Win_Requirements_Item2").Data);
            sb.AppendLine("<li>");
            sb.AppendFormat("{0}<ul>", _staticService.GetLocalization("CDR_Download_Win_Requirements_Item3").Data);
            sb.AppendFormat("<li>{0}</li>", _staticService.GetLocalization("CDR_Download_Win_Requirements_Item3_Sub-item1").Data);
            sb.AppendFormat("<li>{0}</li>", _staticService.GetLocalization("CDR_Download_Win_Requirements_Item3_Sub-item2").Data);
            sb.AppendLine("</ul></li>");
            sb.AppendLine("</ul>");
            sb.AppendFormat("<p>{0}</p>", _staticService.GetLocalization("CDR_Download_Win_Requirements_Note").Data);
            sb.AppendLine("</div></div></div>");

            // Additional Carousel Items with Images
            var imageItems = new List<string>
    {
        "win1.png", "win2.png", "win3.png", "win4.png", "profilesetting1.png",
        "profilesetting2.png", "profilesetting3.png"
    };
            var textItems = new List<string>
    {
        "CDR_Download_Win_Setup_Title", "CDR_Download_Win_Setup_Win", "CDR_Download_Win_Setup_Item1",
        "CDR_Download_Win_Setup_Step2", "CDR_Download_Win_Setup_Step3", "CDR_Download_Win_Setup_Step4",
        "CDR_Download_Profilesetting_Title", "CDR_Download_Profilesetting_Item1", "CDR_Download_Profilesetting_Item2",
        "CDR_Download_Profilesetting_Item3", "CDR_Download_Profilesetting2_Item1", "CDR_Download_Profilesetting2_Item2",
        "CDR_Download_Profilesetting2_Item3", "CDR_Download_Profilesetting2_Note", "CDR_Download_Profilesetting3"
    };

            // Loop through additional items, alternating text and images
            for (int i = 0; i < imageItems.Count; i++)
            {
                sb.AppendLine("<div class=\"item\"><div class=\"row\">");

                // Text Content
                sb.AppendLine("<div class=\"col-md-6\"><ul>");
                sb.AppendFormat("<li>{0}</li>", _staticService.GetLocalization(textItems[i]).Data);
                sb.AppendLine("</ul></div>");

                // Image Content
                sb.AppendFormat("<div class=\"col-md-6\"><img src=\"/app-assets/images/download-page/windows/{0}\" /></div>", imageItems[i]);
                sb.AppendLine("</div></div>");
            }

            // Final item with additional profile settings
            sb.AppendLine("<div class=\"item\"><div class=\"row\"><div class=\"col-md-12\">");
            sb.AppendFormat("<h1 class=\"text-center\">{0}</h1>", _staticService.GetLocalization("CDR_Download_Profilesetting_Title").Data);
            sb.AppendLine("</div><div class=\"col-md-6\"><ul>");
            sb.AppendFormat("<li>{0}</li>", _staticService.GetLocalization("CDR_Download_Profilesetting_Item1").Data);
            sb.AppendFormat("<li>{0}</li>", _staticService.GetLocalization("CDR_Download_Profilesetting_Item2").Data);
            sb.AppendFormat("<li>{0}</li>", _staticService.GetLocalization("CDR_Download_Profilesetting_Item3").Data);
            sb.AppendLine("</ul></div>");
            sb.AppendLine("<div class=\"col-md-6\"><img src=\"/app-assets/images/download-page/profilesetting1.png\" /></div>");
            sb.AppendLine("</div></div>");

            // Close Carousel Div
            sb.AppendLine("</div>");

            return sb.ToString();
        }

        public static string GenerateDownloadLinuxPageHtml(IStaticService staticService)
        {
            var sb = new StringBuilder();

            sb.Append("<div class=\"owl-carousel owl-theme\" id=\"OwlDownload\">");

            // First section
            sb.Append("<div class=\"item\"><div class=\"row\">");
            sb.Append("<div class=\"col-md-12\"><h1 class=\"text-center\">" + staticService.GetLocalization("CDR_Download_Win_Title").Data + "</h1></div>");

            sb.Append("<div class=\"col-md-6\">");
            sb.Append("<h3>" + staticService.GetLocalization("CDR_Download_Lin_Subtitle").Data + "</h3><ul>");
            sb.Append("<li>" + staticService.GetLocalization("CDR_Download_Lin_10").Data + "</li></ul></div>");

            sb.Append("<div class=\"col-md-6\">");
            sb.Append("<h3>" + staticService.GetLocalization("CDR_Download_Win_Requirements").Data + "</h3><ul>");
            sb.Append("<li>" + staticService.GetLocalization("CDR_Download_Lin_Requirements_Item1").Data + "</li>");
            sb.Append("<li>" + staticService.GetLocalization("CDR_Download_Lin_Requirements_Item2").Data + "</li>");
            sb.Append("<li>" + staticService.GetLocalization("CDR_Download_Lin_Requirements_Item3").Data + "<ul>");
            sb.Append("<li>" + staticService.GetLocalization("CDR_Download_Lin_Requirements_Item3_Sub-item1").Data + "</li>");
            sb.Append("<li>" + staticService.GetLocalization("CDR_Download_Lin_Requirements_Item3_Sub-item2").Data + "</li></ul></li>");
            sb.Append("</ul><p>" + staticService.GetLocalization("CDR_Download_Lin_Requirements_Item3_Note").Data + "</p>");
            sb.Append("</div></div></div>");

            // Second section
            sb.Append("<div class=\"item\"><div class=\"row\">");
            sb.Append("<div class=\"col-md-6\"><h1>" + staticService.GetLocalization("CDR_Download_Win_Setup_Title").Data + "</h1>");
            sb.Append("<h3>" + staticService.GetLocalization("CDR_Download_Lin_Setup_Lin").Data + "</h3><ul>");
            sb.Append("<li>" + staticService.GetLocalization("CDR_Download_Lin_Setup_Item1").Data + "</li>");
            sb.Append("<li>" + staticService.GetLocalization("CDR_Download_Lin_Setup_Item2").Data + "</li></ul></div>");
            sb.Append("<div class=\"col-md-6\"><img src=\"/app-assets/images/download-page/linux/lin1.png\" /></div></div></div>");

            // Third section
            sb.Append("<div class=\"item\"><div class=\"row\">");
            sb.Append("<div class=\"col-md-6\"><ul>");
            sb.Append("<li>" + staticService.GetLocalization("CDR_Download_Lin_Setup_Step2").Data + "</li></ul></div>");
            sb.Append("<div class=\"col-md-6\"><img src=\"/app-assets/images/download-page/linux/lin2.png\" /></div></div></div>");

            // Fourth section
            sb.Append("<div class=\"item\"><div class=\"row\">");
            sb.Append("<div class=\"col-md-6\"><ul>");
            sb.Append("<li>" + staticService.GetLocalization("CDR_Download_Lin_Setup_Step3").Data + "</li></ul></div>");
            sb.Append("<div class=\"col-md-6\"><img src=\"/app-assets/images/download-page/linux/lin3.png\" /></div></div></div>");

            // Fifth section
            sb.Append("<div class=\"item\"><div class=\"row\">");
            sb.Append("<div class=\"col-md-6\"><ul>");
            sb.Append("<li>" + staticService.GetLocalization("CDR_Download_Lin_Setup_Step4").Data + "</li></ul></div>");
            sb.Append("<div class=\"col-md-6\"><img src=\"/app-assets/images/download-page/linux/lin4.png\" /></div></div></div>");

            // Profile Setting section
            sb.Append("<div class=\"item\"><div class=\"row\">");
            sb.Append("<div class=\"col-md-12\"><h1 class=\"text-center\">" + staticService.GetLocalization("CDR_Download_Profilesetting_Title").Data + "</h1></div>");
            sb.Append("<div class=\"col-md-6\"><ul>");
            sb.Append("<li>" + staticService.GetLocalization("CDR_Download_Profilesetting_Item1").Data + "</li>");
            sb.Append("<li>" + staticService.GetLocalization("CDR_Download_Profilesetting_Item2").Data + "</li>");
            sb.Append("<li>" + staticService.GetLocalization("CDR_Download_Profilesetting_Item3").Data + "</li></ul></div>");
            sb.Append("<div class=\"col-md-6\"><img src=\"/app-assets/images/download-page/profilesetting1.png\" /></div></div></div>");

            // Additional Profile Settings sections
            sb.Append("<div class=\"item\"><div class=\"row\">");
            sb.Append("<div class=\"col-md-6\"><ul>");
            sb.Append("<li>" + staticService.GetLocalization("CDR_Download_Profilesetting2_Item1").Data + "</li>");
            sb.Append("<li>" + staticService.GetLocalization("CDR_Download_Profilesetting2_Item2").Data + "</li>");
            sb.Append("<li>" + staticService.GetLocalization("CDR_Download_Profilesetting2_Item3").Data + "</li></ul>");
            sb.Append("<p>" + staticService.GetLocalization("CDR_Download_Profilesetting2_Note").Data + "</p></div>");
            sb.Append("<div class=\"col-md-6\"><img src=\"/app-assets/images/download-page/profilesetting2.png\" /></div></div></div>");

            // Final Profile Settings section
            sb.Append("<div class=\"item\"><div class=\"row\">");
            sb.Append("<div class=\"col-md-6\"><ul>");
            sb.Append("<li>" + staticService.GetLocalization("CDR_Download_Profilesetting3").Data + "</li></ul></div>");
            sb.Append("<div class=\"col-md-6\"><img src=\"/app-assets/images/download-page/profilesetting3.png\" /></div></div></div>");

            sb.Append("</div>");

            return sb.ToString();
        }





        public static string GenerateSupportAddHtml(SupportAddDto model, IStaticService staticService, bool stateError)
        {
            var sb = new StringBuilder();

            // Fetch support categories from the service
            var supportCategories = staticService.GetAllSupportCategories().Data;

            // Modal header
            sb.AppendLine("<div class=\"modal-header\">");
            sb.AppendFormat("<h5 class=\"modal-title\" id=\"exampleModalLabel\">{0}</h5>",
                staticService.GetLocalization("CDR_DestekOlustur").Data);
            sb.AppendLine("<button type=\"button\" class=\"close\" data-dismiss=\"modal\" aria-label=\"Close\">");
            sb.AppendLine("<span aria-hidden=\"true\">&times;</span>");
            sb.AppendLine("</button>");
            sb.AppendLine("</div>");

            // Modal body
            sb.AppendLine("<div class=\"modal-body\">");
            sb.AppendLine("<form id=\"form-add-support-information\">");
            if (stateError == true)
            {

                sb.AppendLine("<div class=\"text-danger\" id=\"validation-summary\" aria-hidden=\"true\" \">" +
                    $"{model.ModelError}" +
                    "</div>");
            }



            // Support Categories dropdown
            sb.AppendLine("<div class=\"form-group\">");
            sb.AppendFormat("<label>{0}</label>", staticService.GetLocalization("CDR_SupportCategories").Data);
            sb.AppendLine("<select class=\"form-control bselect\" data-live-search=\"true\" name=\"SupportCategoryId\">");
            sb.AppendFormat("<option value=\"0\" selected disabled>{0}</option>",
                staticService.GetLocalization("CDR_PleaseSelectCategories").Data);
            foreach (var item in supportCategories)
            {
                sb.AppendFormat("<option value=\"{0}\">{1}</option>", item.Id, item.Name);
            }
            sb.AppendLine("</select>");
            sb.AppendLine("</div>");

            // Support Message textarea
            sb.AppendLine("<div class=\"form-group\">");
            sb.AppendFormat("<label>{0}</label>", staticService.GetLocalization("CDR_Mesajiniz").Data);
            sb.AppendFormat("<textarea class=\"form-control\" placeholder=\"{0}\" name=\"SupportMessage\" style=\" height: 225px;resize:none\"></textarea>",
                staticService.GetLocalization("CDR_Mesajiniz").Data);
            sb.AppendLine("</div>");
            sb.AppendLine("</form>");
            sb.AppendLine("</div>");

            // Modal footer
            sb.AppendLine("<div class=\"modal-footer\">");
            sb.AppendFormat("<button type=\"button\" class=\"btn btn-secondary\" data-dismiss=\"modal\">{0}</button>",
                staticService.GetLocalization("CDR_Close").Data);
            sb.AppendFormat("<button type=\"button\" class=\"btn btn-primary\" id=\"AddSupport\">{0}</button>",
                staticService.GetLocalization("CDR_Send").Data);
            sb.AppendLine("</div>");

            return sb.ToString();
        }
        public static string GenerateSupportListHtml(SupportListDto supportListDto, IStaticService staticService)
        {
            var sb = new StringBuilder();

            if (supportListDto.Supports != null && supportListDto.Supports.Count > 0)
            {
                foreach (var support in supportListDto.Supports)
                {
                    // Get the first message and the 'isSeen' status
                    var message = support.SupportMessages.FirstOrDefault() ?? new SupportMessages();
                    bool isSeen = support.SupportMessages.Any(x => !x.IsSeenUser);

                    // Start the list item
                    sb.Append("<li class=\"card support-list-item\" data-item=\"")
                      .Append(support.PublicId)
                      .Append("\">")
                      .Append("<div class=\"row\">");

                    // Notification indicator if unseen
                    if (isSeen)
                    {
                        sb.Append("<span class=\"notification-support-list\"></span>");
                    }

                    // Profile image
                    sb.Append("<div class=\"col-md-2\">")
                      .Append("<img class=\"list-profile-photo\" src=\"/app-assets/images/admin.png\" />")
                      .Append("</div>");

                    // Support details
                    sb.Append("<div class=\"col-md-10\">")
                      .AppendFormat("<p class=\"categories\">{0}</p>", support.Id + 100000)
                      .AppendFormat("<p class=\"date\"><i class=\"ft-calendar\"></i> {0:dd.MM.yyyy}</p>", support.CreatedDate)
                      .Append("<div class=\"clearfix\"></div>");

                    // Support status
                    if (support.Statue == (byte)SupportStatue.SOLVED)
                    {
                        sb.Append("<div class=\"support-statue solved\"><i class=\"ft-check\"></i> ")
                          .Append(staticService.GetLocalization("CDR_SupportSolved").Data)
                          .Append("</div>");
                    }
                    else if (support.Statue == (byte)SupportStatue.NOT_SOLVED)
                    {
                        sb.Append("<div class=\"support-statue not-solved\"><i class=\"ft-x\"></i> ")
                          .Append(staticService.GetLocalization("CDR_SupportNotSolved").Data)
                          .Append("</div>");
                    }
                    else if (support.Statue == (byte)SupportStatue.CANCELLED)
                    {
                        sb.Append("<div class=\"support-statue sp-cancelled\"><i class=\"ft-trash-2\"></i> ")
                          .Append(staticService.GetLocalization("CDR_SupportCancelled").Data)
                          .Append("</div>");
                    }

                    // Support category and message
                    sb.AppendFormat("<p class=\"list-subject\">{0}</p>", support.Category.Name)
                      .Append("<div class=\"text\"><p>")
                      .Append(message.Text ?? "")
                      .Append("</p></div>")
                      .Append("</div>")
                      .Append("</div>")
                      .Append("</li>");
                }
            }
            else
            {

                sb.Append("<p class=\"SupportNotItem\">")
                  .Append(staticService.GetLocalization("CDR_DestekIstegiBulunamadi").Data)
                  .Append("</p>");
            }

            return sb.ToString();
        }

        public static string GenerateSupportMessagesHtml(SupportMessageListModel supportMessages, IStaticService staticService)
        {
            var sb = new StringBuilder();

            if (supportMessages != null && supportMessages.SupportMessages.Count > 0)
            {
                foreach (var message in supportMessages.SupportMessages)
                {
                    // Start each message item
                    sb.Append("<div class=\"support-message-item\">");

                    // Header section
                    sb.Append("<div class=\"support-message-item-header\">");

                    // Left section - Admin/User profile
                    if (message.IsAdmin)
                    {
                        sb.Append("<div class=\"left\">");
                        sb.Append("<img src=\"/app-assets/images/admin.png\" /> Admin");
                        sb.Append("</div>");
                    }
                    else
                    {
                        sb.Append("<div class=\"left\">");
                        sb.AppendFormat("<img src=\"{0}\" /> {1} {2}",
                           CDR.Shared.Utilities.Extensions.BaseExtensions.ProfilePicture(supportMessages.User.FirstName.Substring(0, 1).ToLower()),
                           supportMessages.User.FirstName,
                            supportMessages.User.LastName);
                        sb.Append("</div>");
                    }

                    // Right section - Notification & Date
                    sb.Append("<div class=\"right\">");

                    // Notification for new messages
                    if (supportMessages.NewMessages.Any(x => x == message.Id))
                    {
                        sb.AppendFormat("<span class=\"new-notification\">{0}</span>",
                            staticService.GetLocalization("CDR_New").Data);
                    }

                    // Date and time
                    sb.Append("<i class=\"ft-calendar\"></i>");
                    sb.AppendFormat("{0:dd.MM.yyyy HH:mm}", message.CreatedDate);
                    sb.Append("</div><div class=\"clearfix\"></div></div>");

                    // Body section - Message text
                    sb.Append("<div class=\"support-message-item-body\">");
                    sb.Append(message.Text.Replace("\n", "<br>"));
                    sb.Append("</div></div>");
                }
            }
            else
            {
                // No messages case
                sb.AppendFormat("<p>{0}</p>", staticService.GetLocalization("CDR_DestekMesajiBulunmamaktadir").Data);
            }

            return sb.ToString();
        }

        public static string GeneratePasswordChangeHtml(UserPasswordChangeDto userPasswordChangeDto, IStaticService staticService, bool stateError)
        {
            var sb = new StringBuilder();

            // Hidden field for validation state
            sb.Append("<form id=\"form-user-change-password\">");
            if (stateError == true)
            {

                sb.AppendLine("<div class=\"text-danger\" id=\"validation-summary\" aria-hidden=\"true\" \">" +
                    $"{userPasswordChangeDto.ModelError}" +
                    "</div>");
            }

            // Current password field
            sb.Append("<div class=\"form-group\" id=\"form-user-change-password\">");
            sb.AppendFormat("<label>{0}</label>", staticService.GetLocalization("CDR_CurrentPassword").Data);
            sb.Append("<input type=\"password\" class=\"form-control\" name=\"CurrentPassword\" id=\"CurPassword\"/>");
            sb.Append("</div>");

            // New password field
            sb.Append("<div class=\"form-group\">");
            sb.AppendFormat("<label>{0}</label>", staticService.GetLocalization("CDR_NewPassword").Data);
            sb.Append("<input type=\"password\" class=\"form-control\" name=\"NewPassword\" id=\"NPassword\" />");
            sb.Append("</div>");

            // Repeat password field
            sb.Append("<div class=\"form-group\">");
            sb.AppendFormat("<label>{0}</label>", staticService.GetLocalization("CDR_ReNewPassword").Data);
            sb.Append("<input type=\"password\" class=\"form-control\" name=\"RepeatPassword\" id=\"RePassword\" />");
            sb.Append("</div>");

            // Submit button
            sb.Append("<div class=\"form-group text-right\">");
            sb.AppendFormat("<button class=\"btn btn-primary\" id=\"SavePassword\">{0}</button>", staticService.GetLocalization("CDR_SaveChanges").Data);
            sb.Append("</div>");

            sb.Append("</form>");

            return sb.ToString();
        }

        public static string GenerateUserTimezoneHtml(UserTimezoneDto model, IStaticService staticService, bool stateError)
        {
            var sb = new StringBuilder();

            // Start the form
            sb.AppendLine("<form asp-action=\"Timezone\" id=\"form-user-timezone\">");

            // Hidden input for IsValid
            if (stateError == true)
            {

                sb.AppendLine("<div class=\"text-danger\" id=\"validation-summary\" aria-hidden=\"true\" \">" +
                    $"{model.ModelError}" +
                    "</div>");
            }

            // Select dropdown row
            sb.AppendLine("<div class=\"row text-center ml-1\">");
            sb.AppendLine("<select class=\"js-Selector\"></select>");
            sb.AppendLine("</div>");

            // Save button
            sb.AppendLine("<div class=\"form-group text-right\">");
            sb.AppendFormat("<button class=\"btn btn-primary\" id=\"SaveTimezone\">{0}</button>",
                staticService.GetLocalization("CDR_SaveChanges").Data);
            sb.AppendLine("</div>");

            // End the form
            sb.AppendLine("</form>");

            return sb.ToString();
        }

        public static string GenerateUserProfileHtml(UserProfileInformationDto model, IStaticService staticService, bool stateError)
        {
            var sb = new StringBuilder();

            // Fetch country list from the service
            var countryList = staticService.GetAllCountry().Data;

            // Form start
            sb.AppendLine("<form asp-action=\"ProfileInformation\" id=\"form-user-profile-information\">");


            if (stateError == true)
            {

                sb.AppendLine("<div class=\"text-danger\" id=\"validation-summary\" aria-hidden=\"true\" \">" +
                    $"{model.ModelError}" +
                    "</div>");
            }

            // First Name
            sb.AppendLine("<div class=\"form-group\">");
            sb.AppendFormat("<label>{0}*</label>", staticService.GetLocalization("CDR_Name").Data);
            sb.AppendFormat("<input type=\"text\" name=\"FirstName\"  id=\"FirstName\" class=\"form-control\" value=\"{0}\" />", model.FirstName);
            sb.AppendLine("</div>");

            // Last Name
            sb.AppendLine("<div class=\"form-group\">");
            sb.AppendFormat("<label>{0}*</label>", staticService.GetLocalization("CDR_Surname").Data);
            sb.AppendFormat("<input type=\"text\" name=\"LastName\" id=\"LastName\" class=\"form-control\" value=\"{0}\" />", model.LastName);
            sb.AppendLine("</div>");

            // Email
            sb.AppendLine("<div class=\"form-group\">");
            sb.AppendFormat("<label>{0}*</label>", staticService.GetLocalization("CDR_Email").Data);
            sb.AppendFormat("<input type=\"text\" name=\"Email\" id=\"Email\" class=\"form-control\" value=\"{0}\" />", model.Email);
            sb.AppendLine("</div>");

            // Phone Number
            sb.AppendLine("<div class=\"form-group\">");
            sb.AppendFormat("<label>{0}*</label>", staticService.GetLocalization("CDR_Phone").Data);
            sb.AppendFormat("<input type=\"text\" name=\"PhoneNumber\" id=\"PhoneNumber\" class=\"form-control\" value=\"{0}\" />", model.PhoneNumber);
            sb.AppendLine("</div>");

            // Company Name
            sb.AppendLine("<div class=\"form-group\">");
            sb.AppendFormat("<label>{0}*</label>", staticService.GetLocalization("CDR_CompanyName").Data);
            sb.AppendFormat("<input type=\"text\" name=\"CompanyName\" id=\"CompanyName\" class=\"form-control\" value=\"{0}\" />", model.CompanyName);
            sb.AppendLine("</div>");

            // Country Dropdown
            sb.AppendLine("<div class=\"form-group\">");
            sb.AppendFormat("<label>{0}</label>", staticService.GetLocalization("CDR_Country").Data);
            sb.AppendLine("<select class=\"form-control bselect\" data-live-search=\"true\" name=\"CountryId\" id=\"CountryId\">");
            sb.AppendFormat("<option value=\"0\" selected disabled>{0}</option>", staticService.GetLocalization("CDR_PleaseSelectCountry").Data);
            foreach (var item in countryList)
            {
                sb.AppendFormat("<option value=\"{0}\">{1}</option>", item.Id, item.Name);
            }
            sb.AppendLine("</select>");
            sb.AppendLine("</div>");

            // City Dropdown
            sb.AppendLine("<div class=\"form-group\">");
            sb.AppendFormat("<label>{0}</label>", staticService.GetLocalization("CDR_City").Data);
            sb.AppendLine("<select class=\"form-control bselect\" data-live-search=\"true\" name=\"CityId\" id=\"CityId\">");
            sb.AppendFormat("<option value=\"0\" selected disabled>{0}</option>", staticService.GetLocalization("CDR_PleaseSelectCountry").Data);
            sb.AppendLine("</select>");
            sb.AppendLine("</div>");

            // Address
            sb.AppendLine("<div class=\"form-group\">");
            sb.AppendFormat("<label>{0}</label>", staticService.GetLocalization("CDR_Address").Data);
            sb.AppendFormat("<textarea class=\"form-control\" name=\"Address\" id=\"Address\"  maxlength=\"2000\" style=\"resize:none\">{0}</textarea>", model.Address);
            sb.AppendLine("</div>");

            // Zip Code
            sb.AppendLine("<div class=\"form-group\">");
            sb.AppendFormat("<label>{0}</label>", staticService.GetLocalization("CDR_ZipCode").Data);
            sb.AppendFormat("<input type=\"text\" name=\"ZipCode\" id=\"ZipCode\" class=\"form-control\" maxlength=\"10\" value=\"{0}\" />", model.ZipCode);
            sb.AppendLine("</div>");

            // Submit Button
            sb.AppendLine("<div class=\"form-group text-right\">");
            sb.AppendFormat("<button class=\"btn btn-primary\" id=\"SaveUserInformation\">{0}</button>", staticService.GetLocalization("CDR_SaveChanges").Data);
            sb.AppendLine("</div>");

            // Form end
            sb.AppendLine("</form>");

            return sb.ToString();
        }

        public static string GenerateUserConnectionDetailHtml(UserConnectionDetailDto model, IStaticService staticService, bool stateError)
        {
            var sb = new StringBuilder();

            // Form start
            sb.AppendLine("<form id=\"form-user-connection-detail\">");
            if (stateError == true)
            {

                sb.AppendLine("<div class=\"text-danger\" id=\"validation-summary\" aria-hidden=\"true\" \">" +
                    $"{model.ModelError}" +
                    "</div>");
            }

            // IP Address
            sb.AppendLine("<div class=\"form-group\">");
            sb.AppendFormat("<label>{0}</label>", staticService.GetLocalization("CDR_IpAdress").Data);
            sb.AppendFormat(
                "<input type=\"text\" name=\"IpAddress\" id=\"IpAddressCon\"  class=\"form-control\" value=\"{0}\" />",
                string.IsNullOrWhiteSpace(model.IpAddress) ? "" : model.IpAddress
            );
            sb.AppendLine("</div>");

            // Port
            sb.AppendLine("<div class=\"form-group\">");
            sb.AppendFormat("<label>{0}</label>", staticService.GetLocalization("CDR_Port").Data);
            sb.AppendLine("<select class=\"form-control bselect\" name=\"Port\" id=\"PortCon\">");
            sb.AppendLine("<option value=\"5480\">WINDOWS (5480)</option>");
            sb.AppendLine("<option value=\"5432\">LINUX (5432)</option>");
            sb.AppendLine("</select>");
            sb.AppendLine("</div>");

            // Database Name
            sb.AppendLine("<div class=\"form-group\">");
            sb.AppendFormat("<label>{0}</label>", staticService.GetLocalization("CDR_DatabaseName").Data);
            sb.AppendFormat(
                "<input type=\"text\" name=\"DbName\" id=\"DbNameCon\" class=\"form-control\" value=\"{0}\" />",
                string.IsNullOrWhiteSpace(model.DbName) ? "" : model.DbName
            );
            sb.AppendLine("</div>");

            // Database Username
            sb.AppendLine("<div class=\"form-group\">");
            sb.AppendFormat("<label>{0}</label>", staticService.GetLocalization("CDR_DatabaseUsername").Data);
            sb.AppendFormat(
                "<input type=\"text\" name=\"DbUsername\" id=\"ConDbUsername\" class=\"form-control\" value=\"{0}\" />",
                string.IsNullOrWhiteSpace(model.DbUsername) ? "" : model.DbUsername
            );
            sb.AppendLine("</div>");

            // Database Password
            sb.AppendLine("<div class=\"form-group\">");
            sb.AppendFormat("<label>{0}", staticService.GetLocalization("CDR_DatabasePassword").Data);
            if (!string.IsNullOrWhiteSpace(model.DbPassword))
            {
                sb.AppendFormat(
                    " <span class=\"badge badge-success\">{0}</span>",
                    staticService.GetLocalization("CDR_PasswordSaved").Data
                );
            }
            sb.AppendLine("</label>");
            sb.AppendLine("<input type=\"password\" name=\"DbPassword\" id=\"ConDbPassword\" class=\"form-control\" />");
            sb.AppendLine("</div>");

            // 3CX Version
            sb.AppendLine("<div class=\"form-group\">");
            sb.AppendFormat("<label>{0}</label>", staticService.GetLocalization("CDR_3CXVersion").Data);

            var enumVersion = Enum.GetValues(typeof(VERSION3CX));
            for (int i = 0; i < enumVersion.Length; i++)
            {
                sb.AppendLine("<fieldset class=\"radio\">");
                sb.AppendLine("<label>");
                sb.AppendFormat(
                    "<input type=\"radio\" name=\"Version\" class=\"option-input radio\" value=\"{0}\" /> {1}",
                    i + 1,
                    ((VERSION3CX)enumVersion.GetValue(i)).GetDescription()
                );
                sb.AppendLine("</label>");
                sb.AppendLine("</fieldset>");
            }
            sb.AppendLine("</div>");

            // Save button
            sb.AppendLine("<div class=\"form-group text-right\">");
            sb.AppendFormat("<button class=\"btn btn-primary\" id=\"Save\">{0}</button>", staticService.GetLocalization("CDR_SaveChanges").Data);
            sb.AppendLine("</div>");

            // Form end
            sb.AppendLine("</form>");

            return sb.ToString();
        }

        public static string GenerateMembershipBuyPackageHtml(MembershipBuyPackageModel model, IStaticService staticService)
        {
            var sb = new StringBuilder();

            // Card Header
            sb.AppendLine("<div class=\"card\">");
            sb.AppendLine("  <div class=\"card-header\">");
            sb.AppendFormat("    <h4 class=\"card-title\">{0}</h4>", staticService.GetLocalization("CDR_BuyPackage").Data);
            sb.AppendLine("    <a class=\"heading-elements-toggle\"><i class=\"la la-ellipsis-v font-medium-3\"></i></a>");
            sb.AppendLine("    <div class=\"heading-elements\">");
            sb.AppendLine("      <ul class=\"list-inline mb-0\">");
            sb.AppendLine("        <li><a data-action=\"collapse\"><i class=\"ft-minus\"></i></a></li>");
            sb.AppendLine("      </ul>");
            sb.AppendLine("    </div>");
            sb.AppendLine("  </div>");

            // Card Content
            sb.AppendLine("  <div class=\"card-content collapse show\">");
            sb.AppendLine("    <div class=\"card-body profile-content\">");
            sb.AppendLine("      <form action=\"#\" class=\"number-tab-steps wizard-circle\">");

            // Packages Section
            sb.AppendFormat("        <h6>{0}</h6>", staticService.GetLocalization("CDR_Packages").Data);
            sb.AppendLine("        <fieldset>");
            sb.AppendLine("          <div class=\"row mt-2 justify-content-center\">");
            foreach (var package in model.PackageList)
            {
                sb.AppendLine("            <div class=\"col-xl-4 col-md-6 col-12\">");
                sb.AppendLine("              <div class=\"card profile-card-with-cover package-list-item\">");
                sb.AppendFormat("                <input type=\"hidden\" class=\"SelectedPackageInput\" value=\"{0}\" />", package.PublicId);
                sb.AppendLine("                <div class=\"card-content card-deck text-center\">");
                sb.AppendLine("                  <div class=\"card box-shadow\">");
                sb.AppendLine("                    <div class=\"card-body\">");
                sb.AppendFormat("                      <h2 class=\"my-0 font-weight-bold\">{0}</h2>", package.Name);
                sb.AppendFormat("                      <h1 class=\"pricing-card-title\">{0} <small class=\"text-muted\">{1}</small></h1>",
                    package.MonthPrice.ToNumberString(suffix: CDR.Shared.Utilities.Extensions.BaseExtensions.CurrencyIcon((CDR.Shared.Utilities.Results.ComplexTypes.Enums.Currency)package.Currency), countAfterCommas: 2),
                    staticService.GetLocalization("CDR_mo").Data);
                sb.AppendLine("                      <ul class=\"list-unstyled mt-2 mb-2\" style=\"min-height: 50px;\">");
                if (package.Month == 12)
                {
                    sb.AppendFormat("                        <li>{0}</li>", staticService.GetLocalization("CDR_PricePerMonth").Data);
                }
                sb.AppendLine("                      </ul>");
                sb.AppendLine("                    </div>");
                sb.AppendLine("                    <div class=\"card-footer\">");
                sb.AppendFormat("                      <button type=\"button\" class=\"btn btn-lg btn-block package-button btn-outline-warning selectedPackage\">{0}</button>",
                    staticService.GetLocalization("CDR_SelectPackage").Data);
                sb.AppendLine("                    </div>");
                sb.AppendLine("                  </div>");
                sb.AppendLine("                </div>");
                sb.AppendLine("              </div>");
                sb.AppendLine("            </div>");
            }
            sb.AppendLine("          </div>");
            sb.AppendLine("        </fieldset>");

            // Invoice Section
            sb.AppendFormat("        <h6>{0}</h6>", staticService.GetLocalization("CDR_InvoiceInformation").Data);
            sb.AppendLine("        <fieldset>");
            sb.AppendLine("          <div class=\"step-invoice-content\">");
            sb.AppendLine("            <div class=\"alert bg-info alert-icon-left alert-dismissible mt-1\" role=\"alert\">");
            sb.AppendFormat("              <span class=\"alert-icon\"><i class=\"la la-info\"></i></span>{0}", staticService.GetLocalization("CDR_InvoiceInfoText").Data);
            sb.AppendLine("            </div>");
            sb.AppendFormat("            <div class=\"form-group\"><label>{0}</label>", staticService.GetLocalization("CDR_Country").Data);
            sb.AppendLine("              <select class=\"form-control bselect\" data-live-search=\"true\" id=\"invoice-country\">");
            sb.AppendFormat("                <option value=\"0\" selected disabled>{0}</option>", staticService.GetLocalization("CDR_PleaseSelectCountry").Data);
            foreach (var country in model.Countries)
            {
                sb.AppendFormat("                <option value=\"{0}\">{1}</option>", country.Id, country.Name);
            }
            sb.AppendLine("              </select>");
            sb.AppendLine("            </div>");

            // Address and Zip Code
            sb.AppendFormat("            <div class=\"form-group\"><label>{0}</label><textarea class=\"form-control\" id=\"invoice-address\" maxlength=\"2000\" style=\"resize:none\">{1}</textarea></div>",
                staticService.GetLocalization("CDR_Address").Data, model.User.Address);
            sb.AppendFormat("            <div class=\"form-group\"><label>{0}</label><input type=\"text\" class=\"form-control\" maxlength=\"10\" value=\"{1}\" id=\"invoice-zip-code\" /></div>",
                staticService.GetLocalization("CDR_ZipCode").Data, model.User.ZipCode);
            sb.AppendLine("          </div>");
            sb.AppendLine("        </fieldset>");

            // Credit Card Section
            sb.AppendFormat("        <h6>{0}</h6>", staticService.GetLocalization("CDR_CreditCard").Data);
            sb.AppendLine("        <fieldset>");
            sb.AppendLine("          <div id=\"credit-card-container\"></div>");
            sb.AppendLine("        </fieldset>");

            sb.AppendLine("      </form>");
            sb.AppendLine("    </div>");
            sb.AppendLine("  </div>");
            sb.AppendLine("</div>");

            return sb.ToString();
        }
        public static string GenerateMembershipCreditCardHtml(MembershipCreditCardModel model, IStaticService staticService)
        {
            var sb = new StringBuilder();

            if (model.ResultStatus == CDR.Shared.Utilities.Results.ComplexTypes.ResultStatus.Success)
            {
                // Start of the row
                sb.AppendLine("<div class=\"row justify-content-center\">");

                // Left Column
                sb.AppendLine("<div class=\"col-md-5\">");
                sb.AppendLine("<div class=\"checkout-form-overlay\">");
                sb.AppendLine("<div class=\"checkout-form-content\">");

                // Checkbox for Distance Selling Contract
                sb.AppendLine("<fieldset class=\"checkbox\">");
                sb.AppendLine("<label>");
                sb.AppendLine("<input type=\"checkbox\" class=\"option-input checkbox not-click\" name=\"contract-check\" id=\"DistanceSellingContractCheck\">");
                sb.AppendLine(staticService.GetLocalization("CDR_SatisOkudumOnayliyorum").Data);
                sb.AppendLine("</label>");
                sb.AppendLine("</fieldset>");

                // Checkbox for Privacy Agreement
                sb.AppendLine("<fieldset class=\"checkbox\">");
                sb.AppendLine("<label>");
                sb.AppendLine("<input type=\"checkbox\" class=\"option-input checkbox not-click\" name=\"contract-check\" id=\"PrivacyAgreementCheck\">");
                sb.AppendLine(staticService.GetLocalization("CDR_PrivacyAgreementOkudumOnayliyorum").Data);
                sb.AppendLine("</label>");
                sb.AppendLine("</fieldset>");

                sb.AppendLine("</div>");
                sb.AppendLine("</div>");
                sb.AppendLine("<div id=\"iyzipay-checkout-form\" class=\"responsive\"></div>");
                sb.AppendLine("</div>");

                // Right Column
                sb.AppendLine("<div class=\"col-md-5\">");

                // Package Details
                sb.AppendLine("<div class=\"select-package-details\">");
                sb.AppendFormat("<h2 style=\"margin-bottom: 15px; border-bottom: 1px solid #b3b5da; padding-bottom: 8px;\">{0}</h2>",
                    staticService.GetLocalization("CDR_PackageDetails").Data);

                sb.AppendFormat("<p>{0}: <strong>{1}</strong></p>",
                    staticService.GetLocalization("CDR_SelectedPackage").Data, model.DepositDetail.PackageName);
                sb.AppendFormat("<p>{0}: <strong>{1}</strong></p>",
                    staticService.GetLocalization("CDR_MonthlyPrice").Data,
                    model.DepositDetail.MontlyPrice.ToNumberString(
                        suffix: CDR.Shared.Utilities.Extensions.BaseExtensions.CurrencyIcon(
                            (CDR.Shared.Utilities.Results.ComplexTypes.Enums.Currency)model.DepositDetail.Currency),
                        countAfterCommas: 2));
                sb.AppendFormat("<p>{0}: <strong>{1}</strong></p>",
                    staticService.GetLocalization("CDR_TotalPrice").Data,
                    model.DepositDetail.YearPrice.ToNumberString(
                        suffix: CDR.Shared.Utilities.Extensions.BaseExtensions.CurrencyIcon(
                            (CDR.Shared.Utilities.Results.ComplexTypes.Enums.Currency)model.DepositDetail.Currency),
                        countAfterCommas: 2));
                sb.AppendFormat("<p>{0}: <strong>{1}</strong></p>",
                    staticService.GetLocalization("CDR_Month").Data, model.DepositDetail.Month);

                if (!string.IsNullOrWhiteSpace(model.ExhangeRate))
                {
                    sb.AppendFormat("<p>Euro Kur: <strong>{0}</strong></p>", model.ExhangeRate);
                }
                sb.AppendLine("</div>");

                // Invoice Information
                sb.AppendLine("<div class=\"select-package-details\">");
                sb.AppendFormat("<h2 style=\"margin-bottom: 15px; border-bottom: 1px solid #b3b5da; padding-bottom: 8px;\">{0}</h2>",
                    staticService.GetLocalization("CDR_InvoiceInformation").Data);

                sb.AppendFormat("<p>{0}: <strong>{1}</strong></p>",
                    staticService.GetLocalization("CDR_Country").Data, model.DepositDetail.Country);
                sb.AppendFormat("<p>{0}: <strong>{1}</strong></p>",
                    staticService.GetLocalization("CDR_City").Data, model.DepositDetail.City);
                sb.AppendFormat("<p>{0}: <strong>{1}</strong></p>",
                    staticService.GetLocalization("CDR_Address").Data, model.DepositDetail.Address);
                sb.AppendFormat("<p>{0}: <strong>{1}</strong></p>",
                    staticService.GetLocalization("CDR_ZipCode").Data,
                    string.IsNullOrWhiteSpace(model.DepositDetail.ZipCode) ? "-" : model.DepositDetail.ZipCode);
                sb.AppendLine("</div>");

                sb.AppendLine("</div>");
                sb.AppendLine("</div>");

                // Render Checkout Form
                sb.AppendLine(model.CheckoutForm);
            }
            else
            {
                sb.AppendFormat("<div class=\"alert alert-danger\">{0}</div>", model.Message);
            }

            return sb.ToString();
        }



        private static string GetFormattedDuration(TimeSpan duration)
        {
            return string.Format("{0:D2}:{1:D2}:{2:D2}", (int)duration.TotalHours, duration.Minutes, duration.Seconds);
        }

        private static string GetFormattedTalkTime(TimeSpan talkTime)
        {
            return string.Format("{0:D2}:{1:D2}:{2:D2}", (int)talkTime.TotalHours, talkTime.Minutes, talkTime.Seconds);
        }

        private static string FormatTimeSpan(TimeSpan timeSpan)
        {
            string hours = timeSpan.TotalHours < 10 ? "0" + (int)timeSpan.TotalHours : ((int)timeSpan.TotalHours).ToString();
            string minutes = timeSpan.Minutes < 10 ? "0" + timeSpan.Minutes : timeSpan.Minutes.ToString();
            string seconds = timeSpan.Seconds < 10 ? "0" + timeSpan.Seconds : timeSpan.Seconds.ToString();
            return $"{hours} h : {minutes} m : {seconds} s";
        }




    }

}
