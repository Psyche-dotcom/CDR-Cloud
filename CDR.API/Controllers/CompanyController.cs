using AutoMapper;
using CDR.API.Api.Model;
using CDR.API.Api.Service.Interface;
using CDR.API.PartialViewGeneration;
using CDR.Entities.Concrete;
using CDR.Entities.Dtos;
using CDR.Services.Abstract;
using CDR.Shared.Utilities.Extensions;
using CDR.Shared.Utilities.Results.ComplexTypes;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using System.IdentityModel.Tokens.Jwt;
using System.Text;

namespace CDR.API.Controllers
{
    [Authorize(AuthenticationSchemes = "Bearer")]
    [Route("api/company")]
    [ApiController]
    public class CompanyController : ControllerBase
    {
        private readonly IQueriesService _queriesService;
        private readonly IUserService _userService;
        private readonly IContentService _contentService;
        private readonly GlobalSettings _globalSettings;
        private readonly UserManager<User> _userManager;
        private readonly IMapper _mapper;
        private readonly IPostgreSqlService _postgreSqlService;
        private readonly IReportFilterService _reportFilterService;
        private readonly IWebHostEnvironment _env;


        private readonly IStaticService _staticService;

        public CompanyController(IStaticService staticService,
            IMapper mapper,
            IOptions<GlobalSettings> globalSettings,
            UserManager<User> userManager,
            IContentService contentService,
            IQueriesService queriesService,
            IUserService userService,
            IPostgreSqlService postgreSqlService,
            IReportFilterService reportFilterService,
            IWebHostEnvironment env)
        {
            _staticService = staticService;
            _mapper = mapper;
            _globalSettings = globalSettings.Value;
            _userManager = userManager;
            _contentService = contentService;
            _queriesService = queriesService;
            _userService = userService;
            _postgreSqlService = postgreSqlService;
            _reportFilterService = reportFilterService;
            _env = env;
        }
        [HttpGet("Trunk")]

        public async Task<IActionResult> Trunks()
        {
            var userId = User.Claims.FirstOrDefault(c => c.Type == JwtRegisteredClaimNames.Jti).Value;

            var result = await _userService.UserInfoAsync(userId);
            if (result.StatusCode == 200)
            {
                return Ok(result);
            }
            else if (result.StatusCode == 404)
            {
                return NotFound(result);
            }
            else
            {
                return BadRequest(result);
            }
        }

        [HttpPost("GetMostContacted")]
        public async Task<IActionResult> GetMostContacted(byte? _f)
        {
            var userId = User.Claims.FirstOrDefault(c => c.Type == JwtRegisteredClaimNames.Jti).Value;

            Shared.Utilities.Results.ComplexTypes.Enums.GraphFilter _filter = (_f == null || _f == 0) ? Shared.Utilities.Results.ComplexTypes.Enums.GraphFilter.MONTHLY : (Shared.Utilities.Results.ComplexTypes.Enums.GraphFilter)_f;

            var _filters = 0;

            if (_filter == Shared.Utilities.Results.ComplexTypes.Enums.GraphFilter.WEEKLY)
                _filters = (int)Shared.Utilities.Results.ComplexTypes.Enums.ConsoleQueryType.COMPANY_PROFILE_MOST_CONTACTED_WEEKLY_CALLS;
            else if (_filter == Shared.Utilities.Results.ComplexTypes.Enums.GraphFilter.DAILY)
                _filters = (int)Shared.Utilities.Results.ComplexTypes.Enums.ConsoleQueryType.COMPANY_PROFILE_MOST_CONTACTED_DAILY_CALLS;
            else if (_filter == Shared.Utilities.Results.ComplexTypes.Enums.GraphFilter.MONTHLY)
                _filters = (int)Shared.Utilities.Results.ComplexTypes.Enums.ConsoleQueryType.COMPANY_PROFILE_MOST_CONTACTED_MONTHLY_CALLS;
            else
                _filters = (int)Shared.Utilities.Results.ComplexTypes.Enums.ConsoleQueryType.COMPANY_PROFILE_MOST_CONTACTED_YEARLY_CALLS;

            var query = _staticService.GetQueries(LoggedInUser(userId), (Shared.Utilities.Results.ComplexTypes.Enums.ConsoleQueryType)_filters);

            if (query.ResultStatus == ResultStatus.Success)
            {
                var obj = new List<CompanyMostContactedDto>();

                string queryData = query.Data;

                if (!string.IsNullOrWhiteSpace(queryData) && !queryData.Equals("[]"))
                    obj = JsonConvert.DeserializeObject<List<CompanyMostContactedDto>>(queryData);

                return Ok(new CompanyMostContactedModel
                {
                    DataList = obj,
                    DataNameList = obj.Select(x => x.d_name).ToList()
                });
            }
            else
                return Ok(query.Data);


        }

        [HttpPost("GetTotalCalls")]
        public async Task<IActionResult> GetTotalCalls(byte? _f)
        {

            var userId = User.Claims.FirstOrDefault(c => c.Type == JwtRegisteredClaimNames.Jti).Value;

            var _filter = _f == null || _f == 0 ? Shared.Utilities.Results.ComplexTypes.Enums.GraphFilter.YEARLY : (Shared.Utilities.Results.ComplexTypes.Enums.GraphFilter)_f;

            var _filters = 0;

            if (_filter == Shared.Utilities.Results.ComplexTypes.Enums.GraphFilter.WEEKLY)
                _filters = (int)Shared.Utilities.Results.ComplexTypes.Enums.ConsoleQueryType.COMPANY_PROFILE_MONTHLY_TOTAL_CALLS;
            else if (_filter == Shared.Utilities.Results.ComplexTypes.Enums.GraphFilter.MONTHLY)
                _filters = (int)Shared.Utilities.Results.ComplexTypes.Enums.ConsoleQueryType.COMPANY_PROFILE_MONTHLY_TOTAL_CALLS;
            else
                _filters = (int)Shared.Utilities.Results.ComplexTypes.Enums.ConsoleQueryType.COMPANY_PROFILE_MONTHLY_TOTAL_CALLS;

            var query = _staticService.GetQueries(LoggedInUser(userId), (Shared.Utilities.Results.ComplexTypes.Enums.ConsoleQueryType)_filters);

            if (query.ResultStatus == ResultStatus.Success)
            {
                var obj = new List<DashboardGraphDto>();

                string queryData = query.Data;

                if (!string.IsNullOrWhiteSpace(queryData) && !queryData.Equals("[]"))
                    obj = JsonConvert.DeserializeObject<List<DashboardGraphDto>>(queryData);

                var mdl = new CompanyTotalCallsModel();

                if (_filter == Shared.Utilities.Results.ComplexTypes.Enums.GraphFilter.WEEKLY)
                    mdl.xAxis = obj.Select(x => x.day + "." + (x.month < 10 ? "0" + x.month : x.month.ToString())).ToList();
                else if (_filter == Shared.Utilities.Results.ComplexTypes.Enums.GraphFilter.MONTHLY)
                    mdl.xAxis = obj.Select(x => x.day + "." + (x.month < 10 ? "0" + x.month : x.month.ToString())).ToList();
                else if (_filter == Shared.Utilities.Results.ComplexTypes.Enums.GraphFilter.YEARLY)
                    mdl.xAxis = obj.Select((x, i) => Years.Where(y => y.Order == i + 1).Select(y => y.Value).FirstOrDefault()).ToList();

                mdl.InboundList = obj.Select(x => x.inbound).ToList();
                mdl.OutboundList = obj.Select(x => x.outbound).ToList();
                mdl.TotalList = obj.Select(x => x.total).ToList();

                return Ok(mdl);
            }
            else
                return Ok(query.Data);

        }

