using AutoMapper;
using FormPaperless.Core.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Intrinsics.X86;
using System.Text;
using System.Threading.Tasks;

namespace FormPaperless.Core
{
    public class GenericFormFrofile : Profile
    {
        public GenericFormFrofile()
        {
            //创建转换Map
            CreateMap<FormMasterHead, GenericFormModel>().
                ForMember(g => g.Id, f => f.Ignore());
            CreateMap<FormMasterBody, GenericFormContentModel>().
                ForMember(g => g.InputSuggestions, f =>
                {

                    f.MapFrom(src => src.InputSuggestions.Split(',', System.StringSplitOptions.RemoveEmptyEntries).ToList());
                }
               );
            CreateMap<FormMasterHead, FormInfo>();
        }
    }
}
