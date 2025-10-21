using GymManagementBLL.Services.Interfaces;
using GymManagementBLL.ViewModels.PlanViewModels;
using GymManagementDAL.Entities;
using GymManagementDAL.Repositories.Classes;
using GymManagementDAL.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymManagementBLL.Services.Classes
{
    public class PlanService : IPlanService
    {
        private readonly IUnitOfWork _unitOfWork;

        public PlanService( IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public IEnumerable<PlanViewModel> GetAllPlans()
        {
            var plans = _unitOfWork.GetRepository<Plan>().GetAll();
            if (plans is null || !plans.Any()) return [];

            return plans.Select(P => new PlanViewModel
            {
                Id = P.Id,
                Name = P.Name,
                Description = P.Description,
                DurationDays = P.DurationDays,
                Price = P.Price,
                IsActive = P.IsActive

            });

        }

        public PlanViewModel? GetPlanById(int PlanId)
        {
           var plan = _unitOfWork.GetRepository<Plan>().GetById(PlanId);
            if (plan is null) return null;
            return new PlanViewModel
            {
                Id = plan.Id,
                Name = plan.Name,
                Description = plan.Description,
                DurationDays = plan.DurationDays,
                Price = plan.Price,
                IsActive = plan.IsActive
            };
        }

        public UpdatePlanViewModel? GetPlanTOUpdate(int PlanId)
        {
            var plan = _unitOfWork.GetRepository<Plan>().GetById(PlanId);
            if(plan is null || plan.IsActive == false || HasActiveMemberShips(PlanId)) return null;
            return new UpdatePlanViewModel
            {
                Description = plan.Description,
                DurationDays = plan.DurationDays,
                PlanName = plan.Name,
                Price = plan.Price
            };
        }

        public bool UpdatePlan(int PlanId, UpdatePlanViewModel updatePlan)
        {
            var plan = _unitOfWork.GetRepository<Plan>().GetById(PlanId);
            if (plan is null || HasActiveMemberShips(PlanId)) return false;
           
            try
            {
                (plan.Description, plan.DurationDays, plan.Price, plan.UpdatedAt)
                    = (updatePlan.Description, updatePlan.DurationDays, updatePlan.Price, DateTime.Now);
                _unitOfWork.GetRepository<Plan>().Update(plan);
                 return _unitOfWork.SaveChanges() > 0;

            }
            catch
            {
                return false;
            }
        }
        public bool ToggleStatus(int PlanId)
        {
            var Repo = _unitOfWork.GetRepository<Plan>();
            var plan = Repo.GetById(PlanId);
            if (plan is null || HasActiveMemberShips(PlanId)) return false;
            plan.IsActive = plan.IsActive == true ? false : true;
            plan.UpdatedAt = DateTime.Now;

            try
            {
                Repo.Update(plan);
                return _unitOfWork.SaveChanges() > 0;

            }
            catch
            {
                return false;
            }
        }


        #region Helper
        private bool HasActiveMemberShips(int PlanId)
        {
            var ActiveMemberShips = _unitOfWork.GetRepository<MemberShip>().GetAll(X => X.PlanId == PlanId && X.status == "Active");
            return ActiveMemberShips.Any();

        }
        #endregion
    }
}
