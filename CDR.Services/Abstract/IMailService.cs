using CDR.Entities.Dtos;
using CDR.Shared.Utilities.Results.Abstract;
using CDR.Shared.Utilities.Results.Concrete;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CDR.Services.Abstract
{
    public interface IMailService
    {
        IResult Send(EmailSendDto emailSendDto);
        IResult SendForgotPasswordEmail(EmailSendDto emailSendDto);
        IResult SendEmailConfirmEmail(EmailSendDto emailSendDto);
        IResult SendTransactionEmail(EmailSendDto emailSendDto, TransactionPDFModel transactionPDFModel);
        IResult SendSupportEmail(EmailSendDto emailSendDto,SupportAddReturnDto supportDto);
        IResult SendSalesEmail();
        IResult SendForgotPasswordEmailByResendClient(EmailSendDto emailSendDto);
        IResult SendEmailConfirmEmailByResendClient(EmailSendDto emailSendDto);
        IResult SendSupportEmailByResendClient(EmailSendDto emailSendDto, SupportAddReturnDto supportDto);
        IResult SendSalesEmailByResendClient();
    }
}
