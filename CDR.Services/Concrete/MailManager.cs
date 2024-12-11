using CDR.Entities.Concrete;
using CDR.Entities.Dtos;
using CDR.Services.Abstract;
using CDR.Shared.Entities.Concrete;
using CDR.Shared.Utilities.Extensions;
using CDR.Shared.Utilities.Results.Abstract;
using CDR.Shared.Utilities.Results.ComplexTypes;
using CDR.Shared.Utilities.Results.Concrete;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Net.Mail;

namespace CDR.Services.Concrete
{
    public class MailManager : IMailService
    {
        private readonly SmtpSettings _smtpSettings;
        private readonly IStaticService _staticService;
        private readonly IConfiguration _configuration;

        public MailManager(Microsoft.Extensions.Options.IOptions<SmtpSettings> smtpSettings, IStaticService staticService, IConfiguration configuration)
        {
            _smtpSettings = smtpSettings.Value;
            _staticService = staticService;
            _configuration = configuration;
        }
        public IResult Send(EmailSendDto emailSendDto)
        {
            MailMessage message = new MailMessage
            {
                From = new MailAddress(_smtpSettings.SenderEmail, _smtpSettings.SenderName), 
                To = { new MailAddress(emailSendDto.Email) }, 
                Subject = emailSendDto.Subject, 
                IsBodyHtml = true,
                Body = emailSendDto.Message 
            };
            SmtpClient smtpClient = new SmtpClient
            {
                Host = _smtpSettings.Server,
                Port = _smtpSettings.Port,
                EnableSsl = true,
                UseDefaultCredentials = false,
                Credentials = new NetworkCredential(_smtpSettings.Username, _smtpSettings.Password),
                DeliveryMethod = SmtpDeliveryMethod.Network
            };
            smtpClient.Send(message);

            return new Result(ResultStatus.Success, $"E-Postanız başarıyla gönderilmiştir.");
        }
        public IResult SendForgotPasswordEmail(EmailSendDto emailSendDto)
        {
            string FilePath = Path.Combine(System.IO.Directory.GetCurrentDirectory(), "Templates", "cdr-email-sifre-degisikligi.html");
            System.IO.StreamReader str = new System.IO.StreamReader(FilePath);
            string MailText = str.ReadToEnd();
            str.Close();

            MailText = 
                MailText.Replace("[Name]", emailSendDto.Name)
                .Replace("[Link]", emailSendDto.Token)
                .Replace("[EMAIL_Merhaba]", _staticService.GetLocalization("EMAIL_Merhaba").Data)
                .Replace("[EMAIL_SifreDegistirmeIstegiCumle1]", _staticService.GetLocalization("EMAIL_SifreDegistirmeIstegiCumle1").Data)
                .Replace("[Email_LinkText]", emailSendDto.Token)
                .Replace("[EMAIL_SifremiSifirla]", _staticService.GetLocalization("EMAIL_SifremiSifirla").Data)
                .Replace("[EMAIL_SifreDegistirmeNot]", _staticService.GetLocalization("EMAIL_SifreDegistirmeNot").Data)
                .Replace("[EMAIL_SifreDegistirmeNot2]", _staticService.GetLocalization("EMAIL_SifreDegistirmeNot2").Data);

            MailMessage message = new MailMessage
            {
                From = new MailAddress(_smtpSettings.SenderEmail, _smtpSettings.SenderName), 
                To = { new MailAddress(emailSendDto.Email) },
                Subject = emailSendDto.Subject,
                IsBodyHtml = true,
                Body = MailText
            };

            SmtpClient smtpClient = new SmtpClient
            {
                Host = _smtpSettings.Server,
                Port = _smtpSettings.Port,
                EnableSsl = true,
                UseDefaultCredentials = false,
                Credentials = new NetworkCredential(_smtpSettings.Username, _smtpSettings.Password),
                DeliveryMethod = SmtpDeliveryMethod.Network
            };
            smtpClient.Send(message);

            return new Result(ResultStatus.Success, $"E-Postanız başarıyla gönderilmiştir.");
        }
        public IResult SendEmailConfirmEmail(EmailSendDto emailSendDto)
        {
            string FilePath = Path.Combine(System.IO.Directory.GetCurrentDirectory(), "Templates", "cdr-email-onay.html");
            System.IO.StreamReader str = new System.IO.StreamReader(FilePath);
            string MailText = str.ReadToEnd();
            str.Close();

            MailText =
               MailText.Replace("[Name]", emailSendDto.Name)
               .Replace("[Link]", emailSendDto.Token)
               .Replace("[EMAIL_Merhaba]", _staticService.GetLocalization("EMAIL_Merhaba").Data)
               .Replace("[EMAIL_Thanks]", _staticService.GetLocalization("EMAIL_Thanks").Data)
               .Replace("[EMAIL_EpostaDogrulamasiCumle1]", _staticService.GetLocalization("EMAIL_EpostaDogrulamasiCumle1").Data)
               .Replace("[EMAIL_EpostaDogrulamasiCumle2]", _staticService.GetLocalization("EMAIL_EpostaDogrulamasiCumle2").Data)
               .Replace("[Email_LinkText]", emailSendDto.Token)
               .Replace("[EMAIL_HesabimiDogrula]", _staticService.GetLocalization("EMAIL_HesabimiDogrula").Data);

            MailMessage message = new MailMessage
            {
                From = new MailAddress(_smtpSettings.SenderEmail, _smtpSettings.SenderName),
                To = { new MailAddress(emailSendDto.Email) },
                Subject = emailSendDto.Subject,
                IsBodyHtml = true,
                Body = MailText
            };
            SmtpClient smtpClient = new SmtpClient
            {
                Host = _smtpSettings.Server,
                Port = _smtpSettings.Port,
                EnableSsl = true,
                UseDefaultCredentials = false,
                Credentials = new NetworkCredential(_smtpSettings.Username, _smtpSettings.Password),
                DeliveryMethod = SmtpDeliveryMethod.Network
            };
            smtpClient.Send(message);

            return new Result(ResultStatus.Success, $"E-Postanız başarıyla gönderilmiştir.");
        }
        public IResult SendTransactionEmail(EmailSendDto emailSendDto, TransactionPDFModel transactionPDFModel)
        {
            string FilePath = Path.Combine(System.IO.Directory.GetCurrentDirectory(), "Templates", "cdr-transaction-mail.html");
            System.IO.StreamReader str = new System.IO.StreamReader(FilePath);
            string MailText = str.ReadToEnd();
            str.Close();
            
            string invoiceTotal = BaseExtensions.FormatDecimalToString(transactionPDFModel.Total, transactionPDFModel.Currency);

            MailText =
              MailText.Replace("[EMAIL_Greetings]", _staticService.GetLocalization("EMAIL_Greetings").Data)
              .Replace("[CustomerName]", transactionPDFModel.CustomerName)
              .Replace("[EMAIL_PaketinizTanimlandiCumle1]", _staticService.GetLocalization("EMAIL_PaketinizTanimlandiCumle1").Data)
              .Replace("[EMAIL_SatinAlinanUrun]", _staticService.GetLocalization("EMAIL_SatinAlinanUrun").Data)
              .Replace("[ProductName]", transactionPDFModel.ProductName + "(" + transactionPDFModel.Period + " Month)")
              .Replace("[EMAIL_SatinAlmaTarihi]", _staticService.GetLocalization("EMAIL_SatinAlmaTarihi").Data)
              .Replace("[PurchaseDate]", DateTime.Now.ToString("dd/MM/yyyy"))
              .Replace("[EMAIL_FaturaTutari]", _staticService.GetLocalization("EMAIL_FaturaTutari").Data)
              .Replace("[InvoiceTotal]", invoiceTotal + " " + transactionPDFModel.Currency)
              .Replace("[EMAIL_PaketinizTanimlandiCumle2]", _staticService.GetLocalization("EMAIL_PaketinizTanimlandiCumle2").Data)
              .Replace("[EMAIL_Saygilarimizla]", _staticService.GetLocalization("EMAIL_Saygilarimizla").Data)
              .Replace("[EMAIL_CDRCloudEkibi]", _staticService.GetLocalization("EMAIL_CDRCloudEkibi").Data)
              ;

            var htmlTextForPDF = CreateInvoiceHtmlText(transactionPDFModel);

            TransactionMailSenderModel transactionMailSenderModel = new TransactionMailSenderModel
            {
                PdfHtmlText = htmlTextForPDF,
                MailText = MailText,
                EmailReceiverCustomerMail = emailSendDto.Email,
                EmailSubject = emailSendDto.Subject,
                InvoiceNumber = transactionPDFModel.InvoiceNumber
            };

            var response = RequestToGetPDF(transactionMailSenderModel);

            if (!response.IsSuccessStatusCode)
            {
                return new Result(ResultStatus.Error, $"Mail gönderme sırasında bir sorun oluştu.");
            }

            return new Result(ResultStatus.Success, $"E-Postanız başarıyla gönderilmiştir.");
        }
        public IResult SendSupportEmail(EmailSendDto emailSendDto,SupportAddReturnDto supportDto)
        {
            string FilePath = Path.Combine(System.IO.Directory.GetCurrentDirectory(), "Templates", "cdr-support-user.html");
            System.IO.StreamReader str = new System.IO.StreamReader(FilePath);
            string MailText = str.ReadToEnd();
            str.Close();

            MailText =
              MailText.Replace("[Name]", emailSendDto.Name)
              .Replace("[EMAIL_Merhaba]", _staticService.GetLocalization("EMAIL_Merhaba").Data)
              .Replace("[EMAIL_Destek]", _staticService.GetLocalization("EMAIL_DestekBaslik").Data)
              .Replace("[EMAIL_DestekCumle1]", _staticService.GetLocalization("EMAIL_DestekCumle1").Data)
              .Replace("[EMAIL_Mesaj]", _staticService.GetLocalization("EMAIL_Mesaj").Data)
              .Replace("[Message]", emailSendDto.Message)
              .Replace("[EMAIL_Goruntule]", _staticService.GetLocalization("EMAIL_Goruntule").Data)
              .Replace("[Name]", emailSendDto.Name)
              .Replace("[SupportNumber]", supportDto.SupportNumber.ToString())
              .Replace("[Category]", supportDto.CategoryName)
              ;

            MailMessage message = new MailMessage
            {
                From = new MailAddress(_smtpSettings.SenderEmail, _smtpSettings.SenderName),
                To = { new MailAddress(emailSendDto.Email) },
                Subject = emailSendDto.Subject,
                IsBodyHtml = true,
                Body = MailText
            };
            SmtpClient smtpClient = new SmtpClient
            {
                Host = _smtpSettings.Server,
                Port = _smtpSettings.Port,
                EnableSsl = true,
                UseDefaultCredentials = false,
                Credentials = new NetworkCredential(_smtpSettings.Username, _smtpSettings.Password),
                DeliveryMethod = SmtpDeliveryMethod.Network
            };
            smtpClient.Send(message);

            return new Result(ResultStatus.Success, $"E-Postanız başarıyla gönderilmiştir.");
        }
        public IResult SendSalesEmail()
        {
            string FilePath = Path.Combine(System.IO.Directory.GetCurrentDirectory(), "Templates", "cdr-sales-team-mail.html");
            System.IO.StreamReader str = new System.IO.StreamReader(FilePath);
            string MailText = str.ReadToEnd();
            str.Close();

            MailMessage message = new MailMessage
            {
                From = new MailAddress(_smtpSettings.SenderEmail, _smtpSettings.SenderName),
                To = { new MailAddress(_smtpSettings.SalesMailAddress) },
                Subject = "Paket Satıldı",
                IsBodyHtml = true,
                Body = MailText
            };
            SmtpClient smtpClient = new SmtpClient
            {
                Host = _smtpSettings.Server,
                Port = _smtpSettings.Port,
                EnableSsl = true,
                UseDefaultCredentials = false,
                Credentials = new NetworkCredential(_smtpSettings.Username, _smtpSettings.Password),
                DeliveryMethod = SmtpDeliveryMethod.Network
            };
            smtpClient.Send(message);

            return new Result(ResultStatus.Success, $"E-Postanız başarıyla gönderilmiştir.");
        }
        public HttpResponseMessage RequestToGetPDF(TransactionMailSenderModel transactionMailSenderModel)
        {
            HttpClient httpClient = new HttpClient();

            var response = httpClient.PostAsJsonAsync(_smtpSettings.InvoiceUrlToSendTransactionMail, transactionMailSenderModel).Result;

            return response;
        }
        public string CreateInvoiceHtmlText(TransactionPDFModel transactionPDFModel)
        {
            string FilePath = Path.Combine(Directory.GetCurrentDirectory(), "Templates", "cdr-invoice.html");
            StreamReader str = new StreamReader(FilePath);
            string htmlText = str.ReadToEnd();
            str.Close();

            htmlText = htmlText.Replace("@InvoiceNumber", transactionPDFModel.InvoiceNumber)
                .Replace("@IssuedOnDate", DateTime.Now.ToString("dd/MM/yyyy"))
                .Replace("@CustomerName", transactionPDFModel.CustomerName)
                .Replace("@CustomerAddress", transactionPDFModel.CustomerAddress)
                .Replace("@CustomerMail", transactionPDFModel.CustomerMail)
                .Replace("@ProductName", transactionPDFModel.ProductName)
                .Replace("@Quantity", transactionPDFModel.Quantity)
                .Replace("@UnitPrice", BaseExtensions.FormatDecimalToString(transactionPDFModel.UnitPrice,transactionPDFModel.Currency))
                .Replace("@Tax", BaseExtensions.FormatDecimalToString(transactionPDFModel.Tax,transactionPDFModel.Currency))
                .Replace("@Total", BaseExtensions.FormatDecimalToString(transactionPDFModel.Total, transactionPDFModel.Currency))
                .Replace("@Subtotal", BaseExtensions.FormatDecimalToString(transactionPDFModel.SubTotal, transactionPDFModel.Currency))
                .Replace("@Currency", transactionPDFModel.Currency)
                .Replace("@Period", transactionPDFModel.Period + " Month");

            return htmlText;
        }