        [HttpPost("GetCallSummary")]
        public async Task<IActionResult> GetCallSummary(byte? _f)
        {

            var userId = User.Claims.FirstOrDefault(c => c.Type == JwtRegisteredClaimNames.Jti).Value;

            var _filter = _f == null || _f == 0 ? Shared.Utilities.Results.ComplexTypes.Enums.CallSummaryFilter.HOURLY : (Shared.Utilities.Results.ComplexTypes.Enums.CallSummaryFilter)_f;

            var _filters = 0;

            if (_filter == Shared.Utilities.Results.ComplexTypes.Enums.CallSummaryFilter.HOURLY)
                _filters = (int)Shared.Utilities.Results.ComplexTypes.Enums.ConsoleQueryType.COMPANY_PROFILE_CALL_SUMMARY_HOURLY_CALLS;
            else if (_filter == Shared.Utilities.Results.ComplexTypes.Enums.CallSummaryFilter.DAILY)
                _filters = (int)Shared.Utilities.Results.ComplexTypes.Enums.ConsoleQueryType.COMPANY_PROFILE_CALL_SUMMARY_DAILY_CALLS;
            else
                _filters = (int)Shared.Utilities.Results.ComplexTypes.Enums.ConsoleQueryType.COMPANY_PROFILE_CALL_SUMMARY_DAILY_CALLS;

            var query = _staticService.GetQueries(LoggedInUser(userId), (Shared.Utilities.Results.ComplexTypes.Enums.ConsoleQueryType)_filters);

            if (query.ResultStatus == ResultStatus.Success)
            {
                var obj = new List<CompanyCallSummaryDto>();

                string queryData = query.Data;

                if (!string.IsNullOrWhiteSpace(queryData) && !queryData.Equals("[]"))
                    obj = JsonConvert.DeserializeObject<List<CompanyCallSummaryDto>>(queryData);

                var mdl = new CompanyCallSummaryModel();

                mdl.inbound = obj.Select(x => x.totalinbound).ToList();
                mdl.outbound = obj.Select(x => x.totaloutbound).ToList();
                mdl.missed = obj.Select(x => x.totalmissed).ToList();
                mdl.abandoned = obj.Select(x => x.totalabandoned).ToList();
                mdl.dates = obj.Select(x => x.date).ToList();
                mdl.ext2ext = obj.Select(x => x.totalext2ext).ToList();

                if (_filter == Shared.Utilities.Results.ComplexTypes.Enums.CallSummaryFilter.HOURLY)
                {
                    TimeSpan gmtTime = BaseExtensions.GetGmtTime(LoggedInUser(userId).GMT);

                    for (int i = 0; i < mdl.dates.Count; i++)
                    {
                        var _temp = TimeSpan.Parse(mdl.dates[i]);
                        var _add = _temp.Add(gmtTime);

                        mdl.dates[i] = _add.ToString().Substring(0, 5);
                    }
                }


                return Ok(mdl);
            }
            else
                return Ok(query.Data);


        }


        [HttpGet("GeneralStatistic")]
        public async Task<IActionResult> GeneralStatistic(byte? _f)
        {

            var userId = User.Claims.FirstOrDefault(c => c.Type == JwtRegisteredClaimNames.Jti).Value;

            Shared.Utilities.Results.ComplexTypes.Enums.DashboardFilter _filter = (_f == null || _f == 0) ? Shared.Utilities.Results.ComplexTypes.Enums.DashboardFilter.MONTHLY : (Shared.Utilities.Results.ComplexTypes.Enums.DashboardFilter)_f;

            var _filters = 0;

            if (_filter == Shared.Utilities.Results.ComplexTypes.Enums.DashboardFilter.DAILY)
                _filters = (int)Shared.Utilities.Results.ComplexTypes.Enums.ConsoleQueryType.COMPANY_PROFILE_GENERAL_STATISTIC_DAILY;
            else if (_filter == Shared.Utilities.Results.ComplexTypes.Enums.DashboardFilter.WEEKLY)
                _filters = (int)Shared.Utilities.Results.ComplexTypes.Enums.ConsoleQueryType.COMPANY_PROFILE_GENERAL_STATISTIC_WEEKLY;
            else if (_filter == Shared.Utilities.Results.ComplexTypes.Enums.DashboardFilter.MONTHLY)
                _filters = (int)Shared.Utilities.Results.ComplexTypes.Enums.ConsoleQueryType.COMPANY_PROFILE_GENERAL_STATISTIC_MONTHLY;
            else
                _filters = (int)Shared.Utilities.Results.ComplexTypes.Enums.ConsoleQueryType.COMPANY_PROFILE_GENERAL_STATISTIC_YEARLY;

            var query = _staticService.GetQueries(LoggedInUser(userId), (Shared.Utilities.Results.ComplexTypes.Enums.ConsoleQueryType)_filters);

            if (query.ResultStatus == ResultStatus.Success)
            {
                var obj = new CompanyGeneralStatisticDto();

                if (!string.IsNullOrWhiteSpace(query.Data))
                    obj = JsonConvert.DeserializeObject<CompanyGeneralStatisticDto>(query.Data);

                var htmlContent = PartialViewGen.GenerateCompanyGeneralStatisticsHtml(new CompanyGeneralStatisticModel
                {
                    Data = obj
                }, _staticService);
                return Ok(htmlContent);
            }
            else
            {
                var htmlContent2 = PartialViewGen.GenerateCompanyGeneralStatisticsHtml(new CompanyGeneralStatisticModel()
               , _staticService);
                return Ok(htmlContent2);
            }

        }


        [HttpGet("InternalStatistic")]
        public async Task<IActionResult> InternalStatistic(byte? _f)
        {

            var userId = User.Claims.FirstOrDefault(c => c.Type == JwtRegisteredClaimNames.Jti).Value;


            Shared.Utilities.Results.ComplexTypes.Enums.DashboardFilter _filter = (_f == null || _f == 0) ? Shared.Utilities.Results.ComplexTypes.Enums.DashboardFilter.MONTHLY : (Shared.Utilities.Results.ComplexTypes.Enums.DashboardFilter)_f;

            var _filters = 0;

            if (_filter == Shared.Utilities.Results.ComplexTypes.Enums.DashboardFilter.DAILY)
                _filters = (int)Shared.Utilities.Results.ComplexTypes.Enums.ConsoleQueryType.COMPANY_PROFILE_EXT2EXT_STATISTIC_DAILY;
            else if (_filter == Shared.Utilities.Results.ComplexTypes.Enums.DashboardFilter.WEEKLY)
                _filters = (int)Shared.Utilities.Results.ComplexTypes.Enums.ConsoleQueryType.COMPANY_PROFILE_EXT2EXT_STATISTIC_WEEKLY;
            else if (_filter == Shared.Utilities.Results.ComplexTypes.Enums.DashboardFilter.MONTHLY)
                _filters = (int)Shared.Utilities.Results.ComplexTypes.Enums.ConsoleQueryType.COMPANY_PROFILE_EXT2EXT_STATISTIC_MONTHLY;
            else
                _filters = (int)Shared.Utilities.Results.ComplexTypes.Enums.ConsoleQueryType.COMPANY_PROFILE_EXT2EXT_STATISTIC_YEARLY;

            var query = _staticService.GetQueries(LoggedInUser(userId), (Shared.Utilities.Results.ComplexTypes.Enums.ConsoleQueryType)_filters);

            if (query.ResultStatus == ResultStatus.Success)
            {
                var obj = new CompanyInternalStatisticDto();

                if (!string.IsNullOrWhiteSpace(query.Data))
                {
                    obj = JsonConvert.DeserializeObject<CompanyInternalStatisticDto>(query.Data);
                }

                var htmlContent = PartialViewGen.GenerateCompanyInternalStatisticHtml(new CompanyInternalStatisticModel
                {
                    Data = obj
                }, _staticService);
                return Ok(htmlContent);
            }
            else
            {
                var htmlContent = PartialViewGen.GenerateCompanyInternalStatisticHtml(new CompanyInternalStatisticModel
                {

                }, _staticService);
                return Ok(htmlContent);
            }


        }



