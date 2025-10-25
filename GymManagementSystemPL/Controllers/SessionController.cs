using GymManagementBLL.Services.Interfaces;
using GymManagementSystemBLL.ViewModels.SessionViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace GymManagementSystemPL.Controllers
{
    public class SessionController : Controller
    {
        private readonly ISessionService _sessionService;

        public SessionController(ISessionService sessionService )
        {
            _sessionService = sessionService;
        }

        #region Gett All Session
        public ActionResult Index()
        {
            var Session = _sessionService.GetAllSessions();
            return View(Session);
        }
        #endregion

        #region Get Details
        public ActionResult Details(int id)
        {
            if (id <= 0)
            {
                TempData["ErrorMessage"] = "InValid Session Id";
                return RedirectToAction(nameof(Index));
            }
            var Session = _sessionService.GetSessionById(id);
            if (Session is null)
            {
                TempData["ErrorMessage"] = "Session Not Found";
                return RedirectToAction(nameof(Index));
            }

            return View(Session);
        }
        #endregion

        #region Create Session
        public ActionResult Create()
        {
            LoadDropDownsTrainers();
            LoadDropDownsCategories();
            return View();
        }
        [HttpPost]
        public ActionResult Create (CreateSessionViewModel CreatedSession)
        {
            if (!ModelState.IsValid)
            {
                LoadDropDownsTrainers();
                LoadDropDownsCategories();
                return View(CreatedSession);
            }
            var IsCreated = _sessionService.CreateSession(CreatedSession);
            if (IsCreated)
            {
                TempData["SuccessMessage"] = "Session Created Successfully";
                return RedirectToAction(nameof(Index));
            }
            else
            {
                TempData["ErrorMessage"] = "Failed to Create Session. Please check the entered data.";
                LoadDropDownsTrainers();
                LoadDropDownsCategories();
                return View(CreatedSession);
            }

               
        }
        #endregion

        #region Edit Session
        public ActionResult Edit(int id)
        {
            if (id <= 0)
            {
                TempData["ErrorMessage"] = "InValid Session Id";
                return RedirectToAction(nameof(Index));
            }
            var Session = _sessionService.GetSessionToUpdate(id);
            if(Session is null)
            {
                TempData["ErrorMessage"] = "Session Not Found";
                return RedirectToAction(nameof(Index));
            }
            LoadDropDownsTrainers();
            return View(Session);
        }
        [HttpPost]
        public ActionResult Edit(UpdateSessionViewModel UpdatedSession,int id)
        {
            if (!ModelState.IsValid)
            {
                LoadDropDownsTrainers();
                return View(UpdatedSession);
            }
             var result =_sessionService.UpdatSession(UpdatedSession, id);
            if (result)
            {
                TempData["SuccessMessage"] = "Session Updated Successfully";
                
            }
            else
            {
                TempData["ErrorMessage"] = "Failed to Update Session. Please check the entered data.";
                
            }
            return RedirectToAction(nameof(Index));
        }
        #endregion

        #region Remove Session
        public ActionResult Delete(int id)
        {
            if(id <= 0)
            {
                TempData["ErrorMessage"] = "InValid Session Id";
                return RedirectToAction(nameof(Index));

            }
            var session = _sessionService.GetSessionById(id);
                if(session is null)
                {
                    TempData["ErrorMessage"] = "Session Not Found";
                    return RedirectToAction(nameof(Index));
                }
                ViewBag.SessionId= session.Id;
                 return View();
        }

        [HttpPost]
        public ActionResult DeleteConfirmed(int id)
        {
            var result = _sessionService.RemoveSession(id);
            if(result)
            {
                TempData["SuccessMessage"] = "Session Delete";

            }
            else
            {
                TempData["ErrorMessage"] = "Session Can Not Be Delete";
            }
            return RedirectToAction(nameof(Index));

        }
        #endregion


        #region Helper
        private void LoadDropDownsTrainers()
        {
            var Trainers = _sessionService.GetAllTrainersForDropDown();
            ViewBag.Trainers = new SelectList(Trainers, "Id", "Name");
            
        }
        private void LoadDropDownsCategories()
        {
            var Categories = _sessionService.GetAllCategoriesForDropDown();
            ViewBag.Categories = new SelectList(Categories, "Id", "Name");
        }
        #endregion
    }
}