        public IResult SendForgotPasswordEmailByResendClient(EmailSendDto emailSendDto)
        {
            string FilePath = Path.Combine(System.IO.Directory.GetCurrentDirectory(), "Templates", "cdr-email-sifre-degisikligi.html");
            System.IO.StreamReader str = new System.IO.StreamReader(FilePath);
            string MailText = str.ReadToEnd();
            str.Close();

            MailText =
                MailText.Replace("[Name]", emailSendDto.Name)
                .Replace("[Link]", emailSendDto.Token)
                .Replace("[EMAIL_Merhaba]", _staticService.GetLocalization("EMAIL_Merhaba").Data)
                .Replace("[EMAIL_SifreDegistirmeIstegiCumle1]", _staticService.GetLocalization("EMAIL_SifreDegistirmeIstegiCumle1").Data)
                .Replace("[Email_LinkText]", emailSendDto.Token)
                .Replace("[EMAIL_SifremiSifirla]", _staticService.GetLocalization("EMAIL_SifremiSifirla").Data)
                .Replace("[EMAIL_SifreDegistirmeNot]", _staticService.GetLocalization("EMAIL_SifreDegistirmeNot").Data)
                .Replace("[EMAIL_SifreDegistirmeNot2]", _staticService.GetLocalization("EMAIL_SifreDegistirmeNot2").Data);

            EmailSendRequestDto emailSendRequestDto = new EmailSendRequestDto
            {
                attachments = null,
                bcc = null,
                cc = null,
                from = _smtpSettings.SenderEmail,
                to = new List<string>()
                {
                    emailSendDto.Email
                },
                html = MailText,
                subject = emailSendDto.Subject,
                headers = null,
                react = null,
                reply_to = null,
                tags = null,
                text = null
            };

            using (HttpClient client = new HttpClient())
            {
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _configuration["ResendMailApiSettings:ApiKey"]);
                try
                {
                    HttpResponseMessage httpResponse = client.PostAsJsonAsync(_configuration["ResendMailApiSettings:ApiUrl"], emailSendRequestDto).Result;
                    if (httpResponse.IsSuccessStatusCode)
                    {
                        return new Result(ResultStatus.Success, $"E-Postanız başarıyla gönderilmiştir.");
                    }
                    else
                    {
                        return new Result(ResultStatus.Error, $"Error while sending mail");
                    }
                }
                catch (Exception)
                {
                    return new Result(ResultStatus.Error, $"Error while sending mail");
                    throw;
                }
            }
        }