        [HttpGet("InboundStatistic")]
        public async Task<IActionResult> InboundStatistic(byte? _f)
        {

            var userId = User.Claims.FirstOrDefault(c => c.Type == JwtRegisteredClaimNames.Jti).Value;
            Shared.Utilities.Results.ComplexTypes.Enums.DashboardFilter _filter = (_f == null || _f == 0) ? Shared.Utilities.Results.ComplexTypes.Enums.DashboardFilter.MONTHLY : (Shared.Utilities.Results.ComplexTypes.Enums.DashboardFilter)_f;

            var _filters = 0;

            if (_filter == Shared.Utilities.Results.ComplexTypes.Enums.DashboardFilter.DAILY)
                _filters = (int)Shared.Utilities.Results.ComplexTypes.Enums.ConsoleQueryType.COMPANY_PROFILE_INBOUND_STATISTIC_DAILY;
            else if (_filter == Shared.Utilities.Results.ComplexTypes.Enums.DashboardFilter.WEEKLY)
                _filters = (int)Shared.Utilities.Results.ComplexTypes.Enums.ConsoleQueryType.COMPANY_PROFILE_INBOUND_STATISTIC_WEEKLY;
            else if (_filter == Shared.Utilities.Results.ComplexTypes.Enums.DashboardFilter.MONTHLY)
                _filters = (int)Shared.Utilities.Results.ComplexTypes.Enums.ConsoleQueryType.COMPANY_PROFILE_INBOUND_STATISTIC_MONTHLY;
            else
                _filters = (int)Shared.Utilities.Results.ComplexTypes.Enums.ConsoleQueryType.COMPANY_PROFILE_INBOUND_STATISTIC_YEARLY;

            var query = _staticService.GetQueries(LoggedInUser(userId), (Shared.Utilities.Results.ComplexTypes.Enums.ConsoleQueryType)_filters);

            if (query.ResultStatus == ResultStatus.Success)
            {
                var obj = new CompanyInboundStatisticDto();

                if (!string.IsNullOrWhiteSpace(query.Data))
                    obj = JsonConvert.DeserializeObject<CompanyInboundStatisticDto>(query.Data);
                var htmlContent = PartialViewGen.GenerateCompanyInboundStatisticHtml(new CompanyInboundStatisticModel
                {
                    Data = obj
                }, _staticService);
                return Ok(htmlContent);
            }
            else
            {
                var htmlContent = PartialViewGen.GenerateCompanyInboundStatisticHtml(new CompanyInboundStatisticModel
                {

                }, _staticService);
                return Ok(htmlContent);
            }


        }

        [HttpGet("OutboundStatistic")]
        public async Task<IActionResult> OutboundStatistic(byte? _f)
        {

            var userId = User.Claims.FirstOrDefault(c => c.Type == JwtRegisteredClaimNames.Jti).Value;


            Shared.Utilities.Results.ComplexTypes.Enums.DashboardFilter _filter = (_f == null || _f == 0) ? Shared.Utilities.Results.ComplexTypes.Enums.DashboardFilter.MONTHLY : (Shared.Utilities.Results.ComplexTypes.Enums.DashboardFilter)_f;

            var _filters = 0;

            if (_filter == Shared.Utilities.Results.ComplexTypes.Enums.DashboardFilter.DAILY)
                _filters = (int)Shared.Utilities.Results.ComplexTypes.Enums.ConsoleQueryType.COMPANY_PROFILE_OUTBOUND_STATISTIC_DAILY;
            else if (_filter == Shared.Utilities.Results.ComplexTypes.Enums.DashboardFilter.WEEKLY)
                _filters = (int)Shared.Utilities.Results.ComplexTypes.Enums.ConsoleQueryType.COMPANY_PROFILE_OUTBOUND_STATISTIC_WEEKLY;
            else if (_filter == Shared.Utilities.Results.ComplexTypes.Enums.DashboardFilter.MONTHLY)
                _filters = (int)Shared.Utilities.Results.ComplexTypes.Enums.ConsoleQueryType.COMPANY_PROFILE_OUTBOUND_STATISTIC_MONTHLY;
            else
                _filters = (int)Shared.Utilities.Results.ComplexTypes.Enums.ConsoleQueryType.COMPANY_PROFILE_OUTBOUND_STATISTIC_YEARLY;

            var query = _staticService.GetQueries(LoggedInUser(userId), (Shared.Utilities.Results.ComplexTypes.Enums.ConsoleQueryType)_filters);

            if (query.ResultStatus == ResultStatus.Success)
            {
                var obj = new CompanyOutboundStatisticDto();

                if (!string.IsNullOrWhiteSpace(query.Data))
                    obj = JsonConvert.DeserializeObject<CompanyOutboundStatisticDto>(query.Data);
                var htmlContent = PartialViewGen.GenerateCompanyOutboundStatisticHtml(new CompanyOutboundStatisticModel
                {
                    Data = obj
                }, _staticService);
                return Ok(htmlContent);
            }
            else
            {
                var htmlContent = PartialViewGen.GenerateCompanyOutboundStatisticHtml(new CompanyOutboundStatisticModel
                {

                }, _staticService);
                return Ok(htmlContent);
            }
        }



        [HttpPost("GetReportList")]
        public async Task<IActionResult> GetReportList(ReportRequest req)
        {


            int? draw = req.draw;
            int? start = req.start;
            int? length = req.length;
            string json = req.json;
            string ReportDate = req.ReportDate;
            long ReportCount = req.ReportCount;
            var userId = User.Claims.FirstOrDefault(c => c.Type == JwtRegisteredClaimNames.Jti).Value;

            DateTime nowDate = DateTime.UtcNow;

            long dataCount = 0;

            if (ReportCount == -1)
            {
                var count = await _postgreSqlService.GetReportCount(LoggedInUser(userId), json, nowDate);

                dataCount = count.Data;
            }
            else
                dataCount = ReportCount;


            var list = await _postgreSqlService.GetReportList(LoggedInUser(userId), start ?? 0, length ?? 0, json, nowDate);

            if (list.ResultStatus == ResultStatus.Success)
            {
                TimeSpan gmtTime = BaseExtensions.GetGmtTime(LoggedInUser(userId).GMT);

                foreach (var item in list.Data.DataList)
                {
                    item.starttime = item.starttime.Add(gmtTime);
                    item.stoptime = item.stoptime.Add(gmtTime);

                    //string _inorout = item.tablecolumninorout;

                    //_inorout = _inorout.Replace("CDR_ReportInbound", StaticService.GetLocalization("CDR_ReportInbound").Data)
                    //            .Replace("CDR_ReportOutbound", StaticService.GetLocalization("CDR_ReportOutbound").Data)
                    //            .Replace("CDR_ReportInternal", StaticService.GetLocalization("CDR_ReportInternal").Data)
                    //            .Replace("CDR_ReportMakeCall", StaticService.GetLocalization("CDR_ReportMakeCall").Data);

                    //item.tablecolumninorout = _inorout;

                    //string _status = item.tablecolumnstatus;

                    //_status = _status.Replace("CDR_ReportCompleted", StaticService.GetLocalization("CDR_ReportCompleted").Data)
                    //    .Replace("CDR_ReportMissed", StaticService.GetLocalization("CDR_ReportMissed").Data)
                    //    .Replace("CDR_ReportTransferred", StaticService.GetLocalization("CDR_ReportTransferred").Data)
                    //    .Replace("CDR_ReportRinging", StaticService.GetLocalization("CDR_ReportRinging").Data);


                    //item.tablecolumnstatus = _status;

                    //string _excelinorout = item.excelinorout;

                    //_excelinorout = _excelinorout.Replace("CDR_ReportInbound", StaticService.GetLocalization("CDR_ReportInbound").Data)
                    //            .Replace("CDR_ReportOutbound", StaticService.GetLocalization("CDR_ReportOutbound").Data)
                    //            .Replace("CDR_ReportInternal", StaticService.GetLocalization("CDR_ReportInternal").Data)
                    //            .Replace("CDR_ReportMakeCall", StaticService.GetLocalization("CDR_ReportMakeCall").Data);

                    //item.excelinorout = _excelinorout;

                    //string _excelstatus = item.excelstatus;

                    //_excelstatus = _excelstatus.Replace("CDR_ReportCompleted", StaticService.GetLocalization("CDR_ReportCompleted").Data)
                    //    .Replace("CDR_ReportMissed", StaticService.GetLocalization("CDR_ReportMissed").Data)
                    //    .Replace("CDR_ReportTransferred", StaticService.GetLocalization("CDR_ReportTransferred").Data);


                    //item.excelstatus = _excelstatus;
                }

                return Ok(new
                {
                    draw = draw,
                    recordsFiltered = dataCount,
                    recordsTotal = dataCount,
                    data = list.Data.DataList,
                    reportDate = String.Format("{0:yyyy/MM/dd hh:mm:ss}", nowDate)
                });
            }
            else
            {
                return Ok(new
                {
                    draw = draw,
                    recordsFiltered = 0,
                    recordsTotal = 0,
                    data = new List<CompanyReportDto>(),
                    reportDate = String.Format("{0:yyyy/MM/dd hh:mm:ss}", nowDate)
                });
            }


        }

