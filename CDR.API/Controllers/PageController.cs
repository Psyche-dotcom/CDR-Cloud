using AutoMapper;
using CDR.API.Api.Model;
using CDR.API.Api.Service.Interface;
using CDR.API.Filters;
using CDR.API.PartialViewGeneration;
using CDR.Entities.Concrete;
using CDR.Entities.Dtos;
using CDR.Entities.Dtos.WebApi;
using CDR.Services.Abstract;
using CDR.Shared.Utilities.Results.ComplexTypes;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using System.IdentityModel.Tokens.Jwt;
using System.Text;


namespace CDR.API.Controllers
{
    [Authorize(AuthenticationSchemes = "Bearer")]
    [Route("api/page")]
    [ApiController]
    public class PageController : ControllerBase
    {
        private readonly IQueriesService _queriesService;

        private readonly IContentService _contentService;
        private readonly GlobalSettings _globalSettings;
        private readonly UserManager<User> _userManager;
        private readonly IMapper _mapper;
        private readonly IStaticService _staticService;
        private readonly IUserService _userService;

        public PageController(IQueriesService queriesService,
            IOptions<GlobalSettings> globalSettings,
            IContentService contentService,
            UserManager<User> userManager,
            IMapper mapper,
            IStaticService staticService,
            IUserService userService)
        {
            _queriesService = queriesService;
            _globalSettings = globalSettings.Value;
            _contentService = contentService;
            _userManager = userManager;
            _mapper = mapper;
            _staticService = staticService;
           _userService = userService;
        }


     
        //[ServiceFilter(typeof(ValidationFilterAttribute))]

        [HttpPost("info/dashboard")]
        public async Task<IActionResult> InfoDashboard(UserRegisterDto userRegisterDto)
        {

            var response = new ResponseDto<string>();


            response.StatusCode = 200;
            response.DisplayMessage = "Success";
            response.Result = "successfully registered user";
            return Ok(response);






        }  
        
        [HttpGet("info/dashboard/user")]
        public async Task<IActionResult> InfoUser()
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



        [HttpPost("GetDashboardTotalGraph")]
        public async Task<IActionResult> GetDashboardTotalGraph()
        {
            var userId = User.Claims.FirstOrDefault(c => c.Type == JwtRegisteredClaimNames.Jti).Value;
            var query = _staticService.GetQueries(LoggedInUser(userId), Enums.ConsoleQueryType.DASHBOARD_BARS);

            if (query.ResultStatus == ResultStatus.Success)
            {
                var obj = new List<Entities.Dtos.DashboardTotalGraphDto>();

                string queryData = query.Data;

                if (!string.IsNullOrWhiteSpace(queryData) && !queryData.Equals("[]"))
                    obj = JsonConvert.DeserializeObject<List<Entities.Dtos.DashboardTotalGraphDto>>(queryData);

                return Ok(new DashboardTotalGraphModel

                {
                    dates = obj.Select(x => String.Format("{0:dd/MM/yyyy}", x.datevalue)).ToList(),
                    calls = obj.Select(x => x.totalcalls).ToList(),
                    inbound = obj.Select(x => x.inbound).ToList(),
                    outbound = obj.Select(x => x.outbound).ToList(),
                    missed = obj.Select(x => x.missed).ToList(),
                    abandoned = obj.Select(x => x.abandoned).ToList(),
                    ext2ext = obj.Select(x => x.ext2ext).ToList()
                });
            }
            else
                return Ok(query.Data);
        }
        [HttpPost("change_language")]
        public async Task<IActionResult> ChangeLanguage(string C)
        {

            var culture = System.Globalization.CultureInfo.CurrentCulture;

            string _culture = string.IsNullOrWhiteSpace(C) ? "en-US" : C;

            var preferedCulture = _culture;
            if (Request.Cookies[".AspNetCore.Culture"] != "" && Request.Cookies[".AspNetCore.Culture"] != null)
            {
                HttpContext.Response.Cookies.Delete(".AspNetCore.Culture");
            }

            HttpContext.Response.Cookies.Append(".AspNetCore.Culture",
                $"c={preferedCulture}|uic={preferedCulture}", new CookieOptions { Expires = DateTime.UtcNow.AddYears(1) });

            return Ok(preferedCulture);

        }
        [HttpPost("GetDashboardTotal")]
        public async Task<IActionResult> GetDashboardTotal(byte _f)
        {
            var userId = User.Claims.FirstOrDefault(c => c.Type == JwtRegisteredClaimNames.Jti).Value;
            var _filter = 0;

            if ((Enums.DashboardFilter)_f == Enums.DashboardFilter.DAILY)
                _filter = (int)Enums.ConsoleQueryType.DASHBOARD_TOTALS_DAILY;
            else if ((Enums.DashboardFilter)_f == Enums.DashboardFilter.WEEKLY)
                _filter = (int)Enums.ConsoleQueryType.DASHBOARD_TOTALS_WEEKLY;
            else if ((Enums.DashboardFilter)_f == Enums.DashboardFilter.MONTHLY)
                _filter = (int)Enums.ConsoleQueryType.DASHBOARD_TOTALS_MONTHLY;
            else
                _filter = (int)Enums.ConsoleQueryType.DASHBOARD_TOTALS_YEARLY;

            var query = _staticService.GetQueries(LoggedInUser(userId), (Enums.ConsoleQueryType)_filter);

            if (query.ResultStatus == ResultStatus.Success)
                return Ok(!string.IsNullOrWhiteSpace(query.Data) ? query.Data : null);
            else
                return Ok(query.Data);


        }

