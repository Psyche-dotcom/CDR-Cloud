using CDR.Entities.Concrete;
using CDR.Entities.Dtos;
using CDR.Services.Abstract;
using CDR.Shared.Utilities.Results.Abstract;
using CDR.Shared.Utilities.Results.ComplexTypes;
using CDR.Shared.Utilities.Results.Concrete;
using CDR.Shared.Utilities.Extensions;
using CDR.Services.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Log = Serilog.Log;
using Microsoft.Data.SqlClient;

namespace CDR.Services.Concrete
{
    public class PostgreSqlManager : IPostgreSqlService
    {
        private readonly IStaticService _staticService;

        public PostgreSqlManager(IStaticService staticService)
        {
            _staticService = staticService;
        }

        public async Task<IResult> CreateCompanyPhonebook(User UserDetail, CompanyPhonebookAddDto data)
        {
            var list = await GetCompanyPhonebookList(UserDetail, 0, 100000, string.Empty, string.Empty, string.Empty);

            if (data.idphonebook == null || data.idphonebook == 0)
            {
                if (list.Data.Users.Any(x => x.email == data.Email))
                    return new Result(ResultStatus.Error, _staticService.GetLocalization("DBO_EmailKullanilmaktadir").Data);
            }
            else
            {
                if (list.Data.Users.Any(x => x.email == data.Email && x.idphonebook != data.idphonebook))
                    return new Result(ResultStatus.Error, _staticService.GetLocalization("DBO_EmailKullanilmaktadir").Data);
            }

            try
            {
                using (var conn = new Npgsql.NpgsqlConnection(BaseExtensions.NpgsqlConnectionString(UserDetail.IpAddress, UserDetail.Port, UserDetail.DbName, UserDetail.DbUsername, UserDetail.DbPassword)))
                {
                    conn.Open();

                    bool isCreate = (data.idphonebook ?? 0) < 1;
                    string sql = isCreate ? PostgreSqlList.CompanyPhonebook.CreateCompanyPhonebook(data) : PostgreSqlList.CompanyPhonebook.UpdateCompanyPhonebook(data);

                    Npgsql.NpgsqlCommand runSql = new Npgsql.NpgsqlCommand(sql, conn);

                    runSql.Prepare();
                    var drb = runSql.ExecuteReader();
                    while (drb.Read())
                    {
                    }

                    conn.Close();
                    conn.Dispose();

                    return new Result(ResultStatus.Success, _staticService.GetLocalization(isCreate ? "DBO_KisiEklendi" : "DBO_KisiDuzenlendi").Data);
                }
            }
            catch (Exception ex)
            {
                Log.Error(ex, "PostgreSqlManager -> CreateCompanyPhonebook");
            }

            return new Result(ResultStatus.Error, _staticService.GetLocalization("DBO_BirHataOlustu").Data);
        }

        public async Task<IResult> DeleteCompanyPhonebook(User UserDetail, IList<int> Ids)
        {
            if (Ids.Count == 0)
                return new Result(ResultStatus.Error, _staticService.GetLocalization("CDR_EnAz1VeriSeciniz").Data);

            try
            {
                using (var conn = new Npgsql.NpgsqlConnection(BaseExtensions.NpgsqlConnectionString(UserDetail.IpAddress, UserDetail.Port, UserDetail.DbName, UserDetail.DbUsername, UserDetail.DbPassword)))
                {
                    foreach (var item in Ids)
                    {
                        conn.Open();

                        string sql = PostgreSqlList.CompanyPhonebook.DeleteCompanyPhonebook(item);

                        Npgsql.NpgsqlCommand runSql = new Npgsql.NpgsqlCommand(sql, conn);

                        runSql.Prepare();
                        var drb = runSql.ExecuteReader();
                        while (drb.Read())
                        {
                        }

                        conn.Close();
                        conn.Dispose();
                    }

                    return new Result(ResultStatus.Success, _staticService.GetLocalization("DBO_SeciliVerilerSilindi").Data);
                }
            }
            catch (Exception ex)
            {
                Log.Error(ex, "PostgreSqlManager -> DeleteCompanyPhonebook");
            }

            return new Result(ResultStatus.Error, _staticService.GetLocalization("DBO_BirHataOlustu").Data);
        }

