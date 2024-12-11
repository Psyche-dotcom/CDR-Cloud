using AutoMapper;
using CDR.Entities.Concrete;
using CDR.Services.Abstract;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace CDR.API.Controllers
{
    public class BaseApiController : ControllerBase
    {
        public BaseApiController(UserManager<User> userManager, IMapper mapper, IStaticService staticSevice)
        {
            UserManager = userManager;
            Mapper = mapper;
            StaticService = staticSevice;
        }

        protected UserManager<User> UserManager { get; }
        protected IMapper Mapper { get; }
        protected IStaticService StaticService { get; }
        protected User LoggedInUser => UserManager.GetUserAsync(HttpContext.User).Result;

        protected List<TempTotalCalls> Weeks
        {
            get
            {
                var _list = new List<TempTotalCalls>();

                _list.Add(new TempTotalCalls
                {
                    Order = 1,
                    Value = StaticService.GetLocalization("CDR_Week0").Data
                });

                _list.Add(new TempTotalCalls
                {
                    Order = 2,
                    Value = StaticService.GetLocalization("CDR_Week1").Data
                });

                _list.Add(new TempTotalCalls
                {
                    Order = 3,
                    Value = StaticService.GetLocalization("CDR_Week2").Data
                });

                _list.Add(new TempTotalCalls
                {
                    Order = 4,
                    Value = StaticService.GetLocalization("CDR_Week3").Data
                });

                _list.Add(new TempTotalCalls
                {
                    Order = 5,
                    Value = StaticService.GetLocalization("CDR_Week4").Data
                });

                _list.Add(new TempTotalCalls
                {
                    Order = 6,
                    Value = StaticService.GetLocalization("CDR_Week5").Data
                });

                _list.Add(new TempTotalCalls
                {
                    Order = 7,
                    Value = StaticService.GetLocalization("CDR_Week6").Data
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
                    Value = StaticService.GetLocalization("CDR_Month0").Data
                });

                _list.Add(new TempTotalCalls
                {
                    Order = 2,
                    Value = StaticService.GetLocalization("CDR_Month1").Data
                });

                _list.Add(new TempTotalCalls
                {
                    Order = 3,
                    Value = StaticService.GetLocalization("CDR_Month2").Data
                });

                _list.Add(new TempTotalCalls
                {
                    Order = 4,
                    Value = StaticService.GetLocalization("CDR_Month3").Data
                });

                _list.Add(new TempTotalCalls
                {
                    Order = 5,
                    Value = StaticService.GetLocalization("CDR_Month4").Data
                });

                _list.Add(new TempTotalCalls
                {
                    Order = 6,
                    Value = StaticService.GetLocalization("CDR_Month5").Data
                });

                _list.Add(new TempTotalCalls
                {
                    Order = 7,
                    Value = StaticService.GetLocalization("CDR_Month6").Data
                });

                _list.Add(new TempTotalCalls
                {
                    Order = 8,
                    Value = StaticService.GetLocalization("CDR_Month7").Data
                });

                _list.Add(new TempTotalCalls
                {
                    Order = 9,
                    Value = StaticService.GetLocalization("CDR_Month8").Data
                });

                _list.Add(new TempTotalCalls
                {
                    Order = 10,
                    Value = StaticService.GetLocalization("CDR_Month9").Data
                });

                _list.Add(new TempTotalCalls
                {
                    Order = 11,
                    Value = StaticService.GetLocalization("CDR_Month10").Data
                });

                _list.Add(new TempTotalCalls
                {
                    Order = 12,
                    Value = StaticService.GetLocalization("CDR_Month11").Data
                });

                return _list;
            }
        }

        public class TempTotalCalls
        {
            public byte Order { get; set; }
            public string Value { get; set; }
        }

        public Thread StartTheThread(FileInfo _file)
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

        public string GetIpAddress()
        {
            try
            {
                var remoteIpAddress = HttpContext.Features.Get<IHttpConnectionFeature>()?.RemoteIpAddress;

                return remoteIpAddress != null ? remoteIpAddress.ToString() : string.Empty;
            }
            catch (Exception)
            {
                return string.Empty;
            }

        }

        public decimal GetExchangeRate()
        {
            try
            {
                string exchangeRate = "https://www.tcmb.gov.tr/kurlar/today.xml";
                var xmlDoc = new System.Xml.XmlDocument();
                xmlDoc.Load(exchangeRate);

                string euroXmlData = xmlDoc.SelectSingleNode("Tarih_Date/Currency[@Kod='EUR']/BanknoteSelling").InnerXml;

                decimal euroPrice = Convert.ToDecimal(euroXmlData, new System.Globalization.CultureInfo("en-US"));

                return euroPrice;
            }
            catch (Exception)
            {
                return 0;
            }
        }
    }
}
