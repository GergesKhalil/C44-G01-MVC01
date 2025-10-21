using GymManagementBLL.ViewModels.TrainerViewModels;
using GymManagementDAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymManagementBLL.Services.Interfaces
{
    public interface ITrainerService
    {
        IEnumerable<TrainerViewModel> GetAllTrainers();
        bool CreateTrainer(CreateTrainerViewModel CreateTrainer);   
        TrainerViewModel? GetTrainerDetails(int TrainerId);
        TrainerToUpdateViewModel? GetTrainerToUpdate(int TrainerId);

        bool UpdateTrainerDetails(TrainerToUpdateViewModel UpdateTrainer , int TrainerId);
        bool RemoveTrainer(int TrainerId);


    }
}
