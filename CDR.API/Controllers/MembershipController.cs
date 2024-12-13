using AutoMapper;
using CDR.API.Api.Model;
using CDR.API.Api.Service.Interface;
using CDR.API.PartialViewGeneration;
using CDR.Entities.Concrete;
using CDR.Entities.Dtos;
using CDR.Services.Abstract;
using CDR.Services.Utilities;
using CDR.Shared.Utilities.Results.ComplexTypes;
using CDR.Shared.Utilities.Results.Concrete;
using Iyzipay.Model;
using Iyzipay.Request;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System.IdentityModel.Tokens.Jwt;
using System.Text.Json;

namespace CDR.API.Controllers
{
    [Authorize(AuthenticationSchemes = "Bearer")]
    [Route("api/memebership")]
    [ApiController]
    public class MembershipController : ControllerBase
    {
        private readonly IUserActivePackagesService _userActivePackagesService;
        private readonly IPackageService _packagesService;
        private readonly IContentService _contentService;
        private readonly IDepositService _depositService;
        private readonly ICountryService _countryService;
        private readonly ICityService _cityService;
        private readonly IyzicoSettings _iyzicoSettings;
        private readonly ITransactionService _transactionService;
        private readonly IMailService _mailService;
        private readonly ILogger<MembershipController> _logger;
        private readonly IApi3cxService _api3CxService;
        private readonly IMapper _mapper;
        private readonly UserManager<User> _userManager;
        private readonly IStaticService _staticService;
        private readonly IUserService _userService;

        public MembershipController(IApi3cxService api3CxService, ILogger<MembershipController> logger, IMailService mailService, ITransactionService transactionService, IOptions<IyzicoSettings> iyzicoSettings, ICityService cityService, ICountryService countryService, IDepositService depositService, IContentService contentService, IPackageService packagesService, IUserActivePackagesService userActivePackagesService, IUserService userService, IStaticService staticService, IMapper mapper, UserManager<User> userManager)
        {
            _api3CxService = api3CxService;
            _logger = logger;
            _mailService = mailService;
            _transactionService = transactionService;
            _iyzicoSettings = iyzicoSettings.Value;
            _cityService = cityService;
            _countryService = countryService;
            _depositService = depositService;
            _contentService = contentService;
            _packagesService = packagesService;
            _userActivePackagesService = userActivePackagesService;
            _userService = userService;
            _staticService = staticService;
            _mapper = mapper;
            _userManager = userManager;
        }