        public async Task<IDataResult<CompanyPhonebookUserDto>> GetCompanyPhonebookDetail(User UserDetail, int Id)
        {
            var data = new CompanyPhonebookUserDto();

            try
            {
                using (var conn = new Npgsql.NpgsqlConnection(BaseExtensions.NpgsqlConnectionString(UserDetail.IpAddress, UserDetail.Port, UserDetail.DbName, UserDetail.DbUsername, UserDetail.DbPassword)))
                {
                    string sql = PostgreSqlList.CompanyPhonebook.CompanyPhonebookDetail(Id);

                    using (var cmd = new Npgsql.NpgsqlCommand(sql, conn))
                    {
                        conn.Open();
                        cmd.Prepare();
                        var sqlExecute = cmd.ExecuteReader();

                        while (sqlExecute.Read())
                        {
                            var values = new object[sqlExecute.FieldCount];
                            for (int i = 0; i < sqlExecute.FieldCount; i++)
                            {
                                values[i] = sqlExecute[i];
                            }

                            data.idphonebook = Convert.ToInt32(values[0].ToString());
                            data.firstname = values[1].ToString();
                            data.lastname = values[2].ToString();
                            data.phonenumber = values[3].ToString();
                            data.company = values[4].ToString();
                            data.email = values[5].ToString();
                        }
                        conn.Close();
                        conn.Dispose();
                    }

                    return new DataResult<CompanyPhonebookUserDto>(ResultStatus.Success, data);
                }
            }
            catch (Exception ex)
            { 
                Log.Error(ex, "PostgreSqlManager -> GetCompanyPhonebookDetail"); 
            }

            return new DataResult<CompanyPhonebookUserDto>(ResultStatus.Error, data);
        }

        public async Task<IDataResult<CompanyPhonebookListDto>> GetCompanyPhonebookList(User UserDetail, int skip, int take, string NameSurname, string Email, string Phone)
        {
            var data = new List<CompanyPhonebookUserDto>();

            try
            {
                using (var conn = new Npgsql.NpgsqlConnection(BaseExtensions.NpgsqlConnectionString(UserDetail.IpAddress, UserDetail.Port, UserDetail.DbName, UserDetail.DbUsername, UserDetail.DbPassword)))
                {
                    string sql = PostgreSqlList.CompanyPhonebook.CompanyPhonebookList(skip, take, NameSurname, Email, Phone);

                    using (var cmd = new Npgsql.NpgsqlCommand(sql, conn))
                    {
                        conn.Open();
                        cmd.Prepare();
                        var sqlExecute = cmd.ExecuteReader();

                        while (sqlExecute.Read())
                        {
                            var values = new object[sqlExecute.FieldCount];
                            for (int i = 0; i < sqlExecute.FieldCount; i++)
                            {
                                values[i] = sqlExecute[i];
                            }

                            data.Add(new CompanyPhonebookUserDto
                            {
                                idphonebook = Convert.ToInt32(values[0].ToString()),
                                firstname = values[1].ToString(),
                                lastname = values[2].ToString(),
                                phonenumber = values[3].ToString(),
                                company = values[4].ToString(),
                                email = values[5].ToString()
                            });
                        }
                        conn.Close();
                        conn.Dispose();
                    }
                }

                return new DataResult<CompanyPhonebookListDto>(ResultStatus.Success, new CompanyPhonebookListDto
                {
                    Users = data
                });
            }
            catch (Exception ex) { Log.Error(ex, "PostgreSqlManager -> GetCompanyPhonebookList"); }

            return new DataResult<CompanyPhonebookListDto>(ResultStatus.Error, new CompanyPhonebookListDto
            {
                Users = data
            });
        }

        public async Task<IDataResult<int>> GetCompanyPhonebookCount(User UserDetail, string NameSurname, string Email, string Phone)
        {
            int count = 0;

            try
            {
                using (var conn = new Npgsql.NpgsqlConnection(BaseExtensions.NpgsqlConnectionString(UserDetail.IpAddress, UserDetail.Port, UserDetail.DbName, UserDetail.DbUsername, UserDetail.DbPassword)))
                {
                    string sql = PostgreSqlList.CompanyPhonebook.CompanyPhonebookCount(NameSurname, Email, Phone);

                    using (var cmd = new Npgsql.NpgsqlCommand(sql, conn))
                    {
                        conn.Open();
                        cmd.Prepare();
                        var sqlExecute = cmd.ExecuteReader();

                        while (sqlExecute.Read())
                        {
                            var values = new object[sqlExecute.FieldCount];
                            for (int i = 0; i < sqlExecute.FieldCount; i++)
                            {
                                values[i] = sqlExecute[i];
                            }

                            count = Convert.ToInt32(values[0].ToString());
                        }
                        conn.Close();
                        conn.Dispose();
                    }
                }

                return new DataResult<int>(ResultStatus.Success, count);
            }
            catch (Exception ex) { Log.Error(ex, "PostgreSqlManager -> GetCompanyPhonebookCount"); }

            return new DataResult<int>(ResultStatus.Error, 0);
        }

