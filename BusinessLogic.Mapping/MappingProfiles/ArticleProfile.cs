using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using BusinessLogic.Models;
using DataAccess.Models;

namespace BusinessLogic.Mapping.MappingProfiles
{
    public class ArticleProfile : Profile
    {
        public ArticleProfile()
        {
            CreateMap<ArticlePL, Article>().ReverseMap();
        }
    }
}