        [HttpGet("UserCurrentActiveSub")]
        public async Task<IActionResult> UserCurrentActiveSub()
        {
            var userId = User.Claims.FirstOrDefault(c => c.Type == JwtRegisteredClaimNames.Jti).Value;
            var userPackages = await _userActivePackagesService.GetAllAsync(LoggedInUser(userId).Id);

            return Ok(new MembershipModel
            {
                User = LoggedInUser(userId),
                UserPackages = userPackages.Data.ActivePackages
            });
        }
        [HttpGet("GetBuyPackage")]
        public async Task<IActionResult> GetBuyPackage()
        {
            var userId = User.Claims.FirstOrDefault(c => c.Type == JwtRegisteredClaimNames.Jti).Value;
            var packages = await _packagesService.GetAllWithoutTrialAsync(LoggedInUser(userId).SimultaneousCalls ?? 32);
            var countries = _staticService.GetAllCountry();
            var distanceSellingContract = await _contentService.GetAsync(Enums.Content.DISTANCE_SALES_AGREEMENT);
            var privacyAgreement = await _contentService.GetAsync(Enums.Content.PRIVACY_AGREEMENT);

            return Ok(PartialViewGen.GenerateMembershipBuyPackageHtml(new MembershipBuyPackageModel
            {
                User = LoggedInUser(userId),
                PackageList = packages.Data.Packages,
                PrivacyAgreement = privacyAgreement.Data.Content != null ? privacyAgreement.Data.Content.Text : "",
                DistanceSellingContract = distanceSellingContract.Data.Content != null ? distanceSellingContract.Data.Content.Text : "",
                Countries = countries.Data
            }, _staticService));
        }
        [HttpPost("CreateDeposit")]
        public async Task<IActionResult> CreateDeposit(DepositAddDto data)
        {
            var userId = User.Claims.FirstOrDefault(c => c.Type == JwtRegisteredClaimNames.Jti).Value;
            data.User = LoggedInUser(userId);

            var add = await _depositService.AddAsync(data);

            if (add.ResultStatus == ResultStatus.Success)
            {
                var addModel = System.Text.Json.JsonSerializer.Serialize(new
                {
                    ResultStatus = ResultStatus.Success,
                    Message = add.Message,
                    PublicId = add.Data
                });

                return Ok(addModel);
            }
            else
            {

                var addModel = System.Text.Json.JsonSerializer.Serialize(new
                {
                    ResultStatus = ResultStatus.Error,
                    Message = add.Message
                });

                return Ok(addModel);
            }
        }
        [HttpPost("InvoiceInformation")]
        public async Task<IActionResult> InvoiceInformation(MembershipInvoiceInformationDto membershipInvoiceInformationDto)
        {
            var userId = User.Claims.FirstOrDefault(c => c.Type == JwtRegisteredClaimNames.Jti).Value;
            if (membershipInvoiceInformationDto.CountryId == null || membershipInvoiceInformationDto.CountryId == 0)
            {
                var userProfileInformationAjaxModel = JsonSerializer.Serialize(new UserProfileInformationAjaxViewModel
                {
                    ResultStatus = ResultStatus.Error,
                    Message = _staticService.GetLocalization("DBO_UlkeSeciniz").Data,
                });
                return Ok(userProfileInformationAjaxModel);
            }

            if (membershipInvoiceInformationDto.CityId == null || membershipInvoiceInformationDto.CityId == 0)
            {
                var userProfileInformationAjaxModel = JsonSerializer.Serialize(new UserProfileInformationAjaxViewModel
                {
                    ResultStatus = ResultStatus.Error,
                    Message = _staticService.GetLocalization("DBO_SehirSeciniz").Data,
                });
                return Ok(userProfileInformationAjaxModel);
            }

            if (string.IsNullOrWhiteSpace(membershipInvoiceInformationDto.Address))
            {
                var userProfileInformationAjaxModel = JsonSerializer.Serialize(new UserProfileInformationAjaxViewModel
                {
                    ResultStatus = ResultStatus.Error,
                    Message = _staticService.GetLocalization("DBO_AdresSeciniz").Data,
                });
                return Ok(userProfileInformationAjaxModel);
            }

            var oldUser = await _userManager.FindByIdAsync(userId);

            oldUser.CountryId = membershipInvoiceInformationDto.CountryId;
            oldUser.CityId = membershipInvoiceInformationDto.CityId;
            oldUser.Address = membershipInvoiceInformationDto.Address;
            oldUser.ZipCode = membershipInvoiceInformationDto.ZipCode;

            var result = await _userManager.UpdateAsync(oldUser);

            if (result.Succeeded)
            {
                var userProfileInformationAjaxModel = JsonSerializer.Serialize(new UserProfileInformationAjaxViewModel
                {
                    ResultStatus = ResultStatus.Success,
                    Message = _staticService.GetLocalization("DBO_FaturaBilgileriDuzenlendi").Data,
                });
                return Ok(userProfileInformationAjaxModel);
            }
            else
            {
                var userProfileInformationAjaxErrorModel = JsonSerializer.Serialize(new UserProfileInformationAjaxViewModel
                {
                    ResultStatus = ResultStatus.Error,
                    Message = _staticService.GetLocalization("DBO_BirHataOlustu").Data,
                });
                return Ok(userProfileInformationAjaxErrorModel);
            }
        }
        [HttpGet("CreditCard")]
        public async Task<IActionResult> CreditCard(string DepositId)
        {
            var mdl = new MembershipCreditCardModel();

            try
            {
                var deposit = await _depositService.GetAsync(DepositId);

                if (deposit.ResultStatus == ResultStatus.Error)
                {
                    return Ok(PartialViewGen.GenerateMembershipCreditCardHtml(new MembershipCreditCardModel
                    {
                        ResultStatus = ResultStatus.Error,
                        Message = deposit.Message
                    }, _staticService));
                }

                var city = await _cityService.GetAsync((int)(deposit.Data.User.CityId ?? 0));
                var country = await _countryService.GetAsync((int)(deposit.Data.User.CountryId ?? 0));

                Iyzipay.Options options = new Iyzipay.Options();
                options.ApiKey = _iyzicoSettings.ApiKey;
                options.SecretKey = _iyzicoSettings.SecretKey;
                options.BaseUrl = _iyzicoSettings.BaseUrl;

                var culture = System.Globalization.CultureInfo.CurrentCulture;
                var exchangeRate = GetExchangeRate();
                var price = !culture.Name.Equals("tr-TR") ? deposit.Data.Package.YearPrice.ToString().Replace(",", ".") : (deposit.Data.Package.YearPrice * exchangeRate).ToString().Replace(",", ".");

                CreateCheckoutFormInitializeRequest request = new CreateCheckoutFormInitializeRequest();
                request.Locale = culture.Name.Equals("tr-TR") ? Locale.TR.ToString() : Locale.EN.ToString();
                request.ConversationId = deposit.Data.PublicId.ToString();
                request.Price = price;
                request.PaidPrice = price;
                request.Currency = culture.Name.Equals("tr-TR") ? Currency.TRY.ToString() : (deposit.Data.Package.Currency == (byte)Enums.Currency.DOLAR ? Currency.USD.ToString() : Currency.EUR.ToString());
                request.BasketId = deposit.Data.PublicId.ToString();
                request.PaymentGroup = PaymentGroup.PRODUCT.ToString();
                request.CallbackUrl = _iyzicoSettings.DomainUrl + _iyzicoSettings.CallSubpartUrl + "/dashboard/membership/comfirm";

                List<int> enabledInstallments = new List<int>();
                enabledInstallments.Add(2);
                enabledInstallments.Add(3);
                enabledInstallments.Add(6);
                enabledInstallments.Add(9);
                request.EnabledInstallments = enabledInstallments;

                Buyer buyer = new Buyer();
                buyer.Id = "BY-" + deposit.Data.User.Id;
                buyer.Name = deposit.Data.User.FirstName;
                buyer.Surname = deposit.Data.User.LastName;
                buyer.GsmNumber = "+905555555555";
                buyer.Email = deposit.Data.User.Email;
                buyer.IdentityNumber = "10000000000";
                buyer.LastLoginDate = string.Format("{0:yyyy-MM-dd HH:mm:ss}", DateTime.Now);
                buyer.RegistrationDate = string.Format("{0:yyyy-MM-dd HH:mm:ss}", DateTime.Now.AddDays(-1));
                buyer.RegistrationAddress = deposit.Data.User.Address;
                buyer.Ip = GetIpAddress();
                buyer.City = city.Data.City.Name;
                buyer.Country = country.Data.Country.Name;
                buyer.ZipCode = string.IsNullOrWhiteSpace(deposit.Data.User.ZipCode) ? "34732" : deposit.Data.User.ZipCode;
                request.Buyer = buyer;

                Address shippingAddress = new Address();
                shippingAddress.ContactName = deposit.Data.User.FirstName + " " + deposit.Data.User.LastName;
                shippingAddress.City = city.Data.City.Name;
                shippingAddress.Country = country.Data.Country.Name;
                shippingAddress.Description = deposit.Data.User.Address;
                shippingAddress.ZipCode = string.IsNullOrWhiteSpace(deposit.Data.User.ZipCode) ? "34732" : deposit.Data.User.ZipCode;
                request.ShippingAddress = shippingAddress;

                Address billingAddress = new Address();
                billingAddress.ContactName = deposit.Data.User.FirstName + " " + deposit.Data.User.LastName;
                billingAddress.City = city.Data.City.Name;
                billingAddress.Country = country.Data.Country.Name;
                billingAddress.Description = deposit.Data.User.Address;
                billingAddress.ZipCode = string.IsNullOrWhiteSpace(deposit.Data.User.ZipCode) ? "34732" : deposit.Data.User.ZipCode;
                request.BillingAddress = billingAddress;

                List<BasketItem> basketItems = new List<BasketItem>();
                BasketItem firstBasketItem = new BasketItem();
                firstBasketItem.Id = deposit.Data.PublicId.ToString();
                firstBasketItem.Name = deposit.Data.Package.Name;
                firstBasketItem.Category1 = "Cdr Package";
                firstBasketItem.ItemType = BasketItemType.PHYSICAL.ToString();
                firstBasketItem.Price = price;
                basketItems.Add(firstBasketItem);

                request.BasketItems = basketItems;

                CheckoutFormInitialize checkoutFormInitialize = CheckoutFormInitialize.Create(request, options);
/*
                HttpContext.Session.SetString("Deposit", deposit.Data.PublicId.ToString());*/

                return Ok(PartialViewGen.GenerateMembershipCreditCardHtml(new MembershipCreditCardModel
                {
                    ResultStatus = ResultStatus.Success,
                    CheckoutForm = checkoutFormInitialize.CheckoutFormContent,
                    DepositDetail = new DepositDetailDto
                    {
                        Address = deposit.Data.User.Address,
                        City = city.Data.City.Name,
                        Country = country.Data.Country.Name,
                        ZipCode = deposit.Data.User.ZipCode,
                        Currency = deposit.Data.Package.Currency,
                        Month = deposit.Data.Package.Month,
                        MontlyPrice = deposit.Data.Package.MonthPrice,
                        YearPrice = deposit.Data.Package.YearPrice,
                        PackageName = deposit.Data.Package.Name,
                        DepositPublicId = deposit.Data.PublicId
                    },
                    ExhangeRate = culture.Name.Equals("tr-TR") ? exchangeRate.ToString() : ""
                }, _staticService));
            }
            catch (Exception ex)
            {
                _logger.LogError("CreditCard -> " + ex.Message);
            }

            return Ok(PartialViewGen.GenerateMembershipCreditCardHtml(new MembershipCreditCardModel
            {
                ResultStatus = ResultStatus.Error,
                Message = _staticService.GetLocalization("DBO_BirHataOlustu").Data
            }, _staticService));
        }
        [HttpGet("TransactionResult")]
        public async Task<IActionResult> TransactionResult(string token)
        {
            string paymentId = string.Empty;

            try
            {
                Iyzipay.Options options = new Iyzipay.Options();
                options.ApiKey = _iyzicoSettings.ApiKey;
                options.SecretKey = _iyzicoSettings.SecretKey;
                options.BaseUrl = _iyzicoSettings.BaseUrl;

                RetrieveCheckoutFormRequest request = new RetrieveCheckoutFormRequest();
                request.Token = token;

                CheckoutForm checkoutForm = CheckoutForm.Retrieve(request, options);

                if (!checkoutForm.Status.Equals("success"))
                {
                    var _error = IyzicoErrorCodes.ErrorList.Where(x => x.errorCode == checkoutForm.ErrorCode).FirstOrDefault();

                    var culture = System.Globalization.CultureInfo.CurrentCulture;

                    if (_error == null)
                    {
                        var _errorOther = IyzicoErrorCodes.ErrorList.Where(x => x.errorCode == "-1").FirstOrDefault();

                        if (_errorOther == null)
                            return Ok(new MembershipTransactionResultModel
                            {
                                Message = checkoutForm.ErrorCode + " - " + checkoutForm.ErrorMessage,
                                ResultStatus = ResultStatus.Error,
                                Statue = "FAILURE"
                            });
                        else
                            return Ok(new MembershipTransactionResultModel
                            {
                                Message = _errorOther.errorCode + " - " + (culture.Name.Equals("tr-TR") ? _error.errorMessageTr : _error.errorMessageEn),
                                ResultStatus = ResultStatus.Error,
                                Statue = "FAILURE"
                            });
                    }
                    else
                    {
                        return Ok(new MembershipTransactionResultModel
                        {
                            Message = _error.errorCode + " - " + (culture.Name.Equals("tr-TR") ? _error.errorMessageTr : _error.errorMessageEn),
                            ResultStatus = ResultStatus.Error,
                            Statue = "FAILURE"
                        });
                    }
                }

                var mdl = new MembershipTransactionResultModel();
                var TransactionPaymentStatus = "WAITING";

                if (checkoutForm.PaymentStatus == "SUCCESS" || string.IsNullOrWhiteSpace(checkoutForm.PaymentStatus))
                {
                    TransactionPaymentStatus = "SUCCESS";
                }
                else if (
                    checkoutForm.PaymentStatus == "INIT_THREEDS" ||
                    checkoutForm.PaymentStatus == "CALLBACK_THREEDS" ||
                    checkoutForm.PaymentStatus == "BKM_POS_SELECTED" ||
                    checkoutForm.PaymentStatus == "CALLBACK_PECCO"
                    )
                {
                    TransactionPaymentStatus = "WAITING";
                    mdl.Message = _staticService.GetLocalization("CDR_OdemeBankadanOnayBekliyor").Data;
                }
                else if (checkoutForm.PaymentStatus == "FAILURE")
                {
                    TransactionPaymentStatus = "FAILURE";
                }

                if (TransactionPaymentStatus.Equals("FAILURE"))
                {
                    var _errorOther = IyzicoErrorCodes.ErrorList.Where(x => x.errorCode == "-1").FirstOrDefault();

                    var culture = System.Globalization.CultureInfo.CurrentCulture;

                    if (_errorOther != null)
                        return Ok(new MembershipTransactionResultModel
                        {
                            Message = _errorOther.errorCode + " - " + (culture.Name.Equals("tr-TR") ? _errorOther.errorMessageTr : _errorOther.errorMessageEn),
                            ResultStatus = ResultStatus.Error,
                            Statue = "FAILURE"
                        });
                    else
                        return Ok(new MembershipTransactionResultModel
                        {
                            Message = "-1" + " - " + _staticService.GetLocalization("DBO_OdemeIslemindeHataOlustu").Data,
                            ResultStatus = ResultStatus.Error,
                            Statue = "FAILURE"
                        });
                }

                paymentId = checkoutForm.PaymentId;

                if (string.IsNullOrWhiteSpace(paymentId))
                    return Ok(new MembershipTransactionResultModel
                    {
                        Message = "-1" + " - " + _staticService.GetLocalization("DBO_OdemeIslemindeHataOlustu").Data,
                        ResultStatus = ResultStatus.Error,
                        Statue = "FAILURE"
                    });

                var deposit = await _depositService.GetAsync(checkoutForm.BasketId);

                if (deposit == null)
                {
                    return Ok(new MembershipTransactionResultModel
                    {
                        ResultStatus = ResultStatus.Error,
                        Message = _staticService.GetLocalization("CDR_SistemselOdemeHata").Data.Replace("-_Replace_-", paymentId)
                    });
                }

                var user = await _userManager.FindByEmailAsync(deposit.Data.User.Email);

                if (user == null)
                {
                    return Ok(new MembershipTransactionResultModel
                    {
                        ResultStatus = ResultStatus.Error,
                        Message = _staticService.GetLocalization("CDR_SistemselOdemeHata").Data.Replace("-_Replace_-", paymentId)
                    });
                }

                byte _currency = (byte)Enums.Currency.TL;

                switch (checkoutForm.Currency)
                {
                    case "TRY":
                        _currency = (byte)Enums.Currency.TL;
                        break;
                    case "USD":
                        _currency = (byte)Enums.Currency.DOLAR;
                        break;
                    case "EUR":
                        _currency = (byte)Enums.Currency.EURO;
                        break;
                    default:
                        break;
                }

                decimal _price = checkoutForm.PaidPrice.ToDecimal();

                var transaction =
                    await _transactionService.AddAsync(
                        deposit.Data, paymentId,
                        _price,
                        _currency,
                        new TransactionDetails
                        {
                            binNumber = checkoutForm.BinNumber,
                            cardAssociation = checkoutForm.CardAssociation,
                            cardFamily = checkoutForm.CardFamily,
                            cardType = checkoutForm.CardType,
                            currency = checkoutForm.Currency,
                            lastFourDigits = checkoutForm.LastFourDigits,
                            paidPrice = checkoutForm.PaidPrice,
                            paymentId = checkoutForm.PaymentId,
                            paymentStatus = checkoutForm.PaymentStatus,
                            paymentTransactionId = checkoutForm.PaymentItems.Count > 0 ? checkoutForm.PaymentItems[0].PaymentTransactionId : "",
                            price = checkoutForm.Price
                        });

                if (transaction.ResultStatus == ResultStatus.Error)
                    return Ok(new MembershipTransactionResultModel
                    {
                        ResultStatus = ResultStatus.Error,
                        Message = _staticService.GetLocalization("CDR_SistemselOdemeHata").Data.Replace("-_Replace_-", paymentId)
                    });

                user.PackageFinishDate = transaction.Data;

                var update = await _userManager.UpdateAsync(user);

                if (!update.Succeeded)
                    return Ok
                        (new MembershipTransactionResultModel
                        {
                            ResultStatus = ResultStatus.Error,
                            Message = _staticService.GetLocalization("CDR_SistemselOdemeHata").Data.Replace("-_Replace_-", paymentId)
                        });

                TransactionPDFModel transactionPDFModel = new TransactionPDFModel
                {
                    CustomerMail = user.Email,
                    CustomerName = user.FirstName + " " + user.LastName,
                    CustomerAddress = user.Address,
                    Currency = checkoutForm.Currency,
                    InvoiceNumber = checkoutForm.PaymentId,
                    ProductName = deposit.Data.Package.Name,
                    Quantity = "1",
                    SubTotal = checkoutForm.Price,
                    Tax = "0.00",
                    Total = checkoutForm.Price,
                    UnitPrice = checkoutForm.Price,
                    Period = deposit.Data.Package.Month.ToString()
                };

                var _result = _mailService.SendTransactionEmail(new EmailSendDto
                {
                    Email = user.Email,
                    Name = user.FirstName,
                    PaymentID = checkoutForm.PaymentId,
                    Subject = _staticService.GetLocalization("EMAIL_PaketinizTanimlandi").Data
                }, transactionPDFModel);

                _mailService.SendSalesEmailByResendClient();

                mdl.Message = transaction.Message;
                mdl.ResultStatus = transaction.ResultStatus;
                mdl.Statue = TransactionPaymentStatus;

                return Ok(mdl);
            }
            catch (Exception ex)
            {
                _logger.LogError("TransactionResult -> " + ex.Message);
                return Ok(new MembershipTransactionResultModel
                {
                    ResultStatus = ResultStatus.Error,
                    Message = string.IsNullOrWhiteSpace(paymentId) ? _staticService.GetLocalization("CDR_SistemselHata").Data : _staticService.GetLocalization("CDR_SistemselOdemeHata").Data.Replace("-_Replace_-", paymentId)
                });
            }
        }
        [HttpPost("GetTransactionList")]
        public async Task<IActionResult> GetTransactionList()
        {
            var userId = User.Claims.FirstOrDefault(c => c.Type == JwtRegisteredClaimNames.Jti).Value;
            var list = await _transactionService.GetAllAsync(LoggedInUser(userId).Id);

            return Ok(list.Data);
        }

