using CDR.Entities.Concrete;
using CDR.Entities.Dtos;
using CDR.Services.Abstract;
using CDR.Shared.Utilities.Results.Abstract;
using CDR.Shared.Utilities.Results.ComplexTypes;
using CDR.Shared.Utilities.Results.Concrete;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using System;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Log = Serilog.Log;

namespace CDR.Services.Concrete
{
    public class Api3cxManager : IApi3cxService
    {
        private readonly GlobalSettings _globalSettings;
        public Api3cxManager(IOptions<GlobalSettings> globalSettings)
        {
            _globalSettings = globalSettings.Value;
        }

        public async Task<IDataResult<UserConnectionDetailDto>> GetApiConnectionDetails(User User)
        {
            var data = new UserConnectionDetailDto();

            try
            {
                string ipAddress = string.IsNullOrWhiteSpace(User.IpAddress) ? "" : User.IpAddress;

                string URI = "https://" + ipAddress + ":8899/api/SystemInfo";

                HttpWebRequest request = (HttpWebRequest)HttpWebRequest.Create(URI);
                request.Method = "GET";

                String responseText = String.Empty;
                using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
                {
                    Stream dataStream = response.GetResponseStream();
                    StreamReader reader = new StreamReader(dataStream);
                    responseText = reader.ReadToEnd();
                    reader.Close();
                    reader.Dispose();
                    dataStream.Close();
                    dataStream.Dispose();
                }

                if (responseText.Length > 0)
                {
                    var obj = JsonConvert.DeserializeObject<UserConnectionDetailDto>(responseText);

                    return new DataResult<UserConnectionDetailDto>(ResultStatus.Success, new UserConnectionDetailDto
                    {
                        DbName = "database_single",
                        DbPassword = obj.DbPassword,
                        DbUsername = "phonesystem"
                    });
                }
            }
            catch (Exception ex)
            {
                Log.Error(ex, "GetApiConnectionDetails Exception");
            }

            return new DataResult<UserConnectionDetailDto>(ResultStatus.Error, data);
        }

        public async Task<IDataResult<OrderApiDto>> GetSimultaneousCalls(User User)
        {
            var data = new OrderApiDto();

            try
            {
                string ipAddress = string.IsNullOrWhiteSpace(User.IpAddress) ? "" : User.IpAddress;

                string URI = "https://" + ipAddress + ":8899/api/SystemInfo/GetKeyInfo";

                System.Net.Http.HttpClient client = new System.Net.Http.HttpClient();

                var response = await client.GetAsync(URI);

                string responseStringForRenewPrice = string.Empty;
                HttpResponseMessage renewPriceResponse = new HttpResponseMessage();

                if (response.IsSuccessStatusCode)
                {
                    var responseString = response.Content.ReadAsStringAsync().Result;
                    GetKeyInfoResult getKeyInfoResult = JsonConvert.DeserializeObject<GetKeyInfoResult>(responseString);

                    string URI2 = "https://api.3cx.com/public/v1/order/RenewPrice?licenseKey=" + getKeyInfoResult.LicenseKey + "&years=1";
                    string username = "NmOPDFVyTSdQXtcEbwak";
                    string password = "";
                    var byteArray = Encoding.ASCII.GetBytes($"{username}:{password}");
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", Convert.ToBase64String(byteArray));

                    renewPriceResponse = await client.GetAsync(URI2);

                    if (renewPriceResponse.IsSuccessStatusCode)
                    {
                        responseStringForRenewPrice = renewPriceResponse.Content.ReadAsStringAsync().Result;
                        OrderAPIRenewSuccessResult orderAPIRenewSuccessResult = JsonConvert.DeserializeObject<OrderAPIRenewSuccessResult>(responseStringForRenewPrice);

                        Log.Information($"GetSimultaneousCalls -> {ipAddress} => Order API Returned Successfull StatusCode:{renewPriceResponse.StatusCode} => {orderAPIRenewSuccessResult.Edition}/{orderAPIRenewSuccessResult.SimultaneousCalls}");

                        return new DataResult<OrderApiDto>(ResultStatus.Success, new OrderApiDto
                        {
                            Edition = orderAPIRenewSuccessResult.Edition,
                            SimultaneousCalls = orderAPIRenewSuccessResult.SimultaneousCalls
                        });
                    }
                }

                Log.Error($"GetSimultaneousCalls -> {ipAddress} => Order API Returned Unsuccessfull StatusCode:{renewPriceResponse.StatusCode} => {responseStringForRenewPrice}");
                Log.Warning($"GetSimultaneousCalls -> For {ipAddress} => Simultaneous Calls Set To Default(32) Update Required");

                return new DataResult<OrderApiDto>(ResultStatus.Success, new OrderApiDto
                {
                    Edition = "Default",
                    SimultaneousCalls = 32
                });
            }
            catch (Exception ex)
            {
                Log.Error(ex, "GetSimultaneousCalls Exception");
                return new DataResult<OrderApiDto>(ResultStatus.Success, new OrderApiDto
                {
                    Edition = "Default",
                    SimultaneousCalls = 32
                });
            }
        }