        public async Task<IResult> ImportCompanyPhonebook(User UserDetail, IList<CompanyPhonebookAddDto> DataList)
        {
            var list = await GetCompanyPhonebookList(UserDetail, 0, 100000, string.Empty, string.Empty, string.Empty);

            var addList = new List<CompanyPhonebookAddDto>();
            var updateList = new List<CompanyPhonebookAddDto>();

            try
            {
                foreach (var item in DataList)
                {
                    if (string.IsNullOrWhiteSpace(item.FirstName) || string.IsNullOrWhiteSpace(item.MobileNumber))
                        continue;

                    var _isThere = list.Data.Users.Where(x => x.phonenumber == item.MobileNumber).FirstOrDefault();

                    if (_isThere == null)
                    {
                        addList.Add(item);
                    }
                    else
                    {
                        if (_isThere.firstname == item.FirstName &&
                            _isThere.lastname == item.LastName &&
                            _isThere.email == item.Email &&
                            _isThere.phonenumber == item.MobileNumber &&
                            _isThere.company == item.Company
                            )
                            continue;

                        updateList.Add(new CompanyPhonebookAddDto
                        {
                            idphonebook = _isThere.idphonebook,
                            MobileNumber = item.MobileNumber,
                            Company = item.Company,
                            Email = item.Email,
                            FirstName = item.FirstName,
                            LastName = item.LastName
                        });
                    }
                }

                using (var conn = new Npgsql.NpgsqlConnection(BaseExtensions.NpgsqlConnectionString(UserDetail.IpAddress, UserDetail.Port, UserDetail.DbName, UserDetail.DbUsername, UserDetail.DbPassword)))
                {
                    foreach (var item in addList)
                    {
                        conn.Open();

                        string sql = PostgreSqlList.CompanyPhonebook.CreateCompanyPhonebook(item);

                        Npgsql.NpgsqlCommand runSql = new Npgsql.NpgsqlCommand(sql, conn);

                        runSql.Prepare();
                        var drb = runSql.ExecuteReader();
                        while (drb.Read())
                        {
                        }

                        conn.Close();
                        conn.Dispose();
                    }
                }

                using (var conn = new Npgsql.NpgsqlConnection(BaseExtensions.NpgsqlConnectionString(UserDetail.IpAddress, UserDetail.Port, UserDetail.DbName, UserDetail.DbUsername, UserDetail.DbPassword)))
                {
                    foreach (var item in updateList)
                    {
                        conn.Open();

                        string sql = PostgreSqlList.CompanyPhonebook.UpdateCompanyPhonebook(item);

                        Npgsql.NpgsqlCommand runSql = new Npgsql.NpgsqlCommand(sql, conn);

                        runSql.Prepare();
                        var drb = runSql.ExecuteReader();
                        while (drb.Read())
                        {
                        }

                        conn.Close();
                        conn.Dispose();
                    }
                }

                return new Result(ResultStatus.Success, _staticService.GetLocalization("CDR_ExcelDuzenlemesiTamamlandi").Data);
            }
            catch (Exception ex) { Log.Error(ex, "PostgreSqlManager -> ImportCompanyPhonebook"); }

            return new Result(ResultStatus.Error, _staticService.GetLocalization("DBO_BirHataOlustu").Data);
        }

        public async Task<IDataResult<CompanyReportCallDetailDto>> GetCallDetail(User UserDetail, long callId)
        {
            var data = new CompanyReportCallDetailDto();

            try
            {
                using (var conn = new Npgsql.NpgsqlConnection(BaseExtensions.NpgsqlConnectionString(UserDetail.IpAddress, UserDetail.Port, UserDetail.DbName, UserDetail.DbUsername, UserDetail.DbPassword)))
                {
                    string sql = PostgreSqlList.Call.CallDetail(callId);

                    using (var cmd = new Npgsql.NpgsqlCommand(sql, conn))
                    {
                        conn.Open();
                        cmd.Prepare();
                        var sqlExecute = cmd.ExecuteReader();

                        while (sqlExecute.Read())
                        {
                            var values = new object[sqlExecute.FieldCount];
                            for (int i = 0; i < sqlExecute.FieldCount; i++)
                            {
                                values[i] = sqlExecute[i];
                            }

                            data.call_id = Convert.ToInt32(values[0].ToString());
                            data.from = values[1].ToString();
                            data.to = values[2].ToString();
                            data.duration = !string.IsNullOrWhiteSpace(values[3].ToString()) ? TimeSpan.Parse(values[3].ToString()) : TimeSpan.Parse("00:00:00");
                            data.talktime = !string.IsNullOrWhiteSpace(values[4].ToString()) ? TimeSpan.Parse(values[4].ToString()) : TimeSpan.Parse("00:00:00");
                            data.ringtime = !string.IsNullOrWhiteSpace(values[5].ToString()) ? TimeSpan.Parse(values[5].ToString()) : TimeSpan.Parse("00:00:00");
                            data.year = Convert.ToInt32(values[6].ToString());
                            data.month = Convert.ToInt32(values[7].ToString());
                            data.day = Convert.ToInt32(values[8].ToString());
                            data.starttime = !string.IsNullOrWhiteSpace(values[9].ToString()) ? TimeSpan.Parse(values[9].ToString()) : TimeSpan.Parse("00:00:00");
                            data.stoptime = !string.IsNullOrWhiteSpace(values[10].ToString()) ? TimeSpan.Parse(values[10].ToString()) : TimeSpan.Parse("00:00:00");
                            data.inorout = values[11].ToString();
                            data.status = values[12].ToString();
                            data.AudioUrl = values[13].ToString();
                        }
                        conn.Close();
                        conn.Dispose();
                    }

                    return new DataResult<CompanyReportCallDetailDto>(ResultStatus.Success, data);
                }
            }
            catch (Exception ex) { Log.Error(ex, "PostgreSqlManager -> GetCallDetail"); }

            return new DataResult<CompanyReportCallDetailDto>(ResultStatus.Error, data);
        }