        [HttpPost("UsePromotionCode")]
        public async Task<IActionResult> UsePromotionCode(string PromotionCode)
        {
            var userId = User.Claims.FirstOrDefault(c => c.Type == JwtRegisteredClaimNames.Jti).Value;

            var user = _userManager.FindByIdAsync(LoggedInUser(userId).Id.ToString()).Result;

            if (user is null)
            {
                var errorModel = JsonSerializer.Serialize(new
                {
                    ResultStatus = ResultStatus.Error,
                    Message = _staticService.GetLocalization("DBO_IslemYapmakIstediginizKullaniciBulunamadi").Data
                });
                return Ok(errorModel);
            }

            var licenseKeyResponse = await _api3CxService.GetLicenseKeyForUser(user);

            if (licenseKeyResponse.ResultStatus == ResultStatus.Error)
            {
                var errorModel = JsonSerializer.Serialize(new
                {
                    ResultStatus = ResultStatus.Error,
                    Message = licenseKeyResponse.Message
                });
                return Ok(errorModel);
            }

            var isPromotionCodeUsed = await _transactionService.IsPromotionCodeUsed(PromotionCode, licenseKeyResponse.Data);

            if (isPromotionCodeUsed.ResultStatus == ResultStatus.Error)
            {
                var errorModel = JsonSerializer.Serialize(new
                {
                    ResultStatus = ResultStatus.Error,
                    Message = _staticService.GetLocalization("CDR_PromotionCodeUsedWarning").Data
                });
                return Ok(errorModel);
            }

            var orderApiResponse = await _api3CxService.CheckLicenseKeyFromOrderAPI(PromotionCode, licenseKeyResponse.Data);

            if (orderApiResponse.ResultStatus == ResultStatus.Error)
            {
                var errorModel = JsonSerializer.Serialize(new
                {
                    ResultStatus = ResultStatus.Error,
                    Message = _staticService.GetLocalization("CDR_PromotionCodeUsedWarning").Data
                });
                return Ok(errorModel);
            }

            user.PackageFinishDate = user.PackageFinishDate.AddYears(1);

            var update = await _userManager.UpdateAsync(user);

            if (!update.Succeeded)
            {
                var errorModel = JsonSerializer.Serialize(new
                {
                    ResultStatus = ResultStatus.Error,
                    Message = _staticService.GetLocalization("CDR_Error").Data
                });
                return Ok(errorModel);
            }

            PromotionUsageDto promotionUsageDto = new PromotionUsageDto
            {
                PromotionCode = PromotionCode,
                UserId = user.Id,
            };

            var addPromotionResponse = await _transactionService.AddPromotionUsage(promotionUsageDto, licenseKeyResponse.Data);

            if (addPromotionResponse.ResultStatus == ResultStatus.Error)
            {
                var errorModel = JsonSerializer.Serialize(new
                {
                    ResultStatus = ResultStatus.Error,
                    Message = _staticService.GetLocalization("CDR_Error").Data
                });
                return Ok(errorModel);
            }

            var successModel = JsonSerializer.Serialize(new
            {
                ResultStatus = ResultStatus.Success,
                Message = _staticService.GetLocalization("CDR_PackageExpireDateUpdated").Data
            });

            return Ok(successModel);
        }
        protected string GetIpAddress()
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
        protected decimal GetExchangeRate()
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

        protected User LoggedInUser(string id) => _userManager.FindByIdAsync(id).Result;
    }
}
