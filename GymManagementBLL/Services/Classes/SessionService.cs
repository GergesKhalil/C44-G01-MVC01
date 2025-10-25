using AutoMapper;
using GymManagementBLL.Services.Interfaces;
using GymManagementBLL.ViewModels.SessionViewModels;
using GymManagementDAL.Entities;
using GymManagementDAL.Repositories.Classes;
using GymManagementDAL.Repositories.Interfaces;
using GymManagementSystemBLL.ViewModels.SessionViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymManagementBLL.Services.Classes
{
    public class SessionService : ISessionService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public SessionService(IUnitOfWork unitOfWork , IMapper mapper )
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public bool CreateSession(CreateSessionViewModel CreatedSession)
        {
            try
            {
                //check if trainer exists
                if (!IsTrainerExists(CreatedSession.TrainerId)) return false;
                //chech if category exists
                if (!IsCategoryExists(CreatedSession.CategoryId)) return false;
                //check if StratDate is before EndDate
                if (!IsDateTimeValid(CreatedSession.StartDate, CreatedSession.EndDate)) return false;

                if (CreatedSession.Capacity > 25 || CreatedSession.Capacity < 0) return false;

                var SessionEntity = _mapper.Map<Session>(CreatedSession);
                _unitOfWork.GetRepository<Session>().Add(SessionEntity);
                return _unitOfWork.SaveChanges() > 0;

            }
            catch (Exception ex) 
            {
                Console.WriteLine($"Create Session Failed : {ex}");
                return false;
            }

              
            
        }

        public IEnumerable<SessionViewModel> GetAllSessions()
        {
            var Sessions = _unitOfWork.SessionRepository.GetAllSessionsWithTrainerAndCategory();
            if (!Sessions.Any()) return [];
            var MappedSession = _mapper.Map<IEnumerable<Session>, IEnumerable<SessionViewModel>>(Sessions);
            foreach (var Session in MappedSession)
                Session.AvailbleSlots = Session.Capacity - _unitOfWork.SessionRepository.GetCountOfBookedSlots(Session.Id);
            return MappedSession;

        }
        public SessionViewModel? GetSessionById(int SessionId)
        {
            var session = _unitOfWork.SessionRepository.GetSessionWithTrainerAndCategory(SessionId);
            if (session == null) return null;
            var MappedSession = _mapper.Map<Session , SessionViewModel>(session);
            MappedSession.AvailbleSlots = MappedSession.Capacity - _unitOfWork.SessionRepository.GetCountOfBookedSlots(MappedSession.Id);
            return MappedSession;
         
        }

        public UpdateSessionViewModel? GetSessionToUpdate(int SessionId)
        {
            var session = _unitOfWork.SessionRepository.GetById(SessionId);
            if (!IsSessionAvailableForUpdating(session!)) return null;
            return _mapper.Map<UpdateSessionViewModel>(session);
            
        }

        public bool UpdatSession(UpdateSessionViewModel UpdatedSession, int sessionId)
        {
            try
            {
                var session = _unitOfWork.SessionRepository.GetById(sessionId);
                if (!IsSessionAvailableForUpdating(session!)) return false;
                if (!IsTrainerExists(UpdatedSession.TrainerId)) return false;
                if (!IsDateTimeValid(UpdatedSession.StartDate, UpdatedSession.EndDate)) return false;
                _mapper.Map(UpdatedSession , session);
                session!.UpdatedAt = DateTime.Now;
                _unitOfWork.SessionRepository.Update(session);
                return _unitOfWork.SaveChanges() > 0;

            }
            catch (Exception ex)
            {
                Console.WriteLine($"Update Session Vailed : {ex}");
                return false;
            }
        }
        public bool RemoveSession(int SessionId)
        {
           try
           {
                var Session = _unitOfWork.SessionRepository.GetById(SessionId);
                if (!IsSessionAvailableRemoving(Session!) ) return false;
                _unitOfWork.SessionRepository.Delete(Session!);
                return _unitOfWork.SaveChanges() > 0;

            }
           catch (Exception ex)
           {
                Console.WriteLine($"Remove Session Faild : {ex}");
                return false;
            }
        }

        public IEnumerable<CategorySelectViewModel> GetAllCategoriesForDropDown()
        {
            var Categories = _unitOfWork.GetRepository<Category>().GetAll();
            return _mapper.Map<IEnumerable<CategorySelectViewModel>>(Categories);
        }

        public IEnumerable<TrainerSelectViewModel> GetAllTrainersForDropDown()
        {
            var Trainers = _unitOfWork.GetRepository<Trainer>().GetAll();
            return _mapper.Map<IEnumerable<TrainerSelectViewModel>>(Trainers);
        }



        #region Helper Methods

        private bool IsSessionAvailableForUpdating(Session session)
        {
            // if session completed - no updateed allowed
            if(session.EndDate <= DateTime.Now) return false;
            // if session started - no updateed allowed
            if(session.StartDate <= DateTime.Now) return false;
            // if session has active booking - no updated allowed
            var HasActiveBooking = _unitOfWork.SessionRepository.GetCountOfBookedSlots(session.Id)>0;
            if(HasActiveBooking) return false;
            return true;
        }
        private bool IsSessionAvailableRemoving(Session session)
        {
         
            // if session started - no updateed allowed
            if(session.StartDate <= DateTime.Now && session.EndDate > DateTime.Now) return false;

            // if session is upcoming - no deleted allowed
            if(session.StartDate > DateTime.Now) return false;

            // if session has active booking - no updated allowed
            var HasActiveBooking = _unitOfWork.SessionRepository.GetCountOfBookedSlots(session.Id)>0;
            if(HasActiveBooking) return false;
            return true;
        }
        private bool IsTrainerExists(int TrainerId)
        {
            return _unitOfWork.GetRepository<Trainer>().GetById(TrainerId) is not null;
        }
        private bool IsCategoryExists(int CategoryId)
        {
            return _unitOfWork.GetRepository<Category>().GetById(CategoryId) is not null;
        }
        private bool IsDateTimeValid(DateTime StartDate , DateTime EndDate)
        {
            return StartDate < EndDate&& DateTime.Now < StartDate;
        }

       

        #endregion









    }
}