        public async Task<IDataResult<CompanyReportCallInformationListDto>> GetCallInformation(User UserDetail, long callId)
        {
            var data = new List<CompanyReportCallInformationDto>();

            try
            {
                using (var conn = new Npgsql.NpgsqlConnection(BaseExtensions.NpgsqlConnectionString(UserDetail.IpAddress, UserDetail.Port, UserDetail.DbName, UserDetail.DbUsername, UserDetail.DbPassword)))
                {
                    string sql = PostgreSqlList.Call.CallInformation(callId);

                    using (var cmd = new Npgsql.NpgsqlCommand(sql, conn))
                    {
                        conn.Open();
                        cmd.Prepare();
                        var sqlExecute = cmd.ExecuteReader();

                        while (sqlExecute.Read())
                        {
                            var values = new object[sqlExecute.FieldCount];
                            for (int i = 0; i < sqlExecute.FieldCount; i++)
                            {
                                values[i] = sqlExecute[i];
                            }

                            data.Add(new CompanyReportCallInformationDto
                            {
                                call_id = Convert.ToInt32(values[0].ToString()),
                                start_time = TimeSpan.Parse(values[1].ToString()),
                                end_time = TimeSpan.Parse(values[2].ToString()),
                                ringortalktime = TimeSpan.Parse(values[3].ToString()),
                                durum = values[4].ToString(),
                                act = Convert.ToInt32(values[5].ToString())
                            });
                        }
                        conn.Close();
                        conn.Dispose();
                    }
                }

                return new DataResult<CompanyReportCallInformationListDto>(ResultStatus.Success, new CompanyReportCallInformationListDto
                {
                    DataList = data
                });
            }
            catch (Exception ex) { Log.Error(ex, "PostgreSqlManager -> GetCallInformation"); }

            return new DataResult<CompanyReportCallInformationListDto>(ResultStatus.Error, new CompanyReportCallInformationListDto
            {
                DataList = data
            });
        }

        public async Task<IDataResult<CompanyPersonDetailInformationDto>> GetCompanyPersonDetailInformation(User UserDetail, string dn)
        {
            var data = new CompanyPersonDetailInformationDto();

            try
            {
                using (var conn = new Npgsql.NpgsqlConnection(BaseExtensions.NpgsqlConnectionString(UserDetail.IpAddress, UserDetail.Port, UserDetail.DbName, UserDetail.DbUsername, UserDetail.DbPassword)))
                {
                    string sql = PostgreSqlList.CompanyUserDetail.Detail(dn);

                    using (var cmd = new Npgsql.NpgsqlCommand(sql, conn))
                    {
                        conn.Open();
                        cmd.Prepare();
                        var sqlExecute = cmd.ExecuteReader();

                        while (sqlExecute.Read())
                        {
                            var values = new object[sqlExecute.FieldCount];
                            for (int i = 0; i < sqlExecute.FieldCount; i++)
                            {
                                values[i] = sqlExecute[i];
                            }

                            var _temp = values[0].ToString();

                            data.dn = _temp.Split(" ")[0];
                            data.display_name = _temp.Substring(_temp.IndexOf(' ') + 1);
                        }
                        conn.Close();
                        conn.Dispose();
                    }

                    return new DataResult<CompanyPersonDetailInformationDto>(ResultStatus.Success, data);
                }
            }
            catch (Exception ex) { Log.Error(ex, "PostgreSqlManager -> GetCompanyPersonDetailInformation"); }

            return new DataResult<CompanyPersonDetailInformationDto>(ResultStatus.Error, data);
        }

