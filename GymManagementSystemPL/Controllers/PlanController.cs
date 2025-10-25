using GymManagementBLL.Services.Interfaces;
using GymManagementBLL.ViewModels.PlanViewModels;
using Microsoft.AspNetCore.Mvc;

namespace GymManagementSystemPL.Controllers
{
    public class PlanController : Controller
    {
        private readonly IPlanService _planService;

        public PlanController(IPlanService planService)
        {
            _planService = planService;
        }

        public ActionResult Index()
        {
            var plan = _planService.GetAllPlans();
            return View(plan);
        }

        #region Get All Details

        public ActionResult Details(int id)
        {
            if (id <= 0)
            {
                TempData["ErrorMessage"] = "Invalid Plan Id";
                return RedirectToAction(nameof(Index));
            }
            var plan = _planService.GetPlanById(id);

            if(plan is null)
            {
                TempData["ErrorMessage"] = " Plan Not Found ";
                return RedirectToAction(nameof(Index));
            }
                return View(plan);
        }

        #endregion

        #region Edit Plan
        public ActionResult Edit(int id)
        {
            if (id <= 0)
            {
                TempData["ErrorMessage"] = "Invalid Plan Id";
                return RedirectToAction(nameof(Index));
            }
            var plan = _planService.GetPlanTOUpdate(id);
            if (plan is null)
            {
                TempData["ErrorMessage"] = " Plan Not Found or Cannot be Updated ";
                return RedirectToAction(nameof(Index));
            }
            return View(plan);
        }

        [HttpPost]
        public ActionResult Edit([FromRoute] int id, UpdatePlanViewModel UpdatedPlan)
        {
            if (!ModelState.IsValid)
            {
                ModelState.AddModelError("WrongData", "Check Data Validation.");
                return View(UpdatedPlan);
            }
            var Result = _planService.UpdatePlan(id, UpdatedPlan);
            if (Result)
            {
                TempData["SuccessMessage"] = "Plan Updated Successfully";
               
            }
            else
            {
                TempData["ErrorMessage"] = "Failed to Update Plan";
            }
                
            return RedirectToAction(nameof(Index));
        }
        #endregion

        #region Delete Plan
        public ActionResult Activate(int id)
        {
            var Result = _planService.ToggleStatus(id);
            if (Result)
            {
                TempData["SuccessMessage"] = "Plan Activated Successfully";
            }
            else
            {
                TempData["ErrorMessage"] = "Failed to Change Plan Status";
            }
            return RedirectToAction(nameof(Index));
        }
        #endregion
    }
}