        public async Task<IDataResult<string>> GetLicenseKeyForUser(User User)
        {
            try
            {
                string ipAddress = string.IsNullOrWhiteSpace(User.IpAddress) ? "" : User.IpAddress;

                string URI = "https://" + ipAddress + ":8899/api/SystemInfo/GetKeyInfo";

                System.Net.Http.HttpClient client = new System.Net.Http.HttpClient();

                var response = await client.GetAsync(URI);

                if (response.IsSuccessStatusCode)
                {
                    var responseString = response.Content.ReadAsStringAsync().Result;
                    GetKeyInfoResult getKeyInfoResult = JsonConvert.DeserializeObject<GetKeyInfoResult>(responseString);
                    return new DataResult<string>(ResultStatus.Success, getKeyInfoResult.LicenseKey);
                }

                return new DataResult<string>(ResultStatus.Error, "License Key Not Found For User", null);
            }
            catch (Exception ex)
            {
                Log.Error(ex, "GetLicenseKeyForUser Exception For User => " + User.FirstName + " " + User.LastName);
                return new DataResult<String>(ResultStatus.Error, "", null);
            }
        }

        public async Task<IDataResult<string>> CheckLicenseKeyFromOrderAPI(string PromotionCode, string LicenseKey)
        {
            try
            {
                string requestURL = "https://order.k2msoftware.com/api/coupon/check";
                HttpClient client = new HttpClient();

                var requestObject = new
                {
                    licenseKey = LicenseKey,
                    couponCode = PromotionCode
                };

                //var requestModel = System.Text.Json.JsonSerializer.Serialize(new
                //{
                //    PromotionCode = PromotionCode,
                //    LicenseKey = LicenseKey
                //});

                var response = await client.PostAsJsonAsync(requestURL, requestObject);

                if (response.IsSuccessStatusCode)
                {
                    var responseString = response.Content.ReadAsStringAsync().Result;

                    dynamic getKeyInfoResult = JsonConvert.DeserializeObject(responseString);

                    bool status = getKeyInfoResult.status ?? false;

                    if (!status)
                    {
                        return new DataResult<string>(ResultStatus.Error, "");
                    }

                    return new DataResult<string>(ResultStatus.Success, "");
                }

                return new DataResult<string>(ResultStatus.Error, "License Key Not Found For User", null);
            }
            catch (Exception ex)
            {
                Log.Error(ex, "CheckLicenseKeyFromOrderAPI Exception");
                return new DataResult<String>(ResultStatus.Error, "", null);
            }
        }

        public async Task<IResult> TriggerHangfire(string IpAddress)
        {
            try
            {
                var vm = new { IpAddress = IpAddress };
                using (var client = new WebClient())
                {
                    var dataString = JsonConvert.SerializeObject(vm);
                    client.Headers.Add(HttpRequestHeader.ContentType, "application/json");
                    client.UploadString(new Uri(_globalSettings.HangfireUrl), "POST", dataString);
                }
            }
            catch (Exception ex)
            {
                Log.Error(ex,"Trigger Hangfire Exception");
            }

            return new Result(ResultStatus.Success, "");
        }
    }
}