        public async Task<IDataResult<CompanyReportListDto>> GetCompanyPersonDetailReportList(User UserDetail, string no, Enums.CallInfoFilter Type)
        {
            var data = new List<CompanyReportDto>();

            try
            {
                using (var conn = new Npgsql.NpgsqlConnection(BaseExtensions.NpgsqlConnectionString(UserDetail.IpAddress, UserDetail.Port, UserDetail.DbName, UserDetail.DbUsername, UserDetail.DbPassword)))
                {
                    string sql = PostgreSqlList.CompanyUserDetail.CallInfo(no, Type);

                    using (var cmd = new Npgsql.NpgsqlCommand(sql, conn))
                    {
                        conn.Open();
                        cmd.Prepare();
                        var sqlExecute = cmd.ExecuteReader();

                        while (sqlExecute.Read())
                        {
                            var values = new object[sqlExecute.FieldCount];
                            for (int i = 0; i < sqlExecute.FieldCount; i++)
                            {
                                values[i] = sqlExecute[i];
                            }

                            data.Add(new CompanyReportDto
                            {
                                call_id = Convert.ToInt32(values[0].ToString()),
                                from = values[1].ToString(),
                                to = values[2].ToString(),
                                duration = TimeSpan.Parse(values[3].ToString() != "" ? values[3].ToString() : "00:00:00"),
                                talktime = TimeSpan.Parse(values[4].ToString() != "" ? values[4].ToString() : "00:00:00"),
                                ringtime = TimeSpan.Parse(values[5].ToString() != "" ? values[5].ToString() : "00:00:00"),
                                year = Convert.ToInt32(values[6].ToString()),
                                month = Convert.ToInt32(values[7].ToString()),
                                day = Convert.ToInt32(values[8].ToString()),
                                starttime = TimeSpan.Parse(values[9].ToString() != "" ? values[9].ToString() : "00:00:00"),
                                stoptime = TimeSpan.Parse(values[10].ToString() != "" ? values[10].ToString() : "00:00:00"),
                                inorout = values[11].ToString(),
                                status = values[12].ToString()
                            });
                        }
                        conn.Close();
                        conn.Dispose();
                    }
                }

                return new DataResult<CompanyReportListDto>(ResultStatus.Success, new CompanyReportListDto
                {
                    DataList = data
                });
            }
            catch (Exception ex) { Log.Error(ex, "PostgreSqlManager -> GetCompanyPersonDetailReportList"); }

            return new DataResult<CompanyReportListDto>(ResultStatus.Success, new CompanyReportListDto());
        }

        public async Task<IDataResult<CompanyPersonDetailTopDto>> GetCompanyPersonDetailTop(User UserDetail, string dn, Enums.DashboardFilter _filter)
        {
            var data = new CompanyPersonDetailTopDto();

            try
            {
                var filterDate = BaseExtensions.GetDateFilter(_filter);

                using (var conn = new Npgsql.NpgsqlConnection(BaseExtensions.NpgsqlConnectionString(UserDetail.IpAddress, UserDetail.Port, UserDetail.DbName, UserDetail.DbUsername, UserDetail.DbPassword)))
                {
                    string sql = PostgreSqlList.CompanyUserDetail.Top(dn, filterDate.StartDateString, filterDate.EndDateString);

                    using (var cmd = new Npgsql.NpgsqlCommand(sql, conn))
                    {
                        conn.Open();
                        cmd.Prepare();
                        var sqlExecute = cmd.ExecuteReader();

                        while (sqlExecute.Read())
                        {
                            var values = new object[sqlExecute.FieldCount];
                            for (int i = 0; i < sqlExecute.FieldCount; i++)
                            {
                                values[i] = sqlExecute[i];
                            }

                            data.numofcalls = Convert.ToInt32(!string.IsNullOrWhiteSpace(values[0].ToString()) ? values[0].ToString() : "0");
                            data.totaltalktime = TimeSpan.Parse(!string.IsNullOrWhiteSpace(values[1].ToString()) ? values[1].ToString() : "00:00:00");
                            data.numofanswered = Convert.ToInt32(!string.IsNullOrWhiteSpace(values[2].ToString()) ? values[2].ToString() : "0");
                            data.numofmissed = Convert.ToInt32(!string.IsNullOrWhiteSpace(values[3].ToString()) ? values[3].ToString() : "0");
                        }
                        conn.Close();
                        conn.Dispose();
                    }
                }

                return new DataResult<CompanyPersonDetailTopDto>(ResultStatus.Success, data);
            }
            catch (Exception ex) { Log.Error(ex, "PostgreSqlManager -> GetCompanyPersonDetailTop"); }

            return new DataResult<CompanyPersonDetailTopDto>(ResultStatus.Error, null);
        }

