using CDR.Entities.Concrete;
using CDR.Entities.Dtos;
using CDR.Shared.Utilities.Results.Abstract;
using CDR.Shared.Utilities.Results.ComplexTypes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CDR.Services.Abstract
{
    public interface IPostgreSqlService
    {
        Task<IResult> GetConnectDB(User UserDetail);
        Task<IDataResult<CompanyPersonDetailInformationDto>> GetCompanyPersonDetailInformation(User UserDetail, string dn);
        Task<IDataResult<CompanyPersonDetailTopDto>> GetCompanyPersonDetailTop(User UserDetail, string dn, Enums.DashboardFilter _filter);
        Task<IDataResult<CompanyPersonDetailTopSixNumberList>> GetCompanyPersonDetailTopSixConversationNumber(User UserDetail, string dn, Enums.DashboardFilter _filter);
        Task<IDataResult<CompanyPersonDetailTopSixNumberList>> GetCompanyPersonDetailTopSixAnsweredNumber(User UserDetail, string dn, Enums.DashboardFilter _filter);
        Task<IDataResult<CompanyPersonDetailTopSixNumberList>> GetCompanyPersonDetailTopSixMissedNumber(User UserDetail, string dn, Enums.DashboardFilter _filter);
        Task<IDataResult<CompanyPersonDetailTopSixTimesList>> GetCompanyPersonDetailTopSixTimeConversation(User UserDetail, string dn, Enums.DashboardFilter _filter);
        Task<IDataResult<CompanyReportListDto>> GetCompanyPersonDetailReportList(User UserDetail, string no, Enums.CallInfoFilter Type);
        Task<IDataResult<CompanyReportCallDetailDto>> GetCallDetail(User UserDetail, long callId);
        Task<IDataResult<CompanyReportCallInformationListDto>> GetCallInformation(User UserDetail, long callId);
        Task<IResult> ImportCompanyPhonebook(User UserDetail, IList<CompanyPhonebookAddDto> DataList);
        Task<IDataResult<CompanyPhonebookListDto>> GetCompanyPhonebookList(User UserDetail, int skip, int take, string NameSurname, string Email, string Phone);
        Task<IDataResult<CompanyPhonebookUserDto>> GetCompanyPhonebookDetail(User UserDetail, int Id);
        Task<IDataResult<int>> GetCompanyPhonebookCount(User UserDetail, string NameSurname, string Email, string Phone);
        Task<IResult> CreateCompanyPhonebook(User UserDetail, CompanyPhonebookAddDto data);
        Task<IResult> DeleteCompanyPhonebook(User UserDetail, IList<int> Ids);
        Task<IDataResult<CompanyReportListDto>> GetReportList(User UserDetail, int skip, int take, string json,DateTime now);
        Task<IDataResult<long>> GetReportCount(User UserDetail, string json, DateTime now);
    }
}