        [HttpPost("GetDashboardGraph")]
        public async Task<IActionResult> GetDashboardGraph(byte? _f)
        {
            var userId = User.Claims.FirstOrDefault(c => c.Type == JwtRegisteredClaimNames.Jti).Value;

            var _filter = _f == null || _f == 0 ? Enums.GraphFilter.YEARLY : (Enums.GraphFilter)_f;

            var _filters = 0;

            if ((Enums.GraphFilter)_filter == Enums.GraphFilter.WEEKLY)
                _filters = (int)Enums.ConsoleQueryType.DASHBOARD_TOTAL_CALLS_WEEKLY;
            else if ((Enums.GraphFilter)_filter == Enums.GraphFilter.MONTHLY)
                _filters = (int)Enums.ConsoleQueryType.DASHBOARD_TOTAL_CALLS_MONTHLY;
            else
                _filters = (int)Enums.ConsoleQueryType.DASHBOARD_TOTAL_CALLS_YEARLY;

            var query = _staticService.GetQueries(LoggedInUser(userId), (Enums.ConsoleQueryType)_filters);

            if (query.ResultStatus == ResultStatus.Success)
            {
                var obj = new List<Entities.Dtos.DashboardGraphDto>();

                string queryData = query.Data;

                if (!string.IsNullOrWhiteSpace(queryData) && !queryData.Equals("[]"))
                    obj = JsonConvert.DeserializeObject<List<Entities.Dtos.DashboardGraphDto>>(queryData);

                if ((Enums.GraphFilter)_filter == Enums.GraphFilter.YEARLY)
                    obj = obj.OrderBy(x => x.years).ThenBy(x => x.month).ToList();

                var mdl = new DashboardGraphModel();

                if (_filter == Enums.GraphFilter.WEEKLY)
                    mdl.xAxis = obj.Select((x, i) => Weeks.Where(y => y.Order == i + 1).Select(y => y.Value).FirstOrDefault()).ToList();
                else if (_filter == Enums.GraphFilter.MONTHLY)
                    mdl.xAxis = obj.Select(x => (x.day < 10 ? "0" + x.day : x.day.ToString())).ToList();
                else if (_filter == Enums.GraphFilter.YEARLY)
                    mdl.xAxis = obj.Select((x, i) => Years.Where(y => y.Order == i + 1).Select(y => y.Value).FirstOrDefault()).ToList();

                mdl.InboundList = obj.Select(x => x.inbound).ToList();
                mdl.OutboundList = obj.Select(x => x.outbound).ToList();
                mdl.TotalList = obj.Select(x => x.total).ToList();

                return Ok(mdl);

            }
            else
                return Ok(query.Data);
        }

        [HttpGet("DistanceSellingContract")]
        public async Task<IActionResult> DistanceSellingContract()
        {
            var content = await _contentService.GetAsync(Enums.Content.DISTANCE_SALES_AGREEMENT);
            return Ok(new ContentModel
            {
                Content = content.Data.Content
            });
        }
        [HttpGet("PrivacyAgreement")]
        public async Task<IActionResult> PrivacyAgreement()
        {
            var content = await _contentService.GetAsync(Enums.Content.PRIVACY_AGREEMENT);

            return Ok(new ContentModel
            {
                Content = content.Data.Content
            });
        }