        public async Task<IDataResult<CompanyPersonDetailTopSixNumberList>> GetCompanyPersonDetailTopSixAnsweredNumber(User UserDetail, string dn, Enums.DashboardFilter _filter)
        {
            var data = new List<CompanyPersonDetailTopSixNumberDto>();

            try
            {
                var filterDate = BaseExtensions.GetDateFilter(_filter);

                using (var conn = new Npgsql.NpgsqlConnection(BaseExtensions.NpgsqlConnectionString(UserDetail.IpAddress, UserDetail.Port, UserDetail.DbName, UserDetail.DbUsername, UserDetail.DbPassword)))
                {
                    string sql = PostgreSqlList.CompanyUserDetail.NumberAnsweredTopSix(dn, filterDate.StartDateString, filterDate.EndDateString);

                    using (var cmd = new Npgsql.NpgsqlCommand(sql, conn))
                    {
                        conn.Open();
                        cmd.Prepare();
                        var sqlExecute = cmd.ExecuteReader();

                        while (sqlExecute.Read())
                        {
                            var values = new object[sqlExecute.FieldCount];
                            for (int i = 0; i < sqlExecute.FieldCount; i++)
                            {
                                values[i] = sqlExecute[i];
                            }

                            data.Add(new CompanyPersonDetailTopSixNumberDto
                            {
                                phonenumber = values[0].ToString(),
                                displayname = values[1].ToString(),
                                numofcalls = Convert.ToInt64(values[2].ToString())
                            });
                        }
                        conn.Close();
                        conn.Dispose();
                    }

                    return new DataResult<CompanyPersonDetailTopSixNumberList>(ResultStatus.Success, new CompanyPersonDetailTopSixNumberList
                    {
                        DataList = data
                    });
                }
            }
            catch (Exception ex) { Log.Error(ex, "PostgreSqlManager -> GetCompanyPersonDetailTopSixAnsweredNumber"); }

            return new DataResult<CompanyPersonDetailTopSixNumberList>(ResultStatus.Error, new CompanyPersonDetailTopSixNumberList
            {
                DataList = data
            });
        }

        public async Task<IDataResult<CompanyPersonDetailTopSixNumberList>> GetCompanyPersonDetailTopSixConversationNumber(User UserDetail, string dn, Enums.DashboardFilter _filter)
        {
            var data = new List<CompanyPersonDetailTopSixNumberDto>();

            try
            {
                var filterDate = BaseExtensions.GetDateFilter(_filter);

                using (var conn = new Npgsql.NpgsqlConnection(BaseExtensions.NpgsqlConnectionString(UserDetail.IpAddress, UserDetail.Port, UserDetail.DbName, UserDetail.DbUsername, UserDetail.DbPassword)))
                {
                    string sql = PostgreSqlList.CompanyUserDetail.NumberConversationTopSix(dn, filterDate.StartDateString, filterDate.EndDateString);

                    using (var cmd = new Npgsql.NpgsqlCommand(sql, conn))
                    {
                        conn.Open();
                        cmd.Prepare();
                        var sqlExecute = cmd.ExecuteReader();

                        while (sqlExecute.Read())
                        {
                            var values = new object[sqlExecute.FieldCount];
                            for (int i = 0; i < sqlExecute.FieldCount; i++)
                            {
                                values[i] = sqlExecute[i];
                            }

                            data.Add(new CompanyPersonDetailTopSixNumberDto
                            {
                                phonenumber = values[0].ToString(),
                                displayname = values[1].ToString(),
                                numofcalls = Convert.ToInt64(values[2].ToString())
                            });
                        }
                        conn.Close();
                        conn.Dispose();
                    }

                    return new DataResult<CompanyPersonDetailTopSixNumberList>(ResultStatus.Success, new CompanyPersonDetailTopSixNumberList
                    {
                        DataList = data
                    });
                }
            }
            catch (Exception ex) { Log.Error(ex, "PostgreSqlManager -> GetCompanyPersonDetailTopSixConversationNumber"); }

            return new DataResult<CompanyPersonDetailTopSixNumberList>(ResultStatus.Error, new CompanyPersonDetailTopSixNumberList
            {
                DataList = data
            });
        }

