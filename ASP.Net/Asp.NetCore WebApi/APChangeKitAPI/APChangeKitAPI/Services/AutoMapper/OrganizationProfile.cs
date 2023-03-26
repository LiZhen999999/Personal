using APChangeKitAPI.Models.DTO;
using APChangeKitAPI.Models.Entity;
using AutoMapper;

namespace APChangeKitAPI.Services.AutoMapper
{
    public class OrganizationProfile: Profile
    {
        public OrganizationProfile()
        {
            CreateMap<AP_ChangeKit_FormInfo, APChangeKitChkListRes>();
            CreateMap<AP_ChangeKit_FormDetail, APChangeKitChkListItem>();
            CreateMap<APChangeKitChkListItemAdd, AP_ChangeKit_FormDetail>();
        }
    }
}
