using CDR.Shared.Utilities.Results.ComplexTypes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CDR.Shared.Utilities.Extensions
{
    public static class ReportFilterExtensions
    {
        public static string InternalSourceDn { get { return "0, 9"; } }
        public static string InternalTargetDn { get { return "0"; } }
        public static string InboundSourceDn { get { return "1, 8"; } }
        public static string InboundTargetDn { get { return "0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 12, 13, 14"; } }
        public static string OutboundSourceDn { get { return "0, 6"; } }
        public static string OutboundTargetDn { get { return "1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 12, 13, 14"; } }
        public static string OutboundInternalSourceDn { get { return "0, 4, 6, 9"; } }
        public static string OutboundInternalTargetDn { get { return "0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 12, 13, 14"; } }
        public static string InboundInternalSourceDn { get { return "0, 1, 8, 9"; } }
        public static string InboundInternalTargetDn { get { return "0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 12, 13, 14"; } }
        public static string CompletedPsactionid { get { return "5,6,7"; } }
        public static string CompletedPodidntype { get { return "6,12"; } }
        public static string MissedPsactionid { get { return "5,6,7"; } }
        public static string MissedPosactionid { get { return "13,15,101,102,103,104,107,400,406,407,408,409,410,412,415,417,418,419,420,421,422,423,425,432"; } }
        public static string MissedPodidntype { get { return "12"; } }

        public static BaseExtensions.FilterDateDto WhereDatesSql(int? Dates, DateTime? CustomStart, DateTime? CustomEnd, decimal UserGMT, DateTime now)
        {
            var _result = new BaseExtensions.FilterDateDto();

            var _d = Dates == null ? Enums.ReportFilterDate.ALL : (Enums.ReportFilterDate)Dates;

            DateTime _now = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day);
            DateTime _start = _now;
            DateTime _end = _now;

            switch (_d)
            {
                case Enums.ReportFilterDate.ALL:
                    _start = new DateTime(DateTime.Now.Year, 1, 1);
                    _end = now;
                    break;
                case Enums.ReportFilterDate.LASTDAY:
                    _start = _now.AddDays(-1);
                    _end = _now;
                    break;
                case Enums.ReportFilterDate.LAST7DAYS:
                    _start = _now.AddDays(-7);
                    _end = _now;
                    break;
                case Enums.ReportFilterDate.LAST30DAYS:
                    _start = _now.AddMonths(-1);
                    _end = _now;
                    break;
                case Enums.ReportFilterDate.LAST3MONTHS:
                    _start = _now.AddMonths(-3);
                    _end = _now;
                    break;
                case Enums.ReportFilterDate.LAST6MONTHS:
                    _start = _now.AddMonths(-6);
                    _end = _now;
                    break;
                case Enums.ReportFilterDate.FROMTHEBEGENNINGOFTHISDAY:
                    _start = _now;
                    _end = now;
                    break;
                case Enums.ReportFilterDate.FROMTHEBEGENNINGOFTHISWEEK:
                    _start = BaseExtensions.FirstDayOfWeek(_now);
                    _end = now;
                    break;
                case Enums.ReportFilterDate.FROMTHEBEGENNINGOFTHISMONTH:
                    _start = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
                    _end = now;
                    break;
                case Enums.ReportFilterDate.FROMTHEBEGENNINGOFTHISYEAR:
                    _start = new DateTime(DateTime.Now.Year, 1, 1);
                    _end = now;
                    break;
                case Enums.ReportFilterDate.CUSTOM:
                    TimeSpan gmtTime = -1 * BaseExtensions.GetGmtTime(UserGMT);

                    if (CustomStart != null && CustomEnd == null)
                    {
                        _start = CustomStart ?? _now;
                        _end = _now;
                    }
                    else if (CustomStart == null && CustomEnd != null)
                    {
                        _end = CustomEnd ?? _now;
                    }
                    else
                    {
                        _start = CustomStart ?? _now;
                        _end = CustomEnd ?? _now;
                    }

                    _start = _start.Add(gmtTime);
                    _end = _end.Add(gmtTime);
                    break;
                default:
                    break;
            }

            _result.StartDate = _start;
            _result.EndDate = _end;

            return _result;
        }
        public static BaseExtensions.FilterStatueDto WhereStatuesSql(int? Statue)
        {
            var _result = new BaseExtensions.FilterStatueDto();

            if (Statue == null)
                return _result;

            var _d = (Enums.ReportFilterStatue)Statue;

            switch (_d)
            {
                case Enums.ReportFilterStatue.ANSWEREDANDUNANSWEREDCALLS:
                    break;
                case Enums.ReportFilterStatue.ANSWEREDCALLS:
                    _result.Psactionid = CompletedPsactionid;
                    _result.Posactionid = CompletedPodidntype;
                    break;
                case Enums.ReportFilterStatue.UNANSWEREDCALLS:
                    _result.Psactionid = MissedPsactionid;
                    _result.Posactionid = MissedPosactionid;
                    _result.Podidntype = MissedPodidntype;
                    break;
                default:
                    break;
            }

            return _result;
        }
        public static BaseExtensions.FilterDnDto WhereInboundOutboundSql(int? Source, int? Target, int? SourceCriteria, int? TargetCriteria)
        {
            var _sql = new BaseExtensions.FilterDnDto
            {
                SourceDn = "0",
                TargetDn = "0"
            };

            if (Source == null && Target == null)
                return _sql;

            if (Source != null && ((Enums.ReportFilterSource)Source) == Enums.ReportFilterSource.ALL && Target == null) //INBOUND-OUTBOUND-EXTERNAL
                return _sql;
            else if (Source != null && ((Enums.ReportFilterSource)Source) == Enums.ReportFilterSource.EXTERNALNUMBER && Target == null) //INBOUND
            {
                _sql.SourceDn = InboundSourceDn;
                _sql.TargetDn = InboundTargetDn;
            }
            else if (Source != null && ((Enums.ReportFilterSource)Source) == Enums.ReportFilterSource.INTERNALNUMBER && Target == null) //OUTBOUND-EXT2EXT
            {
                _sql.SourceDn = InboundInternalSourceDn;
                _sql.TargetDn = InboundInternalTargetDn;
            }
            else if (Target != null && ((Enums.ReportFilterSource)Target) == Enums.ReportFilterSource.ALL && Source == null) //INBOUND-OUTBOUND-EXTERNAL
                return _sql;
            else if (Target != null && ((Enums.ReportFilterSource)Target) == Enums.ReportFilterSource.EXTERNALNUMBER && Source == null) //OUTBOUND
            {
                _sql.SourceDn = OutboundSourceDn;
                _sql.TargetDn = OutboundTargetDn;
            }
            else if (Target != null && ((Enums.ReportFilterSource)Target) == Enums.ReportFilterSource.INTERNALNUMBER && Source == null) //INBOUND - EXT2EXT
            {
                _sql.SourceDn = InboundInternalSourceDn;
                _sql.TargetDn = InboundInternalTargetDn;
            }
            else if (((Enums.ReportFilterSource)Source) == Enums.ReportFilterSource.ALL && ((Enums.ReportFilterSource)Target) == Enums.ReportFilterSource.ALL) //INBOUND-OUTBOUND-EXTERNAL
                return _sql;
            else if (((Enums.ReportFilterSource)Source) == Enums.ReportFilterSource.ALL && ((Enums.ReportFilterSource)Target) == Enums.ReportFilterSource.EXTERNALNUMBER) //OUTBOUND
            {
                _sql.SourceDn = OutboundSourceDn;
                _sql.TargetDn = OutboundTargetDn;
            }
            else if (((Enums.ReportFilterSource)Source) == Enums.ReportFilterSource.ALL && ((Enums.ReportFilterSource)Target) == Enums.ReportFilterSource.INTERNALNUMBER) //INBOUND-EXT2EXT
            {
                _sql.SourceDn = InternalSourceDn;
                _sql.TargetDn = InternalTargetDn;
                _sql.OtherSourceDn = InboundSourceDn;
                _sql.OtherTargetDn = InboundTargetDn;
            }
            else if (((Enums.ReportFilterSource)Source) == Enums.ReportFilterSource.EXTERNALNUMBER && ((Enums.ReportFilterSource)Target) == Enums.ReportFilterSource.ALL) //INBOUND
            {
                _sql.SourceDn = InboundSourceDn;
                _sql.TargetDn = InboundTargetDn;
            }
            else if (((Enums.ReportFilterSource)Source) == Enums.ReportFilterSource.EXTERNALNUMBER && ((Enums.ReportFilterSource)Target) == Enums.ReportFilterSource.EXTERNALNUMBER)
            {
                return _sql;
            }
            else if (((Enums.ReportFilterSource)Source) == Enums.ReportFilterSource.EXTERNALNUMBER && ((Enums.ReportFilterSource)Target) == Enums.ReportFilterSource.INTERNALNUMBER) //INBOUND
            {
                _sql.SourceDn = InboundSourceDn;
                _sql.TargetDn = InboundTargetDn;
            }
            else if (((Enums.ReportFilterSource)Source) == Enums.ReportFilterSource.INTERNALNUMBER && ((Enums.ReportFilterSource)Target) == Enums.ReportFilterSource.ALL) //OUTBOUND-EXT2EXT
            {
                //number gelirse eğer sadece outbound, ext gelirse internal, değilse şuanki gibi
                if (TargetCriteria == (int)Enums.ReportFilterSourceCriteria.ALL)
                {
                    _sql.SourceDn = OutboundInternalSourceDn;
                    _sql.TargetDn = OutboundInternalTargetDn;
                }
                else if(TargetCriteria == (int)Enums.ReportFilterSourceCriteria.EXTENSIONORRANGEOFEXTENSION)
                {
                    _sql.SourceDn = InternalSourceDn;
                    _sql.TargetDn = InternalTargetDn;
                }
                else
                {
                    _sql.SourceDn = OutboundSourceDn;
                    _sql.TargetDn = OutboundTargetDn;
                }
            }
            else if (((Enums.ReportFilterSource)Source) == Enums.ReportFilterSource.INTERNALNUMBER && ((Enums.ReportFilterSource)Target) == Enums.ReportFilterSource.EXTERNALNUMBER) //OUTBOUND
            {
                _sql.SourceDn = OutboundSourceDn;
                _sql.TargetDn = OutboundTargetDn;
            }
            else if (((Enums.ReportFilterSource)Source) == Enums.ReportFilterSource.INTERNALNUMBER && ((Enums.ReportFilterSource)Target) == Enums.ReportFilterSource.INTERNALNUMBER) //EXT2EXT
            {
                _sql.SourceDn = InternalSourceDn;
                _sql.TargetDn = InternalTargetDn;
            }

            return _sql;
        }
        public static BaseExtensions.FilterExtNumberDto WhereNumberExtNumber(int? Source, int? Target, string SourceInput, string TargetInput)
        {
            var _sql = new BaseExtensions.FilterExtNumberDto
            {
                SourceCriteria = "",
                TargetCriteria = ""
            };

            SourceInput = string.IsNullOrWhiteSpace(SourceInput) ? SourceInput : SourceInput.Trim();
            TargetInput = string.IsNullOrWhiteSpace(TargetInput) ? TargetInput : TargetInput.Trim();

            try
            {
                if (Source != null && (Target == null || ((Enums.ReportFilterSourceCriteria)Target) == Enums.ReportFilterSourceCriteria.ALL))
                {
                    var _d = (Enums.ReportFilterSourceCriteria)Source;

                    switch (_d)
                    {
                        case Enums.ReportFilterSourceCriteria.ALL:
                            break;
                        case Enums.ReportFilterSourceCriteria.EXTENSIONORRANGEOFEXTENSION:
                            List<int> _list = new List<int>();
                            var _sourceInputs = SourceInput.Split(",").ToList();

                            foreach (var item in _sourceInputs)
                            {
                                if (item.Contains("-"))
                                {
                                    int min = Convert.ToInt32(item.Split("-")[0]);
                                    int max = Convert.ToInt32(item.Split("-")[1]);

                                    for (int i = min; i <= max; i++)
                                        _list.Add(i);
                                }
                                else
                                {
                                    _list.Add(Convert.ToInt32(item));
                                }
                            }

                            _list = _list.Distinct().ToList();

                            string ExtNoList = string.Join(",", _list.Select(x => x.ToString()).ToArray());

                            _sql.SourceCriteria = ExtNoList;
                            break;
                        case Enums.ReportFilterSourceCriteria.NUMBERS:
                            _sql.SourceCriteria = SourceInput + "**number**";
                            break;
                        case Enums.ReportFilterSourceCriteria.NUMBERSCONTAINS:
                            _sql.SourceCriteria = SourceInput + "**number**";
                            break;
                        default:
                            break;
                    }
                }
                else if ((Source == null || ((Enums.ReportFilterSourceCriteria)Source) == Enums.ReportFilterSourceCriteria.ALL) && Target != null)
                {
                    var _d = (Enums.ReportFilterSourceCriteria)Target;

                    switch (_d)
                    {
                        case Enums.ReportFilterSourceCriteria.ALL:
                            break;
                        case Enums.ReportFilterSourceCriteria.EXTENSIONORRANGEOFEXTENSION:
                            List<int> _list = new List<int>();
                            var _targetInputs = TargetInput.Split(",").ToList();

                            foreach (var item in _targetInputs)
                            {
                                if (item.Contains("-"))
                                {
                                    int min = Convert.ToInt32(item.Split("-")[0]);
                                    int max = Convert.ToInt32(item.Split("-")[1]);

                                    for (int i = min; i <= max; i++)
                                        _list.Add(i);
                                }
                                else
                                {
                                    _list.Add(Convert.ToInt32(item));
                                }
                            }

                            _list = _list.Distinct().ToList();

                            string ExtNoList = string.Join(",", _list.Select(x => x.ToString()).ToArray());

                            _sql.TargetCriteria = ExtNoList;
                            break;
                        case Enums.ReportFilterSourceCriteria.NUMBERS:
                            _sql.TargetCriteria = TargetInput + "**number**";
                            break;
                        case Enums.ReportFilterSourceCriteria.NUMBERSCONTAINS:
                            _sql.TargetCriteria = TargetInput + "**number**";
                            break;
                        default:
                            break;
                    }
                }
                else
                {
                    var _dSource = (Enums.ReportFilterSourceCriteria)Source;
                    var _dTarget = (Enums.ReportFilterSourceCriteria)Target;

                    var _sourceWhereText = string.Empty;
                    var _targetWhereText = string.Empty;

                    switch (_dSource)
                    {
                        case Enums.ReportFilterSourceCriteria.ALL:
                            _sourceWhereText = string.Empty;
                            break;
                        case Enums.ReportFilterSourceCriteria.EXTENSIONORRANGEOFEXTENSION:
                            List<int> _list = new List<int>();
                            var _sourceInputs = SourceInput.Split(",").ToList();

                            foreach (var item in _sourceInputs)
                            {
                                if (item.Contains("-"))
                                {
                                    int min = Convert.ToInt32(item.Split("-")[0]);
                                    int max = Convert.ToInt32(item.Split("-")[1]);

                                    for (int i = min; i <= max; i++)
                                        _list.Add(i);
                                }
                                else
                                {
                                    _list.Add(Convert.ToInt32(item));
                                }
                            }

                            _list = _list.Distinct().ToList();

                            string ExtNoList = string.Join(",", _list.Select(x => x.ToString()).ToArray());

                            _sourceWhereText = ExtNoList;
                            break;
                        case Enums.ReportFilterSourceCriteria.NUMBERS:
                            _sourceWhereText = SourceInput + "**number**";
                            break;
                        case Enums.ReportFilterSourceCriteria.NUMBERSCONTAINS:
                            _sourceWhereText = SourceInput + "**number**";
                            break;
                        default:
                            _sourceWhereText = string.Empty;
                            break;
                    }

                    switch (_dTarget)
                    {
                        case Enums.ReportFilterSourceCriteria.ALL:
                            _targetWhereText = string.Empty;
                            break;
                        case Enums.ReportFilterSourceCriteria.EXTENSIONORRANGEOFEXTENSION:
                            List<int> _list = new List<int>();
                            var _sourceInputs = TargetInput.Split(",").ToList();

                            foreach (var item in _sourceInputs)
                            {
                                if (item.Contains("-"))
                                {
                                    int min = Convert.ToInt32(item.Split("-")[0]);
                                    int max = Convert.ToInt32(item.Split("-")[1]);

                                    for (int i = min; i <= max; i++)
                                        _list.Add(i);
                                }
                                else
                                {
                                    _list.Add(Convert.ToInt32(item));
                                }
                            }

                            _list = _list.Distinct().ToList();

                            string ExtNoList = string.Join(",", _list.Select(x => x.ToString()).ToArray());

                            _targetWhereText = ExtNoList;
                            break;
                        case Enums.ReportFilterSourceCriteria.NUMBERS:
                            _targetWhereText = TargetInput + "**number**";
                            break;
                        case Enums.ReportFilterSourceCriteria.NUMBERSCONTAINS:
                            _targetWhereText = TargetInput + "**number**";
                            break;
                        default:
                            _targetWhereText = string.Empty;
                            break;
                    }

                    _sql.SourceCriteria = _sourceWhereText;
                    _sql.TargetCriteria = _targetWhereText;
                }
            }
            catch (Exception)
            {
            }

            return _sql;
        }
    }
}
