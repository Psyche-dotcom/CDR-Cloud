using AutoMapper;
using CDR.Entities.Concrete;
using CDR.Entities.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CDR.Services.AutoMapper
{
    public class ReportFavoriteFilterProfile : Profile
    {
        public ReportFavoriteFilterProfile()
        {
            CreateMap<ReportFavoriteFilterAddDto, ReportFilter>();
        }
    }
}