        public async Task<IDataResult<CompanyPersonDetailTopSixNumberList>> GetCompanyPersonDetailTopSixMissedNumber(User UserDetail, string dn, Enums.DashboardFilter _filter)
        {
            var data = new List<CompanyPersonDetailTopSixNumberDto>();

            try
            {
                var filterDate = BaseExtensions.GetDateFilter(_filter);

                using (var conn = new Npgsql.NpgsqlConnection(BaseExtensions.NpgsqlConnectionString(UserDetail.IpAddress, UserDetail.Port, UserDetail.DbName, UserDetail.DbUsername, UserDetail.DbPassword)))
                {
                    string sql = PostgreSqlList.CompanyUserDetail.NumberMissedTopSix(dn, filterDate.StartDateString, filterDate.EndDateString);

                    using (var cmd = new Npgsql.NpgsqlCommand(sql, conn))
                    {
                        conn.Open();
                        cmd.Prepare();
                        var sqlExecute = cmd.ExecuteReader();

                        while (sqlExecute.Read())
                        {
                            var values = new object[sqlExecute.FieldCount];
                            for (int i = 0; i < sqlExecute.FieldCount; i++)
                            {
                                values[i] = sqlExecute[i];
                            }

                            data.Add(new CompanyPersonDetailTopSixNumberDto
                            {
                                phonenumber = values[0].ToString(),
                                displayname = values[1].ToString(),
                                numofcalls = Convert.ToInt64(values[2].ToString())
                            });
                        }
                        conn.Close();
                        conn.Dispose();
                    }

                    return new DataResult<CompanyPersonDetailTopSixNumberList>(ResultStatus.Success, new CompanyPersonDetailTopSixNumberList
                    {
                        DataList = data
                    });
                }
            }
            catch (Exception ex) { Log.Error(ex, "PostgreSqlManager -> GetCompanyPersonDetailTopSixMissedNumber"); }

            return new DataResult<CompanyPersonDetailTopSixNumberList>(ResultStatus.Error, new CompanyPersonDetailTopSixNumberList
            {
                DataList = data
            });
        }

        public async Task<IDataResult<CompanyPersonDetailTopSixTimesList>> GetCompanyPersonDetailTopSixTimeConversation(User UserDetail, string dn, Enums.DashboardFilter _filter)
        {
            var data = new List<CompanyPersonDetailTopSixTimesDto>();

            try
            {
                var filterDate = BaseExtensions.GetDateFilter(_filter);

                using (var conn = new Npgsql.NpgsqlConnection(BaseExtensions.NpgsqlConnectionString(UserDetail.IpAddress, UserDetail.Port, UserDetail.DbName, UserDetail.DbUsername, UserDetail.DbPassword)))
                {
                    string sql = PostgreSqlList.CompanyUserDetail.TimeConversationTopSix(dn, filterDate.StartDateString, filterDate.EndDateString);

                    using (var cmd = new Npgsql.NpgsqlCommand(sql, conn))
                    {
                        conn.Open();
                        cmd.Prepare();
                        var sqlExecute = cmd.ExecuteReader();

                        while (sqlExecute.Read())
                        {
                            var values = new object[sqlExecute.FieldCount];
                            for (int i = 0; i < sqlExecute.FieldCount; i++)
                            {
                                values[i] = sqlExecute[i];
                            }

                            data.Add(new CompanyPersonDetailTopSixTimesDto
                            {
                                phonenumber = values[0].ToString(),
                                displayname = values[1].ToString(),
                                talkduration = values[2].ToString() != "" ? TimeSpan.Parse(values[2].ToString()) : TimeSpan.MinValue
                            });
                        }
                        conn.Close();
                        conn.Dispose();
                    }

                    return new DataResult<CompanyPersonDetailTopSixTimesList>(ResultStatus.Success, new CompanyPersonDetailTopSixTimesList
                    {
                        DataList = data
                    });
                }
            }
            catch (Exception ex) { Log.Error(ex, "PostgreSqlManager -> GetCompanyPersonDetailTopSixTimeConversation"); }

            return new DataResult<CompanyPersonDetailTopSixTimesList>(ResultStatus.Error, new CompanyPersonDetailTopSixTimesList
            {
                DataList = data
            });
        }

        public async Task<IResult> GetConnectDB(User UserDetail)
        {
            try
            {
                using (var conn = new Npgsql.NpgsqlConnection(BaseExtensions.NpgsqlConnectionString(UserDetail.IpAddress, UserDetail.Port, UserDetail.DbName, UserDetail.DbUsername, UserDetail.DbPassword)))
                {
                    conn.Open();

                    Npgsql.NpgsqlCommand runSql = new Npgsql.NpgsqlCommand("select 1", conn);
                    runSql.ExecuteScalar();

                    conn.Close();
                }

                return new Result(ResultStatus.Success, "");
            }
            catch (Exception ex) { Log.Error(ex, "PostgreSqlManager -> GetConnectDB"); }

            return new Result(ResultStatus.Error, "");
        }

