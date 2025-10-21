using GymManagementBLL.Services.Interfaces;
using GymManagementBLL.ViewModels.TrainerViewModels;
using GymManagementDAL.Entities;
using GymManagementDAL.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymManagementBLL.Services.Classes
{
    public class TrainerService : ITrainerService
    {
        private readonly IUnitOfWork _unitOfWork;

        public TrainerService( IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public bool CreateTrainer(CreateTrainerViewModel CreateTrainer)
        {
            try
            {
                var Repo = _unitOfWork.GetRepository<Trainer>();
                if (IsEmailExist(CreateTrainer.Email) || IsPhoneExist(CreateTrainer.Phone)) return false;
                var Trainer = new Trainer()
                {
                    Name = CreateTrainer.Name,
                    Email = CreateTrainer.Email,
                    Phone = CreateTrainer.Phone,
                    DateOfBrith = CreateTrainer.DateOfBirth,
                    Specialties = CreateTrainer.Specialties,
                    Gender = CreateTrainer.Gender,
                    Address = new Address()
                    {
                        BuildingNumber = CreateTrainer.BuildingNumber,
                        Street = CreateTrainer.Street,
                        City = CreateTrainer.City
                    }
                };
                Repo.Add(Trainer); 
                return _unitOfWork.SaveChanges() > 0;




            }
            catch (Exception)
            {
                return false;
            }
        }

        public IEnumerable<TrainerViewModel> GetAllTrainers()
        {
           var Trainers = _unitOfWork.GetRepository<Trainer>().GetAll();
            if (Trainers is null || !Trainers.Any()) return [];
            return Trainers.Select(T => new TrainerViewModel
            {
                Id = T.Id,
                Name = T.Name,
                Email = T.Email,
                Phone = T.Phone,
                Specialization = T.Specialties.ToString()
            });
           
        }

        public TrainerViewModel? GetTrainerDetails(int TrainerId)
        {
            var Trainer = _unitOfWork.GetRepository<Trainer>().GetById(TrainerId);
            if (Trainer is null) return null;
            return new TrainerViewModel
            {
                Name = Trainer.Name,
                Email = Trainer.Email,
                Phone = Trainer.Phone,
                Specialization = Trainer.Specialties.ToString()
            };
        }

        public TrainerToUpdateViewModel? GetTrainerToUpdate(int TrainerId)
        {
            var Trainer = _unitOfWork.GetRepository<Trainer>().GetById(TrainerId);
            if (Trainer is null) return null;
            return new TrainerToUpdateViewModel()
            {
                Name = Trainer.Name,
                Email = Trainer.Email,
                Phone = Trainer.Phone,
                Street = Trainer.Address.Street,
                City = Trainer.Address.City,
                BuildingNumber = Trainer.Address.BuildingNumber,
                Specialties = Trainer.Specialties,

            };
        }

        public bool RemoveTrainer(int TrainerId)
        {
           var Repo = _unitOfWork.GetRepository<Trainer>();
            var TrainerToRemove = Repo.GetById(TrainerId);
            if (TrainerToRemove is null || HasActiveSessions(TrainerId)) return false;
            Repo.Delete(TrainerToRemove);
            return _unitOfWork.SaveChanges() > 0;
          
        }

        public bool UpdateTrainerDetails(TrainerToUpdateViewModel UpdateTrainer, int TrainerId)
        {
            var Repo = _unitOfWork.GetRepository<Trainer>();
            var TrainerToUpdate = Repo.GetById(TrainerId);
            if (TrainerToUpdate is null || IsEmailExist(UpdateTrainer.Email) || IsPhoneExist(UpdateTrainer.Phone)) return false;

            TrainerToUpdate.Email = UpdateTrainer.Email;
            TrainerToUpdate.Phone = UpdateTrainer.Phone;
            TrainerToUpdate.Address.BuildingNumber = UpdateTrainer.BuildingNumber;
            TrainerToUpdate.Address.Street = UpdateTrainer.Street;
            TrainerToUpdate.Address.City = UpdateTrainer.City;
            TrainerToUpdate.Specialties = UpdateTrainer.Specialties;
            TrainerToUpdate.UpdatedAt = DateTime.Now;
            Repo.Update(TrainerToUpdate);
            return _unitOfWork.SaveChanges() > 0;
        }

        #region Helper

        private bool  IsEmailExist(string Email)
        {
            var existing = _unitOfWork.GetRepository<Member>().GetAll(
                M => M.Email == Email).Any();
            return existing;

        }
        private bool  IsPhoneExist(string Phone)
        {
            var existing = _unitOfWork.GetRepository<Member>().GetAll(
                M => M.Phone == Phone).Any();
            return existing;

        }
        private bool HasActiveSessions(int TrainerId)
        {
            var ActiveSessions = _unitOfWork.GetRepository<Session>().GetAll(
                S => S.TrainerId == TrainerId && S.StartDate > DateTime.Now).Any();
            return ActiveSessions;
        }
        #endregion
    }
}