        [HttpGet("MembershipAgreement")]
        public async Task<IActionResult> MembershipAgreement()
        {
            var content = await _contentService.GetAsync(Enums.Content.MEMBERSHIP_AGREEMENT);

            return Ok(new ContentModel
            {
                Content = content.Data.Content
            });
        }
        [HttpGet("DashboardMostInbound")]
        public async Task<IActionResult> DashboardMostInbound(byte? _f)
        {
            var userId = User.Claims.FirstOrDefault(c => c.Type == JwtRegisteredClaimNames.Jti).Value;

            Enums.FilterTimes _filter = (_f == null || _f == 0) ? Enums.FilterTimes.MONTHLY : (Enums.FilterTimes)_f;

            var _filters = 0;

            if (_filter == Enums.FilterTimes.DAILY)
                _filters = (int)Enums.ConsoleQueryType.DASHBOARD_INBOUND_DAILY;
            else if (_filter == Enums.FilterTimes.WEEKLY)
                _filters = (int)Enums.ConsoleQueryType.DASHBOARD_INBOUND_WEEKLY;
            else if (_filter == Enums.FilterTimes.MONTHLY)
                _filters = (int)Enums.ConsoleQueryType.DASHBOARD_INBOUND_MONTHLY;
            else
                _filters = (int)Enums.ConsoleQueryType.DASHBOARD_INBOUND_YEARLY;

            var query =  _staticService.GetQueries(LoggedInUser(userId), (Enums.ConsoleQueryType)_filters);

            if (query.ResultStatus == ResultStatus.Success)
            {
                var obj = new List<Entities.Dtos.DashboardMostInboundOutboundDto>();

                string queryData = query.Data;

                if (!string.IsNullOrWhiteSpace(queryData) && !queryData.Equals("[]"))
                    obj = JsonConvert.DeserializeObject<List<Entities.Dtos.DashboardMostInboundOutboundDto>>(queryData);

                var htmlContent = PartialViewGen.GenerateDashboardInboundHtml(obj);
                return Ok(htmlContent);
            }
            else
            {
                var emptyContent = "<p class=\"pt-1\">No data available for the selected time range.</p>";
                return Ok( emptyContent );
            }
               
        }

        [HttpGet("DashboardMostAnswered")]
        public async Task<IActionResult> DashboardMostAnswered(byte? _f)
        {
            var userId = User.Claims.FirstOrDefault(c => c.Type == JwtRegisteredClaimNames.Jti).Value;
            Enums.FilterTimes _filter = (_f == null || _f == 0) ? Enums.FilterTimes.MONTHLY : (Enums.FilterTimes)_f;

            var _filters = 0;

            if (_filter == Enums.FilterTimes.DAILY)
                _filters = (int)Enums.ConsoleQueryType.DASHBOARD_ANSWERED_CALLS_DAILY;
            else if (_filter == Enums.FilterTimes.WEEKLY)
                _filters = (int)Enums.ConsoleQueryType.DASHBOARD_ANSWERED_CALLS_WEEKLY;
            else if (_filter == Enums.FilterTimes.MONTHLY)
                _filters = (int)Enums.ConsoleQueryType.DASHBOARD_ANSWERED_CALLS_MONTHLY;
            else
                _filters = (int)Enums.ConsoleQueryType.DASHBOARD_ANSWERED_CALLS_YEARLY;

            var query = _staticService.GetQueries(LoggedInUser(userId), (Enums.ConsoleQueryType)_filters);

            if (query.ResultStatus == ResultStatus.Success)
            {
                var obj = new List<Entities.Dtos.DashboardLeastAnsweredDto>();

                string queryData = query.Data;

                if (!string.IsNullOrWhiteSpace(queryData) && !queryData.Equals("[]"))
                    obj = JsonConvert.DeserializeObject<List<Entities.Dtos.DashboardLeastAnsweredDto>>(queryData);

                var htmlContent = PartialViewGen.GenerateDashboardMostAnsweredHtml(obj);
                return Ok(htmlContent);
            }
            var emptyContent = "<p class=\"pt-1\">No data available for the selected time range.</p>";
            return Ok(emptyContent);

        }