        public async Task<IDataResult<long>> GetReportCount(User UserDetail, string json, DateTime now)
        {
            long data = 0;

            try
            {
                var filter = Newtonsoft.Json.JsonConvert.DeserializeObject<CompanyReportFilterDto>(json);

                using (var conn = new Npgsql.NpgsqlConnection(BaseExtensions.NpgsqlConnectionString(UserDetail.IpAddress, UserDetail.Port, UserDetail.DbName, UserDetail.DbUsername, UserDetail.DbPassword)))
                {
                    string sql = PostgreSqlList.Report.ReportCallsCount(filter, UserDetail.GMT, now);

                    using (var cmd = new Npgsql.NpgsqlCommand(sql, conn))
                    {
                        conn.Open();
                        cmd.Prepare();
                        var sqlExecute = cmd.ExecuteReader();

                        while (sqlExecute.Read())
                        {
                            var values = new object[sqlExecute.FieldCount];
                            for (int i = 0; i < sqlExecute.FieldCount; i++)
                            {
                                values[i] = sqlExecute[i];
                            }

                            data = Convert.ToInt64(values[0].ToString());
                        }
                        conn.Close();
                        conn.Dispose();
                    }
                }

                return new DataResult<long>(ResultStatus.Success, data);
            }
            catch (Exception ex) { Log.Error(ex, "PostgreSqlManager -> GetReportCount"); }

            return new DataResult<long>(ResultStatus.Error, data);
        }

        public async Task<IDataResult<CompanyReportListDto>> GetReportList(User UserDetail, int skip, int take, string json, DateTime now)
        {
            var data = new List<CompanyReportDto>();

            try
            {
                var filter = Newtonsoft.Json.JsonConvert.DeserializeObject<CompanyReportFilterDto>(json);

                using (var conn = new Npgsql.NpgsqlConnection(BaseExtensions.NpgsqlConnectionString(UserDetail.IpAddress, UserDetail.Port, UserDetail.DbName, UserDetail.DbUsername, UserDetail.DbPassword)))
                {
                    string sql = PostgreSqlList.Report.ReportCalls(skip, take, filter, UserDetail.GMT, now);

                    using (var cmd = new Npgsql.NpgsqlCommand(sql, conn))
                    {
                        conn.Open();
                        cmd.Prepare();
                        var sqlExecute = cmd.ExecuteReader();

                        while (sqlExecute.Read())
                        {
                            var values = new object[sqlExecute.FieldCount];
                            for (int i = 0; i < sqlExecute.FieldCount; i++)
                            {
                                values[i] = sqlExecute[i];
                            }

                            data.Add(new CompanyReportDto
                            {
                                call_id = Convert.ToInt32(values[0].ToString()),
                                from = values[1].ToString(),
                                to = values[2].ToString(),
                                duration = !string.IsNullOrWhiteSpace(values[3].ToString()) ? TimeSpan.Parse(values[3].ToString() != "" ? values[3].ToString() : "00:00:00") : TimeSpan.Parse("00:00:00"),
                                talktime = !string.IsNullOrWhiteSpace(values[4].ToString()) ? TimeSpan.Parse(values[4].ToString() != "" ? values[4].ToString() : "00:00:00") : TimeSpan.Parse("00:00:00"),
                                ringtime = !string.IsNullOrWhiteSpace(values[5].ToString()) ? TimeSpan.Parse(values[5].ToString() != "" ? values[5].ToString() : "00:00:00") : TimeSpan.Parse("00:00:00"),
                                year = Convert.ToInt32(values[6].ToString()),
                                month = Convert.ToInt32(values[7].ToString()),
                                day = Convert.ToInt32(values[8].ToString()),
                                starttime = !string.IsNullOrWhiteSpace(values[9].ToString()) ? TimeSpan.Parse(values[9].ToString() != "" ? values[9].ToString() : "00:00:00") : TimeSpan.Parse("00:00:00"),
                                stoptime = !string.IsNullOrWhiteSpace(values[10].ToString()) ? TimeSpan.Parse(values[10].ToString() != "" ? values[10].ToString() : "00:00:00") : TimeSpan.Parse("00:00:00"),
                                inorout = values[11].ToString(),
                                status = values[12].ToString()
                            });
                        }
                        conn.Close();
                        conn.Dispose();
                    }
                }

                return new DataResult<CompanyReportListDto>(ResultStatus.Success, new CompanyReportListDto
                {
                    DataList = data
                });
            }
            catch (Exception ex) { Log.Error(ex, "PostgreSqlManager -> GetReportList"); }

            return new DataResult<CompanyReportListDto>(ResultStatus.Error, new CompanyReportListDto());
        }
      
    }
}
