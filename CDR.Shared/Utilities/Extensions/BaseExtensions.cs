using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Security.Cryptography;
using System.IO;
using CDR.Shared.Utilities.Results.ComplexTypes;
using System.Text.RegularExpressions;
using Log = Serilog.Log;
using SelectPdf;
using CDR.Shared.Utilities.Results.Concrete;
using System.Globalization;

namespace CDR.Shared.Utilities.Extensions
{
    public static class BaseExtensions
    {
        public static string ProfilePicture(string letter)
        {
            string _avatar = "/app-assets/images/avatar-man.png";

            try
            {
                string _filePath = "/app-assets/avatar/";

                switch (letter)
                {
                    case "ç":
                        _filePath += "CC.svg";
                        break;
                    case "i":
                        _filePath += "II.svg";
                        break;
                    case "ö":
                        _filePath += "OO.svg";
                        break;
                    case "ş":
                        _filePath += "SS.svg";
                        break;
                    case "ü":
                        _filePath += "UU.svg";
                        break;
                    default:
                        _filePath += letter.ToUpper() + ".svg";
                        break;
                }

                _avatar = _filePath;
            }
            catch (Exception)
            {
            }

            return _avatar;
        }

        private static string EncryptKey = "09Mehis2019Melis";

        public static string EncryptString(string plainText, string key = "")
        {
            key = string.IsNullOrWhiteSpace(key) ? EncryptKey : key;

            byte[] iv = new byte[16];
            byte[] array;

            using (Aes aes = Aes.Create())
            {
                aes.Key = Encoding.UTF8.GetBytes(key);
                aes.IV = iv;
                aes.Padding = PaddingMode.PKCS7;
                aes.Mode = CipherMode.CBC;

                ICryptoTransform encryptor = aes.CreateEncryptor(aes.Key, aes.IV);

                using (MemoryStream memoryStream = new MemoryStream())
                {
                    using (CryptoStream cryptoStream = new CryptoStream((Stream)memoryStream, encryptor, CryptoStreamMode.Write))
                    {
                        using (StreamWriter streamWriter = new StreamWriter((Stream)cryptoStream))
                        {
                            streamWriter.Write(plainText);
                        }

                        array = memoryStream.ToArray();
                    }
                }
            }

            return Convert.ToBase64String(array);
        }

