using GymManagementBLL.Services.Interfaces;
using GymManagementBLL.ViewModels.TrainerViewModels;
using Microsoft.AspNetCore.Mvc;

namespace GymManagementSystemPL.Controllers
{
    public class TrainerController : Controller
    {
        private readonly ITrainerService _trainerService;

        public TrainerController(ITrainerService trainerService) 
        {
           _trainerService = trainerService;
        }
        #region Get All Trainer
        public ActionResult Index()
        {
            var Trainer = _trainerService.GetAllTrainers();
            return View(Trainer);
        }
        #endregion

        #region Get Trainer Details
        public ActionResult TrainerDetails(int id)
        {
            if(id <= 0)
            {
                TempData["ErrorMessage"] = "Id of Trainer Can Not Be 0 Or Negative Number";
                return RedirectToAction(nameof(Index));
            }
            var Trainer = _trainerService.GetTrainerDetails(id);
            if (Trainer is null)
            {
                TempData["ErrorMessage"] = "Trainer Not Found";
                return RedirectToAction(nameof(Index));
            }
            return View(Trainer);

        }
        #endregion

        #region Edit Trainers
        public ActionResult TrainerEdit(int id)
        {
            if (id <= 0)
            {
                TempData["ErrorMessage"] = "Id Of Trainer Can Not Be 0Or Negative Number";
                return RedirectToAction(nameof(Index));
            }
            var Member = _trainerService.GetTrainerToUpdate(id);
            if (Member is null)
            {
                TempData["ErrorMessage"] = "Trainer Not Found";
                return RedirectToAction(nameof(Index));
            }
            return View(Member);

        }

        [HttpPost]
        public ActionResult TrainerEdit([FromRoute] int id, TrainerToUpdateViewModel MemberToEdit)
        {
            if (!ModelState.IsValid)
                return View(MemberToEdit);
            var Result = _trainerService.UpdateTrainerDetails(MemberToEdit, id);
            if(Result)
                TempData["SuccessMessage"] = "Trainer Update Successfuly";            
            else         
                TempData["SuccessMessage"] = "Trainer Faild To Update";

            return RedirectToAction(nameof(Index));           
        }
        #endregion

        #region Create Trainer
        public ActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public ActionResult CreateTrainer(CreateTrainerViewModel CreateTrainer,[FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                ModelState.AddModelError("DataInvalid", "Check Data And Missing Fields");
                return View(nameof(Create), CreateTrainer);
            }
            bool Result = _trainerService.CreateTrainer(CreateTrainer);
            if (Result)
            {
                TempData["SuccessMessage"] = "Trainer Created Successfuly";
            }
            else 
            {
                TempData["ErrorMessage"] = "Trainer Faild To Create";
            }
            return RedirectToAction(nameof(Index));
        }
        #endregion

        #region Delete Trainer
        public ActionResult Delete(int id)
        {
            if (id <= 0)
            {
                TempData["ErrorMessage"] = "Id Of Trainer Can Not Be 0 Or Negative Number";
                return RedirectToAction(nameof(Index));
            }
            var Trainer = _trainerService.GetTrainerDetails(id);
            if (Trainer is null)
            {
                TempData["ErrorMessage"] = "Trainer Not Found";
                return RedirectToAction(nameof(Index));
            }
            ViewBag.TrainerId = id;
            ViewBag.TrainerName = Trainer.Name;
            return View();
        }

        [HttpPost]
        public ActionResult DeleteConfirmed([FromForm]int id)
        {
            bool Result =_trainerService.RemoveTrainer(id);
            if(Result)
            {
                TempData["SuccessMessage"] = "Trainer Deleted Successfuly";
            }
            else
            {
                TempData["ErrorMessage"] = "Trainer Faild To Delete";
            }
            return RedirectToAction(nameof(Index));

        }
        #endregion
    }
}