        public IResult SendEmailConfirmEmailByResendClient(EmailSendDto emailSendDto)
        {
            string FilePath = Path.Combine(System.IO.Directory.GetCurrentDirectory(), "Templates", "cdr-email-onay.html");
            System.IO.StreamReader str = new System.IO.StreamReader(FilePath);
            string MailText = str.ReadToEnd();
            str.Close();

            MailText =
               MailText.Replace("[Name]", emailSendDto.Name)
               .Replace("[Link]", emailSendDto.Token)
               .Replace("[EMAIL_Merhaba]", _staticService.GetLocalization("EMAIL_Merhaba").Data)
               .Replace("[EMAIL_Thanks]", _staticService.GetLocalization("EMAIL_Thanks").Data)
               .Replace("[EMAIL_EpostaDogrulamasiCumle1]", _staticService.GetLocalization("EMAIL_EpostaDogrulamasiCumle1").Data)
               .Replace("[EMAIL_EpostaDogrulamasiCumle2]", _staticService.GetLocalization("EMAIL_EpostaDogrulamasiCumle2").Data)
               .Replace("[Email_LinkText]", emailSendDto.Token)
               .Replace("[EMAIL_HesabimiDogrula]", _staticService.GetLocalization("EMAIL_HesabimiDogrula").Data);

            EmailSendRequestDto emailSendRequestDto = new EmailSendRequestDto
            {
                attachments = null,
                bcc = null,
                cc = null,
                from = _smtpSettings.SenderEmail,
                to = new List<string>()
                {
                    emailSendDto.Email
                },
                html = MailText,
                subject = emailSendDto.Subject,
                headers = null,
                react = null,
                reply_to = null,
                tags = null,
                text = null
            };

            using (HttpClient client = new HttpClient())
            {
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _configuration["ResendMailApiSettings:ApiKey"]);
                try
                {
                    HttpResponseMessage httpResponse = client.PostAsJsonAsync(_configuration["ResendMailApiSettings:ApiUrl"], emailSendRequestDto).Result;
                    if (httpResponse.IsSuccessStatusCode)
                    {
                        return new Result(ResultStatus.Success, $"E-Postanız başarıyla gönderilmiştir.");
                    }
                    else
                    {
                        return new Result(ResultStatus.Error, $"Error while sending mail");
                    }
                }
                catch (Exception)
                {
                    return new Result(ResultStatus.Error, $"Error while sending mail");
                    throw;
                }
            }
        }

        public IResult SendSupportEmailByResendClient(EmailSendDto emailSendDto, SupportAddReturnDto supportDto)
        {
            string FilePath = Path.Combine(System.IO.Directory.GetCurrentDirectory(), "Templates", "cdr-support-user.html");
            System.IO.StreamReader str = new System.IO.StreamReader(FilePath);
            string MailText = str.ReadToEnd();
            str.Close();

            MailText =
              MailText.Replace("[Name]", emailSendDto.Name)
              .Replace("[EMAIL_Merhaba]", _staticService.GetLocalization("EMAIL_Merhaba").Data)
              .Replace("[EMAIL_Destek]", _staticService.GetLocalization("EMAIL_DestekBaslik").Data)
              .Replace("[EMAIL_DestekCumle1]", _staticService.GetLocalization("EMAIL_DestekCumle1").Data)
              .Replace("[EMAIL_Mesaj]", _staticService.GetLocalization("EMAIL_Mesaj").Data)
              .Replace("[Message]", emailSendDto.Message)
              .Replace("[EMAIL_Goruntule]", _staticService.GetLocalization("EMAIL_Goruntule").Data)
              .Replace("[Name]", emailSendDto.Name)
              .Replace("[SupportNumber]", supportDto.SupportNumber.ToString())
              .Replace("[Category]", supportDto.CategoryName)
              ;

            EmailSendRequestDto emailSendRequestDto = new EmailSendRequestDto
            {
                attachments = null,
                bcc = null,
                cc = null,
                from = _smtpSettings.SenderEmail,
                to = new List<string>()
                {
                    emailSendDto.Email
                },
                html = MailText,
                subject = emailSendDto.Subject,
                headers = null,
                react = null,
                reply_to = null,
                tags = null,
                text = null
            };

            using (HttpClient client = new HttpClient())
            {
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _configuration["ResendMailApiSettings:ApiKey"]);
                try
                {
                    HttpResponseMessage httpResponse = client.PostAsJsonAsync(_configuration["ResendMailApiSettings:ApiUrl"], emailSendRequestDto).Result;
                    if (httpResponse.IsSuccessStatusCode)
                    {
                        return new Result(ResultStatus.Success, $"E-Postanız başarıyla gönderilmiştir.");
                    }
                    else
                    {
                        return new Result(ResultStatus.Error, $"Error while sending mail");
                    }
                }
                catch (Exception)
                {
                    return new Result(ResultStatus.Error, $"Error while sending mail");
                    throw;
                }
            }
        }

        public IResult SendSalesEmailByResendClient()
        {
            string FilePath = Path.Combine(System.IO.Directory.GetCurrentDirectory(), "Templates", "cdr-sales-team-mail.html");
            System.IO.StreamReader str = new System.IO.StreamReader(FilePath);
            string MailText = str.ReadToEnd();
            str.Close();

            EmailSendRequestDto emailSendRequestDto = new EmailSendRequestDto
            {
                attachments = null,
                bcc = null,
                cc = null,
                from = _smtpSettings.SenderEmail,
                to = new List<string>()
                {
                    _smtpSettings.SalesMailAddress
                },
                html = MailText,
                subject = "Paket Satıldı",
                headers = null,
                react = null,
                reply_to = null,
                tags = null,
                text = null
            };

            using (HttpClient client = new HttpClient())
            {
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _configuration["ResendMailApiSettings:ApiKey"]);
                try
                {
                    HttpResponseMessage httpResponse = client.PostAsJsonAsync(_configuration["ResendMailApiSettings:ApiUrl"], emailSendRequestDto).Result;
                    if (httpResponse.IsSuccessStatusCode)
                    {
                        return new Result(ResultStatus.Success, $"E-Postanız başarıyla gönderilmiştir.");
                    }
                    else
                    {
                        return new Result(ResultStatus.Error, $"Error while sending mail");
                    }
                }
                catch (Exception)
                {
                    return new Result(ResultStatus.Error, $"Error while sending mail");
                    throw;
                }
            }
        }
    }
}