        public static string Decrypt(string cipherText, string key = "")
        {
            string _return = string.Empty;
            
            try
            {
                key = string.IsNullOrWhiteSpace(key) ? EncryptKey : key;
                byte[] iv = new byte[16];
                byte[] buffer = Convert.FromBase64String(cipherText);

                using (Aes aes = Aes.Create())
                {
                    aes.Key = Encoding.UTF8.GetBytes(key);
                    aes.IV = iv;
                    ICryptoTransform decryptor = aes.CreateDecryptor(aes.Key, aes.IV);

                    using (MemoryStream memoryStream = new MemoryStream(buffer))
                    {
                        using (CryptoStream cryptoStream = new CryptoStream((Stream)memoryStream, decryptor, CryptoStreamMode.Read))
                        {
                            using (StreamReader streamReader = new StreamReader((Stream)cryptoStream))
                            {
                                _return = streamReader.ReadToEnd();
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Decrypt Error");
                Console.WriteLine(ex.ToString());
            }

            return _return;
        }

        public static string NpgsqlConnectionString(string IpAddress, string Port, string DbName, string DbUsername, string Password)
        {
            IpAddress = string.IsNullOrWhiteSpace(IpAddress) ? "" : IpAddress;
            Port = string.IsNullOrWhiteSpace(Port) ? "" : Port;
            DbName = string.IsNullOrWhiteSpace(DbName) ? "" : DbName;
            DbUsername = string.IsNullOrWhiteSpace(DbUsername) ? "" : DbUsername;
            Password = string.IsNullOrWhiteSpace(Password) ? "" : Decrypt(Password);

            var _default = "Server=@IpAddress;Port=@Port;Database=@DatabaseName;User Id=@Username;Password=@Password;CommandTimeout=0";

            _default = _default.Replace("@IpAddress", IpAddress);
            _default = _default.Replace("@Port", Port);
            _default = _default.Replace("@DatabaseName", DbName);
            _default = _default.Replace("@Username", DbUsername);
            _default = _default.Replace("@Password", Password);

            return _default;
        }

        public static FilterDateDto GetDateFilter(Enums.DashboardFilter _filter)
        {
            var _return = new FilterDateDto();

            try
            {
                DateTime _now = DateTime.Now;

                switch (_filter)
                {
                    case Enums.DashboardFilter.DAILY:
                        _return.StartDate = new DateTime(_now.Year, _now.Month, _now.Day, 0, 0, 0);
                        _return.EndDate = new DateTime(_now.Year, _now.Month, _now.Day, 23, 59, 59);
                        break;
                    case Enums.DashboardFilter.WEEKLY:
                        _return.StartDate = FirstDayOfWeek(DateTime.Now);
                        _return.EndDate = new DateTime(_now.Year, _now.Month, _now.Day, 23, 59, 59);
                        break;
                    case Enums.DashboardFilter.MONTHLY:
                        _return.StartDate = new DateTime(_now.Year, _now.Month, 1, 0, 0, 0);
                        _return.EndDate = new DateTime(_now.Year, _now.Month, _now.Day, 23, 59, 59);
                        break;
                    case Enums.DashboardFilter.ALL:
                        _return.StartDate = new DateTime(_now.Year, 1, 1, 0, 0, 0);
                        _return.EndDate = new DateTime(_now.Year, _now.Month, _now.Day, 23, 59, 59);
                        break;
                    default:
                        _return.StartDate = new DateTime(_now.Year, _now.Month, 1, 0, 0, 0);
                        _return.EndDate = new DateTime(_now.Year, _now.Month, _now.Day, 23, 59, 59);
                        break;
                }
            }
            catch (Exception)
            {
            }

            return _return;
        }

        public static string ParseDate(string s)
        {
            DateTime result;
            if (!DateTime.TryParse(s, out result))
            {
                result = DateTime.ParseExact(s, "yyyy-MM-ddT24:mm:ss", System.Globalization.CultureInfo.InvariantCulture);
                result = result.AddDays(1);
            }
            return string.Format("{0:yyyy-MM-dd HH:mm:ss}", result); ;
        }

        public static DateTime FirstDayOfWeek(DateTime dt)
        {
            var culture = System.Threading.Thread.CurrentThread.CurrentCulture;
            var diff = dt.DayOfWeek - culture.DateTimeFormat.FirstDayOfWeek;
            if (diff < 0)
                diff += 7;
            return dt.AddDays(-diff).Date;
        }

        public static TimeSpan GetGmtTime(decimal GMT)
        {
            var _gmtCurr = Convert.ToDouble(GMT);

            int _hour = (int)_gmtCurr;
            double _minute = _gmtCurr - _hour;

            _minute = _minute * 100;
            _minute = Math.Round(_minute, 2);

            TimeSpan _gmt = new TimeSpan(_hour, (int)Math.Ceiling(_minute), 0);

            return _gmt;
        }

        public static string PhotoTitle(string Title)
        {
            try
            {
                Title = Regex.Replace(Title, @"[ ]{2,}", "-");
                Title = Regex.Replace(Title, @"(\|@|&|'|\(|\)|<|>|#|)", "").Replace("?", "").ToLower();

                Title = Title.Replace("ç", "c").
                    Replace("ş", "s").
                    Replace("ğ", "g").
                    Replace("ı", "i").
                    Replace("ö", "o").
                    Replace("ü", "u").
                    Replace(" ", "-");
            }
            catch (Exception)
            {
            }

            return Title;
        }

        public static string GetUniqueKey(int maxSize)
        {
            var chars = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890".ToCharArray();
            var data = new byte[1];
            using (var crypto = new RNGCryptoServiceProvider())
            {
                crypto.GetNonZeroBytes(data);
                data = new byte[maxSize];
                crypto.GetNonZeroBytes(data);
            }
            var result = new StringBuilder(maxSize);
            foreach (var b in data)
            {
                result.Append(chars[b % (chars.Length)]);
            }
            return result.ToString();
        }

        public static string ToNumberString(this decimal value, string suffix = "", string prefix = "", int countAfterCommas = 0)
        {
            return prefix + " " + value.ToString("N" + countAfterCommas.ToString()) + " " + suffix;
        }

        public static string CurrencyIcon(Enums.Currency _curr)
        {
            string _result = "₺";

            switch (_curr)
            {
                case Enums.Currency.TL:
                    _result = "₺";
                    break;
                case Enums.Currency.DOLAR:
                    _result = "$";
                    break;
                case Enums.Currency.EURO:
                    _result = "€";
                    break;
                default:

                    break;
            }

            return _result;
        }
        public static string FormatDecimalToString(string priceValue, string currency)
        {
            decimal _price;
            string resultPrice = "0.00";

            priceValue = priceValue.Replace(',', '.');

            if (currency.Equals("EUR"))
            {
                if (Decimal.TryParse(priceValue, out _price))
                {
                    string priceInString = _price.ToString("0.00");
                    return priceInString;
                }
            }
            else if (currency.Equals("TRY"))
            {
                if (Decimal.TryParse(priceValue, NumberStyles.Any, CultureInfo.InvariantCulture, out _price))
                {
                    string priceInString = _price.ToString("0.00", CultureInfo.InvariantCulture);
                    return priceInString;
                }
            }

            return resultPrice;
        }

        public class FilterDateDto
        {
            public DateTime StartDate { get; set; }
            public DateTime EndDate { get; set; }

            public string StartDateString
            {
                get
                {
                    return ParseDate(string.Format("{0:yyyy-MM-dd HH:mm:ss}", this.StartDate));
                }
            }

            public string EndDateString
            {
                get
                {
                    return ParseDate(string.Format("{0:yyyy-MM-dd HH:mm:ss}", this.EndDate));
                }
            }

            public string StartDateStringV2
            {
                get
                {
                    return "('" + ParseDate(string.Format("{0:yyyy-MM-dd HH:mm:ss}", this.StartDate)) + "')::timestamp";
                }
            }

            public string EndDateStringV2
            {
                get
                {
                    return "('" + ParseDate(string.Format("{0:yyyy-MM-dd HH:mm:ss}", this.EndDate)) + "')::timestamp";
                }
            }
        }

        public class FilterDnDto
        {
            public string SourceDn { get; set; }
            public string TargetDn { get; set; }
            public string OtherSourceDn { get; set; }
            public string OtherTargetDn { get; set; }
        }

        public class FilterExtNumberDto
        {
            public string SourceCriteria { get; set; }
            public string TargetCriteria { get; set; }
        }

        public class FilterStatueDto
        {
            public string Psactionid { get; set; }
            public string Posactionid { get; set; }
            public string Podidntype { get; set; }
        }
    }
}