        [HttpGet("GetCallDetail")]
        public async Task<IActionResult> GetCallDetail(long Id)
        {
            var userId = User.Claims.FirstOrDefault(c => c.Type == JwtRegisteredClaimNames.Jti).Value;
            var detail = await _postgreSqlService.GetCallDetail(LoggedInUser(userId), Id);
            var information = await _postgreSqlService.GetCallInformation(LoggedInUser(userId), Id);

            if (detail.ResultStatus == ResultStatus.Success && information.ResultStatus == ResultStatus.Success)
            {
                TimeSpan gmtTime = BaseExtensions.GetGmtTime(LoggedInUser(userId).GMT);

                detail.Data.starttime = detail.Data.starttime.Add(gmtTime);
                detail.Data.stoptime = detail.Data.stoptime.Add(gmtTime);

                string _inorout = detail.Data.tablecolumninorout;

                _inorout = _inorout.Replace("CDR_ReportInbound", _staticService.GetLocalization("CDR_ReportInbound").Data)
                            .Replace("CDR_ReportOutbound", _staticService.GetLocalization("CDR_ReportOutbound").Data)
                            .Replace("CDR_ReportInternal", _staticService.GetLocalization("CDR_ReportInternal").Data);

                detail.Data.tablecolumninorout = _inorout;

                string _status = detail.Data.tablecolumnstatus;

                _status = _status.Replace("CDR_ReportCompleted", _staticService.GetLocalization("CDR_ReportCompleted").Data)
                    .Replace("CDR_ReportMissed", _staticService.GetLocalization("CDR_ReportMissed").Data)
                    .Replace("CDR_ReportTransferred", _staticService.GetLocalization("CDR_ReportTransferred").Data);


                detail.Data.tablecolumnstatus = _status;

                foreach (var item in information.Data.DataList)
                {
                    item.start_time = item.start_time.Add(gmtTime);
                    item.end_time = item.end_time.Add(gmtTime);
                }
                var htmlContent = PartialViewGen.GenerateCompanyCallDetailHtml(new CompanyCallDetailModel
                {
                    ResultStatus = ResultStatus.Success,
                    Detail = detail.Data,
                    Information = information.Data.DataList
                }, _staticService);
                return Ok(htmlContent);
            }
            else
            {
                var htmlContent = PartialViewGen.GenerateCompanyCallDetailHtml(new CompanyCallDetailModel
                {

                    ResultStatus = ResultStatus.Error,
                    Message = _staticService.GetLocalization("CDR_KonusmaDetayiBulunamadi").Data
                }, _staticService);
                return Ok(htmlContent);
            }

        }

        [HttpPost("CreateReportFavoriteFilter")]
        public async Task<IActionResult> CreateReportFavoriteFilter(ReportFavoriteFilterAddDto data)
        {
            var userId = User.Claims.FirstOrDefault(c => c.Type == JwtRegisteredClaimNames.Jti).Value;
            data.UserId = LoggedInUser(userId).Id;

            var add = await _reportFilterService.AddAsync(data);

            if (add.ResultStatus == ResultStatus.Success)
            {
                var deleteModel = System.Text.Json.JsonSerializer.Serialize(new
                {
                    ResultStatus = ResultStatus.Success,
                    Message = add.Message
                });

                return Ok(deleteModel);
            }
            else
            {

                var deleteModel = System.Text.Json.JsonSerializer.Serialize(new
                {
                    ResultStatus = ResultStatus.Error,
                    Message = add.Message
                });

                return Ok(deleteModel);
            }

        }
        [HttpGet("GetReportFavoriteFilterList")]
        public async Task<IActionResult> GetReportFavoriteFilterList()
        {
            var userId = User.Claims.FirstOrDefault(c => c.Type == JwtRegisteredClaimNames.Jti).Value;
            var favoriteFilterList = await _reportFilterService.GetAllAsync(LoggedInUser(userId).Id);
            var htmlContent = PartialViewGen.GenerateFavoriteFiltersHtml(new ReportFavoriteFilterListDto
            {
                ReportFilters = favoriteFilterList.Data.ReportFilters
            }, _staticService);
            return Ok(htmlContent);

        }

        [HttpPost("DeleteReportFavoriteFilter")]
        public async Task<IActionResult> DeleteReportFavoriteFilter(int Id)
        {
            var userId = User.Claims.FirstOrDefault(c => c.Type == JwtRegisteredClaimNames.Jti).Value;

            var delete = await _reportFilterService.HardDeleteAsync(Id, LoggedInUser(userId).Id);

            if (delete.ResultStatus == ResultStatus.Success)
            {
                var deleteModel = System.Text.Json.JsonSerializer.Serialize(new
                {
                    ResultStatus = ResultStatus.Success,
                    Message = delete.Message
                });

                return Ok(deleteModel);
            }
            else
            {

                var deleteModel = System.Text.Json.JsonSerializer.Serialize(new
                {
                    ResultStatus = ResultStatus.Error,
                    Message = delete.Message
                });

                return Ok(deleteModel);
            }

        }


