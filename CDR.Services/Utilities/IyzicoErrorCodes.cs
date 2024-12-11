using CDR.Entities.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CDR.Services.Utilities
{
    public static class IyzicoErrorCodes
    {
        public static List<IyzicoErrorCodeDto> ErrorList { get; set; }

        static IyzicoErrorCodes()
        {
            ErrorList = new List<IyzicoErrorCodeDto>();

            ErrorList.Add(new IyzicoErrorCodeDto
            {
                errorCode = "10051",
                errorGroup = "NOT_SUFFICIENT_FUNDS",
                errorMessageTr = "Kart limiti yetersiz, yetersiz bakiye",
                errorMessageEn = "Insufficient funds"
            });

            ErrorList.Add(new IyzicoErrorCodeDto
            {
                errorCode = "10005",
                errorGroup = "DO_NOT_HONOUR",
                errorMessageTr = "İşlem onaylanmadı",
                errorMessageEn = "Operation not approved"
            });

            ErrorList.Add(new IyzicoErrorCodeDto
            {
                errorCode = "10012",
                errorGroup = "INVALID_TRANSACTION",
                errorMessageTr = "Geçersiz işlem",
                errorMessageEn = "Invalid transaction"
            });

            ErrorList.Add(new IyzicoErrorCodeDto
            {
                errorCode = "10041",
                errorGroup = "LOST_CARD",
                errorMessageTr = "Kayıp kart, karta el koyunuz",
                errorMessageEn = "Lost card"
            });

            ErrorList.Add(new IyzicoErrorCodeDto
            {
                errorCode = "10043",
                errorGroup = "STOLEN_CARD",
                errorMessageTr = "Çalıntı kart, karta el koyunuz",
                errorMessageEn = "Stolen card"
            });

            ErrorList.Add(new IyzicoErrorCodeDto
            {
                errorCode = "10054",
                errorGroup = "EXPIRED_CARD",
                errorMessageTr = "Vadesi dolmuş kart",
                errorMessageEn = "Expired card"
            });

            ErrorList.Add(new IyzicoErrorCodeDto
            {
                errorCode = "10084",
                errorGroup = "INVALID_CVC2",
                errorMessageTr = "CVC2 bilgisi hatalı",
                errorMessageEn = "Invalid CVC2"
            });

            ErrorList.Add(new IyzicoErrorCodeDto
            {
                errorCode = "10057",
                errorGroup = "NOT_PERMITTED_TO_CARDHOLDER",
                errorMessageTr = "Kart sahibi bu işlemi yapamaz",
                errorMessageEn = "Operation not permitted to card holder"
            });

            ErrorList.Add(new IyzicoErrorCodeDto
            {
                errorCode = "10058",
                errorGroup = "NOT_PERMITTED_TO_TERMINAL",
                errorMessageTr = "Terminalin bu işlemi yapmaya yetkisi yok",
                errorMessageEn = "Operation not permitted to terminal"
            });

            ErrorList.Add(new IyzicoErrorCodeDto
            {
                errorCode = "10034",
                errorGroup = "FRAUD_SUSPECT",
                errorMessageTr = "Dolandırıcılık şüphesi",
                errorMessageEn = "Fraud suspect"
            });

            ErrorList.Add(new IyzicoErrorCodeDto
            {
                errorCode = "10093",
                errorGroup = "RESTRICTED_BY_LAW",
                errorMessageTr = "Kartınız e-ticaret işlemlerine kapalıdır. Bankanızı arayınız.",
                errorMessageEn = "Pickup card"
            });

            ErrorList.Add(new IyzicoErrorCodeDto
            {
                errorCode = "10201",
                errorGroup = "CARD_NOT_PERMITTED",
                errorMessageTr = "Kart, işleme izin vermedi",
                errorMessageEn = "Operation not permitted by card"
            });

            ErrorList.Add(new IyzicoErrorCodeDto
            {
                errorCode = "10204",
                errorGroup = "UNKNOWN",
                errorMessageTr = "Ödeme işlemi esnasında genel bir hata oluştu",
                errorMessageEn = "An error occurred while processing payment"
            });

            ErrorList.Add(new IyzicoErrorCodeDto
            {
                errorCode = "10206",
                errorGroup = "INVALID_CVC2_LENGTH",
                errorMessageTr = "CVC uzunluğu geçersiz",
                errorMessageEn = "Invalid CVC length"
            });

            ErrorList.Add(new IyzicoErrorCodeDto
            {
                errorCode = "10207",
                errorGroup = "REFER_TO_CARD_ISSUER",
                errorMessageTr = "Bankanızdan onay alınız",
                errorMessageEn = "Need to receive approval by bank"
            });

            ErrorList.Add(new IyzicoErrorCodeDto
            {
                errorCode = "10208",
                errorGroup = "INVALID_MERCHANT_OR_SP",
                errorMessageTr = "Üye işyeri kategori kodu hatalı",
                errorMessageEn = "Invalid bank merchant category"
            });

            ErrorList.Add(new IyzicoErrorCodeDto
            {
                errorCode = "10209",
                errorGroup = "BLOCKED_CARD",
                errorMessageTr = "Bloke statülü kart",
                errorMessageEn = "Blocked card"
            });

            ErrorList.Add(new IyzicoErrorCodeDto
            {
                errorCode = "10210",
                errorGroup = "INVALID_CAVV",
                errorMessageTr = "Hatalı CAVV bilgisi",
                errorMessageEn = "Invalid CAVV"
            });

            ErrorList.Add(new IyzicoErrorCodeDto
            {
                errorCode = "10211",
                errorGroup = "INVALID_ECI",
                errorMessageTr = "Hatalı ECI bilgisi",
                errorMessageEn = "Invalid ECI"
            });

            ErrorList.Add(new IyzicoErrorCodeDto
            {
                errorCode = "10213",
                errorGroup = "BIN_NOT_FOUND",
                errorMessageTr = "BIN bulunamadı",
                errorMessageEn = "Bin is not found"
            });

            ErrorList.Add(new IyzicoErrorCodeDto
            {
                errorCode = "10214",
                errorGroup = "COMMUNICATION_OR_SYSTEM_ERROR",
                errorMessageTr = "İletişim veya sistem hatası",
                errorMessageEn = "Communication or system error"
            });

            ErrorList.Add(new IyzicoErrorCodeDto
            {
                errorCode = "10215",
                errorGroup = "INVALID_CARD_NUMBER",
                errorMessageTr = "Geçersiz kart numarası",
                errorMessageEn = "Invalid card number"
            });

            ErrorList.Add(new IyzicoErrorCodeDto
            {
                errorCode = "10216",
                errorGroup = "NO_SUCH_ISSUER",
                errorMessageTr = "Bankası bulunamadı",
                errorMessageEn = "Issuer not found"
            });

            ErrorList.Add(new IyzicoErrorCodeDto
            {
                errorCode = "10217",
                errorGroup = "DEBIT_CARDS_REQUIRES_3DS",
                errorMessageTr = "Banka kartları sadece 3D Secure işleminde kullanılabilir",
                errorMessageEn = "Debit cards require 3D-Secure"
            });

            ErrorList.Add(new IyzicoErrorCodeDto
            {
                errorCode = "10219",
                errorGroup = "REQUEST_TIMEOUT",
                errorMessageTr = "Bankaya gönderilen istek zaman aşımına uğradı",
                errorMessageEn = "Request timeout"
            });

            ErrorList.Add(new IyzicoErrorCodeDto
            {
                errorCode = "10222",
                errorGroup = "NOT_PERMITTED_TO_INSTALLMENT",
                errorMessageTr = "Terminal taksitli işleme kapalı",
                errorMessageEn = "Installment option not allowed for the terminal"
            });

            ErrorList.Add(new IyzicoErrorCodeDto
            {
                errorCode = "10223",
                errorGroup = "REQUIRES_DAY_END",
                errorMessageTr = "Gün sonu yapılmalı",
                errorMessageEn = "Requires day end"
            });

            ErrorList.Add(new IyzicoErrorCodeDto
            {
                errorCode = "10225",
                errorGroup = "RESTRICTED_CARD",
                errorMessageTr = "Kısıtlı kart",
                errorMessageEn = "Restricted card"
            });

            ErrorList.Add(new IyzicoErrorCodeDto
            {
                errorCode = "10226",
                errorGroup = "EXCEEDS_ALLOWABLE_PIN_TRIES",
                errorMessageTr = "İzin verilen PIN giriş sayısı aşılmış",
                errorMessageEn = "Max PIN tries exceeded"
            });

            ErrorList.Add(new IyzicoErrorCodeDto
            {
                errorCode = "10227",
                errorGroup = "INVALID_PIN",
                errorMessageTr = "Geçersiz PIN",
                errorMessageEn = "Invalid PIN"
            });

            ErrorList.Add(new IyzicoErrorCodeDto
            {
                errorCode = "10228",
                errorGroup = "ISSUER_OR_SWITCH_INOPERATIVE",
                errorMessageTr = "Banka veya terminal işlem yapamıyor",
                errorMessageEn = "Bank or terminal is not processing operation"
            });

            ErrorList.Add(new IyzicoErrorCodeDto
            {
                errorCode = "10229",
                errorGroup = "INVALID_EXPIRE_YEAR_MONTH",
                errorMessageTr = "Son kullanma tarihi geçersiz",
                errorMessageEn = "Expiration year or month is invalid"
            });

            ErrorList.Add(new IyzicoErrorCodeDto
            {
                errorCode = "10232",
                errorGroup = "INVALID_AMOUNT",
                errorMessageTr = "Geçersiz tutar",
                errorMessageEn = "Invalid amount"
            });

            ErrorList.Add(new IyzicoErrorCodeDto
            {
                errorCode = "-1",
                errorGroup = "UNKNOWN",
                errorMessageTr = "Ödeme işlemi esnasında genel bir hata oluştu",
                errorMessageEn = "An error occurred while processing payment"
            });
        }
    }
}