        [HttpGet("DashboardMostOutbound")]
        public async Task<IActionResult> DashboardMostOutbound(byte? _f)
        {


            var userId = User.Claims.FirstOrDefault(c => c.Type == JwtRegisteredClaimNames.Jti).Value;
            Enums.FilterTimes _filter = (_f == null || _f == 0) ? Enums.FilterTimes.MONTHLY : (Enums.FilterTimes)_f;

            var _filters = 0;

            if (_filter == Enums.FilterTimes.DAILY)
                _filters = (int)Enums.ConsoleQueryType.DASHBOARD_OUTBOUND_DAILY;
            else if (_filter == Enums.FilterTimes.WEEKLY)
                _filters = (int)Enums.ConsoleQueryType.DASHBOARD_OUTBOUND_WEEKLY;
            else if (_filter == Enums.FilterTimes.MONTHLY)
                _filters = (int)Enums.ConsoleQueryType.DASHBOARD_OUTBOUND_MONTHLY;
            else
                _filters = (int)Enums.ConsoleQueryType.DASHBOARD_OUTBOUND_YEARLY;

            var query = _staticService.GetQueries(LoggedInUser(userId), (Enums.ConsoleQueryType)_filters);

            if (query.ResultStatus == ResultStatus.Success)
            {
                var obj = new List<Entities.Dtos.DashboardMostInboundOutboundDto>();

                string queryData = query.Data;

                if (!string.IsNullOrWhiteSpace(queryData) && !queryData.Equals("[]"))
                    obj = JsonConvert.DeserializeObject<List<Entities.Dtos.DashboardMostInboundOutboundDto>>(queryData);

                var htmlContent = PartialViewGen.GenerateDashboardOutboundHtml(obj);
                return Ok(htmlContent);
            }
            else
            {
                var emptyContent = "<p class=\"pt-1\">No data available for the selected time range.</p>";
                return Ok(emptyContent);
            }
        }
        [HttpGet("DashboardMostTalk")]
        public async Task<IActionResult> DashboardMostTalk(byte? _f)
        {
            var userId = User.Claims.FirstOrDefault(c => c.Type == JwtRegisteredClaimNames.Jti).Value;
            Enums.FilterTimes _filter = (_f == null || _f == 0) ? Enums.FilterTimes.MONTHLY : (Enums.FilterTimes)_f;

            var _filters = 0;

            if (_filter == Enums.FilterTimes.DAILY)
                _filters = (int)Enums.ConsoleQueryType.DASHBOARD_TALK_TIME_DAILY;
            else if (_filter == Enums.FilterTimes.WEEKLY)
                _filters = (int)Enums.ConsoleQueryType.DASHBOARD_TALK_TIME_WEEKLY;
            else if (_filter == Enums.FilterTimes.MONTHLY)
                _filters = (int)Enums.ConsoleQueryType.DASHBOARD_TALK_TIME_MONTHLY;
            else
                _filters = (int)Enums.ConsoleQueryType.DASHBOARD_TALK_TIME_YEARLY;

            var query = _staticService.GetQueries(LoggedInUser(userId), (Enums.ConsoleQueryType)_filters);

            if (query.ResultStatus == ResultStatus.Success)
            {
                var obj = new List<Entities.Dtos.DashboardLeastTalkDto>();

                string queryData = query.Data;

                if (!string.IsNullOrWhiteSpace(queryData) && !queryData.Equals("[]"))
                    obj = JsonConvert.DeserializeObject<List<Entities.Dtos.DashboardLeastTalkDto>>(queryData);

                var htmlContent = PartialViewGen.GenerateDashboardMostTalkHtml(obj);
                return Ok(htmlContent);
            }
            var emptyContent = "<p class=\"pt-1\">No data available for the selected time range.</p>";
            return Ok(emptyContent);
        }

        [HttpGet("DownloadWindows")]
        public async Task<IActionResult> DownloadWindows()
        {
            var htmlContent = PartialViewGen.GenerateDownloadPageWindowHtml(_staticService);
            return Ok(htmlContent);
        }
        [HttpGet("DownloadLinux")]
        public async Task<IActionResult> DownloadLinux()
        {
            var htmlContent = PartialViewGen.GenerateDownloadLinuxPageHtml(_staticService);
            return Ok(htmlContent);
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
        protected User LoggedInUser(string id) => _userManager.FindByIdAsync(id).Result;
       




    }
}
