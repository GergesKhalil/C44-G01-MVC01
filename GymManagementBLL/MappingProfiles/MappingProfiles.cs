using AutoMapper;
using GymManagementBLL.ViewModels.MemberViewModels;
using GymManagementBLL.ViewModels.SessionViewModels;
using GymManagementDAL.Entities;
using GymManagementSystemBLL.ViewModels.SessionViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymManagementBLL
{
    public class MappingProfiles : Profile
    {
        public MappingProfiles()
        {
            #region Session
            CreateMap<Session, SessionViewModel>()
                   .ForMember(dest => dest.CategoryName, Options => Options.MapFrom(src => src.SessionCategory.CategoryName))
                   .ForMember(dest => dest.TrainerName, Options => Options.MapFrom(src => src.SessionTrainer.Name))
                   .ForMember(dest => dest.AvailbleSlots, Option => Option.Ignore());

            CreateMap<CreateSessionViewModel, Session>();

            CreateMap<Session, UpdateSessionViewModel>().ReverseMap();
            #endregion

            //#region Member
            //CreateMap<CreateMemberViewModel, Member>()
            //    .ForMember(dest => dest.Address, Options => Options.MapFrom(src => src.BuildingNumber))
            //    .ForMember(dest => dest.Address, Options => Options.MapFrom(src => src.Street))
            //    .ForMember(dest => dest.Address, Options => Options.MapFrom(src => src.City))
            //    .ForMember(dest => dest.HealthRecord, Options => Options.MapFrom(src => src.HealthRecordViewModel.Height))
            //    .ForMember(dest => dest.HealthRecord, Options => Options.MapFrom(src => src.HealthRecordViewModel.Weight))
            //    .ForMember(dest => dest.HealthRecord, Options => Options.MapFrom(src => src.HealthRecordViewModel.BloodType))
            //    .ForMember(dest => dest.HealthRecord, Options => Options.MapFrom(src => src.HealthRecordViewModel.Note));

            //CreateMap<Member, MemberViewModel>().ReverseMap();

            //CreateMap<Member, HealthRecordViewModel>();

            //CreateMap<Member, MemberToUpdateViewModel>()
            //    .ForMember(dest => dest.BuildingNumber, Options => Options.MapFrom(src => src.Address.BuildingNumber))
            //    .ForMember(dest => dest.Street, Options => Options.MapFrom(src => src.Address.Street))
            //    .ForMember(dest => dest.City, Options => Options.MapFrom(src => src.Address.City));

            //CreateMap<MemberToUpdateViewModel, Member>()
            //    .ForMember(dest => dest.Address.BuildingNumber, Options => Options.MapFrom(src => src.BuildingNumber))
            //    .ForMember(dest => dest.Address.Street, Options => Options.MapFrom(src => src.Street))
            //    .ForMember(dest => dest.Address.City, Options => Options.MapFrom(src => src.City));


            //#endregion

        }

        
    }
}