        [HttpGet("ExportReportCalls")]
        public async Task<IActionResult> ExportReportCalls(string json)
        {
            var userId = User.Claims.FirstOrDefault(c => c.Type == JwtRegisteredClaimNames.Jti).Value;

            string sWebRootFolder = Path.Combine(_env.WebRootPath, "UploadExcel");
            string sFileName = @"report-calls-" + string.Format("{0:ddMMyyyy}", DateTime.Now) + "-" + Shared.Utilities.Extensions.BaseExtensions.GetUniqueKey(10) + ".xlsx";
            FileInfo file = new FileInfo(Path.Combine(sWebRootFolder, sFileName));
            var memory = new MemoryStream();
            using (var fs = new FileStream(Path.Combine(sWebRootFolder, sFileName), FileMode.Create, FileAccess.Write))
            {
                IWorkbook workbook;
                workbook = new XSSFWorkbook();
                ISheet excelSheet = workbook.CreateSheet("report");

                int _row = 0;

                IRow row = excelSheet.CreateRow(_row);
                row.CreateCell(0).SetCellValue(_staticService.GetLocalization("CDR_CallId").Data);
                row.CreateCell(1).SetCellValue(_staticService.GetLocalization("CDR_From").Data);
                row.CreateCell(2).SetCellValue(_staticService.GetLocalization("CDR_To").Data);
                row.CreateCell(3).SetCellValue(_staticService.GetLocalization("CDR_Duration").Data);
                row.CreateCell(4).SetCellValue(_staticService.GetLocalization("CDR_TalkTime").Data);
                row.CreateCell(5).SetCellValue(_staticService.GetLocalization("CDR_RingTime").Data);
                row.CreateCell(6).SetCellValue(_staticService.GetLocalization("CDR_Date").Data);
                row.CreateCell(7).SetCellValue(_staticService.GetLocalization("CDR_StartTime").Data);
                row.CreateCell(8).SetCellValue(_staticService.GetLocalization("CDR_StopTime").Data);
                row.CreateCell(9).SetCellValue(_staticService.GetLocalization("CDR_InOrOut").Data);
                row.CreateCell(10).SetCellValue(_staticService.GetLocalization("CDR_Status").Data);

                var data = await _postgreSqlService.GetReportList(LoggedInUser(userId), 0, int.MaxValue, json, DateTime.UtcNow);

                TimeSpan gmtTime = BaseExtensions.GetGmtTime(LoggedInUser(userId).GMT);

                foreach (var item in data.Data.DataList)
                {
                    ++_row;

                    item.starttime = item.starttime.Add(gmtTime);
                    item.stoptime = item.stoptime.Add(gmtTime);

                    string _inorout = item.tablecolumninorout;

                    _inorout = _inorout.Replace("CDR_ReportInbound", _staticService.GetLocalization("CDR_ReportInbound").Data)
                                .Replace("CDR_ReportOutbound", _staticService.GetLocalization("CDR_ReportOutbound").Data)
                                .Replace("CDR_ReportInternal", _staticService.GetLocalization("CDR_ReportInternal").Data)
                                .Replace("CDR_ReportMakeCall", _staticService.GetLocalization("CDR_ReportMakeCall").Data);

                    item.tablecolumninorout = _inorout;

                    string _status = item.tablecolumnstatus;

                    _status = _status.Replace("CDR_ReportCompleted", _staticService.GetLocalization("CDR_ReportCompleted").Data)
                        .Replace("CDR_ReportMissed", _staticService.GetLocalization("CDR_ReportMissed").Data)
                        .Replace("CDR_ReportTransferred", _staticService.GetLocalization("CDR_ReportTransferred").Data);


                    item.tablecolumnstatus = _status;

                    string _excelinorout = item.excelinorout;

                    _excelinorout = _excelinorout.Replace("CDR_ReportInbound", _staticService.GetLocalization("CDR_ReportInbound").Data)
                                .Replace("CDR_ReportOutbound", _staticService.GetLocalization("CDR_ReportOutbound").Data)
                                .Replace("CDR_ReportInternal", _staticService.GetLocalization("CDR_ReportInternal").Data)
                                .Replace("CDR_ReportMakeCall", _staticService.GetLocalization("CDR_ReportMakeCall").Data);

                    item.excelinorout = _excelinorout;

                    string _excelstatus = item.excelstatus;

                    _excelstatus = _excelstatus.Replace("CDR_ReportCompleted", _staticService.GetLocalization("CDR_ReportCompleted").Data)
                        .Replace("CDR_ReportMissed", _staticService.GetLocalization("CDR_ReportMissed").Data)
                        .Replace("CDR_ReportTransferred", _staticService.GetLocalization("CDR_ReportTransferred").Data);


                    item.excelstatus = _excelstatus;

                    row = excelSheet.CreateRow(_row);
                    row.CreateCell(0).SetCellValue(item.call_id);
                    row.CreateCell(1).SetCellValue(item.from);
                    row.CreateCell(2).SetCellValue(item.to);
                    row.CreateCell(3).SetCellValue(item.durationstring);
                    row.CreateCell(4).SetCellValue(item.talktimestring);
                    row.CreateCell(5).SetCellValue(item.ringtimestring);
                    row.CreateCell(6).SetCellValue(item.date);
                    row.CreateCell(7).SetCellValue(item.starttimestring);
                    row.CreateCell(8).SetCellValue(item.stoptimestring);
                    row.CreateCell(9).SetCellValue(item.excelinorout);
                    row.CreateCell(10).SetCellValue(item.excelstatus);
                }

                workbook.Write(fs);
            }
            using (var stream = new FileStream(Path.Combine(sWebRootFolder, sFileName), FileMode.Open))
            {
                await stream.CopyToAsync(memory);
            }
            memory.Position = 0;

            StartTheThread(file);

            return File(memory, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", sFileName);

        }
        [HttpPost("GetUsers")]
        public async Task<IActionResult> GetUsers()
        {
            var userId = User.Claims.FirstOrDefault(c => c.Type == JwtRegisteredClaimNames.Jti).Value;

            var query = _staticService.GetQueries(LoggedInUser(userId), Shared.Utilities.Results.ComplexTypes.Enums.ConsoleQueryType.COMPANY_USERS);

            if (query.ResultStatus == ResultStatus.Success)
            {
                var obj = new List<CompanyPersonDto>();

                string queryData = query.Data;

                if (!string.IsNullOrWhiteSpace(queryData) && !queryData.Equals("[]"))
                    obj = JsonConvert.DeserializeObject<List<CompanyPersonDto>>(queryData);

                return Ok(new
                {
                    Users = obj
                });
            }
            else
                return Ok(query.Data);
        }

        [HttpPost("GetUsersStatistic")]
        public async Task<IActionResult> GetUsersStatistic()
        {
            var userId = User.Claims.FirstOrDefault(c => c.Type == JwtRegisteredClaimNames.Jti).Value;

            var query = _staticService.GetQueries(LoggedInUser(userId), Shared.Utilities.Results.ComplexTypes.Enums.ConsoleQueryType.COMPANY_USERS_STATISTIC);

            if (query.ResultStatus == ResultStatus.Success)
            {
                var obj = new List<CompanyPersonDto>();

                string queryData = query.Data;

                if (!string.IsNullOrWhiteSpace(queryData) && !queryData.Equals("[]"))
                    obj = JsonConvert.DeserializeObject<List<CompanyPersonDto>>(queryData);

                return Ok(new
                {
                    Labels = obj.Select(x => x.dn + " - " + x.display_name).ToList(),
                    Inbound = obj.Select(x => x.totalinbound).ToList(),
                    Outbound = obj.Select(x => x.totaloutbound).ToList(),
                    Missed = obj.Select(x => x.totalmissed).ToList()
                });
            }
            else
                return Ok(query.Data);
        }

        [HttpGet("PhoneBookManagement")]
        public async Task<IActionResult> PhoneBookManagement(int? userId)
        {
            var userSignId = User.Claims.FirstOrDefault(c => c.Type == JwtRegisteredClaimNames.Jti).Value;

            if (userId == null || userId == 0)
            {
                var htmlContent = PartialViewGen.GenerateCompanyPhonebookAddHtml(new CompanyPhonebookAddDto
                { }, _staticService);
                return Ok(htmlContent);
            }
            else
            {
                var _detail = _postgreSqlService.GetCompanyPhonebookDetail(LoggedInUser(userSignId), userId ?? 0).Result;

                if (_detail.ResultStatus == ResultStatus.Success)
                {
                    var htmlContent = PartialViewGen.GenerateCompanyPhonebookAddHtml(new CompanyPhonebookAddDto
                    {
                        Company = _detail.Data.company,
                        Email = _detail.Data.email,
                        FirstName = _detail.Data.firstname,
                        idphonebook = _detail.Data.idphonebook,
                        LastName = _detail.Data.lastname,
                        MobileNumber = _detail.Data.phonenumber
                    }, _staticService);
                    return Ok(htmlContent);
                }
                else
                {
                    var htmlContent = PartialViewGen.GenerateCompanyPhonebookAddHtml(new CompanyPhonebookAddDto
                    { }, _staticService);
                    return Ok(htmlContent);
                }
            }
        }

        [HttpPost("PhoneBookManagement/action")]
        public async Task<IActionResult> PhoneBookManagementAction(CompanyPhonebookAddDto companyPhonebookAddDto)
        {
            var userSignId = User.Claims.FirstOrDefault(c => c.Type == JwtRegisteredClaimNames.Jti).Value;

            if (ModelState.IsValid)
            {
                var create = await _postgreSqlService.CreateCompanyPhonebook(LoggedInUser(userSignId), companyPhonebookAddDto);

                if (create.ResultStatus == ResultStatus.Success)
                {
                    var htmlContent = PartialViewGen.GenerateCompanyPhonebookAddHtmlv2(companyPhonebookAddDto, _staticService);
                    var companyPhonebookManagementAjaxModel = System.Text.Json.JsonSerializer.Serialize(new CompanyPhonebookAjaxViewModel
                    {
                        CompanyPhonebookManagementPartial = htmlContent,
                        ResultStatus = ResultStatus.Success,
                        Message = create.Message,
                    });

                    return Ok(companyPhonebookManagementAjaxModel);
                }
                else
                {
                    ModelState.AddModelError("", create.Message);
                    var htmlContent = PartialViewGen.GenerateCompanyPhonebookAddHtmlv2(companyPhonebookAddDto, _staticService);
                    var companyPhonebookAjaxModelStateErrorModel = System.Text.Json.JsonSerializer.Serialize(new CompanyPhonebookAjaxViewModel
                    {
                        CompanyPhonebookAddDto = companyPhonebookAddDto,
                        ResultStatus = ResultStatus.Error,
                        CompanyPhonebookManagementPartial = htmlContent
                    });

                    return Ok(companyPhonebookAjaxModelStateErrorModel);
                }


            }
            else
            {
                var htmlContent = PartialViewGen.GenerateCompanyPhonebookAddHtmlv2(companyPhonebookAddDto, _staticService);
                var companyPhonebookAjaxModelStateErrorModel = System.Text.Json.JsonSerializer.Serialize(new CompanyPhonebookAjaxViewModel
                {
                    CompanyPhonebookAddDto = companyPhonebookAddDto,
                    ResultStatus = ResultStatus.Error,
                    CompanyPhonebookManagementPartial = htmlContent
                });

                return Ok(companyPhonebookAjaxModelStateErrorModel);
            }
        }

        [HttpPost("GetPhoneBookList")]
        public async Task<IActionResult> GetPhoneBookList(GetPhoneBookRequestDto req)
        {
            int? draw = req.draw;
            int? start = req.start;
            int? length = req.length;
            string namesurname = req.namesurname;
            string email = req.email;
            string phone = req.phone;
            var userSignId = User.Claims.FirstOrDefault(c => c.Type == JwtRegisteredClaimNames.Jti).Value;

            var data = await _postgreSqlService.GetCompanyPhonebookList(LoggedInUser(userSignId), start ?? 0, length ?? 0, namesurname, email, phone);
            var count = await _postgreSqlService.GetCompanyPhonebookCount(LoggedInUser(userSignId), namesurname, email, phone);

            if (data.ResultStatus == ResultStatus.Success && count.ResultStatus == ResultStatus.Success)
            {
                return Ok(new
                {
                    draw = draw,
                    recordsFiltered = count.Data,
                    recordsTotal = count.Data,
                    data = data.Data.Users
                });
            }
            else
            {
                return Ok(new
                {
                    draw = draw,
                    recordsFiltered = 0,
                    recordsTotal = 0,
                    data = ""
                });
            }

        }

        [HttpPost("DeletePhoneBook")]
        public async Task<IActionResult> DeletePhoneBook(List<int> Ids)
        {
            var userId = User.Claims.FirstOrDefault(c => c.Type == JwtRegisteredClaimNames.Jti).Value;

            var delete = await _postgreSqlService.DeleteCompanyPhonebook(LoggedInUser(userId), Ids);

            if (delete.ResultStatus == ResultStatus.Success)
            {
                var deleteModel = System.Text.Json.JsonSerializer.Serialize(new CompanyPhonebookAjaxViewModel
                {
                    ResultStatus = ResultStatus.Success,
                    Message = delete.Message
                });

                return Ok(deleteModel);
            }
            else
            {

                var deleteModel = System.Text.Json.JsonSerializer.Serialize(new CompanyPhonebookAjaxViewModel
                {
                    ResultStatus = ResultStatus.Error,
                    Message = delete.Message
                });

                return Ok(deleteModel);
            }
        }

        [HttpGet("ExportCompanyPhonebook")]
        public async Task<IActionResult> ExportCompanyPhonebook(string NameSurname, string Email, string Phone)
        {
            var userId = User.Claims.FirstOrDefault(c => c.Type == JwtRegisteredClaimNames.Jti).Value;
            string sWebRootFolder = Path.Combine(_env.WebRootPath, "UploadExcel");
            string sFileName = @"phone-book-" + string.Format("{0:ddMMyyyy}", DateTime.Now) + "-" + Shared.Utilities.Extensions.BaseExtensions.GetUniqueKey(10) + ".xlsx";
            FileInfo file = new FileInfo(Path.Combine(sWebRootFolder, sFileName));
            var memory = new MemoryStream();
            using (var fs = new FileStream(Path.Combine(sWebRootFolder, sFileName), FileMode.Create, FileAccess.Write))
            {
                IWorkbook workbook;
                workbook = new XSSFWorkbook();
                ISheet excelSheet = workbook.CreateSheet("phonebook");

                int _row = 0;

                IRow row = excelSheet.CreateRow(_row);
                row.CreateCell(0).SetCellValue(_staticService.GetLocalization("CDR_Name").Data);
                row.CreateCell(1).SetCellValue(_staticService.GetLocalization("CDR_Surname").Data);
                row.CreateCell(2).SetCellValue(_staticService.GetLocalization("CDR_Email").Data);
                row.CreateCell(3).SetCellValue(_staticService.GetLocalization("CDR_Phone").Data);
                row.CreateCell(4).SetCellValue(_staticService.GetLocalization("CDR_CompanyName").Data);

                var data = await _postgreSqlService.GetCompanyPhonebookList(LoggedInUser(userId), 0, int.MaxValue, NameSurname, Email, Phone);

                foreach (var item in data.Data.Users)
                {
                    ++_row;

                    row = excelSheet.CreateRow(_row);
                    row.CreateCell(0).SetCellValue(item.firstname);
                    row.CreateCell(1).SetCellValue(item.lastname);
                    row.CreateCell(2).SetCellValue(item.email);
                    row.CreateCell(3).SetCellValue(item.phonenumber);
                    row.CreateCell(4).SetCellValue(item.company);
                }

                workbook.Write(fs);
            }
            using (var stream = new FileStream(Path.Combine(sWebRootFolder, sFileName), FileMode.Open))
            {
                await stream.CopyToAsync(memory);
            }
            memory.Position = 0;

            StartTheThread(file);

            return File(memory, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", sFileName);
        }

        [HttpPost("ImportCompanyPhonebook")]
        public async Task<IActionResult> ImportCompanyPhonebook()
        {
            var userId = User.Claims.FirstOrDefault(c => c.Type == JwtRegisteredClaimNames.Jti).Value;

            IFormFile file = Request.Form.Files[0];
            string folderName = "UploadExcel";
            string webRootPath = _env.WebRootPath;
            string newPath = Path.Combine(webRootPath, folderName);
            StringBuilder sb = new StringBuilder();

            if (!Directory.Exists(newPath))
            {
                Directory.CreateDirectory(newPath);
            }
            if (file.Length > 0)
            {
                string _name = Path.GetFileNameWithoutExtension(file.FileName);
                string _unique = Shared.Utilities.Extensions.BaseExtensions.GetUniqueKey(20);
                string _ext = Path.GetExtension(file.FileName).ToLower();

                var _fileName = _name + _unique + _ext;

                string sFileExtension = Path.GetExtension(file.FileName).ToLower();
                ISheet sheet;
                string fullPath = Path.Combine(newPath, _fileName);
                using (var stream = new FileStream(fullPath, FileMode.Create))
                {
                    file.CopyTo(stream);
                    stream.Position = 0;
                    if (sFileExtension == ".xls")
                    {
                        HSSFWorkbook hssfwb = new HSSFWorkbook(stream);
                        sheet = hssfwb.GetSheetAt(0);
                    }
                    else
                    {
                        XSSFWorkbook hssfwb = new XSSFWorkbook(stream);
                        sheet = hssfwb.GetSheetAt(0);
                    }
                    IRow headerRow = sheet.GetRow(0);
                    int cellCount = headerRow.LastCellNum;

                    var dataList = new List<CompanyPhonebookAddDto>();

                    for (int i = (sheet.FirstRowNum + 1); i <= sheet.LastRowNum; i++)
                    {
                        IRow row = sheet.GetRow(i);
                        if (row == null) continue;
                        if (row.Cells.All(d => d.CellType == CellType.Blank)) continue;
                        var values = new object[cellCount];
                        for (int j = row.FirstCellNum; j < cellCount; j++)
                        {
                            if (row.GetCell(j) != null)
                                values[j] = row.GetCell(j).ToString();
                        }

                        dataList.Add(new CompanyPhonebookAddDto
                        {
                            FirstName = values[0] != null ? values[0].ToString() : "",
                            LastName = values[1] != null ? values[1].ToString() : "",
                            Email = values[2] != null ? values[2].ToString() : "",
                            MobileNumber = values[3] != null ? values[3].ToString() : "",
                            Company = values[4] != null ? values[4].ToString() : ""
                        });
                    }

                    var add = await _postgreSqlService.ImportCompanyPhonebook(LoggedInUser(userId), dataList);

                    if (add.ResultStatus == ResultStatus.Success)
                    {
                        var excelModel = System.Text.Json.JsonSerializer.Serialize(new
                        {
                            ResultStatus = ResultStatus.Success,
                            Message = add.Message
                        });

                        return Ok(excelModel);
                    }
                    else
                    {
                        var excelModel = System.Text.Json.JsonSerializer.Serialize(new
                        {
                            ResultStatus = ResultStatus.Error,
                            Message = add.Message
                        });

                        return Ok(excelModel);
                    }
                }
            }

            var excelErrorModel = System.Text.Json.JsonSerializer.Serialize(new
            {
                ResultStatus = ResultStatus.Error,
                Message = ""
            });

            return Ok(excelErrorModel);
        }

        [HttpPost("GetUserDetaiReport")]
        public async Task<IActionResult> GetUserDetaiReport(string id, byte _f)
        {
            var userId = User.Claims.FirstOrDefault(c => c.Type == JwtRegisteredClaimNames.Jti).Value;
            var detail = await _postgreSqlService.GetCompanyPersonDetailReportList(LoggedInUser(userId), id, (Shared.Utilities.Results.ComplexTypes.Enums.CallInfoFilter)_f);

            if (detail.ResultStatus == ResultStatus.Success)
            {
                TimeSpan gmtTime = BaseExtensions.GetGmtTime(LoggedInUser(userId).GMT);

                foreach (var item in detail.Data.DataList)
                {
                    item.starttime = item.starttime.Add(gmtTime);
                    item.stoptime = item.stoptime.Add(gmtTime);
                }

                return Ok(new
                {
                    detail.Data.DataList
                });
            }
            else
                return Ok(detail.Data);
        }
        [HttpGet("ExportCompanyUserDetaiReport")]
        public async Task<IActionResult> ExportCompanyUserDetaiReport(string id, byte f)
        {
            var userId = User.Claims.FirstOrDefault(c => c.Type == JwtRegisteredClaimNames.Jti).Value;
            string sWebRootFolder = Path.Combine(_env.WebRootPath, "UploadExcel");
            string sFileName = @"call-list-" + string.Format("{0:ddMMyyyyHHmmss}", DateTime.Now) + "-" + Shared.Utilities.Extensions.BaseExtensions.GetUniqueKey(10) + ".xlsx";
            FileInfo file = new FileInfo(sWebRootFolder);
            var memory = new MemoryStream();
            using (var fs = new FileStream(Path.Combine(sWebRootFolder, sFileName), FileMode.Create, FileAccess.Write))
            {
                IWorkbook workbook;
                workbook = new XSSFWorkbook();
                ISheet excelSheet = workbook.CreateSheet("call");

                int _row = 0;

                IRow row = excelSheet.CreateRow(_row);
                row.CreateCell(0).SetCellValue(_staticService.GetLocalization("CDR_CallId").Data);
                row.CreateCell(1).SetCellValue(_staticService.GetLocalization("CDR_From").Data);
                row.CreateCell(2).SetCellValue(_staticService.GetLocalization("CDR_To").Data);
                row.CreateCell(3).SetCellValue(_staticService.GetLocalization("CDR_Duration").Data);
                row.CreateCell(4).SetCellValue(_staticService.GetLocalization("CDR_TalkTime").Data);
                row.CreateCell(5).SetCellValue(_staticService.GetLocalization("CDR_RingTime").Data);
                row.CreateCell(6).SetCellValue(_staticService.GetLocalization("CDR_Date").Data);
                row.CreateCell(7).SetCellValue(_staticService.GetLocalization("CDR_StartTime").Data);
                row.CreateCell(8).SetCellValue(_staticService.GetLocalization("CDR_StopTime").Data);
                row.CreateCell(9).SetCellValue(_staticService.GetLocalization("CDR_Type").Data);
                row.CreateCell(10).SetCellValue(_staticService.GetLocalization("CDR_Status").Data);

                var data = await _postgreSqlService.GetCompanyPersonDetailReportList(LoggedInUser(userId), id, (Shared.Utilities.Results.ComplexTypes.Enums.CallInfoFilter)f);

                TimeSpan gmtTime = BaseExtensions.GetGmtTime(LoggedInUser(userId).GMT);

                foreach (var item in data.Data.DataList)
                {
                    ++_row;

                    row = excelSheet.CreateRow(_row);
                    row.CreateCell(0).SetCellValue(item.call_id);
                    row.CreateCell(1).SetCellValue(item.from);
                    row.CreateCell(2).SetCellValue(item.to);
                    row.CreateCell(3).SetCellValue(item.durationstring);
                    row.CreateCell(4).SetCellValue(item.talktimestring);
                    row.CreateCell(5).SetCellValue(item.ringtimestring);
                    row.CreateCell(6).SetCellValue(item.date);
                    row.CreateCell(7).SetCellValue(item.starttimestring);
                    row.CreateCell(8).SetCellValue(item.stoptimestring);
                    row.CreateCell(9).SetCellValue(item.excelinorout);
                    row.CreateCell(10).SetCellValue(item.excelstatus);
                }

                workbook.Write(fs);
            }
            using (var stream = new FileStream(Path.Combine(sWebRootFolder, sFileName), FileMode.Open))
            {
                await stream.CopyToAsync(memory);
            }
            memory.Position = 0;

            StartTheThread(file);

            return File(memory, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", sFileName);
        }
        [HttpPost("GetUserDetailTop")]
        public async Task<IActionResult> GetUserDetailTop(string id, byte? _f)
        {
            var userId = User.Claims.FirstOrDefault(c => c.Type == JwtRegisteredClaimNames.Jti).Value;
            Shared.Utilities.Results.ComplexTypes.Enums.DashboardFilter filter = (_f == null || _f == 0) ? Shared.Utilities.Results.ComplexTypes.Enums.DashboardFilter.MONTHLY : (Shared.Utilities.Results.ComplexTypes.Enums.DashboardFilter)_f;

            var detail = await _postgreSqlService.GetCompanyPersonDetailTop(LoggedInUser(userId), id, filter);

            if (detail.ResultStatus == ResultStatus.Success)
            {
                string _totalHourstotalcalltime = (int)detail.Data.totaltalktime.TotalHours < 10 ? "0" + (int)detail.Data.totaltalktime.TotalHours : ((int)detail.Data.totaltalktime.TotalHours).ToString();
                string _minutestotalcalltime = detail.Data.totaltalktime.Minutes < 10 ? "0" + detail.Data.totaltalktime.Minutes : detail.Data.totaltalktime.Minutes.ToString();
                string _secondstotalcalltime = detail.Data.totaltalktime.Seconds < 10 ? "0" + detail.Data.totaltalktime.Seconds : detail.Data.totaltalktime.Seconds.ToString();

                return Ok(new
                {
                    detail.Data.numofcalls,
                    detail.Data.numofanswered,
                    detail.Data.numofmissed,
                    totaltalktime = _totalHourstotalcalltime + ":" + _minutestotalcalltime + ":" + _secondstotalcalltime,
                    totalminutes = (long)detail.Data.totaltalktime.TotalMinutes
                });
            }
            else
                return Ok(detail.Data);
        }

        [HttpGet("GetUserDetailTopSixCalls")]
        public async Task<IActionResult> GetUserDetailTopSixCalls(string id, byte? _f)
        {
            var userId = User.Claims.FirstOrDefault(c => c.Type == JwtRegisteredClaimNames.Jti).Value;
            Shared.Utilities.Results.ComplexTypes.Enums.DashboardFilter filter = (_f == null || _f == 0) ? Shared.Utilities.Results.ComplexTypes.Enums.DashboardFilter.MONTHLY : (Shared.Utilities.Results.ComplexTypes.Enums.DashboardFilter)_f;

            var detail = await _postgreSqlService.GetCompanyPersonDetailTopSixConversationNumber(LoggedInUser(userId), id, filter);

            if (detail.ResultStatus == ResultStatus.Success)
            {
                return Ok(PartialViewGen.GenerateCompanyUserDetailHtml(new CompanyUserDetailTopSixNumberModel
                {
                    DataList = detail.Data.DataList
                }, _staticService));
            }
            else
            {
                return Ok(PartialViewGen.GenerateCompanyUserDetailHtml(new CompanyUserDetailTopSixNumberModel(), _staticService));
            }
        }
        [HttpGet("GetUserDetailTopSixAnsweredCalls")]
        public async Task<IActionResult> GetUserDetailTopSixAnsweredCalls(string id, byte? _f)
        {
            var userId = User.Claims.FirstOrDefault(c => c.Type == JwtRegisteredClaimNames.Jti).Value;
            Shared.Utilities.Results.ComplexTypes.Enums.DashboardFilter filter = (_f == null || _f == 0) ? Shared.Utilities.Results.ComplexTypes.Enums.DashboardFilter.MONTHLY : (Shared.Utilities.Results.ComplexTypes.Enums.DashboardFilter)_f;

            var detail = await _postgreSqlService.GetCompanyPersonDetailTopSixAnsweredNumber(LoggedInUser(userId), id, filter);

            if (detail.ResultStatus == ResultStatus.Success)
            {
                return Ok(PartialViewGen.GenerateCompanyUserDetailTopSixNumberModel(new CompanyUserDetailTopSixNumberModel
                {
                    DataList = detail.Data.DataList
                }, _staticService));
            }
            else
            {
                return Ok(PartialViewGen.GenerateCompanyUserDetailTopSixNumberModel(new CompanyUserDetailTopSixNumberModel(), _staticService));
            }
        }


        [HttpGet("GetUserDetailTopMissedCalls")]
        public async Task<IActionResult> GetUserDetailTopMissedCalls(string id, byte? _f)
        {
            var userId = User.Claims.FirstOrDefault(c => c.Type == JwtRegisteredClaimNames.Jti).Value;
            Shared.Utilities.Results.ComplexTypes.Enums.DashboardFilter filter = (_f == null || _f == 0) ? Shared.Utilities.Results.ComplexTypes.Enums.DashboardFilter.MONTHLY : (Shared.Utilities.Results.ComplexTypes.Enums.DashboardFilter)_f;

            var detail = await _postgreSqlService.GetCompanyPersonDetailTopSixMissedNumber(LoggedInUser(userId), id, filter);

            if (detail.ResultStatus == ResultStatus.Success)
            {
                return Ok(PartialViewGen.GenerateCompanyUserDetailTopSixNumberHtml(new CompanyUserDetailTopSixNumberModel
                {
                    DataList = detail.Data.DataList
                }, _staticService));
            }
            else
            {
                return Ok(PartialViewGen.GenerateCompanyUserDetailTopSixNumberModel(new CompanyUserDetailTopSixNumberModel(), _staticService));
            }
        }
        protected User LoggedInUser(string id) => _userManager.FindByIdAsync(id).Result;
        private Thread StartTheThread(FileInfo _file)
        {
            var t = new Thread(() => RealStart(_file));
            t.Start();
            return t;
        }
        private static void RealStart(FileInfo _file)
        {
            try
            {
                if (_file.Exists)
                {
                    _file.Delete();
                }
            }
            catch (Exception)
            {
            }
        }
        protected List<TempTotalCalls> Weeks
        {
            get
            {
                var _list = new List<TempTotalCalls>();

                _list.Add(new TempTotalCalls
                {
                    Order = 1,
                    Value = _staticService.GetLocalization("CDR_Week0").Data
                });

                _list.Add(new TempTotalCalls
                {
                    Order = 2,
                    Value = _staticService.GetLocalization("CDR_Week1").Data
                });

                _list.Add(new TempTotalCalls
                {
                    Order = 3,
                    Value = _staticService.GetLocalization("CDR_Week2").Data
                });

                _list.Add(new TempTotalCalls
                {
                    Order = 4,
                    Value = _staticService.GetLocalization("CDR_Week3").Data
                });

                _list.Add(new TempTotalCalls
                {
                    Order = 5,
                    Value = _staticService.GetLocalization("CDR_Week4").Data
                });

                _list.Add(new TempTotalCalls
                {
                    Order = 6,
                    Value = _staticService.GetLocalization("CDR_Week5").Data
                });

                _list.Add(new TempTotalCalls
                {
                    Order = 7,
                    Value = _staticService.GetLocalization("CDR_Week6").Data
                });

                return _list;
            }
        }

        public List<TempTotalCalls> Years
        {
            get
            {
                var _list = new List<TempTotalCalls>();

                _list.Add(new TempTotalCalls
                {
                    Order = 1,
                    Value = _staticService.GetLocalization("CDR_Month0").Data
                });

                _list.Add(new TempTotalCalls
                {
                    Order = 2,
                    Value = _staticService.GetLocalization("CDR_Month1").Data
                });

                _list.Add(new TempTotalCalls
                {
                    Order = 3,
                    Value = _staticService.GetLocalization("CDR_Month2").Data
                });

                _list.Add(new TempTotalCalls
                {
                    Order = 4,
                    Value = _staticService.GetLocalization("CDR_Month3").Data
                });

                _list.Add(new TempTotalCalls
                {
                    Order = 5,
                    Value = _staticService.GetLocalization("CDR_Month4").Data
                });

                _list.Add(new TempTotalCalls
                {
                    Order = 6,
                    Value = _staticService.GetLocalization("CDR_Month5").Data
                });

                _list.Add(new TempTotalCalls
                {
                    Order = 7,
                    Value = _staticService.GetLocalization("CDR_Month6").Data
                });

                _list.Add(new TempTotalCalls
                {
                    Order = 8,
                    Value = _staticService.GetLocalization("CDR_Month7").Data
                });

                _list.Add(new TempTotalCalls
                {
                    Order = 9,
                    Value = _staticService.GetLocalization("CDR_Month8").Data
                });

                _list.Add(new TempTotalCalls
                {
                    Order = 10,
                    Value = _staticService.GetLocalization("CDR_Month9").Data
                });

                _list.Add(new TempTotalCalls
                {
                    Order = 11,
                    Value = _staticService.GetLocalization("CDR_Month10").Data
                });

                _list.Add(new TempTotalCalls
                {
                    Order = 12,
                    Value = _staticService.GetLocalization("CDR_Month11").Data
                });

                return _list;
            }
        }

        public class TempTotalCalls
        {
            public byte Order { get; set; }
            public string Value { get; set; }
        }



    }
}
