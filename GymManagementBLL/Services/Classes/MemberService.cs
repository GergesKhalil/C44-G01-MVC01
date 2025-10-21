using AutoMapper;
using GymManagementBLL.Services.Interfaces;
using GymManagementBLL.ViewModels.MemberViewModels;
using GymManagementDAL.Entities;
using GymManagementDAL.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymManagementBLL.Services.Classes
{
    public class MemberService : IMemberService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public MemberService(IUnitOfWork unitOfWork , IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }
       

        public bool CreateMember(CreateMemberViewModel createMember)
        {
            try
            {

                if (IsEmailExists(createMember.Email) || IsPhoneExists(createMember.Phone)) return false;

                var member = new Member()
                {
                    Name = createMember.Name,
                    Email = createMember.Email,
                    Phone = createMember.Phone,
                    Gender = createMember.Gender,
                    DateOfBrith = createMember.DateOfBirth,
                    Address = new Address()
                    {
                        BuildingNumber = createMember.BuildingNumber,
                        Street = createMember.Street,
                        City = createMember.City
                    },
                    HealthRecord = new HealthRecord()
                    {
                        Height = createMember.HealthRecordViewModel.Height,
                        Weight = createMember.HealthRecordViewModel.Weight,
                        BloodType = createMember.HealthRecordViewModel.BloodType,
                        Note = createMember.HealthRecordViewModel.Note
                    }


                };

               _unitOfWork.GetRepository<Member>().Add(member);
                return _unitOfWork.SaveChanges() > 0;
            }
            catch (Exception)
            {
                return false;
            }

        }
        public IEnumerable<MemberViewModel> GetAllMembers()
        {
            var members = _unitOfWork.GetRepository<Member>().GetAll();
            if (members is null || !members.Any()) return [];
            var MemberViewModels = members.Select(X => new MemberViewModel
            {
                Id = X.Id,
                Name = X.Name,
                Email = X.Email,
                Phone = X.Phone,
                photo = X.Photo,
                Gender = X.Gender.ToString()

            });
            return MemberViewModels;




        }

        public MemberViewModel? GetMemberDetails(int MemberId)
        {
            var Member = _unitOfWork.GetRepository<Member>().GetById(MemberId);
            if (Member is null) return null;

            var ViewModel = new MemberViewModel()
            {
                Name = Member.Name,
                Email = Member.Email,
                Phone = Member.Phone,
                Gender = Member.Gender.ToString(),
                DateOfBirth = Member.DateOfBrith.ToShortDateString(),
                Address = $"{Member.Address.BuildingNumber} - {Member.Address.Street} - {Member.Address.City}",
                photo = Member.Photo
            };
            var ActiveMemberShip = _unitOfWork.GetRepository<MemberShip>().GetAll(x => x.MemberId == MemberId && x.status == "Active")
                                                  .FirstOrDefault();
            if (ActiveMemberShip is not null)
            {
                var Plan = _unitOfWork.GetRepository<Plan>().GetById(ActiveMemberShip.PlanId);
                ViewModel.PlanName = Plan?.Name;
                ViewModel.MemberShipStartDate = ActiveMemberShip.CreatedAt.ToShortDateString();
                ViewModel.MemberShipEndDate = ActiveMemberShip.EndDate.ToShortDateString();



            }
            return ViewModel;

        }

        public HealthRecordViewModel? GetMemberHealthRecordDetails(int MemberId)
        {
            var MemberHealthRecord = _unitOfWork.GetRepository<HealthRecord>().GetById(MemberId);
            if (MemberHealthRecord is null) return null;
            return new HealthRecordViewModel()
            {
                Height = MemberHealthRecord.Height,
                Weight = MemberHealthRecord.Weight,
                BloodType = MemberHealthRecord.BloodType,
                Note = MemberHealthRecord.Note
            };

        }

        public MemberToUpdateViewModel? GetMemberToUpdate(int MemberId)
        {
            var Member = _unitOfWork.GetRepository<Member>().GetById(MemberId);
            if (Member is null) return null;
            return new MemberToUpdateViewModel()
            {
                Email = Member.Email,
                Nmae = Member.Name,
                Phone = Member.Phone,
                Photo = Member.Photo,
                BuildingNumber = Member.Address.BuildingNumber,
                Street = Member.Address.Street,
                City = Member.Address.City


            };
        }

        public bool RemoveMember(int MemberId)
        {
            var MemberRepo = _unitOfWork.GetRepository<Member>();


            var Member = MemberRepo.GetById(MemberId);
            if (Member is null) return false;
            var HasActiveMemberSessions = _unitOfWork.GetRepository<MemberSession>()
                .GetAll(X => X.MemberId == MemberId && X.Session.StartDate > DateTime.Now).Any();
            if (HasActiveMemberSessions) return false;

            var MemberShipRepo = _unitOfWork.GetRepository<MemberShip>();

            var MemberShips = MemberShipRepo.GetAll(X => X.MemberId == MemberId);
            try
            {
                if (MemberShips.Any())
                {
                    foreach (var memberShip in MemberShips)
                    {
                        MemberShipRepo.Delete(memberShip);
                    }
                }
                 MemberRepo.Delete(Member);
                return _unitOfWork.SaveChanges() > 0;


            }
            catch 
            {
                return false;

            }
        }

        public bool UpdateMemberDetails(int Id, MemberToUpdateViewModel updateMember)
        {
            try
            {
                if (IsEmailExists(updateMember.Email) || IsPhoneExists(updateMember.Phone)) return false;
                var Repo = _unitOfWork.GetRepository<Member>();

                var Member = Repo.GetById(Id);
                if (Member is null) return false;
                Member.Email = updateMember.Email;
                Member.Phone = updateMember.Phone;
                Member.Address.BuildingNumber = updateMember.BuildingNumber;
                Member.Address.Street = updateMember.Street;
                Member.Address.City = updateMember.City;
                Member.UpdatedAt = DateTime.Now;
                 Repo.Update(Member);
                return _unitOfWork.SaveChanges() > 0;
            }
            catch (Exception)
            {
                return false;
            }
        }
        #region Helper Methods
        private bool IsEmailExists(string email)
        {
            return _unitOfWork.GetRepository<Member>().GetAll(X => X.Email == email).Any();
        }
        private bool IsPhoneExists(string phone)
        {
            return _unitOfWork.GetRepository<Member>().GetAll(X => X.Phone == phone).Any();
        }
        #endregion
    }
}
